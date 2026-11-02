using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    /// <summary>
    /// BIRTHDAY-MAIL 02/11/2026: truy cập bảng NVCore_BirthdayMail_* trong DB CRM (DefaultCRMConnection) — chỉ qua stored procedure
    /// (script: CRM PROJECT\NVCore.Modules.BirthdayMail\SQL\NVCore_BirthdayMail.sql).
    /// </summary>
    public interface IBirthdayMailRepository
    {
        Task<IReadOnlyList<BirthdayMailConfigRow>> GetEnabledConfigsAsync();
        Task<BirthdayMailConfigRow?> GetConfigAsync(int portalId);
        Task<BirthdayMailTemplateRow?> GetTemplateAsync(int portalId, int templateId);
        Task<IReadOnlyList<BirthdayMailItemRow>> GetActiveItemsAsync(int portalId, DateTime date);
        Task<IReadOnlyList<BirthdayRecipientRow>> GetRecipientsAsync(int portalId, DateTime date, int runId, bool onlyConsented, int top);

        Task<BirthdayRunBeginRow> BeginRunAsync(int portalId, DateTime runDate, bool manual, string triggeredBy, int staleMinutes, DateTime now);
        Task<BirthdayRunInfoRow?> GetRunByDateAsync(int portalId, DateTime runDate);
        Task SetRunInfoAsync(int runId, int templateId, string templateName, string subject, DateTime now);
        Task HeartbeatAsync(int runId, DateTime now);
        Task FinishRunAsync(int runId, string status, string note, DateTime now);

        Task<int> UpsertLinkAsync(int runId, int itemId, byte itemType, string url, string title);
        Task<long> InsertLogAsync(int runId, int portalId, int studentId, string email, string fullName, string token, DateTime now);
        Task SetLogResultAsync(long logId, string status, string error, DateTime now);

        Task TrackOpenAsync(string token, DateTime now);
        Task<string?> TrackClickAsync(string token, int linkId, DateTime now);
    }

    public class BirthdayMailRepository : IBirthdayMailRepository
    {
        private readonly string _connStr;

        public BirthdayMailRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultCRMConnection")
                       ?? throw new InvalidOperationException("DefaultCRMConnection not configured");
        }

        private SqlConnection Open() => new SqlConnection(_connStr);

        private const CommandType Sp = CommandType.StoredProcedure;

        public async Task<IReadOnlyList<BirthdayMailConfigRow>> GetEnabledConfigsAsync()
        {
            using var conn = Open();
            return (await conn.QueryAsync<BirthdayMailConfigRow>("NVCore_BirthdayMail_Config_GetEnabled", commandType: Sp)).ToList();
        }

        public async Task<BirthdayMailConfigRow?> GetConfigAsync(int portalId)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<BirthdayMailConfigRow>("NVCore_BirthdayMail_Config_Get", new { PortalId = portalId }, commandType: Sp);
        }

        public async Task<BirthdayMailTemplateRow?> GetTemplateAsync(int portalId, int templateId)
        {
            if (templateId <= 0) return null;
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<BirthdayMailTemplateRow>("NVCore_BirthdayMail_Template_GetForSend",
                new { PortalId = portalId, TemplateId = templateId }, commandType: Sp);
        }

        public async Task<IReadOnlyList<BirthdayMailItemRow>> GetActiveItemsAsync(int portalId, DateTime date)
        {
            using var conn = Open();
            return (await conn.QueryAsync<BirthdayMailItemRow>("NVCore_BirthdayMail_Item_GetActive",
                new { PortalId = portalId, Date = date.Date }, commandType: Sp)).ToList();
        }

        public async Task<IReadOnlyList<BirthdayRecipientRow>> GetRecipientsAsync(int portalId, DateTime date, int runId, bool onlyConsented, int top)
        {
            using var conn = Open();
            return (await conn.QueryAsync<BirthdayRecipientRow>("NVCore_BirthdayMail_Recipients",
                new { PortalId = portalId, Date = date.Date, RunId = runId, OnlyConsented = onlyConsented, Top = top },
                commandType: Sp, commandTimeout: 180)).ToList();
        }

        public async Task<BirthdayRunBeginRow> BeginRunAsync(int portalId, DateTime runDate, bool manual, string triggeredBy, int staleMinutes, DateTime now)
        {
            using var conn = Open();
            return await conn.QueryFirstAsync<BirthdayRunBeginRow>("NVCore_BirthdayMail_Run_Begin",
                new { PortalId = portalId, RunDate = runDate.Date, IsManual = manual, TriggeredBy = Clip(triggeredBy, 200), StaleMinutes = staleMinutes, Now = now },
                commandType: Sp);
        }

        public async Task<BirthdayRunInfoRow?> GetRunByDateAsync(int portalId, DateTime runDate)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<BirthdayRunInfoRow>("NVCore_BirthdayMail_Run_GetByDate",
                new { PortalId = portalId, RunDate = runDate.Date }, commandType: Sp);
        }

        public async Task SetRunInfoAsync(int runId, int templateId, string templateName, string subject, DateTime now)
        {
            using var conn = Open();
            await conn.ExecuteAsync("NVCore_BirthdayMail_Run_SetInfo",
                new { RunId = runId, TemplateId = templateId, TemplateName = Clip(templateName, 200), Subject = Clip(subject, 250), Now = now }, commandType: Sp);
        }

        public async Task HeartbeatAsync(int runId, DateTime now)
        {
            using var conn = Open();
            await conn.ExecuteAsync("NVCore_BirthdayMail_Run_Heartbeat", new { RunId = runId, Now = now }, commandType: Sp);
        }

        public async Task FinishRunAsync(int runId, string status, string note, DateTime now)
        {
            using var conn = Open();
            await conn.ExecuteAsync("NVCore_BirthdayMail_Run_Finish", new { RunId = runId, Status = status, Note = Clip(note, 500), Now = now }, commandType: Sp);
        }

        public async Task<int> UpsertLinkAsync(int runId, int itemId, byte itemType, string url, string title)
        {
            using var conn = Open();
            return await conn.ExecuteScalarAsync<int>("NVCore_BirthdayMail_Link_Upsert",
                new { RunId = runId, ItemId = itemId, ItemType = itemType, Url = Clip(url, 2000), Title = Clip(title, 300) }, commandType: Sp);
        }

        public async Task<long> InsertLogAsync(int runId, int portalId, int studentId, string email, string fullName, string token, DateTime now)
        {
            using var conn = Open();
            return await conn.ExecuteScalarAsync<long>("NVCore_BirthdayMail_Log_Insert",
                new { RunId = runId, PortalId = portalId, StudentId = studentId, Email = Clip(email, 256), FullName = Clip(fullName, 300), Token = token, Now = now },
                commandType: Sp);
        }

        public async Task SetLogResultAsync(long logId, string status, string error, DateTime now)
        {
            using var conn = Open();
            await conn.ExecuteAsync("NVCore_BirthdayMail_Log_SetResult", new { LogId = logId, Status = status, Error = Clip(error, 1000), Now = now }, commandType: Sp);
        }

        public async Task TrackOpenAsync(string token, DateTime now)
        {
            using var conn = Open();
            await conn.ExecuteAsync("NVCore_BirthdayMail_Track_Open", new { Token = token, Now = now }, commandType: Sp);
        }

        public async Task<string?> TrackClickAsync(string token, int linkId, DateTime now)
        {
            using var conn = Open();
            return await conn.ExecuteScalarAsync<string?>("NVCore_BirthdayMail_Track_Click", new { Token = token, LinkId = linkId, Now = now }, commandType: Sp);
        }

        private static string Clip(string? s, int max)
        {
            s ??= string.Empty;
            return s.Length > max ? s[..max] : s;
        }
    }
}
