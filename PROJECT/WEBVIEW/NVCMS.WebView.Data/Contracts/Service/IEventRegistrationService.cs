using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface IEventRegistrationService
{
    /// <summary>
    /// Lookup student by phone or email for real-time form autofill.
    /// </summary>
    Task<CheckStudentResult> CheckStudentAsync(string? phone, string? email);

    /// <summary>
    /// Register a student for a specific event location.
    /// Returns success/duplicate/error status and a message.
    /// </summary>
    Task<(bool Success, bool IsDuplicate, string Message, int StudentId, string StudentCode)>
        RegisterAsync(EventRegistrationInputViewModel input, int portalId, CancellationToken ct = default);
}
