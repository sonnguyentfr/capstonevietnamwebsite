using System.ComponentModel.DataAnnotations;

namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZnsSendRequest
{
    [Required]
    public long TemplateId { get; set; }

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public Dictionary<string, object?> TemplateData { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public int? CampaignId { get; set; }
    public int? EventCatId { get; set; }
    public int? EventId { get; set; }
    public string? ContextType { get; set; }
    public string? CreatedBy { get; set; }
}

public class ZnsSendResult
{
    public bool Success { get; set; }
    public int ErrorCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? MsgId { get; set; }
    public string? SentTime { get; set; }
    public string? SendingMode { get; set; }
    public int? RemainingQuota { get; set; }
    public int? DailyQuota { get; set; }
    public long? QueueId { get; set; }
    public string? JobId { get; set; }
}
