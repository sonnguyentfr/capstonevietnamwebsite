using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NVCMS.API.SendMail.Domain;
namespace NVCMS.API.SendMail.Interfaces
{
    public interface IMailQueueRepository
    {
        Task<IList<MailQueueItem>> FetchAndLockBatchAsync(int batchSize, string workerId, CancellationToken ct);
        Task UpdateStatusAsync(long id, MailQueueStatus status, string smtpResponse, CancellationToken ct);
        Task MarkRetryAsync(long id, string error, DateTime nextRetryDate, CancellationToken ct);
        Task MarkBounceAsync(long id, string bounceCode, string bounceMessage, CancellationToken ct);
        Task<int> GetPendingCountAsync(long campaignId, CancellationToken ct);
        Task BulkInsertAsync(IList<MailQueueItem> items, CancellationToken ct);
    }
    public interface ICampaignRepository
    {
        Task<Campaign> GetByIdAsync(long id, CancellationToken ct);
        Task<IList<Campaign>> GetAllAsync(CancellationToken ct);
        Task<long> CreateAsync(Campaign campaign, CancellationToken ct);
        Task UpdateAsync(Campaign campaign, CancellationToken ct);
        Task IncrementCountersAsync(long id, int sent, int failed, CancellationToken ct);
        Task<CampaignStats> GetStatsAsync(long id, CancellationToken ct);
    }
    public interface IUnsubscribeRepository
    {
        Task<HashSet<string>> GetAllEmailsAsync(CancellationToken ct);
        Task AddAsync(string email, string source, CancellationToken ct);
        Task<bool> IsUnsubscribedAsync(string email, CancellationToken ct);
    }
    public interface IMailTrackingRepository
    {
        Task RecordOpenAsync(long queueId, string userAgent, string ipAddress, CancellationToken ct);
        Task RecordClickAsync(long queueId, string url, string userAgent, string ipAddress, CancellationToken ct);
    }
}
