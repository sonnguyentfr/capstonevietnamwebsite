using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface ILadingPageRepository
{
    Task<IEnumerable<NVCMS_LadingPageModel>> GetAllAsync(int portalId);
    Task<IEnumerable<NVCMS_LadingPageModel>> GetAllByParentIdAsync(int parentId, int portalId);
    Task<NVCMS_LadingPageModel?> GetByIdAsync(int id, int portalId);
}
