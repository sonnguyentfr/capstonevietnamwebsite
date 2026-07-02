using Microsoft.Extensions.Caching.Memory;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class EventsService : IEventsService
{
    private readonly IEventsRepository       _repo;
    private readonly ITruongRepository       _truong;
    private readonly IOrganizationRepository _org;
    private readonly ContentUrlRewriter      _rewriter;
    private readonly IMemoryCache            _cache;

    public EventsService(
        IEventsRepository       repo,
        ITruongRepository       truong,
        IOrganizationRepository org,
        ContentUrlRewriter      rewriter,
        IMemoryCache            cache)
    {
        _repo     = repo;
        _truong   = truong;
        _org      = org;
        _rewriter = rewriter;
        _cache    = cache;
    }

    public async Task<IEnumerable<EventsCatViewModel>> GetActiveCatsWithEventsAsync(int portalid)
    {
        var key = CacheKeys.EventsActive(portalid);
        if (_cache.TryGetValue(key, out IEnumerable<EventsCatViewModel>? cached) && cached is not null)
            return cached;

        var cats = await _repo.GetActiveCatsAsync(portalid);
        var result = await MapCatsAsync(cats);
        _cache.Set(key, result, CacheKeys.TtlHomepage);
        return result;
    }

    public async Task<IEnumerable<EventsCatViewModel>> GetAllCatsWithEventsAsync()
    {
        var cats = await _repo.GetAllCatsAsync();
        return await MapCatsAsync(cats);
    }

    public async Task<EventsCatViewModel?> GetCatWithEventsAsync(int catId, int portalId)
    {
        var key = CacheKeys.EventsCatDetail(catId, portalId);
        if (_cache.TryGetValue(key, out EventsCatViewModel? cachedVm) && cachedVm is not null)
            return cachedVm;

        var cat = await _repo.GetCatByIdAsync(catId);
        if (cat is null) return null;

        var events = await _repo.GetEventsByCatAsync(catId, portalId);
        var eventList = events
            .OrderBy(e => e.Fromdatetime.HasValue ? 0 : 1)
            .ThenBy(e => e.Fromdatetime)
            .ToList();

        // Collect all unique IDs in one pass — avoids per-event DB round-trips
        var catSchoolIds         = BuildMergedSchoolIds(cat.FairSchool, eventList);
        var schoolIdsForEvents   = eventList.Select(e => ParseIds(e.School)).ToList();
        var allSchoolIds         = catSchoolIds
            .Concat(schoolIdsForEvents.SelectMany(x => x))
            .Distinct().ToList();
        var catOrgIds            = ParseIds(cat.FairOrg);
        var evOrgIds             = eventList.Select(e => ParseIds(e.Org)).ToList();
        var allOrgIds            = catOrgIds
            .Concat(evOrgIds.SelectMany(x => x))
            .Distinct().ToList();

        // Two parallel DB calls instead of N×2 sequential calls
        var schoolTask = allSchoolIds.Count > 0
            ? _truong.GetByIdsAsync(allSchoolIds)
            : Task.FromResult(Enumerable.Empty<TruongModel>());
        var orgTask = allOrgIds.Count > 0
            ? _org.GetByIdsAsync(allOrgIds)
            : Task.FromResult(Enumerable.Empty<OrganizationModel>());

        await Task.WhenAll(schoolTask, orgTask);

        var schoolMap = (await schoolTask).ToDictionary(t => t.Id);
        var orgMap    = (await orgTask).ToDictionary(o => o.Id);

        var catSchools = BuildSchoolCards(catSchoolIds, schoolMap);
        var catOrgs    = BuildOrgCards(catOrgIds, orgMap);

        var mappedEvents = eventList.Select((ev, i) =>
            MapEvent(ev,
                BuildSchoolCards(schoolIdsForEvents[i], schoolMap),
                BuildOrgCards(evOrgIds[i], orgMap))
        ).ToList();

        var vm = MapCat(cat, mappedEvents, catSchools, catOrgs);
        _cache.Set(key, vm, CacheKeys.TtlHomepage);
        return vm;
    }

    /// <summary>
    /// Gộp FairSchool + School của từng event (đã sắp xếp theo Fromdatetime).
    /// Trả về danh sách ID đã deduplicate, giữ thứ tự xuất hiện đầu tiên.
    /// </summary>
    private static List<int> BuildMergedSchoolIds(
        string? fairSchoolCsv,
        IEnumerable<EventsModel> sortedEvents)
    {
        var seen   = new HashSet<int>();
        var result = new List<int>();

        void AddCsv(string? csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return;
            foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (int.TryParse(part, out var id) && seen.Add(id))
                    result.Add(id);
            }
        }

        // 1. Schools khai báo ở cấp NV_Events_Cat trước
        AddCsv(fairSchoolCsv);

        // 2. Schools từ từng NV_Events (đã sắp theo Fromdatetime)
        foreach (var ev in sortedEvents)
            AddCsv(ev.School);

        return result;
    }

    public async Task<IEnumerable<EventsCatViewModel>> GetPastCatsWithEventsAsync(int portalid)
    {
        var cats = await _repo.GetPastCatsAsync(portalid);
        return await MapCatsAsync(cats);
    }

    public async Task<(IEnumerable<EventsCatViewModel> Items, int Total)> GetPastCatsPagedAsync(int portalid, int page, int pageSize)
    {
        var (cats, total) = await _repo.GetPastCatsPagedAsync(portalid, page, pageSize);
        var items = await MapCatsAsync(cats);
        return (items, total);
    }

    private async Task<IEnumerable<EventsCatViewModel>> MapCatsAsync(IEnumerable<EventsCatModel> cats)
    {
        var result = new List<EventsCatViewModel>();
        foreach (var cat in cats)
        {
            var events = await _repo.GetEventsByCatAsync(cat.Id, cat.PortalId);
            result.Add(MapCat(cat, events.Select(e => MapEvent(e, [], [])).ToList(), [], []));
        }
        return result;
    }

    private EventsCatViewModel MapCat(
        EventsCatModel c,
        List<EventsViewModel> events,
        IEnumerable<TruongCardViewModel> catSchools,
        IEnumerable<OrgCardViewModel> catOrgs) =>
        new()
        {
            Id              = c.Id,
            CatName         = c.CatName         ?? string.Empty,
            CatNameEN       = c.CatNameEN        ?? string.Empty,
            Slug            = SlugHelper.ToSlug(c.CatName ?? string.Empty),
            AvatarUrl       = _rewriter.ResolveUrl(c.Avatar),
            Desception      = _rewriter.ResolveHtml(c.Desception),
            DesceptionEN    = _rewriter.ResolveHtml(c.DesceptionEN),
            Contentx        = _rewriter.ResolveHtml(c.Contentx),
            ContentxEN      = _rewriter.ResolveHtml(c.ContentxEN),
            FromDate        = c.FromDate,
            EndDate         = c.EndDate,
            DateShow        = c.DateShow,
            FairSchool      = c.FairSchool,
            FairDiengia     = c.FairDiengia,
            FairTestimonial = c.FairTestimonial,
            FairDonviTaiTro = c.FairDonviTaiTro,
            FairOrg         = c.FairOrg,
            Email           = c.Email,
            Link_pr         = c.Link_pr,
            TabID           = c.TabID,
            Ordernumber     = c.Ordernumber,
            Is_show_website = c.Is_show_website,
            Sendcode        = c.Sendcode,
            ContentMail     = c.ContentMail,
            Schools         = catSchools,
            Orgs            = catOrgs,
            Events          = events
        };

    private EventsViewModel MapEvent(
        EventsModel e,
        IEnumerable<TruongCardViewModel> schools,
        IEnumerable<OrgCardViewModel> orgs) =>
        new()
        {
            Id            = e.Id,
            Title         = e.Title          ?? string.Empty,
            TitleEN       = e.TitleEN        ?? string.Empty,
            AvatarUrl     = _rewriter.ResolveUrl(e.Avatar),
            Diadiem       = e.Diadiem,
            DiadiemEN     = e.DiadiemEN,
            Fromdatetime  = e.Fromdatetime,
            Enddatetime   = e.Enddatetime,
            Thanhphan     = e.Thanhphan,
            ThanhphanEN   = e.ThanhphanEN,
            School        = e.School,
            Org           = e.Org,
            Gia           = e.Gia,
            Descreption   = _rewriter.ResolveHtml(e.Descreption),
            DescreptionEN = _rewriter.ResolveHtml(e.DescreptionEN),
            LienheName    = e.LienheName,
            LienheEmail   = e.LienheEmail,
            LienheMobile  = e.LienheMobile,
            CatId         = e.CatId,
            Ordernumber   = e.Ordernumber,
            Schools       = schools,
            Orgs          = orgs
        };

    // ── Helpers: parse CSV IDs ───────────────────────────────────────────────

    private static List<int> ParseIds(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv)) return [];
        var result = new List<int>();
        foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            if (int.TryParse(part, out var id)) result.Add(id);
        return result;
    }

    // Build card lists from pre-loaded dictionaries (no DB calls)
    private IEnumerable<TruongCardViewModel> BuildSchoolCards(
        List<int> ids, Dictionary<int, TruongModel> map)
    {
        var result = new List<TruongCardViewModel>(ids.Count);
        foreach (var id in ids)
        {
            if (!map.TryGetValue(id, out var t)) continue;
            result.Add(new TruongCardViewModel
            {
                Id           = t.Id,
                NameofSchool = t.NameofSchool,
                Tomtat       = t.Tomtat,
                LogoUrl      = _rewriter.ResolveUrl(t.Logo),
                CoverUrl     = _rewriter.ResolveUrl(t.Conver),
                Loai         = t.Loai,
                CountryId    = t.Country,
                CountryName  = s_countryNames.TryGetValue(t.Country ?? 0, out var cn) ? cn : null,
                StateName    = string.IsNullOrWhiteSpace(t.ThanhPholongannhat) ? null : t.ThanhPholongannhat,
                IsPartner    = t.isPartner ?? false,
                Slug         = SlugHelper.ToSlug(t.NameofSchool ?? string.Empty),
            });
        }
        return result;
    }

    private IEnumerable<OrgCardViewModel> BuildOrgCards(
        List<int> ids, Dictionary<int, OrganizationModel> map)
    {
        var result = new List<OrgCardViewModel>(ids.Count);
        foreach (var id in ids)
        {
            if (!map.TryGetValue(id, out var o)) continue;
            result.Add(new OrgCardViewModel
            {
                Id          = o.Id,
                Name        = o.Name,
                LogoUrl     = _rewriter.ResolveUrl(o.Logo),
                Website     = o.Website,
                Email       = o.Email,
                Phone       = o.Phone,
                Diachi      = o.Diachi,
                CountryName = s_countryNames.TryGetValue(o.quocgia ?? 0, out var cn) ? cn : null,
            });
        }
        return result;
    }

    private static readonly Dictionary<int, string> s_countryNames = new()
    {
        {1,   "Úc"},
        {3,   "Canada"},
        {23,  "Thụy Sĩ"},
        {28,  "Anh"},
        {38,  "Mỹ"},
        {99,  "New Zealand"},
        {401, "Ireland"},
    };
}

