using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface INewsRepository
{
    Task<PaginatedList<NewsModel>> GetByCategoryAsync(int categoryId, int portalId, int page, int pageSize);
    Task<NewsModel?> GetByIdAsync(int newId, int portalId);
    Task<IEnumerable<NewsModel>> GetRelatedAsync(int categoryId, int excludeId, int portalId, int top = 5);
    Task<IEnumerable<NewsCategoryModel>> GetAllCategoriesAsync(int portalId);
    Task<NewsCategoryModel?> GetCategoryByIdAsync(int categoryId);
    Task IncrementViewCountAsync(int newId);
}