using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    /// <inheritdoc />
    public class CrmSyncRepository : ICrmSyncRepository
    {
        private readonly string _connStr;

        public CrmSyncRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultCRMConnection")
                       ?? throw new InvalidOperationException("DefaultCRMConnection not configured");
        }

        private SqlConnection Open() => new SqlConnection(_connStr);

        // ── NV_Events_Cat / NV_Events ────────────────────────────────────────

        public async Task<IReadOnlyList<EventCatRow>> GetOnlineEventCatsAsync(int portalId)
        {
            using var conn = Open();
            var rows = await conn.QueryAsync<EventCatRow>(
                "NV_Events_Cat_SelectAllOnline",
                new { Portalid = portalId },
                commandType: CommandType.StoredProcedure);
            return rows.ToList();
        }

        public async Task<EventRow?> GetEventByIdAsync(int eventId, int portalId)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<EventRow>(
                "NV_Events_SelectByID",
                new { id = eventId, Portalid = portalId },
                commandType: CommandType.StoredProcedure);
        }

        // ── student_from_ladipage ────────────────────────────────────────────

        public async Task<IReadOnlyList<LadipageRow>> GetPendingLadipageRowsAsync(int eventCatId)
        {
            using var conn = Open();
            var rows = await conn.QueryAsync<LadipageRow>(
                "sp_student_from_ladipage_select_by_event_id",
                new { event_id = eventCatId, is_update_crm = false },
                commandType: CommandType.StoredProcedure);
            return rows.ToList();
        }

        public async Task MarkLadipageSyncedAsync(int ladipageId)
        {
            using var conn = Open();
            await conn.ExecuteAsync(
                "sp_student_from_ladipage_update_crm",
                new { Id = ladipageId },
                commandType: CommandType.StoredProcedure);
        }

        // ── Student_Info ─────────────────────────────────────────────────────

        public async Task<StudentInfoRow?> GetStudentByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<StudentInfoRow>(
                "Student_Info_SelectByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<StudentInfoRow?> GetStudentByPhoneAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;

            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<StudentInfoRow>(
                "Student_Info_SelectBySodienthoai",
                new { sodienthoai = phone },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertStudentAsync(StudentInsertArgs a)
        {
            using var conn = Open();

            // SP kết thúc bằng SELECT SCOPE_IDENTITY() → kiểu decimal
            var newId = await conn.ExecuteScalarAsync<decimal?>(
                "Student_Info_Insert",
                new
                {
                    a.VP,
                    a.Type,
                    a.Hotendem,
                    a.Ten,
                    a.Sex,
                    a.Ngaysinh,
                    a.Kieungaysinh,
                    a.Sodienthoai,
                    a.Email,
                    a.Diachi,
                    a.Tinh,
                    a.Huyen,
                    a.EB5,
                    a.PermissionUser,
                    a.FollowPhuongThuc,
                    a.FollowKetQua,
                    a.FollowNoiDung,
                    a.FollowUpStatus,
                    a.FollowUpDateUpdate,
                    a.TuVanHocVanmongmuon,
                    a.TuVanNamdi,
                    a.TuVanKyhoc,
                    a.TuVanNganhhoc,
                    a.TuVanTruongdukien,
                    a.TuVanQuocgia,
                    a.TuVanDiadiem,
                    a.TuVanKhanangchitra,
                    a.TuVanKhac,
                    a.TuVanEditUserId,
                    a.TuVanEditDate,
                    a.TuVanApproveUserId,
                    a.TuVanApproveDate,
                    a.HocVanDanghoc,
                    a.HocVanTruongdanghoc,
                    a.HocVanDiemtrungbinh,
                    a.HocVanDiemsobaithichuanhoa,
                    a.HocVanLuuy,
                    a.HocVanEditUserId,
                    a.HocVanEditDate,
                    a.HocVanApproveUserId,
                    a.HocVanApproveDate,
                    a.CreatedDate,
                    a.UserId,
                    a.PortalId,
                    a.Xoa
                },
                commandType: CommandType.StoredProcedure);

            return newId.HasValue ? Convert.ToInt32(newId.Value) : 0;
        }

        public async Task UpdateStudentCodeAsync(int studentId, string code)
        {
            using var conn = Open();
            await conn.ExecuteAsync(
                "Student_Info_InsertCode",
                new { id = studentId, CODE = code },
                commandType: CommandType.StoredProcedure);
        }

        public async Task InsertFollowLogAsync(int studentId, string content, DateTime createdDate, int portalId)
        {
            using var conn = Open();
            await conn.ExecuteAsync(
                "Student_Follow_Log_Insert",
                new
                {
                    StudentId = studentId,
                    Noidung = content,
                    CreatedDate = createdDate,
                    PortalId = portalId
                },
                commandType: CommandType.StoredProcedure);
        }

        // ── NV_Events_Student ────────────────────────────────────────────────

        public async Task<bool> EventStudentExistsAsync(int eventId, int studentId)
        {
            using var conn = Open();
            // SP trả về SELECT * → dùng dynamic để không phụ thuộc thứ tự cột
            var row = await conn.QueryFirstOrDefaultAsync(
                "NV_Events_Student_SelectByEventstudentid",
                new { EventId = eventId, StudentId = studentId },
                commandType: CommandType.StoredProcedure);
            return row != null;
        }

        public async Task InsertEventStudentAsync(
            int eventId, int eventCatId, int studentId, string studentCode,
            int source, string nguon, DateTime createdDate, int portalId, string nguonTutao)
        {
            using var conn = Open();
            await conn.ExecuteAsync(
                "NV_Events_Student_Insert",
                new
                {
                    EventId = eventId,
                    EventCatId = eventCatId,
                    StudentId = studentId,
                    StudentCode = studentCode,
                    Source = source,
                    Nguon = nguon,
                    CreatedDate = createdDate,
                    PortalId = portalId,
                    nguontutao = nguonTutao
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateEventStudentNguonAsync(int eventId, int studentId, string nguon)
        {
            using var conn = Open();
            await conn.ExecuteAsync(
                "NV_Events_Student_UpdateStudentNguon",
                new { EventId = eventId, StudentId = studentId, Nguon = nguon },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateEventStudentNguonTutaoAsync(int eventId, int studentId, string nguonTutao)
        {
            using var conn = Open();
            await conn.ExecuteAsync(
                "NV_Events_Student_UpdateStudentNguonTutao",
                new { EventId = eventId, StudentId = studentId, Nguontutao = nguonTutao },
                commandType: CommandType.StoredProcedure);
        }
    }
}
