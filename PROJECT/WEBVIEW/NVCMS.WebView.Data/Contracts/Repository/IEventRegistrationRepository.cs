using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IEventRegistrationRepository
{
    /// <summary>Find student by normalized phone or email. Returns null if not found.</summary>
    Task<StudentInfoModel?> FindStudentAsync(string? normalizedPhone, string? email);

    /// <summary>
    /// Insert a new Student_Info row and return (newId, newCode).
    /// Uses Student_Info_Insert stored procedure.
    /// </summary>
    Task<(int StudentId, string StudentCode)> InsertStudentAsync(
        StudentInfoModel student, int portalId);

    /// <summary>
    /// Check whether StudentId is already registered for EventId.
    /// </summary>
    Task<bool> RegistrationExistsAsync(int studentId, int eventId, int eventCatId);

    /// <summary>
    /// Atomic: if student does not exist insert it; then insert NV_Events_Student.
    /// Returns (studentId, studentCode, isDuplicate).
    /// </summary>
    Task<(int StudentId, string StudentCode, bool IsDuplicate)> RegisterAsync(
        StudentInfoModel student,
        int eventId,
        int eventCatId,
        int portalId,
        CancellationToken ct = default);
}
