using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
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
        private readonly IMarketingSendLogRepository    _sendLogRepo;
        private readonly IMarketingUnsubRepository      _unsubRepo;
        private readonly IHttpClientFactory             _httpClientFactory;
        private readonly ILogger<SesWebhookController> _logger;

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SesWebhookController(
            IMarketingSendLogRepository    sendLogRepo,
            IMarketingUnsubRepository      unsubRepo,
            IHttpClientFactory             httpClientFactory,
            ILogger<SesWebhookController>  logger)
        {
            _sendLogRepo       = sendLogRepo;
            _unsubRepo         = unsubRepo;
            _httpClientFactory = httpClientFactory;
            _logger            = logger;
        }

        // POST /api/emailmarketing/ses-events
        [HttpPost("ses-events")]
        public async Task<IActionResult> HandleSesEvent()
        {
            string rawBody;
            using (var reader = new StreamReader(Request.Body))
                rawBody = await reader.ReadToEndAsync();

            SnsNotification? sns;
            try { sns = JsonSerializer.Deserialize<SnsNotification>(rawBody, _jsonOpts); }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SES webhook: failed to parse SNS envelope");
                return BadRequest("Invalid SNS payload");
            }

            if (sns is null) return BadRequest("Empty payload");

            if (string.Equals(sns.Type, "SubscriptionConfirmation", StringComparison.OrdinalIgnoreCase))
            {
                await ConfirmSubscriptionAsync(sns.SubscribeURL);
                return Ok("Subscription confirmed");
            }

            if (!string.Equals(sns.Type, "Notification", StringComparison.OrdinalIgnoreCase))
                return Ok("Ignored");

            if (string.IsNullOrWhiteSpace(sns.Message))
                return BadRequest("Empty Message field");

            SesEventPayload? payload;
            try { payload = JsonSerializer.Deserialize<SesEventPayload>(sns.Message, _jsonOpts); }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SES webhook: failed to parse SES event payload");
                return BadRequest("Invalid SES event payload");
            }

            if (payload?.mail is null) return Ok("No mail info");

            var messageId = payload.mail.messageId;
            if (string.IsNullOrWhiteSpace(messageId)) return Ok("No messageId");

            var sendLog = await _sendLogRepo.GetBySesMessageIdAsync(messageId);
            if (sendLog is null)
            {
                _logger.LogWarning("SES webhook: no send-log found for MessageId={MessageId}", messageId);
                return Ok("Not found – ignored");
            }

            var eventType = (payload.eventType ?? string.Empty).ToUpperInvariant();
            _logger.LogInformation("SES webhook: eventType={EventType} messageId={MsgId}", eventType, messageId);

            switch (eventType)
            {
                case "DELIVERY":
                    await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Delivered);
                    break;

                case "OPEN":
                    await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Opened);
                    break;

                case "CLICK":
                    await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Clicked);
                    break;

                case "BOUNCE":
                    await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Failed,
                        errorMessage: $"Bounce: {payload.bounce?.bounceType}/{payload.bounce?.bounceSubType}");
                    break;

                case "COMPLAINT":
                    await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Failed,
                        errorMessage: $"Complaint: {payload.complaint?.complaintFeedbackType}");

                    var alreadyUnsub = await _unsubRepo.IsUnsubscribedAsync(sendLog.Email, 0);
                    if (!alreadyUnsub)
                    {
                        await _unsubRepo.AddAsync(new MarketingMailListMailUnsub
                        {
                            Email        = sendLog.Email,
                            reason       = 6,
                            created_date = DateTime.UtcNow
                        });
                    }
                    break;

                default:
                    _logger.LogDebug("SES webhook: unhandled eventType={EventType}", eventType);
                    break;
            }

            return Ok("Processed");
        }

        private async Task ConfirmSubscriptionAsync(string? subscribeUrl)
        {
            if (string.IsNullOrWhiteSpace(subscribeUrl)) return;
            try
            {
                var client = _httpClientFactory.CreateClient();
                await client.GetAsync(subscribeUrl);
                _logger.LogInformation("SNS subscription confirmed: {Url}", subscribeUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to confirm SNS subscription");
            }
        }
    }
}
