using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Domain;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Worker
{
    public class MailDispatchWorker
    {
        private readonly IMailQueueRepository _queue;
        private readonly IMailSenderService   _sender;
        private readonly IRetryEngine         _retry;
        private readonly IRateLimiter         _rate;
        private readonly ICampaignRepository  _camp;
        private readonly string _workerId = string.Format("worker-{0}-{1:N}", Environment.MachineName, Guid.NewGuid());
        private CancellationTokenSource _cts;
        private Thread _thread;
        public MailDispatchWorker(IMailQueueRepository queue, IMailSenderService sender,
            IRetryEngine retry, IRateLimiter rate, ICampaignRepository camp)
        { _queue=queue; _sender=sender; _retry=retry; _rate=rate; _camp=camp; }
        public void Start()
        {
            _cts = new CancellationTokenSource();
            _thread = new Thread(() => RunLoop(_cts.Token)) { IsBackground=true, Name="MailDispatchWorker" };
            _thread.Start();
            Log("Started [{0}]", _workerId);
        }
        public void Stop() { _cts?.Cancel(); _thread?.Join(TimeSpan.FromSeconds(30)); Log("Stopped"); }
        private void RunLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try   { ProcessBatchAsync(ct).GetAwaiter().GetResult(); }
                catch (OperationCanceledException) { break; }
                catch (Exception ex) { Log("ERROR: {0}", ex.Message); Thread.Sleep(10000); }
            }
        }
        private async Task ProcessBatchAsync(CancellationToken ct)
        {
            var batch = await _queue.FetchAndLockBatchAsync(AppConfig.BatchSize, _workerId, ct);
            if (batch.Count == 0) { await Task.Delay(AppConfig.IdleDelaySeconds * 1000, ct); return; }
            Log("Batch: {0} emails", batch.Count);
            using (var sem = new SemaphoreSlim(AppConfig.MaxConcurrent, AppConfig.MaxConcurrent))
            {
                var tasks = new List<Task>();
                foreach (var item in batch)
                {
                    var cap = item;
                    tasks.Add(Task.Run(async () => {
                        await sem.WaitAsync(ct);
                        try { await _rate.AcquireAsync(ct); await SendOneAsync(cap, ct); }
                        finally { sem.Release(); }
                    }, ct));
                }
                await Task.WhenAll(tasks);
            }
            int sent=0, failed=0;
            foreach (var i in batch)
            {
                if (i.Status == MailQueueStatus.Sent)   sent++;
                if (i.Status == MailQueueStatus.Failed || i.Status == MailQueueStatus.Bounce) failed++;
            }
            if (sent > 0 || failed > 0)
                await _camp.IncrementCountersAsync(batch[0].CampaignId, sent, failed, ct);
        }
        private async Task SendOneAsync(MailQueueItem item, CancellationToken ct)
        {
            var result = await _sender.SendAsync(new OutgoingMailMessage
            {
                From=AppConfig.DefaultFromEmail, FromName=AppConfig.DefaultFromName,
                To=item.Email, Subject=item.Subject, HtmlBody=item.Body
            }, ct);
            if (result.Success)
            {
                item.Status = MailQueueStatus.Sent;
                await _queue.UpdateStatusAsync(item.Id, MailQueueStatus.Sent, result.SmtpResponse, ct);
                Log("OK [{0}] {1}", item.Id, item.Email);
            }
            else if (result.ShouldRetry && _retry.ShouldRetry(item.RetryCount, AppConfig.MaxRetries))
            {
                await _queue.MarkRetryAsync(item.Id, result.Error, _retry.GetNextRetryTime(item.RetryCount), ct);
                Log("RETRY [{0}] {1}", item.Id, item.Email);
            }
            else
            {
                item.Status = MailQueueStatus.Failed;
                await _queue.UpdateStatusAsync(item.Id, MailQueueStatus.Failed, result.Error, ct);
                Log("FAIL [{0}] {1}: {2}", item.Id, item.Email, result.Error);
            }
        }
        private static void Log(string fmt, params object[] args)
            => Console.WriteLine("[{0:HH:mm:ss}] {1}", DateTime.Now, string.Format(fmt, args));
    }
}
