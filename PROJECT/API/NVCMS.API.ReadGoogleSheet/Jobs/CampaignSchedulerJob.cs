using Hangfire;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Recurring job – chạy mỗi phút.
    /// Quét tất cả campaign có Status=Queued và ScheduledAt &lt;= now,
    /// gọi EmailMarketingService.SendCampaignAsync để tạo CampaignSend và enqueue CampaignBatchJob.
    /// </summary>
    public class CampaignSchedulerJob
    {
        private const int StatusQueued  = 1;
        private const int StatusSending = 2;

        private readonly IMarketingCampaignRepository    _campaignRepo;
        private readonly IMarketingHangfireLogRepository _logRepo;
        private readonly IEmailMarketingService          _marketingService;
        private readonly ILogger<CampaignSchedulerJob>   _logger;

        public CampaignSchedulerJob(
            IMarketingCampaignRepository    campaignRepo,
            IMarketingHangfireLogRepository logRepo,
            IEmailMarketingService          marketingService,
            ILogger<CampaignSchedulerJob>   logger)
        {
            _campaignRepo     = campaignRepo;
            _logRepo          = logRepo;
            _marketingService = marketingService;
            _logger           = logger;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ExecuteAsync()
        {
            var queued = await _campaignRepo.GetByStatusAsync(StatusQueued);
            var due    = queued
                .Where(c => c.ScheduledAt == null || c.ScheduledAt <= DateTime.UtcNow)
                .ToList();

            if (!due.Any())
            {
                _logger.LogDebug("CampaignSchedulerJob: no due campaigns");
                return;
            }

            foreach (var campaign in due)
            {
                _logger.LogInformation(
                    "CampaignSchedulerJob: starting campaign {Id} '{Title}'",
                    campaign.id, campaign.Title);

                try
                {
                    var result = await _marketingService.SendCampaignAsync(new SendCampaignBodyRequest
                    {
                        CampaignId = campaign.id,
                        Subject    = campaign.Subject ?? string.Empty,
                        Body       = string.Empty
                    });

                    await _campaignRepo.UpdateStatusAsync(campaign.id, StatusSending, DateTime.UtcNow);

                    await _logRepo.AddAsync(new Entities.MarketingMailHangfireLog
                    {
                        CampaignId  = campaign.id,
                        BatchNo     = 0,
                        Status      = "Scheduled",
                        Message     = $"CampaignSendId={result.CampaignSendId}, TotalRecipient={result.TotalRecipient}",
                        CreatedDate = DateTime.UtcNow
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CampaignSchedulerJob: failed for campaign {Id}", campaign.id);
                }
            }
        }
    }
}
