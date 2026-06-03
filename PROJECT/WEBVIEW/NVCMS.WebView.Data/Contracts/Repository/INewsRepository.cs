using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface INewsRepository
{
    Task<PaginatedList<NewsModel>> GetByCategoryAsync(int categoryId, int portalId, int page, int pageSize);
    Task<PaginatedList<NewsModel>> GetByCategoryIdsAsync(IEnumerable<int> categoryIds, int portalId, int page, int pageSize);
    Task<PaginatedList<NewsModel>> GetAllPagedAsync(int portalId, int page, int pageSize);
    Task<NewsModel?> GetByIdAsync(int newId, int portalId);
    Task<IEnumerable<NewsModel>> GetRelatedAsync(int categoryId, int excludeId, int portalId, int top = 5);
    Task<IEnumerable<NewsCategoryModel>> GetAllCategoriesAsync(int portalId);
    Task<IEnumerable<(int CategoryId, int Count)>> GetCategoryCountsAsync(int portalId);
    Task<NewsCategoryModel?> GetCategoryByIdAsync(int categoryId);
    Task<NewsCategoryModel?> GetCategoryBySlugAsync(string slug, int portalId);
    Task IncrementViewCountAsync(int newId);
    Task<IEnumerable<NewsModel>> GetFeaturedAsync(int portalId, int top);

    // NewsBySchool
    Task<IEnumerable<TruongModel>> GetSchoolsByNewsAsync(int newId);
    Task<IEnumerable<NewsModel>>   GetNewsBySchoolAsync(int schoolId);
}
