using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class NewsService : INewsService
{
    private readonly INewsRepository _repo;
    private readonly ContentUrlRewriter _rewriter;

    public NewsService(INewsRepository repo, ContentUrlRewriter rewriter)
    {
        _repo = repo;
        _rewriter = rewriter;
    }

    public async Task<PaginatedList<NewsItemViewModel>> GetByCategorySlugAsync(
        string categorySlug, int portalId, int page, int pageSize)
    {
        var categories = await _repo.GetAllCategoriesAsync(portalId);
        var category   = categories.FirstOrDefault(c =>
            SlugHelper.ToSlug(c.CategoryName) == categorySlug);

        if (category is null)
            return new PaginatedList<NewsItemViewModel>([], 0, page, pageSize);

        var paged = await _repo.GetByCategoryAsync(category.CategoryID, portalId, page, pageSize);
        var vms   = paged.Items.Select(n => MapToItem(n, category)).ToList();
        return new PaginatedList<NewsItemViewModel>(vms, paged.TotalCount, page, pageSize);
    }

    public async Task<NewsDetailViewModel?> GetDetailAsync(int newId, int portalId)
    {
        var news = await _repo.GetByIdAsync(newId, portalId);
        if (news is null) return null;

        await _repo.IncrementViewCountAsync(newId);

        var category = await _repo.GetCategoryByIdAsync(news.CategoryId);
        var related  = await _repo.GetRelatedAsync(news.CategoryId, newId, portalId);

        return new NewsDetailViewModel
        {
            NewId           = news.NewId,
            Title           = news.Title,
            ImagePath       = _rewriter.ResolveUrl(news.ImagePath),
            Content         = _rewriter.ResolveHtml(news.Content),
            Summary         = _rewriter.ResolveHtml(news.Summary),
            Tacgia          = news.Tacgia,
            SourceText      = news.SourceText,
            MetaTitle       = news.MetaTitle   ?? news.Title,
            MetaDescription = news.MetaDescription ?? news.Summary,
            MetaImage       = _rewriter.ResolveUrl(news.MetaImage ?? news.ImagePath),
            ViewCount       = news.ViewCount,
            PublishedDate   = news.PublishedDate,
            CategoryId      = news.CategoryId,
            CategoryName    = category?.CategoryName ?? string.Empty,
            CategorySlug    = SlugHelper.ToSlug(category?.CategoryName ?? string.Empty),
            Slug            = SlugHelper.ToSlug(news.Title),
            RelatedNews     = related.Select(r => MapToItem(r, category)).ToList()
        };
    }

    public async Task<IEnumerable<CategoryViewModel>> GetMenuCategoriesAsync(int portalId)
    {
        var all = await _repo.GetAllCategoriesAsync(portalId);
        return BuildTree(all, 0);
    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategoriesWithCountAsync(int portalId)
    {
        var all    = await _repo.GetAllCategoriesAsync(portalId);
        var counts = (await _repo.GetCategoryCountsAsync(portalId))
                     .ToDictionary(x => x.CategoryId, x => x.Count);

        return all.Select(c => new CategoryViewModel
        {
            CategoryID   = c.CategoryID,
            ParentId     = c.ParentId,
            CategoryName = c.CategoryName,
            Slug         = SlugHelper.ToSlug(c.CategoryName),
            Description  = c.Description,
            NewsCount    = counts.TryGetValue(c.CategoryID, out var cnt) ? cnt : 0,
        }).ToList();
    }

    public async Task<PaginatedList<NewsItemViewModel>> GetAllPagedAsync(int portalId, int page, int pageSize)
    {
        var paged  = await _repo.GetAllPagedAsync(portalId, page, pageSize);
        var catIds = paged.Items.Select(n => n.CategoryId).Distinct();
        var catTasks = catIds.Select(id => _repo.GetCategoryByIdAsync(id));
        var cats     = (await Task.WhenAll(catTasks))
                       .Where(c => c is not null)
                       .ToDictionary(c => c!.CategoryID, c => c!);

        var vms = paged.Items.Select(n =>
        {
            cats.TryGetValue(n.CategoryId, out var cat);
            return MapToItem(n, cat);
        }).ToList();

        return new PaginatedList<NewsItemViewModel>(vms, paged.TotalCount, page, pageSize);
    }

    public async Task<PaginatedList<NewsItemViewModel>> GetByCategoryIdAsync(
        int categoryId, int portalId, int page, int pageSize)
    {
        var category = await _repo.GetCategoryByIdAsync(categoryId);
        var all      = await _repo.GetAllCategoriesAsync(portalId);

        // collect the root category + all descendants
        var ids = new HashSet<int> { categoryId };
        CollectDescendants(all.ToList(), categoryId, ids);

        PaginatedList<NewsModel> paged;
        if (ids.Count == 1)
            paged = await _repo.GetByCategoryAsync(categoryId, portalId, page, pageSize);
        else
            paged = await _repo.GetByCategoryIdsAsync(ids, portalId, page, pageSize);

        // cache categories for name/slug mapping
        var catMap = all.ToDictionary(c => c.CategoryID);
        var vms = paged.Items.Select(n =>
        {
            catMap.TryGetValue(n.CategoryId, out var cat);
            return MapToItem(n, cat);
        }).ToList();
        return new PaginatedList<NewsItemViewModel>(vms, paged.TotalCount, page, pageSize);
    }

    private static void CollectDescendants(
        List<NewsCategoryModel> all, int parentId, HashSet<int> result)
    {
        foreach (var c in all.Where(x => x.ParentId == parentId))
        {
            if (result.Add(c.CategoryID))
                CollectDescendants(all, c.CategoryID, result);
        }
    }

    public async Task<IEnumerable<NewsItemViewModel>> GetFeaturedAsync(int portalId, int top)
    {
        var items = await _repo.GetFeaturedAsync(portalId, top);
        var catIds = items.Select(n => n.CategoryId).Distinct();

        // Load categories in batch để map tên/slug
        var catTasks = catIds.Select(id => _repo.GetCategoryByIdAsync(id));
        var cats     = (await Task.WhenAll(catTasks))
                       .Where(c => c is not null)
                       .ToDictionary(c => c!.CategoryID, c => c!);

        return items.Select(n =>
        {
            cats.TryGetValue(n.CategoryId, out var cat);
            return MapToItem(n, cat);
        });
    }

    public async Task<CategoryViewModel?> GetCategoryByIdAsync(int categoryId)
    {
        var cat = await _repo.GetCategoryByIdAsync(categoryId);
        if (cat is null) return null;
        return new CategoryViewModel
        {
            CategoryID   = cat.CategoryID,
            ParentId     = cat.ParentId,
            CategoryName = cat.CategoryName,
            Slug         = SlugHelper.ToSlug(cat.CategoryName),
            Description  = cat.Description
        };
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
        Slug          = SlugHelper.ToSlug(n.Title),
        CategorySlug  = SlugHelper.ToSlug(cat?.CategoryName ?? string.Empty),
        CategoryName  = cat?.CategoryName ?? string.Empty
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
                Slug         = SlugHelper.ToSlug(c.CategoryName),
                Description  = c.Description,
                Children     = BuildTree(all, c.CategoryID)
            })];
    }
}