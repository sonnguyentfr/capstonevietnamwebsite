using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Repository;

public class EventRegistrationRepository : IEventRegistrationRepository
{
    private readonly string _connectionString;

    public EventRegistrationRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    // ── Student lookup ────────────────────────────────────────────────────────

    public async Task<StudentInfoModel?> FindStudentAsync(string? normalizedPhone, string? email)
    {
        // Try phone first (more unique), fall back to email
        if (!string.IsNullOrWhiteSpace(normalizedPhone))
        {
            const string sql = @"
                SELECT TOP 1
                    id         AS Id,
                    CODE       AS Code,
                    Hotendem,
                    Ten,
                    Sodienthoai,
                    Email,
                    Diachi,
                    Portalid   AS PortalId
                FROM Student_Info
                WHERE Sodienthoai = @phone AND (Xoa IS NULL OR Xoa = 0)";

            await using var conn = CreateConn();
            var row = await conn.QueryFirstOrDefaultAsync<StudentInfoModel>(
                sql, new { phone = normalizedPhone });
            if (row is not null) return row;
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            const string sql = @"
                SELECT TOP 1
                    id         AS Id,
                    CODE       AS Code,
                    Hotendem,
                    Ten,
                    Sodienthoai,
                    Email,
                    Diachi,
                    Portalid   AS PortalId
                FROM Student_Info
                WHERE Email = @email AND (Xoa IS NULL OR Xoa = 0)";

            await using var conn = CreateConn();
            return await conn.QueryFirstOrDefaultAsync<StudentInfoModel>(
                sql, new { email });
        }

        return null;
    }

    // ── Student insert ────────────────────────────────────────────────────────

    public async Task<(int StudentId, string StudentCode)> InsertStudentAsync(
        StudentInfoModel student, int portalId, int eventCatId = 0)
    {
        await using var conn = CreateConn();
        // Student_Info_Insert SP returns the new ID as scalar
        var newId = await conn.ExecuteScalarAsync<int>(
            "Student_Info_Insert",
            new
            {
                VP               = 0,
                Type             = 0,
                Hotendem         = student.Hotendem ?? string.Empty,
                Ten              = student.Ten ?? string.Empty,
                Sex              = false,
                Ngaysinh         = (DateTime?)null,
                KieuNgaysinh     = 0,
                Sodienthoai      = student.Sodienthoai ?? string.Empty,
                Email            = student.Email ?? string.Empty,
                Diachi           = student.Diachi ?? string.Empty,
                Tinh             = 0,
                Huyen            = 0,
                EB5              = false,
                PermissionUser   = string.Empty,
                FollowPhuongThuc = 0,
                FollowKetQua     = 0,
                FollowNoiDung    = (string?)null,
                FollowUpStatus   = 0,
                FollowUpDateUpdate = DateTime.Now,
                TuVanHocVanmongmuon  = (string?)null,
                TuVanNamdi           = (string?)null,
                TuVanKyhoc           = (string?)null,
                TuVanNganhhoc        = (string?)null,
                TuVanTruongdukien    = (string?)null,
                TuVanQuocgia         = (string?)null,
                TuVanDiadiem         = 0,
                TuVanKhanangchitra   = 0,
                TuVanKhac            = (string?)null,
                TuVanEditUserId      = 0,
                TuVanEditDate        = DateTime.Now,
                TuVanApproveUserId   = 0,
                TuVanApproveDate     = DateTime.Now,
                HocVanDanghoc            = (string?)null,
                HocVanTruongdanghoc      = (string?)null,
                HocVanDiemtrungbinh      = (string?)null,
                HocVanDiemsobaithichuanhoa = (string?)null,
                HocVanLuuy               = (string?)null,
                HocVanEditUserId         = 0,
                HocVanEditDate           = DateTime.Now,
                HocVanApproveUserId      = 0,
                HocVanApproveDate        = DateTime.Now,
                CreatedDate = DateTime.Now,
                UserId      = 0,
                PortalId    = portalId,
                Xoa         = false
            },
            commandType: CommandType.StoredProcedure);

        // Generate code: {CatCode}{YY}{MM}{StudentId}
        string catCode = string.Empty;
        if (eventCatId > 0)
        {
            catCode = await conn.ExecuteScalarAsync<string>(
                "SELECT ISNULL(Code, '') FROM NV_Events_Cat WHERE id = @eventCatId",
                new { eventCatId }) ?? string.Empty;
        }
        var now = DateTime.Now;
        var code = catCode
            + now.ToString("yy")
            + now.ToString("MM")
            + newId.ToString();

        // Persist code back using Student_Info_InsertCode SP
        await conn.ExecuteAsync(
            "Student_Info_InsertCode",
            new { id = newId, Code = code },
            commandType: CommandType.StoredProcedure);

        return (newId, code);
    }

    // ── Registration exists check ─────────────────────────────────────────────

    public async Task<bool> RegistrationExistsAsync(int studentId, int eventId, int eventCatId)
    {
        const string sql = @"
            SELECT COUNT(1)
            FROM NV_Events_Student
            WHERE StudentId = @studentId
              AND EventId    = @eventId
              AND EventCatId = @eventCatId";

        await using var conn = CreateConn();
        var count = await conn.ExecuteScalarAsync<int>(
            sql, new { studentId, eventId, eventCatId });
        return count > 0;
    }

    // ── Atomic register via WebView_EventRegistration_Upsert ─────────────────

    public async Task<(int StudentId, string StudentCode, bool IsDuplicate)> RegisterAsync(
        StudentInfoModel student,
        int eventId,
        int eventCatId,
        int portalId,
        CancellationToken ct = default)
    {
        var p = new DynamicParameters();
        p.Add("Hotendem",    student.Hotendem     ?? string.Empty);
        p.Add("Ten",         student.Ten           ?? string.Empty);
        p.Add("Sodienthoai", student.Sodienthoai  ?? string.Empty);
        p.Add("Email",       student.Email         ?? string.Empty);
        p.Add("Diachi",      student.Diachi        ?? string.Empty);
        p.Add("Ngaysinh",    student.Ngaysinh,      dbType: DbType.DateTime);
        p.Add("TinhId",      student.Tinh,          dbType: DbType.Int32);
        p.Add("PortalId",    portalId);
        p.Add("EventId",     eventId);
        p.Add("EventCatId",  eventCatId);
        p.Add("StudentId",   dbType: DbType.Int32,  direction: ParameterDirection.Output);
        p.Add("StudentCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
        p.Add("IsDuplicate", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        await using var conn = CreateConn();
        await conn.ExecuteAsync(
            "WebView_EventRegistration_Upsert",
            p,
            commandType: CommandType.StoredProcedure);

        var studentId   = p.Get<int>("StudentId");
        var studentCode = p.Get<string>("StudentCode") ?? string.Empty;
        var isDuplicate = p.Get<bool>("IsDuplicate");

        return (studentId, studentCode, isDuplicate);
    }
}
