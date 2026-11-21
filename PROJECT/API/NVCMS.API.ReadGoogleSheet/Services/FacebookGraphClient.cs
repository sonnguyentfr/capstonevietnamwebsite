using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;

namespace NVCMS.API.ReadGoogleSheet.Services;

/// <summary>Lỗi do Graph API trả về (token hết hạn, thiếu quyền...).</summary>
public class FacebookApiException : Exception
{
    public int Code { get; }
    public int HttpStatus { get; }

    public FacebookApiException(string message, int code, int httpStatus) : base(message)
    {
        Code = code;
        HttpStatus = httpStatus;
    }
}

/// <summary>
/// FB-SYNC 19/11/2026: đọc bài đã đăng của Facebook Page qua Graph API và tải ảnh của bài.
/// Token gửi qua header Authorization để không lọt vào log URL.
/// </summary>
public class FacebookGraphClient
{
    private const int PageSize = 25;
    private const int MaxPages = 20;
    private const long MaxImageBytes = 10 * 1024 * 1024;

    private const string PostFields =
        "id,message,created_time,permalink_url,full_picture,status_type," +
        "attachments{media_type,type,url,media,subattachments{media_type,type,url,media}}";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _http;
    private readonly FacebookSyncSettings _settings;
    private readonly ILogger<FacebookGraphClient> _logger;

    public FacebookGraphClient(
        HttpClient http,
        IOptions<FacebookSyncSettings> settings,
        ILogger<FacebookGraphClient> logger)
    {
        _http = http;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <summary>Lấy mọi bài Page đã đăng từ <paramref name="sinceUtc"/> tới nay (mới nhất trước).</summary>
    public async Task<List<FbPost>> GetPublishedPostsAsync(DateTime sinceUtc, CancellationToken ct)
    {
        var version = string.IsNullOrWhiteSpace(_settings.GraphVersion) ? "v25.0" : _settings.GraphVersion.Trim();
        var since = new DateTimeOffset(DateTime.SpecifyKind(sinceUtc, DateTimeKind.Utc)).ToUnixTimeSeconds();

        var url = $"https://graph.facebook.com/{version}/{Uri.EscapeDataString(_settings.PageId.Trim())}/published_posts" +
                  $"?fields={Uri.EscapeDataString(PostFields)}&limit={PageSize}&since={since}";

        var proof = BuildAppSecretProof();
        if (proof != null)
            url += "&appsecret_proof=" + proof;

        var posts = new List<FbPost>();
        string? next = url;

        for (var page = 0; page < MaxPages && !string.IsNullOrEmpty(next); page++)
        {
            var body = await GetAsync(next, ct);
            var parsed = JsonSerializer.Deserialize<FbPagedResponse<FbPost>>(body, JsonOptions);
            if (parsed == null || parsed.Data.Count == 0)
                break;

            posts.AddRange(parsed.Data);
            next = RemoveQueryParam(parsed.Paging?.Next, "access_token");
        }

        return posts;
    }

    /// <summary>
    /// Tải một ảnh của bài. Chỉ nhận https từ máy chủ ảnh của Facebook, tối đa 10 MB,
    /// và phải đúng là ảnh (kiểm tra byte đầu). Trả null nếu không hợp lệ.
    /// </summary>
    public async Task<(byte[] Bytes, string Ext)?> DownloadImageAsync(string url, CancellationToken ct)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps || !IsFacebookMediaHost(uri.Host))
        {
            _logger.LogWarning("FB-SYNC: bỏ qua ảnh có địa chỉ không hợp lệ ({Host})", uri?.Host);
            return null;
        }

        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("FB-SYNC: tải ảnh lỗi HTTP {Status}", (int)response.StatusCode);
            return null;
        }

        if (response.Content.Headers.ContentLength > MaxImageBytes)
            return null;

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var ms = new MemoryStream();
        var buffer = new byte[81920];
        int read;
        while ((read = await stream.ReadAsync(buffer, ct)) > 0)
        {
            ms.Write(buffer, 0, read);
            if (ms.Length > MaxImageBytes)
                return null;
        }

        var bytes = ms.ToArray();
        var ext = DetectImageExt(bytes);
        if (ext == null)
            return null;

        return (bytes, ext);
    }

    private async Task<string> GetAsync(string url, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.PageAccessToken.Trim());

        using var response = await _http.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.IsSuccessStatusCode)
            return body;

        var (message, code) = ParseError(body);
        var hint = code switch
        {
            190 => " (Page Access Token hết hạn hoặc không hợp lệ - cần lấy token mới và cập nhật FacebookSync:PageAccessToken)",
            10 or 200 => " (token thiếu quyền - cần pages_read_engagement / pages_show_list trên Page này)",
            4 or 17 or 32 or 613 => " (vượt giới hạn gọi API - job sẽ thử lại ở lần chạy sau)",
            _ => string.Empty
        };

        throw new FacebookApiException(
            $"Graph API lỗi HTTP {(int)response.StatusCode}, code {code}: {message}{hint}",
            code, (int)response.StatusCode);
    }

    private static (string Message, int Code) ParseError(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("error", out var err))
            {
                var msg = err.TryGetProperty("message", out var m) ? m.GetString() ?? string.Empty : string.Empty;
                var code = err.TryGetProperty("code", out var c) && c.TryGetInt32(out var n) ? n : 0;
                return (msg, code);
            }
        }
        catch (JsonException)
        {
            // body không phải JSON
        }

        return (body.Length > 300 ? body[..300] : body, 0);
    }

    private string? BuildAppSecretProof()
    {
        if (string.IsNullOrWhiteSpace(_settings.AppSecret))
            return null;

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.AppSecret.Trim()));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_settings.PageAccessToken.Trim()));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    /// <summary>Link "next" của Graph có kèm access_token - bỏ đi vì token đã gửi qua header.</summary>
    private static string? RemoveQueryParam(string? url, string name)
    {
        if (string.IsNullOrEmpty(url))
            return null;

        var q = url.IndexOf('?');
        if (q < 0)
            return url;

        var kept = url[(q + 1)..]
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Where(p => !p.StartsWith(name + "=", StringComparison.OrdinalIgnoreCase));

        return url[..q] + "?" + string.Join("&", kept);
    }

    private static bool IsFacebookMediaHost(string host) =>
        host.EndsWith(".fbcdn.net", StringComparison.OrdinalIgnoreCase) ||
        host.EndsWith(".facebook.com", StringComparison.OrdinalIgnoreCase) ||
        host.EndsWith(".fbsbx.com", StringComparison.OrdinalIgnoreCase);

    private static string? DetectImageExt(byte[] b)
    {
        if (b.Length < 12) return null;
        if (b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF) return "jpg";
        if (b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47) return "png";
        if (b[0] == 0x47 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x38) return "gif";
        if (b[0] == 0x52 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x46 &&
            b[8] == 0x57 && b[9] == 0x45 && b[10] == 0x42 && b[11] == 0x50) return "webp";
        return null;
    }
}
