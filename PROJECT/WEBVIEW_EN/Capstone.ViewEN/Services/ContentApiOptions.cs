namespace Capstone.ViewEN.Services;

public sealed class ContentApiOptions
{
    public const string SectionName = "ContentApi";

    /// <summary>Base URL of the EN content API, ending with a slash. Example: https://api.capstonevietnam.com/api/v1/en/</summary>
    public string BaseUrl { get; set; } = "http://localhost:5320/api/v1/en/";

    /// <summary>true = read MockData/*.json directly instead of calling the API (front-end work without a backend).</summary>
    public bool UseMock { get; set; }

    /// <summary>Folder with the sample JSON when UseMock = true. Default: ../MockData next to the project.</summary>
    public string? MockDataPath { get; set; }

    /// <summary>Optional key sent as the X-Api-Key header.</summary>
    public string? ApiKey { get; set; }

    public int TimeoutSeconds { get; set; } = 8;

    /// <summary>How long GET responses are cached in memory. 0 disables caching.</summary>
    public int CacheMinutes { get; set; } = 5;
}

public sealed class SiteOptions
{
    public const string SectionName = "Site";

    /// <summary>Public origin of the EN site, used for canonical and Open Graph URLs.</summary>
    public string BaseUrl { get; set; } = "https://en.capstonevietnam.com";

    public string DefaultOgImage { get; set; } = "/img/og-default.jpg";
}
