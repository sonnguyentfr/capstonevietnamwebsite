namespace NVCMS.WebView.Data.SiteSettings;

/// <summary>
/// Strongly-typed website settings loaded from PortalSettings table.
/// Keys follow the settingPage* convention used by the legacy CMS.
/// </summary>
public class WebSiteSettings
{
    public int PortalId { get; init; }

    public GeneralInfo  General  { get; init; } = new();
    public SocialInfo   Social   { get; init; } = new();
    public ChatInfo     Chat     { get; init; } = new();
    public MailInfo     Mail     { get; init; } = new();
    public GoogleInfo   Google   { get; init; } = new();
    public CdnInfo      Cdn      { get; init; } = new();
    public LogoInfo     Logo     { get; init; } = new();
    public CodeInfo     Code     { get; init; } = new();
    public IReadOnlyList<BranchInfo> Branches { get; init; } = [];
}

public class GeneralInfo
{
    public string SiteName    { get; init; } = string.Empty;
    public string SiteWeb     { get; init; } = string.Empty;
    public string SiteAddress { get; init; } = string.Empty;
    public string SiteEmail   { get; init; } = string.Empty;
    public string SitePhone   { get; init; } = string.Empty;
    public string SiteSummary { get; init; } = string.Empty;
    public string SiteTag     { get; init; } = string.Empty;
}

public class SocialInfo
{
    public string Facebook  { get; init; } = string.Empty;
    public string Youtube   { get; init; } = string.Empty;
    public string LinkedIn  { get; init; } = string.Empty;
    public string Instagram { get; init; } = string.Empty;
    public string Zalo      { get; init; } = string.Empty;
    public string Twitter   { get; init; } = string.Empty;
    public string Whatsapp  { get; init; } = string.Empty;
    public string Skype     { get; init; } = string.Empty;
}

public class ChatInfo
{
    public string ZaloId         { get; init; } = string.Empty;
    public string FacebookChatId { get; init; } = string.Empty;
    public string LiveChatId     { get; init; } = string.Empty;
}

public class MailInfo
{
    public bool   EnableMail { get; init; }
    public string MailList   { get; init; } = string.Empty;
}

public class GoogleInfo
{
    public string CaptchaKey    { get; init; } = string.Empty;
    public string CaptchaSecret { get; init; } = string.Empty;
}

public class CdnInfo
{
    public string Cdn        { get; init; } = string.Empty;
    public string FileServer { get; init; } = string.Empty;
}

public class LogoInfo
{
    public string HeaderLogo { get; init; } = string.Empty;
    public string FooterLogo { get; init; } = string.Empty;
}

public class CodeInfo
{
    public string HeaderCode { get; init; } = string.Empty;
    public string FooterCode { get; init; } = string.Empty;
}

public class BranchInfo
{
    public string Name    { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Email   { get; init; } = string.Empty;
    public string Phone   { get; init; } = string.Empty;
    public string Thoigianlamviec   { get; init; } = string.Empty;
}
