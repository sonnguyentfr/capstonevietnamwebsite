using Hangfire;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

/// <summary>
/// BIRTHDAY-MAIL 02/11/2026: thư chúc mừng sinh nhật khách hàng CRM.
///
///   Execute  — recurring "birthday-mail" (HangfireJobs:BirthdayMail, mặc định 10 phút/lần). Mỗi tick đọc cấu hình Marketing trong DB CRM
///              (NVCore_BirthdayMail_Config): portal nào đang BẬT và đã tới giờ gửi mà hôm nay chưa có lượt -> gửi.
///              Bảng NVCore_BirthdayMail_Run UNIQUE (PortalId, RunDate) => 1 ngày đúng 1 lượt, tick sau chỉ bỏ qua.
///   RunNow   — CRM bấm "Gửi ngay" (BirthdayMailController.run-now enqueue): bỏ qua giờ gửi, vẫn 1 ngày 1 lượt.
///
/// Bật / tắt: Marketing bật-tắt trong CRM (không cần deploy). Tắt hẳn job ở tầng Hangfire: HangfireJobs:BirthdayMail:Enabled = false.
/// </summary>
public class BirthdayMailJob
{
    public const string JobId = "birthday-mail";
    public const string JobName = nameof(BirthdayMailJob);

    private readonly IBirthdayMailRepository _repo;
    private readonly BirthdayMailService _service;
    private readonly IJobAlertService _alert;
    private readonly ILogger<BirthdayMailJob> _logger;

    public BirthdayMailJob(IBirthdayMailRepository repo, BirthdayMailService service, IJobAlertService alert, ILogger<BirthdayMailJob> logger)
    {
        _repo = repo;
        _service = service;
        _alert = alert;
        _logger = logger;
    }

    // Không retry chồng: tick sau (10 phút) tự chạy lại. Không cần DisableConcurrentExecution — 2 tick chạy chồng thì proc
    // NVCore_BirthdayMail_Run_Begin (UPDLOCK) chỉ cho 1 bên gửi, bên kia nhận "busy" và thoát.
    [AutomaticRetry(Attempts = 0)]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var configs = await _repo.GetEnabledConfigsAsync();
        foreach (var cfg in configs)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var r = await _service.RunAsync(cfg, false, "Hangfire", cancellationToken);
                if (r.RunId > 0) _logger.LogInformation("BirthdayMail portal {Portal}: {Message}", cfg.PortalId, r.Message);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await ReportAsync(cfg.PortalId, ex);
            }
        }
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task RunNow(int portalId, string triggeredBy, CancellationToken cancellationToken = default)
    {
        var cfg = await _repo.GetConfigAsync(portalId);
        if (cfg == null)
        {
            _logger.LogWarning("BirthdayMail RunNow: portal {Portal} chưa có cấu hình.", portalId);
            return;
        }
        try
        {
            var r = await _service.RunAsync(cfg, true, string.IsNullOrWhiteSpace(triggeredBy) ? "CRM" : triggeredBy, cancellationToken);
            _logger.LogInformation("BirthdayMail RunNow portal {Portal} ({By}): {Action} {Message}", portalId, triggeredBy, r.Action, r.Message);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await ReportAsync(portalId, ex);
            throw;   // Hangfire ghi Failed để xem trên dashboard
        }
    }

    private async Task ReportAsync(int portalId, Exception ex)
    {
        _logger.LogError(ex, "BirthdayMail portal {Portal} lỗi", portalId);
        try
        {
            await _alert.ReportAsync(JobName, new List<JobFailure>
            {
                new JobFailure
                {
                    Step = "Gửi thư sinh nhật (portal " + portalId + ")",
                    FallbackSource = JobName,
                    Exception = ex
                }
            }, aborted: true);
        }
        catch (Exception) { }
    }
}
