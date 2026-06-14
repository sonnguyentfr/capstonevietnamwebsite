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

    public EventsService(
        IEventsRepository       repo,
        ITruongRepository       truong,
        IOrganizationRepository org,
        ContentUrlRewriter      rewriter)
    {
        _repo     = repo;
        _truong   = truong;
        _org      = org;
        _rewriter = rewriter;
    }

    public async Task<IEnumerable<EventsCatViewModel>> GetActiveCatsWithEventsAsync(int portalid)
    {
        
        var cats = await _repo.GetActiveCatsAsync(portalid);
        return await MapCatsAsync(cats);
    }

    public async Task<IEnumerable<EventsCatViewModel>> GetAllCatsWithEventsAsync()
    {
        var cats = await _repo.GetAllCatsAsync();
        return await MapCatsAsync(cats);
    }

    public async Task<EventsCatViewModel?> GetCatWithEventsAsync(int catId, int portalId)
    {
        var cat = await _repo.GetCatByIdAsync(catId);
        if (cat is null) return null;

        var events    = await _repo.GetEventsByCatAsync(catId, portalId);

        // Sắp xếp theo thời gian diễn ra (null xuống cuối)
        var eventList = events
            .OrderBy(e => e.Fromdatetime.HasValue ? 0 : 1)
            .ThenBy(e => e.Fromdatetime)
            .ToList();

        // ── Category-level schools: FairSchool + tất cả NV_Events.School (theo thứ tự thời gian) ──
        // Build ordered, deduplicated ID list
        var catSchoolIds = BuildMergedSchoolIds(cat.FairSchool, eventList);
        var catSchools   = await ResolveSchoolsByIdsAsync(catSchoolIds);

        // Category-level orgs (FairOrg)
        var catOrgs = await ResolveOrgsAsync(cat.FairOrg);

        // Per-event schools + orgs (mỗi event chỉ resolve schools của chính nó)
        var mappedEvents = new List<EventsViewModel>();
        foreach (var ev in eventList)
        {
            var evSchools = await ResolveSchoolsAsync(ev.School);
            var evOrgs    = await ResolveOrgsAsync(ev.Org);
            mappedEvents.Add(MapEvent(ev, evSchools, evOrgs));
        }

        return MapCat(cat, mappedEvents, catSchools, catOrgs);
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

    // Parse "144,4633,290,..." → load TruongCardViewModel list  (dùng cho per-event)
    private Task<IEnumerable<TruongCardViewModel>> ResolveSchoolsAsync(string? idsCsv)
    {
        if (string.IsNullOrWhiteSpace(idsCsv)) return Task.FromResult(Enumerable.Empty<TruongCardViewModel>());

        var ids = idsCsv
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => int.TryParse(s, out var n) ? (int?)n : null)
            .Where(n => n.HasValue)
            .Select(n => n!.Value)
            .ToList();

        return ResolveSchoolsByIdsAsync(ids);
    }

    // Resolve từ danh sách ID đã có sẵn (dùng cho category-level merged list)
    private async Task<IEnumerable<TruongCardViewModel>> ResolveSchoolsByIdsAsync(List<int> ids)
    {
        if (ids.Count == 0) return [];

        var truongs = await _truong.GetByIdsAsync(ids);

        // Giữ thứ tự từ danh sách ID đầu vào
        var map    = truongs.ToDictionary(t => t.Id);
        var result = new List<TruongCardViewModel>();
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

    // Parse "28,44,..." → load OrgCardViewModel list
    private async Task<IEnumerable<OrgCardViewModel>> ResolveOrgsAsync(string? idsCsv)
    {
        if (string.IsNullOrWhiteSpace(idsCsv)) return [];

        var ids = idsCsv
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => int.TryParse(s, out var n) ? (int?)n : null)
            .Where(n => n.HasValue)
            .Select(n => n!.Value)
            .ToList();

        if (ids.Count == 0) return [];

        var orgs = await _org.GetByIdsAsync(ids);

        var map    = orgs.ToDictionary(o => o.Id);
        var result = new List<OrgCardViewModel>();
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
}
