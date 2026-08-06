using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface ILadingPageService
{
    Task<List<NVCMS_LadingPageModel>> GetAllAsync(int portalId);
    Task<List<NVCMS_LadingPageModel>> GetAllByParentIdAsync(int parentId, int portalId);
    Task<NVCMS_LadingPageModel?> GetByIdAsync(int id, int portalId);
}
