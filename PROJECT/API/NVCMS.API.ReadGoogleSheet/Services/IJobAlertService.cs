namespace NVCMS.API.ReadGoogleSheet.Services;

/// <summary>Một lỗi xảy ra trong lúc job chạy.</summary>
public sealed class JobFailure
{
    /// <summary>Thời điểm lỗi.</summary>
    public DateTime OccurredAt { get; init; } = DateTime.Now;

    /// <summary>Bước nghiệp vụ đang chạy, ví dụ "Đọc Google Sheet".</summary>
    public string Step { get; init; } = string.Empty;

    /// <summary>
    /// Nơi gọi (Class.Method) do job tự khai báo. Chỉ dùng khi không lần được
    /// vị trí thật từ stack trace của exception.
    /// </summary>
    public string? FallbackSource { get; init; }

    /// <summary>Dữ liệu ngữ cảnh: EventCatId, LadipageId, Phone...</summary>
    public IReadOnlyDictionary<string, string?> Context { get; init; }
        = new Dictionary<string, string?>();

    /// <summary>Exception gốc.</summary>
    public required Exception Exception { get; init; }
}

/// <summary>
/// Gửi email cảnh báo cho đội IT khi job nền gặp lỗi.
/// </summary>
public interface IJobAlertService
{
    /// <summary>
    /// Gửi báo cáo lỗi của một lần chạy job.
    /// Không ném exception ra ngoài - lỗi gửi mail chỉ được ghi log.
    /// </summary>
    /// <param name="jobName">Tên job, ví dụ "CopyStudentFromLadiJob".</param>
    /// <param name="failures">Danh sách lỗi đã gom trong lần chạy.</param>
    /// <param name="aborted">
    /// true nếu job dừng hẳn (exception thoát khỏi Execute), false nếu job vẫn
    /// chạy hết nhưng có bản ghi lỗi.
    /// </param>
    /// <param name="summary">Dòng tóm tắt kết quả lần chạy (tuỳ chọn).</param>
    Task ReportAsync(
        string jobName,
        IReadOnlyList<JobFailure> failures,
        bool aborted,
        string? summary = null);
}
