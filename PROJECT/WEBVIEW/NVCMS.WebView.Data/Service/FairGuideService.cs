using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class FairGuideService : IFairGuideService
{
    private readonly IFairGuideRepository _repo;
    private readonly ContentUrlRewriter   _rewriter;

    public FairGuideService(IFairGuideRepository repo, ContentUrlRewriter rewriter)
    {
        _repo     = repo;
        _rewriter = rewriter;
    }

    public async Task<IEnumerable<FairGuideItemVM>> GetAllAsync(int portalId)
    {
        var list = await _repo.GetAllActiveAsync(portalId);
        return list.Select(m => new FairGuideItemVM
        {
            Id     = m.Id,
            Title  = m.Title,
            Avatar = _rewriter.ResolveUrl(m.Avatar),
            Slug   = SlugHelper.ToSlug(m.Title)
        });
    }

    public async Task<FairGuideDetailVM?> GetDetailAsync(int id, int portalId)
    {
        var model = await _repo.GetByIdAsync(id, portalId);
        if (model is null) return null;

        var media = await _repo.GetMediaAsync(id, portalId);

        return new FairGuideDetailVM
        {
            Id          = model.Id,
            Title       = model.Title,
            Avatar      = _rewriter.ResolveUrl(model.Avatar),
            Descreption = model.Descreption,
            Noidung     = _rewriter.ResolveHtml(model.Noidung),
            Slug        = SlugHelper.ToSlug(model.Title),
            Media       = media.Select(m => new FairGuideMediaVM
            {
                Id          = m.Id,
                Title       = !string.IsNullOrWhiteSpace(m.Title) ? m.Title : m.FileName,
                //'MediaUrl    = _rewriter.ResolveUrl(m.MediaUrl),
                MediaUrl    = m.MediaUrl,
                OrderNumber = m.OrderNumber
            }).ToList()
        };
    }
}
