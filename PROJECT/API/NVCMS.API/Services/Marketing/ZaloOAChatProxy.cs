using DotNetNuke.Entities.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using DotNetNuke.Common.Utilities;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace NVCMS.API.Marketing.Services.Marketing
{
    /// <summary>
    /// Quyền dùng Zalo OA Chat.
    /// Role cấu hình ở web.config appSettings "zalooa_chat_roles" (phân cách ; hoặc ,), mặc định "Bien Tap".
    /// SuperUser và Administrators luôn được phép. Dùng chung cho WebAPI và module (Viewer.ascx.vb).
    /// </summary>
    public static class ZaloOAChatPermission
    {
        public const string RolesSettingKey = "zalooa_chat_roles";
        public const string DefaultRoles = "Bien Tap";

        public static bool CanUseChat(UserInfo user)
        {
            if (user == null || user.UserID <= 0)
                return false;
            if (user.IsSuperUser || user.IsInRole("Administrators"))
                return true;

            var setting = Config.GetSetting(RolesSettingKey);
            var roles = (string.IsNullOrWhiteSpace(setting) ? DefaultRoles : setting)
                .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim())
                .Where(r => r.Length > 0);
            return roles.Any(user.IsInRole);
        }

        /// <summary>Thao tác quản trị (nhập lịch sử): chỉ SuperUser / Administrators.</summary>
        public static bool IsChatAdmin(UserInfo user)
            => user != null && user.UserID > 0 && (user.IsSuperUser || user.IsInRole("Administrators"));
    }

    /// <summary>Phản hồi thô từ NVCMS.API.ReadGoogleSheet - proxy trả nguyên status + JSON cho trình duyệt.</summary>
    public class ZaloOAProxyResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Body { get; set; }
    }

    /// <summary>
    /// Gọi API Zalo OA Chat của NVCMS.API.ReadGoogleSheet bằng JWT dịch vụ.
    /// Tương đương WWW/App_Code/CapApiClient.vb (UltiCapApiClient) và dùng CHUNG:
    ///   - appSettings cap_api_url / cap_api_url_login / cap_api_user / cap_api_password
    ///   - cache JWT HttpRuntime.Cache["CAP_API_TOKEN"]
    /// Không dùng trực tiếp được class VB vì nó nằm trong App_Code (biên dịch lúc chạy).
    /// JWT và mọi secret chỉ nằm phía server, không trả xuống trình duyệt.
    /// </summary>
    public static class ZaloOAChatProxy
    {
        private const string TokenCacheKey = "CAP_API_TOKEN";
        private static readonly object TokenLock = new object();
        private static readonly HttpClient Http = CreateHttpClient();

        private static string BaseUrl => (Config.GetSetting("cap_api_url") ?? "").TrimEnd('/');

        private static HttpClient CreateHttpClient()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var handler = new HttpClientHandler
            {
                // Chỉ chấp nhận chứng chỉ không hợp lệ khi gọi API trên chính máy này (https://localhost - dev cert
                // của ASP.NET Core không được app pool IIS tin cậy). Domain thật luôn kiểm tra SSL đầy đủ.
                ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
                    errors == System.Net.Security.SslPolicyErrors.None
                    || (request?.RequestUri != null && request.RequestUri.IsLoopback)
            };
            var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(60) };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static Task<ZaloOAProxyResponse> GetAsync(string pathAndQuery)
            => SendAsync(HttpMethod.Get, pathAndQuery, () => null);

        public static Task<ZaloOAProxyResponse> PostJsonAsync(string path, object body)
        {
            var json = JsonConvert.SerializeObject(body);
            return SendAsync(HttpMethod.Post, path, () => new StringContent(json, Encoding.UTF8, "application/json"));
        }

        /// <summary>contentFactory được gọi lại khi retry sau 401 (HttpContent không dùng lại được).</summary>
        public static async Task<ZaloOAProxyResponse> SendAsync(HttpMethod method, string pathAndQuery, Func<HttpContent> contentFactory)
        {
            if (string.IsNullOrWhiteSpace(BaseUrl))
                return Error(HttpStatusCode.ServiceUnavailable, "NOT_CONFIGURED", "Chưa cấu hình cap_api_url.");

            for (var attempt = 0; attempt < 2; attempt++)
            {
                string token;
                try
                {
                    token = GetToken(forceRefresh: attempt > 0);
                }
                catch (Exception ex)
                {
                    DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                    return Error(HttpStatusCode.BadGateway, "API_AUTH_ERROR", "Không đăng nhập được API Zalo.");
                }

                using (var request = new HttpRequestMessage(method, BaseUrl + pathAndQuery))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    request.Content = contentFactory();

                    HttpResponseMessage response;
                    try
                    {
                        response = await Http.SendAsync(request).ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        return Error(HttpStatusCode.GatewayTimeout, "API_TIMEOUT", "API Zalo không phản hồi.");
                    }
                    catch (HttpRequestException ex)
                    {
                        DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                        return Error(HttpStatusCode.BadGateway, "API_UNREACHABLE", "Không kết nối được API Zalo.");
                    }

                    using (response)
                    {
                        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (response.StatusCode == HttpStatusCode.Unauthorized && attempt == 0)
                        {
                            HttpRuntime.Cache.Remove(TokenCacheKey);
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(body) || !LooksLikeJson(body))
                            return Error(response.IsSuccessStatusCode ? HttpStatusCode.BadGateway : response.StatusCode,
                                "API_ERROR", "API Zalo trả phản hồi không hợp lệ (HTTP " + (int)response.StatusCode + ").");

                        return new ZaloOAProxyResponse { StatusCode = response.StatusCode, Body = body };
                    }
                }
            }

            return Error(HttpStatusCode.BadGateway, "API_AUTH_ERROR", "Không xác thực được API Zalo.");
        }

        private static string GetToken(bool forceRefresh)
        {
            var cached = HttpRuntime.Cache[TokenCacheKey] as string;
            if (!forceRefresh && !string.IsNullOrWhiteSpace(cached))
                return cached;

            lock (TokenLock)
            {
                cached = HttpRuntime.Cache[TokenCacheKey] as string;
                if (!forceRefresh && !string.IsNullOrWhiteSpace(cached))
                    return cached;

                var loginUrl = BaseUrl + Config.GetSetting("cap_api_url_login");
                var payload = JsonConvert.SerializeObject(new
                {
                    username = Config.GetSetting("cap_api_user"),
                    password = Config.GetSetting("cap_api_password")
                });

                using (var content = new StringContent(payload, Encoding.UTF8, "application/json"))
                using (var response = Http.PostAsync(loginUrl, content).GetAwaiter().GetResult())
                {
                    var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                        throw new InvalidOperationException("CAP API login thất bại. Status=" + (int)response.StatusCode);

                    var json = JObject.Parse(body);
                    var token = (string)json.SelectToken("Data.Token") ?? (string)json.SelectToken("data.token");
                    if (string.IsNullOrWhiteSpace(token))
                        throw new InvalidOperationException("CAP API login không trả token.");

                    DateTime expiration;
                    var exp = (string)json.SelectToken("Data.Expiration") ?? (string)json.SelectToken("data.expiration");
                    var absolute = DateTime.TryParse(exp, out expiration) ? expiration.AddMinutes(-5) : DateTime.Now.AddHours(23);
                    HttpRuntime.Cache.Insert(TokenCacheKey, token, null, absolute, Cache.NoSlidingExpiration);
                    return token;
                }
            }
        }

        private static bool LooksLikeJson(string s)
        {
            var t = s.TrimStart();
            return t.StartsWith("{") || t.StartsWith("[");
        }

        /// <summary>
        /// Lỗi phía proxy - cùng format ApiResponse của ReadGoogleSheet (ASP.NET Core trả camelCase):
        /// { success, message, errorCode, data }.
        /// </summary>
        public static ZaloOAProxyResponse Error(HttpStatusCode status, string errorCode, string message)
            => new ZaloOAProxyResponse
            {
                StatusCode = status,
                Body = JsonConvert.SerializeObject(new { success = false, message = message, errorCode = errorCode, data = (object)null })
            };
    }
}
