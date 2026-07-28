using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Common;

namespace Capstone.View.Controllers;

/// <summary>
/// Controller để generate sitemap XML chuẩn Google Webmaster Tools
/// </summary>
public class SitemapController : Controller
{
    private readonly INewsService _newsService;
    private readonly ITruongService _truongService;
    private readonly IEventsService _eventsService;
    private readonly int _portalId;
    private const string SitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

    public SitemapController(
        INewsService newsService,
        ITruongService truongService,
        IEventsService eventsService,
        IConfiguration config)
    {
        _newsService = newsService;
        _truongService = truongService;
        _eventsService = eventsService;
        _portalId = config.GetValue<int>("SiteSettings:PortalId");
    }

    /// <summary>
    /// GET /sitemap.xml - Sitemap index chính
    /// Chứa links đến các sitemap con
    /// </summary>
    [Route("sitemap.xml")]
    public IActionResult Index()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var xml = new StringBuilder();

        xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xml.AppendLine($"<sitemapindex xmlns=\"{SitemapNamespace}\">");

        // Sitemap tĩnh
        xml.AppendLine("  <sitemap>");
        xml.AppendLine($"    <loc>{baseUrl}/sitemap-static.xml</loc>");
        xml.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        xml.AppendLine("  </sitemap>");

        // Sitemap tin tức
        xml.AppendLine("  <sitemap>");
        xml.AppendLine($"    <loc>{baseUrl}/sitemap-news.xml</loc>");
        xml.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        xml.AppendLine("  </sitemap>");

        // Sitemap trường đối tác
        xml.AppendLine("  <sitemap>");
        xml.AppendLine($"    <loc>{baseUrl}/sitemap-schools.xml</loc>");
        xml.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        xml.AppendLine("  </sitemap>");

        // Sitemap sự kiện
        xml.AppendLine("  <sitemap>");
        xml.AppendLine($"    <loc>{baseUrl}/sitemap-events.xml</loc>");
        xml.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
        xml.AppendLine("  </sitemap>");

        xml.AppendLine("</sitemapindex>");

