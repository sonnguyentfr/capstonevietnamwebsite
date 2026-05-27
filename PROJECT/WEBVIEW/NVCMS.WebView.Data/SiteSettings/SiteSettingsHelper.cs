using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

namespace NVCMS.WebView.Data.SiteSettings;

/// <summary>
/// Loads website settings by calling stored procedure WebView_GetSiteSettings
/// via DefaultConnection, and caches the result per portalId.
/// </summary>
public class SiteSettingsHelper : ISiteSettingsHelper
{
    private readonly string _connectionString;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public SiteSettingsHelper(string connectionString, IMemoryCache cache)
    {
        _connectionString = connectionString;
        _cache = cache;
    }

    public async Task<WebSiteSettings> GetSettingsAsync(int portalId)
    {
        var cacheKey = $"WebSiteSettings_{portalId}";
        if (_cache.TryGetValue(cacheKey, out WebSiteSettings? cached) && cached is not null)
            return cached;

        var settings = await LoadFromDbAsync(portalId);
        _cache.Set(cacheKey, settings, CacheDuration);
        return settings;
    }

    public async Task<IReadOnlyList<BranchInfo>> GetBranchesAsync(int portalId)
    {
        var cacheKey = $"Branches_{portalId}";
        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<BranchInfo>? cached) && cached is not null)
            return cached;

        var branches = await LoadBranchesFromDbAsync(portalId);
        _cache.Set(cacheKey, branches, CacheDuration);
        return branches;
    }

    public async Task<HeaderFooterData> GetHeaderFooterDataAsync(int portalId)
    {
        var cacheKey = $"HeaderFooterData_{portalId}";
        if (_cache.TryGetValue(cacheKey, out HeaderFooterData? cached) && cached is not null)
            return cached;

        var data = await LoadHeaderFooterDataFromDbAsync(portalId);
        _cache.Set(cacheKey, data, CacheDuration);
        return data;
    }

    public void InvalidateCache(int portalId)
    {
        _cache.Remove($"WebSiteSettings_{portalId}");
        _cache.Remove($"Branches_{portalId}");
        _cache.Remove($"HeaderFooterData_{portalId}");
    }

    private async Task<WebSiteSettings> LoadFromDbAsync(int portalId)
    {
        // SP returns rows with columns: SettingName, SettingValue
        using var conn = new SqlConnection(_connectionString);
        var rows = await conn.QueryAsync<(string Key, string Value)>(
            "WebView_GetSiteSettings",
            new { PortalId = portalId },
            commandType: System.Data.CommandType.StoredProcedure);

        var dict = rows.ToDictionary(
            r => r.Key ?? string.Empty,
            r => (r.Value ?? string.Empty).Trim(),
            StringComparer.OrdinalIgnoreCase);

        string Get(string key) =>
            dict.TryGetValue(key, out var v) ? v : string.Empty;

        bool GetBool(string key) =>
            Get(key).Equals("true", StringComparison.OrdinalIgnoreCase);

        var branches = new List<BranchInfo>();
        for (int i = 0; i <= 5; i++)
        {
            var s       = i == 0 ? "" : i.ToString();
            var name    = Get("settingPagesitechinhnhanh" + s);
            var address = Get("settingPagesitediachi"     + s);
            var email   = Get("settingPagesiteemail"      + s);
            var phone   = Get("settingPagesitedienthoai"  + s);

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(address)
                || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone))
            {
                branches.Add(new BranchInfo
                {
                    Name    = name,
                    Address = address,
                    Email   = email,
                    Phone   = phone,
                });
            }
        }

        return new WebSiteSettings
        {
            PortalId = portalId,
            General  = new GeneralInfo
            {
                SiteName    = Get("settingPagesitename"),
                SiteWeb     = Get("settingPagesiteweb"),
                SiteAddress = Get("settingPagesitediachi"),
                SiteEmail   = Get("settingPagesiteemail"),
                SitePhone   = Get("settingPagesitedienthoai"),
                SiteSummary = Get("settingPagesitesummary"),
                SiteTag     = Get("settingPagesitetag"),
            },
            Social   = new SocialInfo
            {
                Facebook  = Get("settingPagesiteFacebook"),
                Youtube   = Get("settingPagesiteYoutube"),
                LinkedIn  = Get("settingPagesiteLinkedin"),
                Instagram = Get("settingPagesiteInstagram"),
                Zalo      = Get("settingPagesiteZalo"),
                Twitter   = Get("settingPagesiteTwitter"),
                Whatsapp  = Get("settingPagesiteWhatsapp"),
                Skype     = Get("settingPagesiteSkype"),
            },
            Chat     = new ChatInfo
            {
                ZaloId         = Get("settingPagesiteZaloId"),
                FacebookChatId = Get("settingPagesiteFacebookChatId"),
                LiveChatId     = Get("settingPagesiteLiveChatId"),
            },
            Mail     = new MailInfo
            {
                EnableMail = GetBool("settingPagesiteNhanMail"),
                MailList   = Get("settingPagesiteMailList"),
            },
            Google   = new GoogleInfo
            {
                CaptchaKey    = Get("settingPagesiteCaptchaKey"),
                CaptchaSecret = Get("settingPagesiteCaptchaSecret"),
            },
            Cdn      = new CdnInfo
            {
                Cdn        = Get("settingPagesiteCdn"),
                FileServer = Get("settingPagesiteFileServer"),
            },
            Logo     = new LogoInfo
            {
                HeaderLogo = Get("settingPagesiteLogo"),
                FooterLogo = Get("settingPagesiteLogoFooter"),
            },
            Code     = new CodeInfo
            {
                HeaderCode = Get("settingPagesiteHeaderCode"),
                FooterCode = Get("settingPagesiteFooterCode"),
            },
            Branches = branches.AsReadOnly(),
        };
    }

    private async Task<IReadOnlyList<BranchInfo>> LoadBranchesFromDbAsync(int portalId)
    {
        using var conn = new SqlConnection(_connectionString);
        var rows = await conn.QueryAsync<(string Key, string Value)>(
            "WebView_GetSiteSettings",
            new { PortalId = portalId },
            commandType: System.Data.CommandType.StoredProcedure);

        var dict = rows.ToDictionary(
            r => r.Key ?? string.Empty,
            r => (r.Value ?? string.Empty).Trim(),
            StringComparer.OrdinalIgnoreCase);

        string Get(string key) =>
            dict.TryGetValue(key, out var v) ? v : string.Empty;

        // Load all branches
        var branches = new List<BranchInfo>();
        for (int i = 0; i <= 5; i++)
        {
            var s       = i == 0 ? "" : i.ToString();
            var name    = Get("settingPagesitechinhnhanh" + s);
            var address = Get("settingPagesitediachi"     + s);
            var email   = Get("settingPagesiteemail"      + s);
            var phone   = Get("settingPagesitedienthoai"  + s);

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(address)
                || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone))
            {
                branches.Add(new BranchInfo
                {
                    Name    = name,
                    Address = address,
                    Email   = email,
                    Phone   = phone,
                });
            }
        }

        return branches.AsReadOnly();
    }

    private async Task<HeaderFooterData> LoadHeaderFooterDataFromDbAsync(int portalId)
    {
        using var conn = new SqlConnection(_connectionString);
        var rows = await conn.QueryAsync<(string Key, string Value)>(
            "WebView_GetSiteSettings",
            new { PortalId = portalId },
            commandType: System.Data.CommandType.StoredProcedure);

        var dict = rows.ToDictionary(
            r => r.Key ?? string.Empty,
            r => (r.Value ?? string.Empty).Trim(),
            StringComparer.OrdinalIgnoreCase);

        string Get(string key) =>
            dict.TryGetValue(key, out var v) ? v : string.Empty;

        // Load only first 2 branches
        var branches = new List<BranchInfo>();
        for (int i = 0; i <= 2; i++)
        {
            var s       = i == 0 ? "" : i.ToString();
            var name    = Get("settingPagesitechinhnhanh" + s);
            var address = Get("settingPagesitediachi"     + s);
            var email   = Get("settingPagesiteemail"      + s);
            var phone   = Get("settingPagesitedienthoai"  + s);

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(address)
                || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phone))
            {
                branches.Add(new BranchInfo
                {
                    Name    = name,
                    Address = address,
                    Email   = email,
                    Phone   = phone,
                });
            }
        }

        return new HeaderFooterData
        {
            Branches = branches.AsReadOnly(),
            Social = new SocialInfo
            {
                Facebook  = Get("settingPagesiteFacebook"),
                Youtube   = Get("settingPagesiteYoutube"),
                LinkedIn  = Get("settingPagesiteLinkedin"),
                Instagram = Get("settingPagesiteInstagram"),
                Zalo      = Get("settingPagesiteZalo"),
                Twitter   = Get("settingPagesiteTwitter"),
                Whatsapp  = Get("settingPagesiteWhatsapp"),
                Skype     = Get("settingPagesiteSkype"),
            },
            SitePhone = Get("settingPagesitedienthoai"),
            SiteEmail = Get("settingPagesiteemail"),
            HeaderLogo = Get("settingPagesiteLogo"),
            FooterLogo = Get("settingPagesiteLogoFooter"),
            HeaderCode = Get("settingPagesiteHeaderCode"),
            FooterCode = Get("settingPagesiteFooterCode"),
            SiteName = Get("settingPagesitename"),
        };
    }
}
