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

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrEmpty(CdnSrc)) return;

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

        output.Attributes.RemoveAll(CdnSrcAttr);
        output.Attributes.RemoveAll(CdnWAttr);
        output.Attributes.RemoveAll(CdnHAttr);
    }
}

