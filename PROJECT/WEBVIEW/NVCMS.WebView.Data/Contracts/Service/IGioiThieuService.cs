using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface IGioiThieuService
{
    Task<GioiThieuViewModel?> GetByIdAsync(int id, int portalId);
    Task<IEnumerable<GioiThieuViewModel>> GetAllAsync(int portalId);
    Task<IEnumerable<GioiThieuViewModel>> GetAllByParentIdAsync(int parentId, int portalId);
}
