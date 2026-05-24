namespace Capstone.View.Options;

public class SiteSettings
{
    public const string SectionName = "SiteSettings";

    public int PortalId { get; set; } = 0;
    public string ServerFilesBaseUrl { get; set; } = string.Empty;
}
