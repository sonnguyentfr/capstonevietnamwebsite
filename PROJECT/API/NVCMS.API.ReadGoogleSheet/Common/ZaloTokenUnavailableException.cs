namespace NVCMS.API.ReadGoogleSheet.Common;

/// <summary>Không có / không lấy được / không giải mã được access token Zalo hợp lệ.</summary>
public class ZaloTokenUnavailableException : Exception
{
    public ZaloTokenUnavailableException(string message, Exception? inner = null) : base(message, inner) { }
}
