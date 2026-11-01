using Capstone.ApiEN.Infrastructure;
using Capstone.ApiEN.Leads;
using Capstone.Content;
using Capstone.Content.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace Capstone.ApiEN.Endpoints;

/// <summary>
/// /api/v1/en/* — the contract consumed by Capstone.ViewEN (see README, section 3).
/// GET responses are output-cached (tag "content"); POST /admin/cache/clear empties the cache after a CMS edit.
/// </summary>
public static class ContentEndpoints
{
    public const string CacheTag = "content";
    public const string CachePolicy = "content";

    public static IEndpointRouteBuilder MapContentEndpoints(this IEndpointRouteBuilder app)
    {
        // Two groups on the same prefix: cached reads, uncached writes.
        var read = app.MapGroup("/api/v1/en")
            .AddEndpointFilter<ApiKeyFilter>()
            .WithTags("Content EN")
            .CacheOutput(CachePolicy);

        var api = app.MapGroup("/api/v1/en")
            .AddEndpointFilter<ApiKeyFilter>()
            .WithTags("Content EN");

        // ---- Site & CMS blocks
        read.MapGet("/site", (IContentApi c, CancellationToken ct) => c.GetSiteAsync(ct))
            .WithSummary("Menu, offices, socials, hotline, stats, leader quote, tracking code");
        read.MapGet("/home", (IContentApi c, CancellationToken ct) => c.GetHomeAsync(ct))
            .WithSummary("Editable home page blocks");
        read.MapGet("/pages/{slug}", GetPage).WithSummary("Text page (about, institutions, privacy…)");
        read.MapGet("/pages", (IContentApi c, string section, CancellationToken ct) => c.GetPagesAsync(section, ct))
            .WithSummary("Pages of a section (e.g. section=about) for side menus");
        read.MapGet("/team", (IContentApi c, CancellationToken ct) => c.GetTeamAsync(ct)).WithSummary("Our team");

        // ---- Study destinations
        read.MapGet("/destinations", (IContentApi c, CancellationToken ct) => c.GetDestinationsAsync(ct))
            .WithSummary("Study in the USA, Canada, UK, Australia, other countries");
        read.MapGet("/destinations/{slug}", GetDestination);

        // ---- Schools
        read.MapGet("/countries", (IContentApi c, CancellationToken ct) => c.GetCountriesAsync(ct))
            .WithSummary("Destinations with partner-school counts");
        read.MapGet("/schools", GetSchools).WithSummary("Partner schools, filtered and paged");
        read.MapGet("/schools/{id:int}", GetSchool).WithSummary("School detail with related schools and upcoming events");

        // ---- Services
        read.MapGet("/services", (IContentApi c, CancellationToken ct) => c.GetServicesAsync(ct));
        read.MapGet("/services/{slug}", GetService);

        // ---- Events & fairs
        read.MapGet("/events", GetEvents).WithSummary("when=upcoming|past, type=fair|info-session");
        read.MapGet("/events/{id:int}", GetEvent).WithSummary("Event detail with sessions per city and participating schools");

        // ---- News
        read.MapGet("/news/categories", (IContentApi c, CancellationToken ct) => c.GetNewsCategoriesAsync(ct));
        read.MapGet("/news", GetNews);
        read.MapGet("/news/{id:int}", GetArticle);

        // ---- Testimonials
        read.MapGet("/testimonials", (IContentApi c, int? take, CancellationToken ct) => c.GetTestimonialsAsync(Math.Clamp(take ?? 4, 1, 50), ct));

        // ---- Leads (never cached, rate limited per IP)
        api.MapPost("/leads", PostLead)
            .RequireRateLimiting("leads")
            .WithSummary("Consultation or partnership enquiry from the EN site");

        // ---- Cache control for editors / deploy scripts
        api.MapPost("/admin/cache/clear", async (IOutputCacheStore store, CancellationToken ct) =>
        {
            await store.EvictByTagAsync(CacheTag, ct);
            return TypedResults.NoContent();
        }).WithSummary("Clear cached GET responses after content changes");

        return app;
    }

