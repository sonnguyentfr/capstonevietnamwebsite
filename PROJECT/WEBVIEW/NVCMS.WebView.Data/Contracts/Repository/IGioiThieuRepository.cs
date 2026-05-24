using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IGioiThieuRepository
{
    Task<GioiThieuModel?> GetByIdAsync(int id, int portalId);
    Task<IEnumerable<GioiThieuModel>> GetAllAsync(int portalId);
    Task<IEnumerable<GioiThieuModel>> GetAllByParentIdAsync(int parentId, int portalId);
}
