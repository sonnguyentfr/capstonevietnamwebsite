using Hangfire;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

public class ZnsTemplateSyncJob
{
    private readonly IZnsTemplateService _templateService;
    private readonly ILogger<ZnsTemplateSyncJob> _logger;

    public ZnsTemplateSyncJob(IZnsTemplateService templateService, ILogger<ZnsTemplateSyncJob> logger)
    {
        _templateService = templateService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ZNS template sync job started at {Now}", DateTime.UtcNow);
        var changed = await _templateService.SyncTemplatesAsync(cancellationToken);
        _logger.LogInformation("ZNS template sync job done, changed={Changed}", changed);
    }
}
