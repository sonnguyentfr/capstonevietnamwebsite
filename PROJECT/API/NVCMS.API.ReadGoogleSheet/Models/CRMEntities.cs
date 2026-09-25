namespace NVCMS.API.ReadGoogleSheet.Models;

public class Marketing_Zalo_ListSdt
{
    public int Id { get; set; }
    public int Marketing_Zalo_CampaignId { get; set; }
    public string? PhoneRaw { get; set; }
    public string Phone { get; set; } = string.Empty;
    public byte Status { get; set; }
    public int SendCount { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }
    public int PortalId { get; set; }
}

public class NV_Event
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Diadiem { get; set; }
    public DateTime? Fromdatetime { get; set; }
    public DateTime? Enddatetime { get; set; }
    public int? CatId { get; set; }
    public bool? Isactive { get; set; }
    public int? Portalid { get; set; }
}

public class NV_Events_Cat
{
    public int Id { get; set; }
    public string? CatName { get; set; }
    public string? FairOrg { get; set; }
    public string? DateShow { get; set; }
    public string? Desception { get; set; }
    public string? sendzalo_content { get; set; }
    public string? ContentMail { get; set; }
    public string? FairDiengia { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? Isactive { get; set; }
    public int? PortalId { get; set; }
    public bool? is_show_website { get; set; }
    public string? Link_pr { get; set; }
    public string? TitleMail { get; set; }
}

public class NV_Events_Student
{
    public int Id { get; set; }
    public int? EventId { get; set; }
    public int? EventCatId { get; set; }
    public int? StudentId { get; set; }
    public string? StudentCode { get; set; }
}

public class Student_Info
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Hotendem { get; set; }
    public string? Ten { get; set; }
    public string? Sodienthoai { get; set; }
    public string? Email { get; set; }
    public int? PortalId { get; set; }
}
