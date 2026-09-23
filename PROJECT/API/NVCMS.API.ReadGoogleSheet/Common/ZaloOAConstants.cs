namespace NVCMS.API.ReadGoogleSheet.Common;

public static class ZaloOADirection
{
    public const string In = "IN";
    public const string Out = "OUT";
    public const string System = "SYS";
}

public static class ZaloOASenderType
{
    public const string Customer = "CUSTOMER";
    public const string OA = "OA";
    public const string Agent = "AGENT";
    public const string System = "SYSTEM";
}

public static class ZaloOAMessageType
{
    public const string Text = "TEXT";
    public const string Image = "IMAGE";
    public const string File = "FILE";
    public const string Video = "VIDEO";
    public const string Audio = "AUDIO";
    public const string Sticker = "STICKER";
    public const string Location = "LOCATION";
    public const string Link = "LINK";
    public const string Contact = "CONTACT";
    public const string Event = "EVENT";
    public const string Other = "OTHER";
}

public static class ZaloOAMessageStatus
{
    public const string Pending = "PENDING";
    public const string Received = "RECEIVED";
    public const string Sent = "SENT";
    public const string Delivered = "DELIVERED";
    public const string Seen = "SEEN";
    public const string Failed = "FAILED";
    public const string Unknown = "UNKNOWN";
}

public static class ZaloOAConversationStatus
{
    public const string Open = "OPEN";
    public const string Pending = "PENDING";
    public const string Closed = "CLOSED";

    public static readonly string[] All = [Open, Pending, Closed];
}

public static class ZaloOAWebhookStatus
{
    public const string Pending = "PENDING";
    public const string Processed = "PROCESSED";
    public const string Failed = "FAILED";
    public const string Ignored = "IGNORED";
}

/// <summary>Mã lỗi trả về trong ApiResponse.ErrorCode của các API Zalo OA Chat.</summary>
public static class ZaloOAErrorCodes
{
    public const string ValidationError = "VALIDATION_ERROR";
    public const string NotFound = "NOT_FOUND";
    public const string ZaloApiError = "ZALO_API_ERROR";
    public const string ZaloTimeout = "ZALO_TIMEOUT";
    public const string ZaloTokenUnavailable = "ZALO_TOKEN_UNAVAILABLE";
    public const string ZaloUserNotInteracted = "ZALO_USER_NOT_INTERACTED";
    public const string InvalidSignature = "INVALID_SIGNATURE";
    public const string NotConfigured = "NOT_CONFIGURED";
    public const string InternalError = "INTERNAL_ERROR";
}

/// <summary>
/// Mã lỗi Zalo OA API (developers.zalo.me/docs/official-account/phu-luc/ma-loi).
/// Mã âm = lỗi Zalo; mã dương dưới đây là mã nội bộ khi không nhận được phản hồi hợp lệ.
/// </summary>
public static class ZaloApiErrorCodes
{
    public const int Success = 0;
    public const int RateLimit = -32;
    public const int AccessTokenInvalid = -216;
    public const int AccessTokenExpired = -220;
    public const int UserNotInteracted7Days = -230;
    public const int UserNotInteracted = -232;

    // Mã nội bộ
    public const int Timeout = 900001;
    public const int HttpError = 900002;
    public const int InvalidResponse = 900003;
    public const int TokenUnavailable = 900004;

    public static bool IsTokenError(int code) => code is AccessTokenInvalid or AccessTokenExpired;

    public static bool IsInteractionWindowError(int code) => code is UserNotInteracted7Days or UserNotInteracted;
}
