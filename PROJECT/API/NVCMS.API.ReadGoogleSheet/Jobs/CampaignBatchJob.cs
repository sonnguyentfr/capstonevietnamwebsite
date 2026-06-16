using Hangfire;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Hangfire job xử lý gửi email cho một CampaignSend.
    /// Đọc Marketing_Mail_Send_Log (Status=Queued), gửi theo batch 50, delay giữa các batch.
    /// </summary>
    public class CampaignBatchJob
    {
        private const int BatchSize = 50;
        private static readonly TimeSpan BatchDelay = TimeSpan.FromSeconds(2);

        private readonly IMarketingCampaignSendRepository _campaignSendRepo;
        private readonly IMarketingSendLogRepository      _sendLogRepo;
        private readonly IMarketingListMailRepository     _listMailRepo;
        private readonly IMarketingEventRepository        _eventRepo;
        private readonly ISESService                      _sesService;
        private readonly ILogger<CampaignBatchJob>        _logger;

        public CampaignBatchJob(
            IMarketingCampaignSendRepository campaignSendRepo,
            IMarketingSendLogRepository      sendLogRepo,
            IMarketingListMailRepository     listMailRepo,
            IMarketingEventRepository        eventRepo,
            ISESService                      sesService,
            ILogger<CampaignBatchJob>        logger)
        {
            _campaignSendRepo = campaignSendRepo;
            _sendLogRepo      = sendLogRepo;
            _listMailRepo     = listMailRepo;
            _eventRepo        = eventRepo;
            _sesService       = sesService;
            _logger           = logger;
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
        public async Task ExecuteAsync(int campaignSendId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CampaignBatchJob: start campaignSendId={Id}", campaignSendId);

            var campaignSend = await _campaignSendRepo.GetByIdAsync(campaignSendId);
            if (campaignSend is null)
            {
                _logger.LogError("CampaignBatchJob: CampaignSend {Id} not found", campaignSendId);
                return;
            }

            // Đánh dấu Processing
            campaignSend.Status      = "Processing";
            campaignSend.StartedTime = DateTime.UtcNow;
            await _campaignSendRepo.UpdateAsync(campaignSend);

            var subject = campaignSend.Subject ?? string.Empty;
            var body    = campaignSend.Body    ?? string.Empty;

            // Lấy toàn bộ SendLog Queued
            var queuedLogs = (await _sendLogRepo.GetQueuedByCampaignSendIdAsync(campaignSendId)).ToList();
            _logger.LogInformation("CampaignBatchJob: {Count} queued logs for {Id}", queuedLogs.Count, campaignSendId);

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
                _logger.LogInformation(
                    "CampaignBatchJob: batch {Batch} – {Count} emails", batchNo, batch.Count);

                foreach (var log in batch)
                {
                    try
                    {
                        var messageId = await _sesService.SendBodyEmailAsync(
                            log.Email ?? string.Empty,
                            string.Empty,
                            subject,
                            body);

                        await _sendLogRepo.UpdateStatusAsync(log.Id, "Sent", sesMessageId: messageId);

                        // Cập nhật MessageId vào listMail để SNS webhook lookup được
                        if (log.ListMailId.HasValue)
                        {
                            var listMail = await _listMailRepo.GetByIdAsync(log.ListMailId.Value);
                            if (listMail is not null)
                            {
                                listMail.MessageId      = messageId;
                                listMail.RecipientStatus = 1; // Sent
                                listMail.SentAt          = DateTime.UtcNow;
                                listMail.sendcount       = (listMail.sendcount ?? 0) + 1;
                                await _listMailRepo.UpdateAsync(listMail);
                            }
                        }

                        totalSent++;
                        _logger.LogDebug("Sent to {Email} msgId={MsgId}", log.Email, messageId);
                    }
                    catch (Exception ex)
                    {
                        await _sendLogRepo.UpdateStatusAsync(log.Id, "Failed", errorMessage: ex.Message);
                        totalFailed++;

                        _logger.LogError(ex, "Failed to send to {Email} logId={LogId}", log.Email, log.Id);
                    }
                }

                // Cập nhật TotalSent liên tục
                campaignSend = await _campaignSendRepo.GetByIdAsync(campaignSendId);
                if (campaignSend is not null)
                {
                    campaignSend.TotalSent = totalSent;
                    await _campaignSendRepo.UpdateAsync(campaignSend);
                }

                // Delay giữa các batch để tránh throttle SES
                if (i + BatchSize < queuedLogs.Count)
                    await Task.Delay(BatchDelay, cancellationToken);
            }

            // Finalize
            campaignSend = await _campaignSendRepo.GetByIdAsync(campaignSendId);
            if (campaignSend is not null)
            {
                campaignSend.TotalSent     = totalSent;
                campaignSend.Status        = totalFailed > 0 && totalSent == 0 ? "Failed" : "Completed";
                campaignSend.CompletedTime = DateTime.UtcNow;
                await _campaignSendRepo.UpdateAsync(campaignSend);
            }

            _logger.LogInformation(
                "CampaignBatchJob: done campaignSendId={Id} sent={Sent} failed={Failed}",
                campaignSendId, totalSent, totalFailed);
        }
    }
}
