using System.Text.Json;
using Capstone.Content.Models;
using Microsoft.Extensions.Logging;

namespace Capstone.Content.Json;

/// <summary>
/// Serves the JSON files in MockData/ with the same filtering, sorting and paging rules
/// the real data source must follow. Used by:
/// - the API when Content:DataSource = "Json" (and for CMS blocks not yet in the database),
/// - the website when ContentApi:UseMock = true (front-end work without the API running).
/// Files are re-read automatically when they change on disk, so editors can tweak content live.
/// </summary>
public sealed class JsonContentStore : IContentApi
{
    private readonly string _dir;
    private readonly ILogger<JsonContentStore> _logger;
    private readonly object _gate = new();
    private Store? _store;
    private DateTime _stamp;

    public JsonContentStore(string directory, ILogger<JsonContentStore> logger)
    {
        _dir = directory;
        _logger = logger;
    }

    /// <summary>
    /// Finds the MockData folder: an explicit path, then {contentRoot}/MockData,
    /// then {contentRoot}/../MockData (shared folder in the solution), then the build output.
    /// </summary>
    public static string ResolveDirectory(string contentRoot, string? configuredPath = null)
    {
        var candidates = new List<string>();
        if (!string.IsNullOrWhiteSpace(configuredPath))
            candidates.Add(Path.IsPathRooted(configuredPath) ? configuredPath : Path.Combine(contentRoot, configuredPath));
        candidates.Add(Path.Combine(contentRoot, "MockData"));
        candidates.Add(Path.Combine(contentRoot, "..", "MockData"));
        candidates.Add(Path.Combine(AppContext.BaseDirectory, "MockData"));
        return Path.GetFullPath(candidates.FirstOrDefault(Directory.Exists) ?? candidates[^1]);
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
        List<PageDto> Pages,
        List<DestinationDto> Destinations,
        List<TeamMemberDto> Team,
        List<CategoryDto> Categories);

    private Store S
    {
        get
        {
            var stamp = LatestWrite();
            lock (_gate)
            {
                if (_store is null || stamp > _stamp)
                {
                    _store = Load();
                    _stamp = stamp;
                }
                return _store;
            }
        }
    }

    private DateTime LatestWrite()
    {
        try
        {
            return Directory.Exists(_dir)
                ? Directory.EnumerateFiles(_dir, "*.json").Select(File.GetLastWriteTimeUtc).DefaultIfEmpty().Max()
                : DateTime.MinValue;
        }
        catch (IOException)
        {
            return _stamp;
        }
    }

