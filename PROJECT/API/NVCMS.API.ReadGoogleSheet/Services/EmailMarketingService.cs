using Hangfire;
using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Data;
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
        private readonly CRMDbContext                   _crmContext;
        private readonly ILogger<EmailMarketingService> _logger;

        public EmailMarketingService(
            IMarketingCampaignRepository   campaignRepo,
            IMarketingListMailRepository   listMailRepo,
            IMarketingUnsubRepository      unsubRepo,
            IMarketingSendLogRepository    sendLogRepo,
            IBackgroundJobClient           jobClient,
            CRMDbContext                   crmContext,
            ILogger<EmailMarketingService> logger)
        {
            _campaignRepo = campaignRepo;
            _listMailRepo = listMailRepo;
            _unsubRepo    = unsubRepo;
            _sendLogRepo  = sendLogRepo;
            _jobClient    = jobClient;
            _crmContext   = crmContext;
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
            var seen          = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var validMailItems = new List<Marketing_Mail_ListMail>();

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

                validMailItems.Add(lm);
            }

            // Bước 4 – Insert 1 bản ghi Marketing_Mail_Campaign_Send cho lần gửi này
            var campaignSend = new MarketingMailCampaignSend
            {
                CampaignId     = request.CampaignId,
                Subject        = request.Subject,
                Body           = request.Body,
                Status         = 0,
                TotalRecipient = validMailItems.Count,
                CreatedDate    = DateTime.UtcNow
            };
            _crmContext.CampaignSends.Add(campaignSend);
            await _crmContext.SaveChangesAsync();

            _logger.LogInformation("Inserted Campaign_Send Id={CampaignSendId} for campaign {CampaignId}",
                campaignSend.Id, request.CampaignId);

            // Bước 5 – Insert Marketing_Mail_Send_Log (1 bản ghi / email), CampaignSendId = campaignSend.Id
            var sendLogs = validMailItems.Select(lm => new MarketingMailSendLog
            {
                CampaignSendId = campaignSend.Id,
                ListMailId     = lm.id,
                Email          = lm.Email!.Trim(),
                Status         = MailSendStatus.Queued,
                CreatedDate    = DateTime.UtcNow
            }).ToList();

            if (sendLogs.Count > 0)
                await _sendLogRepo.AddRangeAsync(sendLogs);

            _logger.LogInformation("Inserted {Count} Queued send-logs for campaignSendId={CampaignSendId}",
                sendLogs.Count, campaignSend.Id);

            // Bước 6 – Enqueue Hangfire job trực tiếp
            _jobClient.Enqueue<CampaignBatchJob>(job =>
                job.ExecuteAsync(
                    campaignSend.Id,
                    request.Subject,
                    request.Body,
                    request.EmailAccountId,
                    CancellationToken.None));

            _logger.LogInformation("Enqueued CampaignBatchJob for campaignSendId={Id}", campaignSend.Id);

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

            // Lấy tất cả Campaign_Send Ids thuộc campaign này
            var campaignSendIds = await _crmContext.CampaignSends
                .Where(cs => cs.CampaignId == campaignId)
                .Select(cs => cs.Id)
                .ToListAsync();

            var logs = campaignSendIds.Count > 0
                ? (await _crmContext.SendLogs
                    .Where(l => campaignSendIds.Contains(l.CampaignSendId))
                    .ToListAsync())
                : new List<MarketingMailSendLog>();

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
