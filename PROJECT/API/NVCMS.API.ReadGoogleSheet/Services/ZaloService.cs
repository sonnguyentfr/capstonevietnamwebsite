using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Http;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Locking;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Security;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;
using System.Text.Json;
namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class ZaloService : IZaloService
    {
        private readonly BaseApi _api;
        private readonly IZaloTokenRepository _repository;
        private readonly IZaloMessageLogRepository _logRepository;
        private readonly IZnsSendLogRepository _znsSendLogRepository;
        private readonly ZaloSettings _config;
        private readonly IZaloTokenProtector _protector;
        private readonly IZaloTokenRefreshLock _refreshLock;
        private readonly IZaloTokenColumnInspector _columnInspector;
        private readonly ILogger<ZaloService> _logger;

        /// <summary>Zalo: access token sống 25h (expires_in = "90000").</summary>
        private const int DefaultExpiresInSeconds = 90000;

        public ZaloService(
            BaseApi api,
            IZaloTokenRepository repository,
            IZaloMessageLogRepository logRepository,
            IZnsSendLogRepository znsSendLogRepository,
            IOptions<ZaloSettings> config,
            IZaloTokenProtector protector,
            IZaloTokenRefreshLock refreshLock,
            IZaloTokenColumnInspector columnInspector,
            ILogger<ZaloService> logger)
        {
            _api = api;
            _repository = repository;
            _logRepository = logRepository;
            _znsSendLogRepository = znsSendLogRepository;
            _config = config.Value;
            _protector = protector;
            _refreshLock = refreshLock;
            _columnInspector = columnInspector;
            _logger = logger;
        }

        public async Task<ZaloTokenResponse> GetAndSaveTokenAsync(string code)
        {
            var token = await _api.PostFormAsync<ZaloTokenResponse>(
                _config.TokenEndpoint,
                new()
                {
                { "code", code },
                { "app_id", _config.AppId },
                { "grant_type", "authorization_code" },
                { "app_secret", _config.AppSecret }
                },
                new()
                {
                { "secret_key", _config.AppSecret }
                });

            LogTokenError("authorization_code", token);
            await SaveToken(token);

            return token;
        }

        /// <summary>
        /// Refresh token (job ZnsRefreshTokenJob + API get-refresh-token).
        /// Chạy trong khoá; nếu refresh token truyền vào đã bị một tiến trình khác thay
        /// (token mới còn hạn) thì không gọi Zalo nữa để tránh dùng refresh token đã bị huỷ.
        /// </summary>
        public async Task<ZaloTokenResponse> RefreshAndSaveTokenAsync(string refreshToken)
        {
            await using (await _refreshLock.AcquireAsync())
            {
                var latest = await TryGetLastTokenAsync();
                if (latest != null
                    && !string.Equals(latest.RefreshToken, refreshToken, StringComparison.Ordinal)
                    && !NeedsRefresh(latest))
                {
                    _logger.LogInformation(
                        "Zalo token đã được refresh bởi tiến trình khác (TokenId={TokenId}), bỏ qua lần refresh này.",
                        latest.Id);
                    return new ZaloTokenResponse
                    {
                        access_token = latest.AccessToken,
                        refresh_token = latest.RefreshToken,
                        expires_in = latest.ExpiresIn
                    };
                }

                return await RefreshCoreAsync(refreshToken);
            }
        }

        public async Task<Zalo_Token> GetLastTokenAsync()
        {
            var stored = await _repository.GetLastAsync();
            if (stored == null)
                return null!;

            // Trả bản sao đã giải mã - KHÔNG sửa entity đang được EF theo dõi
            // (nếu sửa, lần SaveChanges sau sẽ ghi plaintext đè lên dòng cũ).
            return new Zalo_Token
            {
                Id = stored.Id,
                AccessToken = _protector.Unprotect(stored.AccessToken),
                RefreshToken = _protector.Unprotect(stored.RefreshToken),
                ExpiresIn = stored.ExpiresIn,
                CreatedAt = stored.CreatedAt
            };
        }

        public async Task<string> GetValidAccessTokenAsync(bool forceRefresh = false, CancellationToken cancellationToken = default)
        {
            var token = await TryGetLastTokenAsync()
                        ?? throw new ZaloTokenUnavailableException(
                            "Chưa có Zalo access token trong bảng Zalo_Token. Cấp token qua POST /api/Zalo/get-access-token.");

            if (!forceRefresh && !NeedsRefresh(token))
                return token.AccessToken;

            await using (await _refreshLock.AcquireAsync(cancellationToken))
            {
                // Kiểm tra lại sau khi có khoá: tiến trình khác có thể vừa refresh xong.
                var latest = await TryGetLastTokenAsync() ?? token;
                if (latest.Id != token.Id && !NeedsRefresh(latest))
                    return latest.AccessToken;
                if (!forceRefresh && !NeedsRefresh(latest))
                    return latest.AccessToken;

                if (string.IsNullOrWhiteSpace(latest.RefreshToken))
                    throw new ZaloTokenUnavailableException("Zalo token không có refresh token.");

                _logger.LogInformation("Zalo access token cần refresh (TokenId={TokenId}, Force={Force}, ExpiresAt={ExpiresAt:o}).",
                    latest.Id, forceRefresh, GetExpiresAt(latest));

                var refreshed = await RefreshCoreAsync(latest.RefreshToken);
                if (string.IsNullOrWhiteSpace(refreshed?.access_token))
                    throw new ZaloTokenUnavailableException(
                        $"Refresh Zalo token thất bại (error={refreshed?.error}, {refreshed?.error_name}). " +
                        "Nếu refresh token đã hết hạn (3 tháng) cần cấp lại qua POST /api/Zalo/get-access-token.");

                return refreshed.access_token;
            }
        }

        public async Task<ZaloTokenStatus?> GetTokenStatusAsync()
        {
            Zalo_Token stored;
            try { stored = await _repository.GetLastAsync(); }
            catch (InvalidOperationException) { return null; }
            if (stored == null) return null;

            var expiresAt = GetExpiresAt(stored);
            return new ZaloTokenStatus
            {
                Id = stored.Id,
                CreatedAt = DateTime.SpecifyKind(stored.CreatedAt, DateTimeKind.Utc),
                ExpiresAt = expiresAt,
                IsExpired = DateTime.UtcNow >= expiresAt,
                IsEncrypted = _protector.IsProtected(stored.AccessToken)
            };
        }

        /// <summary>Hết hạn = CreatedAt (UTC, xem SaveToken) + expires_in giây.</summary>
        public static DateTime GetExpiresAt(Zalo_Token token)
        {
            var seconds = int.TryParse(token.ExpiresIn, out var s) && s > 0 ? s : DefaultExpiresInSeconds;
            return DateTime.SpecifyKind(token.CreatedAt, DateTimeKind.Utc).AddSeconds(seconds);
        }

        private bool NeedsRefresh(Zalo_Token token)
        {
            var margin = TimeSpan.FromMinutes(Math.Max(0, _config.TokenRefreshMarginMinutes));
            return DateTime.UtcNow >= GetExpiresAt(token) - margin;
        }

        private async Task<Zalo_Token?> TryGetLastTokenAsync()
        {
            try
            {
                return await GetLastTokenAsync();
            }
            catch (InvalidOperationException)
            {
                return null; // ZaloTokenRepository ném khi bảng chưa có dòng nào
            }
        }

        /// <summary>Gọi Zalo refresh + lưu. Chỉ gọi khi đang giữ _refreshLock.</summary>
        private async Task<ZaloTokenResponse> RefreshCoreAsync(string refreshToken)
        {
            var token = await _api.PostFormAsync<ZaloTokenResponse>(
                _config.TokenEndpoint,
                new()
                {
                { "refresh_token", refreshToken },
                { "app_id", _config.AppId },
                { "grant_type", "refresh_token" }
                },
                new()
                {
                { "secret_key", _config.AppSecret }
                });

            LogTokenError("refresh_token", token);
            await SaveToken(token);

            return token;
        }

        private void LogTokenError(string grantType, ZaloTokenResponse? token)
        {
            if (token == null || string.IsNullOrWhiteSpace(token.access_token))
            {
                // Không log giá trị token - chỉ log mã lỗi Zalo trả về
                _logger.LogError("Zalo OAuth {GrantType} thất bại: error={Error}, name={ErrorName}, description={Description}",
                    grantType, token?.error, token?.error_name, token?.error_description);
            }
        }

        private async Task SaveToken(ZaloTokenResponse token)
        {
            if (token == null)
                return;

            if (string.IsNullOrWhiteSpace(token.access_token))
                return;

            var entity = new Zalo_Token
            {
                AccessToken = await ProtectForColumnAsync(token.access_token, "AccessToken"),
                RefreshToken = await ProtectForColumnAsync(token.refresh_token, "RefreshToken"),
                ExpiresIn = token.expires_in,
                CreatedAt = DateTime.UtcNow
            };

            // Zalo đã xoay vòng refresh token: nếu không lưu được thì mất chuỗi token → thử lại 1 lần.
            try
            {
                await _repository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lưu Zalo token thất bại, thử lại 1 lần.");
                await Task.Delay(500);
                // entity vẫn ở trạng thái Added trong DbContext → Add lại là no-op, SaveChanges ghi lại đúng 1 dòng
                await _repository.AddAsync(entity);
            }
        }

        private async Task<string> ProtectForColumnAsync(string value, string column)
        {
            if (!_config.EncryptTokens || string.IsNullOrEmpty(value))
                return value;

            string protectedValue;
            try
            {
                protectedValue = _protector.Protect(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mã hoá Zalo token thất bại - lưu plaintext để không mất token.");
                return value;
            }

            try
            {
                var maxChars = await _columnInspector.GetMaxCharsAsync(column);
                if (maxChars.HasValue && protectedValue.Length > maxChars.Value)
                {
                    _logger.LogWarning(
                        "Cột Zalo_Token.{Column} chỉ chứa {Max} ký tự, token mã hoá dài {Length} → lưu plaintext. " +
                        "Chạy Sql/Migrations/20260923_002_ZaloToken_WidenColumns.sql để bật mã hoá.",
                        column, maxChars.Value, protectedValue.Length);
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Không kiểm tra được độ dài cột Zalo_Token.{Column} - lưu plaintext.", column);
                return value;
            }

            return protectedValue;
        }
        /// <summary>
        /// Gửi tin nhắn Zalo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ZaloMessageResponse> SendTemplateAsync<T>(ZaloMessageRequest<T> request)
        {
            var token = await GetLastTokenAsync();
            if (token == null)
                throw new Exception("Không tìm thấy Access Token.");
            var body = new
            {
                phone = request.phone,
                template_id = request.template_id,
                template_data = request.template_data,
                tracking_id = request.tracking_id
            };

            var result = await _api.PostJsonAsync<object, ZaloMessageResponse>(
                _config.ApiSendMessage,
                body,
                new()
                {
                    { "access_token", token.AccessToken }
                });

            // Save legacy Zalo message log
            await SaveLogAsync(request, body, result);

            // Save unified ZNS send log
            await SaveZnsSendLogAsync(request, body, result);

            return result;
        }

        /// <summary>
        /// Lưu log sau khi gửi tin nhắn Zalo
        /// </summary>
        private async Task SaveLogAsync<T>(ZaloMessageRequest<T> request, object requestBody, ZaloMessageResponse response)
        {
            var fullName = "";
            var property = typeof(T).GetProperty("student_fullname");
            if (property != null)
            {
                fullName = property.GetValue(request.template_data)?.ToString();
            }
            await _logRepository.AddAsync(new Zalo_Message_Log
            {
                Phone = request.phone,
                FullName = fullName,
                TemplateId = request.template_id,
                TrackingId = request.tracking_id,
                Status = response.Error,
                Message = response.Message,
                RequestJson = JsonSerializer.Serialize(requestBody),
                ResponseJson = JsonSerializer.Serialize(response),
                CreatedTime = DateTime.Now
            });
        }

        private async Task SaveZnsSendLogAsync<T>(ZaloMessageRequest<T> request, object requestBody, ZaloMessageResponse response)
        {
            var isSuccess = response.Error == 0;
            var sentTime = ParseDateTimeFromMs(response.Data?.SentTime);
            var remainingQuota = ParseInt(response.Data?.Quota?.RemainingQuota);
            var dailyQuota = ParseInt(response.Data?.Quota?.DailyQuota);

            await _znsSendLogRepository.AddAsync(new ZnsSendLog
            {
                ZnsTemplateId = request.template_id,
                ZaloTemplateId = request.template_id,
                Phone = request.phone,
                ParamsJson = JsonSerializer.Serialize(request.template_data),
                RequestJson = JsonSerializer.Serialize(requestBody),
                ResponseJson = JsonSerializer.Serialize(response),
                Status = isSuccess ? ZnsSendStatus.Sent : ZnsSendStatus.Failed,
                ZaloMessageId = response.Data?.MsgId,
                SentTime = isSuccess ? (sentTime ?? DateTime.UtcNow) : null,
                SendingMode = response.Data?.SendingMode,
                RemainingQuota = remainingQuota,
                DailyQuota = dailyQuota,
                ErrorCode = isSuccess ? null : response.Error,
                ErrorMessage = isSuccess ? null : response.Message,
                Type = request.type,
                CampaignId = request.campaingId,
                EventCatId = request.eventCatId,
                EventId = request.eventId,
                ContextType = "ZaloService.SendTemplateAsync",
                CreatedBy = request.userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
        }

        private static DateTime? ParseDateTimeFromMs(string? ms)
        {
            if (!long.TryParse(ms, out var v)) return null;
            try { return DateTimeOffset.FromUnixTimeMilliseconds(v).UtcDateTime; }
            catch { return null; }
        }

        private static int? ParseInt(string? s) => int.TryParse(s, out var x) ? x : null;
    }
}