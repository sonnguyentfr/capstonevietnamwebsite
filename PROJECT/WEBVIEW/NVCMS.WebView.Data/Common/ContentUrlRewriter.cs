using System.Text.RegularExpressions;

namespace NVCMS.WebView.Data.Common;

/// <summary>
/// Strip /DATA/ roi prepend ServerFilesBaseUrl.
/// Vi du: /DATA/IMAGES/abc.jpg  ->  https://server.com/IMAGES/abc.jpg
/// </summary>
public class ContentUrlRewriter
{
    private readonly string _baseUrl;

    private static readonly string[] StripPrefixes = ["/data/", "/DATA/", "/Portals/"];

    // Match src/href/poster="/DATA/path"  -> group[4] = "path"
    private static readonly Regex AttrPattern = new(
        @"(src|href|poster)\s*=\s*([""'])(/[Dd]ata/|/Portals/)([^""']*)\2",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Match url('/DATA/path') -> group[2] = "path"
    private static readonly Regex CssUrlPattern = new(
        @"url\(\s*['""]?(/[Dd]ata/|/Portals/)([^'""\)]*)['""]?\s*\)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public ContentUrlRewriter(string serverFilesBaseUrl)
    {
        _baseUrl = serverFilesBaseUrl.TrimEnd('/');
    }

    /// <summary>
    /// Rewrite toan bo src/href/poster/url() trong HTML:
    ///   src="/DATA/IMAGES/abc.jpg"  ->  src="https://server.com/IMAGES/abc.jpg"
    /// </summary>
    public string ResolveHtml(string? html)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;

        html = AttrPattern.Replace(html, m =>
            $"{m.Groups[1].Value}={m.Groups[2].Value}{_baseUrl}/{m.Groups[4].Value}{m.Groups[2].Value}");

        html = CssUrlPattern.Replace(html, m =>
            $"url('{_baseUrl}/{m.Groups[2].Value}')");

        return html;
    }

    /// <summary>
    /// Resolve URL don le:
    ///   /DATA/IMAGES/abc.jpg  ->  https://server.com/IMAGES/abc.jpg
    /// </summary>
    public string ResolveUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return string.Empty;
        if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return url;
        foreach (var prefix in StripPrefixes)
        {
            if (url.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return _baseUrl + "/" + url[prefix.Length..];
        }
        return url;
    }
}
