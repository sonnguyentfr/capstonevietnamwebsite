using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Common;

public static class NewsUrlBuilder
{
    public static string BuildNewsUrl(string categorySlug, string newsMetaUrl, int newsId)
    {
        var catPath = categorySlug.Trim('/');
        return $"/{catPath}/{newsMetaUrl}-{newsId}";
    }
    
    public static string BuildNewsUrl(ViewModels.NewsItemViewModel item)
    {
        return BuildNewsUrl(item.CategorySlug, item.Slug, item.NewId);
    }
    
    public static string BuildNewsUrl(ViewModels.NewsDetailViewModel item)
    {
        return BuildNewsUrl(item.CategorySlug, item.Slug, item.NewId);
    }
    
    public static string BuildNewsUrl(NewsModel news, NewsCategoryModel category)
    {
        var metaUrl = !string.IsNullOrEmpty(news.MetaUrl) ? news.MetaUrl : SlugHelper.ToSlug(news.Title);
        var catSlug = !string.IsNullOrEmpty(category.Slug) ? category.Slug : SlugHelper.ToSlug(category.CategoryName);
        return BuildNewsUrl(catSlug, metaUrl, news.NewId);
    }
    
    public static string BuildFullNewsUrl(string scheme, string host, string categorySlug, string newsMetaUrl, int newsId)
    {
        return $"{scheme}://{host}{BuildNewsUrl(categorySlug, newsMetaUrl, newsId)}";
    }
    
    public static string BuildFullNewsUrl(string scheme, string host, ViewModels.NewsItemViewModel item)
    {
        return $"{scheme}://{host}{BuildNewsUrl(item)}";
    }
    
    public static string BuildFullNewsUrl(string scheme, string host, ViewModels.NewsDetailViewModel item)
    {
        return $"{scheme}://{host}{BuildNewsUrl(item)}";
    }
}
