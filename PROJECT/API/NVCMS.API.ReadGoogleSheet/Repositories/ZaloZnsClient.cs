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
        var url = RequireEndpoint(_settings.TemplateListEndpoint, nameof(_settings.TemplateListEndpoint));

        return await _api.GetJsonAsync<ZaloApiEnvelope<List<ZaloTemplateListItemDto>>>(
            url,
            new Dictionary<string, string> { { "access_token", token.AccessToken } });
    }

    public async Task<ZaloApiEnvelope<ZaloTemplateDetailDto>> GetTemplateDetailAsync(long templateId, CancellationToken cancellationToken = default)
    {
        var token = await _zaloService.GetLastTokenAsync();
        var url = RequireEndpoint(_settings.TemplateDetailEndpoint, nameof(_settings.TemplateDetailEndpoint))
            .Replace("{template_id}", templateId.ToString());

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
            RequireEndpoint(_settings.ApiSendMessage, nameof(_settings.ApiSendMessage)),
            body,
            new Dictionary<string, string> { { "access_token", token.AccessToken } });
    }

    /// <summary>
    /// Kiểm tra endpoint lấy từ appsettings trước khi gọi HttpClient.
    ///
    /// Thiếu key thì HttpClient chỉ ném "An invalid request URI was provided",
    /// không cho biết thiếu cấu hình nào - ZnsTemplateSyncJob từng hỏng 95 lần
    /// liên tiếp trên server vì lý do này mà không ai biết. Thông báo dưới đây
    /// nêu thẳng tên key và tên máy để sửa được ngay.
    /// </summary>
    private static string RequireEndpoint(string? value, string key)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Thiếu cấu hình \"ZaloSettings:{key}\" trong appsettings.json của máy {Environment.MachineName}. " +
                $"Bổ sung key này rồi khởi động lại ứng dụng.");

        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
            throw new InvalidOperationException(
                $"Cấu hình \"ZaloSettings:{key}\" trên máy {Environment.MachineName} không phải URL tuyệt đối: \"{value}\".");

        return value;
    }
}
