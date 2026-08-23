namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZnsTemplate
{
    public long Id { get; set; }
    public long TemplateId { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public long CreatedTime { get; set; }
    public string? Status { get; set; }
    public string? TemplateQuality { get; set; }
    public string? TemplateTag { get; set; }
    public long? Timeout { get; set; }
    public string? PreviewUrl { get; set; }
    public decimal? Price { get; set; }
    public decimal? PriceUid { get; set; }
    public decimal? PriceSdt { get; set; }
    public bool ApplyTemplateQuota { get; set; }
    public string? Reason { get; set; }
    public bool IsActive { get; set; } = true;
    public string? DetailJson { get; set; }
    public DateTime? LastSyncedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ZnsTemplateParam> Params { get; set; } = new List<ZnsTemplateParam>();
    public ICollection<ZnsTemplateButton> Buttons { get; set; } = new List<ZnsTemplateButton>();
}
