using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface INewsService
{
    Task<PaginatedList<NewsItemViewModel>> GetByCategoryFullSlugAsync(
        string categoryFullSlug, int portalId, int page, int pageSize);

    Task<PaginatedList<NewsItemViewModel>> GetByCategoryIdAsync(
        int categoryId, int portalId, int page, int pageSize);

    Task<PaginatedList<NewsItemViewModel>> GetAllPagedAsync(
        int portalId, int page, int pageSize);

    Task<IEnumerable<CategoryViewModel>> GetCategoriesWithCountAsync(int portalId);

    Task<IEnumerable<NewsItemViewModel>> GetFeaturedAsync(int portalId, int top);

    Task<NewsDetailViewModel?> GetDetailAsync(int newId, int portalId);
    Task<Models.NewsModel?> GetByIdAsync(int newId, int portalId);
    Task<IEnumerable<CategoryViewModel>> GetMenuCategoriesAsync(int portalId);
    Task<CategoryViewModel?> GetCategoryByIdAsync(int categoryId);
    Task<CategoryViewModel?> GetCategoryByFullSlugAsync(string fullSlug, int portalId);

    // NewsBySchool
    Task<IEnumerable<NewsItemViewModel>> GetNewsBySchoolAsync(int schoolId);

    // ByTag
    Task<PaginatedList<NewsItemViewModel>> GetByTagAsync(string tag, int portalId, int page, int pageSize);
}