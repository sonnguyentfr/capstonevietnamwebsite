using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _repo;
    private readonly ContentUrlRewriter _rewriter;

    public BannerService(IBannerRepository repo, ContentUrlRewriter rewriter)
    {
        _repo = repo;
        _rewriter = rewriter;
    }

    private BannerViewModel Map(BannerModel b) => new()
    {
        Id         = b.Id,
        Title      = b.Title,
        KieuBanner = b.KieuBanner,
        IMGLink    = _rewriter.ResolveUrl(b.IMGLink),
        Link       = b.Link,
        Vitri      = b.Vitri,
        Height     = b.Height,
        Width      = b.Width
    };

    public async Task<IEnumerable<BannerViewModel>> GetAllAsync(int portalId)
    {
        var items = await _repo.GetAllAsync(portalId);
        return items.Select(Map);
    }

    public async Task<IEnumerable<BannerViewModel>> GetAllShowAsync(int portalId, int vitri)
    {
        var items = await _repo.GetAllShowAsync(portalId, vitri);
        return items.Select(Map);
    }

    public async Task<IEnumerable<BannerViewModel>> GetByVitriAsync(int vitri, int portalId)
    {
        var items = await _repo.GetByVitriAsync(vitri, portalId);
        return items.Select(Map);
    }

    public async Task<BannerViewModel?> GetByIdAsync(int bannerId)
    {
        var b = await _repo.GetByIdAsync(bannerId);
        return b is null ? null : Map(b);
    }

    public Task UpdateClickAsync(int bannerId) => _repo.UpdateClickAsync(bannerId);

    public Task UpdateViewAsync(int bannerId) => _repo.UpdateViewAsync(bannerId);
}