using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Capstone.Content;
using Capstone.Content.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Capstone.ViewEN.Services;

/// <summary>
/// HTTP implementation of <see cref="IContentApi"/>. Endpoints are relative to ContentApi:BaseUrl.
/// The full contract (routes, query strings, sample JSON) is documented in README.md and MockData/.
///
/// Error policy
/// - Lists and page blocks: on failure log and return an empty result, so one broken
///   endpoint never takes the whole page down.
/// - Detail endpoints: 404 → null (page shows "not found"); other failures throw → error page.
/// </summary>
public sealed class ContentApiClient(
    HttpClient http,
    IMemoryCache cache,
    IOptions<ContentApiOptions> options,
    ILogger<ContentApiClient> logger) : IContentApi
{
    private static readonly JsonSerializerOptions Json = ContentJson.Options;

    private readonly TimeSpan _ttl = TimeSpan.FromMinutes(Math.Max(0, options.Value.CacheMinutes));

    // ---------- Site & pages ----------

    public Task<SiteDto> GetSiteAsync(CancellationToken ct = default)
        => GetOrDefaultAsync("site", () => new SiteDto(), ct);

    public Task<HomeContentDto> GetHomeAsync(CancellationToken ct = default)
        => GetOrDefaultAsync("home", () => new HomeContentDto(), ct);

    public Task<PageDto?> GetPageAsync(string slug, CancellationToken ct = default)
        => GetAsync<PageDto>($"pages/{Uri.EscapeDataString(slug)}", ct);

    // ---------- Schools ----------

    public async Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken ct = default)
        => await GetOrDefaultAsync<List<CountryDto>>("countries", () => [], ct);

    public Task<PagedResult<SchoolDto>> GetSchoolsAsync(SchoolQuery q, CancellationToken ct = default)
        => GetOrDefaultAsync(
            "schools" + Qs(("q", q.Keyword), ("country", q.Country), ("level", q.Level), ("featured", q.Featured == true ? "true" : null), ("page", q.Page.ToString()), ("pageSize", q.PageSize.ToString())),
            () => PagedResult<SchoolDto>.Empty(q.Page, q.PageSize), ct);

    public Task<SchoolDetailDto?> GetSchoolAsync(int id, CancellationToken ct = default)
        => GetAsync<SchoolDetailDto>($"schools/{id}", ct);

    // ---------- Services ----------

    public async Task<IReadOnlyList<ServiceDto>> GetServicesAsync(CancellationToken ct = default)
        => await GetOrDefaultAsync<List<ServiceDto>>("services", () => [], ct);

    public Task<ServiceDto?> GetServiceAsync(string slug, CancellationToken ct = default)
        => GetAsync<ServiceDto>($"services/{Uri.EscapeDataString(slug)}", ct);

    // ---------- Events ----------

    public Task<PagedResult<EventDto>> GetEventsAsync(EventQuery q, CancellationToken ct = default)
        => GetOrDefaultAsync(
            "events" + Qs(("type", q.Type), ("when", q.When), ("page", q.Page.ToString()), ("pageSize", q.PageSize.ToString())),
            () => PagedResult<EventDto>.Empty(q.Page, q.PageSize), ct);

    public Task<EventDetailDto?> GetEventAsync(int id, CancellationToken ct = default)
        => GetAsync<EventDetailDto>($"events/{id}", ct);

    // ---------- News ----------

    public async Task<IReadOnlyList<CategoryDto>> GetNewsCategoriesAsync(CancellationToken ct = default)
        => await GetOrDefaultAsync<List<CategoryDto>>("news/categories", () => [], ct);

    public Task<PagedResult<NewsDto>> GetNewsAsync(NewsQuery q, CancellationToken ct = default)
        => GetOrDefaultAsync(
            "news" + Qs(("category", q.Category), ("country", q.Country), ("page", q.Page.ToString()), ("pageSize", q.PageSize.ToString())),
            () => PagedResult<NewsDto>.Empty(q.Page, q.PageSize), ct);

    public Task<NewsDetailDto?> GetNewsArticleAsync(int id, CancellationToken ct = default)
        => GetAsync<NewsDetailDto>($"news/{id}", ct);

    // ---------- Misc ----------

    public async Task<IReadOnlyList<TestimonialDto>> GetTestimonialsAsync(int take = 4, CancellationToken ct = default)
        => await GetOrDefaultAsync<List<TestimonialDto>>($"testimonials?take={take}", () => [], ct);

    public async Task<IReadOnlyList<DestinationDto>> GetDestinationsAsync(CancellationToken ct = default)
        => await GetOrDefaultAsync<List<DestinationDto>>("destinations", () => [], ct);

    public Task<DestinationDto?> GetDestinationAsync(string slug, CancellationToken ct = default)
        => GetAsync<DestinationDto>($"destinations/{Uri.EscapeDataString(slug)}", ct);

    public async Task<IReadOnlyList<TeamMemberDto>> GetTeamAsync(CancellationToken ct = default)
        => await GetOrDefaultAsync<List<TeamMemberDto>>("team", () => [], ct);

    public async Task<IReadOnlyList<PageDto>> GetPagesAsync(string section, CancellationToken ct = default)
        => await GetOrDefaultAsync<List<PageDto>>($"pages?section={Uri.EscapeDataString(section)}", () => [], ct);

    public async Task<LeadResult> SubmitLeadAsync(LeadRequest request, CancellationToken ct = default)
    {
        try
        {
            using var resp = await http.PostAsJsonAsync("leads", request, Json, ct);
            if (resp.IsSuccessStatusCode)
                return await resp.Content.ReadFromJsonAsync<LeadResult>(Json, ct) ?? new LeadResult { Success = true };

            logger.LogWarning("Lead submission rejected: {Status}", (int)resp.StatusCode);
            return new LeadResult { Success = false, Message = "We couldn't send your enquiry. Check the highlighted fields, or call our hotline." };
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogError(ex, "Lead submission failed");
            return new LeadResult { Success = false, Message = "Our server didn't respond. Your details weren't sent; please try again in a minute or call our hotline." };
        }
    }

    // ---------- Plumbing ----------

    private async Task<T?> GetAsync<T>(string url, CancellationToken ct) where T : class
    {
        var key = "capi:" + url;
        if (_ttl > TimeSpan.Zero && cache.TryGetValue(key, out T? cached))
            return cached;

        using var resp = await http.GetAsync(url, ct);
        if (resp.StatusCode == HttpStatusCode.NotFound)
            return null;
        resp.EnsureSuccessStatusCode();

        var data = await resp.Content.ReadFromJsonAsync<T>(Json, ct);
        if (data is not null && _ttl > TimeSpan.Zero)
            cache.Set(key, data, _ttl);
        return data;
    }

    private async Task<T> GetOrDefaultAsync<T>(string url, Func<T> fallback, CancellationToken ct) where T : class
    {
        try
        {
            return await GetAsync<T>(url, ct) ?? fallback();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogError(ex, "Content API call failed: {Url}", url);
            return fallback();
        }
    }

    private static string Qs(params (string Key, string? Value)[] pairs)
    {
        var parts = pairs
            .Where(p => !string.IsNullOrWhiteSpace(p.Value))
            .Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value!)}");
        var qs = string.Join('&', parts);
        return qs.Length == 0 ? string.Empty : "?" + qs;
    }
}
