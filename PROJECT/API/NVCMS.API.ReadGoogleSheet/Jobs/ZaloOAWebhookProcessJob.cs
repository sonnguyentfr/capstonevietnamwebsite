using Hangfire;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

/// <summary>Xử lý 1 webhook event Zalo OA (enqueue từ ZaloOAWebhookController).</summary>
public class ZaloOAWebhookProcessJob
{
    private readonly IZaloOAWebhookService _webhook;

    public ZaloOAWebhookProcessJob(IZaloOAWebhookService webhook)
    {
        _webhook = webhook;
    }

    // Không để Hangfire tự retry: số lần retry được đếm trong ZaloOA_WebhookEvent.RetryCount
    // và do ZaloOAWebhookReprocessJob đảm nhận (tránh xử lý song song 2 đường).
    [AutomaticRetry(Attempts = 0)]
    public Task ExecuteAsync(long eventId, CancellationToken cancellationToken = default)
        => _webhook.ProcessAsync(eventId, cancellationToken);
}

/// <summary>Job định kỳ: xử lý lại event FAILED (còn lượt) hoặc PENDING bị kẹt (enqueue lỗi, app restart...).</summary>
public class ZaloOAWebhookReprocessJob
{
    private readonly IZaloOAChatRepository _repo;
    private readonly IZaloOAWebhookService _webhook;
    private readonly ZaloOAChatSettings _settings;
    private readonly ILogger<ZaloOAWebhookReprocessJob> _logger;

    public ZaloOAWebhookReprocessJob(IZaloOAChatRepository repo, IZaloOAWebhookService webhook,
        IOptions<ZaloOAChatSettings> settings, ILogger<ZaloOAWebhookReprocessJob> logger)
    {
        _repo = repo;
        _webhook = webhook;
        _settings = settings.Value;
        _logger = logger;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 600)]
    [AutomaticRetry(Attempts = 0)]
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var ids = await _repo.GetWebhookEventsForReprocessAsync(
            _settings.WebhookReprocessStaleMinutes, _settings.WebhookMaxRetry, top: 100);
        if (ids.Count == 0)
            return;

        _logger.LogInformation("Zalo webhook reprocess: {Count} event", ids.Count);
        int ok = 0, fail = 0;
        foreach (var id in ids)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await _webhook.ProcessAsync(id, cancellationToken);
                ok++;
            }
            catch (Exception ex)
            {
                fail++;
                _logger.LogWarning("Zalo webhook reprocess EventId={EventId} vẫn lỗi: {Error}", id, ex.Message);
            }
        }
        _logger.LogInformation("Zalo webhook reprocess xong: OK={Ok} Lỗi={Fail}", ok, fail);
    }
}
