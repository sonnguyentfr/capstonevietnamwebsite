using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class EventsService : IEventsService
{
    private readonly IEventsRepository _repo;
    private readonly ContentUrlRewriter _rewriter;

    public EventsService(IEventsRepository repo, ContentUrlRewriter rewriter)
    {
        _repo     = repo;
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

        var events = await _repo.GetEventsByCatAsync(catId, portalId);
        return MapCat(cat, events);
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
            result.Add(MapCat(cat, events));
        }
        return result;
    }

    private EventsCatViewModel MapCat(EventsCatModel c, IEnumerable<EventsModel> events) =>
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
            Events          = events.Select(MapEvent).ToList()
        };

    private EventsViewModel MapEvent(EventsModel e) =>
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
            Ordernumber   = e.Ordernumber
        };
}
