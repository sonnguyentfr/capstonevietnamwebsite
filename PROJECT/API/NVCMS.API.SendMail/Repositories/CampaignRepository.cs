using System.Collections.Generic;
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
    public class CampaignRepository : ICampaignRepository
    {
        private SqlConnection Conn() => new SqlConnection(AppConfig.ConnectionString);

        public async Task<Campaign> GetByIdAsync(long id, CancellationToken ct)
        {
            using (var c = Conn())
                return await c.QueryFirstOrDefaultAsync<Campaign>(new CommandDefinition(
                    "SELECT * FROM dbo.Campaign WHERE Id=@id", new { id }, cancellationToken:ct));
        }
        public async Task<IList<Campaign>> GetAllAsync(CancellationToken ct)
        {
            using (var c = Conn())
                return (await c.QueryAsync<Campaign>(new CommandDefinition(
                    "SELECT * FROM dbo.Campaign ORDER BY CreatedDate DESC", cancellationToken:ct))).ToList();
        }
        public async Task<long> CreateAsync(Campaign x, CancellationToken ct)
        {
            const string sql = @"INSERT INTO dbo.Campaign
                (Name,Subject,HtmlContent,Status,FromEmail,FromName,CreatedDate,ScheduledDate,TotalRecipients)
                VALUES(@Name,@Subject,@HtmlContent,@Status,@FromEmail,@FromName,@CreatedDate,@ScheduledDate,@TotalRecipients);
                SELECT SCOPE_IDENTITY();";
            using (var c = Conn())
                return await c.ExecuteScalarAsync<long>(new CommandDefinition(sql, x, cancellationToken:ct));
        }
        public async Task UpdateAsync(Campaign x, CancellationToken ct)
        {
            const string sql = @"UPDATE dbo.Campaign SET Name=@Name,Subject=@Subject,
                HtmlContent=@HtmlContent,Status=@Status,FromEmail=@FromEmail,
                FromName=@FromName,ScheduledDate=@ScheduledDate WHERE Id=@Id";
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(sql, x, cancellationToken:ct));
        }
        public async Task IncrementCountersAsync(long id, int sent, int failed, CancellationToken ct)
        {
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(
                    "UPDATE dbo.Campaign SET SentCount=SentCount+@sent,FailedCount=FailedCount+@failed WHERE Id=@id",
                    new { id, sent, failed }, cancellationToken:ct));
        }
        public async Task<CampaignStats> GetStatsAsync(long id, CancellationToken ct)
        {
            const string sql = @"SELECT c.Id AS CampaignId,c.Name,c.TotalRecipients AS Total,
                c.SentCount AS Sent,c.FailedCount AS Failed,c.OpenCount AS Opens,c.ClickCount AS Clicks,
                (SELECT COUNT(1) FROM dbo.MailQueue q WHERE q.CampaignId=c.Id AND q.Status IN (0,1,4)) AS Pending
                FROM dbo.Campaign c WHERE c.Id=@id";
            using (var c = Conn())
            {
                var s = await c.QueryFirstOrDefaultAsync<CampaignStats>(new CommandDefinition(sql, new { id }, cancellationToken:ct));
                return s ?? new CampaignStats();
            }
        }
    }
}
