using System.Text;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Infrastructure.Http
{
    public class BaseApi
    {
        private readonly HttpClient _client;
        private readonly ILogger<BaseApi> _logger;

        public BaseApi(HttpClient client,
            ILogger<BaseApi> logger)
        {
            _client = client;
            _logger = logger;
            _client.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<TResponse> GetJsonAsync<TResponse>(
            string url,
            Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(json);

            if (!response.IsSuccessStatusCode)
                throw new Exception(json);

            return JsonSerializer.Deserialize<TResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }

        public async Task<TResponse> PostJsonAsync<TRequest, TResponse>(
            string url,
            TRequest body,
            Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            var response = await _client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(json);

            if (!response.IsSuccessStatusCode)
                throw new Exception(json);

            return JsonSerializer.Deserialize<TResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }

        public async Task<TResponse> PostFormAsync<TResponse>(
            string url,
            Dictionary<string, string> formData,
            Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.Headers.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

            request.Content = new FormUrlEncodedContent(formData);

            var response = await _client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(json);

            if (!response.IsSuccessStatusCode)
                throw new Exception(json);

            return JsonSerializer.Deserialize<TResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}