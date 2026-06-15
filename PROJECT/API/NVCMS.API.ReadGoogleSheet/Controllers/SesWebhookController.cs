using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
using System.Net.Http;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// Nhận SNS notification từ AWS SES.
    /// Endpoint PUBLIC – không [Authorize] vì SNS gọi trực tiếp.
    /// Route: POST /api/emailmarketing/ses-events
    /// </summary>
    [Route("api/emailmarketing")]
    [ApiController]
    public class SesWebhookController : ControllerBase
    {
        private readonly IMarketingListMailRepository  _listMailRepo;
        private readonly IMarketingEventRepository     _eventRepo;
        private readonly IMarketingClickRepository     _clickRepo;
        private readonly IMarketingUnsubRepository     _unsubRepo;
        private readonly IHttpClientFactory            _httpClientFactory;
        private readonly ILogger<SesWebhookController> _logger;

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SesWebhookController(
            IMarketingListMailRepository   listMailRepo,
            IMarketingEventRepository      eventRepo,
            IMarketingClickRepository      clickRepo,
            IMarketingUnsubRepository      unsubRepo,
            IHttpClientFactory             httpClientFactory,
            ILogger<SesWebhookController>  logger)
        {
            _listMailRepo      = listMailRepo;
            _eventRepo         = eventRepo;
            _clickRepo         = clickRepo;
            _unsubRepo         = unsubRepo;
            _httpClientFactory = httpClientFactory;
            _logger            = logger;
        }

        // POST /api/emailmarketing/ses-events
        [HttpPost("ses-events")]
        public async Task<IActionResult> HandleSesEvent()
        {
            // Đọc raw body để parse SNS envelope
            string rawBody;
            using (var reader = new StreamReader(Request.Body))
                rawBody = await reader.ReadToEndAsync();

            SnsNotification? sns;
            try
            {
                sns = JsonSerializer.Deserialize<SnsNotification>(rawBody, _jsonOpts);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SES webhook: failed to parse SNS envelope");
                return BadRequest("Invalid SNS payload");
            }

            if (sns is null) return BadRequest("Empty payload");

            // ── SNS subscription confirmation ─────────────────────────────────
            if (string.Equals(sns.Type, "SubscriptionConfirmation", StringComparison.OrdinalIgnoreCase))
            {
                await ConfirmSubscriptionAsync(sns.SubscribeURL);
                return Ok("Subscription confirmed");
            }

            // ── SNS Notification ──────────────────────────────────────────────
            if (!string.Equals(sns.Type, "Notification", StringComparison.OrdinalIgnoreCase))
                return Ok("Ignored");

            if (string.IsNullOrWhiteSpace(sns.Message))
                return BadRequest("Empty Message field");

            SesEventPayload? payload;
            try
            {
                payload = JsonSerializer.Deserialize<SesEventPayload>(sns.Message, _jsonOpts);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SES webhook: failed to parse SES event payload");
                return BadRequest("Invalid SES event payload");
            }

            if (payload?.mail is null)
                return Ok("No mail info");

            var messageId = payload.mail.messageId;
            if (string.IsNullOrWhiteSpace(messageId))
                return Ok("No messageId");

            // Tìm recipient theo MessageId
            var recipient = await _listMailRepo.GetByMessageIdAsync(messageId);
            if (recipient is null)
            {
                _logger.LogWarning("SES webhook: no recipient found for MessageId={MessageId}", messageId);
                return Ok("Recipient not found – ignored");
            }

            // Dispatch theo eventType
            var eventType = payload.eventType ?? string.Empty;
            _logger.LogInformation(
                "SES webhook: eventType={EventType} messageId={MessageId} recipientId={RecipientId}",
                eventType, messageId, recipient.id);

            switch (eventType.ToUpperInvariant())
            {
                case "DELIVERY":
                    await HandleDeliveryAsync(recipient, payload.delivery, rawBody);
                    break;

                case "BOUNCE":
                    await HandleBounceAsync(recipient, payload.bounce, rawBody);
                    break;

                case "COMPLAINT":
                    await HandleComplaintAsync(recipient, payload.complaint, rawBody);
                    break;

                case "OPEN":
                    await HandleOpenAsync(recipient, payload.open, rawBody);
                    break;

                case "CLICK":
                    await HandleClickAsync(recipient, payload.click, rawBody);
                    break;

                default:
                    _logger.LogDebug("SES webhook: unhandled eventType={EventType}", eventType);
                    break;
            }

            return Ok("Processed");
        }

        // ── Event handlers ────────────────────────────────────────────────────

        private async Task HandleDeliveryAsync(
            Marketing_Mail_ListMail recipient,
            SesDelivery?          delivery,
            string                rawPayload)
        {
            // Chỉ update nếu chưa ở trạng thái cao hơn (Open/Click)
            if (recipient.RecipientStatus < 2)
                await _listMailRepo.UpdateRecipientStatusAsync(recipient.id, 2, DateTime.UtcNow);

            await WriteEventAsync(recipient.id, "Delivery", rawPayload);
        }

        private async Task HandleBounceAsync(
            Marketing_Mail_ListMail recipient,
            SesBounce?            bounce,
            string                rawPayload)
        {
            var reason = bounce is null ? string.Empty
                : $"{bounce.bounceType}/{bounce.bounceSubType} – " +
                  string.Join("; ", bounce.bouncedRecipients
                      .Select(r => r.diagnosticCode ?? r.emailAddress ?? string.Empty));

            // Cập nhật BounceReason trực tiếp
            var record = await _listMailRepo.GetByIdAsync(recipient.id);
            if (record is not null)
            {
                record.BounceReason    = reason;
                record.RecipientStatus = 5; // Bounced
                await _listMailRepo.UpdateAsync(record);
            }

            await WriteEventAsync(recipient.id, "Bounce", rawPayload);
        }

        private async Task HandleComplaintAsync(
            Marketing_Mail_ListMail recipient,
            SesComplaint?         complaint,
            string                rawPayload)
        {
            var reason = complaint?.complaintFeedbackType ?? "unknown";

            var record = await _listMailRepo.GetByIdAsync(recipient.id);
            if (record is not null)
            {
                record.ComplaintReason = reason;
                record.RecipientStatus = 6; // Complaint
                await _listMailRepo.UpdateAsync(record);
            }

            // Tự động unsub khi có complaint
            if (!string.IsNullOrWhiteSpace(recipient.Email))
            {
                var alreadyUnsub = await _unsubRepo.IsUnsubscribedAsync(
                    recipient.Email, recipient.PortalId ?? 0);

                if (!alreadyUnsub)
                {
                    await _unsubRepo.AddAsync(new MarketingMailListMailUnsub
                    {
                        Email        = recipient.Email,
                        reason       = 6,               // Complaint
                        created_date = DateTime.UtcNow,
                        PortalId     = recipient.PortalId,
                        Token        = Guid.NewGuid(),
                        IPAddress    = HttpContext.Connection.RemoteIpAddress?.ToString()
                    });
                }
            }

            await WriteEventAsync(recipient.id, "Complaint", rawPayload);
        }

        private async Task HandleOpenAsync(
            Marketing_Mail_ListMail recipient,
            SesOpen?              open,
            string                rawPayload)
        {
            // Open chỉ update nếu chưa ở trạng thái Clicked (4)
            if (recipient.RecipientStatus < 3)
                await _listMailRepo.UpdateRecipientStatusAsync(recipient.id, 3, DateTime.UtcNow);

            await WriteEventAsync(recipient.id, "Open",
                rawPayload,
                data: $"{{\"ip\":\"{open?.ipAddress}\",\"ua\":\"{EscapeJson(open?.userAgent)}\"}}");
        }

        private async Task HandleClickAsync(
            Marketing_Mail_ListMail recipient,
            SesClick?             click,
            string                rawPayload)
        {
            await _listMailRepo.UpdateRecipientStatusAsync(recipient.id, 4, DateTime.UtcNow);

            // Ghi vào Marketing_Mail_Click
            if (!string.IsNullOrWhiteSpace(click?.link))
            {
                await _clickRepo.AddAsync(new MarketingMailClick
                {
                    ListMailId = recipient.id,
                    Url        = click.link,
                    ClickedAt  = DateTime.UtcNow
                });
            }

            await WriteEventAsync(recipient.id, "Click",
                rawPayload,
                data: $"{{\"url\":\"{EscapeJson(click?.link)}\",\"ip\":\"{click?.ipAddress}\"}}");
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private async Task WriteEventAsync(int listMailId, string eventType, string rawPayload, string? data = null)
        {
            await _eventRepo.AddAsync(new MarketingMailEvent
            {
                ListMailId  = listMailId,
                EventType   = eventType,
                Payload     = data ?? rawPayload,
                CreatedDate = DateTime.UtcNow
            });
        }

        private async Task ConfirmSubscriptionAsync(string? subscribeUrl)
        {
            if (string.IsNullOrWhiteSpace(subscribeUrl)) return;
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                await client.GetAsync(subscribeUrl);
                _logger.LogInformation("SNS subscription confirmed: {Url}", subscribeUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to confirm SNS subscription");
            }
        }

        private static string EscapeJson(string? value) =>
            (value ?? string.Empty)
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
    }
}
