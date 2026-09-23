using Hangfire;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

/// <summary>
/// Tách danh sách SĐT của campaign thành từng ZnsSendQueue + ZnsSendJob.
/// Chạy nền để API không bị timeout với campaign lớn (hàng chục nghìn SĐT).
/// </summary>
public class ZnsCampaignEnqueueJob
{
    private readonly IZnsSendService _sendService;
    private readonly ILogger<ZnsCampaignEnqueueJob> _logger;

    public ZnsCampaignEnqueueJob(IZnsSendService sendService, ILogger<ZnsCampaignEnqueueJob> logger)
    {
        _sendService = sendService;
        _logger = logger;
    }

    // Retry an toàn: service bỏ qua SĐT đã enqueue kể từ RequestedAt
    [AutomaticRetry(Attempts = 2, DelaysInSeconds = new[] { 60, 300 })]
    public async Task ExecuteAsync(ZnsCampaignEnqueueArgs args, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ZNS campaign enqueue start campaignId={CampaignId}, templateId={TemplateId}",
            args.CampaignId, args.TemplateId);

        var total = await _sendService.ProcessCampaignEnqueueAsync(args, cancellationToken);

        _logger.LogInformation("ZNS campaign enqueue done campaignId={CampaignId}, enqueued={Total}",
            args.CampaignId, total);
    }
}
