using Hangfire;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Hangfire job d?c Marketing_Mail_Send_Log (Status = "Queued") cho m?t campaign,
    /// g?i qua AWS SES theo batch 100, c?p nh?t Status sau m?i l?n g?i.
    /// </summary>
    public class CampaignBatchJob
    {
        private const int BatchSize = 100;
        private static readonly TimeSpan BatchDelay = TimeSpan.FromSeconds(2);

        private readonly IMarketingSendLogRepository _sendLogRepo;
        private readonly ISESService _sesService;
        private readonly ILogger<CampaignBatchJob> _logger;

        public CampaignBatchJob(
            IMarketingSendLogRepository sendLogRepo,
            ISESService sesService,
            ILogger<CampaignBatchJob> logger)
        {
            _sendLogRepo = sendLogRepo;
            _sesService = sesService;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
        public async Task ExecuteAsync(int campaignId, string subject, string body, int emailAccountId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("CampaignBatchJob: start campaignId={CampaignId}", campaignId);
            var queuedLogs = (await _sendLogRepo.GetQueuedByCampaignIdAsync(campaignId)).ToList();
            _logger.LogInformation("CampaignBatchJob: {Count} queued send-logs", queuedLogs.Count);
            int totalSent = 0;
            int totalFailed = 0;
            int batchNo = 0;

            for (int i = 0; i < queuedLogs.Count; i += BatchSize)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("CampaignBatchJob: cancelled at batch {Batch}", batchNo);
                    break;
                }

                batchNo++;
                var batch = queuedLogs.Skip(i).Take(BatchSize).ToList();
                _logger.LogInformation("CampaignBatchJob: batch {Batch} – {Count} emails", batchNo, batch.Count);

                foreach (var log in batch)
                {
                    try
                    {
                        var sesMessageId = await _sesService.SendBodyEmailAsync(log.Email, string.Empty, subject, body);
                        await _sendLogRepo.UpdateStatusAsync(log.Id, MailSendStatus.Sent, sesMessageId: sesMessageId);
                        totalSent++;
                        _logger.LogDebug("Sent to {Email} sesId={SesId}", log.Email, sesMessageId);
                    }
                    catch (Exception ex)
                    {
                        await _sendLogRepo.UpdateStatusAsync(log.Id, MailSendStatus.Failed, errorMessage: ex.Message);
                        totalFailed++;
                        _logger.LogError(ex, "Failed to send to {Email} logId={LogId}", log.Email, log.Id);
                    }
                }

                if (i + BatchSize < queuedLogs.Count)
                    await Task.Delay(BatchDelay, cancellationToken);
            }

            _logger.LogInformation("CampaignBatchJob: done campaignId={CampaignId} sent={Sent} failed={Failed}", campaignId, totalSent, totalFailed);
        }
    }
}
