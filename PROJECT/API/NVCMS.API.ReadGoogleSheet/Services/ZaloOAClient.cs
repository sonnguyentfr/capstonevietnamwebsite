using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Http;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using System.Globalization;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    /// <summary>
    /// Gọi Zalo Official Account Open API.
    /// Tài liệu: developers.zalo.me/docs/official-account (đối chiếu 09/2026):
    ///   - Gửi tin tư vấn : POST v3.0/oa/message/cs          (khách phải tương tác trong 7 ngày, nếu không lỗi -230)
    ///   - Upload ảnh/file: POST v2.0/oa/upload/image | file  (multipart, field "file"; ảnh jpg/png ≤1MB, file ≤5MB)
    ///   - Thông tin user : GET  v3.0/oa/user/detail?data={"user_id":"..."}
    ///   - Thông tin OA   : GET  v2.0/oa/getoa
    ///   - Lịch sử chat   : GET  v2.0/oa/listrecentchat | conversation (tối đa 10 bản ghi / request)
    /// Mọi request gửi access token qua header "access_token". Lỗi -216/-220 (token hỏng/hết hạn)
    /// → refresh token và gọi lại đúng 1 lần.
    /// </summary>
    public interface IZaloOAClient
    {
        Task<ZaloOAApiResult<ZaloOASendResult>> SendTextAsync(string userId, string text, CancellationToken cancellationToken = default);
        Task<ZaloOAApiResult<ZaloOASendResult>> SendImageAsync(string userId, string? attachmentId, string? imageUrl, string? caption, CancellationToken cancellationToken = default);
        Task<ZaloOAApiResult<ZaloOASendResult>> SendFileAsync(string userId, string fileToken, CancellationToken cancellationToken = default);

        /// <summary>Gửi object "message" bất kỳ theo format Zalo (dùng cho loại tin chưa có hàm riêng).</summary>
        Task<ZaloOAApiResult<ZaloOASendResult>> SendMessageAsync(string userId, object message, CancellationToken cancellationToken = default);

        /// <summary>Trả về attachment_id.</summary>
        Task<ZaloOAApiResult<string>> UploadImageAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default);

        /// <summary>Trả về token file.</summary>
        Task<ZaloOAApiResult<string>> UploadFileAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default);

        Task<ZaloOAApiResult<ZaloOAUserDetail>> GetUserDetailAsync(string userId, CancellationToken cancellationToken = default);
        Task<ZaloOAApiResult<ZaloOAInfo>> GetOAInfoAsync(CancellationToken cancellationToken = default);
        Task<ZaloOAApiResult<List<ZaloOAHistoryMessage>>> GetRecentChatsAsync(int offset, int count, CancellationToken cancellationToken = default);
        Task<ZaloOAApiResult<List<ZaloOAHistoryMessage>>> GetConversationAsync(string userId, int offset, int count, CancellationToken cancellationToken = default);
    }

    public class ZaloOAClient : IZaloOAClient
    {
        public const int MaxHistoryPageSize = 10;
        public const int MaxTextLength = 2000;

        private readonly BaseApi _api;
        private readonly IZaloService _tokenService;
        private readonly ZaloSettings _settings;
        private readonly ILogger<ZaloOAClient> _logger;

        public ZaloOAClient(BaseApi api, IZaloService tokenService, IOptions<ZaloSettings> settings, ILogger<ZaloOAClient> logger)
        {
            _api = api;
            _tokenService = tokenService;
            _settings = settings.Value;
            _logger = logger;
        }

        private string BaseUrl => (string.IsNullOrWhiteSpace(_settings.OpenApiBaseUrl) ? "https://openapi.zalo.me" : _settings.OpenApiBaseUrl).TrimEnd('/');

        // ── Gửi tin ─────────────────────────────────────────────────────────

        public Task<ZaloOAApiResult<ZaloOASendResult>> SendTextAsync(string userId, string text, CancellationToken cancellationToken = default)
            => SendMessageAsync(userId, new { text }, cancellationToken);

        public Task<ZaloOAApiResult<ZaloOASendResult>> SendImageAsync(string userId, string? attachmentId, string? imageUrl, string? caption, CancellationToken cancellationToken = default)
        {
            // Zalo: dùng attachment_id HOẶC url, không dùng cả hai.
            object element = !string.IsNullOrWhiteSpace(attachmentId)
                ? new { media_type = "image", attachment_id = attachmentId }
                : new { media_type = "image", url = imageUrl };

            var attachment = new
            {
                type = "template",
                payload = new { template_type = "media", elements = new[] { element } }
            };

            object message = string.IsNullOrWhiteSpace(caption)
                ? new { attachment }
                : new { text = caption, attachment };

            return SendMessageAsync(userId, message, cancellationToken);
        }

        public Task<ZaloOAApiResult<ZaloOASendResult>> SendFileAsync(string userId, string fileToken, CancellationToken cancellationToken = default)
            => SendMessageAsync(userId, new { attachment = new { type = "file", payload = new { token = fileToken } } }, cancellationToken);

        public async Task<ZaloOAApiResult<ZaloOASendResult>> SendMessageAsync(string userId, object message, CancellationToken cancellationToken = default)
        {
            var body = new { recipient = new { user_id = userId }, message };
            var url = $"{BaseUrl}/v3.0/oa/message/cs";

            var result = await ExecuteAsync("SendMessage", url,
                token => _api.PostJsonAsync<object, JsonElement>(url, body, TokenHeader(token)), cancellationToken);

            return Map(result, data => new ZaloOASendResult
            {
                MessageId = GetString(data, "message_id"),
                UserId = GetString(data, "user_id"),
                SentTime = ParseEpochMs(GetString(data, "sent_time"))
            });
        }

        // ── Upload ──────────────────────────────────────────────────────────

        public async Task<ZaloOAApiResult<string>> UploadImageAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrl}/v2.0/oa/upload/image";
            var result = await UploadAsync("UploadImage", url, content, fileName, contentType, cancellationToken);
            return Map(result, data => GetString(data, "attachment_id") ?? "");
        }

        public async Task<ZaloOAApiResult<string>> UploadFileAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrl}/v2.0/oa/upload/file";
            var result = await UploadAsync("UploadFile", url, content, fileName, contentType, cancellationToken);
            return Map(result, data => GetString(data, "token") ?? "");
        }

        private Task<ZaloOAApiResult<JsonElement>> UploadAsync(string operation, string url, Stream content, string fileName, string contentType, CancellationToken cancellationToken)
        {
            if (!content.CanSeek)
                throw new ArgumentException("Upload stream phải seek được (để gửi lại sau khi refresh token).", nameof(content));

            var start = content.Position;
            return ExecuteAsync(operation, url, token =>
            {
                content.Position = start;
                return _api.PostMultipartAsync<JsonElement>(url, "file", content, fileName, contentType, TokenHeader(token));
            }, cancellationToken);
        }

        // ── Thông tin ───────────────────────────────────────────────────────

        public async Task<ZaloOAApiResult<ZaloOAUserDetail>> GetUserDetailAsync(string userId, CancellationToken cancellationToken = default)
        {
            var data = JsonSerializer.Serialize(new { user_id = userId });
            var url = $"{BaseUrl}/v3.0/oa/user/detail?data={Uri.EscapeDataString(data)}";

            var result = await ExecuteAsync("GetUserDetail", url,
                token => _api.GetJsonAsync<JsonElement>(url, TokenHeader(token)), cancellationToken);

            return Map(result, ParseUserDetail);
        }

        public async Task<ZaloOAApiResult<ZaloOAInfo>> GetOAInfoAsync(CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrl}/v2.0/oa/getoa";
            var result = await ExecuteAsync("GetOAInfo", url,
                token => _api.GetJsonAsync<JsonElement>(url, TokenHeader(token)), cancellationToken);

            return Map(result, d => new ZaloOAInfo
            {
                OAId = GetString(d, "oaid") ?? GetString(d, "oa_id"),
                Name = GetString(d, "name"),
                Avatar = GetString(d, "avatar"),
                Raw = d.Clone()
            });
        }

        public async Task<ZaloOAApiResult<List<ZaloOAHistoryMessage>>> GetRecentChatsAsync(int offset, int count, CancellationToken cancellationToken = default)
        {
            count = Math.Clamp(count, 1, MaxHistoryPageSize);
            var data = $"{{\"offset\":{Math.Max(0, offset)},\"count\":{count}}}";
            var url = $"{BaseUrl}/v2.0/oa/listrecentchat?data={Uri.EscapeDataString(data)}";

            var result = await ExecuteAsync("ListRecentChat", url,
                token => _api.GetJsonAsync<JsonElement>(url, TokenHeader(token)), cancellationToken);

            return Map(result, ParseHistory);
        }

        public async Task<ZaloOAApiResult<List<ZaloOAHistoryMessage>>> GetConversationAsync(string userId, int offset, int count, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId) || !userId.All(char.IsAsciiDigit))
                return ZaloOAApiResult<List<ZaloOAHistoryMessage>>.Fail(ZaloApiErrorCodes.InvalidResponse, "user_id không hợp lệ");

            count = Math.Clamp(count, 1, MaxHistoryPageSize);
            // Theo ví dụ tài liệu Zalo: user_id dạng số trong tham số data.
            var data = $"{{\"user_id\":{userId},\"offset\":{Math.Max(0, offset)},\"count\":{count}}}";
            var url = $"{BaseUrl}/v2.0/oa/conversation?data={Uri.EscapeDataString(data)}";

            var result = await ExecuteAsync("Conversation", url,
                token => _api.GetJsonAsync<JsonElement>(url, TokenHeader(token)), cancellationToken);

            return Map(result, ParseHistory);
        }

        // ── Lõi: token + retry + chuẩn hoá lỗi ─────────────────────────────

        private async Task<ZaloOAApiResult<JsonElement>> ExecuteAsync(
            string operation, string url, Func<string, Task<JsonElement>> call, CancellationToken cancellationToken)
        {
            var endpoint = StripQuery(url);

            for (var attempt = 0; attempt < 2; attempt++)
            {
                string token;
                try
                {
                    token = await _tokenService.GetValidAccessTokenAsync(forceRefresh: attempt > 0, cancellationToken);
                }
                catch (ZaloTokenUnavailableException ex)
                {
                    _logger.LogError("Zalo OA {Operation}: không có access token hợp lệ - {Reason}", operation, ex.Message);
                    return ZaloOAApiResult<JsonElement>.Fail(ZaloApiErrorCodes.TokenUnavailable, ex.Message);
                }

                JsonElement json;
                try
                {
                    json = await call(token);
                }
                catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogError("Zalo OA {Operation} timeout. Endpoint={Endpoint}", operation, endpoint);
                    return ZaloOAApiResult<JsonElement>.Fail(ZaloApiErrorCodes.Timeout, "Zalo API không phản hồi (timeout).");
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Zalo OA {Operation} lỗi kết nối. Endpoint={Endpoint}", operation, endpoint);
                    return ZaloOAApiResult<JsonElement>.Fail(ZaloApiErrorCodes.HttpError, "Không kết nối được Zalo API.");
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Zalo OA {Operation} trả JSON không hợp lệ. Endpoint={Endpoint}", operation, endpoint);
                    return ZaloOAApiResult<JsonElement>.Fail(ZaloApiErrorCodes.InvalidResponse, "Phản hồi Zalo không hợp lệ.");
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    // BaseApi ném Exception(body) khi HTTP status không phải 2xx
                    var body = Truncate(ex.Message, 1000);
                    _logger.LogError("Zalo OA {Operation} HTTP lỗi. Endpoint={Endpoint} Body={Body}", operation, endpoint, body);
                    return ZaloOAApiResult<JsonElement>.Fail(ZaloApiErrorCodes.HttpError, "Zalo API trả lỗi HTTP.", body);
                }

                var raw = json.ValueKind == JsonValueKind.Undefined ? null : json.GetRawText();
                var error = GetInt(json, "error") ?? ZaloApiErrorCodes.InvalidResponse;
                var message = GetString(json, "message");

                if (error == ZaloApiErrorCodes.Success)
                {
                    var data = json.TryGetProperty("data", out var d) ? d.Clone() : default;
                    return new ZaloOAApiResult<JsonElement> { ErrorCode = 0, Data = data, RawJson = raw };
                }

                if (ZaloApiErrorCodes.IsTokenError(error) && attempt == 0)
                {
                    _logger.LogWarning("Zalo OA {Operation}: token lỗi {ErrorCode} ({Message}) → refresh và gọi lại.", operation, error, message);
                    continue;
                }

                _logger.LogWarning("Zalo OA {Operation} lỗi. Endpoint={Endpoint} ErrorCode={ErrorCode} Message={Message}",
                    operation, endpoint, error, message);
                return ZaloOAApiResult<JsonElement>.Fail(error, message, raw);
            }

            return ZaloOAApiResult<JsonElement>.Fail(ZaloApiErrorCodes.TokenUnavailable, "Access token vẫn lỗi sau khi refresh.");
        }

        private static Dictionary<string, string> TokenHeader(string token) => new() { { "access_token", token } };

        private static ZaloOAApiResult<TOut> Map<TOut>(ZaloOAApiResult<JsonElement> source, Func<JsonElement, TOut> map)
        {
            if (!source.Success)
                return ZaloOAApiResult<TOut>.Fail(source.ErrorCode, source.ErrorMessage, source.RawJson);

            try
            {
                return new ZaloOAApiResult<TOut> { ErrorCode = 0, Data = map(source.Data), RawJson = source.RawJson };
            }
            catch (Exception ex) when (ex is InvalidOperationException or FormatException or JsonException)
            {
                return ZaloOAApiResult<TOut>.Fail(ZaloApiErrorCodes.InvalidResponse, "Không đọc được dữ liệu Zalo: " + ex.Message, source.RawJson);
            }
        }

        // ── Parse ───────────────────────────────────────────────────────────

        internal static ZaloOAUserDetail ParseUserDetail(JsonElement d)
        {
            var detail = new ZaloOAUserDetail
            {
                UserId = GetString(d, "user_id"),
                UserIdByApp = GetString(d, "user_id_by_app"),
                DisplayName = GetString(d, "display_name"),
                UserAlias = GetString(d, "user_alias"),
                IsFollower = GetBool(d, "user_is_follower"),
                IsSensitive = GetBool(d, "is_sensitive"),
                RawJson = d.ValueKind == JsonValueKind.Undefined ? null : d.GetRawText()
            };

            detail.Avatar = GetString(d, "avatar");
            if (d.ValueKind == JsonValueKind.Object && d.TryGetProperty("avatars", out var avatars) && avatars.ValueKind == JsonValueKind.Object)
                detail.Avatar = GetString(avatars, "240") ?? GetString(avatars, "120") ?? detail.Avatar;

            if (d.ValueKind == JsonValueKind.Object && d.TryGetProperty("shared_info", out var shared) && shared.ValueKind == JsonValueKind.Object)
            {
                detail.SharedName = NullIfEmpty(GetString(shared, "name"));
                detail.SharedPhone = NullIfEmpty(GetString(shared, "phone"));   // Zalo có thể trả dạng số
                detail.SharedDob = NullIfEmpty(GetString(shared, "user_dob"));
                var parts = new[] { GetString(shared, "address"), GetString(shared, "district"), GetString(shared, "city") }
                    .Where(x => !string.IsNullOrWhiteSpace(x));
                detail.SharedAddress = NullIfEmpty(string.Join(", ", parts));
            }

            if (d.ValueKind == JsonValueKind.Object && d.TryGetProperty("tags_and_notes_info", out var tn) && tn.ValueKind == JsonValueKind.Object)
            {
                detail.TagNames = GetStringArray(tn, "tag_names");
                detail.Notes = GetStringArray(tn, "notes");
            }

            return detail;
        }

        internal static List<ZaloOAHistoryMessage> ParseHistory(JsonElement data)
        {
            var list = new List<ZaloOAHistoryMessage>();
            if (data.ValueKind != JsonValueKind.Array)
                return list;

            foreach (var item in data.EnumerateArray())
            {
                list.Add(new ZaloOAHistoryMessage
                {
                    Src = GetInt(item, "src") ?? 0,
                    Time = GetLong(item, "time") ?? 0,
                    Type = GetString(item, "type"),
                    Message = GetString(item, "message"),
                    MessageId = GetString(item, "message_id"),
                    FromId = GetString(item, "from_id"),
                    ToId = GetString(item, "to_id"),
                    FromDisplayName = GetString(item, "from_display_name"),
                    FromAvatar = GetString(item, "from_avatar"),
                    ToDisplayName = GetString(item, "to_display_name"),
                    ToAvatar = GetString(item, "to_avatar"),
                    Url = GetString(item, "url"),
                    Thumb = GetString(item, "thumb"),
                    RawJson = item.GetRawText()
                });
            }
            return list;
        }

        // ── JSON helpers (Zalo trả lẫn số/chuỗi cho cùng 1 field) ──────────

        internal static string? GetString(JsonElement e, string name)
        {
            if (e.ValueKind != JsonValueKind.Object || !e.TryGetProperty(name, out var p))
                return null;
            return p.ValueKind switch
            {
                JsonValueKind.String => p.GetString(),
                JsonValueKind.Number => p.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => null
            };
        }

        internal static int? GetInt(JsonElement e, string name)
        {
            var s = GetString(e, name);
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) ? v : null;
        }

        internal static long? GetLong(JsonElement e, string name)
        {
            var s = GetString(e, name);
            return long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) ? v : null;
        }

        private static bool? GetBool(JsonElement e, string name)
        {
            var s = GetString(e, name);
            return s switch { "true" or "1" => true, "false" or "0" => false, _ => null };
        }

        private static List<string> GetStringArray(JsonElement e, string name)
        {
            var list = new List<string>();
            if (e.TryGetProperty(name, out var arr) && arr.ValueKind == JsonValueKind.Array)
                foreach (var x in arr.EnumerateArray())
                    if (x.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(x.GetString()))
                        list.Add(x.GetString()!);
            return list;
        }

        internal static DateTime? ParseEpochMs(string? ms)
        {
            if (!long.TryParse(ms, out var v) || v <= 0) return null;
            try { return DateTimeOffset.FromUnixTimeMilliseconds(v).UtcDateTime; }
            catch (ArgumentOutOfRangeException) { return null; }
        }

        private static string? NullIfEmpty(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

        private static string StripQuery(string url)
        {
            var i = url.IndexOf('?');
            return i < 0 ? url : url[..i];
        }

        private static string Truncate(string? s, int max) => s == null ? "" : (s.Length <= max ? s : s[..max]);
    }
}
