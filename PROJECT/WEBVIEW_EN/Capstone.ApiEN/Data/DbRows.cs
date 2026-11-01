namespace Capstone.ApiEN.Data;

// Column subsets of the existing tables, read with Dapper (unlisted columns are ignored).
// Names follow the database exactly, as in NVCMS.WebView.Data/Models.

/// <summary>CapstoneVietnam_old.dbo.Cap_Truong</summary>
internal sealed class CapTruongRow
{
    public int Id { get; set; }
    public string? NameofSchool { get; set; }
    public string? Tomtat { get; set; }
    public string? TomTatEN { get; set; }
    public string? Logo { get; set; }
    public string? Conver { get; set; }
    public string? VideoLink { get; set; }
    public string? Descreption { get; set; }
    public string? DescreptionEN { get; set; }
    public string? DescreptionWebsiteEN { get; set; }
    public string? Namthanhlap { get; set; }
    public string? Website { get; set; }
    public string? ThanhPholongannhat { get; set; }
    public string? ThanhPholongannhatEN { get; set; }
    public string? Kiemdinh { get; set; }
    public string? KiemdinhEN { get; set; }
    public string? Loai { get; set; }
    public string? ProgramOfered { get; set; }
    public int? Country { get; set; }
    public bool? isPartner { get; set; }
}

/// <summary>Result of WebView_Truong_GetCountriesWithCount.</summary>
internal sealed class CountryCountRow
{
    public int Id { get; set; }
    public int TruongCount { get; set; }
}

/// <summary>CapstoneVietnam_old.dbo.NV_Events_Cat — one fair / event series.</summary>
internal sealed class EventCatRow
{
    public int Id { get; set; }
    public string? CatName { get; set; }
    public string? CatNameEN { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Avatar { get; set; }
    public string? Desception { get; set; }
    public string? DesceptionEN { get; set; }
    public string? Contentx { get; set; }
    public string? ContentxEN { get; set; }
    public string? FairSchool { get; set; }
    public string? Link_pr { get; set; }
    public int? Ordernumber { get; set; }
}

/// <summary>CapstoneVietnam_old.dbo.NV_Events — one session of a fair (a city / a day).</summary>
internal sealed class EventSessionRow
{
    public int Id { get; set; }
    public int CatId { get; set; }
    public string? Title { get; set; }
    public string? TitleEN { get; set; }
    public int? Vanphong { get; set; }
    public string? Diadiem { get; set; }
    public string? DiadiemEN { get; set; }
    public DateTime? Fromdatetime { get; set; }
    public DateTime? Enddatetime { get; set; }
    public string? School { get; set; }
    public string? Avatar { get; set; }
}

/// <summary>CapstoneVietNamWeb NVCMS news (SP WebView_NVCMS_News_*).</summary>
internal sealed class NewsRow
{
    public int NewId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string? Tags { get; set; }
    public string? Tacgia { get; set; }
    public string? MetaUrl { get; set; }
    public DateTime PublishedDate { get; set; }
}

/// <summary>CapstoneVietNamWeb news categories (SP WebView_NVCMS_NewsCategory_SelectAll).</summary>
internal sealed class NewsCategoryRow
{
    public int CategoryID { get; set; }
    public int ParentId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public bool IsActive { get; set; }
    public int OrderNumber { get; set; }
}
