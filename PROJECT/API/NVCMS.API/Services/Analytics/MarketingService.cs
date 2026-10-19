using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;

namespace NVCMS.API.Analytics
{
    /// <summary>
    /// Phân tích Marketing từ Google Analytics 4 (trang chủ quản trị - skin _admin/controls/index/U_NhuanBut.ascx):
    ///  - Tổng quan: KPI so kỳ trước, xu hướng theo ngày, kênh, thiết bị, khách mới / quay lại, giờ vàng, trang đích, quốc gia, chiến dịch, realtime;
    ///  - Nội dung: bài / trang theo lượt xem - thời gian đọc - chuyển đổi, lọc theo tiêu đề, phân loại (ngôi sao / tiềm năng / cần tối ưu),
    ///    xu hướng tăng / giảm, chuyên mục cung - cầu (lượt xem so với số bài xuất bản);
    ///  - Nguồn &amp; UTM: kênh / source / medium / campaign / utm_content / utm_term, kiểm tra quy ước đặt UTM;
    ///  - Chiến dịch: hiệu quả từng campaign, chi tiết theo nguồn, nội dung quảng cáo, từ khóa, trang đích;
    ///  - Khán giả: địa lý, thiết bị, hệ điều hành, trình duyệt, ngôn ngữ, độ tuổi, giới tính;
    ///  - Gợi ý hành động: luật phân tích trên toàn bộ số liệu -> việc Marketing cần làm, xếp theo mức độ.
    /// Cấu hình ở Cấu hình website (PortalSettings NVCMS.Analytics.*), khóa service account chỉ ở máy chủ.
    /// Quyền xem do MarketingAnalyticsController kiểm tra (role).
    /// Kết quả cache theo khoảng ngày (NVCMS.Analytics.CacheMinutes, kỳ đã qua tối thiểu 60 phút).
    /// </summary>
    public class MarketingService
    {
        private readonly AnalyticsSettings _s;
        private int Portal { get; }

        /// <summary>Giá trị hệ thống của sessionCampaignName (không phải chiến dịch thật).</summary>
        internal static readonly HashSet<string> SystemCampaigns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "", "(not set)", "(direct)", "(organic)", "(referral)", "(cross-network)", "(data not available)", "(none)", "(not provided)"
        };

        /// <summary>utm_medium theo quy ước của GA4 Default Channel Group.</summary>
        internal static readonly HashSet<string> StandardMediums = new HashSet<string>(StringComparer.Ordinal)
        {
            "(none)", "(not set)", "organic", "referral", "cpc", "ppc", "paid", "paidsearch", "cpm", "cpv", "cpa", "cpp", "display", "banner", "interstitial",
            "email", "e-mail", "e_mail", "e mail", "social", "social-network", "social-media", "sm", "social network", "social media", "paid-social", "paid_social",
            "affiliate", "affiliates", "video", "audio", "sms", "push", "mobile", "notification", "qr", "qrcode", "referral_program", "influencer", "partner", "app"
        };

        public MarketingService(int portalId)
        {
            Portal = portalId;
            _s = AnalyticsSettings.For(portalId);
        }

        private bool Ready => _s.Configured;

        private void DemandGa()
        {
            if (!Ready) throw new AnalyticsException("Chưa kết nối Google Analytics 4. Khai báo Property ID và khóa service account ở Cấu hình website › Google Analytics.");
        }

        // =================================================================================================
        // Khởi tạo
        // =================================================================================================

        public object Init()
        {
            return new
            {
                configured = Ready,
                propertyId = _s.Ga4PropertyId,
                keyEvents = _s.KeyEvents,
                monthlyTarget = _s.MonthlyTarget,
                cacheMinutes = _s.CacheMinutes,
                ranges = new[]
                {
                    new { key = "today", name = "Hôm nay" }, new { key = "yesterday", name = "Hôm qua" }, new { key = "7d", name = "7 ngày" },
                    new { key = "28d", name = "28 ngày" }, new { key = "90d", name = "90 ngày" }, new { key = "month", name = "Tháng này" },
                    new { key = "lastmonth", name = "Tháng trước" }, new { key = "quarter", name = "Quý này" }, new { key = "custom", name = "Tùy chọn" }
                },
                dims = AcqDims.Select(kv => new { key = kv.Key, name = kv.Value.Item2 }),
                now = DateTime.Now
            };
        }

        // =================================================================================================
        // Khoảng thời gian + cache
        // =================================================================================================

        public static MkRange MakeRange(string key, DateTime? from, DateTime? to)
        {
            DateTime today = DateTime.Today, f, t;
            key = (key ?? "").ToLowerInvariant();
            switch (key)
            {
                case "today": f = today; t = today; break;
                case "yesterday": f = today.AddDays(-1); t = f; break;
                case "7d": f = today.AddDays(-6); t = today; break;
                case "28d": f = today.AddDays(-27); t = today; break;
                case "90d": f = today.AddDays(-89); t = today; break;
                case "month": f = new DateTime(today.Year, today.Month, 1); t = today; break;
                case "lastmonth": t = new DateTime(today.Year, today.Month, 1).AddDays(-1); f = new DateTime(t.Year, t.Month, 1); break;
                case "quarter": f = new DateTime(today.Year, (today.Month - 1) / 3 * 3 + 1, 1); t = today; break;
                case "custom":
                    f = (from ?? today.AddDays(-27)).Date; t = (to ?? today).Date;
                    if (t < f) { DateTime x = f; f = t; t = x; }
                    if (t > today) t = today;
                    if (f > t) f = t;
                    if ((t - f).TotalDays > 400) f = t.AddDays(-400);
                    break;
                default: key = "28d"; f = today.AddDays(-27); t = today; break;
            }
            int days = (int)(t - f).TotalDays + 1;
            DateTime pt = f.AddDays(-1);
            return new MkRange { Key = key, From = f, To = t, PrevTo = pt, PrevFrom = pt.AddDays(-(days - 1)) };
        }

        private static readonly ConcurrentDictionary<string, object> Locks = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Cache theo portal + báo cáo + khoảng (+ tham số). Nhiều người mở cùng lúc chỉ gọi GA4 một lần.
        /// Lỗi GA4 không cache, trả thông báo ngắn (chi tiết vào log DNN).
        /// </summary>
        private T Cached<T>(string name, MkRange r, string extra, Func<T> load, int? minutes = null) where T : class
        {
            string key = AnalyticsSettings.Prefix + "Mkt." + Portal + "." + name + "." + r.From.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "." +
                         r.To.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + (string.IsNullOrEmpty(extra) ? "" : "." + Hash(extra));
            if (DataCache.GetCache(key) is T hit) return hit;
            lock (Locks.GetOrAdd(key, _ => new object()))
            {
                if (DataCache.GetCache(key) is T again) return again;
                T v;
                try
                {
                    // HTTP + SQL trên thread pool để không nghẽn SynchronizationContext của ASP.NET
                    v = Task.Run(load).GetAwaiter().GetResult();
                }
                catch (AnalyticsException) { throw; }
                catch (Exception ex)
                {
                    DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                    throw new AnalyticsException("Không lấy được số liệu từ Google Analytics: " + Clean.Text(Clean.Short(ex), 200));
                }
                int min = minutes ?? (r.To >= DateTime.Today ? _s.CacheMinutes : Math.Max(60, _s.CacheMinutes));
                DataCache.SetCache(key, v, DateTime.Now.AddMinutes(min));
                return v;
            }
        }

        /// <summary>Truy vấn CSDL DNN (SiteSqlServer), đọc từng dòng.</summary>
        private static void Sql(string sql, Action<SqlCommand> args, Action<IDataRecord> row)
        {
            using (var cn = new SqlConnection(Config.GetConnectionString()))
            using (var cmd = new SqlCommand(sql, cn) { CommandTimeout = 30 })
            {
                args?.Invoke(cmd);
                cn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                    while (r.Read()) row(r);
            }
        }

