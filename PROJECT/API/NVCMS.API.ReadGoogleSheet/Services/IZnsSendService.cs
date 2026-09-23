using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Services;

public interface IZnsSendService
{
    Task<ZnsSendResult> SendNowAsync(ZnsSendRequest request, CancellationToken cancellationToken = default);
    Task<ZnsEnqueueResult> EnqueueAsync(ZnsSendRequest request, CancellationToken cancellationToken = default);
    Task<int> ProcessCampaignEnqueueAsync(ZnsCampaignEnqueueArgs args, CancellationToken cancellationToken = default);
    Task<ZnsSendResult> SendFromQueueAsync(long queueId, CancellationToken cancellationToken = default);
}
