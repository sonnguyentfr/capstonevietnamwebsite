namespace NVCMS.API.ReadGoogleSheet.Models.ZaloOA;

/// <summary>
/// Gửi tin từ website. AgentUserId do proxy DNN gắn (UserId DNN đang đăng nhập) - client không tự đặt được.
/// ClientMessageId: GUID do trình duyệt sinh cho mỗi lần bấm gửi → gửi lại cùng GUID không tạo tin trùng.
/// </summary>
public class ZaloOASendMessageRequest
{
    public long ConversationId { get; set; }
    public Guid ClientMessageId { get; set; }
    public int? AgentUserId { get; set; }

    /// <summary>TEXT (mặc định) hoặc IMAGE (gửi ảnh theo URL công khai).</summary>
    public string? MessageType { get; set; }

    public string? Text { get; set; }
    public string? ImageUrl { get; set; }
}

public class ZaloOARetryMessageRequest
{
    public int? AgentUserId { get; set; }
}

public class ZaloOAMarkReadRequest
{
    public int UserId { get; set; }
}

public class ZaloOASetStatusRequest
{
    /// <summary>OPEN / PENDING / CLOSED.</summary>
    public string Status { get; set; } = "";
    public int UserId { get; set; }
}

public class ZaloOAHistoryImportRequest
{
    public int? MaxConversations { get; set; }
    public int? MaxMessagesPerUser { get; set; }
    public int? RequestedByUserId { get; set; }
}

public class ZaloOAHistoryImportResult
{
    public int ConversationsScanned { get; set; }
    public int Customers { get; set; }
    public int MessagesInserted { get; set; }
    public int MessagesDuplicate { get; set; }
    public int Errors { get; set; }
}

/// <summary>Kết quả nghiệp vụ chuẩn hoá để controller map sang HTTP + ApiResponse.</summary>
public class ZaloOAServiceResult<T>
{
    public bool Success { get; set; }
    public string? ErrorCode { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public int HttpStatus { get; set; } = 200;

    public static ZaloOAServiceResult<T> Ok(T data, string message = "OK") => new() { Success = true, Data = data, Message = message };

    public static ZaloOAServiceResult<T> Fail(string errorCode, string message, int httpStatus, T? data = default)
        => new() { Success = false, ErrorCode = errorCode, Message = message, HttpStatus = httpStatus, Data = data };
}
