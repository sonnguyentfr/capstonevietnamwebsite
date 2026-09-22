using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    /// <summary>
    /// Truy cập CRM qua đúng bộ Stored Procedure mà NVCMS.Modules.Scheduler (VB)
    /// đang dùng, để hành vi của job Hangfire giống hệt job DNN cũ.
    /// </summary>
    public interface ICrmSyncRepository
    {
        // ── NV_Events_Cat / NV_Events ────────────────────────────────────────
        Task<IReadOnlyList<EventCatRow>> GetOnlineEventCatsAsync(int portalId);

        Task<EventRow?> GetEventByIdAsync(int eventId, int portalId);

        // ── student_from_ladipage ────────────────────────────────────────────
        Task<IReadOnlyList<LadipageRow>> GetPendingLadipageRowsAsync(int eventCatId);

        Task MarkLadipageSyncedAsync(int ladipageId);

        // ── Student_Info ─────────────────────────────────────────────────────
        Task<StudentInfoRow?> GetStudentByEmailAsync(string email);

        Task<StudentInfoRow?> GetStudentByPhoneAsync(string phone);

        Task<int> InsertStudentAsync(StudentInsertArgs args);

        Task UpdateStudentCodeAsync(int studentId, string code);

        Task InsertFollowLogAsync(int studentId, string content, DateTime createdDate, int portalId);

        // ── NV_Events_Student ────────────────────────────────────────────────
        Task<bool> EventStudentExistsAsync(int eventId, int studentId);

        Task InsertEventStudentAsync(
            int eventId, int eventCatId, int studentId, string studentCode,
            int source, string nguon, DateTime createdDate, int portalId, string nguonTutao);

        Task UpdateEventStudentNguonAsync(int eventId, int studentId, string nguon);

        Task UpdateEventStudentNguonTutaoAsync(int eventId, int studentId, string nguonTutao);
    }
}
