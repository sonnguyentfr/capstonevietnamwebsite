using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ZaloTokenService : IZaloTokenService
{
    private readonly IZaloTokenRepository _repository;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public ZaloTokenService(IZaloTokenRepository repository, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ZaloTokenResponse> GetAndSaveTokenAsync(string code)
    {
        var zaloConfig = _config.GetSection("Zalo");
        var endpoint = zaloConfig["TokenEndpoint"];
        var appId = zaloConfig["AppId"];
        var appSecret = zaloConfig["AppSecret"];

        var client = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Add("secret_key", appSecret);

        var formData = new List<KeyValuePair<string, string>>
        {
            new("code", code),
            new("app_id", appId),
            new("grant_type", "authorization_code"),
            new("app_secret", appSecret)
        };
        request.Content = new FormUrlEncodedContent(formData);

        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        var tokenResponse = JsonSerializer.Deserialize<ZaloTokenResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if ( tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
        {
            var token = new Zalo_Token
            {
                AccessToken = tokenResponse.access_token,
                RefreshToken = tokenResponse.refresh_token,
                ExpiresIn = tokenResponse.expires_in,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(token);
        }

        return tokenResponse;
    }
    public async Task<ZaloTokenResponse> RefreshAndSaveTokenAsync(string refresh_token)
    {
        var zaloConfig = _config.GetSection("Zalo");
        var endpoint = zaloConfig["TokenEndpoint"];
        var appId = zaloConfig["AppId"];
        var appSecret = zaloConfig["AppSecret"];
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Add("secret_key", appSecret);

        var formData = new List<KeyValuePair<string, string>>
        {
            new("refresh_token", refresh_token),
            new("app_id", appId),
            new("grant_type", "refresh_token"),
        };
        request.Content = new FormUrlEncodedContent(formData);

        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        var tokenResponse = JsonSerializer.Deserialize<ZaloTokenResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (response.IsSuccessStatusCode && tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
        {
            var token = new Zalo_Token
            {
                AccessToken = tokenResponse.access_token,
                RefreshToken = tokenResponse.refresh_token,
                ExpiresIn = tokenResponse.expires_in,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(token);
        }

        return tokenResponse;
    }
    

    public async Task<Zalo_Token> GetLastTokenAsync()
    {
        return await _repository.GetLastAsync();
    }
}