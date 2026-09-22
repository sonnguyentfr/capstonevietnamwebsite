using System.Net;
using Hangfire;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

/// <summary>
/// Thay cho DNN job NVCMS.Modules.Scheduler.CopyDataStudentFromLadiScheduledJob.
///
/// Flow (giữ nguyên nghiệp vụ job VB cũ):
///   1. Duyệt các nhóm sự kiện đang mở (NV_Events_Cat_SelectAllOnline).
///   2. Lấy các bản ghi student_from_ladipage có is_update_crm = 0.
///   3. Với mỗi bản ghi:
///        a. Trùng Email  → dùng Student_Info sẵn có, Source = 0 (StatusCu)
///        b. Trùng SĐT    → dùng Student_Info sẵn có, Source = 0 (StatusCu)
///        c. Chưa có      → Student_Info_Insert + sinh Code + Follow_Log,
///                          Source = 1 (StatusOnline)
///      Nếu đã đăng ký sự kiện rồi thì chỉ update Nguon / NguonTutao.
///   4. Đánh dấu is_update_crm = 1 rồi gửi mail xác nhận.
///
/// Toàn bộ thao tác ghi đều gọi lại đúng Stored Procedure cũ (qua
/// ICrmSyncRepository) nên dữ liệu ghi ra giống hệt job DNN.
///
/// Khác biệt có chủ đích so với job VB: mỗi bản ghi được bọc try/catch riêng.
/// Job cũ chỉ có một try bao cả vòng lặp, nên 1 bản ghi lỗi (ví dụ
/// event_dia_diem_id trỏ tới sự kiện không tồn tại) sẽ chặn toàn bộ các bản
/// ghi còn lại của nhóm sự kiện đó.
/// </summary>
public class CopyStudentFromLadiJob
{
    /// <summary>NVCMS.Modules.EventsWebsite.SuKienZ.StatusCu</summary>
    private const int SourceStatusCu = 0;

    /// <summary>NVCMS.Modules.EventsWebsite.SuKienZ.StatusOnline</summary>
    private const int SourceStatusOnline = 1;

    private const string NguonLadi = "ladi";

    /// <summary>Giá trị email rác cần bỏ qua (job VB check "na" không phân biệt hoa thường).</summary>
    private static readonly string[] InvalidEmails = ["", "na"];

    public const string JobName = nameof(CopyStudentFromLadiJob);

    private readonly ICrmSyncRepository _crm;
    private readonly IEmailService _email;
    private readonly IJobAlertService _alert;
    private readonly CrmSyncSettings _settings;
    private readonly ILogger<CopyStudentFromLadiJob> _logger;

