using Hangfire;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class EmailMarketingService : IEmailMarketingService
    {
        private readonly IMarketingCampaignRepository   _campaignRepo;
        private readonly IMarketingListMailRepository   _listMailRepo;
        private readonly IMarketingUnsubRepository      _unsubRepo;
        private readonly IMarketingSendLogRepository    _sendLogRepo;
        private readonly IBackgroundJobClient           _jobClient;
        private readonly ILogger<EmailMarketingService> _logger;

        public EmailMarketingService(
            IMarketingCampaignRepository   campaignRepo,
            IMarketingListMailRepository   listMailRepo,
            IMarketingUnsubRepository      unsubRepo,
            IMarketingSendLogRepository    sendLogRepo,
            IBackgroundJobClient           jobClient,
            ILogger<EmailMarketingService> logger)
        {
            _campaignRepo = campaignRepo;
            _listMailRepo = listMailRepo;
            _unsubRepo    = unsubRepo;
            _sendLogRepo  = sendLogRepo;
            _jobClient    = jobClient;
            _logger       = logger;
        }

        // ── SendCampaignAsync ─────────────────────────────────────────────────
        public async Task<SendCampaignResult> SendCampaignAsync(SendCampaignBodyRequest request)
        {
            _logger.LogInformation("SendCampaignAsync: campaignId={CampaignId}", request.CampaignId);

            // Bước 1 – Lấy toàn bộ email từ Marketing_Mail_ListMail
            var listMails = (await _listMailRepo.GetByCampaignIdAsync(request.CampaignId)).ToList();
            _logger.LogInformation("Found {Count} list-mail rows for campaign {Id}",
                listMails.Count, request.CampaignId);

            // Bước 2+3 – Lọc unsub, làm sạch email (trim/distinct/validate)
            var seen     = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var sendLogs = new List<MarketingMailSendLog>();

            foreach (var lm in listMails)
            {
                var email = lm.Email?.Trim();
                if (string.IsNullOrEmpty(email)) continue;
                if (!IsValidEmail(email)) continue;
                if (!seen.Add(email)) continue;

                var isUnsub = await _unsubRepo.IsUnsubscribedAsync(
                    email.ToLowerInvariant(), lm.PortalId ?? 0);
                if (isUnsub)
                {
                    _logger.LogDebug("Skip unsubscribed {Email}", email);
                    continue;
                }

                // Bước 4 – Tạo send-log với Status = Queued
                sendLogs.Add(new MarketingMailSendLog
                {
                    CampaignSendId = request.CampaignId,
                    ListMailId     = lm.id,
                    Email          = email,
                    Status         = MailSendStatus.Queued,
                    CreatedDate    = DateTime.UtcNow
                });
            }

            if (sendLogs.Count > 0)
                await _sendLogRepo.AddRangeAsync(sendLogs);

            _logger.LogInformation("Inserted {Count} Queued send-logs for campaign {Id}",
                sendLogs.Count, request.CampaignId);

            // Bước 5 – Enqueue Hangfire job trực tiếp
            _jobClient.Enqueue<CampaignBatchJob>(job =>
                job.ExecuteAsync(
                    request.CampaignId,
                    request.Subject,
                    request.Body,
                    request.EmailAccountId,
                    CancellationToken.None));

            _logger.LogInformation("Enqueued CampaignBatchJob for campaignId={Id}", request.CampaignId);

            return new SendCampaignResult
            {
                Success        = true,
                CampaignId     = request.CampaignId,
                TotalRecipient = sendLogs.Count
            };
        }

        // ── CreateCampaign ────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> CreateCampaignAsync(CreateCampaignRequest request)
        {
            var campaign = new Marketing_Mail_Campaing
            {
                Title       = request.Title,
                Description = request.Description,
                PortalId    = request.PortalId,
                UserId      = request.UserId,
                CreatedDate = DateTime.UtcNow
            };

            await _campaignRepo.AddAsync(campaign);
            _logger.LogInformation("Created campaign {Id} '{Title}'", campaign.id, campaign.Title);

            return new CampaignStatusResponse
            {
                Id    = campaign.id,
                Title = campaign.Title
            };
        }

        // ── GetStatistics ─────────────────────────────────────────────────────
        public async Task<CampaignStatisticsResponse> GetStatisticsAsync(int campaignId)
        {
            var campaign = await _campaignRepo.GetByIdAsync(campaignId);
            if (campaign is null)
                throw new KeyNotFoundException($"Campaign {campaignId} not found");

            var logs = (await _sendLogRepo.GetAllByCampaignIdAsync(campaignId)).ToList();

            return new CampaignStatisticsResponse
            {
                CampaignId      = campaign.id,
                Title           = campaign.Title ?? string.Empty,
                StatusLabel     = string.Empty,
                TotalRecipients = logs.Count,
                SentCount       = logs.Count(l => l.Status == MailSendStatus.Sent),
                DeliveredCount  = logs.Count(l => l.Status == MailSendStatus.Delivered),
                OpenedCount     = logs.Count(l => l.Status == MailSendStatus.Opened),
                ClickedCount    = logs.Count(l => l.Status == MailSendStatus.Clicked),
                BouncedCount    = 0,
                ComplaintCount  = 0,
                UnsubscribedCount = 0,
                FailedCount     = logs.Count(l => l.Status == MailSendStatus.Failed),
            };
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static bool IsValidEmail(string email)
        {
            try { _ = new System.Net.Mail.MailAddress(email); return true; }
            catch { return false; }
        }
    }
}
