using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public interface IZaloZnsClient
{
    Task<ZaloApiEnvelope<List<ZaloTemplateListItemDto>>> GetTemplateListAsync(CancellationToken cancellationToken = default);
    Task<ZaloApiEnvelope<ZaloTemplateDetailDto>> GetTemplateDetailAsync(long templateId, CancellationToken cancellationToken = default);
    Task<ZaloApiEnvelope<ZaloSendResponseData>> SendMessageAsync(long templateId, string phone, Dictionary<string, object?> templateData, string trackingId, CancellationToken cancellationToken = default);
}
