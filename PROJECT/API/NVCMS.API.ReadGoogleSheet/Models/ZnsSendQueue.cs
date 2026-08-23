namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZnsSendQueue
{
    public long Id { get; set; }
    public long TemplateId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string TemplateDataJson { get; set; } = "{}";
    public string Status { get; set; } = string.Empty;
    public int RetryCount { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? MsgId { get; set; }

    public int? CampaignId { get; set; }
    public int? EventCatId { get; set; }
    public int? EventId { get; set; }
    public string? ContextType { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
