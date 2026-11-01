using System.Data;
using System.Text;
using Capstone.ApiEN.Infrastructure;
using Capstone.Content;
using Capstone.Content.Json;
using Capstone.Content.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Capstone.ApiEN.Data;

/// <summary>
/// Content from the existing SQL Server databases (Content:DataSource = "Sql").
/// - Schools, countries, events: CapstoneVietnam_old (ConnectionStrings:CrmDb), EN columns of Cap_Truong / NV_Events*.
/// - News: CapstoneVietNamWeb (ConnectionStrings:WebDb), EN news portal (Content:NewsPortalId).
/// - Site, home, pages, services, testimonials: no tables yet → read from MockData JSON (edit those files).
/// Queries reuse the stored procedures of the Vietnamese site where they exist.
/// </summary>
public sealed class SqlContentStore(
    JsonContentStore cms,
    IConfiguration config,
    IOptions<ContentSourceOptions> options,
    ILogger<SqlContentStore> logger) : IContentApi
{
    private readonly ContentSourceOptions _o = options.Value;
    private readonly FileUrls _files = new(options.Value.FileServerBaseUrl);
    private readonly string _crmDb = Required(config, "CrmDb");
    private readonly string _webDb = Required(config, "WebDb");

    internal static string Required(IConfiguration config, string name)
        => config.GetConnectionString(name) is { Length: > 0 } cs
            ? cs
            : throw new InvalidOperationException($"ConnectionStrings:{name} is required when Content:DataSource = Sql.");

    // Cap_Truong.Country = Cap_Location.LocationId (same table the Vietnamese site maps).
    private static readonly (int Id, string Slug, string Name, string? Flag)[] KnownCountries =
    [
        (38, "united-states", "United States", "/img/flags/us.svg"),
        (3, "canada", "Canada", "/img/flags/ca.svg"),
        (28, "united-kingdom", "United Kingdom", "/img/flags/gb.svg"),
        (1, "australia", "Australia", "/img/flags/au.svg"),
        (99, "new-zealand", "New Zealand", "/img/flags/nz.svg"),
        (23, "switzerland", "Switzerland", "/img/flags/ch.svg"),
        (401, "ireland", "Ireland", "/img/flags/ie.svg"),
    ];
    private const string OtherSlug = "other";
    private const string OtherName = "Other destinations";

    // ============================================================ CMS blocks (JSON for now)

    public Task<SiteDto> GetSiteAsync(CancellationToken ct = default) => cms.GetSiteAsync(ct);
    public Task<HomeContentDto> GetHomeAsync(CancellationToken ct = default) => cms.GetHomeAsync(ct);
    public Task<PageDto?> GetPageAsync(string slug, CancellationToken ct = default) => cms.GetPageAsync(slug, ct);
    public Task<IReadOnlyList<ServiceDto>> GetServicesAsync(CancellationToken ct = default) => cms.GetServicesAsync(ct);
    public Task<ServiceDto?> GetServiceAsync(string slug, CancellationToken ct = default) => cms.GetServiceAsync(slug, ct);
    public Task<IReadOnlyList<TestimonialDto>> GetTestimonialsAsync(int take = 4, CancellationToken ct = default) => cms.GetTestimonialsAsync(take, ct);
    public Task<LeadResult> SubmitLeadAsync(LeadRequest request, CancellationToken ct = default) => cms.SubmitLeadAsync(request, ct);
    public Task<IReadOnlyList<TeamMemberDto>> GetTeamAsync(CancellationToken ct = default) => cms.GetTeamAsync(ct);
    public Task<IReadOnlyList<PageDto>> GetPagesAsync(string section, CancellationToken ct = default) => cms.GetPagesAsync(section, ct);

    /// <summary>Destination texts come from JSON; school counts from the database.</summary>
    public async Task<IReadOnlyList<DestinationDto>> GetDestinationsAsync(CancellationToken ct = default)
    {
        var list = await cms.GetDestinationsAsync(ct);
        var counts = (await GetCountriesAsync(ct)).ToDictionary(c => c.Slug, c => c.SchoolCount, StringComparer.OrdinalIgnoreCase);
        foreach (var d in list) d.SchoolCount = counts.GetValueOrDefault(d.CountrySlug);
        return list;
    }

    public async Task<DestinationDto?> GetDestinationAsync(string slug, CancellationToken ct = default)
    {
        var d = await cms.GetDestinationAsync(slug, ct);
        if (d is null) return null;
        var counts = await GetCountriesAsync(ct);
        d.SchoolCount = counts.FirstOrDefault(c => string.Equals(c.Slug, d.CountrySlug, StringComparison.OrdinalIgnoreCase))?.SchoolCount ?? 0;
        return d;
    }

    // ============================================================ Countries & schools

    public async Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_crmDb);
        var rows = await conn.QueryAsync<CountryCountRow>(new CommandDefinition(
            "WebView_Truong_GetCountriesWithCount", new { IsPartner = true },
            commandType: CommandType.StoredProcedure, cancellationToken: ct));

        var counts = rows.ToDictionary(r => r.Id, r => r.TruongCount);
        var result = KnownCountries
            .Where(c => counts.GetValueOrDefault(c.Id) > 0)
            .Select(c => new CountryDto { Slug = c.Slug, Name = c.Name, FlagUrl = c.Flag, SchoolCount = counts[c.Id] })
            .OrderByDescending(c => c.SchoolCount)
            .ToList();

        var other = counts.Where(kv => KnownCountries.All(c => c.Id != kv.Key)).Sum(kv => kv.Value);
        if (other > 0)
            result.Add(new CountryDto { Slug = OtherSlug, Name = OtherName, SchoolCount = other });
        return result;
    }

    public async Task<PagedResult<SchoolDto>> GetSchoolsAsync(SchoolQuery q, CancellationToken ct = default)
    {
        var page = Math.Max(1, q.Page);
        var size = Math.Clamp(q.PageSize, 1, 100);
        await using var conn = new SqlConnection(_crmDb);

        if (q.Featured == true)
        {
            // Same SP as the Vietnamese home page.
            var featured = (await conn.QueryAsync<CapTruongRow>(new CommandDefinition(
                "WebView_Truong_RandomPartners", new { Count = size, PortalId = (int?)null },
                commandType: CommandType.StoredProcedure, cancellationToken: ct))).Select(MapSchool).ToList();
            return new PagedResult<SchoolDto> { Items = featured, Total = featured.Count, Page = 1, PageSize = size };
        }

        var where = new StringBuilder("t.Status = 1 AND t.isPartner = 1");
        var p = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(q.Keyword))
        {
            where.Append(" AND (t.NameofSchool LIKE @Kw OR t.ThanhPholongannhatEN LIKE @Kw OR t.ThanhPholongannhat LIKE @Kw)");
            p.Add("Kw", "%" + EscapeLike(q.Keyword.Trim()) + "%");
        }

        if (!string.IsNullOrWhiteSpace(q.Country))
        {
            if (string.Equals(q.Country, OtherSlug, StringComparison.OrdinalIgnoreCase))
            {
                where.Append(" AND (t.Country IS NULL OR t.Country NOT IN @MainIds)");
                p.Add("MainIds", KnownCountries.Select(c => c.Id).ToArray());
            }
            else
            {
                var country = KnownCountries.FirstOrDefault(c => string.Equals(c.Slug, q.Country, StringComparison.OrdinalIgnoreCase));
                if (country.Slug is null) return PagedResult<SchoolDto>.Empty(page, size);
                where.Append(" AND t.Country = @CountryId");
                p.Add("CountryId", country.Id);
            }
        }

        if (!string.IsNullOrWhiteSpace(q.Level))
        {
            var loaiIds = LoaiIdsFor(q.Level);
            if (loaiIds.Length == 0) return PagedResult<SchoolDto>.Empty(page, size);
            where.Append(" AND LTRIM(RTRIM(t.Loai)) IN @LoaiIds");
            p.Add("LoaiIds", loaiIds);
        }

        p.Add("Offset", (page - 1) * size);
        p.Add("Take", size);

        var sql = $"""
            SELECT COUNT(*) FROM Cap_Truong t WHERE {where};
            SELECT t.* FROM Cap_Truong t WHERE {where}
            ORDER BY t.NameofSchool
            OFFSET @Offset ROWS FETCH NEXT @Take ROWS ONLY;
            """;

        using var multi = await conn.QueryMultipleAsync(new CommandDefinition(sql, p, cancellationToken: ct));
        var total = await multi.ReadSingleAsync<int>();
        var items = (await multi.ReadAsync<CapTruongRow>()).Select(MapSchool).ToList();
        return new PagedResult<SchoolDto> { Items = items, Total = total, Page = page, PageSize = size };
    }

    public async Task<SchoolDetailDto?> GetSchoolAsync(int id, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_crmDb);
        var row = await conn.QueryFirstOrDefaultAsync<CapTruongRow>(new CommandDefinition(
            "WebView_Truong_GetById", new { Id = id },
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
        if (row is null) return null;

        var school = Fill(new SchoolDetailDto(), row);
        school.Founded = int.TryParse(row.Namthanhlap?.Trim(), out var year) && year is > 1000 and < 2100 ? year : null;
        school.Website = NormalizeUrl(row.Website);
        school.Accreditation = PlainText.FirstNonEmpty(row.KiemdinhEN);
        school.Programs = (row.ProgramOfered ?? string.Empty)
            .Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase).Take(30).ToList();
        school.DescriptionHtml = _files.Html(PlainText.FirstNonEmpty(row.DescreptionWebsiteEN, row.DescreptionEN));
        school.VideoUrl = YouTubeEmbed(row.VideoLink);

        var related = await conn.QueryAsync<CapTruongRow>(new CommandDefinition("""
            SELECT TOP (3) t.* FROM Cap_Truong t
            WHERE t.Status = 1 AND t.isPartner = 1 AND t.Country = @Country AND t.Id <> @Id
            ORDER BY NEWID()
            """, new { Country = row.Country, Id = id }, cancellationToken: ct));
        school.Related = related.Select(MapSchool).ToList();

        var pattern = $"%,{id},%";
        var cats = (await conn.QueryAsync<EventCatRow>(new CommandDefinition($"""
            SELECT TOP (3) c.* FROM NV_Events_Cat c
            WHERE {UpcomingCatFilter}
              AND (',' + REPLACE(ISNULL(c.FairSchool, ''), ' ', '') + ',' LIKE @Pattern
                   OR EXISTS (SELECT 1 FROM NV_Events e WHERE e.CatId = c.Id
                              AND ',' + REPLACE(ISNULL(e.School, ''), ' ', '') + ',' LIKE @Pattern))
            ORDER BY c.FromDate
            """, new { _o.EventsPortalId, Pattern = pattern }, cancellationToken: ct))).ToList();
        school.UpcomingEvents = await MapEventsAsync(conn, cats, ct);

        return school;
    }

    // ============================================================ Events

    private const string UpcomingCatFilter =
        "c.Isactive = 1 AND c.is_show_website = 1 AND c.PortalId = @EventsPortalId AND ISNULL(c.EndDate, c.FromDate) >= CAST(GETDATE() AS date)";
    private const string PastCatFilter =
        "c.Isactive = 1 AND c.is_show_website = 1 AND c.PortalId = @EventsPortalId AND ISNULL(c.EndDate, c.FromDate) < CAST(GETDATE() AS date)";

    public async Task<PagedResult<EventDto>> GetEventsAsync(EventQuery q, CancellationToken ct = default)
    {
        var past = string.Equals(q.When, "past", StringComparison.OrdinalIgnoreCase);
        var sql = past
            ? $"SELECT TOP (300) c.* FROM NV_Events_Cat c WHERE {PastCatFilter} ORDER BY ISNULL(c.EndDate, c.FromDate) DESC"
            : $"SELECT c.* FROM NV_Events_Cat c WHERE {UpcomingCatFilter} ORDER BY c.FromDate, c.Ordernumber";

        await using var conn = new SqlConnection(_crmDb);
        var cats = (await conn.QueryAsync<EventCatRow>(new CommandDefinition(sql, new { _o.EventsPortalId }, cancellationToken: ct))).ToList();

        // The event type is derived (number of schools / sessions), so filter after mapping. Volumes are small.
        IEnumerable<EventDto> events = await MapEventsAsync(conn, cats, ct);
        if (!string.IsNullOrWhiteSpace(q.Type))
            events = events.Where(e => string.Equals(e.Type, q.Type, StringComparison.OrdinalIgnoreCase));

        return JsonContentStore.Page(events, q.Page, q.PageSize);
    }

    public async Task<EventDetailDto?> GetEventAsync(int id, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_crmDb);
        var cat = await conn.QueryFirstOrDefaultAsync<EventCatRow>(new CommandDefinition(
            "SELECT c.* FROM NV_Events_Cat c WHERE c.Id = @Id AND c.Isactive = 1 AND c.is_show_website = 1",
            new { Id = id }, cancellationToken: ct));
        if (cat is null) return null;

        var sessions = (await LoadSessionsAsync(conn, [cat.Id], ct)).ToList();
        var detail = MapEvent(new EventDetailDto(), cat, sessions);
        detail.BodyHtml = _files.Html(PlainText.FirstNonEmpty(cat.ContentxEN, cat.DesceptionEN));

        var schoolIds = SchoolIds(cat, sessions);
        if (schoolIds.Count > 0)
        {
            var rows = await conn.QueryAsync<CapTruongRow>(new CommandDefinition(
                "SELECT t.* FROM Cap_Truong t WHERE t.Id IN @Ids", new { Ids = schoolIds }, cancellationToken: ct));
            var byId = rows.ToDictionary(r => r.Id);
            detail.Schools = schoolIds.Where(byId.ContainsKey).Select(i => MapSchool(byId[i])).ToList();
        }
        return detail;
    }

    private async Task<List<EventDto>> MapEventsAsync(SqlConnection conn, List<EventCatRow> cats, CancellationToken ct)
    {
        if (cats.Count == 0) return [];
        var sessions = (await LoadSessionsAsync(conn, cats.Select(c => c.Id).ToArray(), ct))
            .GroupBy(s => s.CatId)
            .ToDictionary(g => g.Key, g => g.ToList());
        return cats.Select(c => MapEvent(new EventDto(), c, sessions.GetValueOrDefault(c.Id) ?? [])).ToList();
    }

    private static Task<IEnumerable<EventSessionRow>> LoadSessionsAsync(SqlConnection conn, int[] catIds, CancellationToken ct)
        => conn.QueryAsync<EventSessionRow>(new CommandDefinition(
            "SELECT e.* FROM NV_Events e WHERE e.CatId IN @CatIds AND ISNULL(e.Isactive, 1) = 1 ORDER BY e.Fromdatetime",
            new { CatIds = catIds }, cancellationToken: ct));

    private T MapEvent<T>(T dto, EventCatRow cat, List<EventSessionRow> sessions) where T : EventDto
    {
        var ordered = sessions.Where(s => s.Fromdatetime is not null).OrderBy(s => s.Fromdatetime).ToList();
        var schoolIds = SchoolIds(cat, ordered);

        dto.Id = cat.Id;
        dto.Title = PlainText.FirstNonEmpty(cat.CatNameEN, cat.CatName) ?? $"Event {cat.Id}";
        dto.Slug = SlugText.From(dto.Title);
        dto.Type = schoolIds.Count > 1 || ordered.Count > 1 ? EventTypes.Fair : EventTypes.InfoSession;
        dto.IsFeatured = dto.Type == EventTypes.Fair;
        dto.Summary = PlainText.Summary(PlainText.FirstNonEmpty(cat.DesceptionEN, cat.ContentxEN));
        dto.CoverUrl = _files.Url(PlainText.FirstNonEmpty(cat.Avatar, ordered.FirstOrDefault()?.Avatar));
        dto.SchoolCount = schoolIds.Count;

        dto.Sessions = ordered.Select(s => new EventSessionDto
        {
            City = CityOf(s),
            Venue = PlainText.FirstNonEmpty(s.DiadiemEN, s.Diadiem),
            StartAt = VnTime.From(s.Fromdatetime!.Value),
            EndAt = VnTime.From(s.Enddatetime)
        }).ToList();

        dto.StartAt = VnTime.From(cat.FromDate) ?? dto.Sessions.FirstOrDefault()?.StartAt ?? DateTimeOffset.Now;
        dto.EndAt = VnTime.From(cat.EndDate) ?? dto.Sessions.LastOrDefault()?.EndAt;

        var cities = dto.Sessions.Select(s => s.City).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().ToList();
        dto.City = cities.Count > 0 ? string.Join(" and ", cities) : null;
        dto.Venue = dto.Sessions.Count == 1 ? dto.Sessions[0].Venue : null;
        dto.IsOnline = dto.Sessions.Count > 0 && dto.Sessions.All(s =>
            (s.Venue ?? string.Empty).Contains("online", StringComparison.OrdinalIgnoreCase) ||
            (s.Venue ?? string.Empty).Contains("zoom", StringComparison.OrdinalIgnoreCase));

        dto.StudentRegistrationUrl = $"/events/{dto.Slug}-{dto.Id}#register";
        dto.ExhibitorRegistrationUrl = dto.Type == EventTypes.Fair ? "/institutions#exhibit" : null;
        return dto;
    }

    private static List<int> SchoolIds(EventCatRow cat, IEnumerable<EventSessionRow> sessions)
        => PlainText.Ids(new[] { cat.FairSchool }.Concat(sessions.Select(s => s.School)).ToArray());

    private string CityOf(EventSessionRow s)
    {
        if (s.Vanphong is { } office && _o.OfficeCities.TryGetValue(office.ToString(), out var city))
            return city;
        var venue = PlainText.FirstNonEmpty(s.DiadiemEN, s.Diadiem) ?? string.Empty;
        if (venue.Contains("Hà Nội", StringComparison.OrdinalIgnoreCase) || venue.Contains("Hanoi", StringComparison.OrdinalIgnoreCase)) return "Hanoi";
        if (venue.Contains("Hồ Chí Minh", StringComparison.OrdinalIgnoreCase) || venue.Contains("HCM", StringComparison.OrdinalIgnoreCase)
            || venue.Contains("Ho Chi Minh", StringComparison.OrdinalIgnoreCase)) return "Ho Chi Minh City";
        if (venue.Contains("Đà Nẵng", StringComparison.OrdinalIgnoreCase) || venue.Contains("Da Nang", StringComparison.OrdinalIgnoreCase)) return "Da Nang";
        return string.Empty;
    }

    // ============================================================ News

    public async Task<IReadOnlyList<CategoryDto>> GetNewsCategoriesAsync(CancellationToken ct = default)
        => (await LoadCategoriesAsync(ct)).Values.ToList();

    public async Task<PagedResult<NewsDto>> GetNewsAsync(NewsQuery q, CancellationToken ct = default)
    {
        var page = Math.Max(1, q.Page);
        var size = Math.Clamp(q.PageSize, 1, 100);
        var categories = await LoadCategoriesAsync(ct);

        // Destination articles live in their own news categories ("study-in-the-usa"…), see Content:CountryNewsCategories.
        var categorySlug = q.Category;
        if (string.IsNullOrWhiteSpace(categorySlug) && !string.IsNullOrWhiteSpace(q.Country))
        {
            if (!_o.CountryNewsCategories.TryGetValue(q.Country, out var mapped)) return PagedResult<NewsDto>.Empty(page, size);
            categorySlug = mapped;
        }

        await using var conn = new SqlConnection(_webDb);
        SqlMapper.GridReader multi;
        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var cat = categories.FirstOrDefault(kv => string.Equals(kv.Value.Slug, categorySlug, StringComparison.OrdinalIgnoreCase));
            if (cat.Value is null) return PagedResult<NewsDto>.Empty(page, size);
            multi = await conn.QueryMultipleAsync(new CommandDefinition(
                "WebView_NVCMS_News_SelectByCategory",
                new { CategoryId = cat.Key, PortalId = _o.NewsPortalId, PageIndex = page, PageSize = size },
                commandType: CommandType.StoredProcedure, cancellationToken: ct));
        }
        else
        {
            multi = await conn.QueryMultipleAsync(new CommandDefinition(
                "WebView_NVCMS_News_SelectAll",
                new { PortalId = _o.NewsPortalId, PageIndex = page, PageSize = size },
                commandType: CommandType.StoredProcedure, cancellationToken: ct));
        }

        using (multi)
        {
            var total = await multi.ReadFirstOrDefaultAsync<int>();
            var items = (await multi.ReadAsync<NewsRow>()).Select(r => MapNews(new NewsDto(), r, categories)).ToList();
            return new PagedResult<NewsDto> { Items = items, Total = total, Page = page, PageSize = size };
        }
    }

    public async Task<NewsDetailDto?> GetNewsArticleAsync(int id, CancellationToken ct = default)
    {
        var categories = await LoadCategoriesAsync(ct);
        await using var conn = new SqlConnection(_webDb);
        var row = await conn.QueryFirstOrDefaultAsync<NewsRow>(new CommandDefinition(
            "WebView_NVCMS_News_SelectByID", new { NewId = id, PortalId = _o.NewsPortalId },
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
        if (row is null) return null;

        var article = MapNews(new NewsDetailDto(), row, categories);
        article.BodyHtml = _files.Html(row.Content);
        article.Tags = (row.Tags ?? string.Empty)
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var related = await conn.QueryAsync<NewsRow>(new CommandDefinition(
            "WebView_NVCMS_News_SelectRelated",
            new { CategoryId = row.CategoryId, ExcludeId = id, PortalId = _o.NewsPortalId, Top = 3 },
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
        article.Related = related.Select(r => MapNews(new NewsDto(), r, categories)).ToList();
        return article;
    }

    private async Task<Dictionary<int, CategoryDto>> LoadCategoriesAsync(CancellationToken ct)
    {
        try
        {
            await using var conn = new SqlConnection(_webDb);
            var rows = await conn.QueryAsync<NewsCategoryRow>(new CommandDefinition(
                "WebView_NVCMS_NewsCategory_SelectAll", new { PortalId = _o.NewsPortalId },
                commandType: CommandType.StoredProcedure, cancellationToken: ct));
            return rows
                .Where(r => r.IsActive)
                .OrderBy(r => r.OrderNumber)
                .GroupBy(r => r.CategoryID)
                .ToDictionary(g => g.Key, g =>
                {
                    var slug = string.IsNullOrWhiteSpace(g.First().Slug) ? SlugText.From(g.First().CategoryName) : g.First().Slug!.Trim().Trim('/');
                    return new CategoryDto
                    {
                        Name = g.First().CategoryName.Trim(),
                        Slug = slug,
                        Group = _o.CountryNewsCategories.Values.Contains(slug, StringComparer.OrdinalIgnoreCase) ? "destination" : "topic"
                    };
                });
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "Loading news categories failed");
            return new Dictionary<int, CategoryDto>();
        }
    }

    private T MapNews<T>(T dto, NewsRow r, Dictionary<int, CategoryDto> categories) where T : NewsDto
    {
        var cat = categories.GetValueOrDefault(r.CategoryId);
        dto.Id = r.NewId;
        dto.Title = r.Title.Trim();
        dto.Slug = SlugText.From(r.Title);
        dto.Summary = PlainText.Summary(r.Summary, 300);
        dto.ImageUrl = _files.Url(r.ImagePath);
        dto.CategoryName = cat?.Name;
        dto.CategorySlug = cat?.Slug;
        dto.CountrySlug = cat is null ? null : _o.CountryNewsCategories.FirstOrDefault(kv => string.Equals(kv.Value, cat.Slug, StringComparison.OrdinalIgnoreCase)).Key;
        dto.PublishedAt = VnTime.From(r.PublishedDate);
        dto.Author = PlainText.FirstNonEmpty(r.Tacgia);
        return dto;
    }

    // ============================================================ Mapping helpers

    private SchoolDto MapSchool(CapTruongRow r) => Fill(new SchoolDto(), r);

    private T Fill<T>(T s, CapTruongRow r) where T : SchoolDto
    {
        var c = CountryOf(r.Country);
        s.Id = r.Id;
        s.Name = (r.NameofSchool ?? string.Empty).Trim();
        s.Slug = SlugText.From(s.Name);
        s.LogoUrl = _files.Url(r.Logo);
        s.CoverUrl = _files.Url(r.Conver);
        s.CountrySlug = c.Slug;
        s.CountryName = c.Name;
        s.FlagUrl = c.Flag;
        s.City = PlainText.FirstNonEmpty(r.ThanhPholongannhatEN, r.ThanhPholongannhat);
        s.Level = LevelOf(r.Loai);
        s.IsPartner = r.isPartner == true;
        // English only: never fall back to the Vietnamese summary on the EN site.
        s.Summary = PlainText.Summary(r.TomTatEN);
        return s;
    }

    private static (string Slug, string Name, string? Flag) CountryOf(int? id)
    {
        foreach (var c in KnownCountries)
            if (c.Id == id) return (c.Slug, c.Name, c.Flag);
        return (OtherSlug, OtherName, null);
    }

    /// <summary>Cap_Truong.Loai holds Cap_Loaitruong ids (see TruongService.NormalizeLoai on the Vietnamese site).</summary>
    private static string? LevelOf(string? loai) => loai?.Trim() switch
    {
        "2" or "10" or "4Y" => "University",
        "3" or "2Y" => "Community college",
        "6" => "College",
        "4" or "5" or "BF" or "HS" => "High school",
        "9" or "GR" => "Graduate school",
        "1" or "11" or "ESL" => "ESL",
        "7" or "8" => "Summer programme",
        _ => null
    };

    private static string[] LoaiIdsFor(string level) => level.Trim().ToLowerInvariant() switch
    {
        "university" => ["2", "10", "4Y"],
        "community college" => ["3", "2Y"],
        "college" => ["6"],
        "high school" => ["4", "5", "BF", "HS"],
        "graduate school" => ["9", "GR"],
        "esl" => ["1", "11", "ESL"],
        "summer programme" => ["7", "8"],
        _ => []
    };

    private static string EscapeLike(string value)
        => value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");

    private static string? NormalizeUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;
        url = url.Trim();
        return url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? url : "https://" + url.TrimStart('/');
    }

    /// <summary>YouTube watch/short links → privacy-friendly embed URL; anything else is not embedded.</summary>
    private static string? YouTubeEmbed(string? link)
    {
        if (string.IsNullOrWhiteSpace(link) || !Uri.TryCreate(link.Trim(), UriKind.Absolute, out var uri)) return null;
        string? id = null;
        if (uri.Host.EndsWith("youtu.be", StringComparison.OrdinalIgnoreCase))
            id = uri.AbsolutePath.Trim('/');
        else if (uri.Host.Contains("youtube.com", StringComparison.OrdinalIgnoreCase))
        {
            if (uri.AbsolutePath.StartsWith("/embed/", StringComparison.OrdinalIgnoreCase))
                id = uri.AbsolutePath["/embed/".Length..];
            else
                id = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("v", out var v) ? v.ToString() : null;
        }
        return string.IsNullOrWhiteSpace(id) ? null : $"https://www.youtube-nocookie.com/embed/{Uri.EscapeDataString(id)}";
    }
}
