using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Domain;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Repositories
{
    public class MailQueueRepository : IMailQueueRepository
    {
        private SqlConnection Conn() => new SqlConnection(AppConfig.ConnectionString);

        public async Task<IList<MailQueueItem>> FetchAndLockBatchAsync(int batchSize, string workerId, CancellationToken ct)
        {
            const string sql = @"
                UPDATE TOP (@batchSize) q
                SET q.Status=@processing, q.LockedAt=GETUTCDATE(), q.LockedBy=@workerId
                OUTPUT inserted.Id,inserted.CampaignId,inserted.RecipientId,
                       inserted.Email,inserted.Subject,inserted.Body,
                       inserted.RetryCount,inserted.Status,inserted.LastError,inserted.CreatedDate
                FROM dbo.MailQueue q WITH (ROWLOCK,READPAST)
                WHERE q.Status IN (@pending,@retry)
                  AND (q.NextRetryDate IS NULL OR q.NextRetryDate<=GETUTCDATE())";
            using (var c = Conn())
                return (await c.QueryAsync<MailQueueItem>(new CommandDefinition(sql,
                    new { batchSize, processing=(int)MailQueueStatus.Processing,
                          pending=(int)MailQueueStatus.Pending,
                          retry=(int)MailQueueStatus.Retry, workerId }, cancellationToken:ct))).ToList();
        }

        public async Task UpdateStatusAsync(long id, MailQueueStatus status, string smtpResponse, CancellationToken ct)
        {
            const string sql = @"UPDATE dbo.MailQueue SET Status=@status,
                SentDate=CASE WHEN @status=2 THEN GETUTCDATE() ELSE NULL END,
                LastError=@smtpResponse, LockedAt=NULL, LockedBy=NULL WHERE Id=@id";
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(sql,
                    new { id, status=(int)status, smtpResponse }, cancellationToken:ct));
        }

        public async Task MarkRetryAsync(long id, string error, DateTime nextRetryDate, CancellationToken ct)
        {
            const string sql = @"UPDATE dbo.MailQueue SET Status=@retry,
                RetryCount=RetryCount+1, LastError=@error,
                NextRetryDate=@nextRetryDate, LockedAt=NULL, LockedBy=NULL WHERE Id=@id";
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(sql,
                    new { id, error, nextRetryDate, retry=(int)MailQueueStatus.Retry }, cancellationToken:ct));
        }

        public async Task MarkBounceAsync(long id, string bounceCode, string bounceMessage, CancellationToken ct)
        {
            const string sql = @"UPDATE dbo.MailQueue SET Status=@bounce,
                LastError=@bounceMessage, LockedAt=NULL, LockedBy=NULL WHERE Id=@id";
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(sql,
                    new { id, bounceMessage, bounce=(int)MailQueueStatus.Bounce }, cancellationToken:ct));
        }

        public async Task<int> GetPendingCountAsync(long campaignId, CancellationToken ct)
        {
            using (var c = Conn())
                return await c.ExecuteScalarAsync<int>(new CommandDefinition(
                    "SELECT COUNT(1) FROM dbo.MailQueue WHERE CampaignId=@campaignId AND Status IN (0,1,4)",
                    new { campaignId }, cancellationToken:ct));
        }

        public async Task BulkInsertAsync(IList<MailQueueItem> items, CancellationToken ct)
        {
            if (items == null || items.Count == 0) return;
            using (var c = Conn())
            {
                await c.OpenAsync(ct);
                using (var bulk = new SqlBulkCopy(c))
                {
                    bulk.DestinationTableName = "dbo.MailQueue";
                    bulk.BatchSize = 1000; bulk.BulkCopyTimeout = 300;
                    var dt = new DataTable();
                    dt.Columns.Add("CampaignId",  typeof(long));
                    dt.Columns.Add("RecipientId", typeof(long));
                    dt.Columns.Add("Email",       typeof(string));
                    dt.Columns.Add("Subject",     typeof(string));
                    dt.Columns.Add("Body",        typeof(string));
                    dt.Columns.Add("Status",      typeof(int));
                    dt.Columns.Add("RetryCount",  typeof(int));
                    dt.Columns.Add("CreatedDate", typeof(DateTime));
                    foreach (var item in items)
                        dt.Rows.Add(item.CampaignId, item.RecipientId, item.Email,
                            item.Subject, item.Body, (int)MailQueueStatus.Pending, 0, DateTime.UtcNow);
                    foreach (DataColumn col in dt.Columns) bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    await bulk.WriteToServerAsync(dt);
                }
            }
        }
    }
}
