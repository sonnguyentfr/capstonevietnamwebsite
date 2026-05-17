using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Repositories
{
    public class UnsubscribeRepository : IUnsubscribeRepository
    {
        private SqlConnection Conn() => new SqlConnection(AppConfig.ConnectionString);
        public async Task<HashSet<string>> GetAllEmailsAsync(CancellationToken ct)
        {
            using (var c = Conn())
            {
                var emails = await c.QueryAsync<string>(new CommandDefinition(
                    "SELECT Email FROM dbo.Unsubscribe", cancellationToken:ct));
                return new HashSet<string>(emails, StringComparer.OrdinalIgnoreCase);
            }
        }
        public async Task AddAsync(string email, string source, CancellationToken ct)
        {
            const string sql = @"IF NOT EXISTS (SELECT 1 FROM dbo.Unsubscribe WHERE Email=@email)
                INSERT INTO dbo.Unsubscribe (Email,Source,UnsubDate) VALUES (@email,@source,GETUTCDATE())";
            using (var c = Conn())
                await c.ExecuteAsync(new CommandDefinition(sql, new { email, source }, cancellationToken:ct));
        }
        public async Task<bool> IsUnsubscribedAsync(string email, CancellationToken ct)
        {
            using (var c = Conn())
                return await c.ExecuteScalarAsync<int>(new CommandDefinition(
                    "SELECT COUNT(1) FROM dbo.Unsubscribe WHERE Email=@email",
                    new { email }, cancellationToken:ct)) > 0;
        }
    }
}
