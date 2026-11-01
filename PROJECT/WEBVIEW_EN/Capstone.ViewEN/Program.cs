using System.Text;
using System.Threading.RateLimiting;
using Capstone.ViewEN.Components;
using Capstone.ViewEN.Helpers;
using Capstone.Content;
using Capstone.Content.Models;
using Capstone.ViewEN.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Static server-side rendering only (no SignalR circuit, no WebAssembly):
// every page is plain HTML, fast for first load and fully indexable.
builder.Services.AddRazorComponents();

// Content: real API (typed HttpClient) or MockData, see ContentApi:UseMock.
builder.Services.AddContentApi(builder.Configuration);

builder.Services.AddResponseCompression(o =>
{
    o.EnableForHttps = true;
    o.Providers.Add<BrotliCompressionProvider>();
    o.Providers.Add<GzipCompressionProvider>();
});

// Per-visitor limit on form posts (consultation form). GET pages are not limited.
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    // Write a body so the status-code page middleware doesn't re-execute the POST.
    o.OnRejected = (ctx, ct) => new ValueTask(ctx.HttpContext.Response.WriteAsync(
        "Too many submissions from your connection. Wait a minute and try again, or call our hotline.", ct));
    o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        HttpMethods.IsPost(ctx.Request.Method)
            ? RateLimitPartition.GetFixedWindowLimiter(
                "post:" + (ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown"),
                _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 })
            : RateLimitPartition.GetNoLimiter("get"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseResponseCompression();
app.UseHttpsRedirection();

app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["X-Frame-Options"] = "SAMEORIGIN";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    await next();
});

app.UseRateLimiter();
app.UseAntiforgery();
app.MapStaticAssets();

// ── 301s from the old en.capstonevietnam.com (WebForms) URLs ─────────────────
var legacy = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    ["/default.aspx"] = "/",
    ["/about-capstone.html"] = "/about",
    ["/about-capstone/our-services.html"] = "/services",
    ["/about-capstone/vision-missions-and-values.html"] = "/about/vision-mission-values",
    ["/about-capstone/why-capstone.html"] = "/about/why-capstone",
    ["/about-capstone/careers.html"] = "/about/careers",
    ["/about-capstone/our-team.html"] = "/about/our-team",
    ["/our-partners.html"] = "/partners",
    ["/events.html"] = "/events",
    ["/events/education-fairs.html"] = "/events?type=fair",
    ["/events/info-sessions.html"] = "/events?type=info-session",
    ["/news.html"] = "/news",
    ["/news/general-information.html"] = "/news?category=general-information",
    ["/news/scholarships.html"] = "/news?category=scholarships",
    ["/contact.html"] = "/contact",
    ["/terms-of-service.html"] = "/terms",
    ["/privacy-policy.html"] = "/privacy",
    ["/overseas-study/study-in-the-usa.html"] = "/study-in/usa",
    ["/overseas-study/study-in-canada.html"] = "/study-in/canada",
    ["/overseas-study/study-in-uk.html"] = "/study-in/uk",
    ["/overseas-study/study-in-australia.html"] = "/study-in/australia",
    ["/overseas-study/study-in-other-countries.html"] = "/study-in/other-countries",
};
// /our-partners/study-in-us/high-school.html → /partners?country=united-states&level=High school
var partnerCountries = new Dictionary<string, string> { ["us"] = "united-states", ["canada"] = "canada", ["uk"] = "united-kingdom", ["australia"] = "australia" };
var partnerLevels = new Dictionary<string, string> { ["high-school"] = "High school", ["community-college"] = "Community college", ["university"] = "University", ["esl"] = "ESL" };
foreach (var (key, country) in partnerCountries)
{
    legacy[$"/our-partners/study-in-{key}.html"] = $"/partners?country={country}";
    foreach (var (levelKey, level) in partnerLevels)
        legacy[$"/our-partners/study-in-{key}/{levelKey}.html"] = $"/partners?country={country}&level={Uri.EscapeDataString(level)}";
}
foreach (var (from, to) in legacy)
    app.MapGet(from, () => Results.Redirect(to, permanent: true));

// Old article URLs end in "-{id}.html" under news-like folders (news/, events/, overseas-study/),
// e.g. /news/scholarships/safe-high-rank-…-5958.html. Article ids are kept → /news/{slug}-{id}.
// Two-segment ones (/news/x-5958.html) are caught by NewsDetail itself, see SlugId.
// Any other old *.html page goes to the closest section.
string[] articleFolders = ["news/", "events/", "overseas-study/"];
app.MapGet("{**path:regex(\\.html$)}", (string path) =>
{
    var file = path.Split('/').Last();
    var m = System.Text.RegularExpressions.Regex.Match(file, @"^(?<slug>.+)-(?<id>\d+)\.html$");
    var target = path.StartsWith("our-partners/", StringComparison.OrdinalIgnoreCase) ? "/partners"
        : m.Success && articleFolders.Any(f => path.StartsWith(f, StringComparison.OrdinalIgnoreCase))
            ? $"/news/{Uri.EscapeDataString(m.Groups["slug"].Value)}-{m.Groups["id"].Value}"
            : "/";
    return Results.Redirect(target, permanent: true);
});

// ── robots.txt & sitemap.xml (built from the content API) ────────────────────
app.MapGet("/robots.txt", (IOptions<SiteOptions> site) =>
    Results.Text($"User-agent: *\nAllow: /\nSitemap: {site.Value.BaseUrl.TrimEnd('/')}/sitemap.xml\n", "text/plain"));

app.MapGet("/sitemap.xml", async (IContentApi api, IOptions<SiteOptions> site, CancellationToken ct) =>
{
    var root = site.Value.BaseUrl.TrimEnd('/');
    var urls = new List<string> { "/", "/study-in", "/partners", "/services", "/events", "/news", "/institutions", "/about",
        "/about/vision-mission-values", "/about/why-capstone", "/about/our-team", "/about/careers", "/contact" };
    urls.AddRange((await api.GetDestinationsAsync(ct)).Select(d => $"/study-in/{d.Slug}"));

    var schools = await api.GetSchoolsAsync(new SchoolQuery { PageSize = 100 }, ct);
    var services = await api.GetServicesAsync(ct);
    var events = await api.GetEventsAsync(new EventQuery { PageSize = 100 }, ct);
    var news = await api.GetNewsAsync(new NewsQuery { PageSize = 100 }, ct);
    urls.AddRange(schools.Items.Select(Links.School));
    urls.AddRange(services.Select(Links.Service));
    urls.AddRange(events.Items.Select(Links.Event));
    urls.AddRange(news.Items.Select(Links.News));

    var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");
    foreach (var u in urls.Distinct())
        sb.Append("  <url><loc>").Append(System.Security.SecurityElement.Escape(root + u)).Append("</loc></url>\n");
    sb.Append("</urlset>");
    return Results.Text(sb.ToString(), "application/xml", Encoding.UTF8);
});

app.MapRazorComponents<App>();

app.Run();
