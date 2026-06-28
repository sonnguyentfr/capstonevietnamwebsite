using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Helpers;

public class NewsUrlHelper
{
    public string BuildDetailUrlPath(NewsItemViewModel item)
    {
        return NVCMS.WebView.Data.Common.NewsUrlBuilder.BuildNewsUrl(item);
    }
}
