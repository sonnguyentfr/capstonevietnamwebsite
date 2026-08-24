using Hangfire;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

public class ZnsSendJob
{
    private readonly IZnsSendService _sendService;
    private readonly ILogger<ZnsSendJob> _logger;

    public ZnsSendJob(IZnsSendService sendService, ILogger<ZnsSendJob> logger)
    {
        _sendService = sendService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
    public async Task ExecuteAsync(long queueId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ZNS send job start queueId={QueueId}", queueId);
        var result = await _sendService.SendFromQueueAsync(queueId, cancellationToken);

        if (!result.Success)
            _logger.LogWarning("ZNS send job business fail queueId={QueueId}, code={Code}, msg={Message}", queueId, result.ErrorCode, result.Message);
        else
            _logger.LogInformation("ZNS send job done queueId={QueueId}, msgId={MsgId}", queueId, result.MsgId);
    }
}