    public CopyStudentFromLadiJob(
        ICrmSyncRepository crm,
        IEmailService email,
        IJobAlertService alert,
        IOptions<CrmSyncSettings> settings,
        ILogger<CopyStudentFromLadiJob> logger)
    {
        _crm = crm;
        _email = email;
        _alert = alert;
        _settings = settings.Value;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 2, DelaysInSeconds = new[] { 120, 600 })]
    [DisableConcurrentExecution(timeoutInSeconds: 1800)]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var failures = new List<JobFailure>();

        try
        {
            await RunAsync(failures, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;   // Hangfire dừng job - không phải lỗi, không cảnh báo
        }
        catch (Exception ex)
        {
            failures.Add(new JobFailure
            {
                Step = "Chạy job CopyStudentFromLadiJob",
                FallbackSource = $"{JobName}.RunAsync",
                Exception = ex
            });

            await _alert.ReportAsync(JobName, failures, aborted: true);
            throw;   // để Hangfire ghi nhận job failed và retry
        }
    }

    private async Task RunAsync(List<JobFailure> failures, CancellationToken cancellationToken)
    {
        var portalId = _settings.PortalId;
        _logger.LogInformation("CopyStudentFromLadiJob bắt đầu (PortalId={PortalId})", portalId);

        var eventCats = await _crm.GetOnlineEventCatsAsync(portalId);
        _logger.LogInformation("Tìm thấy {Count} nhóm sự kiện đang mở", eventCats.Count);

        var processed = 0;
        var created = 0;
        var skippedNoEvent = 0;

        foreach (var cat in eventCats)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var rows = await _crm.GetPendingLadipageRowsAsync(cat.id);
            if (rows.Count == 0)
            {
                _logger.LogInformation("EventCat {Id} ({Name}): không có bản ghi chờ đẩy CRM",
                    cat.id, cat.CatName);
                continue;
            }

            // Tiền tố mã khách hàng: Code nhóm sự kiện + yyMM (ví dụ: ff2609)
            var codePrefix = (cat.Code ?? string.Empty) + DateTime.Now.ToString("yMM");

            _logger.LogInformation("EventCat {Id} ({Name}): {Count} bản ghi chờ đẩy CRM",
                cat.id, cat.CatName, rows.Count);

            foreach (var row in rows)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (_settings.MaxRecordsPerRun > 0 && processed >= _settings.MaxRecordsPerRun)
                {
                    _logger.LogWarning(
                        "Đã đạt giới hạn {Max} bản ghi/lần chạy, dừng sớm. Phần còn lại sẽ xử lý ở lần chạy sau.",
                        _settings.MaxRecordsPerRun);
                    goto done;
                }

                try
                {
                    var outcome = await ProcessRowAsync(cat, row, codePrefix, portalId, cancellationToken);

                    switch (outcome)
                    {
                        case RowOutcome.CreatedNew:
                            processed++;
                            created++;
                            break;

                        case RowOutcome.LinkedExisting:
                            processed++;
                            break;

                        case RowOutcome.SkippedNoEvent:
                            // Lỗi dữ liệu chứ không phải lỗi code: bản ghi không trỏ tới
                            // địa điểm nào hợp lệ. Không đánh dấu is_update_crm để khi
                            // marketing sửa lại event_dia_diem_id thì lần chạy sau xử lý được.
                            skippedNoEvent++;
                            _logger.LogWarning(
                                "Ladipage id={LadipageId} (EventCat {CatId}): event_dia_diem_id={EventId} không tồn tại trong NV_Events (PortalId={PortalId}) → bỏ qua",
                                row.id, cat.id, row.event_dia_diem_id, portalId);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Không đánh dấu is_update_crm → bản ghi sẽ được thử lại lần chạy sau
                    failures.Add(new JobFailure
                    {
                        Step = $"Đẩy bản ghi ladipage #{row.id} sang CRM",
                        FallbackSource = $"{JobName}.ProcessRowAsync",
                        Context = new Dictionary<string, string?>
                        {
                            ["LadipageId"] = row.id.ToString(),
                            ["EventCatId"] = cat.id.ToString(),
                            ["Tên nhóm sự kiện"] = cat.CatName,
                            ["EventId (địa điểm)"] = row.event_dia_diem_id?.ToString(),
                            ["Họ tên"] = $"{row.hotendem} {row.ten}".Trim(),
                            ["Email"] = row.email,
                            ["Số điện thoại"] = row.so_dien_thoai
                        },
                        Exception = ex
                    });

                    _logger.LogError(ex,
                        "Ladipage id={LadipageId} (EventCat {CatId}) - lỗi đẩy CRM, sẽ thử lại lần sau",
                        row.id, cat.id);
                }
            }
        }

    done:
        var summary =
            $"xử lý {processed} bản ghi ({created} khách mới), " +
            $"{skippedNoEvent} bỏ qua do thiếu địa điểm, {failures.Count} lỗi";

        _logger.LogInformation("CopyStudentFromLadiJob xong: {Summary}", summary);

        await _alert.ReportAsync(JobName, failures, aborted: false, summary);
    }

    private enum RowOutcome
    {
        /// <summary>Đã tạo Student_Info mới.</summary>
        CreatedNew,

        /// <summary>Khách đã có sẵn, chỉ gắn vào sự kiện.</summary>
        LinkedExisting,

        /// <summary>Bản ghi không trỏ tới NV_Events hợp lệ → để lại cho lần sau.</summary>
        SkippedNoEvent
    }

    /// <summary>Xử lý 1 bản ghi ladipage.</summary>
    private async Task<RowOutcome> ProcessRowAsync(
        EventCatRow cat,
        LadipageRow row,
        string codePrefix,
        int portalId,
        CancellationToken cancellationToken)
    {
        // event_dia_diem_id = id của địa điểm/sự kiện con trong NV_Events
        var eventId = row.event_dia_diem_id ?? 0;
        var evt = eventId > 0 ? await _crm.GetEventByIdAsync(eventId, portalId) : null;

        if (evt == null)
        {
            // Job VB cũ ném NullReferenceException ở đây (đọc objEvent.fromdatetime)
            // và mất luôn các bản ghi còn lại của nhóm sự kiện. Ở đây chỉ bỏ qua
            // đúng bản ghi này, các bản ghi hợp lệ khác vẫn chạy tiếp.
            return RowOutcome.SkippedNoEvent;
        }

        var now = DateTime.Now;
        var isNewStudent = false;

        int studentId;
        string? firstName;
        string? lastName;
        string? toEmail;
        string studentCode;

        var existingByEmail = await _crm.GetStudentByEmailAsync(row.email ?? string.Empty);

        if (existingByEmail != null)
        {
            // ── a. Đã có khách hàng trùng Email ──────────────────────────────
            studentId = existingByEmail.id;
            firstName = existingByEmail.Hotendem;
            lastName = existingByEmail.Ten;
            toEmail = existingByEmail.Email;
            studentCode = ResolveStudentCode(existingByEmail, codePrefix);

            await LinkStudentToEventAsync(row, cat, studentId, studentCode, eventId, portalId, now);
        }
        else
        {
            var existingByPhone = await _crm.GetStudentByPhoneAsync(row.so_dien_thoai ?? string.Empty);

            if (existingByPhone != null)
            {
                // ── b. Đã có khách hàng trùng số điện thoại ──────────────────
                studentId = existingByPhone.id;
                firstName = existingByPhone.Hotendem;
                lastName = existingByPhone.Ten;
                toEmail = existingByPhone.Email;
                studentCode = ResolveStudentCode(existingByPhone, codePrefix);

                await LinkStudentToEventAsync(row, cat, studentId, studentCode, eventId, portalId, now);
            }
            else
            {
                // ── c. Khách hàng mới ────────────────────────────────────────
                isNewStudent = true;
                firstName = row.hotendem;
                lastName = row.ten;
                toEmail = row.email;

                studentId = await _crm.InsertStudentAsync(BuildStudentArgs(cat, row, evt, portalId, now));
                if (studentId <= 0)
                    throw new InvalidOperationException(
                        $"Student_Info_Insert không trả về Id cho ladipage id={row.id}");

                studentCode = codePrefix + studentId;
                await _crm.UpdateStudentCodeAsync(studentId, studentCode);

                await _crm.InsertFollowLogAsync(
                    studentId,
                    $"KHÁCH HÀNG: [{firstName} {lastName}] - ĐĂNG KÝ TẠI {cat.CatName}",
                    now,
                    portalId);

                // Khách mới → Source = StatusOnline (1), nguontutao = "ladi,"
                await _crm.InsertEventStudentAsync(
                    eventId, cat.id, studentId, studentCode,
                    SourceStatusOnline, NguonLadi, now, portalId, NguonLadi + ",");

                await _crm.UpdateEventStudentNguonAsync(eventId, studentId, NguonLadi);
                await _crm.UpdateEventStudentNguonTutaoAsync(eventId, studentId, row.link + ",");
            }
        }

        // Đánh dấu đã đẩy CRM (kể cả khi gửi mail lỗi - giống job VB)
        await _crm.MarkLadipageSyncedAsync(row.id);

        await SendConfirmationMailAsync(
            cat, evt, firstName, lastName, toEmail, studentCode, cancellationToken);

        return isNewStudent ? RowOutcome.CreatedNew : RowOutcome.LinkedExisting;
    }

    /// <summary>
    /// Gắn khách hàng đã tồn tại vào sự kiện: nếu đã đăng ký thì chỉ cập nhật
    /// nguồn, chưa đăng ký thì insert mới với Source = StatusCu (0).
    /// </summary>
    private async Task LinkStudentToEventAsync(
        LadipageRow row, EventCatRow cat, int studentId, string studentCode,
        int eventId, int portalId, DateTime now)
    {
        if (await _crm.EventStudentExistsAsync(eventId, studentId))
        {
            await _crm.UpdateEventStudentNguonAsync(eventId, studentId, NguonLadi);
            await _crm.UpdateEventStudentNguonTutaoAsync(eventId, studentId, row.link + ",");
        }
        else
        {
            // Job VB truyền row.event_id (= EventCatId) ở nhánh khách cũ
            await _crm.InsertEventStudentAsync(
                eventId, row.event_id ?? cat.id, studentId, studentCode,
                SourceStatusCu, NguonLadi, now, portalId, row.link ?? string.Empty);
        }
    }

    private StudentInsertArgs BuildStudentArgs(
        EventCatRow cat, LadipageRow row, EventRow evt, int portalId, DateTime now)
    {
        return new StudentInsertArgs
        {
            VP = evt.Vanphong ?? 0,
            Type = 1,
            Hotendem = row.hotendem,
            Ten = row.ten,
            Sex = row.gioi_tinh ?? false,
            Ngaysinh = SafeSqlDate(row.ngay_sinh) ?? new DateTime(1970, 1, 1),
            Sodienthoai = NormalizeVnPhone(row.so_dien_thoai),
            Email = row.email,
            HocVanTruongdanghoc = row.truong_dang_hoc,

            FollowPhuongThuc = 15,
            FollowUpStatus = 1,
            FollowUpDateUpdate = now,

            // Job VB gán thong_tin_khac rồi ghi đè ngay bằng câu mô tả dưới đây
            TuVanKhac = $"<mark>Khách đăng ký Online tại: {cat.CatName}</mark>.",
            TuVanHocVanmongmuon = string.Empty,
            TuVanNamdi = string.Empty,
            TuVanKyhoc = "0",
            TuVanNganhhoc = string.Empty,
            TuVanTruongdukien = string.Empty,
            TuVanQuocgia = "0",
            TuVanEditDate = now,
            TuVanApproveDate = now,

            HocVanEditDate = now,
            HocVanApproveDate = now,

            CreatedDate = SafeSqlDate(row.created_date) ?? now,
            UserId = 1,
            PortalId = portalId
        };
    }

    // ── Gửi mail xác nhận ────────────────────────────────────────────────────

    private async Task SendConfirmationMailAsync(
        EventCatRow cat, EventRow evt,
        string? firstName, string? lastName, string? toEmail, string studentCode,
        CancellationToken cancellationToken)
    {
        var subject = $"Thư xác nhận đăng ký thành công: {cat.CatName}";

        var body = EventRegistrationMailTemplate.BuildCustomerBody(
            isSendCode: cat.sendcode ?? false,
            titleMail: cat.titleMail,
            firstName: firstName,
            lastName: lastName,
            catName: cat.CatName,
            thoiGianTu: FormatEventTime(evt.fromdatetime),
            thoiGianDen: FormatEventTime(evt.enddatetime),
            diaDiem: evt.diadiem,
            contentMail: WebUtility.HtmlDecode(cat.ContentMail ?? string.Empty),
            urlDomain: _settings.UrlDomain,
            studentCode: studentCode);

        string recipient;
        string? cc;

        if (cat.sendmail == true)
        {
            // Gửi cho khách, CC email nhóm sự kiện
            if (IsInvalidEmail(toEmail))
            {
                _logger.LogInformation(
                    "Bỏ qua gửi mail: email khách không hợp lệ ({Email}), mã {Code}",
                    toEmail, studentCode);
                return;
            }

            recipient = toEmail!;
            cc = cat.Email;
        }
        else
        {
            // Nhóm sự kiện tắt gửi mail khách → chỉ báo nội bộ về email nhóm
            if (IsInvalidEmail(cat.Email))
            {
                _logger.LogWarning(
                    "EventCat {Id} tắt sendmail nhưng không có email nhóm → không gửi được thông báo",
                    cat.id);
                return;
            }

            recipient = cat.Email!;
            cc = null;
        }

        await _email.SendEmailAsync(
            _settings.FromEmail,
            _settings.FromName,
            recipient,
            subject,
            body,
            cc,
            _settings.BccEmails);

        if (_settings.SendMailThrottleMs > 0)
            await Task.Delay(_settings.SendMailThrottleMs, cancellationToken);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Mã khách hàng dùng cho QR/barcode trong mail.
    ///
    /// KHÁC job VB: job cũ luôn dựng mã bằng (Code nhóm sự kiện + yyMM hiện tại + StudentId)
    /// kể cả với khách đã có sẵn trong CRM. Với khách đăng ký từ tháng trước,
    /// mã đó KHÔNG khớp Student_Info.Code thật (vd. thật là ff2607150998 nhưng
    /// mail in ra ff2609150998) nên QR không check-in được.
    /// Ở đây ưu tiên Code thật; chỉ khi Code trống mới quay về cách dựng cũ.
    /// </summary>
    private static string ResolveStudentCode(StudentInfoRow student, string codePrefix)
        => string.IsNullOrWhiteSpace(student.Code)
            ? codePrefix + student.id
            : student.Code.Trim();

    private static bool IsInvalidEmail(string? email)
        => string.IsNullOrWhiteSpace(email)
           || InvalidEmails.Contains(email.Trim(), StringComparer.OrdinalIgnoreCase);

    private static string FormatEventTime(DateTime? value)
        => value?.ToString("dd/MM/yyyy HH:mm") ?? string.Empty;

    /// <summary>
    /// Chặn giá trị nằm ngoài dải của kiểu SQL datetime (trước 01/01/1753),
    /// vì truyền thẳng sẽ ném SqlDateTime overflow.
    /// </summary>
    private static DateTime? SafeSqlDate(DateTime? value)
        => value.HasValue && value.Value >= new DateTime(1753, 1, 1) ? value : null;

    /// <summary>Bản port của NormalizeVnPhone trong job VB.</summary>
    internal static string NormalizeVnPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return string.Empty;

        var p = phone.Trim()
                     .Replace(" ", string.Empty)
                     .Replace(".", string.Empty)
                     .Replace("-", string.Empty);

        if (p.StartsWith("+84")) return "84" + p[3..];
        if (p.StartsWith("84")) return p;
        if (p.StartsWith("0")) return "84" + p[1..];
        if (p.Length == 9) return "84" + p;

        return p;
    }
}
