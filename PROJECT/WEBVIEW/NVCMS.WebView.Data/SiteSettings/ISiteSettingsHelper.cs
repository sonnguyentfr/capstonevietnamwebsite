namespace NVCMS.WebView.Data.SiteSettings;

public interface ISiteSettingsHelper
{
    /// <summary>
    /// Load all website settings for the given portal, cached by portalId.
    /// </summary>
    Task<WebSiteSettings> GetSettingsAsync(int portalId);

    /// <summary>
    /// Load only branches for the given portal, cached by portalId.
    /// </summary>
    Task<IReadOnlyList<BranchInfo>> GetBranchesAsync(int portalId);

    /// <summary>
    /// Load lightweight data for Header/Footer: 2 branches, social, contact.
    /// </summary>
    Task<HeaderFooterData> GetHeaderFooterDataAsync(int portalId);

    /// <summary>Invalidate the cache for a specific portal (call after admin saves).</summary>
    void InvalidateCache(int portalId);
}
