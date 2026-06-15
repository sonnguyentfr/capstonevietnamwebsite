using Hangfire;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Recurring job – chạy mỗi phút.
    /// Quét tất cả campaign có Status=Queued và ScheduledAt &lt;= now,
    /// đánh dấu Sending rồi enqueue CampaignBatchJob theo từng batch 100 recipients.
    /// Không enqueue từng email riêng lẻ.
    /// </summary>
    public class CampaignSchedulerJob
    {
        private const int BatchSize      = 100;
        private const int StatusQueued   = 1;
        private const int StatusSending  = 2;

        private readonly IMarketingCampaignRepository    _campaignRepo;
        private readonly IMarketingListMailRepository    _listMailRepo;
        private readonly IMarketingHangfireLogRepository _logRepo;
        private readonly IBackgroundJobClient            _jobClient;
        private readonly ILogger<CampaignSchedulerJob>  _logger;

        public CampaignSchedulerJob(
            IMarketingCampaignRepository    campaignRepo,
            IMarketingListMailRepository    listMailRepo,
            IMarketingHangfireLogRepository logRepo,
            IBackgroundJobClient            jobClient,
            ILogger<CampaignSchedulerJob>   logger)
        {
            _campaignRepo = campaignRepo;
            _listMailRepo = listMailRepo;
            _logRepo      = logRepo;
            _jobClient    = jobClient;
            _logger       = logger;
        }

        [AutomaticRetry(Attempts = 0)] // scheduler không retry – batch job tự retry
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

                // 1. Đánh dấu Sending ngay
                await _campaignRepo.UpdateStatusAsync(campaign.id, StatusSending, DateTime.UtcNow);

                // 2. Lấy toàn bộ recipients Pending
                var pending = (await _listMailRepo.GetPendingByCampaignIdAsync(campaign.id)).ToList();
                var total   = pending.Count;

                if (total == 0)
                {
                    _logger.LogWarning(
                        "Campaign {Id} has no pending recipients – marking Completed", campaign.id);
                    await _campaignRepo.UpdateStatusAsync(campaign.id, 3, DateTime.UtcNow);
                    continue;
                }

                // 3. Chia thành batch, mỗi batch enqueue 1 job duy nhất
                var batches = (int)Math.Ceiling((double)total / BatchSize);
                for (int batchNo = 0; batchNo < batches; batchNo++)
                {
                    int skip = batchNo * BatchSize;
                    var batchIds = pending
                        .Skip(skip)
                        .Take(BatchSize)
                        .Select(r => r.id)
                        .ToList();

                    _jobClient.Enqueue<CampaignBatchJob>(job =>
                        job.ExecuteAsync(campaign.id, batchNo + 1, batchIds, default));

                    _logger.LogInformation(
                        "Campaign {Id} – enqueued batch {BatchNo}/{Total} ({Count} recipients)",
                        campaign.id, batchNo + 1, batches, batchIds.Count);
                }

                // 4. Ghi log
                await _logRepo.AddAsync(new Entities.MarketingMailHangfireLog
                {
                    CampaignId  = campaign.id,
                    BatchNo     = 0,
                    Status      = "Scheduled",
                    Message     = $"Dispatched {batches} batch(es) for {total} recipients",
                    CreatedDate = DateTime.UtcNow
                });
            }
        }
    }
}
