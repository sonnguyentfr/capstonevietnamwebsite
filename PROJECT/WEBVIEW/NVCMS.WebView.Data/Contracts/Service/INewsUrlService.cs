using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface INewsUrlService
{
    int? ExtractNewsId(string slug);
    Task<string?> BuildCanonicalUrl(NewsModel news, string scheme, string host);
    Task<string?> BuildCategoryPath(int categoryId);
    bool IsCanonical(string requestUrl, string canonicalUrl);
}
