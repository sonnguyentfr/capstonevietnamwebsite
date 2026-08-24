using System.Text.Json.Serialization;

namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZaloApiEnvelope<T>
{
    [JsonPropertyName("error")]
    public int Error { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("metadata")]
    public ZaloTemplateListMetadata? Metadata { get; set; }
}

public class ZaloTemplateListMetadata
{
    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class ZaloTemplateListItemDto
{
    [JsonPropertyName("templateId")]
    public long TemplateId { get; set; }

    [JsonPropertyName("templateName")]
    public string TemplateName { get; set; } = string.Empty;

    [JsonPropertyName("createdTime")]
    public long CreatedTime { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("templateQuality")]
    public string? TemplateQuality { get; set; }
}

public class ZaloTemplateDetailDto
{
    [JsonPropertyName("templateId")]
    public long TemplateId { get; set; }

    [JsonPropertyName("templateName")]
    public string TemplateName { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("listParams")]
    public List<ZaloTemplateParamDto> ListParams { get; set; } = [];

    [JsonPropertyName("listButtons")]
    public List<ZaloTemplateButtonDto> ListButtons { get; set; } = [];

    [JsonPropertyName("timeout")]
    public long? Timeout { get; set; }

    [JsonPropertyName("previewUrl")]
    public string? PreviewUrl { get; set; }

    [JsonPropertyName("templateQuality")]
    public string? TemplateQuality { get; set; }

    [JsonPropertyName("templateTag")]
    public string? TemplateTag { get; set; }

    [JsonPropertyName("price")]
    public string? Price { get; set; }

    [JsonPropertyName("applyTemplateQuota")]
    public bool? ApplyTemplateQuota { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("price_uid")]
    public string? PriceUid { get; set; }

    [JsonPropertyName("price_sdt")]
    public string? PriceSdt { get; set; }
}

public class ZaloTemplateParamDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("require")]
    public bool Require { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "STRING";

    [JsonPropertyName("maxLength")]
    public int? MaxLength { get; set; }

    [JsonPropertyName("minLength")]
    public int? MinLength { get; set; }

    [JsonPropertyName("acceptNull")]
    public bool AcceptNull { get; set; }
}

public class ZaloTemplateButtonDto
{
    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

public class ZaloSendResponseData
{
    [JsonPropertyName("sent_time")]
    public string? SentTime { get; set; }

    [JsonPropertyName("sending_mode")]
    public string? SendingMode { get; set; }

    [JsonPropertyName("quota")]
    public ZaloSendQuotaDto? Quota { get; set; }

    [JsonPropertyName("msg_id")]
    public string? MsgId { get; set; }
}

public class ZaloSendQuotaDto
{
    [JsonPropertyName("remainingQuota")]
    public string? RemainingQuota { get; set; }

    [JsonPropertyName("dailyQuota")]
    public string? DailyQuota { get; set; }
}
