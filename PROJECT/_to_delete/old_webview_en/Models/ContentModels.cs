namespace Capstone.ViewEN.Models;

// ---------------------------------------------------------------------------
// DTOs returned by the content API (JSON, camelCase).
// MockData/*.json contains one sample response for each of these.
// ---------------------------------------------------------------------------

public sealed class SiteDto
{
    public string Name { get; set; } = "Capstone Vietnam";
    public string Tagline { get; set; } = string.Empty;
    public string HeaderLogo { get; set; } = "/img/logo-color.png";
    public string FooterLogo { get; set; } = "/img/logo-white.png";
    public string Hotline { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string VietnameseSiteUrl { get; set; } = "https://capstonevietnam.com/";
    public string RecruitSiteUrl { get; set; } = "https://recruitinvietnam.com/";
    public List<OfficeDto> Offices { get; set; } = [];
    public List<SocialLinkDto> Socials { get; set; } = [];
    public List<MenuItemDto> Menu { get; set; } = [];
    public List<StatDto> Stats { get; set; } = [];
    public LeaderDto? Leader { get; set; }
    /// <summary>Raw HTML injected into &lt;head&gt; (GTM, GA4…). Managed in the CMS.</summary>
    public string? HeaderCode { get; set; }
    /// <summary>Raw HTML injected before &lt;/body&gt;.</summary>
    public string? FooterCode { get; set; }
}

public sealed class OfficeDto
{
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? MapUrl { get; set; }
}

public sealed class SocialLinkDto
{
    /// <summary>facebook | linkedin | youtube | instagram | zalo | twitter</summary>
    public string Network { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public sealed class MenuItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = "/";
    public string? Description { get; set; }
    public List<MenuItemDto> Children { get; set; } = [];
}

public sealed class StatDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public sealed class LeaderDto
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Quote { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string? ProfileUrl { get; set; }
}

// ---------- Home page blocks (editable in the CMS) ----------

public sealed class HomeContentDto
{
    public HeroDto Hero { get; set; } = new();
    public SectionIntroDto ServicesIntro { get; set; } = new();
    public InstitutionsBlockDto Institutions { get; set; } = new();
}

public sealed class HeroDto
{
    public string Title { get; set; } = string.Empty;
    public string Lead { get; set; } = string.Empty;
    public DoorDto StudentDoor { get; set; } = new();
    public DoorDto InstitutionDoor { get; set; } = new();
}

public sealed class DoorDto
{
    public string Who { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = "/";
}

public sealed class SectionIntroDto
{
    public string Title { get; set; } = string.Empty;
    public string Intro { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public sealed class InstitutionsBlockDto
{
    public string Title { get; set; } = string.Empty;
    public string Lead { get; set; } = string.Empty;
    public List<IconTextDto> Items { get; set; } = [];
}

public sealed class IconTextDto
{
    public string Icon { get; set; } = "badge";
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

// ---------- Schools ----------

public sealed class CountryDto
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? FlagUrl { get; set; }
    public int SchoolCount { get; set; }
}

public class SchoolDto
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string CountrySlug { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string? FlagUrl { get; set; }
    public string? City { get; set; }
    /// <summary>High school | Community college | College | University | ESL | Summer programme</summary>
    public string? Level { get; set; }
    public bool IsPartner { get; set; }
    public string? Summary { get; set; }
}

public sealed class SchoolDetailDto : SchoolDto
{
    public int? Founded { get; set; }
    public string? Website { get; set; }
    public string? Accreditation { get; set; }
    public string? TuitionFrom { get; set; }
    public List<string> Programs { get; set; } = [];
    public string? DescriptionHtml { get; set; }
    public string? VideoUrl { get; set; }
    public List<EventDto> UpcomingEvents { get; set; } = [];
    public List<SchoolDto> Related { get; set; } = [];
}

public sealed class SchoolQuery
{
    public string? Keyword { get; set; }
    public string? Country { get; set; }
    public string? Level { get; set; }
    /// <summary>true = only schools flagged for the home page.</summary>
    public bool? Featured { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

// ---------- Services ----------

public sealed class ServiceDto
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Icon { get; set; } = "compass";
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    /// <summary>students | institutions</summary>
    public string Audience { get; set; } = "students";
    public int Order { get; set; }
    public string? BodyHtml { get; set; }
}

// ---------- Events & fairs ----------

public static class EventTypes
{
    public const string Fair = "fair";
    public const string InfoSession = "info-session";
    public const string Workshop = "workshop";
    public const string Webinar = "webinar";

    public static string Label(string? type) => type switch
    {
        Fair => "Education fair",
        InfoSession => "Info session",
        Workshop => "Workshop",
        Webinar => "Webinar",
        _ => "Event"
    };
}

public class EventDto
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Type { get; set; } = EventTypes.Fair;
    public bool IsFeatured { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? CoverUrl { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset? EndAt { get; set; }
    public string? City { get; set; }
    public string? Venue { get; set; }
    public bool IsOnline { get; set; }
    public int SchoolCount { get; set; }
    public string? StudentRegistrationUrl { get; set; }
    public string? ExhibitorRegistrationUrl { get; set; }
    /// <summary>Multi-city fairs: one session per city.</summary>
    public List<EventSessionDto> Sessions { get; set; } = [];
}

public sealed class EventSessionDto
{
    public string City { get; set; } = string.Empty;
    public string? Venue { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset? EndAt { get; set; }
}

public sealed class EventDetailDto : EventDto
{
    public string? BodyHtml { get; set; }
    public List<SchoolDto> Schools { get; set; } = [];
}

public sealed class EventQuery
{
    /// <summary>fair | info-session | null (all)</summary>
    public string? Type { get; set; }
    /// <summary>upcoming | past</summary>
    public string When { get; set; } = "upcoming";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// ---------- News ----------

public class NewsDto
{
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? ImageUrl { get; set; }
    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public string? Author { get; set; }
}

public sealed class NewsDetailDto : NewsDto
{
    public string? BodyHtml { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<NewsDto> Related { get; set; } = [];
}

public sealed class CategoryDto
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public sealed class NewsQuery
{
    public string? Category { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
}

// ---------- Misc ----------

public sealed class TestimonialDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? SchoolName { get; set; }
    public string Outcome { get; set; } = string.Empty;
    public string? Quote { get; set; }
    public string? PhotoUrl { get; set; }
}

public sealed class PageDto
{
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? BodyHtml { get; set; }
    public string? ImageUrl { get; set; }
}

public sealed class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(Total / (double)PageSize);

    public static PagedResult<T> Empty(int page = 1, int pageSize = 12) => new() { Page = page, PageSize = pageSize };
}

// ---------- Leads (consultation / partnership enquiries) ----------

public sealed class LeadRequest
{
    /// <summary>student | parent | institution</summary>
    public string Role { get; set; } = "student";
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Organisation { get; set; }
    public string Interest { get; set; } = string.Empty;
    public string? Message { get; set; }
    public bool Consent { get; set; }
    public string? SourceUrl { get; set; }
    public string? UtmSource { get; set; }
    public string? UtmMedium { get; set; }
    public string? UtmCampaign { get; set; }
    public string Language { get; set; } = "en";
}

public sealed class LeadResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
