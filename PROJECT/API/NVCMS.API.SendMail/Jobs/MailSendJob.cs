using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Domain;
using NVCMS.API.SendMail.Interfaces;

namespace NVCMS.API.SendMail.Jobs
{
    public class MailSendJob
    {
        private readonly IMailSenderService    _sender;
        private readonly IMailQueueRepository  _queue;
        private readonly ICampaignRepository   _camp;

        public MailSendJob(IMailSenderService sender, IMailQueueRepository queue, ICampaignRepository camp)
        {
            _sender = sender;
            _queue  = queue;
            _camp   = camp;
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
        public async Task SendAsync(int mailQueueItemId)
        {
            var ct   = CancellationToken.None;
            var item = await _queue.GetByIdAsync(mailQueueItemId, ct);
            if (item == null || item.Status == MailQueueStatus.Sent) return;

            var result = await _sender.SendAsync(new OutgoingMailMessage
            {
                From     = AppConfig.DefaultFromEmail,
                FromName = AppConfig.DefaultFromName,
                To       = item.Email,
                Subject  = item.Subject,
                HtmlBody = item.Body
            }, ct);

            if (result.Success)
            {
                await _queue.UpdateStatusAsync(item.Id, MailQueueStatus.Sent, result.SmtpResponse, ct);
                await _camp.IncrementCountersAsync(item.CampaignId, 1, 0, ct);
            }
            else
            {
                await _queue.UpdateStatusAsync(item.Id, MailQueueStatus.Failed, result.ErrorMessage, ct);
                if (!result.CanRetry)
                    await _camp.IncrementCountersAsync(item.CampaignId, 0, 1, ct);
                // Ném exception để Hangfire tự retry nếu CanRetry = true
                if (result.CanRetry)
                    throw new System.Exception(result.ErrorMessage);
            }
        }

        /// <summary>
        /// Job recurring: quét queue và enqueue từng email vào Hangfire
        /// </summary>
        public async Task EnqueuePendingMailsAsync()
        {
            var ct    = CancellationToken.None;
            var batch = await _queue.FetchAndLockBatchAsync(AppConfig.BatchSize, "hangfire", ct);
            foreach (var item in batch)
                BackgroundJob.Enqueue<MailSendJob>(j => j.SendAsync(item.Id));
        }
    }
}