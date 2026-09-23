using NVCMS.API.ReadGoogleSheet.Common;
using System.Text.Json.Serialization;

namespace NVCMS.API.ReadGoogleSheet.Models.ZaloOA;

// Các class dưới đây map 1-1 với kết quả stored procedure ZaloOA_* (Dapper) và dùng luôn làm DTO trả về API.
// Mọi DateTime lưu UTC; converter bảo đảm JSON có hậu tố "Z".

public class ZaloOACustomer
{
    public long Id { get; set; }
    public string OAId { get; set; } = "";
    public string ZaloUserId { get; set; } = "";
    public string? ZaloUserIdByApp { get; set; }
    public string? DisplayName { get; set; }
    public string? UserAlias { get; set; }
    public string? AvatarUrl { get; set; }
    public bool? IsFollower { get; set; }
    public bool? IsSensitive { get; set; }
    public string? SharedName { get; set; }
    public string? SharedPhone { get; set; }
    public string? SharedAddress { get; set; }
    public string? SharedDob { get; set; }
    public string? Gender { get; set; }
    public string? TagsJson { get; set; }
    public string? NotesJson { get; set; }
    public string Status { get; set; } = "";
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? FirstInteractionAt { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? LastInteractionAt { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? LastProfileSyncAt { get; set; }
    [JsonIgnore] public string? ProfileRawJson { get; set; }
    [JsonIgnore] public bool IsDeleted { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime CreatedAt { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime UpdatedAt { get; set; }

    /// <summary>Chỉ có ở GetById/GetList.</summary>
    public long? ConversationId { get; set; }

    /// <summary>Chỉ có ở kết quả Upsert.</summary>
    [JsonIgnore] public bool IsNew { get; set; }
}

public class ZaloOAConversation
{
    public long Id { get; set; }
    public string OAId { get; set; } = "";
    public long CustomerId { get; set; }
    public string Status { get; set; } = "";
    public int? AssignedToUserId { get; set; }
    public int UnreadCount { get; set; }
    public long? LastMessageId { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? LastMessageAt { get; set; }
    public string? LastMessagePreview { get; set; }
    public string? LastMessageSenderType { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? LastCustomerMessageAt { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? LastReadAt { get; set; }
    public int? LastReadByUserId { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime StartedAt { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? ClosedAt { get; set; }
    public int? ClosedByUserId { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime CreatedAt { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime UpdatedAt { get; set; }

    // Thông tin khách (join) - có ở GetById/GetList
    public string? ZaloUserId { get; set; }
    public string? DisplayName { get; set; }
    public string? UserAlias { get; set; }
    public string? AvatarUrl { get; set; }
    public bool? IsFollower { get; set; }
    public string? SharedName { get; set; }
    public string? SharedPhone { get; set; }

    /// <summary>
    /// Zalo chỉ cho gửi tin tư vấn khi khách tương tác trong 7 ngày gần nhất (lỗi -230).
    /// Tính theo tin nhắn cuối của khách mà hệ thống biết - chỉ để cảnh báo trên UI, không chặn gửi.
    /// </summary>
    public bool CanReplyWithin7Days => LastCustomerMessageAt.HasValue && LastCustomerMessageAt.Value > DateTime.UtcNow.AddDays(-7);

    [JsonIgnore] public bool IsNew { get; set; }
}

public class ZaloOAMessage
{
    public long Id { get; set; }
    public long ConversationId { get; set; }
    public long CustomerId { get; set; }
    public string Direction { get; set; } = "";
    public string SenderType { get; set; } = "";
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public int? AgentUserId { get; set; }
    public string MessageType { get; set; } = "";
    public string? Content { get; set; }
    public string? AttachmentsJson { get; set; }
    public string? QuoteZaloMessageId { get; set; }
    public string? ZaloMessageId { get; set; }
    public Guid? ClientMessageId { get; set; }
    public string Status { get; set; } = "";
    public int? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime SentAt { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? ReceivedAt { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? DeliveredAt { get; set; }
    [JsonConverter(typeof(UtcNullableDateTimeConverter))] public DateTime? SeenAt { get; set; }
    [JsonIgnore] public long? WebhookEventId { get; set; }
    [JsonIgnore] public string? RawPayload { get; set; }
    [JsonIgnore] public string? ResponseJson { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime CreatedAt { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime UpdatedAt { get; set; }

    [JsonIgnore] public bool IsDuplicate { get; set; }
}

public class ZaloOAWebhookEventRow
{
    public long Id { get; set; }
    public string DedupKey { get; set; } = "";
    public string EventName { get; set; } = "";
    public string? OAId { get; set; }
    public string? ZaloUserId { get; set; }
    public string? ZaloMessageId { get; set; }
    public long? EventTimestamp { get; set; }
    public string RawPayload { get; set; } = "";
    public bool SignatureValid { get; set; }
    public string ProcessStatus { get; set; } = "";
    public int RetryCount { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime ReceivedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ZaloOAUpsertResult
{
    public long Id { get; set; }
    public bool IsDuplicate { get; set; }
    public string? ProcessStatus { get; set; }
}

public class ZaloOAUnreadSummary
{
    public int TotalUnread { get; set; }
    public int UnreadConversations { get; set; }
    public long LatestInboundMessageId { get; set; }
    public List<ZaloOANewMessageNotice> NewMessages { get; set; } = new();
}

public class ZaloOANewMessageNotice
{
    public long Id { get; set; }
    public long ConversationId { get; set; }
    public long CustomerId { get; set; }
    public string MessageType { get; set; } = "";
    public string? Content { get; set; }
    [JsonConverter(typeof(UtcDateTimeConverter))] public DateTime SentAt { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
}

/// <summary>Tham số lưu 1 tin nhắn từ webhook / lịch sử.</summary>
public class ZaloOAMessageUpsert
{
    public long ConversationId { get; set; }
    public long CustomerId { get; set; }
    public string Direction { get; set; } = "";
    public string SenderType { get; set; } = "";
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public string MessageType { get; set; } = "";
    public string? Content { get; set; }
    public string? AttachmentsJson { get; set; }
    public string? QuoteZaloMessageId { get; set; }
    public string? ZaloMessageId { get; set; }
    public string Status { get; set; } = "";
    public DateTime SentAt { get; set; }
    public DateTime? ReceivedAt { get; set; }
    public long? WebhookEventId { get; set; }
    public string? RawPayload { get; set; }
    public string? Preview { get; set; }
    public bool IncrementUnread { get; set; }
}

/// <summary>Tham số tạo tin PENDING trước khi gọi Zalo.</summary>
public class ZaloOAPendingMessage
{
    public long ConversationId { get; set; }
    public long CustomerId { get; set; }
    public string? SenderId { get; set; }
    public string? ReceiverId { get; set; }
    public int? AgentUserId { get; set; }
    public string MessageType { get; set; } = "";
    public string? Content { get; set; }
    public string? AttachmentsJson { get; set; }
    public Guid ClientMessageId { get; set; }
    public string? Preview { get; set; }
}

public class ZaloOAConversationQuery
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    /// <summary>-1 tất cả, 0 chưa assign, &gt;0 theo DNN UserId.</summary>
    public int AssignedTo { get; set; } = -1;
    public bool UnreadOnly { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    /// <summary>LastMessageAt / UnreadCount / CreatedAt / DisplayName.</summary>
    public string? SortBy { get; set; }
    /// <summary>ASC / DESC.</summary>
    public string? SortDir { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 20;
}

public class ZaloOACustomerQuery
{
    public string? Keyword { get; set; }
    /// <summary>-1 tất cả, 0 / 1.</summary>
    public int IsFollower { get; set; } = -1;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    /// <summary>LastInteractionAt / DisplayName / CreatedAt.</summary>
    public string? SortBy { get; set; }
    public string? SortDir { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 20;
}

public class ZaloOAPagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalRecords { get; set; }
}
