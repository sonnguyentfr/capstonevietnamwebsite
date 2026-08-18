using Hangfire;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Hangfire job ??c Marketing_Mail_Send_Log (Status = "Queued") cho m?t campaignSendId,
    /// chèn tracking pixel vào body theo log.Id, g?i qua AWS SES theo batch 100,
    /// c?p nh?t Status sau m?i l?n g?i.
    /// </summary>
    public class CampaignBatchJob
    {
        private const int BatchSize = 100;
        private static readonly TimeSpan BatchDelay = TimeSpan.FromSeconds(2);

        private readonly IMarketingSendLogRepository _sendLogRepo;
        private readonly ISESService _sesService;
        private readonly IMailAccountRepository _mailAccountRepo;
        private readonly IConfiguration _config;
        private readonly ILogger<CampaignBatchJob> _logger;

        public CampaignBatchJob(
            IMarketingSendLogRepository sendLogRepo,
            ISESService sesService,
            IMailAccountRepository mailAccountRepo,
            IConfiguration config,
            ILogger<CampaignBatchJob> logger)
        {
            _sendLogRepo     = sendLogRepo;
            _sesService      = sesService;
            _mailAccountRepo = mailAccountRepo;
            _config          = config;
            _logger          = logger;
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
        public async Task ExecuteAsync(int campaignSendId, string subject, string body, int emailAccountId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("CampaignBatchJob: start campaignSendId={CampaignSendId}", campaignSendId);

            var apiBaseUrl = (_config["ApiSelfBaseUrl"] ?? string.Empty).TrimEnd('/');

            var mailAccount  = await _mailAccountRepo.GetByIdAsync(emailAccountId);
            var fromEmail    = mailAccount?.Mail ?? string.Empty;
            var fromName     = mailAccount?.Name ?? string.Empty;

            var queuedLogs = (await _sendLogRepo.GetQueuedByCampaignIdAsync(campaignSendId)).ToList();
            _logger.LogInformation("CampaignBatchJob: {Count} queued send-logs", queuedLogs.Count);
            int totalSent   = 0;
            int totalFailed = 0;
            int batchNo     = 0;

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
                        // Chèn tracking pixel riêng cho t?ng email, dùng log.Id làm logId
                        var trackingPixel    = $"<img src=\"{apiBaseUrl}/api/EmailTracking/open?id={log.Id}\" width=\"1\" height=\"1\" alt=\"\" style=\"display:none;\">";
                        var personalizedBody = InjectTrackingPixel(body, trackingPixel);

                        var sesMessageId = await _sesService.SendBodyEmailAsync(fromEmail, log.Email, fromName, subject, personalizedBody);
                        await _sendLogRepo.UpdateStatusAsync(log.Id, MailSendStatus.Sent, sesMessageId: sesMessageId);
                        totalSent++;
                        _logger.LogDebug("Sent to {Email} logId={LogId} sesId={SesId}", log.Email, log.Id, sesMessageId);
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

            _logger.LogInformation("CampaignBatchJob: done campaignSendId={CampaignSendId} sent={Sent} failed={Failed}", campaignSendId, totalSent, totalFailed);
        }

        /// <summary>
        /// Chèn tracking pixel ngay tr??c &lt;/body&gt;.
        /// N?u body không có th? &lt;/body&gt; thì append cu?i chu?i.
        /// </summary>
        private static string InjectTrackingPixel(string body, string pixel)
        {
            const string closeBody = "</body>";
            var idx = body.LastIndexOf(closeBody, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0)
                return body.Insert(idx, pixel);

            return body + pixel;
        }
    }
}
