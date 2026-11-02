using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    /// <summary>
    /// BIRTHDAY-MAIL 02/11/2026: dựng thư chúc mừng sinh nhật từ mẫu HTML + mã BBCode.
    ///
    /// 2 bước để gửi nhanh và an toàn:
    ///   1. <see cref="PrepareBase"/> (1 lần / lượt): bỏ chú thích &lt;!--! --&gt; và &lt;script&gt;, thay mã chung ([News] [Events] [HeroImage]
    ///      [LogoUrl] [Hotline] ...), gắn tracking cho MỌI link http(s) (đăng ký link vào DB qua <c>registerLink</c>), chèn ảnh đếm lượt mở,
    ///      tự thêm [Unsubscribe] nếu mẫu quên.
    ///   2. <see cref="Personalize"/> (mỗi người nhận): thay [Fullname] [FirstName] ... (đã mã hóa HTML — tên khách không chèn được thẻ / link),
    ///      token tracking, link hủy nhận thư.
    /// Mã không phân biệt hoa thường; mã lạ giữ nguyên (Marketing có thể viết chữ trong ngoặc vuông bình thường).
    /// </summary>
    public class BirthdayMailRenderer
    {
        /// <summary>Chỗ giữ token tracking trong bản dựng chung — thay ở bước 2.</summary>
        public const string TokenMarker = "__BMTOKEN__";

        private static readonly Regex RxDocComment = new(@"<!--!.*?-->", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex RxScript = new(@"<script\b.*?</script\s*>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RxOnAttr = new(@"\son[a-z]+\s*=\s*(""[^""]*""|'[^']*')", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex RxToken = new(@"\[([A-Za-z][A-Za-z0-9_]{1,30})\]", RegexOptions.Compiled);
        private static readonly Regex RxHref = new(@"(<a\b[^>]*?\bhref\s*=\s*)([""'])(.*?)\2", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex RxTagText = new(@"<[^>]+>", RegexOptions.Compiled);
        private static readonly Regex RxBodyEnd = new(@"</body\s*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

        /// <summary>Thông tin công ty + cấu hình dùng cho mã chung.</summary>
        public sealed class SiteInfo
        {
            public string WebsiteUrl { get; init; } = "https://capstonevietnam.com";
            public string Website { get; init; } = "www.capstonevietnam.com";
            public string LogoUrl { get; init; } = string.Empty;
            public string Hotline { get; init; } = string.Empty;
            public string ContactEmail { get; init; } = string.Empty;
            public string Facebook { get; init; } = string.Empty;
            public string HaNoiAddress { get; init; } = string.Empty;
            public string HaNoiPhone { get; init; } = string.Empty;
            public string HCMAddress { get; init; } = string.Empty;
            public string HCMPhone { get; init; } = string.Empty;
            public string HeroImageUrl { get; init; } = string.Empty;
            /// <summary>Đoạn HTML hủy nhận thư, chứa {unsubmail} (Marketing:MailUnsubContent). Trống = đoạn mặc định.</summary>
            public string UnsubscribeHtml { get; init; } = string.Empty;
            /// <summary>Link hủy nhận thư, chứa {unsubmail}.</summary>
            public string UnsubscribeUrl { get; init; } = string.Empty;
        }

        /// <param name="registerLink">(url, tiêu đề, itemId, itemType) -> linkId; null = không gắn tracking (xem trước).</param>
        /// <param name="trackingBase">https://api.../api/BirthdayMail — null/trống = không gắn tracking.</param>
        public async Task<string> PrepareBase(string templateHtml, IReadOnlyList<BirthdayMailItemRow> items, SiteInfo site, DateTime today,
            string? trackingBase, Func<string, string, int, byte, Task<int>>? registerLink)
        {
            var track = registerLink != null && !string.IsNullOrWhiteSpace(trackingBase);
            var html = templateHtml ?? string.Empty;
            html = RxDocComment.Replace(html, string.Empty);
            html = RxScript.Replace(html, string.Empty);
            html = RxOnAttr.Replace(html, string.Empty);

            // khối tin / sự kiện: link của item gắn tracking riêng (biết item nào được click)
            var news = items.Where(i => i.ItemType == 1).ToList();
            var events = items.Where(i => i.ItemType == 2).ToList();
            var newsHtml = await BuildItemsBlock(news, false, track ? registerLink : null, trackingBase);
            var eventsHtml = await BuildItemsBlock(events, true, track ? registerLink : null, trackingBase);

            var hasNews = ContainsToken(html, "News");
            var hasEvents = ContainsToken(html, "Events");
            if (!hasNews && newsHtml.Length > 0 || !hasEvents && eventsHtml.Length > 0)
            {
                // mẫu Marketing không đặt [News]/[Events] nhưng có tin đang bật -> chèn trước chân thư
                var extra = (hasNews ? "" : "[News]") + (hasEvents ? "" : "[Events]");
                html = InsertBeforeBodyEnd(html, "<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td align=\"center\">" +
                    "<table role=\"presentation\" width=\"700\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"max-width:700px;width:100%;\"><tr><td style=\"padding:0 20px;\">" +
                    extra + "</td></tr></table></td></tr></table>");
            }
            if (!ContainsToken(html, "Unsubscribe") && !ContainsToken(html, "UnsubscribeUrl"))
                html = InsertBeforeBodyEnd(html, "<div style=\"text-align:center;padding:10px 16px 24px;font-family:Arial,Helvetica,sans-serif;font-size:11px;color:#8a8f99;\">[Unsubscribe]</div>");

            var global = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["News"] = newsHtml,
                ["Events"] = eventsHtml,
                ["HeroImage"] = HeroBlock(site.HeroImageUrl),
                ["HeroImageUrl"] = Enc(site.HeroImageUrl),
                ["LogoUrl"] = Enc(site.LogoUrl),
                ["WebsiteUrl"] = Enc(site.WebsiteUrl),
                ["Website"] = Enc(site.Website),
                ["Hotline"] = Enc(site.Hotline),
                ["ContactEmail"] = Enc(site.ContactEmail),
                ["Facebook"] = Enc(site.Facebook),
                ["HaNoiAddress"] = Enc(site.HaNoiAddress),
                ["HaNoiPhone"] = Enc(site.HaNoiPhone),
                ["HCMAddress"] = Enc(site.HCMAddress),
                ["HCMPhone"] = Enc(site.HCMPhone),
                ["Today"] = today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                ["Year"] = today.Year.ToString(CultureInfo.InvariantCulture),
            };
            html = ReplaceTokens(html, global);

            if (track)
            {
                // mọi link còn lại trong mẫu (http/https) -> link click qua API; bỏ qua mailto:, tel:, #, link đã gắn, [UnsubscribeUrl]
                var map = new Dictionary<string, int>(StringComparer.Ordinal);
                var matches = RxHref.Matches(html).Cast<Match>().ToList();
                var sb = new StringBuilder(html.Length + 2048);
                var last = 0;
                foreach (var m in matches)
                {
                    var raw = WebUtility.HtmlDecode(m.Groups[3].Value).Trim();
                    if (!IsTrackable(raw, trackingBase!)) continue;
                    if (!map.TryGetValue(raw, out var linkId))
                    {
                        linkId = await registerLink!(raw, LinkTitle(html, m), 0, 0);
                        map[raw] = linkId;
                    }
                    if (linkId <= 0) continue;
                    sb.Append(html, last, m.Index - last);
                    sb.Append(m.Groups[1].Value).Append(m.Groups[2].Value).Append(ClickUrl(trackingBase!, linkId)).Append(m.Groups[2].Value);
                    last = m.Index + m.Length;
                }
                sb.Append(html, last, html.Length - last);
                html = sb.ToString();

                html = InsertBeforeBodyEnd(html, "<img src=\"" + TrimBase(trackingBase!) + "/open?t=" + TokenMarker +
                    "\" width=\"1\" height=\"1\" alt=\"\" style=\"display:block;width:1px;height:1px;border:0;opacity:0;\">");
            }
            return html;
        }

        /// <summary>Bước 2: thay mã cá nhân. <paramref name="token"/> null = bản xem trước (không có tracking).</summary>
        public string Personalize(string baseHtml, BirthdayRecipientRow r, DateTime today, SiteInfo site, string? token, Func<string, string> encryptEmail)
        {
            var map = PersonalTokens(r, today, true);
            var unsubUrl = BuildUnsubUrl(site, r.Email, encryptEmail);
            map["UnsubscribeUrl"] = Enc(unsubUrl);
            map["Unsubscribe"] = BuildUnsubHtml(site, r.Email, unsubUrl, encryptEmail);
            var html = ReplaceTokens(baseHtml, map);
            return token == null ? html : html.Replace(TokenMarker, token, StringComparison.Ordinal);
        }

        public string PersonalizeSubject(string subject, BirthdayRecipientRow r, DateTime today)
        {
            var s = ReplaceTokens(subject ?? string.Empty, PersonalTokens(r, today, false));
            s = Regex.Replace(s, @"[\r\n\t]+", " ").Trim();
            return s.Length > 200 ? s[..200] : s;
        }

        /// <summary>Người nhận giả để xem trước / gửi thử.</summary>
        public static BirthdayRecipientRow SampleRecipient(string? fullName, string email, DateTime today)
        {
            fullName = string.IsNullOrWhiteSpace(fullName) ? "Nguyễn Minh Anh" : fullName.Trim();
            var idx = fullName.LastIndexOf(' ');
            return new BirthdayRecipientRow
            {
                StudentId = 0,
                Email = email,
                FullName = fullName,
                Hotendem = idx > 0 ? fullName[..idx] : string.Empty,
                Ten = idx > 0 ? fullName[(idx + 1)..] : fullName,
                BirthDate = new DateTime(today.Year - 18, today.Month, today.Day == 29 && today.Month == 2 ? 28 : today.Day),
                Age = 18
            };
        }

        public static string ClickUrl(string trackingBase, int linkId) =>
            TrimBase(trackingBase) + "/click?t=" + TokenMarker + "&amp;l=" + linkId.ToString(CultureInfo.InvariantCulture);

        // ── helpers ─────────────────────────────────────────────────────────

        private static Dictionary<string, string> PersonalTokens(BirthdayRecipientRow r, DateTime today, bool html)
        {
            string E(string v) => html ? Enc(v) : v;
            var full = string.IsNullOrWhiteSpace(r.FullName) ? (r.Hotendem + " " + r.Ten).Trim() : r.FullName.Trim();
            var first = string.IsNullOrWhiteSpace(r.Ten) ? LastWord(full) : r.Ten.Trim();
            if (full.Length == 0) full = "Quý khách";
            if (first.Length == 0) first = "bạn";
            var bd = r.BirthDate ?? today;
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Fullname"] = E(TitleCase(full)),
                ["FullName"] = E(TitleCase(full)),
                ["FirstName"] = E(TitleCase(first)),
                ["Ten"] = E(TitleCase(first)),
                ["LastName"] = E(TitleCase(r.Hotendem?.Trim() ?? string.Empty)),
                ["Email"] = E(r.Email ?? string.Empty),
                ["BirthDay"] = bd.ToString("dd.MM", CultureInfo.InvariantCulture),
                ["BirthDate"] = bd.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                ["Age"] = r.Age > 0 ? r.Age.ToString(CultureInfo.InvariantCulture) : string.Empty,
                ["Today"] = today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                ["Year"] = today.Year.ToString(CultureInfo.InvariantCulture),
            };
        }

        /// <summary>Thay [Mã] có trong <paramref name="map"/>; mã không có giữ nguyên.</summary>
        public static string ReplaceTokens(string text, IReadOnlyDictionary<string, string> map) =>
            RxToken.Replace(text, m => map.TryGetValue(m.Groups[1].Value, out var v) ? v : m.Value);

        private static bool ContainsToken(string html, string name) =>
            html.IndexOf("[" + name + "]", StringComparison.OrdinalIgnoreCase) >= 0;

        private static bool IsTrackable(string url, string trackingBase)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var u)) return false;
            if (u.Scheme != Uri.UriSchemeHttp && u.Scheme != Uri.UriSchemeHttps) return false;
            if (url.StartsWith(TrimBase(trackingBase), StringComparison.OrdinalIgnoreCase)) return false;
            if (url.Contains("unsubscribe", StringComparison.OrdinalIgnoreCase) || url.Contains("{unsubmail}", StringComparison.Ordinal)) return false;
            return url.Length <= 2000;
        }

        private static string LinkTitle(string html, Match m)
        {
            var close = html.IndexOf("</a", m.Index + m.Length, StringComparison.OrdinalIgnoreCase);
            if (close < 0) return string.Empty;
            var gt = html.IndexOf('>', m.Index + m.Length);
            if (gt < 0 || gt > close) return string.Empty;
            var inner = html.Substring(gt + 1, close - gt - 1);
            var text = WebUtility.HtmlDecode(RxTagText.Replace(inner, " "));
            text = Regex.Replace(text, @"\s+", " ").Trim();
            if (text.Length == 0)
            {
                var alt = Regex.Match(inner, @"\balt\s*=\s*[""']([^""']*)", RegexOptions.IgnoreCase);
                text = alt.Success ? "[ảnh] " + WebUtility.HtmlDecode(alt.Groups[1].Value) : "[ảnh]";
            }
            return text.Length > 300 ? text[..300] : text;
        }

        private static async Task<string> BuildItemsBlock(List<BirthdayMailItemRow> items, bool isEvent,
            Func<string, string, int, byte, Task<int>>? registerLink, string? trackingBase)
        {
            if (items.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%;\">");
            sb.Append("<tr><td style=\"padding:26px 0 4px 0;border-top:1px solid #d9c49a;font-family:'Nunito',Arial,Helvetica,sans-serif;\">");
            sb.Append("<div style=\"font-family:'Nunito',Arial,Helvetica,sans-serif;font-size:11px;line-height:16px;letter-spacing:3px;color:#b8893b;text-transform:uppercase;font-weight:700;\">")
              .Append(isEvent ? "Sự kiện sắp diễn ra" : "Tin tức dành cho bạn").Append("</div>");
            sb.Append("<div style=\"font-family:'Nunito',Arial,Helvetica,sans-serif;font-size:18px;line-height:26px;color:#0f2c59;font-weight:700;padding-top:4px;\">")
              .Append(isEvent ? "Hẹn gặp bạn tại Capstone" : "Góc thông tin từ Capstone").Append("</div>");
            sb.Append("</td></tr>");

            foreach (var it in items)
            {
                var href = Enc(it.Url);
                if (registerLink != null && Uri.TryCreate(it.Url, UriKind.Absolute, out _))
                {
                    var id = await registerLink(it.Url, it.Title, it.Id, it.ItemType);
                    if (id > 0) href = ClickUrl(trackingBase!, id);
                }
                var btn = string.IsNullOrWhiteSpace(it.ButtonText) ? (isEvent ? "Đăng ký tham dự" : "Đọc tiếp") : it.ButtonText.Trim();
                var hasImg = Uri.TryCreate(it.ImageUrl, UriKind.Absolute, out _);

                sb.Append("<tr><td style=\"padding:16px 0 4px 0;\">");
                sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr>");
                if (hasImg)
                {
                    sb.Append("<td class=\"stack stack-img\" width=\"180\" valign=\"top\" style=\"width:180px;\">")
                      .Append("<a href=\"").Append(href).Append("\" target=\"_blank\" style=\"text-decoration:none;\">")
                      .Append("<img src=\"").Append(Enc(it.ImageUrl)).Append("\" width=\"180\" alt=\"").Append(Enc(it.Title))
                      .Append("\" style=\"display:block;width:180px;height:auto;border:0;border-radius:4px;\"></a></td>");
                }
                sb.Append("<td class=\"stack stack-txt\" valign=\"top\" style=\"").Append(hasImg ? "padding:0 0 0 18px;" : "padding:0;")
                  .Append("font-family:'Roboto',Arial,Helvetica,sans-serif;\">");
                if (!string.IsNullOrWhiteSpace(it.MetaText))
                    sb.Append("<div style=\"font-family:'Roboto',Arial,Helvetica,sans-serif;font-size:12px;line-height:18px;color:#a41034;font-weight:500;padding-bottom:4px;\">")
                      .Append(Enc(it.MetaText)).Append("</div>");
                sb.Append("<a href=\"").Append(href).Append("\" target=\"_blank\" style=\"font-family:'Nunito',Arial,Helvetica,sans-serif;font-size:16px;line-height:22px;color:#0f2c59;text-decoration:none;font-weight:700;\">")
                  .Append(Enc(it.Title)).Append("</a>");
                if (!string.IsNullOrWhiteSpace(it.Summary))
                    sb.Append("<div style=\"font-family:'Roboto',Arial,Helvetica,sans-serif;font-size:14px;line-height:21px;color:#475569;padding-top:6px;\">").Append(Enc(Cut(it.Summary, 260))).Append("</div>");
                sb.Append("<div style=\"padding-top:12px;\">");
                if (isEvent)
                    sb.Append("<a href=\"").Append(href).Append("\" target=\"_blank\" style=\"display:inline-block;background:#a41034;color:#ffffff;font-family:'Roboto',Arial,Helvetica,sans-serif;font-size:13px;line-height:16px;font-weight:500;text-decoration:none;padding:10px 18px;border-radius:3px;\">")
                      .Append(Enc(btn)).Append(" &rarr;</a>");
                else
                    sb.Append("<a href=\"").Append(href).Append("\" target=\"_blank\" style=\"font-family:'Roboto',Arial,Helvetica,sans-serif;font-size:13px;line-height:16px;color:#a41034;font-weight:500;text-decoration:none;\">")
                      .Append(Enc(btn)).Append(" &rarr;</a>");
                sb.Append("</div></td></tr></table></td></tr>");
            }
            sb.Append("<tr><td style=\"height:18px;line-height:18px;font-size:0;\">&nbsp;</td></tr></table>");
            return sb.ToString();
        }

        private static string HeroBlock(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var u) || (u.Scheme != Uri.UriSchemeHttps && u.Scheme != Uri.UriSchemeHttp)) return string.Empty;
            return "<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding:0;\">" +
                   "<img src=\"" + Enc(url) + "\" width=\"668\" alt=\"Chúc mừng sinh nhật\" style=\"display:block;width:100%;max-width:668px;height:auto;border:0;\">" +
                   "</td></tr></table>";
        }

        private static string BuildUnsubUrl(SiteInfo site, string email, Func<string, string> encrypt)
        {
            if (string.IsNullOrWhiteSpace(site.UnsubscribeUrl)) return string.Empty;
            return site.UnsubscribeUrl.Replace("{unsubmail}", Uri.EscapeDataString(encrypt(email ?? string.Empty)), StringComparison.Ordinal);
        }

        private static string BuildUnsubHtml(SiteInfo site, string email, string unsubUrl, Func<string, string> encrypt)
        {
            if (!string.IsNullOrWhiteSpace(site.UnsubscribeHtml))
                return site.UnsubscribeHtml.Replace("{unsubmail}", Uri.EscapeDataString(encrypt(email ?? string.Empty)), StringComparison.Ordinal);
            if (unsubUrl.Length == 0) return string.Empty;
            return "<br>Không muốn nhận thư từ Capstone? <a href=\"" + Enc(unsubUrl) + "\" target=\"_blank\" style=\"color:#8a8f99;text-decoration:underline;\">Hủy nhận thư</a>.";
        }

        private static string InsertBeforeBodyEnd(string html, string fragment)
        {
            var m = RxBodyEnd.Matches(html);
            if (m.Count == 0) return html + fragment;
            var last = m[^1];
            return html.Insert(last.Index, fragment);
        }

        private static string TrimBase(string b) => (b ?? string.Empty).TrimEnd('/');

        private static string Enc(string? s) => WebUtility.HtmlEncode(s ?? string.Empty);

        private static string Cut(string s, int max)
        {
            s = s.Trim();
            if (s.Length <= max) return s;
            var cut = s.LastIndexOf(' ', max);
            return (cut > max / 2 ? s[..cut] : s[..max]) + "…";
        }

        private static string LastWord(string s)
        {
            s = (s ?? string.Empty).Trim();
            var i = s.LastIndexOf(' ');
            return i >= 0 ? s[(i + 1)..] : s;
        }

        /// <summary>"nguyễn văn AN" -> "Nguyễn Văn An" (dữ liệu CRM hay viết thường / viết hoa hết).</summary>
        private static string TitleCase(string s)
        {
            s = Regex.Replace(s ?? string.Empty, @"\s+", " ").Trim();
            if (s.Length == 0) return s;
            var parts = s.Split(' ');
            for (var i = 0; i < parts.Length; i++)
            {
                var p = parts[i].ToLower(Vi);
                parts[i] = p.Length == 0 ? p : char.ToUpper(p[0], Vi) + p[1..];
            }
            return string.Join(' ', parts);
        }
    }
}