    private static async Task<Results<Ok<PageDto>, NotFound>> GetPage(string slug, IContentApi c, CancellationToken ct)
        => await c.GetPageAsync(slug, ct) is { } page ? TypedResults.Ok(page) : TypedResults.NotFound();

    private static async Task<Results<Ok<DestinationDto>, NotFound>> GetDestination(string slug, IContentApi c, CancellationToken ct)
        => await c.GetDestinationAsync(slug, ct) is { } d ? TypedResults.Ok(d) : TypedResults.NotFound();

    private static Task<PagedResult<SchoolDto>> GetSchools(
        IContentApi c, CancellationToken ct,
        string? q = null, string? country = null, string? level = null, bool? featured = null, int? page = null, int? pageSize = null)
        => c.GetSchoolsAsync(new SchoolQuery
        {
            Keyword = Trim(q, 100),
            Country = Trim(country, 50),
            Level = Trim(level, 50),
            Featured = featured,
            Page = Math.Max(1, page ?? 1),
            PageSize = Math.Clamp(pageSize ?? 12, 1, 100)
        }, ct);

    private static async Task<Results<Ok<SchoolDetailDto>, NotFound>> GetSchool(int id, IContentApi c, CancellationToken ct)
        => await c.GetSchoolAsync(id, ct) is { } school ? TypedResults.Ok(school) : TypedResults.NotFound();

    private static async Task<Results<Ok<ServiceDto>, NotFound>> GetService(string slug, IContentApi c, CancellationToken ct)
        => await c.GetServiceAsync(slug, ct) is { } service ? TypedResults.Ok(service) : TypedResults.NotFound();

    private static Task<PagedResult<EventDto>> GetEvents(
        IContentApi c, CancellationToken ct,
        string? type = null, string? when = null, int? page = null, int? pageSize = null)
        => c.GetEventsAsync(new EventQuery
        {
            Type = Trim(type, 30),
            When = string.Equals(when, "past", StringComparison.OrdinalIgnoreCase) ? "past" : "upcoming",
            Page = Math.Max(1, page ?? 1),
            PageSize = Math.Clamp(pageSize ?? 10, 1, 100)
        }, ct);

    private static async Task<Results<Ok<EventDetailDto>, NotFound>> GetEvent(int id, IContentApi c, CancellationToken ct)
        => await c.GetEventAsync(id, ct) is { } ev ? TypedResults.Ok(ev) : TypedResults.NotFound();

    private static Task<PagedResult<NewsDto>> GetNews(
        IContentApi c, CancellationToken ct,
        string? category = null, string? country = null, int? page = null, int? pageSize = null)
        => c.GetNewsAsync(new NewsQuery
        {
            Category = Trim(category, 80),
            Country = Trim(country, 50),
            Page = Math.Max(1, page ?? 1),
            PageSize = Math.Clamp(pageSize ?? 9, 1, 100)
        }, ct);

    private static async Task<Results<Ok<NewsDetailDto>, NotFound>> GetArticle(int id, IContentApi c, CancellationToken ct)
        => await c.GetNewsArticleAsync(id, ct) is { } article ? TypedResults.Ok(article) : TypedResults.NotFound();

    private static async Task<Results<Ok<LeadResult>, ValidationProblem>> PostLead(
        LeadRequest lead, ILeadStore store, HttpContext http, CancellationToken ct)
    {
        var errors = LeadValidator.Validate(lead);
        if (errors.Count > 0)
            return TypedResults.ValidationProblem(errors);

        lead.Language = "en";
        lead.SourceUrl ??= http.Request.Headers.Referer.ToString();
        var result = await store.SaveAsync(lead, ct);
        return TypedResults.Ok(result);
    }

    private static string? Trim(string? value, int max)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        value = value.Trim();
        return value.Length > max ? value[..max] : value;
    }
}
