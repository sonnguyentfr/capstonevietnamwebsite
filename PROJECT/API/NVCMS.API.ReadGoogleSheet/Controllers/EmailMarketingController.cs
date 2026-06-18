using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    [Route("api/emailmarketing")]
    [ApiController]
    [Authorize]
    public class EmailMarketingController : ControllerBase
    {
        private readonly IEmailMarketingService          _marketingService;
        private readonly IMarketingListMailRepository    _listMailRepo;
        private readonly IMarketingSendLogRepository     _sendLogRepo;
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
            IMarketingSendLogRepository       sendLogRepo,
            IMarketingUnsubRepository         unsubRepo,
            IHttpClientFactory                httpClientFactory,
            ILogger<EmailMarketingController> logger)
        {
            _marketingService  = marketingService;
            _listMailRepo      = listMailRepo;
            _sendLogRepo       = sendLogRepo;
            _unsubRepo         = unsubRepo;
            _httpClientFactory = httpClientFactory;
            _logger            = logger;
        }

        // -- POST /api/emailmarketing/send -------------------------------------
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
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // -- POST /api/emailmarketing/campaign ---------------------------------
        [HttpPost("campaign")]
        [Authorize]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest request)
        {
            try
            {
                var result = await _marketingService.CreateCampaignAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating campaign");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // -- GET /api/emailmarketing/campaign/{id}/statistics ------------------
        [HttpGet("campaign/{id:int}/statistics")]
        [Authorize]
        public async Task<IActionResult> GetStatistics(int id)
        {
            try
            {
                var result = await _marketingService.GetStatisticsAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics for campaign {Id}", id);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // -- GET /api/emailmarketing/campaign/{id}/logs ------------------------
        [HttpGet("campaign/{id:int}/logs")]
        [Authorize]
        public async Task<IActionResult> GetSendLogs(int id)
        {
            try
            {
                var logs = (await _sendLogRepo.GetAllByCampaignIdAsync(id)).ToList();
                return Ok(new { totalRecords = logs.Count, data = logs });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting send logs for campaign {Id}", id);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // -- POST /api/emailmarketing/sns (PUBLIC – AWS SNS callback) ----------
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

            var sendLog = await _sendLogRepo.GetBySesMessageIdAsync(messageId);
            var eventType = (payload.eventType ?? string.Empty).ToUpperInvariant();

            switch (eventType)
            {
                case "DELIVERY":
                    if (sendLog is not null)
                        await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Delivered);
                    break;

                case "OPEN":
                    if (sendLog is not null)
                        await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Opened);
                    break;

                case "CLICK":
                    if (sendLog is not null)
                        await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Clicked);
                    break;

                case "BOUNCE":
                    if (sendLog is not null)
                        await _sendLogRepo.UpdateStatusAsync(sendLog.Id, MailSendStatus.Failed,
                            errorMessage: $"Bounce: {payload.bounce?.bounceType}/{payload.bounce?.bounceSubType}");
                    break;

                case "COMPLAINT":
                    if (sendLog is not null)
                    {
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
                    }
                    break;

                default:
                    _logger.LogDebug("SNS: unhandled eventType={EventType}", eventType);
                    break;
            }

            return Ok("Processed");
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
    }
}
