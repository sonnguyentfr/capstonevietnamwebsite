namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IShortyUrlRepository
{
    /// <summary>Tìm real_url theo short_url. Trả về null nếu không tồn tại.</summary>
    Task<string?> GetRealUrlAsync(string shortUrl);

    /// <summary>Tăng short_clicks lên 1 (fire-and-forget safe).</summary>
    Task IncrementClickAsync(string shortUrl);
}
