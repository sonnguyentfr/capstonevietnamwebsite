using NVCMS.API.ReadGoogleSheet.Infrastructure.Http;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public class ZaloZnsClient : IZaloZnsClient
{
    private readonly BaseApi _api;
    private readonly IZaloService _zaloService;
    private readonly ZaloSettings _settings;

    public ZaloZnsClient(BaseApi api, IZaloService zaloService, Microsoft.Extensions.Options.IOptions<ZaloSettings> settings)
    {
        _api = api;
        _zaloService = zaloService;
        _settings = settings.Value;
    }

    public async Task<ZaloApiEnvelope<List<ZaloTemplateListItemDto>>> GetTemplateListAsync(CancellationToken cancellationToken = default)
    {
        var token = await _zaloService.GetLastTokenAsync();
        var url = _settings.TemplateListEndpoint;

        return await _api.GetJsonAsync<ZaloApiEnvelope<List<ZaloTemplateListItemDto>>>(
            url,
            new Dictionary<string, string> { { "access_token", token.AccessToken } });
    }

    public async Task<ZaloApiEnvelope<ZaloTemplateDetailDto>> GetTemplateDetailAsync(long templateId, CancellationToken cancellationToken = default)
    {
        var token = await _zaloService.GetLastTokenAsync();
        var url = _settings.TemplateDetailEndpoint.Replace("{template_id}", templateId.ToString());

        return await _api.GetJsonAsync<ZaloApiEnvelope<ZaloTemplateDetailDto>>(
            url,
            new Dictionary<string, string> { { "access_token", token.AccessToken } });
    }

    public async Task<ZaloApiEnvelope<ZaloSendResponseData>> SendMessageAsync(long templateId, string phone, Dictionary<string, object?> templateData, string trackingId, CancellationToken cancellationToken = default)
    {
        var token = await _zaloService.GetLastTokenAsync();
        var body = new
        {
            phone,
            template_id = templateId,
            template_data = templateData,
            tracking_id = trackingId
        };

        return await _api.PostJsonAsync<object, ZaloApiEnvelope<ZaloSendResponseData>>(
            _settings.ApiSendMessage,
            body,
            new Dictionary<string, string> { { "access_token", token.AccessToken } });
    }
}
