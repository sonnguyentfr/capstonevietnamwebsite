using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Models.Config;

namespace NVCMS.API.ReadGoogleSheet.Services;

/// <summary>Thông tin thương hiệu dùng trong mail: tên site, URL site, logo.</summary>
public sealed record SiteBranding(string SiteName, string SiteUrl, string LogoUrl);

public interface ISiteBrandingProvider
{
    Task<SiteBranding> GetAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Đọc tên site / URL / logo từ DB website (SP WebView_GetSiteSettings) giống hệt
/// Capstone.View SiteSettingsHelper, để mail gửi từ job trùng với mail của trang
/// /dang-ky-su-kien. Cache 30 phút; lỗi DB thì dùng giá trị dự phòng trong CrmSync.
/// </summary>
public class SiteBrandingProvider : ISiteBrandingProvider
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    private readonly string? _webConnStr;
    private readonly CrmSyncSettings _settings;
    private readonly ILogger<SiteBrandingProvider> _logger;

    private readonly SemaphoreSlim _lock = new(1, 1);
    private SiteBranding? _cached;
    private DateTime _cachedAtUtc;

    public SiteBrandingProvider(
        IConfiguration config,
        IOptions<CrmSyncSettings> settings,
        ILogger<SiteBrandingProvider> logger)
    {
        _webConnStr = config.GetConnectionString("DefaultConnection");
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<SiteBranding> GetAsync(CancellationToken cancellationToken = default)
    {
        if (_cached != null && DateTime.UtcNow - _cachedAtUtc < CacheDuration)
            return _cached;

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_cached != null && DateTime.UtcNow - _cachedAtUtc < CacheDuration)
                return _cached;

            _cached = await LoadAsync();
            _cachedAtUtc = DateTime.UtcNow;
            return _cached;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<SiteBranding> LoadAsync()
    {
        var fallback = new SiteBranding(_settings.SiteName, _settings.SiteUrl, _settings.LogoUrl);

        if (string.IsNullOrWhiteSpace(_webConnStr))
            return fallback;

        try
        {
            using var conn = new SqlConnection(_webConnStr);
            var rows = await conn.QueryAsync<(string Key, string Value)>(
                "WebView_GetSiteSettings",
                new { PortalId = _settings.WebsitePortalId },
                commandType: CommandType.StoredProcedure);

            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var (key, value) in rows)
                dict[key ?? string.Empty] = (value ?? string.Empty).Trim();

            string Get(string k) => dict.TryGetValue(k, out var v) ? v : string.Empty;

            var siteName = Get("settingPagesitename");
            var siteUrl = Get("settingPagesiteweb");
            var logo = Get("settingPagesiteLogo");
            var fileServer = Get("settingPagesiteFileServer");

            // Giống EventRegistrationMailService: HeaderLogo.Replace("/DATA", FileServer)
            if (!string.IsNullOrEmpty(logo) && !string.IsNullOrEmpty(fileServer))
                logo = logo.Replace("/DATA", fileServer);

            return new SiteBranding(
                string.IsNullOrWhiteSpace(siteName) ? fallback.SiteName : siteName,
                string.IsNullOrWhiteSpace(siteUrl) ? fallback.SiteUrl : siteUrl,
                string.IsNullOrWhiteSpace(logo) ? fallback.LogoUrl : logo);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Không đọc được WebView_GetSiteSettings (PortalId={PortalId}) → dùng tên/logo dự phòng trong CrmSync",
                _settings.WebsitePortalId);
            return fallback;
        }
    }
}
