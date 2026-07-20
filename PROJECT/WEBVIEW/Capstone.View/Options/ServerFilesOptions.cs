namespace Capstone.View.Options;

public class SiteSettings
{
    public const string SectionName = "SiteSettings";

    public int PortalId { get; set; } = 0;
    public int PortalCRMId { get; set; } = 50;
    public string ServerFilesBaseUrl { get; set; } = string.Empty;
}
