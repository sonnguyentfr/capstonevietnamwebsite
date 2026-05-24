using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface INewsService
{
    Task<PaginatedList<NewsItemViewModel>> GetByCategorySlugAsync(
        string categorySlug, int portalId, int page, int pageSize);
    Task<NewsDetailViewModel?> GetDetailAsync(int newId, int portalId);
    Task<IEnumerable<CategoryViewModel>> GetMenuCategoriesAsync(int portalId);
}