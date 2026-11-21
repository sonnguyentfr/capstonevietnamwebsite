using Hangfire;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

/// <summary>
/// FB-SYNC 19/11/2026: quét định kỳ Facebook Page, bài mới được tạo thành bài Tin tức và xuất bản luôn.
/// Lịch chạy: HangfireJobs:FacebookPageSync. Cấu hình Page / chuyên mục: section FacebookSync.
/// Lỗi (token hết hạn, không ghi được ảnh...) gửi mail cảnh báo qua IJobAlertService.
/// </summary>
public class FacebookPageSyncJob
{
    public const string JobId = "facebook-page-sync";
    public const string JobName = nameof(FacebookPageSyncJob);

    private readonly FacebookPageSyncService _service;
    private readonly IJobAlertService _alert;
    private readonly ILogger<FacebookPageSyncJob> _logger;

    public FacebookPageSyncJob(
        FacebookPageSyncService service,
        IJobAlertService alert,
        ILogger<FacebookPageSyncJob> logger)
    {
        _service = service;
        _alert = alert;
        _logger = logger;
    }

    // Không tự retry: job chạy lại theo lịch, và mỗi bài lỗi đã có lượt thử lại riêng (News_FacebookSync.RetryCount)
    [AutomaticRetry(Attempts = 0)]
    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var failures = new List<JobFailure>();
        FacebookSyncResult? result = null;

        try
        {
            result = await _service.RunAsync(failures, cancellationToken);
            _logger.LogInformation("FB-SYNC: {Summary}", result);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;   // Hangfire dừng job - không phải lỗi, không cảnh báo
        }
        catch (Exception ex)
        {
            failures.Add(new JobFailure
            {
                Step = "Đọc bài từ Facebook Page",
                FallbackSource = $"{JobName}.{nameof(Execute)}",
                Exception = ex
            });

            await _alert.ReportAsync(JobName, failures, aborted: true);
            throw;
        }

        if (failures.Count > 0)
            await _alert.ReportAsync(JobName, failures, aborted: false, summary: result?.ToString());
    }
}
