namespace NVCMS.WebView.Data.SiteSettings;

/// <summary>
/// Lightweight DTO for Header/Footer - only 2 branches and essential info
/// </summary>
public class HeaderFooterData
{
    public IReadOnlyList<BranchInfo> Branches { get; init; } = [];
    public SocialInfo Social { get; init; } = new();
    public string SitePhone { get; init; } = string.Empty;
    public string SiteEmail { get; init; } = string.Empty;
    public string HeaderLogo { get; init; } = string.Empty;
    public string FooterLogo { get; init; } = string.Empty;
    public string HeaderCode { get; init; } = string.Empty;
    public string FooterCode { get; init; } = string.Empty;
    public string SiteName { get; init; } = string.Empty;
}
