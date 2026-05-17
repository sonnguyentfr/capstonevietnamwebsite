using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Repositories
{
    public class MailTrackingRepository : IMailTrackingRepository
    {
        private SqlConnection Conn() => new SqlConnection(AppConfig.ConnectionString);
        public async Task RecordOpenAsync(long queueId, string userAgent, string ipAddress, CancellationToken ct)
        {
            const string sql = @"INSERT INTO dbo.MailTracking(QueueId,TrackingType,IpAddress,UserAgent,TrackedDate)
                VALUES(@queueId,0,@ipAddress,@userAgent,GETUTCDATE());
                UPDATE dbo.Campaign SET OpenCount=OpenCount+1
                WHERE Id=(SELECT CampaignId FROM dbo.MailQueue WHERE Id=@queueId)";
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(sql, new { queueId,userAgent,ipAddress }, cancellationToken:ct));
        }
        public async Task RecordClickAsync(long queueId, string url, string userAgent, string ipAddress, CancellationToken ct)
        {
            const string sql = @"INSERT INTO dbo.MailTracking(QueueId,TrackingType,ClickUrl,IpAddress,UserAgent,TrackedDate)
                VALUES(@queueId,1,@url,@ipAddress,@userAgent,GETUTCDATE());
                UPDATE dbo.Campaign SET ClickCount=ClickCount+1
                WHERE Id=(SELECT CampaignId FROM dbo.MailQueue WHERE Id=@queueId)";
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(sql, new { queueId,url,userAgent,ipAddress }, cancellationToken:ct));
        }
    }
}
