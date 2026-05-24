using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class GioiThieuService : IGioiThieuService
{
    private readonly IGioiThieuRepository _repo;
    private readonly ContentUrlRewriter _rewriter;

    public GioiThieuService(IGioiThieuRepository repo, ContentUrlRewriter rewriter)
    {
        _repo = repo;
        _rewriter = rewriter;
    }

    public async Task<GioiThieuViewModel?> GetByIdAsync(int id, int portalId)
    {
        var model = await _repo.GetByIdAsync(id, portalId);
        return model is null ? null : Map(model);
    }

    public async Task<IEnumerable<GioiThieuViewModel>> GetAllAsync(int portalId)
    {
        var list = await _repo.GetAllAsync(portalId);
        return list.Select(Map);
    }

    public async Task<IEnumerable<GioiThieuViewModel>> GetAllByParentIdAsync(int parentId, int portalId)
    {
        var list = await _repo.GetAllByParentIdAsync(parentId, portalId);
        return list.Select(Map);
    }

    private GioiThieuViewModel Map(GioiThieuModel m) => new()
    {
        Id           = m.Id,
        TrangDanhMuc = m.TrangDanhMuc,
        Tieudephu    = m.Tieudephu,
        ImagePath    = _rewriter.ResolveUrl(m.ImagePath),
        Tomtat       = _rewriter.ResolveHtml(m.Tomtat),
        Noidung      = _rewriter.ResolveHtml(m.Noidung),
        Link         = m.Link,
        ParentId     = m.ParentId,
        Ordernumber  = m.Ordernumber,
        PortalId     = m.PortalId,
    };
}
