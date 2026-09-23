using NVCMS.API.ReadGoogleSheet.Models;
using System.Threading.Tasks;

public interface IZaloService
{
    Task<ZaloTokenResponse> GetAndSaveTokenAsync(string code);
    Task<ZaloTokenResponse> RefreshAndSaveTokenAsync(string refresh_token);

    /// <summary>Token mới nhất (đã giải mã). Ném InvalidOperationException nếu chưa có token.</summary>
    Task<Zalo_Token> GetLastTokenAsync();

    Task<ZaloMessageResponse> SendTemplateAsync<T>(ZaloMessageRequest<T> request);

    /// <summary>
    /// Access token còn hạn: tự refresh khi sắp hết hạn hoặc khi <paramref name="forceRefresh"/> = true
    /// (Zalo báo -216/-220). Ném ZaloTokenUnavailableException nếu không có token dùng được.
    /// </summary>
    Task<string> GetValidAccessTokenAsync(bool forceRefresh = false, CancellationToken cancellationToken = default);

    /// <summary>Thời điểm hết hạn của token mới nhất (UTC), không kèm giá trị token.</summary>
    Task<ZaloTokenStatus?> GetTokenStatusAsync();
}

public class ZaloTokenStatus
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired { get; set; }
    public bool IsEncrypted { get; set; }
}
