using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.ViewModels;

public class LandingPageGroupViewModel
{
    public NVCMS_LadingPageModel Parent { get; set; } = null!;
    public List<NVCMS_LadingPageModel> Children { get; set; } = new();
}
