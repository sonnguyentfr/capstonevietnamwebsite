using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Service;

public class LadingPageService : ILadingPageService
{
    private readonly ILadingPageRepository _repo;

    public LadingPageService(ILadingPageRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<NVCMS_LadingPageModel>> GetAllAsync(int portalId)
    {
        var result = await _repo.GetAllAsync(portalId);
        return result.ToList();
    }

    public async Task<List<NVCMS_LadingPageModel>> GetAllByParentIdAsync(int parentId, int portalId)
    {
        var result = await _repo.GetAllByParentIdAsync(parentId, portalId);
        return result.ToList();
    }

    public async Task<NVCMS_LadingPageModel?> GetByIdAsync(int id, int portalId)
    {
        return await _repo.GetByIdAsync(id, portalId);
    }
}
