using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Services;

public interface IZnsSendService
{
    Task<ZnsSendResult> SendNowAsync(ZnsSendRequest request, CancellationToken cancellationToken = default);
    Task<(long queueId, string jobId)> EnqueueAsync(ZnsSendRequest request, CancellationToken cancellationToken = default);
    Task<ZnsSendResult> SendFromQueueAsync(long queueId, CancellationToken cancellationToken = default);
}
