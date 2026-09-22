using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Models.Config;

namespace NVCMS.API.ReadGoogleSheet.Services;

/// <inheritdoc />
public class JobAlertService : IJobAlertService
{
    /// <summary>Chỉ những frame thuộc code của mình mới dùng để xác định vị trí lỗi.</summary>
    private const string OwnNamespacePrefix = "NVCMS.API.ReadGoogleSheet";

    private readonly IEmailService _email;
    private readonly CrmSyncSettings _settings;
    private readonly ILogger<JobAlertService> _logger;

    public JobAlertService(
        IEmailService email,
        IOptions<CrmSyncSettings> settings,
        ILogger<JobAlertService> logger)
    {
        _email = email;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task ReportAsync(
        string jobName,
        IReadOnlyList<JobFailure> failures,
        bool aborted,
        string? summary = null)
    {
        if (failures.Count == 0)
            return;

        if (!_settings.AlertEnabled)
        {
            _logger.LogWarning(
                "Job {JobName} có {Count} lỗi nhưng CrmSync:AlertEnabled = false → không gửi mail",
                jobName, failures.Count);
            return;
        }

        var recipients = _settings.AlertEmails;
        if (string.IsNullOrWhiteSpace(recipients))
        {
            _logger.LogWarning(
                "Job {JobName} có {Count} lỗi nhưng CrmSync:AlertEmails đang trống → không gửi mail",
                jobName, failures.Count);
            return;
        }

        try
        {
            var subject = BuildSubject(jobName, failures, aborted);
            var body = BuildBody(jobName, failures, aborted, summary);

            // Truyền đủ cc/bcc để chọn đúng overload (fromEmail, fromName, toEmail, ...)
            await _email.SendEmailAsync(
                _settings.FromEmail,
                _settings.FromName,
                recipients,
                subject,
                body,
                ccEmail: null,
                bccEmail: null);

            _logger.LogInformation(
                "Đã gửi mail cảnh báo lỗi job {JobName} tới {Recipients} ({Count} lỗi)",
                jobName, recipients, failures.Count);
        }
        catch (Exception ex)
        {
            // Không để lỗi gửi mail che mất lỗi gốc của job
            _logger.LogError(ex,
                "Không gửi được mail cảnh báo cho job {JobName} ({Count} lỗi)",
                jobName, failures.Count);
        }
    }

    // ── Tiêu đề ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Ví dụ:
    ///   [LỖI JOB] CopyStudentFromLadiJob - DỪNG TOÀN BỘ - CrmSyncRepository.GetOnlineEventCatsAsync
    ///   [LỖI JOB] ImportCrmDataJob - 3 lỗi - CrmDataService.ImportFromGoogleSheetAsync
    /// </summary>
    private static string BuildSubject(string jobName, IReadOnlyList<JobFailure> failures, bool aborted)
    {
        var source = ResolveSource(failures[0]);
        var scale = aborted ? "DỪNG TOÀN BỘ" : $"{failures.Count} lỗi";

        return $"[LỖI JOB] {jobName} - {scale} - {source}";
    }

    /// <summary>
    /// Lần vị trí lỗi thật: frame sâu nhất còn nằm trong code của mình.
    /// Nhờ vậy tiêu đề chỉ đúng Repository/Service/Controller gây lỗi thay vì
    /// chỉ ra tên job chung chung.
    /// </summary>
    private static string ResolveSource(JobFailure failure)
    {
        var site = DescribeFrame(FindOwnFrame(failure.Exception));

        if (!string.IsNullOrEmpty(site.TypeName) && !string.IsNullOrEmpty(site.MethodName))
            return $"{site.TypeName}.{site.MethodName}";

        return failure.FallbackSource ?? failure.Exception.GetType().Name;
    }

    /// <summary>
    /// Lấy Class/Method thật từ một stack frame.
    ///
    /// Với method async, compiler sinh ra struct state machine nên frame trông như
    /// "&lt;GetByEmailOrPhoneAsync&gt;d__1.MoveNext". Hàm này bóc ngược về
    /// "CrmSyncRepository.GetByEmailOrPhoneAsync" để tiêu đề mail chỉ đúng chỗ lỗi.
    /// </summary>
    private static (string? TypeFullName, string? TypeName, string? MethodName) DescribeFrame(StackFrame? frame)
    {
        var method = frame?.GetMethod();
        if (method == null)
            return (null, null, null);

        var declaring = method.DeclaringType;
        var methodName = method.Name;

        if (declaring != null && IsCompilerGenerated(declaring))
        {
            // async/iterator: class "<Foo>d__3", method "MoveNext" → Foo trên class cha
            var real = ExtractAngleName(declaring.Name);
            if (!string.IsNullOrEmpty(real))
                methodName = real;

            declaring = declaring.DeclaringType ?? declaring;
        }

        // lambda / local function: "<Foo>b__3_0", "<Foo>g__Bar|3_0".
        // Chạy cả sau nhánh trên vì display class "<>c" không mang tên method.
        if (methodName.StartsWith('<'))
        {
            var real = ExtractAngleName(methodName);
            if (!string.IsNullOrEmpty(real))
                methodName = real;
        }

        return (declaring?.FullName, declaring?.Name, methodName);
    }

    private static bool IsCompilerGenerated(Type t)
        => t.Name.StartsWith('<')
           || t.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);

    /// <summary>"&lt;GetAllAsync&gt;d__4" → "GetAllAsync". Không khớp thì trả null.</summary>
    private static string? ExtractAngleName(string? name)
    {
        if (string.IsNullOrEmpty(name) || name[0] != '<')
            return null;

        var close = name.IndexOf('>');
        return close > 1 ? name.Substring(1, close - 1) : null;
    }