    private Store Load()
    {
        T Read<T>(string file, T fallback)
        {
            var path = Path.Combine(_dir, file);
            if (!File.Exists(path))
            {
                _logger.LogWarning("Mock data file missing: {Path}", path);
                return fallback;
            }
            try
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(path), ContentJson.Options) ?? fallback;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Invalid JSON in {Path}", path);
                return fallback;
            }
        }

        _logger.LogInformation("Loading content from {Dir}", _dir);
        return new Store(
            Read("site.json", new SiteDto()),
            Read("home.json", new HomeContentDto()),
            Read<List<CountryDto>>("countries.json", []),
            Read<List<SchoolDetailDto>>("schools.json", []),
            Read<List<ServiceDto>>("services.json", []),
            Read<List<EventDetailDto>>("events.json", []),
            Read<List<NewsDetailDto>>("news.json", []),
            Read<List<TestimonialDto>>("testimonials.json", []),
            Read<List<PageDto>>("pages.json", []),
            Read<List<DestinationDto>>("destinations.json", []),
            Read<List<TeamMemberDto>>("team.json", []),
            Read<List<CategoryDto>>("news-categories.json", []));
    }

    // ---------- Site & pages ----------

    public Task<SiteDto> GetSiteAsync(CancellationToken ct = default) => Task.FromResult(S.Site);

    public Task<HomeContentDto> GetHomeAsync(CancellationToken ct = default) => Task.FromResult(S.Home);

    public Task<PageDto?> GetPageAsync(string slug, CancellationToken ct = default)
        => Task.FromResult(S.Pages.FirstOrDefault(p => Eq(p.Slug, slug)));

    // ---------- Schools ----------

    /// <summary>Counts come from schools.json so the sample data stays consistent.</summary>
    public Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken ct = default)
    {
        var store = S;
        return Task.FromResult<IReadOnlyList<CountryDto>>(store.Countries
            .Select(c => new CountryDto
            {
                Slug = c.Slug, Name = c.Name, FlagUrl = c.FlagUrl,
                SchoolCount = store.Schools.Count > 0 ? store.Schools.Count(s => Eq(s.CountrySlug, c.Slug)) : c.SchoolCount
            })
            .Where(c => c.SchoolCount > 0 || store.Schools.Count == 0)
            .ToList());
    }

    public Task<PagedResult<SchoolDto>> GetSchoolsAsync(SchoolQuery q, CancellationToken ct = default)
    {
        IEnumerable<SchoolDto> items = S.Schools;
        if (!string.IsNullOrWhiteSpace(q.Keyword))
            items = items.Where(s => Contains(s.Name, q.Keyword) || Contains(s.City, q.Keyword));
        if (!string.IsNullOrWhiteSpace(q.Country))
            items = items.Where(s => Eq(s.CountrySlug, q.Country));
        if (!string.IsNullOrWhiteSpace(q.Level))
            items = items.Where(s => Eq(s.Level, q.Level));
        // Featured: in the JSON store, the first schools of each destination in file order.
        if (q.Featured == true)
            return Task.FromResult(Page(items.GroupBy(s => s.CountrySlug).SelectMany(g => g.Take(2)).Concat(items).Distinct().Select(Summary), q.Page, q.PageSize));

        return Task.FromResult(Page(items.OrderBy(s => s.Name).Select(Summary), q.Page, q.PageSize));
    }

    public Task<SchoolDetailDto?> GetSchoolAsync(int id, CancellationToken ct = default)
    {
        var store = S;
        var school = store.Schools.FirstOrDefault(s => s.Id == id);
        if (school is null) return Task.FromResult<SchoolDetailDto?>(null);

        var copy = Clone(school);
        copy.Related = store.Schools.Where(s => s.CountrySlug == school.CountrySlug && s.Id != id).Take(3).Select(Summary).ToList();
        copy.UpcomingEvents = store.Events
            .Where(e => End(e) >= DateTimeOffset.Now && (e.Type == EventTypes.Fair || e.Schools.Any(s => s.Id == id)))
            .OrderBy(e => e.StartAt).Take(2).Select(Summary).ToList();
        return Task.FromResult<SchoolDetailDto?>(copy);
    }

    // ---------- Services ----------

    public Task<IReadOnlyList<ServiceDto>> GetServicesAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<ServiceDto>>(S.Services.OrderBy(s => s.Order).ToList());

    public Task<ServiceDto?> GetServiceAsync(string slug, CancellationToken ct = default)
        => Task.FromResult(S.Services.FirstOrDefault(s => Eq(s.Slug, slug)));

    // ---------- Events ----------

    public Task<PagedResult<EventDto>> GetEventsAsync(EventQuery q, CancellationToken ct = default)
    {
        var now = DateTimeOffset.Now;
        IEnumerable<EventDto> items = S.Events;
        if (!string.IsNullOrWhiteSpace(q.Type))
            items = items.Where(e => Eq(e.Type, q.Type));

        items = Eq(q.When, "past")
            ? items.Where(e => End(e) < now).OrderByDescending(e => e.StartAt)
            : items.Where(e => End(e) >= now).OrderBy(e => e.StartAt);

        return Task.FromResult(Page(items.Select(Summary), q.Page, q.PageSize));
    }

    public Task<EventDetailDto?> GetEventAsync(int id, CancellationToken ct = default)
    {
        var store = S;
        var ev = store.Events.FirstOrDefault(e => e.Id == id);
        if (ev is null) return Task.FromResult<EventDetailDto?>(null);

        var copy = Clone(ev);
        if (copy.Schools.Count == 0 && copy.Type == EventTypes.Fair)
            copy.Schools = store.Schools.Take(Math.Min(copy.SchoolCount, 12)).Select(Summary).ToList();
        return Task.FromResult<EventDetailDto?>(copy);
    }

    // ---------- News ----------

    /// <summary>news-categories.json defines names, groups and order; counts come from news.json.</summary>
    public Task<IReadOnlyList<CategoryDto>> GetNewsCategoriesAsync(CancellationToken ct = default)
    {
        var store = S;
        var defined = store.Categories.Count > 0
            ? store.Categories
            : store.News.Where(n => !string.IsNullOrEmpty(n.CategorySlug)).GroupBy(n => n.CategorySlug!)
                .Select(g => new CategoryDto { Slug = g.Key, Name = g.First().CategoryName ?? g.Key }).ToList();
        return Task.FromResult<IReadOnlyList<CategoryDto>>(defined
            .Select(c => new CategoryDto
            {
                Slug = c.Slug, Name = c.Name, Group = c.Group,
                Count = store.News.Count(n => Eq(n.CategorySlug, c.Slug))
            })
            .ToList());
    }

    public Task<PagedResult<NewsDto>> GetNewsAsync(NewsQuery q, CancellationToken ct = default)
    {
        IEnumerable<NewsDetailDto> items = S.News;
        if (!string.IsNullOrWhiteSpace(q.Category))
            items = items.Where(n => Eq(n.CategorySlug, q.Category));
        if (!string.IsNullOrWhiteSpace(q.Country))
            items = items.Where(n => Eq(n.CountrySlug, q.Country));
        return Task.FromResult(Page(items.OrderByDescending(n => n.PublishedAt).Select(Summary), q.Page, q.PageSize));
    }

    public Task<NewsDetailDto?> GetNewsArticleAsync(int id, CancellationToken ct = default)
    {
        var store = S;
        var article = store.News.FirstOrDefault(n => n.Id == id);
        if (article is null) return Task.FromResult<NewsDetailDto?>(null);

        var copy = Clone(article);
        copy.Related = store.News
            .Where(n => n.Id != id)
            .OrderByDescending(n => n.CategorySlug == article.CategorySlug)
            .ThenByDescending(n => n.PublishedAt)
            .Take(3).Select(Summary).ToList();
        return Task.FromResult<NewsDetailDto?>(copy);
    }

    // ---------- Misc ----------

    public Task<IReadOnlyList<TestimonialDto>> GetTestimonialsAsync(int take = 4, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<TestimonialDto>>(S.Testimonials.Take(Math.Clamp(take, 1, 50)).ToList());

    public Task<IReadOnlyList<DestinationDto>> GetDestinationsAsync(CancellationToken ct = default)
    {
        var store = S;
        return Task.FromResult<IReadOnlyList<DestinationDto>>(store.Destinations
            .OrderBy(d => d.Order)
            .Select(d => WithCount(d, store, includeBody: false))
            .ToList());
    }

    public Task<DestinationDto?> GetDestinationAsync(string slug, CancellationToken ct = default)
    {
        var store = S;
        var d = store.Destinations.FirstOrDefault(x => Eq(x.Slug, slug));
        return Task.FromResult(d is null ? null : WithCount(d, store, includeBody: true));
    }

    private static DestinationDto WithCount(DestinationDto d, Store store, bool includeBody)
    {
        var copy = Clone(d);
        copy.SchoolCount = store.Schools.Count(s => Eq(s.CountrySlug, d.CountrySlug));
        if (!includeBody) copy.BodyHtml = null;
        return copy;
    }

    public Task<IReadOnlyList<TeamMemberDto>> GetTeamAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<TeamMemberDto>>(S.Team.OrderBy(t => t.Order).ToList());

    public Task<IReadOnlyList<PageDto>> GetPagesAsync(string section, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<PageDto>>(S.Pages
            .Where(p => Eq(p.Section, section))
            .OrderBy(p => p.Order)
            .Select(p => new PageDto { Slug = p.Slug, Section = p.Section, Order = p.Order, Title = p.Title, Summary = p.Summary })
            .ToList());

    public Task<LeadResult> SubmitLeadAsync(LeadRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation("[JSON store] Lead received: {Role} {Email} interest={Interest} utm={Utm}",
            request.Role, request.Email, request.Interest, request.UtmSource);
        return Task.FromResult(new LeadResult { Success = true });
    }

    // ---------- Projections: list endpoints return summaries, not detail payloads ----------

    public static SchoolDto Summary(SchoolDto s) => new()
    {
        Id = s.Id, Slug = s.Slug, Name = s.Name, LogoUrl = s.LogoUrl, CoverUrl = s.CoverUrl,
        CountrySlug = s.CountrySlug, CountryName = s.CountryName, FlagUrl = s.FlagUrl,
        City = s.City, Level = s.Level, IsPartner = s.IsPartner, Summary = s.Summary
    };

    public static EventDto Summary(EventDto e) => new()
    {
        Id = e.Id, Slug = e.Slug, Type = e.Type, IsFeatured = e.IsFeatured, Title = e.Title, Summary = e.Summary,
        CoverUrl = e.CoverUrl, StartAt = e.StartAt, EndAt = e.EndAt, City = e.City, Venue = e.Venue,
        IsOnline = e.IsOnline, SchoolCount = e.SchoolCount, StudentRegistrationUrl = e.StudentRegistrationUrl,
        ExhibitorRegistrationUrl = e.ExhibitorRegistrationUrl, Sessions = e.Sessions
    };

    public static NewsDto Summary(NewsDto n) => new()
    {
        Id = n.Id, Slug = n.Slug, Title = n.Title, Summary = n.Summary, ImageUrl = n.ImageUrl,
        CategoryName = n.CategoryName, CategorySlug = n.CategorySlug, CountrySlug = n.CountrySlug,
        PublishedAt = n.PublishedAt, Author = n.Author
    };

    // ---------- helpers ----------

    private static T Clone<T>(T value) =>
        JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, ContentJson.Options), ContentJson.Options)!;

    private static DateTimeOffset End(EventDto e) => e.EndAt ?? e.StartAt;

    private static bool Eq(string? a, string? b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    private static bool Contains(string? text, string term) =>
        text is not null && text.Contains(term.Trim(), StringComparison.OrdinalIgnoreCase);

    public static PagedResult<T> Page<T>(IEnumerable<T> source, int page, int pageSize)
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
