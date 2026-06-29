using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Service;

public class NewsUrlService : INewsUrlService
{
    private readonly INewsRepository _repo;

    public NewsUrlService(INewsRepository repo)
    {
        _repo = repo;
    }

    public int? ExtractNewsId(string slug)
    {
        if (string.IsNullOrEmpty(slug)) return null;
        var lastDash = slug.LastIndexOf('-');
        if (lastDash < 0 || lastDash == slug.Length - 1) return null;
        var idPart = slug[(lastDash + 1)..];
        return int.TryParse(idPart, out var id) ? id : null;
    }

    public async Task<string?> BuildCanonicalUrl(NewsModel news, string scheme, string host)
    {
        if (news.CategoryId <= 0) return null;
        var categoryPath = await BuildCategoryPath(news.CategoryId);
        if (categoryPath is null) return null;
        var metaUrl = !string.IsNullOrEmpty(news.MetaUrl) ? news.MetaUrl : Common.SlugHelper.ToSlug(news.Title);
        return $"{scheme}://{host}{categoryPath}/{metaUrl}-{news.NewId}";
    }

    public async Task<string?> BuildCategoryPath(int categoryId)
    {
        var category = await _repo.GetCategoryByIdAsync(categoryId);
        if (category is null) return null;
        
        return "/" + category.FullSlug;
    }

    public bool IsCanonical(string requestUrl, string canonicalUrl)
    {
        return string.Equals(requestUrl, canonicalUrl, StringComparison.OrdinalIgnoreCase);
    }
}
