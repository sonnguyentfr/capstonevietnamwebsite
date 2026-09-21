using System.ComponentModel.DataAnnotations;

namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZnsSendRequest
{
    [Required]
    public long TemplateId { get; set; }

    [Required]
    public int CampaignId { get; set; }

    public string? Phone { get; set; }

    public Dictionary<string, object?> TemplateData { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public string? TrackingId { get; set; }
    public string? Qr { get; set; }

    public string? Type { get; set; }
    public int? EventCatId { get; set; }
    public int? EventId { get; set; }
    public string? ContextType { get; set; }
    public string? CreatedBy { get; set; }
}

public class ZnsEnqueueItemResult
{
    public long QueueId { get; set; }
    public string JobId { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string TrackingId { get; set; } = string.Empty;
}

public class ZnsEnqueueResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int TotalRecipients { get; set; }
    public List<ZnsEnqueueItemResult> Items { get; set; } = [];
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
