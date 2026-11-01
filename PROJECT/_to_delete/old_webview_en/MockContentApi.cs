using System.Text.Json;
using Capstone.ViewEN.Models;

namespace Capstone.ViewEN.Services;

/// <summary>
/// Serves MockData/*.json with the same filtering and paging rules the real API must follow.
/// Turn on with ContentApi:UseMock = true. The JSON files are sample data, not production content.
/// </summary>
public sealed class MockContentApi : IContentApi
{
    private readonly string _dir;
    private readonly ILogger<MockContentApi> _logger;
    private readonly Lazy<Store> _store;

    public MockContentApi(IWebHostEnvironment env, ILogger<MockContentApi> logger)
    {
        _dir = Path.Combine(env.ContentRootPath, "MockData");
        if (!Directory.Exists(_dir))
            _dir = Path.Combine(AppContext.BaseDirectory, "MockData");
        _logger = logger;
        _store = new Lazy<Store>(Load, isThreadSafe: true);
    }

    private sealed record Store(
        SiteDto Site,
        HomeContentDto Home,
        List<CountryDto> Countries,
        List<SchoolDetailDto> Schools,
        List<ServiceDto> Services,
        List<EventDetailDto> Events,
        List<NewsDetailDto> News,
        List<TestimonialDto> Testimonials,
        List<PageDto> Pages);