        return Content(xml.ToString(), "application/xml", Encoding.UTF8);
    }

    /// <summary>
    /// GET /sitemap-static.xml - Sitemap cho các trang tĩnh
    /// </summary>
    [Route("sitemap-static.xml")]
    public IActionResult StaticSitemap()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var xml = new StringBuilder();

        xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xml.AppendLine($"<urlset xmlns=\"{SitemapNamespace}\">");

        // Trang chủ
        AddUrl(xml, baseUrl, DateTime.UtcNow, "daily", "1.0");

        // Trang tin tức
        AddUrl(xml, $"{baseUrl}/tin-tuc", DateTime.UtcNow, "daily", "0.9");

        // Trang trường đối tác
        AddUrl(xml, $"{baseUrl}/truong-doi-tac", DateTime.UtcNow, "weekly", "0.9");

        // Trang tìm trường
        AddUrl(xml, $"{baseUrl}/tim-truong", DateTime.UtcNow, "weekly", "0.8");

        // Trang sự kiện
        AddUrl(xml, $"{baseUrl}/su-kien", DateTime.UtcNow, "weekly", "0.9");

        // Trang giới thiệu
        AddUrl(xml, $"{baseUrl}/gioi-thieu", DateTime.UtcNow, "monthly", "0.7");
        AddUrl(xml, $"{baseUrl}/gioi-thieu/ve-chung-toi", DateTime.UtcNow, "monthly", "0.7");
        AddUrl(xml, $"{baseUrl}/gioi-thieu/doi-ngu", DateTime.UtcNow, "monthly", "0.7");

        // Trang dịch vụ
        AddUrl(xml, $"{baseUrl}/dich-vu", DateTime.UtcNow, "monthly", "0.8");

        // Trang thông tin du học
        AddUrl(xml, $"{baseUrl}/thong-tin-du-hoc", DateTime.UtcNow, "weekly", "0.8");

        // Trang tư vấn định cư
        AddUrl(xml, $"{baseUrl}/tu-van-dinh-cu", DateTime.UtcNow, "weekly", "0.8");
        AddUrl(xml, $"{baseUrl}/tu-van-dinh-cu/tin-tuc-dinh-cu", DateTime.UtcNow, "weekly", "0.7");
        AddUrl(xml, $"{baseUrl}/tu-van-dinh-cu/tin-tuc-dau-tu", DateTime.UtcNow, "weekly", "0.7");

        // Trang liên hệ
        AddUrl(xml, $"{baseUrl}/lien-he", DateTime.UtcNow, "monthly", "0.6");

        xml.AppendLine("</urlset>");

        return Content(xml.ToString(), "application/xml", Encoding.UTF8);
    }

    /// <summary>
    /// Helper method để thêm URL vào sitemap
    /// </summary>
    private static void AddUrl(StringBuilder xml, string loc, DateTime lastmod, string changefreq, string priority)
    {
        xml.AppendLine("  <url>");
        xml.AppendLine($"    <loc>{XmlEscape(loc)}</loc>");
        xml.AppendLine($"    <lastmod>{lastmod:yyyy-MM-dd}</lastmod>");
        xml.AppendLine($"    <changefreq>{changefreq}</changefreq>");
        xml.AppendLine($"    <priority>{priority}</priority>");
        xml.AppendLine("  </url>");
    }

    /// <summary>
    /// GET /sitemap-news.xml - Sitemap cho tin tức
    /// </summary>
    [Route("sitemap-news.xml")]
    public async Task<IActionResult> NewsSitemap()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var xml = new StringBuilder();

        xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xml.AppendLine($"<urlset xmlns=\"{SitemapNamespace}\">");

        try
        {
            // Lấy tất cả tin tức (sử dụng pageSize lớn để lấy nhiều records)
            // Google cho phép tối đa 50,000 URLs per sitemap
            var newsItems = await _newsService.GetAllPagedAsync(_portalId, 1, 10000);

            foreach (var news in newsItems.Items)
            {
                // Build URL tin tức
                var newsUrl = NVCMS.WebView.Data.Common.NewsUrlBuilder.BuildFullNewsUrl(
                    Request.Scheme, Request.Host.Value, news);

                AddUrl(xml, newsUrl, news.PublishedDate, "weekly", "0.7");
            }

            // Thêm các category tin tức
            var categories = await _newsService.GetCategoriesWithCountAsync(_portalId);
            foreach (var cat in categories)
            {
                if (!string.IsNullOrEmpty(cat.Slug))
                {
                    var catUrl = $"{baseUrl}/{cat.Slug}";
                    AddUrl(xml, catUrl, DateTime.UtcNow, "daily", "0.6");
                }
            }
        }
        catch (Exception)
        {
            // Log error nếu cần, nhưng vẫn trả về sitemap (có thể empty)
        }

        xml.AppendLine("</urlset>");

        return Content(xml.ToString(), "application/xml", Encoding.UTF8);
    }

    /// <summary>
    /// GET /sitemap-schools.xml - Sitemap cho trường đối tác
    /// </summary>
    [Route("sitemap-schools.xml")]
    public async Task<IActionResult> SchoolsSitemap()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var xml = new StringBuilder();

        xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xml.AppendLine($"<urlset xmlns=\"{SitemapNamespace}\">");

        try
        {
            // Lấy tất cả trường đối tác
            var filter = new NVCMS.WebView.Data.ViewModels.TruongSearchFilterViewModel
            {
                IsPartner = true,
                Page = 1,
                PageSize = 10000
            };

            var schools = await _truongService.SearchAsync(filter);

            foreach (var school in schools.Items)
            {
                var schoolUrl = $"{baseUrl}{school.DetailUrl}";
                AddUrl(xml, schoolUrl, DateTime.UtcNow, "monthly", "0.8");
            }

            // Thêm các trang quốc gia
            var countries = await _truongService.GetCountriesAsync();
            var countrySlugMap = new Dictionary<int, string>
            {
                { 38, "my" },      // USA
                { 1, "uc" },       // Australia
                { 3, "canada" },
                { 28, "anh" },     // UK
                { 401, "ireland" },
                { 23, "thuy-si" }, // Switzerland
                { 99, "new-zealand" }
            };

            foreach (var country in countries)
            {
                if (country.TruongCount > 0 && countrySlugMap.TryGetValue(country.Id, out var slug))
                {
                    var countryUrl = $"{baseUrl}/truong-doi-tac/{slug}";
                    AddUrl(xml, countryUrl, DateTime.UtcNow, "weekly", "0.7");
                }
            }
        }
        catch (Exception)
        {
            // Log error nếu cần, nhưng vẫn trả về sitemap (có thể empty)
        }

        xml.AppendLine("</urlset>");

        return Content(xml.ToString(), "application/xml", Encoding.UTF8);
    }

    /// <summary>
    /// GET /sitemap-events.xml - Sitemap cho sự kiện
    /// </summary>
    [Route("sitemap-events.xml")]
    public async Task<IActionResult> EventsSitemap()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var xml = new StringBuilder();

        xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xml.AppendLine($"<urlset xmlns=\"{SitemapNamespace}\">");

        try
        {
            // Lấy tất cả sự kiện (bao gồm cả upcoming và past)
            var allEvents = await _eventsService.GetAllCatsWithEventsAsync();

            foreach (var evt in allEvents)
            {
                if (!string.IsNullOrEmpty(evt.Slug))
                {
                    var eventUrl = $"{baseUrl}/su-kien/{evt.Slug}-{evt.Id}";
                    var lastmod = evt.FromDate ?? DateTime.UtcNow;

                    // Sự kiện sắp diễn ra có priority cao hơn
                    var isUpcoming = evt.EndDate.HasValue && evt.EndDate.Value >= DateTime.Now;
                    var priority = isUpcoming ? "0.8" : "0.6";
                    var changefreq = isUpcoming ? "daily" : "monthly";

                    AddUrl(xml, eventUrl, lastmod, changefreq, priority);
                }
            }
        }
        catch (Exception)
        {
            // Log error nếu cần, nhưng vẫn trả về sitemap (có thể empty)
        }

        xml.AppendLine("</urlset>");

        return Content(xml.ToString(), "application/xml", Encoding.UTF8);
    }

    /// <summary>
    /// Escape XML special characters
    /// </summary>
    private static string XmlEscape(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
    }
}
