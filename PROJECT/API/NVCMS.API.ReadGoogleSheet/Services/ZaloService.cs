using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Http;
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
        private readonly IRepository<Zalo_Message_Log> _logRepository;
        private readonly ZaloSettings _config;

        public ZaloService(
            BaseApi api,
            IZaloTokenRepository repository,
            IRepository<Zalo_Message_Log> logRepository,
            IOptions<ZaloSettings> config)
        {
            _api = api;
            _repository = repository;
            _logRepository = logRepository;
            _config = config.Value;
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

            await SaveToken(token);

            return token;
        }

        public async Task<ZaloTokenResponse> RefreshAndSaveTokenAsync(string refreshToken)
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

            await SaveToken(token);

            return token;
        }

        public Task<Zalo_Token> GetLastTokenAsync()
        {
            return _repository.GetLastAsync();
        }

        private async Task SaveToken(ZaloTokenResponse token)
        {
            if (token == null)
                return;

            if (string.IsNullOrWhiteSpace(token.access_token))
                return;

            await _repository.AddAsync(new Zalo_Token
            {
                AccessToken = token.access_token,
                RefreshToken = token.refresh_token,
                ExpiresIn = token.expires_in,
                CreatedAt = DateTime.UtcNow
            });
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
            // Save log
            await SaveLogAsync(request, body, result);
            return result;
        }
        /// <summary>
        /// Lưu log sau khi gửi tin nhắn Zalo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="requestBody"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task SaveLogAsync<T>(ZaloMessageRequest<T> request, object requestBody, ZaloMessageResponse response)
        {
            var fullName = "";
            // Lấy student_fullname nếu có
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
    }
}