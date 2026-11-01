namespace Capstone.ApiEN.Infrastructure;

public sealed class ContentSourceOptions
{
    public const string SectionName = "Content";

    /// <summary>"Json" = serve MockData/*.json (default). "Sql" = schools, events and news from SQL Server.</summary>
    public string DataSource { get; set; } = "Json";

    /// <summary>Folder with the JSON files. Default: ../MockData (shared with the website).</summary>
    public string? MockDataPath { get; set; }

    /// <summary>File server for relative /DATA/ and /Portals/ paths stored in the database.</summary>
    public string FileServerBaseUrl { get; set; } = "https://capstonevietnam-fileserver.nvcms.net";

    /// <summary>PortalId of the English news in CapstoneVietNamWeb (NVCMS_News has no EN columns; VN news use PortalId 0). Check the real value.</summary>
    public int NewsPortalId { get; set; } = 1;

    /// <summary>PortalId of event categories (NV_Events_Cat). The Vietnamese site uses 50 (CRM portal).</summary>
    public int EventsPortalId { get; set; } = 50;

    /// <summary>Country slug → news category slug holding articles about that destination (as on the old EN site).</summary>
    public Dictionary<string, string> CountryNewsCategories { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        ["united-states"] = "study-in-the-usa",
        ["canada"] = "study-in-canada",
        ["united-kingdom"] = "study-in-uk",
        ["australia"] = "study-in-australia",
        ["other"] = "study-in-other-countries"
    };

    /// <summary>NV_Events.Vanphong → city name. Adjust to the real office ids.</summary>
    public Dictionary<string, string> OfficeCities { get; set; } = new()
    {
        ["1"] = "Hanoi",
        ["2"] = "Ho Chi Minh City"
    };

    /// <summary>Leads: CRM portal and code prefix for WebView_TuVan_Upsert (same SP as the Vietnamese site).</summary>
    public int LeadCrmPortalId { get; set; } = 50;
    public string LeadCodePrefix { get; set; } = "WEBEN";
    public int LeadHinhThucId { get; set; } = 7;

    /// <summary>Leads in Json mode are appended here (one JSON object per line).</summary>
    public string LeadsFile { get; set; } = "App_Data/leads.jsonl";

    public bool UseSql => string.Equals(DataSource, "Sql", StringComparison.OrdinalIgnoreCase);
}

public sealed class ApiSecurityOptions
{
    public const string SectionName = "ApiSecurity";

    /// <summary>Accepted values of the X-Api-Key header. Empty = no key required (development).</summary>
    public string[] ApiKeys { get; set; } = [];
}