    private Store Load()
    {
        T Read<T>(string file, T fallback)
        {
            var path = Path.Combine(_dir, file);
            if (!File.Exists(path))
            {
                _logger.LogWarning("Mock file missing: {Path}", path);
                return fallback;
            }
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path), ContentApiClient.Json) ?? fallback;
        }

        return new Store(
            Read("site.json", new SiteDto()),
            Read("home.json", new HomeContentDto()),
            Read<List<CountryDto>>("countries.json", []),
            Read<List<SchoolDetailDto>>("schools.json", []),
            Read<List<ServiceDto>>("services.json", []),
            Read<List<EventDetailDto>>("events.json", []),
            Read<List<NewsDetailDto>>("news.json", []),
            Read<List<TestimonialDto>>("testimonials.json", []),
            Read<List<PageDto>>("pages.json", []));
    }

    private Store S => _store.Value;

    public Task<SiteDto> GetSiteAsync(CancellationToken ct = default) => Task.FromResult(S.Site);

    public Task<HomeContentDto> GetHomeAsync(CancellationToken ct = default) => Task.FromResult(S.Home);

    public Task<PageDto?> GetPageAsync(string slug, CancellationToken ct = default)
        => Task.FromResult(S.Pages.FirstOrDefault(p => Eq(p.Slug, slug)));

    public Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<CountryDto>>(S.Countries);

    public Task<PagedResult<SchoolDto>> GetSchoolsAsync(SchoolQuery q, CancellationToken ct = default)
    {
        IEnumerable<SchoolDto> items = S.Schools;
        if (!string.IsNullOrWhiteSpace(q.Keyword))
            items = items.Where(s => Contains(s.Name, q.Keyword) || Contains(s.City, q.Keyword));
        if (!string.IsNullOrWhiteSpace(q.Country))
            items = items.Where(s => Eq(s.CountrySlug, q.Country));
        if (!string.IsNullOrWhiteSpace(q.Level))
            items = items.Where(s => Eq(s.Level, q.Level));

        return Task.FromResult(Page(items.OrderBy(s => s.Name), q.Page, q.PageSize));
    }

    public Task<SchoolDetailDto?> GetSchoolAsync(int id, CancellationToken ct = default)
    {
        var school = S.Schools.FirstOrDefault(s => s.Id == id);
        if (school is not null)
        {
            school.Related = S.Schools.Where(s => s.CountrySlug == school.CountrySlug && s.Id != id).Take(3).Cast<SchoolDto>().ToList();
            school.UpcomingEvents = S.Events.Where(e => e.Type == EventTypes.Fair && End(e) >= DateTimeOffset.Now).Take(2).Cast<EventDto>().ToList();
        }
        return Task.FromResult(school);
    }

    public Task<IReadOnlyList<ServiceDto>> GetServicesAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<ServiceDto>>(S.Services.OrderBy(s => s.Order).ToList());

    public Task<ServiceDto?> GetServiceAsync(string slug, CancellationToken ct = default)
        => Task.FromResult(S.Services.FirstOrDefault(s => Eq(s.Slug, slug)));

    public Task<PagedResult<EventDto>> GetEventsAsync(EventQuery q, CancellationToken ct = default)
    {
        var now = DateTimeOffset.Now;
        IEnumerable<EventDto> items = S.Events;
        if (!string.IsNullOrWhiteSpace(q.Type))
            items = items.Where(e => Eq(e.Type, q.Type));

        items = Eq(q.When, "past")
            ? items.Where(e => End(e) < now).OrderByDescending(e => e.StartAt)
            : items.Where(e => End(e) >= now).OrderBy(e => e.StartAt);

        return Task.FromResult(Page(items, q.Page, q.PageSize));
    }

    public Task<EventDetailDto?> GetEventAsync(int id, CancellationToken ct = default)
    {
        var ev = S.Events.FirstOrDefault(e => e.Id == id);
        if (ev is not null && ev.Schools.Count == 0 && ev.Type == EventTypes.Fair)
            ev.Schools = S.Schools.Take(Math.Min(ev.SchoolCount, 12)).Cast<SchoolDto>().ToList();
        return Task.FromResult(ev);
    }

    public Task<IReadOnlyList<CategoryDto>> GetNewsCategoriesAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<CategoryDto>>(S.News
            .Where(n => !string.IsNullOrEmpty(n.CategorySlug))
            .GroupBy(n => n.CategorySlug!)
            .Select(g => new CategoryDto { Slug = g.Key, Name = g.First().CategoryName ?? g.Key })
            .ToList());

    public Task<PagedResult<NewsDto>> GetNewsAsync(NewsQuery q, CancellationToken ct = default)
    {
        IEnumerable<NewsDto> items = S.News;
        if (!string.IsNullOrWhiteSpace(q.Category))
            items = items.Where(n => Eq(n.CategorySlug, q.Category));
        return Task.FromResult(Page(items.OrderByDescending(n => n.PublishedAt), q.Page, q.PageSize));
    }

    public Task<NewsDetailDto?> GetNewsArticleAsync(int id, CancellationToken ct = default)
    {
        var article = S.News.FirstOrDefault(n => n.Id == id);
        if (article is not null)
            article.Related = S.News.Where(n => n.Id != id).OrderByDescending(n => n.PublishedAt).Take(3).Cast<NewsDto>().ToList();
        return Task.FromResult(article);
    }

    public Task<IReadOnlyList<TestimonialDto>> GetTestimonialsAsync(int take = 4, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<TestimonialDto>>(S.Testimonials.Take(take).ToList());

    public Task<LeadResult> SubmitLeadAsync(LeadRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation("[Mock] Lead received: {Role} {Email} interest={Interest} utm={Utm}",
            request.Role, request.Email, request.Interest, request.UtmSource);
        return Task.FromResult(new LeadResult { Success = true });
    }

    // ---------- helpers ----------

    private static DateTimeOffset End(EventDto e) => e.EndAt ?? e.StartAt;

    private static bool Eq(string? a, string? b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    private static bool Contains(string? text, string term) =>
        text is not null && text.Contains(term.Trim(), StringComparison.OrdinalIgnoreCase);

    private static PagedResult<T> Page<T>(IEnumerable<T> source, int page, int pageSize)
    {
        var list = source.ToList();
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        return new PagedResult<T>
        {
            Items = list.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
            Total = list.Count,
            Page = page,
            PageSize = pageSize
        };
    }
}