    private static StackFrame? FindOwnFrame(Exception ex)
    {
        // Đi từ exception trong cùng ra ngoài: chỗ ném đầu tiên là chỗ sát lỗi nhất
        for (var current = ex; current != null; current = current.InnerException)
        {
            var frames = new StackTrace(current, fNeedFileInfo: true).GetFrames();

            foreach (var f in frames)
            {
                var ns = f.GetMethod()?.DeclaringType?.Namespace;
                if (ns != null && ns.StartsWith(OwnNamespacePrefix, StringComparison.Ordinal))
                    return f;
            }
        }

        return null;
    }

    // ── Nội dung ─────────────────────────────────────────────────────────────

    private string BuildBody(
        string jobName,
        IReadOnlyList<JobFailure> failures,
        bool aborted,
        string? summary)
    {
        var max = _settings.MaxAlertDetails > 0 ? _settings.MaxAlertDetails : failures.Count;
        var shown = failures.Take(max).ToList();

        var sb = new StringBuilder();

        sb.Append("""
            <html><head><meta charset="utf-8"></head>
            <body style="font-family:Segoe UI,Arial,sans-serif;font-size:14px;color:#1d1d1d;">
            """);

        sb.Append($"""
            <h2 style="color:#b11116;margin:0 0 4px;">Job nền gặp lỗi: {H(jobName)}</h2>
            <p style="margin:0 0 16px;color:#777;">
              Máy chủ <b>{H(Environment.MachineName)}</b> ·
              {H(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))}
            </p>
            """);

        // ── Tổng quan ────────────────────────────────────────────────────────
        sb.Append("""<table cellpadding="6" cellspacing="0" border="0" style="border-collapse:collapse;margin-bottom:18px;">""");
        AppendRow(sb, "Job", jobName);
        AppendRow(sb, "Mức độ", aborted
            ? "NGHIÊM TRỌNG - job dừng giữa chừng, dữ liệu chưa xử lý xong"
            : "Job vẫn chạy hết, một số bản ghi bị bỏ qua");
        AppendRow(sb, "Số lỗi", failures.Count.ToString());
        AppendRow(sb, "Vị trí lỗi đầu tiên", ResolveSource(failures[0]));
        if (!string.IsNullOrWhiteSpace(summary))
            AppendRow(sb, "Kết quả lần chạy", summary);
        sb.Append("</table>");

        // ── Chi tiết từng lỗi ────────────────────────────────────────────────
        var index = 0;
        foreach (var f in shown)
        {
            index++;
            var frame = FindOwnFrame(f.Exception);
            var site = DescribeFrame(frame);
            var file = frame?.GetFileName();
            var line = frame?.GetFileLineNumber() ?? 0;

            sb.Append($"""
                <div style="border:1px solid #e1e0d9;border-left:4px solid #d03b3b;
                            padding:12px 16px;margin-bottom:14px;background:#fcfcfb;">
                  <h3 style="margin:0 0 8px;font-size:15px;">
                    Lỗi {index}/{failures.Count} · {H(f.Step)}
                  </h3>
                  <table cellpadding="4" cellspacing="0" border="0" style="border-collapse:collapse;">
                """);

            AppendRow(sb, "Thời điểm", f.OccurredAt.ToString("dd/MM/yyyy HH:mm:ss"));
            AppendRow(sb, "Class", site.TypeFullName ?? f.FallbackSource ?? "(không xác định)");
            AppendRow(sb, "Method", site.MethodName ?? "(không xác định)");

            if (!string.IsNullOrEmpty(file))
                AppendRow(sb, "File", line > 0 ? $"{file}:{line}" : file);

            AppendRow(sb, "Loại exception", f.Exception.GetType().FullName ?? "(?)");
            AppendRow(sb, "Message", f.Exception.Message);

            foreach (var kv in f.Context)
                AppendRow(sb, kv.Key, kv.Value ?? "(null)");

            sb.Append("</table>");

            // Exception đầy đủ, gồm cả inner exception
            sb.Append($"""
                  <div style="margin-top:10px;">
                    <div style="font-weight:600;margin-bottom:4px;">Chi tiết exception</div>
                    <pre style="background:#f4f4f4;border:1px solid #e1e0d9;padding:10px;
                                white-space:pre-wrap;word-break:break-word;font-size:12px;
                                font-family:Consolas,monospace;margin:0;">{H(f.Exception.ToString())}</pre>
                  </div>
                </div>
                """);
        }

        if (failures.Count > shown.Count)
        {
            sb.Append($"""
                <p style="color:#8a5c00;background:#fdf3e0;padding:10px;border-radius:4px;">
                  Còn {failures.Count - shown.Count} lỗi nữa không hiển thị ở đây
                  (giới hạn CrmSync:MaxAlertDetails = {_settings.MaxAlertDetails}).
                  Xem đầy đủ trong log ứng dụng hoặc Hangfire Dashboard.
                </p>
                """);
        }

        sb.Append("""
            <p style="color:#777;font-size:12px;margin-top:20px;">
              Email tự động từ NVCMS.API.ReadGoogleSheet. Theo dõi job tại /hangfire.
            </p>
            </body></html>
            """);

        return sb.ToString();
    }

    private static void AppendRow(StringBuilder sb, string label, string value)
    {
        sb.Append($"""
            <tr>
              <td style="color:#52514e;vertical-align:top;white-space:nowrap;"><b>{H(label)}</b></td>
              <td style="word-break:break-word;">{H(value)}</td>
            </tr>
            """);
    }

    private static string H(string? s) => WebUtility.HtmlEncode(s ?? string.Empty);
}
