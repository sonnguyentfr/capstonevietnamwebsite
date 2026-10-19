using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using DotNetNuke.Common.Utilities;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NVCMS.API.Analytics
{
    /// <summary>Lỗi nghiệp vụ trả cho giao diện (HTTP 400) - thông báo ngắn, chi tiết ghi log DNN.</summary>
    public class AnalyticsException : Exception
    {
        public AnalyticsException(string message) : base(message) { }
    }

    /// <summary>
    /// Cấu hình Google Analytics theo portal (khóa / token KHÔNG bao giờ gửi xuống trình duyệt).
    /// Nhập ở Cấu hình website (NVCMS.HeThong / Trang / PageSettings.ascx, mục GOOGLE ANALYTICS) -> PortalSettings cùng tên khóa;
    /// khóa nào portal chưa có thì lấy &lt;appSettings&gt; của web.config:
    /// <code>
    /// NVCMS.Analytics.GA4.PropertyId      123456789 (GA4 › Admin › Property details)
    /// NVCMS.Analytics.GA4.CredentialFile  ~/App_Data/NVCMS/ga4-0.json (khóa service account, quyền Viewer trên property)
    /// NVCMS.Analytics.ArticleIdPattern    lấy NewId từ đường dẫn bài (mặc định /tin-tuc/…-{id}, /tu-van-dinh-cu/…-{id}, /tu-van-dau-tu/…-{id}, …-{id}.html)
    /// NVCMS.Analytics.CacheMinutes        10
    /// NVCMS.Analytics.KeyEvents           generate_lead,sign_up   tối đa 3 key event tính là chuyển đổi (trống = tổng keyEvents của GA4)
    /// NVCMS.Analytics.MonthlyTarget       50000                   mục tiêu phiên / tháng (0 = không đặt)
    /// </code>
    /// Lưu ở PageSettings xong thì xóa cache "NVCMS.Analytics." (cấu hình + báo cáo) để áp dụng ngay.
    /// </summary>
    public sealed class AnalyticsSettings
    {
        public const string Prefix = "NVCMS.Analytics.";
        public const string DefaultArticlePattern = @"^/(?:tin-tuc|tu-van-dinh-cu|tu-van-dau-tu)/[^/]*-(\d+)/?$|-(\d+)\.html?$";

        public string Ga4PropertyId { get; private set; }
        public string Ga4CredentialFile { get; private set; }
        public Regex ArticleId { get; private set; }
        public int CacheMinutes { get; private set; }
        /// <summary>Tên key event GA4 tính là chuyển đổi; rỗng = dùng metric keyEvents (mọi sự kiện đã đánh dấu Key event).</summary>
        public IList<string> KeyEvents { get; private set; }
        /// <summary>Mục tiêu số phiên mỗi tháng (0 = không đặt).</summary>
        public long MonthlyTarget { get; private set; }

        public bool Configured => Ga4PropertyId.Length > 0 && Ga4CredentialFile.Length > 0;

        /// <summary>Cấu hình của portal (cache 10 phút, xóa khi lưu Cấu hình website).</summary>
        public static AnalyticsSettings For(int portalId)
        {
            string key = Prefix + "Settings." + portalId;
            if (DataCache.GetCache(key) is AnalyticsSettings hit) return hit;
            AnalyticsSettings s = Load(portalId);
            DataCache.SetCache(key, s, DateTime.Now.AddMinutes(10));
            return s;
        }

        /// <summary>Đọc một khóa: PortalSettings của portal, chưa có thì web.config.</summary>
        public static string Read(int portalId, string name)
        {
            string v;
            try { v = DotNetNuke.Entities.Portals.PortalController.GetPortalSetting(Prefix + name, portalId, null); }
            catch (Exception) { v = null; }   // ngoài request DNN -> chỉ web.config
            if (string.IsNullOrWhiteSpace(v)) v = System.Web.Configuration.WebConfigurationManager.AppSettings[Prefix + name];
            return (v ?? "").Trim();
        }

        private static AnalyticsSettings Load(int portalId)
        {
            Func<string, string> app = k => Read(portalId, k);
            string pattern = app("ArticleIdPattern");
            Regex rx;
            try { rx = new Regex(string.IsNullOrEmpty(pattern) ? DefaultArticlePattern : pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(200)); }
            catch (ArgumentException) { rx = new Regex(DefaultArticlePattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(200)); }
            int cache = int.TryParse(app("CacheMinutes"), out int c) && c >= 1 && c <= 1440 ? c : 10;
            string property = app("GA4.PropertyId");
            if (property.StartsWith("properties/", StringComparison.OrdinalIgnoreCase)) property = property.Substring(11);
            return new AnalyticsSettings
            {
                Ga4PropertyId = Regex.IsMatch(property, @"^\d{5,15}$") ? property : "",
                Ga4CredentialFile = app("GA4.CredentialFile"),
                ArticleId = rx,
                CacheMinutes = cache,
                KeyEvents = app("KeyEvents").Split(new[] { ',', ';', '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).Where(x => Regex.IsMatch(x, @"^[A-Za-z][A-Za-z0-9_]{0,39}$")).Distinct().Take(3).ToList(),
                MonthlyTarget = long.TryParse(app("MonthlyTarget"), out long t) && t > 0 ? t : 0
            };
        }

        /// <summary>Xóa cache cấu hình + mọi báo cáo analytics (gọi sau khi đổi cấu hình).</summary>
        public static void ClearCache() => DataCache.ClearCache(Prefix);

        /// <summary>NewId từ đường dẫn: nhóm bắt được đầu tiên của mẫu; 0 nếu không phải trang bài.</summary>
        internal int ArticleIdOf(string url)
        {
            if (string.IsNullOrEmpty(url)) return 0;
            string path = url;
            int q = path.IndexOfAny(new[] { '?', '#' });
            if (q >= 0) path = path.Substring(0, q);
            try
            {
                Match m = ArticleId.Match(path);
                if (!m.Success) return 0;
                for (int i = 1; i < m.Groups.Count; i++)
                    if (m.Groups[i].Success && int.TryParse(m.Groups[i].Value, out int id)) return id;
                return 0;
            }
            catch (RegexMatchTimeoutException) { return 0; }
        }
    }

    /// <summary>Gọi Google Analytics Data API v1beta bằng service account (quyền analytics.readonly).</summary>
    internal sealed class Ga4Client
    {
        private const string Scope = "https://www.googleapis.com/auth/analytics.readonly";
        private static readonly HttpClient Http = new HttpClient { Timeout = TimeSpan.FromSeconds(25) };
        // khóa theo đường dẫn + thời điểm sửa tệp: thay tệp khóa (Cấu hình website) là nạp lại
        private static readonly ConcurrentDictionary<string, GoogleCredential> Creds = new ConcurrentDictionary<string, GoogleCredential>(StringComparer.OrdinalIgnoreCase);
        private readonly AnalyticsSettings _s;

        public Ga4Client(AnalyticsSettings s) { _s = s; }

        internal static string CredentialPath(string file)
        {
            if (string.IsNullOrEmpty(file)) return "";
            try { return file.StartsWith("~/", StringComparison.Ordinal) || file.StartsWith("/", StringComparison.Ordinal) ? HostingEnvironment.MapPath(file) ?? "" : file; }
            catch (Exception) { return ""; }
        }

        internal string Token()
        {
            string path = CredentialPath(_s.Ga4CredentialFile);
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) throw new AnalyticsException("Không thấy tệp khóa service account GA4 (Cấu hình website › Google Analytics).");
            string key = path + "|" + File.GetLastWriteTimeUtc(path).Ticks;
#pragma warning disable CS0618 // khóa do quản trị tự tải lên máy chủ, đã kiểm tra type = service_account khi lưu
            GoogleCredential cred = Creds.GetOrAdd(key, _ => GoogleCredential.FromFile(path).CreateScoped(Scope));
#pragma warning restore CS0618
            return ((ITokenAccess)cred).GetAccessTokenForRequestAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        internal JObject Post(string token, string method, object body)
        {
            string url = "https://analyticsdata.googleapis.com/v1beta/properties/" + Uri.EscapeDataString(_s.Ga4PropertyId) + ":" + method;
            using (var req = new HttpRequestMessage(HttpMethod.Post, url))
            {
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                req.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                using (HttpResponseMessage resp = Http.SendAsync(req).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    string text = resp.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!resp.IsSuccessStatusCode)
                    {
                        string msg = "GA4 HTTP " + (int)resp.StatusCode;
                        try { msg += ": " + (string)JObject.Parse(text)["error"]?["message"]; } catch (JsonException) { }
                        throw new InvalidOperationException(msg);
                    }
                    return JObject.Parse(text);
                }
            }
        }
    }

    /// <summary>Làm sạch chữ do trình duyệt bất kỳ gửi lên GA (tiêu đề trang, URL, nguồn giới thiệu - giả mạo được).</summary>
    internal static class Clean
    {
        private static readonly Regex RxTags = new Regex("<[^>]*>", RegexOptions.Compiled);
        private static readonly Regex RxControl = new Regex(@"[\u0000-\u0008\u000B\u000C\u000E-\u001F\u007F​-‏‪-‮⁦-⁩]", RegexOptions.Compiled);
        private static readonly Regex RxSpaces = new Regex(@"\s+", RegexOptions.Compiled);

        /// <summary>Chuỗi thuần: giải mã entity, bỏ thẻ, ký tự điều khiển / đảo chiều, gộp khoảng trắng, cắt tối đa maxLength.</summary>
        public static string Text(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            string s = WebUtility.HtmlDecode(value);
            s = RxTags.Replace(s, " ").Replace("<", "").Replace(">", "");
            s = RxControl.Replace(s, "");
            s = RxSpaces.Replace(s, " ").Trim();
            if (maxLength > 0 && s.Length > maxLength) s = s.Substring(0, maxLength).TrimEnd();
            return s;
        }

        /// <summary>URL an toàn: http(s) tuyệt đối hoặc đường dẫn gốc "/…" (không phải "//"). Khác -> "".</summary>
        public static string SafeUrl(string value)
        {
            string u = Text(value, 1000);
            if (u.Length == 0) return string.Empty;
            if (u.StartsWith("/", StringComparison.Ordinal))
                return u.StartsWith("//", StringComparison.Ordinal) || u.IndexOf('\\') >= 0 ? string.Empty : u;
            return Uri.TryCreate(u, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) ? u : string.Empty;
        }

        public static string Short(Exception ex)
        {
            Exception e = ex is AggregateException ag && ag.InnerException != null ? ag.InnerException : ex;
            string m = e.Message ?? "";
            return m.Length > 200 ? m.Substring(0, 200) : m;
        }
    }
}
