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
        StudentInfoModel student, int portalId)
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

        // Generate code: C + zero-padded id
        var code = "C" + newId.ToString("D6");

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

    // ── Atomic register ───────────────────────────────────────────────────────

    public async Task<(int StudentId, string StudentCode, bool IsDuplicate)> RegisterAsync(
        StudentInfoModel student,
        int eventId,
        int eventCatId,
        int portalId,
        CancellationToken ct = default)
    {
        await using var conn = CreateConn();
        await conn.OpenAsync(ct);
        await using var tx = await conn.BeginTransactionAsync(ct);
        try
        {
            // 1. Resolve or create student
            int    studentId;
            string studentCode;

            if (student.Id > 0)
            {
                studentId   = student.Id;
                studentCode = student.Code ?? "C" + student.Id.ToString("D6");
            }
            else
            {
                // Insert into Student_Info
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
                    transaction: tx,
                    commandType: CommandType.StoredProcedure);

                var code = "C" + newId.ToString("D6");
                await conn.ExecuteAsync(
                    "Student_Info_InsertCode",
                    new { id = newId, Code = code },
                    transaction: tx,
                    commandType: CommandType.StoredProcedure);

                studentId   = newId;
                studentCode = code;
            }

            // 2. Duplicate check
            var exists = await conn.ExecuteScalarAsync<int>(
                @"SELECT COUNT(1) FROM NV_Events_Student
                  WHERE StudentId=@s AND EventId=@e AND EventCatId=@c",
                new { s = studentId, e = eventId, c = eventCatId },
                transaction: tx);

            if (exists > 0)
            {
                await tx.RollbackAsync(ct);
                return (studentId, studentCode, IsDuplicate: true);
            }

            // 3. Insert registration using existing SP
            await conn.ExecuteAsync(
                "NV_Events_Student_Insert",
                new
                {
                    EventId     = eventId,
                    EventCatId  = eventCatId,
                    StudentId   = studentId,
                    StudentCode = studentCode,
                    Source      = 8,          // 8 = Website
                    Nguon       = "WEBSITE",
                    CreatedDate = DateTime.Now,
                    PortalId    = portalId,
                    Nguontutao  = "WEBSITE"
                },
                transaction: tx,
                commandType: CommandType.StoredProcedure);

            await tx.CommitAsync(ct);
            return (studentId, studentCode, IsDuplicate: false);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}
