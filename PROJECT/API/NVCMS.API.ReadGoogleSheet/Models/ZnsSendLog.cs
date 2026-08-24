namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZnsSendLog
{
    public long Id { get; set; }
    public long? ZnsTemplateId { get; set; }
    public long ZaloTemplateId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? ParamsJson { get; set; }
    public string? RequestJson { get; set; }
    public string? ResponseJson { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ZaloMessageId { get; set; }
    public DateTime? SentTime { get; set; }
    public string? SendingMode { get; set; }
    public int? RemainingQuota { get; set; }
    public int? DailyQuota { get; set; }
    public int? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public int? CampaignId { get; set; }
    public int? EventCatId { get; set; }
    public int? EventId { get; set; }
    public string? ContextType { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
