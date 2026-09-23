using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Models.ZaloOA;

/// <summary>Kết quả 1 lần gọi Zalo OA Open API (đã chuẩn hoá lỗi).</summary>
public class ZaloOAApiResult<T>
{
    public bool Success => ErrorCode == 0;

    /// <summary>0 = OK; mã âm = mã lỗi Zalo; 9000xx = mã nội bộ (xem ZaloApiErrorCodes).</summary>
    public int ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public T? Data { get; set; }

    /// <summary>JSON Zalo trả về (không chứa token) - lưu để audit.</summary>
    public string? RawJson { get; set; }

    public static ZaloOAApiResult<T> Fail(int code, string? message, string? raw = null)
        => new() { ErrorCode = code, ErrorMessage = message, RawJson = raw };
}

/// <summary>data của API gửi tin tư vấn v3.0/oa/message/cs.</summary>
public class ZaloOASendResult
{
    public string? MessageId { get; set; }
    public string? UserId { get; set; }
    public DateTime? SentTime { get; set; }
}

/// <summary>data của v2.0/oa/getoa (chỉ các field dùng tới).</summary>
public class ZaloOAInfo
{
    public string? OAId { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public JsonElement Raw { get; set; }
}

/// <summary>data của v3.0/oa/user/detail.</summary>
public class ZaloOAUserDetail
{
    public string? UserId { get; set; }
    public string? UserIdByApp { get; set; }
    public string? DisplayName { get; set; }
    public string? UserAlias { get; set; }
    public string? Avatar { get; set; }
    public bool? IsFollower { get; set; }
    public bool? IsSensitive { get; set; }
    public string? SharedName { get; set; }
    public string? SharedPhone { get; set; }
    public string? SharedAddress { get; set; }
    public string? SharedDob { get; set; }
    public List<string> TagNames { get; set; } = new();
    public List<string> Notes { get; set; } = new();
    public string? RawJson { get; set; }
}

/// <summary>1 phần tử của v2.0/oa/listrecentchat và v2.0/oa/conversation.</summary>
public class ZaloOAHistoryMessage
{
    /// <summary>0 = OA gửi khách, 1 = khách gửi OA.</summary>
    public int Src { get; set; }
    public long Time { get; set; }
    public string? Type { get; set; }
    public string? Message { get; set; }
    public string? MessageId { get; set; }
    public string? FromId { get; set; }
    public string? ToId { get; set; }
    public string? FromDisplayName { get; set; }
    public string? FromAvatar { get; set; }
    public string? ToDisplayName { get; set; }
    public string? ToAvatar { get; set; }
    public string? Url { get; set; }
    public string? Thumb { get; set; }
    public string RawJson { get; set; } = "";
}
