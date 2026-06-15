using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    [Route("api/emailmarketing")]
    [ApiController]
    public class EmailMarketingController : ControllerBase
    {
        private readonly IEmailMarketingService          _marketingService;
        private readonly IMarketingListMailRepository    _listMailRepo;
        private readonly IMarketingCampaignSendRepository _campaignSendRepo;
        private readonly IMarketingSendLogRepository     _sendLogRepo;
        private readonly IMarketingEventRepository       _eventRepo;
        private readonly IMarketingClickRepository       _clickRepo;
        private readonly IMarketingUnsubRepository       _unsubRepo;
        private readonly IHttpClientFactory              _httpClientFactory;
        private readonly ILogger<EmailMarketingController> _logger;

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public EmailMarketingController(
            IEmailMarketingService            marketingService,
            IMarketingListMailRepository      listMailRepo,
            IMarketingCampaignSendRepository  campaignSendRepo,
            IMarketingSendLogRepository       sendLogRepo,
            IMarketingEventRepository         eventRepo,
            IMarketingClickRepository         clickRepo,
            IMarketingUnsubRepository         unsubRepo,
            IHttpClientFactory                httpClientFactory,
            ILogger<EmailMarketingController> logger)
        {
            _marketingService  = marketingService;
            _listMailRepo      = listMailRepo;
            _campaignSendRepo  = campaignSendRepo;
            _sendLogRepo       = sendLogRepo;
            _eventRepo         = eventRepo;
            _clickRepo         = clickRepo;
            _unsubRepo         = unsubRepo;
            _httpClientFactory = httpClientFactory;
            _logger            = logger;
        }

        // ── POST /api/emailmarketing/send ─────────────────────────────────────
        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> Send([FromBody] SendCampaignBodyRequest request)
        {
            try
            {
                var result = await _marketingService.SendCampaignAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Send campaignId={CampaignId}", request.CampaignId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        // ── POST /api/emailmarketing/sns (PUBLIC – SNS gọi trực tiếp) ─────────
        [HttpPost("sns")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleSns()
        {
            string rawBody;
            using (var reader = new StreamReader(Request.Body))
                rawBody = await reader.ReadToEndAsync();

            SnsNotification? sns;
            try { sns = JsonSerializer.Deserialize<SnsNotification>(rawBody, _jsonOpts); }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SNS: failed to parse envelope");
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

            if (string.IsNullOrWhiteSpace(sns.Message)) return BadRequest("Empty Message");

            SesEventPayload? payload;
            try { payload = JsonSerializer.Deserialize<SesEventPayload>(sns.Message, _jsonOpts); }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SNS: failed to parse SES event");
                return BadRequest("Invalid SES event");
            }

            if (payload?.mail is null) return Ok("No mail info");

            var messageId = payload.mail.messageId;
            if (string.IsNullOrWhiteSpace(messageId)) return Ok("No messageId");

            _logger.LogInformation("SNS: eventType={EventType} messageId={MsgId}",
                payload.eventType, messageId);

            // Tìm send-log theo SesMessageId
            var sendLog = await _sendLogRepo.GetBySesMessageIdAsync(messageId);
            // Fallback: tìm theo list-mail MessageId (luồng cũ)
            var listMail = await _listMailRepo.GetByMessageIdAsync(messageId);

            var eventType = (payload.eventType ?? string.Empty).ToUpperInvariant();

            switch (eventType)
            {
                case "DELIVERY":
                    await HandleDeliveryAsync(sendLog, listMail, rawBody, messageId);
                    break;
                case "OPEN":
                    await HandleOpenAsync(sendLog, listMail, payload.open, rawBody, messageId);
                    break;
                case "CLICK":
                    await HandleClickAsync(sendLog, listMail, payload.click, rawBody, messageId);
                    break;
                case "BOUNCE":
                    await HandleBounceAsync(sendLog, listMail, payload.bounce, rawBody, messageId);
                    break;
                case "COMPLAINT":
                    await HandleComplaintAsync(sendLog, listMail, payload.complaint, rawBody, messageId);
                    break;
                default:
                    _logger.LogDebug("SNS: unhandled eventType={EventType}", eventType);
                    break;
            }

            return Ok("Processed");
        }

        // ── SNS event handlers ────────────────────────────────────────────────

        private async Task HandleDeliveryAsync(
            MarketingMailSendLog? sendLog, Marketing_Mail_ListMail? listMail,
            string rawPayload, string messageId)
        {
            await WriteEventAsync(sendLog, listMail, messageId, "Delivery", rawPayload);
            if (sendLog is not null)
                await _campaignSendRepo.IncrementCounterAsync(sendLog.CampaignSendId!.Value, "TotalDelivered");
            if (listMail is not null && (listMail.RecipientStatus ?? 0) < 2)
                await _listMailRepo.UpdateRecipientStatusAsync(listMail.id, 2, DateTime.UtcNow);
        }

        private async Task HandleOpenAsync(
            MarketingMailSendLog? sendLog, Marketing_Mail_ListMail? listMail,
            SesOpen? open, string rawPayload, string messageId)
        {
            await WriteEventAsync(sendLog, listMail, messageId, "Open", rawPayload);
            if (sendLog is not null)
                await _campaignSendRepo.IncrementCounterAsync(sendLog.CampaignSendId!.Value, "TotalOpened");
            if (listMail is not null && (listMail.RecipientStatus ?? 0) < 3)
                await _listMailRepo.UpdateRecipientStatusAsync(listMail.id, 3, DateTime.UtcNow);
        }

        private async Task HandleClickAsync(
            MarketingMailSendLog? sendLog, Marketing_Mail_ListMail? listMail,
            SesClick? click, string rawPayload, string messageId)
        {
            await WriteEventAsync(sendLog, listMail, messageId, "Click", rawPayload);
            if (sendLog is not null)
                await _campaignSendRepo.IncrementCounterAsync(sendLog.CampaignSendId!.Value, "TotalClicked");
            if (listMail is not null)
                await _listMailRepo.UpdateRecipientStatusAsync(listMail.id, 4, DateTime.UtcNow);
            if (!string.IsNullOrWhiteSpace(click?.link) && listMail is not null)
            {
                await _clickRepo.AddAsync(new MarketingMailClick
                {
                    ListMailId = listMail.id,
                    Url        = click.link,
                    ClickedAt  = DateTime.UtcNow
                });
            }
        }

        private async Task HandleBounceAsync(
            MarketingMailSendLog? sendLog, Marketing_Mail_ListMail? listMail,
            SesBounce? bounce, string rawPayload, string messageId)
        {
            await WriteEventAsync(sendLog, listMail, messageId, "Bounce", rawPayload);
            if (sendLog is not null)
                await _campaignSendRepo.IncrementCounterAsync(sendLog.CampaignSendId!.Value, "TotalBounced");
            if (listMail is not null)
            {
                var reason = bounce is null ? string.Empty
                    : $"{bounce.bounceType}/{bounce.bounceSubType}";
                listMail.BounceReason    = reason;
                listMail.RecipientStatus = 5;
                await _listMailRepo.UpdateAsync(listMail);
            }
        }

        private async Task HandleComplaintAsync(
            MarketingMailSendLog? sendLog, Marketing_Mail_ListMail? listMail,
            SesComplaint? complaint, string rawPayload, string messageId)
        {
            await WriteEventAsync(sendLog, listMail, messageId, "Complaint", rawPayload);
            if (sendLog is not null)
                await _campaignSendRepo.IncrementCounterAsync(sendLog.CampaignSendId!.Value, "TotalComplaint");
            if (listMail is not null)
            {
                listMail.ComplaintReason = complaint?.complaintFeedbackType ?? "unknown";
                listMail.RecipientStatus = 6;
                await _listMailRepo.UpdateAsync(listMail);

                if (!string.IsNullOrWhiteSpace(listMail.Email))
                {
                    var alreadyUnsub = await _unsubRepo.IsUnsubscribedAsync(
                        listMail.Email, listMail.PortalId ?? 0);
                    if (!alreadyUnsub)
                    {
                        await _unsubRepo.AddAsync(new MarketingMailListMailUnsub
                        {
                            Email        = listMail.Email,
                            reason       = 6,
                            created_date = DateTime.UtcNow,
                            PortalId     = listMail.PortalId,
                            Token        = Guid.NewGuid(),
                            IPAddress    = HttpContext.Connection.RemoteIpAddress?.ToString()
                        });
                    }
                }
            }
        }

        // ── Helper ────────────────────────────────────────────────────────────

        private async Task WriteEventAsync(
            MarketingMailSendLog? sendLog,
            Marketing_Mail_ListMail? listMail,
            string sesMessageId,
            string eventType,
            string rawPayload)
        {
            await _eventRepo.AddAsync(new MarketingMailEvent
            {
                CampaignSendId = sendLog?.CampaignSendId,
                ListMailId     = sendLog?.ListMailId ?? listMail?.id,
                SesMessageId   = sesMessageId,
                EventType      = eventType,
                Payload        = rawPayload,
                CreatedDate    = DateTime.UtcNow
            });
        }

        private async Task ConfirmSubscriptionAsync(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            try
            {
                var client = _httpClientFactory.CreateClient();
                await client.GetAsync(url);
                _logger.LogInformation("SNS subscription confirmed: {Url}", url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to confirm SNS subscription");
            }
        }

        // ── Legacy endpoints (kept) ───────────────────────────────────────────

        [HttpPost("campaign")]
        [Authorize]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest request)
        {
            try
            {
                var result = await _marketingService.CreateCampaignAsync(request);
                return Ok(ApiResponse<CampaignStatusResponse>.SuccessResponse(result, "Campaign created"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating campaign");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("campaign/{id:int}/statistics")]
        [Authorize]
        public async Task<IActionResult> GetStatistics(int id)
        {
            try
            {
                var result = await _marketingService.GetStatisticsAsync(id);
                return Ok(ApiResponse<CampaignStatisticsResponse>.SuccessResponse(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics for campaign {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("campaign/{id:int}/recipients")]
        [Authorize]
        public async Task<IActionResult> GetRecipients(int id)
        {
            try
            {
                var recipients = await _listMailRepo.GetByCampaignIdAsync(id);
                var result = recipients.Select(r => new RecipientStatusResponse
                {
                    Id              = r.id,
                    Email           = r.Email,
                    FullName        = r.FullName,
                    RecipientStatus = r.RecipientStatus,
                    SentAt          = r.SentAt,
                    OpenedAt        = r.OpenedAt,
                    ClickedAt       = r.ClickedAt,
                    BounceReason    = r.BounceReason
                }).ToList();
                return Ok(ApiResponse<List<RecipientStatusResponse>>.SuccessResponse(result, totalRecords: result.Count));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recipients for campaign {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }
    }
}
