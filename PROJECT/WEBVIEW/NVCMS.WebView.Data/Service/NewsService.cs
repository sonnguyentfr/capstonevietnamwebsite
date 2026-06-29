using Microsoft.Extensions.Caching.Memory;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class NewsService : INewsService
{
    private readonly INewsRepository    _repo;
    private readonly ContentUrlRewriter _rewriter;
    private readonly IMemoryCache       _cache;

    public NewsService(INewsRepository repo, ContentUrlRewriter rewriter, IMemoryCache cache)
    {
        _repo     = repo;
        _rewriter = rewriter;
        _cache    = cache;
    }

    private async Task<IReadOnlyList<NewsCategoryModel>> GetAllCatsCachedAsync(int portalId)
    {
        var key = CacheKeys.AllCategories(portalId);
        if (_cache.TryGetValue(key, out IReadOnlyList<NewsCategoryModel>? cached) && cached is not null)
            return cached;
        var list = (await _repo.GetAllCategoriesAsync(portalId)).ToList();
        _cache.Set(key, (IReadOnlyList<NewsCategoryModel>)list, CacheKeys.TtlCategories);
        return list;
    }

    public async Task<PaginatedList<NewsItemViewModel>> GetByCategoryFullSlugAsync(
        string categoryFullSlug, int portalId, int page, int pageSize)
    {
        var categories = await GetAllCatsCachedAsync(portalId);
        var category   = categories.FirstOrDefault(c => c.FullSlug == categoryFullSlug);
        if (category is null) return new PaginatedList<NewsItemViewModel>([], 0, page, pageSize);
        var paged = await _repo.GetByCategoryAsync(category.CategoryID, portalId, page, pageSize);
        var vms   = paged.Items.Select(n => MapToItem(n, category)).ToList();
        return new PaginatedList<NewsItemViewModel>(vms, paged.TotalCount, page, pageSize);
    }

    public async Task<NewsDetailViewModel?> GetDetailAsync(int newId, int portalId)
    {
        var cacheKey = CacheKeys.NewsDetail(newId, portalId);
        if (_cache.TryGetValue(cacheKey, out NewsDetailViewModel? cached) && cached is not null)
        {
            _ = Task.Run(() => _repo.IncrementViewCountAsync(newId));
            return cached;
        }
        var news = await _repo.GetByIdAsync(newId, portalId);
        if (news is null) return null;
        _ = Task.Run(() => _repo.IncrementViewCountAsync(newId));
        var categoryTask = _repo.GetCategoryByIdAsync(news.CategoryId);
        var relatedTask  = _repo.GetRelatedAsync(news.CategoryId, newId, portalId);
        var schoolsTask  = _repo.GetSchoolsByNewsAsync(newId);
        await Task.WhenAll(categoryTask, relatedTask, schoolsTask);
        var category = categoryTask.Result;
        var related  = relatedTask.Result;
        var schools  = schoolsTask.Result;
        var vm = new NewsDetailViewModel
        {
            NewId           = news.NewId,
            Title           = news.Title,
            ImagePath       = _rewriter.ResolveUrl(news.ImagePath),
            Content         = _rewriter.ResolveHtml(news.Content),
            Summary         = _rewriter.ResolveHtml(news.Summary),
            Tacgia          = news.Tacgia,
            SourceText      = news.SourceText,
            MetaTitle       = news.MetaTitle ?? news.Title,
            MetaDescription = news.MetaDescription ?? news.Summary,
            MetaImage       = _rewriter.ResolveUrl(news.MetaImage ?? news.ImagePath),
            ViewCount       = news.ViewCount,
            PublishedDate   = news.PublishedDate,
            CategoryId      = news.CategoryId,
            CategoryName    = category?.CategoryName ?? string.Empty,
            CategorySlug    = category?.FullSlug ?? string.Empty,
            Slug            = !string.IsNullOrEmpty(news.MetaUrl) ? news.MetaUrl : SlugHelper.ToSlug(news.Title),
            Tags            = news.Tags,
            RelatedNews     = related.Select(r => MapToItem(r, category)).ToList(),
            RelatedSchools  = schools.Select(MapToSchoolCard).ToList()
        };
        _cache.Set(cacheKey, vm, CacheKeys.TtlNewsDetail);
        return vm;
    }

    public async Task<NewsModel?> GetByIdAsync(int newId, int portalId)
    {
        return await _repo.GetByIdAsync(newId, portalId);
    }

    public async Task<IEnumerable<CategoryViewModel>> GetMenuCategoriesAsync(int portalId)
    {
        var all = await GetAllCatsCachedAsync(portalId);
        return BuildTree(all, 0);
    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategoriesWithCountAsync(int portalId)
    {
        var all = await GetAllCatsCachedAsync(portalId);
        var countsKey = CacheKeys.CategoryCounts(portalId);
        if (!_cache.TryGetValue(countsKey, out Dictionary<int, int>? counts) || counts is null)
        {
            counts = (await _repo.GetCategoryCountsAsync(portalId)).ToDictionary(x => x.CategoryId, x => x.Count);
            _cache.Set(countsKey, counts, CacheKeys.TtlCategories);
        }
        return all.Select(c => new CategoryViewModel
        {
            CategoryID   = c.CategoryID,
            ParentId     = c.ParentId,
            CategoryName = c.CategoryName,
            Slug         = c.FullSlug,
            Description  = c.Description,
            NewsCount    = counts.TryGetValue(c.CategoryID, out var cnt) ? cnt : 0,
        }).ToList();
    }

    public async Task<PaginatedList<NewsItemViewModel>> GetAllPagedAsync(int portalId, int page, int pageSize)
    {
        var paged  = await _repo.GetAllPagedAsync(portalId, page, pageSize);
        var catMap = (await GetAllCatsCachedAsync(portalId)).ToDictionary(c => c.CategoryID);
        var vms = paged.Items.Select(n =>
        {
            catMap.TryGetValue(n.CategoryId, out var cat);
            return MapToItem(n, cat);
        }).ToList();
        return new PaginatedList<NewsItemViewModel>(vms, paged.TotalCount, page, pageSize);
    }

    public async Task<PaginatedList<NewsItemViewModel>> GetByCategoryIdAsync(
        int categoryId, int portalId, int page, int pageSize)
    {
        var all = await GetAllCatsCachedAsync(portalId);
        var ids = new HashSet<int> { categoryId };
        CollectDescendants(all, categoryId, ids);
        var cacheKey = ids.Count == 1
            ? CacheKeys.NewsList(portalId, categoryId, page, pageSize)
            : CacheKeys.NewsListByIds(portalId, string.Join(",", ids.OrderBy(x => x)), page, pageSize);
        if (!_cache.TryGetValue(cacheKey, out PaginatedList<NewsItemViewModel>? cachedPage) || cachedPage is null)
        {
            PaginatedList<NewsModel> paged;
            if (ids.Count == 1)
                paged = await _repo.GetByCategoryAsync(categoryId, portalId, page, pageSize);
            else
                paged = await _repo.GetByCategoryIdsAsync(ids, portalId, page, pageSize);
            var catMap = all.ToDictionary(c => c.CategoryID);
            var vms = paged.Items.Select(n =>
            {
                catMap.TryGetValue(n.CategoryId, out var cat);
                return MapToItem(n, cat);
            }).ToList();
            cachedPage = new PaginatedList<NewsItemViewModel>(vms, paged.TotalCount, page, pageSize);
            _cache.Set(cacheKey, cachedPage, CacheKeys.TtlNewsList);
        }
        return cachedPage;
    }

    public async Task<IEnumerable<NewsItemViewModel>> GetFeaturedAsync(int portalId, int top)
    {
        var cacheKey = CacheKeys.NewsFeatured(portalId, top);
        if (_cache.TryGetValue(cacheKey, out IEnumerable<NewsItemViewModel>? cached) && cached is not null)
            return cached;
        var items   = await _repo.GetFeaturedAsync(portalId, top);
        var allCats = (await GetAllCatsCachedAsync(portalId)).ToDictionary(c => c.CategoryID);
        var vms = items.Select(n =>
        {
            allCats.TryGetValue(n.CategoryId, out var cat);
            var parentSlug = cat?.ParentId > 0 && allCats.TryGetValue(cat.ParentId, out var parent)
                ? parent.FullSlug
                : null;
            var vm = MapToItem(n, cat);
            vm.CategoryParentSlug = parentSlug;
            return vm;
        }).ToList();
        _cache.Set(cacheKey, (IEnumerable<NewsItemViewModel>)vms, CacheKeys.TtlHomepage);
        return vms;
    }

    public async Task<CategoryViewModel?> GetCategoryByIdAsync(int categoryId)
    {
        var cacheKey = CacheKeys.CategoryById(categoryId);
        if (_cache.TryGetValue(cacheKey, out CategoryViewModel? cached) && cached is not null) return cached;
        var cat = await _repo.GetCategoryByIdAsync(categoryId);
        if (cat is null) return null;
        var vm = new CategoryViewModel
        {
            CategoryID   = cat.CategoryID,
            ParentId     = cat.ParentId,
            CategoryName = cat.CategoryName,
            Slug         = cat.FullSlug,
            Description  = cat.Description
        };
        _cache.Set(cacheKey, vm, CacheKeys.TtlCategories);
        return vm;
    }

    public async Task<CategoryViewModel?> GetCategoryByFullSlugAsync(string fullSlug, int portalId)
    {
        var cacheKey = CacheKeys.CategoryBySlug(portalId, fullSlug);
        if (_cache.TryGetValue(cacheKey, out CategoryViewModel? cached) && cached is not null) return cached;
        var cat = await _repo.GetCategoryByFullSlugAsync(fullSlug, portalId);
        if (cat is null)
        {
            var all = await GetAllCatsCachedAsync(portalId);
            cat = all.FirstOrDefault(c => c.FullSlug == fullSlug);
        }
        if (cat is null) return null;
        var vm = new CategoryViewModel
        {
            CategoryID   = cat.CategoryID,
            ParentId     = cat.ParentId,
            CategoryName = cat.CategoryName,
            Slug         = cat.FullSlug,
            Description  = cat.Description
        };
        _cache.Set(cacheKey, vm, CacheKeys.TtlCategories);
        return vm;
    }

    public async Task<IEnumerable<NewsItemViewModel>> GetNewsBySchoolAsync(int schoolId)
    {
        var items  = await _repo.GetNewsBySchoolAsync(schoolId);
        var catMap = (await GetAllCatsCachedAsync(0)).ToDictionary(c => c.CategoryID);
        return items.Select(n =>
        {
            catMap.TryGetValue(n.CategoryId, out var cat);
            return MapToItem(n, cat);
        });
    }

    public void InvalidateNewsCache(int newId, int portalId)
    {
        _cache.Remove(CacheKeys.NewsDetail(newId, portalId));
        _cache.Remove(CacheKeys.NewsFeatured(portalId, 7));
        _cache.Remove(CacheKeys.NewsFeatured(portalId, 8));
        _cache.Remove(CacheKeys.CategoryCounts(portalId));
    }

    public void InvalidateCategoryCache(int portalId)
    {
        _cache.Remove(CacheKeys.AllCategories(portalId));
        _cache.Remove(CacheKeys.CategoryCounts(portalId));
    }

    private static void CollectDescendants(
        IEnumerable<NewsCategoryModel> all, int parentId, HashSet<int> result)
    {
        foreach (var c in all.Where(x => x.ParentId == parentId))
            if (result.Add(c.CategoryID))
                CollectDescendants(all, c.CategoryID, result);
    }

    private NewsItemViewModel MapToItem(NewsModel n, NewsCategoryModel? cat) => new()
    {
        NewId         = n.NewId,
        CategoryId    = n.CategoryId,
        Title         = n.Title,
        ImagePath     = _rewriter.ResolveUrl(n.ImagePath),
        Summary       = _rewriter.ResolveHtml(n.Summary),
        Tacgia        = n.Tacgia,
        Tags          = n.Tags,
        PublishedDate = n.PublishedDate,
        Slug          = !string.IsNullOrEmpty(n.MetaUrl) ? n.MetaUrl : SlugHelper.ToSlug(n.Title),
        CategorySlug  = cat?.FullSlug ?? string.Empty,
        CategoryName  = cat?.CategoryName ?? string.Empty
    };

    private static readonly Dictionary<int, string> CountryNames = new()
    {
        {1,  "Uc"}, {3, "Canada"}, {23, "Thuy Si"}, {28, "Anh"},
        {38, "My"}, {99, "New Zealand"}, {401, "Ireland"}
    };

    private TruongCardViewModel MapToSchoolCard(TruongModel t) => new()
    {
        Id           = t.Id,
        NameofSchool = t.NameofSchool,
        Tomtat       = t.Tomtat,
        LogoUrl      = _rewriter.ResolveUrl(t.Logo),
        CoverUrl     = _rewriter.ResolveUrl(t.Conver),
        IsPartner    = t.isPartner ?? false,
        CountryId    = t.Country,
        CountryName  = CountryNames.TryGetValue(t.Country ?? 0, out var cn) ? cn : null,
        Slug         = SlugHelper.ToSlug(t.NameofSchool ?? string.Empty)
    };

    private static List<CategoryViewModel> BuildTree(
        IEnumerable<NewsCategoryModel> all, int parentId)
    {
        return [.. all
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.OrderNumber)
            .Select(c => new CategoryViewModel
            {
                CategoryID   = c.CategoryID,
                ParentId     = c.ParentId,
                CategoryName = c.CategoryName,
                Slug         = c.FullSlug,
                Description  = c.Description,
                Children     = BuildTree(all, c.CategoryID)
            })];
    }
}