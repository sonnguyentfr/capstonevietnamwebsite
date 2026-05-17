using System;
using System.Threading;
using System.Threading.Tasks;
using NVCMS.API.SendMail.Domain;
namespace NVCMS.API.SendMail.Interfaces
{
    public interface IMailSenderService  { Task<MailSendResult> SendAsync(OutgoingMailMessage msg, CancellationToken ct); }
    public interface IRetryEngine        { DateTime GetNextRetryTime(int retryCount); bool ShouldRetry(int retryCount, int max); }
    public interface IRateLimiter        { Task AcquireAsync(CancellationToken ct); }
    public interface ICampaignQueueService { Task<long> EnqueueCampaignAsync(CreateCampaignRequest req, CancellationToken ct); }
}