        private static string Hash(string s)
        {
            using (SHA1 sha = SHA1.Create())
                return string.Concat(sha.ComputeHash(Encoding.UTF8.GetBytes(s ?? "")).Take(8).Select(b => b.ToString("x2")));
        }

        /// <summary>Bỏ dấu tiếng Việt, chữ thường (lọc tiêu đề không phân biệt dấu).</summary>
        internal static string Fold(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            string d = s.Replace('đ', 'd').Replace('Đ', 'D').Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(d.Length);
            foreach (char c in d)
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) sb.Append(c);
            return sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }

        // =================================================================================================
        // Gộp dòng GA4 thành MkRow (kỳ này + kỳ trước)
        // =================================================================================================

        /// <summary>Gộp theo khóa (hàm lấy khóa từ dòng); kỳ trước gắn vào Prev. Tỷ trọng theo phiên kỳ này.</summary>
        private static List<MkRow> Rows(GaTable t, Func<GaRow, string> key, string[] conv, Func<string, string> name = null)
        {
            var map = new Dictionary<string, MkRow>(StringComparer.Ordinal);
            foreach (GaRow row in t.Rows)
            {
                string k = key(row) ?? "";
                if (!map.TryGetValue(k, out MkRow m)) map[k] = m = new MkRow { Key = k, Name = name != null ? name(k) : k };
                if (row.Range == "prev")
                {
                    if (m.Prev == null) m.Prev = new MkMetrics();
                    m.Prev.Add(row.Metrics(conv));
                }
                else m.Add(row.Metrics(conv));
            }
            List<MkRow> list = map.Values.ToList();
            long total = list.Sum(x => x.Sessions);
            foreach (MkRow x in list)
            {
                x.Share = total > 0 ? Math.Round((double)x.Sessions / total, 4) : 0;
                x.Name = Clean.Text(x.Name, 300);
                x.Key = Clean.Text(x.Key, 300);
            }
            return list.OrderByDescending(x => x.Sessions).ThenByDescending(x => x.Prev?.Sessions ?? 0).ToList();
        }

        private static MkMetrics Total(GaTable t, string range, string[] conv)
        {
            var m = new MkMetrics();
            foreach (GaRow row in t.Rows.Where(x => x.Range == range)) m.Add(row.Metrics(conv));
            return m;
        }

        internal static string ChannelName(string raw)
        {
            switch ((raw ?? "").Trim())
            {
                case "Organic Search": return "Tìm kiếm tự nhiên";
                case "Paid Search": return "Tìm kiếm trả phí";
                case "Direct": return "Truy cập trực tiếp";
                case "Organic Social": return "Mạng xã hội tự nhiên";
                case "Paid Social": return "Mạng xã hội trả phí";
                case "Referral": return "Website giới thiệu";
                case "Email": return "Email";
                case "Display": return "Hiển thị (Display)";
                case "Paid Video": return "Video trả phí";
                case "Organic Video": return "Video tự nhiên";
                case "Affiliates": return "Tiếp thị liên kết";
                case "Cross-network": return "Đa mạng (Performance Max)";
                case "Paid Other": return "Trả phí khác";
                case "Paid Shopping": return "Mua sắm trả phí";
                case "Organic Shopping": return "Mua sắm tự nhiên";
                case "SMS": return "SMS";
                case "Mobile Push Notifications": return "Thông báo đẩy";
                case "Audio": return "Âm thanh";
                case "Unassigned": return "Chưa phân loại (Unassigned)";
                default: return raw;
            }
        }

        /// <summary>Loại trang theo đường dẫn của site công khai (Capstone.View): key + tên hiển thị.</summary>
        internal static readonly Tuple<string, string, string>[] PageTypes =
        {
            Tuple.Create("/dang-ky-tu-van/thanh-cong", "lead", "Đăng ký tư vấn thành công"),
            Tuple.Create("/dang-ky-tu-van", "form", "Form đăng ký tư vấn"),
            Tuple.Create("/dang-ky-su-kien", "form", "Form đăng ký tư vấn"),
            Tuple.Create("/tin-tuc", "news", "Tin tức / bài viết"),
            Tuple.Create("/tu-van-dinh-cu", "settle", "Tư vấn định cư"),
            Tuple.Create("/tu-van-dau-tu", "invest", "Tư vấn đầu tư"),
            Tuple.Create("/truong-doi-tac", "school", "Trường đối tác"),
            Tuple.Create("/tim-truong", "search", "Tìm trường / ngành"),
            Tuple.Create("/tim-nganh-hoc", "search", "Tìm trường / ngành"),
            Tuple.Create("/tim-kiem", "search", "Tìm trường / ngành"),
            Tuple.Create("/thong-tin-du-hoc", "country", "Thông tin du học theo nước"),
            Tuple.Create("/su-kien", "event", "Sự kiện / hội thảo"),
            Tuple.Create("/dich-vu", "service", "Dịch vụ"),
            Tuple.Create("/gioi-thieu", "about", "Giới thiệu"),
        };

        internal static Tuple<string, string> PageType(string path)
        {
            string p = (path ?? "").ToLowerInvariant();
            if (p == "/" || p.Length == 0) return Tuple.Create("home", "Trang chủ");
            foreach (Tuple<string, string, string> t in PageTypes)
                if (p == t.Item1 || p.StartsWith(t.Item1 + "/", StringComparison.Ordinal) || p.StartsWith(t.Item1 + "?", StringComparison.Ordinal))
                    return Tuple.Create(t.Item2, t.Item3);
            return Regex.IsMatch(p, @"-\d+\.html?$") ? Tuple.Create("news", "Tin tức / bài viết") : Tuple.Create("other", "Trang khác");
        }

        internal static string DeviceName(string raw)
        {
            switch ((raw ?? "").ToLowerInvariant())
            {
                case "mobile": return "Điện thoại";
                case "desktop": return "Máy tính";
                case "tablet": return "Máy tính bảng";
                case "smart tv": return "Smart TV";
                default: return string.IsNullOrEmpty(raw) ? "Khác" : raw;
            }
        }

        // =================================================================================================
        // Tổng quan
        // =================================================================================================

        public class MkOverview
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
            public DateTime PrevFrom { get; set; }
            public DateTime PrevTo { get; set; }
            public string Range { get; set; }
            public MkMetrics Summary { get; set; } = new MkMetrics();
            public MkMetrics Previous { get; set; } = new MkMetrics();
            public List<MkPoint> Series { get; set; } = new List<MkPoint>();
            public List<MkRow> Channels { get; set; } = new List<MkRow>();
            public List<MkRow> Devices { get; set; } = new List<MkRow>();
            public List<MkRow> UserTypes { get; set; } = new List<MkRow>();
            /// <summary>7 hàng (Thứ Hai .. Chủ nhật) x 24 giờ: số phiên.</summary>
            public List<long[]> Heat { get; set; } = new List<long[]>();
            public List<MkRow> Landing { get; set; } = new List<MkRow>();
            public List<MkRow> Countries { get; set; } = new List<MkRow>();
            public List<MkRow> SourceMedium { get; set; } = new List<MkRow>();
            public List<MkRow> Campaigns { get; set; } = new List<MkRow>();
            public List<MkContent> TopContent { get; set; } = new List<MkContent>();
            public List<MkCategory> Categories { get; set; } = new List<MkCategory>();
            /// <summary>Hiệu quả theo loại trang (trang vào đầu tiên): phiên, tương tác, chuyển đổi.</summary>
            public List<MkRow> PageTypes { get; set; } = new List<MkRow>();
            /// <summary>Lượt xem trang "Đăng ký tư vấn thành công" (đo lead khi chưa có key event).</summary>
            public long ThankYouViews { get; set; }
            public List<MkInsight> Insights { get; set; } = new List<MkInsight>();
            public object Target { get; set; }
            public bool HasConversions { get; set; }
            public DateTime GeneratedAt { get; set; } = DateTime.Now;
        }

        public object Overview(string range, DateTime? from, DateTime? to)
        {
            DemandGa();
            MkRange r = MakeRange(range, from, to);
            MkOverview o = Cached("overview", r, null, () => LoadOverview(r));
            return new { overview = o, realtime = RealtimeSafe() };
        }

        private MkOverview LoadOverview(MkRange r)
        {
            var g = new Ga4Query(_s);
            string[] std = g.Std, conv = g.Conv;
            List<GaTable> t = g.Batch(
                Ga4Query.Req(r, true, null, std),                                                                                   // 0 tổng
                Ga4Query.Req(r, true, new[] { "date" }, new[] { "activeUsers", "sessions", "screenPageViews" }.Concat(conv).ToArray(), null, "", 1000), // 1 theo ngày
                Ga4Query.Req(r, true, new[] { "sessionDefaultChannelGroup" }, std, null, "sessions", 100),                            // 2 kênh
                Ga4Query.Req(r, false, new[] { "deviceCategory" }, std, null, "sessions", 10),                                        // 3 thiết bị
                Ga4Query.Req(r, false, new[] { "newVsReturning" }, std, null, "sessions", 10),                                        // 4 mới / quay lại
                Ga4Query.Req(r, false, new[] { "dayOfWeek", "hour" }, new[] { "sessions" }, null, null, 200),                         // 5 thứ x giờ
                Ga4Query.Req(r, false, new[] { "landingPage" }, std, null, "sessions", 2000),                                         // 6 trang đích
                Ga4Query.Req(r, false, new[] { "country" }, std, null, "sessions", 10),                                               // 7 quốc gia
                Ga4Query.Req(r, true, new[] { "sessionSourceMedium" }, std, null, "sessions", 200),                                   // 8 nguồn / phương tiện
                Ga4Query.Req(r, true, new[] { "sessionCampaignName" }, std, null, "sessions", 400));                                  // 9 chiến dịch

            var o = new MkOverview { From = r.From, To = r.To, PrevFrom = r.PrevFrom, PrevTo = r.PrevTo, Range = r.Key };
            o.Summary = Total(t[0], "cur", conv);
            o.Previous = Total(t[0], "prev", conv);

            var cur = new Dictionary<string, GaRow>();
            var prev = new Dictionary<string, GaRow>();
            foreach (GaRow row in t[1].Rows) (row.Range == "prev" ? prev : cur)[row.D("date")] = row;
            for (DateTime d = r.From, p = r.PrevFrom; d <= r.To; d = d.AddDays(1), p = p.AddDays(1))
            {
                cur.TryGetValue(d.ToString("yyyyMMdd", CultureInfo.InvariantCulture), out GaRow c);
                prev.TryGetValue(p.ToString("yyyyMMdd", CultureInfo.InvariantCulture), out GaRow q);
                o.Series.Add(new MkPoint
                {
                    Date = d, Users = c?.L("activeUsers") ?? 0, Sessions = c?.L("sessions") ?? 0, Views = c?.L("screenPageViews") ?? 0,
                    Conversions = c?.Conv(conv) ?? 0, PrevSessions = q?.L("sessions") ?? 0, PrevViews = q?.L("screenPageViews") ?? 0
                });
            }

            o.Channels = Rows(t[2], x => x.D("sessionDefaultChannelGroup"), conv, ChannelName);
            o.Devices = Rows(t[3], x => x.D("deviceCategory"), conv, DeviceName);
            o.UserTypes = Rows(t[4], x => x.D("newVsReturning"), conv, k => k == "new" ? "Khách mới" : k == "returning" ? "Khách quay lại" : "Không xác định");
            o.Heat = Heat(t[5]);
            List<MkRow> landing = Rows(t[6], x => x.D("landingPage"), conv);
            o.Landing = landing.Take(20).ToList();
            o.Countries = Rows(t[7], x => x.D("country"), conv).Take(10).ToList();
            o.SourceMedium = Rows(t[8], x => x.D("sessionSourceMedium"), conv).Where(x => x.Sessions > 0).Take(15).ToList();
            List<MkRow> campaigns = Rows(t[9], x => x.D("sessionCampaignName"), conv).Where(x => !SystemCampaigns.Contains(x.Key) && x.Sessions > 0).ToList();
            o.Campaigns = campaigns.Take(10).ToList();
            o.HasConversions = o.Summary.Conversions > 0 || o.Previous.Conversions > 0;

            ContentData content = Content(r);
            o.TopContent = content.ByArticle.Where(x => x.NewsId > 0).Take(10).ToList();
            if (o.TopContent.Count == 0) o.TopContent = content.ByArticle.Take(10).ToList();
            o.ThankYouViews = content.ByPage.Where(x => x.Path.StartsWith("/dang-ky-tu-van/thanh-cong", StringComparison.OrdinalIgnoreCase)).Sum(x => x.Views);
            var types = new Dictionary<string, MkRow>();
            foreach (MkRow l in landing)
            {
                Tuple<string, string> pt = PageType(l.Key);
                if (!types.TryGetValue(pt.Item1, out MkRow tr)) types[pt.Item1] = tr = new MkRow { Key = pt.Item1, Name = pt.Item2 };
                tr.Add(l);
            }
            long typeTotal = types.Values.Sum(x => x.Sessions);
            foreach (MkRow tr in types.Values) tr.Share = typeTotal > 0 ? Math.Round((double)tr.Sessions / typeTotal, 4) : 0;
            o.PageTypes = types.Values.OrderByDescending(x => x.Sessions).ToList();
            o.Categories = Categories(r, content);
            List<MkUtmIssue> utm = Hygiene(r);
            o.Target = Target(r, o.Summary);
            o.Insights = new InsightBuilder(r, o, content, campaigns, utm, _s).Build();
            return o;
        }

        private static List<long[]> Heat(GaTable t)
        {
            var heat = Enumerable.Range(0, 7).Select(_ => new long[24]).ToList();
            foreach (GaRow row in t.Rows)
            {
                if (!int.TryParse(row.D("dayOfWeek"), out int dow) || dow < 0 || dow > 6) continue;
                if (!int.TryParse(row.D("hour"), out int h) || h < 0 || h > 23) continue;
                heat[(dow + 6) % 7][h] += row.L("sessions");   // GA4: 0 = Chủ nhật -> hàng 0 = Thứ Hai
            }
            return heat;
        }

        /// <summary>Tiến độ so với mục tiêu phiên / tháng, quy theo số ngày của khoảng.</summary>
        private object Target(MkRange r, MkMetrics s)
        {
            if (_s.MonthlyTarget <= 0) return null;
            double perDay = _s.MonthlyTarget / 30.4375;
            if (r.Key == "month")
            {
                int dim = DateTime.DaysInMonth(r.From.Year, r.From.Month), elapsed = Math.Max(1, (int)(DateTime.Now - r.From).TotalDays);
                elapsed = Math.Min(elapsed, dim);
                double projected = (double)s.Sessions / elapsed * dim;
                return new { goal = _s.MonthlyTarget, actual = s.Sessions, expected = Math.Round((double)_s.MonthlyTarget * elapsed / dim), projected = Math.Round(projected), progress = Math.Round(s.Sessions / (double)_s.MonthlyTarget, 4), pace = Math.Round(projected / _s.MonthlyTarget, 4), month = true };
            }
            double expected = perDay * r.Days;
            return new { goal = Math.Round(expected), actual = s.Sessions, expected = Math.Round(expected), projected = (double)s.Sessions, progress = expected > 0 ? Math.Round(s.Sessions / expected, 4) : 0, pace = expected > 0 ? Math.Round(s.Sessions / expected, 4) : 0, month = false };
        }

        // =================================================================================================
        // Realtime
        // =================================================================================================

        private object RealtimeSafe()
        {
            try
            {
                var r = new MkRange { Key = "rt", From = DateTime.Today, To = DateTime.Today, PrevFrom = DateTime.Today, PrevTo = DateTime.Today };
                return Cached("realtime", r, null, () =>
                {
                    var g = new Ga4Query(_s);
                    GaTable total = g.Realtime(new Dictionary<string, object> { ["metrics"] = new[] { new Dictionary<string, object> { ["name"] = "activeUsers" } } });
                    GaTable pages = g.Realtime(new Dictionary<string, object>
                    {
                        ["dimensions"] = new[] { new Dictionary<string, object> { ["name"] = "unifiedScreenName" } },
                        ["metrics"] = new[] { new Dictionary<string, object> { ["name"] = "activeUsers" } },
                        ["limit"] = 5
                    });
                    return (object)new
                    {
                        activeUsers = total.Rows.Sum(x => x.L("activeUsers")),
                        pages = pages.Rows.Select(x => new { title = Clean.Text(x.D("unifiedScreenName"), 300), users = x.L("activeUsers") }).ToList()
                    };
                }, 1);
            }
            catch (AnalyticsException)
            {
                return null;   // realtime là phụ: lỗi quyền / mạng không làm hỏng tổng quan
            }
        }

        // =================================================================================================
        // Nội dung
        // =================================================================================================

        internal sealed class ContentData
        {
            public List<MkContent> ByPage { get; set; } = new List<MkContent>();
            public List<MkContent> ByArticle { get; set; } = new List<MkContent>();
            public long TotalViews { get; set; }
            public long PrevTotalViews { get; set; }
            public bool Truncated { get; set; }
            public double V50, V75, E25, E50, E75;
        }

        internal ContentData Content(MkRange r) => Cached("content", r, null, () => LoadContent(r));

        private ContentData LoadContent(MkRange r)
        {
            var g = new Ga4Query(_s);
            GaTable t = g.Run(Ga4Query.Req(r, true, new[] { "pagePath", "pageTitle", "hostName" }, g.PageMets, null, "screenPageViews", 10000));
            string[] conv = g.Conv;

            // theo đường dẫn (một trang có thể nhiều tiêu đề / host): tiêu đề = tiêu đề nhiều lượt xem nhất kỳ này
            var pages = new Dictionary<string, MkContent>(StringComparer.Ordinal);
            var titles = new Dictionary<string, Dictionary<string, long>>(StringComparer.Ordinal);
            foreach (GaRow row in t.Rows)
            {
                string path = row.D("pagePath");
                if (string.IsNullOrEmpty(path)) continue;
                if (!pages.TryGetValue(path, out MkContent p))
                {
                    pages[path] = p = new MkContent { Key = path, Path = path, NewsId = _s.ArticleIdOf(path), Paths = 1 };
                    p.AllPaths.Add(path);
                    titles[path] = new Dictionary<string, long>(StringComparer.Ordinal);
                }
                long v = row.L("screenPageViews");
                if (string.IsNullOrEmpty(p.Host)) p.Host = row.D("hostName");
                if (row.Range == "prev") { p.PrevViews += v; continue; }
                p.Views += v;
                p.Users += row.L("activeUsers");
                p.EngagementSec += row.M("userEngagementDuration");
                p.Conversions += row.Conv(conv);
                Dictionary<string, long> tv = titles[path];
                tv.TryGetValue(row.D("pageTitle"), out long bv);
                tv[row.D("pageTitle")] = bv + v;
            }
            foreach (MkContent p in pages.Values)
                p.Title = titles[p.Path].OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault() ?? "";

            var data = new ContentData { Truncated = t.RowCount > 10000 };
            data.ByPage = pages.Values.ToList();

            // gộp theo bài (cùng NewId nhiều URL: amp, đổi slug, ?page…): đường dẫn / tiêu đề đại diện = URL nhiều lượt xem nhất
            var byArticle = new Dictionary<string, MkContent>(StringComparer.Ordinal);
            var mainViews = new Dictionary<string, long>(StringComparer.Ordinal);
            foreach (MkContent p in data.ByPage)
            {
                string k = p.NewsId > 0 ? "n" + p.NewsId : p.Path;
                if (!byArticle.TryGetValue(k, out MkContent a))
                    byArticle[k] = a = new MkContent { Key = k, NewsId = p.NewsId };
                if (!mainViews.TryGetValue(k, out long mv) || p.Views > mv || a.Path == null)
                {
                    mainViews[k] = p.Views;
                    a.Path = p.Path; a.Title = p.Title; a.Host = p.Host;
                }
                a.AllPaths.Add(p.Path);
                a.Paths = a.AllPaths.Count;
                a.Views += p.Views; a.PrevViews += p.PrevViews; a.Users += p.Users; a.EngagementSec += p.EngagementSec; a.Conversions += p.Conversions;
            }
            data.ByArticle = byArticle.Values.ToList();

            Decorate(data);
            data.TotalViews = data.ByPage.Sum(x => x.Views);
            data.PrevTotalViews = data.ByPage.Sum(x => x.PrevViews);
            Classify(data, data.ByArticle, true);
            Classify(data, data.ByPage, false);
            data.ByArticle = data.ByArticle.OrderByDescending(x => x.Views).ToList();
            data.ByPage = data.ByPage.OrderByDescending(x => x.Views).ToList();
            return data;
        }

        private sealed class ArticleRef
        {
            public int NewId { get; set; }
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string Title { get; set; }
            public string AuthorName { get; set; }
            public DateTime? PublishedDate { get; set; }
        }

        /// <summary>Tiêu đề / chuyên mục / tác giả / ngày xuất bản từ CSDL cho trang bài; làm sạch chữ do trình duyệt gửi lên GA.</summary>
        private void Decorate(ContentData data)
        {
            List<int> ids = data.ByArticle.Where(x => x.NewsId > 0).Select(x => x.NewsId).Distinct().ToList();
            var refs = new Dictionary<int, ArticleRef>();
            for (int i = 0; i < ids.Count; i += 1000)
                Sql(@"
SELECT n.NewId, ISNULL(n.CategoryId, 0) AS CategoryId, c.CategoryName, n.Title,
       COALESCE(NULLIF(LTRIM(RTRIM(n.ButDanh)), ''), NULLIF(u.DisplayName, ''), u.Username) AS AuthorName, n.PublishedDate
  FROM dbo.News n
  LEFT JOIN dbo.NewsCategories c ON c.CategoryID = n.CategoryId
  LEFT JOIN dbo.Users u ON u.UserID = n.UserId
 WHERE n.NewId IN (" + string.Join(",", ids.Skip(i).Take(1000)) + ") AND n.PortalId = @PortalId",
                    cmd => cmd.Parameters.AddWithValue("@PortalId", Portal),
                    r => refs[(int)r["NewId"]] = new ArticleRef
                    {
                        NewId = (int)r["NewId"], CategoryId = Convert.ToInt32(r["CategoryId"]), CategoryName = r["CategoryName"] as string,
                        Title = r["Title"] as string, AuthorName = r["AuthorName"] as string, PublishedDate = r["PublishedDate"] as DateTime?
                    });

            foreach (MkContent x in data.ByArticle.Concat(data.ByPage))
            {
                if (x.NewsId > 0 && refs.TryGetValue(x.NewsId, out ArticleRef a))
                {
                    if (!string.IsNullOrWhiteSpace(a.Title)) x.Title = a.Title;
                    x.CategoryId = a.CategoryId;
                    x.CategoryName = a.CategoryName;
                    x.AuthorName = a.AuthorName;
                    x.PublishedDate = a.PublishedDate;
                }
                x.Title = Clean.Text(string.IsNullOrWhiteSpace(x.Title) || x.Title == "(not set)" ? x.Path : x.Title, 300);
                x.Path = Clean.Text(x.Path, 1000);
                string host = Clean.Text(x.Host, 200);
                x.Url = Clean.SafeUrl(string.IsNullOrEmpty(host) || host == "(not set)" ? x.Path : "https://" + host + x.Path);
                x.EngagementSec = Math.Round(x.EngagementSec, 1);
            }
        }

        private static double Pct(List<double> sorted, double p)
        {
            if (sorted.Count == 0) return 0;
            double idx = (sorted.Count - 1) * p;
            int lo = (int)Math.Floor(idx), hi = (int)Math.Ceiling(idx);
            return sorted[lo] + (sorted[hi] - sorted[lo]) * (idx - lo);
        }

        /// <summary>
        /// Phân loại theo ma trận lượt xem x thời gian đọc (ngưỡng = phân vị của các trang đủ dữ liệu):
        /// star (xem nhiều + đọc sâu), optimize (xem nhiều + đọc lướt), potential (ít xem + đọc rất sâu), low (quá ít dữ liệu), normal.
        /// Xu hướng: new (kỳ trước chưa có), up (gấp đôi), down (mất từ một nửa, trước đó đáng kể).
        /// </summary>
        private static void Classify(ContentData data, List<MkContent> list, bool keep)
        {
            const int min = 5;
            List<MkContent> enough = list.Where(x => x.Views >= min && x.Users > 0).ToList();
            List<double> vs = enough.Select(x => (double)x.Views).OrderBy(x => x).ToList();
            List<double> es = enough.Select(x => x.AvgEngagementSec).OrderBy(x => x).ToList();
            double v50 = Pct(vs, .5), v75 = Pct(vs, .75), e25 = Pct(es, .25), e50 = Pct(es, .5), e75 = Pct(es, .75);
            List<double> pv = list.Where(x => x.PrevViews >= min).Select(x => (double)x.PrevViews).OrderBy(x => x).ToList();
            double p50 = Pct(pv, .5);
            long total = list.Sum(x => x.Views);
            foreach (MkContent x in list)
            {
                x.Share = total > 0 ? Math.Round((double)x.Views / total, 4) : 0;
                double e = x.AvgEngagementSec;
                if (x.Views < min || x.Users == 0) x.Segment = "low";
                else if (x.Views >= v75 && e >= e50) x.Segment = "star";
                else if (x.Views >= v75 && e < e25) x.Segment = "optimize";
                else if (x.Views < v50 && e >= e75 && e > 0) x.Segment = "potential";
                else x.Segment = "normal";

                if (x.PrevViews == 0 && x.Views >= Math.Max(min, v50)) x.Trend = "new";
                else if (x.PrevViews >= min && x.Views >= 2 * x.PrevViews) x.Trend = "up";
                else if (x.PrevViews >= Math.Max(20, p50) && x.Views <= x.PrevViews / 2) x.Trend = "down";
                else x.Trend = null;
            }
            if (keep) { data.V50 = v50; data.V75 = v75; data.E25 = e25; data.E50 = e50; data.E75 = e75; }
        }

        private static readonly Dictionary<string, Func<MkContent, double>> ContentSorts = new Dictionary<string, Func<MkContent, double>>(StringComparer.OrdinalIgnoreCase)
        {
            ["views"] = x => x.Views, ["users"] = x => x.Users, ["engagement"] = x => x.AvgEngagementSec, ["conversions"] = x => x.Conversions,
            ["growth"] = x => x.Growth ?? (x.PrevViews == 0 && x.Views > 0 ? double.MaxValue / 4 : double.MinValue / 4),
            ["delta"] = x => x.Views - x.PrevViews, ["vpu"] = x => x.ViewsPerUser,
            ["published"] = x => x.PublishedDate.HasValue ? x.PublishedDate.Value.Ticks : 0
        };

        /// <summary>
        /// Bảng nội dung: group = article (gộp URL cùng bài) | page; q lọc theo tiêu đề / đường dẫn (không dấu, nhiều từ = phải có đủ);
        /// segment / trend / categoryId lọc tiếp; sort + dir + phân trang.
        /// </summary>
        public object ContentList(string range, DateTime? from, DateTime? to, string group, string q, string segment, string trend, int categoryId, string sort, string dir, int page, int pageSize)
        {
            DemandGa();
            MkRange r = MakeRange(range, from, to);
            ContentData data = Content(r);
            bool byPage = string.Equals(group, "page", StringComparison.OrdinalIgnoreCase);
            IEnumerable<MkContent> list = byPage ? data.ByPage : data.ByArticle;

            string[] words = Fold(q).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Take(8).ToArray();
            if (words.Length > 0)
                list = list.Where(x => { string h = Fold(x.Title) + " " + Fold(x.Path); return words.All(w => h.Contains(w)); });
            if (categoryId > 0) list = list.Where(x => x.CategoryId == categoryId);
            List<MkContent> filtered = list.ToList();

            // đếm phân loại trên tập đã lọc theo tiêu đề / chuyên mục (chip lọc hiển thị số)
            var segments = filtered.GroupBy(x => x.Segment).ToDictionary(x => x.Key ?? "normal", x => x.Count());
            var trends = filtered.Where(x => x.Trend != null).GroupBy(x => x.Trend).ToDictionary(x => x.Key, x => x.Count());
            if (!string.IsNullOrEmpty(segment)) filtered = filtered.Where(x => x.Segment == segment).ToList();
            if (!string.IsNullOrEmpty(trend)) filtered = filtered.Where(x => x.Trend == trend).ToList();

            Func<MkContent, double> key = ContentSorts.TryGetValue(sort ?? "", out Func<MkContent, double> k) ? k : ContentSorts["views"];
            bool asc = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
            filtered = (asc ? filtered.OrderBy(key) : filtered.OrderByDescending(key)).ThenByDescending(x => x.Views).ToList();

            pageSize = pageSize <= 0 ? 50 : Math.Min(pageSize, 500);
            int total = filtered.Count, pages = Math.Max(1, (total + pageSize - 1) / pageSize);
            page = Math.Max(1, Math.Min(page <= 0 ? 1 : page, pages));
            var sum = new MkContent
            {
                Views = filtered.Sum(x => x.Views), PrevViews = filtered.Sum(x => x.PrevViews), Users = filtered.Sum(x => x.Users),
                EngagementSec = Math.Round(filtered.Sum(x => x.EngagementSec), 1), Conversions = filtered.Sum(x => x.Conversions)
            };
            return new
            {
                from = r.From, to = r.To, prevFrom = r.PrevFrom, prevTo = r.PrevTo,
                group = byPage ? "page" : "article",
                total, page, pageSize,
                rows = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                summary = sum,
                siteViews = data.TotalViews,
                segments, trends,
                thresholds = new { v50 = Math.Round(data.V50), v75 = Math.Round(data.V75), e25 = Math.Round(data.E25), e50 = Math.Round(data.E50), e75 = Math.Round(data.E75) },
                categories = Categories(r, data),
                truncated = data.Truncated
            };
        }

        // =================================================================================================
        // Chuyên mục: cầu (lượt xem) - cung (bài xuất bản trong kỳ)
        // =================================================================================================

        private sealed class PublishedCount
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public int Published { get; set; }
        }

        internal List<MkCategory> Categories(MkRange r, ContentData data) => Cached("categories", r, null, () =>
        {
            var map = new Dictionary<int, MkCategory>();
            foreach (MkContent x in data.ByArticle.Where(a => a.NewsId > 0 && a.CategoryId > 0))
            {
                if (!map.TryGetValue(x.CategoryId, out MkCategory c)) map[x.CategoryId] = c = new MkCategory { Id = x.CategoryId, Name = x.CategoryName };
                c.Views += x.Views; c.PrevViews += x.PrevViews; c.Users += x.Users; c.EngagementSec += x.EngagementSec;
                if (x.Views > 0) c.Articles++;
            }
            var counts = new List<PublishedCount>();
            Sql(@"
SELECT ISNULL(n.CategoryId, 0) AS CategoryId, MAX(c.CategoryName) AS CategoryName, COUNT(*) AS Published
  FROM dbo.News n
  LEFT JOIN dbo.NewsCategories c ON c.CategoryID = n.CategoryId
 WHERE n.PortalId = @PortalId AND n.Status = 2 AND n.PublishedDate >= @F AND n.PublishedDate < @T
 GROUP BY ISNULL(n.CategoryId, 0)",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@PortalId", Portal);
                    cmd.Parameters.Add("@F", SqlDbType.DateTime).Value = r.From;
                    cmd.Parameters.Add("@T", SqlDbType.DateTime).Value = r.To.AddDays(1);
                },
                rd => counts.Add(new PublishedCount { CategoryId = Convert.ToInt32(rd["CategoryId"]), CategoryName = rd["CategoryName"] as string, Published = Convert.ToInt32(rd["Published"]) }));
            foreach (PublishedCount p in counts)
            {
                if (p.CategoryId <= 0) continue;
                if (!map.TryGetValue(p.CategoryId, out MkCategory c)) map[p.CategoryId] = c = new MkCategory { Id = p.CategoryId, Name = p.CategoryName };
                c.Published = p.Published;
            }
            List<MkCategory> list = map.Values.ToList();
            long views = list.Sum(x => x.Views);
            int pub = list.Sum(x => x.Published);
            foreach (MkCategory c in list)
            {
                c.Name = Clean.Text(string.IsNullOrEmpty(c.Name) ? "Chuyên mục #" + c.Id : c.Name, 200);
                c.EngagementSec = Math.Round(c.EngagementSec, 1);
                c.ViewShare = views > 0 ? Math.Round((double)c.Views / views, 4) : 0;
                c.PublishShare = pub > 0 ? Math.Round((double)c.Published / pub, 4) : 0;
                c.DemandIndex = c.PublishShare > 0 ? Math.Round(c.ViewShare / c.PublishShare, 2) : (double?)null;
                c.Balance = c.Published == 0 && c.ViewShare >= 0.03 ? "under"
                          : c.DemandIndex >= 1.5 && c.ViewShare >= 0.03 ? "under"
                          : c.DemandIndex.HasValue && c.DemandIndex <= 0.5 && c.PublishShare >= 0.05 ? "over"
                          : "balanced";
            }
            return list.OrderByDescending(x => x.Views).ThenByDescending(x => x.Published).ToList();
        });

        // =================================================================================================
        // Chi tiết một bài / trang
        // =================================================================================================

        public object Article(string range, DateTime? from, DateTime? to, string key)
        {
            DemandGa();
            MkRange r = MakeRange(range, from, to);
            key = (key ?? "").Trim();
            if (key.Length == 0 || key.Length > 1000) throw new ArgumentException("Thiếu bài viết / đường dẫn.");
            ContentData data = Content(r);
            MkContent item = data.ByArticle.FirstOrDefault(x => x.Key == key) ?? data.ByPage.FirstOrDefault(x => x.Key == key);
            List<string> paths = item != null && item.AllPaths.Count > 0 ? item.AllPaths.Take(50).ToList() : new List<string> { key };
            // bài không có lượt xem trong kỳ: không biết đường dẫn để lọc -> báo rỗng
            if (item == null && !key.StartsWith("/", StringComparison.Ordinal))
                return new { item = (MkContent)null, empty = true };

            object payload = Cached("article", r, string.Join("\n", paths), () =>
            {
                var g = new Ga4Query(_s);
                string[] pm = g.PageMets, conv = g.Conv;
                object f = Ga4Query.InList("pagePath", paths);
                List<GaTable> t = g.Batch(
                    Ga4Query.Req(r, true, null, pm, f),                                                                       // 0 tổng
                    Ga4Query.Req(r, false, new[] { "date" }, pm, f, "", 1000),                                                // 1 theo ngày
                    Ga4Query.Req(r, false, new[] { "sessionSource", "sessionMedium" }, pm, f, "screenPageViews", 20),         // 2 nguồn
                    Ga4Query.Req(r, false, new[] { "sessionCampaignName" }, pm, f, "screenPageViews", 20),                   // 3 chiến dịch
                    Ga4Query.Req(r, false, new[] { "deviceCategory" }, pm, f, "screenPageViews", 10),                        // 4 thiết bị
                    Ga4Query.Req(r, false, new[] { "city" }, pm, f, "screenPageViews", 10),                                  // 5 thành phố
                    Ga4Query.Req(r, false, new[] { "pageReferrer" }, pm, f, "screenPageViews", 10),                          // 6 trang dẫn tới
                    Ga4Query.Req(r, false, new[] { "sessionDefaultChannelGroup" }, pm, f, "screenPageViews", 20));           // 7 kênh

                Func<GaTable, string, MkContent> tot = (tb, rg) =>
                {
                    var m = new MkContent();
                    foreach (GaRow x in tb.Rows.Where(x => x.Range == rg))
                    { m.Views += x.L("screenPageViews"); m.Users += x.L("activeUsers"); m.EngagementSec += x.M("userEngagementDuration"); m.Conversions += x.Conv(conv); }
                    m.EngagementSec = Math.Round(m.EngagementSec, 1);
                    return m;
                };
                var byDate = t[1].Rows.ToDictionary(x => x.D("date"), x => x);
                var series = new List<object>();
                for (DateTime d = r.From; d <= r.To; d = d.AddDays(1))
                {
                    byDate.TryGetValue(d.ToString("yyyyMMdd", CultureInfo.InvariantCulture), out GaRow x);
                    series.Add(new { date = d, views = x?.L("screenPageViews") ?? 0, users = x?.L("activeUsers") ?? 0, engagementSec = Math.Round(x?.M("userEngagementDuration") ?? 0, 1) });
                }
                Func<GaTable, Func<GaRow, string>, Func<string, string>, List<MkSlice>> slices = (tb, k, nm) =>
                {
                    List<MkSlice> list = tb.Rows.GroupBy(k).Select(gr => new MkSlice { Key = Clean.Text(gr.Key, 300), Name = Clean.Text(nm != null ? nm(gr.Key) : gr.Key, 300), Value = gr.Sum(x => x.L("screenPageViews")) })
                        .OrderByDescending(x => x.Value).ToList();
                    double sum = list.Sum(x => x.Value);
                    foreach (MkSlice s in list) s.Share = sum > 0 ? Math.Round(s.Value / sum, 4) : 0;
                    return list;
                };
                return (object)new
                {
                    total = tot(t[0], "cur"),
                    previous = tot(t[0], "prev"),
                    series,
                    sources = slices(t[2], x => x.D("sessionSource") + " / " + x.D("sessionMedium"), null),
                    campaigns = slices(t[3], x => x.D("sessionCampaignName"), null).Where(x => !SystemCampaigns.Contains(x.Key)).ToList(),
                    devices = slices(t[4], x => x.D("deviceCategory"), DeviceName),
                    cities = slices(t[5], x => x.D("city"), null),
                    referrers = slices(t[6], x => x.D("pageReferrer"), null).Where(x => x.Key.Length > 0 && x.Key != "(not set)").Select(x => { x.Key = Clean.SafeUrl(x.Key); return x; }).ToList(),
                    channels = slices(t[7], x => x.D("sessionDefaultChannelGroup"), ChannelName)
                };
            });
            return new { item, paths, data = payload, from = r.From, to = r.To, prevFrom = r.PrevFrom, prevTo = r.PrevTo, thresholds = new { e50 = Math.Round(data.E50), v75 = Math.Round(data.V75) } };
        }

        // =================================================================================================
        // Nguồn & UTM
        // =================================================================================================

        internal static readonly Dictionary<string, Tuple<string, string>> AcqDims = new Dictionary<string, Tuple<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["channel"] = Tuple.Create("sessionDefaultChannelGroup", "Kênh"),
            ["sourceMedium"] = Tuple.Create("sessionSourceMedium", "Nguồn / Phương tiện"),
            ["source"] = Tuple.Create("sessionSource", "utm_source"),
            ["medium"] = Tuple.Create("sessionMedium", "utm_medium"),
            ["campaign"] = Tuple.Create("sessionCampaignName", "utm_campaign"),
            ["content"] = Tuple.Create("sessionManualAdContent", "utm_content"),
            ["term"] = Tuple.Create("sessionManualTerm", "utm_term"),
            ["landing"] = Tuple.Create("landingPage", "Trang đích")
        };

        public object Acquisition(string range, DateTime? from, DateTime? to, string dim)
        {
            DemandGa();
            MkRange r = MakeRange(range, from, to);
            if (!AcqDims.TryGetValue(dim ?? "", out Tuple<string, string> d)) { dim = "channel"; d = AcqDims["channel"]; }
            string gaDim = d.Item1;
            bool channel = dim.Equals("channel", StringComparison.OrdinalIgnoreCase);
            List<MkRow> rows = Cached("acq", r, gaDim, () =>
            {
                var g = new Ga4Query(_s);
                GaTable t = g.Run(Ga4Query.Req(r, true, new[] { gaDim }, g.Std, null, "sessions", 1000));
                return Rows(t, x => x.D(gaDim), g.Conv, channel ? (Func<string, string>)ChannelName : null);
            });
            var total = new MkMetrics();
            var prevTotal = new MkMetrics();
            foreach (MkRow x in rows) { total.Add(x); prevTotal.Add(x.Prev); }
            return new
            {
                from = r.From, to = r.To, prevFrom = r.PrevFrom, prevTo = r.PrevTo,
                dim = dim, dimName = d.Item2,
                rows = rows.Take(300).ToList(),
                more = Math.Max(0, rows.Count - 300),
                total, previous = prevTotal,
                utm = Hygiene(r)
            };
        }

        /// <summary>
        /// Kiểm tra quy ước UTM trên các phiên có source / medium / campaign:
        /// trùng giá trị khác hoa thường (Facebook vs facebook), medium ngoài quy ước GA4 (rơi vào Unassigned), khoảng trắng / chữ hoa,
        /// có utm_source nhưng thiếu utm_campaign.
        /// </summary>
        internal List<MkUtmIssue> Hygiene(MkRange r) => Cached("utm", r, null, () =>
        {
            var g = new Ga4Query(_s);
            GaTable t = g.Run(Ga4Query.Req(r, false, new[] { "sessionSource", "sessionMedium", "sessionCampaignName" }, new[] { "sessions" }, null, "sessions", 2000));
            var issues = new List<MkUtmIssue>();
            var fields = new[] { Tuple.Create("sessionSource", "utm_source"), Tuple.Create("sessionMedium", "utm_medium"), Tuple.Create("sessionCampaignName", "utm_campaign") };
            foreach (Tuple<string, string> f in fields)
            {
                var vals = t.Rows.GroupBy(x => x.D(f.Item1)).Select(gr => new { v = gr.Key, s = gr.Sum(x => x.L("sessions")) })
                    .Where(x => !SystemCampaigns.Contains(x.v)).ToList();
                foreach (var dup in vals.GroupBy(x => x.v.Trim().ToLowerInvariant()).Where(gr => gr.Count() > 1))
                    issues.Add(new MkUtmIssue
                    {
                        Kind = "case", Field = f.Item2, Sessions = dup.Sum(x => x.s),
                        Values = dup.OrderByDescending(x => x.s).Select(x => Clean.Text(x.v, 100)).ToList(),
                        Message = f.Item2 + " bị ghi nhiều kiểu cho cùng một giá trị - GA4 tách thành nhiều dòng, số liệu bị chia nhỏ. Thống nhất chữ thường."
                    });
                List<string> spaced = vals.Where(x => x.v.Contains(" ") || x.v != x.v.Trim()).OrderByDescending(x => x.s).Select(x => x.v).Take(8).ToList();
                if (spaced.Count > 0)
                    issues.Add(new MkUtmIssue
                    {
                        Kind = "spaces", Field = f.Item2, Sessions = vals.Where(x => spaced.Contains(x.v)).Sum(x => x.s),
                        Values = spaced.Select(x => Clean.Text(x, 100)).ToList(),
                        Message = f.Item2 + " có khoảng trắng - dễ sai khi mã hóa URL. Dùng dấu gạch ngang hoặc gạch dưới (VD: tuyen-sinh-2026)."
                    });
            }
            var mediums = t.Rows.GroupBy(x => x.D("sessionMedium")).Select(gr => new { v = gr.Key, s = gr.Sum(x => x.L("sessions")) }).ToList();
            var odd = mediums.Where(x => !SystemCampaigns.Contains(x.v) && !StandardMediums.Contains(x.v.Trim().ToLowerInvariant())).OrderByDescending(x => x.s).Take(10).ToList();
            if (odd.Count > 0)
                issues.Add(new MkUtmIssue
                {
                    Kind = "nonstandard", Field = "utm_medium", Sessions = odd.Sum(x => x.s), Values = odd.Select(x => Clean.Text(x.v, 100)).ToList(),
                    Message = "utm_medium ngoài quy ước của GA4 -> phiên rơi vào kênh \"Unassigned\". Dùng: cpc, social, paid-social, email, display, referral, affiliate, sms, push, qr."
                });
            var noCampaign = t.Rows.Where(x => SystemCampaigns.Contains(x.D("sessionCampaignName")) && !SystemCampaigns.Contains(x.D("sessionMedium"))
                                               && !new[] { "organic", "referral" }.Contains(x.D("sessionMedium").ToLowerInvariant()))
                .GroupBy(x => x.D("sessionSource") + " / " + x.D("sessionMedium")).Select(gr => new { v = gr.Key, s = gr.Sum(x => x.L("sessions")) })
                .OrderByDescending(x => x.s).Take(8).ToList();
            if (noCampaign.Count > 0 && noCampaign.Sum(x => x.s) >= 10)
                issues.Add(new MkUtmIssue
                {
                    Kind = "notset", Field = "utm_campaign", Sessions = noCampaign.Sum(x => x.s), Values = noCampaign.Select(x => Clean.Text(x.v, 200)).ToList(),
                    Message = "Có utm_source / utm_medium nhưng thiếu utm_campaign -> không đo được hiệu quả từng chiến dịch."
                });
            return issues.OrderByDescending(x => x.Sessions).ToList();
        });

        // =================================================================================================
        // Chiến dịch
        // =================================================================================================

        public object Campaigns(string range, DateTime? from, DateTime? to)
        {
            DemandGa();
            MkRange r = MakeRange(range, from, to);
            return Cached("campaigns", r, null, () =>
            {
                var g = new Ga4Query(_s);
                string[] std = g.Std, conv = g.Conv;
                object notSystem = Ga4Query.Not(Ga4Query.InList("sessionCampaignName", SystemCampaigns.Where(x => x.Length > 0)));
                List<GaTable> t = g.Batch(
                    Ga4Query.Req(r, true, new[] { "sessionCampaignName" }, std, notSystem, "sessions", 1000),                                         // 0
                    Ga4Query.Req(r, false, new[] { "sessionCampaignName", "sessionSourceMedium" }, std, notSystem, "sessions", 3000),                  // 1
                    Ga4Query.Req(r, false, new[] { "date", "sessionCampaignName" }, new[] { "sessions" }.Concat(conv).ToArray(), notSystem, "", 10000), // 2
                    Ga4Query.Req(r, true, null, std));                                                                                                // 3 cả site
                List<MkRow> rows = Rows(t[0], x => x.D("sessionCampaignName"), conv).Where(x => x.Sessions > 0 || (x.Prev?.Sessions ?? 0) > 0).ToList();
                var bySm = t[1].Rows.GroupBy(x => x.D("sessionCampaignName")).ToDictionary(gr => gr.Key, gr => gr.OrderByDescending(x => x.L("sessions")).ToList());
                foreach (MkRow x in rows)
                    if (bySm.TryGetValue(x.Key, out List<GaRow> sm) && sm.Count > 0)
                        x.Sub = Clean.Text(sm[0].D("sessionSourceMedium"), 200) + (sm.Count > 1 ? " +" + (sm.Count - 1) : "");
                MkMetrics site = Total(t[3], "cur", conv);

                List<string> top = rows.Where(x => x.Sessions > 0).Take(5).Select(x => x.Key).ToList();
                var daily = t[2].Rows.GroupBy(x => x.D("sessionCampaignName")).ToDictionary(gr => gr.Key, gr => gr.ToDictionary(x => x.D("date"), x => x.L("sessions")));
                var dates = new List<DateTime>();
                for (DateTime d = r.From; d <= r.To; d = d.AddDays(1)) dates.Add(d);
                var series = top.Select(name => new
                {
                    name = Clean.Text(name, 300),
                    values = dates.Select(d => daily.TryGetValue(name, out Dictionary<string, long> m) && m.TryGetValue(d.ToString("yyyyMMdd", CultureInfo.InvariantCulture), out long v) ? v : 0).ToList()
                }).ToList();

                double siteEr = site.EngagementRate, siteCr = site.ConversionRate;
                var flags = rows.Select(x => new
                {
                    key = x.Key,
                    noConv = x.Sessions >= 30 && x.Conversions == 0,
                    lowEng = x.Sessions >= 30 && x.EngagementRate < siteEr - 0.15,
                    bestConv = x.Sessions >= 30 && x.Conversions > 0 && x.ConversionRate >= Math.Max(siteCr * 1.5, 0.005)
                }).Where(x => x.noConv || x.lowEng || x.bestConv).ToList();
                var campTotal = new MkMetrics();
                foreach (MkRow x in rows) campTotal.Add(x);
                return (object)new
                {
                    from = r.From, to = r.To, prevFrom = r.PrevFrom, prevTo = r.PrevTo,
                    rows, site, campaigns = campTotal,
                    campaignShare = site.Sessions > 0 ? Math.Round((double)campTotal.Sessions / site.Sessions, 4) : 0,
                    dates, series, flags
                };
            });
        }

        public object Campaign(string range, DateTime? from, DateTime? to, string name)
        {
            DemandGa();
            MkRange r = MakeRange(range, from, to);
            name = name ?? "";
            if (name.Length == 0 || name.Length > 500) throw new ArgumentException("Thiếu tên chiến dịch.");
            return Cached("campaign", r, name, () =>
            {
                var g = new Ga4Query(_s);
                string[] std = g.Std, conv = g.Conv;
                object f = Ga4Query.Exact("sessionCampaignName", name);
                List<GaTable> t = g.Batch(
                    Ga4Query.Req(r, true, null, std, f),                                                                                  // 0
                    Ga4Query.Req(r, false, new[] { "date" }, new[] { "sessions", "activeUsers", "engagedSessions" }.Concat(conv).ToArray(), f, "", 1000), // 1
                    Ga4Query.Req(r, false, new[] { "sessionSourceMedium" }, std, f, "sessions", 50),                                     // 2
                    Ga4Query.Req(r, false, new[] { "sessionManualAdContent" }, std, f, "sessions", 50),                                  // 3
                    Ga4Query.Req(r, false, new[] { "sessionManualTerm" }, std, f, "sessions", 50),                                       // 4
                    Ga4Query.Req(r, false, new[] { "landingPage" }, std, f, "sessions", 20),                                             // 5
                    Ga4Query.Req(r, false, new[] { "deviceCategory" }, std, f, "sessions", 10),                                          // 6
                    Ga4Query.Req(r, false, new[] { "city" }, std, f, "sessions", 10));                                                   // 7
                var byDate = t[1].Rows.ToDictionary(x => x.D("date"), x => x);
                var series = new List<object>();
                for (DateTime d = r.From; d <= r.To; d = d.AddDays(1))
                {
                    byDate.TryGetValue(d.ToString("yyyyMMdd", CultureInfo.InvariantCulture), out GaRow x);
                    series.Add(new { date = d, sessions = x?.L("sessions") ?? 0, users = x?.L("activeUsers") ?? 0, engaged = x?.L("engagedSessions") ?? 0, conversions = x?.Conv(conv) ?? 0 });
                }
                return (object)new
                {
                    name = Clean.Text(name, 300),
                    from = r.From, to = r.To, prevFrom = r.PrevFrom, prevTo = r.PrevTo,
                    total = Total(t[0], "cur", conv), previous = Total(t[0], "prev", conv),
                    series,
                    sourceMedium = Rows(t[2], x => x.D("sessionSourceMedium"), conv),
                    content = Rows(t[3], x => x.D("sessionManualAdContent"), conv),
                    term = Rows(t[4], x => x.D("sessionManualTerm"), conv),
                    landing = Rows(t[5], x => x.D("landingPage"), conv),
                    devices = Rows(t[6], x => x.D("deviceCategory"), conv, DeviceName),
                    cities = Rows(t[7], x => x.D("city"), conv)
                };
            });
        }

        // =================================================================================================
        // Khán giả
        // =================================================================================================

        public object Audience(string range, DateTime? from, DateTime? to)
        {
            DemandGa();
            MkRange r = MakeRange(range, from, to);
            return Cached("audience", r, null, () =>
            {
                var g = new Ga4Query(_s);
                string[] std = g.Std, conv = g.Conv;
                List<GaTable> t = g.Batch(
                    Ga4Query.Req(r, true, new[] { "country" }, std, null, "sessions", 60),            // 0
                    Ga4Query.Req(r, true, new[] { "city" }, std, null, "sessions", 60),               // 1
                    Ga4Query.Req(r, true, new[] { "deviceCategory" }, std, null, "sessions", 20),     // 2
                    Ga4Query.Req(r, false, new[] { "operatingSystem" }, std, null, "sessions", 10),   // 3
                    Ga4Query.Req(r, false, new[] { "browser" }, std, null, "sessions", 10),           // 4
                    Ga4Query.Req(r, false, new[] { "language" }, std, null, "sessions", 10),          // 5
                    Ga4Query.Req(r, true, new[] { "newVsReturning" }, std, null, "sessions", 10),     // 6
                    Ga4Query.Req(r, false, new[] { "userAgeBracket" }, std, null, "", 20),            // 7
                    Ga4Query.Req(r, false, new[] { "userGender" }, std, null, "sessions", 10),        // 8
                    Ga4Query.Req(r, false, new[] { "dayOfWeek", "hour" }, new[] { "sessions" }.Concat(conv).ToArray(), null, null, 200)); // 9
                return (object)new
                {
                    from = r.From, to = r.To, prevFrom = r.PrevFrom, prevTo = r.PrevTo,
                    countries = Rows(t[0], x => x.D("country"), conv).Take(25).ToList(),
                    cities = Rows(t[1], x => x.D("city"), conv).Take(25).ToList(),
                    devices = Rows(t[2], x => x.D("deviceCategory"), conv, DeviceName),
                    os = Rows(t[3], x => x.D("operatingSystem"), conv),
                    browsers = Rows(t[4], x => x.D("browser"), conv),
                    languages = Rows(t[5], x => x.D("language"), conv),
                    userTypes = Rows(t[6], x => x.D("newVsReturning"), conv, k => k == "new" ? "Khách mới" : k == "returning" ? "Khách quay lại" : "Không xác định"),
                    ages = Rows(t[7], x => x.D("userAgeBracket"), conv, k => k == "unknown" ? "Không xác định" : k).OrderBy(x => x.Key == "unknown" ? "zz" : x.Key).ToList(),
                    genders = Rows(t[8], x => x.D("userGender"), conv, k => k == "male" ? "Nam" : k == "female" ? "Nữ" : "Không xác định"),
                    heat = Heat(t[9])
                };
            });
        }
    }
}
