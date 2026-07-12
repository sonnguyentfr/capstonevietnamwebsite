using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;
using Capstone.View.Helpers;
namespace NVCMS.WebView.Data.Service;

public class EventRegistrationService : IEventRegistrationService
{
    private readonly IEventRegistrationRepository _repo;
    private readonly IEventsRepository _eventsRepo;

    public EventRegistrationService(
        IEventRegistrationRepository repo,
        IEventsRepository eventsRepo)
    {
        _repo = repo;
        _eventsRepo = eventsRepo;
    }

    public async Task<CheckStudentResult> CheckStudentAsync(string? phone, string? email)
    {
        var normalized = PhoneHelper.Normalize(phone);

        var student = await _repo.FindStudentAsync(
            string.IsNullOrWhiteSpace(normalized) ? null : normalized,
            string.IsNullOrWhiteSpace(email) ? null : email?.Trim());

        if (student is null)
            return new CheckStudentResult { Found = false };

        return new CheckStudentResult
        {
            Found = true,
            StudentId = student.Id,
            StudentCode = student.Code ?? string.Empty,
            Hotendem = student.Hotendem ?? string.Empty,
            Ten = student.Ten ?? string.Empty,
            FullName = student.FullName,
            Phone = student.Sodienthoai ?? string.Empty,
            Email = student.Email ?? string.Empty,
            DiaChi = student.Diachi ?? string.Empty,
        };
    }

    public async Task<(bool Success, bool IsDuplicate, string Message, int StudentId, string StudentCode)>
        RegisterAsync(EventRegistrationInputViewModel input, int portalId, CancellationToken ct = default)
    {


        input.Hotendem = InputCleaner.Name(input.Hotendem);
        input.Ten = InputCleaner.Name(input.Ten);
        input.TinhThanh = InputCleaner.Text(input.TinhThanh);
        input.Email = InputCleaner.Email(input.Email);
        // ── Validate phone ────────────────────────────────────────────────────
        var normalizedPhone = PhoneHelper.Normalize(input.SoDienThoai);
        if (!PhoneHelper.IsValid(normalizedPhone))
            return (false, false, "Số điện thoại không hợp lệ.", 0, string.Empty);

        // ── Validate email if provided ────────────────────────────────────────
        if (!string.IsNullOrWhiteSpace(input.Email) && !EmailHelper.IsValid(input.Email))
            return (false, false, "Email không hợp lệ.", 0, string.Empty);

        // ── Validate event is still active ───────────────────────────────────
        var cat = await _eventsRepo.GetCatByIdAsync(input.EventCatId);
        if (cat is null)
            return (false, false, "Sự kiện không tồn tại.", 0, string.Empty);

        var now = DateTime.Now;
        //if (cat.FromDate.HasValue && now < cat.FromDate.Value.Date)
        //    return (false, false, "Sự kiện chưa mở đăng ký.", 0, string.Empty);
        if (cat.EndDate.HasValue && now > cat.EndDate.Value)
            return (false, false, "Sự kiện đã kết thúc đăng ký.", 0, string.Empty);

        // ── Lookup existing student ───────────────────────────────────────────
        var existing = await _repo.FindStudentAsync(normalizedPhone, input.Email?.Trim());

        var student = existing ?? new StudentInfoModel
        {
            Id = 0,
            Hotendem = input.Hotendem.Trim(),
            Ten = input.Ten.Trim(),
            Sodienthoai = normalizedPhone,
            Email = input.Email?.Trim(),
            Diachi = input.TinhThanh?.Trim(),
        };

        // ── Atomic register ───────────────────────────────────────────────────
        var (studentId, studentCode, isDuplicate) = await _repo.RegisterAsync(
            student, input.EventId, input.EventCatId, portalId, ct);

        if (isDuplicate)
            return (false, true, "Bạn đã đăng ký địa điểm này rồi.", studentId, studentCode);

        return (true, false,
            "Đăng ký thành công. Email xác nhận sẽ được gửi trong ít phút.",
            studentId, studentCode);
    }
}
