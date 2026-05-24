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