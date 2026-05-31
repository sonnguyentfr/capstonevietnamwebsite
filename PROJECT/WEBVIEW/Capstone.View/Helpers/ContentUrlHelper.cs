using System.Text.RegularExpressions;
using Capstone.View.Options;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Capstone.View.Helpers;

// ─────────────────────────────────────────────────────────────────────────────
// Service helper – dùng trong C# code (controllers, services nếu cần)
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Replace /DATA/ /data/ /Portals/ trong HTML content hoac URL don le
/// thanh ServerFilesBaseUrl day du.
/// </summary>
public class ContentUrlHelper
{
    private readonly string _baseUrl;

    private static readonly string[] CdnPrefixes = ["/data/", "/DATA/", "/Portals/"];

    private static readonly Regex AttrPattern = new(
        @"(src|href|poster)\s*=\s*([""'])(/[Dd]ata/|/Portals/)([^""']*)\2",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex CssUrlPattern = new(
        @"url\(\s*['""]?(/[Dd]ata/|/Portals/)([^'""\)]*)['""]?\s*\)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public ContentUrlHelper(IOptions<SiteSettings> options)
    {
        _baseUrl = options.Value.ServerFilesBaseUrl.TrimEnd('/');
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
        foreach (var prefix in CdnPrefixes)
        {
            if (url.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return _baseUrl + "/" + url[prefix.Length..];
        }
        return url;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // YouTube embed converter
    // ─────────────────────────────────────────────────────────────────────────

    private static readonly System.Text.RegularExpressions.Regex _ytWatch =
        new(@"(?:https?://)?(?:www\.)?youtube\.com/watch\?(?:[^#&]*&)*v=([a-zA-Z0-9_-]{11})",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase |
            System.Text.RegularExpressions.RegexOptions.Compiled);

    private static readonly System.Text.RegularExpressions.Regex _ytShort =
        new(@"(?:https?://)?youtu\.be/([a-zA-Z0-9_-]{11})",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase |
            System.Text.RegularExpressions.RegexOptions.Compiled);

    private static readonly System.Text.RegularExpressions.Regex _ytEmbed =
        new(@"(?:https?://)?(?:www\.)?youtube\.com/embed/([a-zA-Z0-9_-]{11})",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase |
            System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>
    /// Convert bất kỳ dạng link YouTube nào sang embed URL chuẩn.
    /// Hỗ trợ:
    ///   https://www.youtube.com/watch?v=VIDEOID
    ///   https://www.youtube.com/watch?v=VIDEOID&amp;t=30s
    ///   https://youtu.be/VIDEOID
    ///   https://www.youtube.com/embed/VIDEOID  (giữ nguyên)
    /// Trả về null nếu không phải link YouTube hợp lệ.
    /// </summary>
    public static string? ToYouTubeEmbedUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;

        var m = _ytWatch.Match(url);
        if (m.Success) return $"https://www.youtube.com/embed/{m.Groups[1].Value}";

        m = _ytShort.Match(url);
        if (m.Success) return $"https://www.youtube.com/embed/{m.Groups[1].Value}";

        m = _ytEmbed.Match(url);
        if (m.Success) return $"https://www.youtube.com/embed/{m.Groups[1].Value}";

        return null;
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Tag Helper – dùng trong Razor views
//
//   <img     cdn-src="@model.ImagePath"  alt="..." loading="lazy">
//   <video   cdn-src="@model.VideoPath"  controls>
//   <source  cdn-src="@model.VideoPath"  type="video/mp4">
//
// cdn-src tu dong replace /DATA/... -> ServerFilesBaseUrl/DATA/...
// Neu can ImageResizer them ?width=&height= thi dung cdn-src ket hop cdn-w / cdn-h:
//   <img cdn-src="@banner.IMGLink" cdn-w="1900" cdn-h="850" alt="...">
// ─────────────────────────────────────────────────────────────────────────────

[HtmlTargetElement("img",    Attributes = CdnSrcAttr)]
[HtmlTargetElement("video",  Attributes = CdnSrcAttr)]
[HtmlTargetElement("source", Attributes = CdnSrcAttr)]
public class CdnSrcTagHelper : TagHelper
{
    private const string CdnSrcAttr = "cdn-src";
    private const string CdnWAttr   = "cdn-w";
    private const string CdnHAttr   = "cdn-h";

    private readonly string _baseUrl;

    public CdnSrcTagHelper(IOptions<SiteSettings> options)
    {
        _baseUrl = options.Value.ServerFilesBaseUrl.TrimEnd('/');
    }

    /// <summary>URL goc lay tu model (co the con dang /DATA/...).</summary>
    [HtmlAttributeName(CdnSrcAttr)]
    public string? CdnSrc { get; set; }

    /// <summary>Chieu rong cho ImageResizer (tuy chon).</summary>
    [HtmlAttributeName(CdnWAttr)]
    public int CdnW { get; set; }

    /// <summary>Chieu cao cho ImageResizer (tuy chon).</summary>
    [HtmlAttributeName(CdnHAttr)]
    public int CdnH { get; set; }

    private const string NoPhotoUrl = "/static/img/no-photo.svg";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // If no cdn-src provided, show placeholder immediately
        if (string.IsNullOrEmpty(CdnSrc))
        {
            output.Attributes.SetAttribute("src", NoPhotoUrl);
            output.Attributes.RemoveAll(CdnSrcAttr);
            output.Attributes.RemoveAll(CdnWAttr);
            output.Attributes.RemoveAll(CdnHAttr);
            return;
        }

        // 1. Strip /DATA/ /data/ /Portals/ roi prepend base URL
        //    /DATA/IMAGES/abc.jpg  ->  https://server.com/IMAGES/abc.jpg
        string resolved;
        if (CdnSrc.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            resolved = CdnSrc;
        }
        else
        {
            string[] stripPrefixes = ["/data/", "/DATA/", "/Portals/"];
            var path = CdnSrc;
            foreach (var prefix in stripPrefixes)
            {
                if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    path = path[prefix.Length..];
                    break;
                }
            }
            resolved = _baseUrl + "/" + path.TrimStart('/');
        }

        // 2. Append ImageResizer params neu co cdn-w / cdn-h
        if (CdnW > 0 || CdnH > 0)
        {
            var w = CdnW > 0 ? CdnW : 1900;
            var h = CdnH > 0 ? CdnH : 850;
            resolved += $"?width={w}&height={h}&mode=crop&anchor=middle";

            if (!output.Attributes.ContainsName("width"))
                output.Attributes.SetAttribute("width", w);
            if (!output.Attributes.ContainsName("height"))
                output.Attributes.SetAttribute("height", h);
        }

        output.Attributes.SetAttribute("src", resolved);

        // 3. onerror fallback -> no-photo if CDN image is broken / 404
        if (!output.Attributes.ContainsName("onerror"))
            output.Attributes.SetAttribute("onerror", $"this.onerror=null;this.src='{NoPhotoUrl}'");

        output.Attributes.RemoveAll(CdnSrcAttr);
        output.Attributes.RemoveAll(CdnWAttr);
        output.Attributes.RemoveAll(CdnHAttr);
    }
}

