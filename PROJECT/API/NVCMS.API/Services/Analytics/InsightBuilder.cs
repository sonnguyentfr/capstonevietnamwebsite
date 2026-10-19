using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NVCMS.API.Analytics
{
    /// <summary>
    /// Luật phân tích số liệu GA4 -> việc Marketing cần làm. Mỗi luật chỉ bật khi đủ dữ liệu (ngưỡng tối thiểu) để tránh kết luận từ vài phiên.
    /// Nhóm: traffic (xu hướng + nguyên nhân theo kênh), channel (chất lượng / chuyển đổi từng kênh), seo, utm, audience (thiết bị, khách quay lại, giờ vàng),
    /// content (ngôi sao / tiềm năng / cần tối ưu / đang giảm), campaign, goal (mục tiêu tháng), setup (thiếu cấu hình chuyển đổi).
    /// </summary>
    internal sealed class InsightBuilder
    {
        private readonly MkRange _r;
        private readonly MarketingService.MkOverview _o;
        private readonly MarketingService.ContentData _c;
        private readonly List<MkRow> _camps;
        private readonly List<MkUtmIssue> _utm;
        private readonly AnalyticsSettings _s;
        private readonly List<MkInsight> _out = new List<MkInsight>();

        private static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

        public InsightBuilder(MkRange r, MarketingService.MkOverview o, MarketingService.ContentData c, List<MkRow> campaigns, List<MkUtmIssue> utm, AnalyticsSettings s)
        {
            _r = r; _o = o; _c = c; _camps = campaigns ?? new List<MkRow>(); _utm = utm ?? new List<MkUtmIssue>(); _s = s;
        }

        private static readonly Dictionary<string, int> Order = new Dictionary<string, int> { ["danger"] = 0, ["warning"] = 1, ["opportunity"] = 2, ["success"] = 3, ["info"] = 4 };

        public List<MkInsight> Build()
        {
            MkMetrics s = _o.Summary;
            if (s.Sessions < 30)
            {
                Add("info", "traffic", "overview", 0, "Chưa đủ dữ liệu để phân tích",
                    "Khoảng đang xem chỉ có " + N(s.Sessions) + " phiên.",
                    "Chọn khoảng dài hơn (28 hoặc 90 ngày) để các gợi ý có ý nghĩa thống kê.");
                return _out;
            }
            Traffic();
            Engagement();
            Conversions();
            Channels();
            Audience();
            Content();
            Categories();
            Campaigns();
            Landing();
            PageTypes();
            Utm();
            Goal();
            return _out.OrderBy(x => Order.TryGetValue(x.Level, out int o) ? o : 9).ThenByDescending(x => x.Impact).ToList();
        }

        // ------------------------------------------------------------------ định dạng
        private static string N(double v) => Math.Round(v).ToString("#,0", Vi);
        private static string P(double ratio) => (ratio * 100).ToString(Math.Abs(ratio) < 0.1 ? "0.#" : "0", Vi) + "%";
        private static string Pts(double diff) => (diff >= 0 ? "+" : "−") + Math.Abs(diff * 100).ToString("0.#", Vi) + " điểm";
        private static string Chg(double cur, double prev) => prev > 0 ? ((cur - prev) / prev >= 0 ? "+" : "−") + P(Math.Abs((cur - prev) / prev)) : "mới";
        private static string Sec(double sec) { int x = (int)Math.Round(sec); return x >= 60 ? (x / 60) + " phút " + (x % 60).ToString("00") + " giây" : x + " giây"; }
        private static string Q(string s) => "\"" + (s != null && s.Length > 70 ? s.Substring(0, 67) + "…" : s) + "\"";

        private void Add(string level, string area, string tab, double impact, string title, string detail, string action)
        {
            _out.Add(new MkInsight { Level = level, Area = area, Tab = tab, Impact = Math.Round(impact, 2), Title = title, Detail = detail, Action = action });
        }

        // ------------------------------------------------------------------ traffic: xu hướng + kênh gây ra
        private void Traffic()
        {
            MkMetrics s = _o.Summary, p = _o.Previous;
            if (p == null || p.Sessions < 30) return;
            double d = (double)(s.Sessions - p.Sessions) / p.Sessions;
            var deltas = _o.Channels.Select(c => new { c, delta = c.Sessions - (c.Prev?.Sessions ?? 0) }).ToList();
            if (d <= -0.10)
            {
                var worst = deltas.OrderBy(x => x.delta).FirstOrDefault(x => x.delta < 0);
                string why = worst != null ? " Giảm mạnh nhất ở kênh " + worst.c.Name + ": " + N(-worst.delta) + " phiên (" + Chg(worst.c.Sessions, worst.c.Prev?.Sessions ?? 0) + "), chiếm " +
                                             P(Math.Abs((double)worst.delta) / Math.Max(1, p.Sessions - s.Sessions)) + " mức giảm." : "";
                Add(d <= -0.25 ? "danger" : "warning", "traffic", "acquisition", Math.Abs(d) * 100,
                    "Lượng truy cập giảm " + P(-d) + " so với kỳ trước",
                    N(s.Sessions) + " phiên so với " + N(p.Sessions) + " phiên kỳ trước." + why,
                    worst == null ? "Kiểm tra thẻ GA4 trên các trang mới / thay đổi giao diện, lịch đăng bài và ngân sách quảng cáo."
                    : ChannelFix(worst.c.Key));
            }
            else if (d >= 0.10)
            {
                var best = deltas.OrderByDescending(x => x.delta).FirstOrDefault(x => x.delta > 0);
                Add("success", "traffic", "acquisition", d * 100,
                    "Lượng truy cập tăng " + P(d) + " so với kỳ trước",
                    N(s.Sessions) + " phiên (kỳ trước " + N(p.Sessions) + ")." + (best != null ? " Đóng góp lớn nhất: " + best.c.Name + " +" + N(best.delta) + " phiên." : ""),
                    best != null ? "Tìm nội dung / chiến dịch đã kéo tăng kênh " + best.c.Name + " và nhân rộng (thêm bài cùng chủ đề, tăng tần suất đăng, phân bổ thêm ngân sách)."
                                 : "Duy trì nhịp nội dung hiện tại.");
            }
        }

        private static string ChannelFix(string channel)
        {
            switch (channel)
            {
                case "Organic Search": return "SEO: kiểm tra Google Search Console (thứ hạng, lỗi index, cập nhật thuật toán), tối ưu lại title / meta các bài top cũ, sitemap.";
                case "Organic Social": return "Mạng xã hội: tăng tần suất đăng, dùng định dạng video ngắn / carousel, khung giờ vàng, gắn link có UTM.";
                case "Paid Social": case "Paid Search": case "Display": case "Cross-network": case "Paid Video": case "Paid Other":
                    return "Quảng cáo: kiểm tra ngân sách, trạng thái duyệt mẫu quảng cáo, giá thầu và tần suất hiển thị (ad fatigue).";
                case "Email": return "Email: kiểm tra lịch gửi, tỷ lệ mở / click, danh sách bị spam; gắn UTM cho mọi liên kết trong email.";
                case "Referral": return "Website giới thiệu: kiểm tra các backlink / đối tác lớn còn trỏ về site không, liên hệ đối tác đăng lại.";
                case "Direct": return "Direct giảm thường do bớt chia sẻ qua Zalo / tin nhắn hoặc truy cập lặp lại: đẩy newsletter, thông báo đẩy, bookmark.";
                default: return "Rà soát kênh này: lịch đăng, ngân sách, liên kết hỏng, thẻ GA4 trên các trang mới.";
            }
        }

        // ------------------------------------------------------------------ chất lượng truy cập
        private void Engagement()
        {
            MkMetrics s = _o.Summary, p = _o.Previous;
            if (p != null && p.Sessions >= 100)
            {
                double diff = s.EngagementRate - p.EngagementRate;
                if (diff <= -0.05)
                    Add("warning", "traffic", "overview", Math.Abs(diff) * 100,
                        "Tỷ lệ tương tác giảm " + Math.Abs(diff * 100).ToString("0.#", Vi) + " điểm",
                        "Tỷ lệ phiên có tương tác " + P(s.EngagementRate) + " (kỳ trước " + P(p.EngagementRate) + "); thời gian tương tác TB " + Sec(s.AvgEngagementSec) + ".",
                        "Xem tab Nguồn: kênh nào mang về phiên chất lượng thấp (quảng cáo sai tệp, click nhầm). Trang đích cần tải nhanh, nội dung khớp quảng cáo, có CTA rõ.");
                else if (diff >= 0.05)
                    Add("success", "traffic", "overview", diff * 100, "Chất lượng truy cập cải thiện",
                        "Tỷ lệ tương tác " + P(s.EngagementRate) + " (" + Pts(diff) + "); thời gian tương tác TB " + Sec(s.AvgEngagementSec) + ".",
                        "Ghi nhận thay đổi gần đây (nội dung, giao diện, nguồn quảng cáo) để giữ hướng đi.");
            }
        }

        private void Conversions()
        {
            MkMetrics s = _o.Summary, p = _o.Previous;
            if (!_o.HasConversions)
            {
                Add("warning", "setup", "overview", 50, "Chưa đo được chuyển đổi",
                    "GA4 không ghi nhận key event nào trong kỳ" + (_s.KeyEvents.Count > 0 ? " với sự kiện: " + string.Join(", ", _s.KeyEvents) : "") + ". Không thể biết kênh / bài / chiến dịch nào mang lại khách hàng." +
                    (_o.ThankYouViews > 0 ? " Trong khi đó trang \"Đăng ký tư vấn thành công\" có " + N(_o.ThankYouViews) + " lượt xem - đây chính là lead đang bị bỏ sót." : ""),
                    (_o.ThankYouViews > 0 ? "Tạo sự kiện generate_lead khi xem /dang-ky-tu-van/thanh-cong (GA4 › Admin › Events › Create event) và đánh dấu Key event. " : "") +
                    "Đánh dấu Key event trong GA4 (Admin › Events): gửi form liên hệ / đăng ký tư vấn, click hotline, click Zalo / Messenger, tải tài liệu. Có thể khai báo tên sự kiện ở Cấu hình website › Google Analytics.");
                return;
            }
            if (p != null && p.Conversions >= 10)
            {
                double d = (s.Conversions - p.Conversions) / p.Conversions;
                if (d <= -0.2)
                    Add(d <= -0.4 ? "danger" : "warning", "traffic", "acquisition", Math.Abs(d) * 120, "Chuyển đổi giảm " + P(-d),
                        N(s.Conversions) + " chuyển đổi (kỳ trước " + N(p.Conversions) + "); tỷ lệ chuyển đổi / phiên " + P(s.ConversionRate) + " (kỳ trước " + P(p.ConversionRate) + ").",
                        "Kiểm tra form / nút liên hệ còn hoạt động, tốc độ trang đích; so sánh tỷ lệ chuyển đổi theo kênh để tìm nơi rơi rớt.");
                else if (d >= 0.2)
                    Add("success", "traffic", "acquisition", d * 100, "Chuyển đổi tăng " + P(d),
                        N(s.Conversions) + " chuyển đổi (kỳ trước " + N(p.Conversions) + ").",
                        "Xác định kênh / chiến dịch đóng góp chính (tab Nguồn & UTM) và tăng ngân sách cho chúng.");
            }
        }

        // ------------------------------------------------------------------ kênh
        private void Channels()
        {
            MkMetrics s = _o.Summary;
            double er = s.EngagementRate, cr = s.ConversionRate;
            foreach (MkRow c in _o.Channels.Where(x => x.Share >= 0.05 && x.Sessions >= 50))
            {
                if (c.EngagementRate < er - 0.15)
                    Add("warning", "channel", "acquisition", c.Share * 100,
                        "Kênh " + c.Name + " mang về truy cập chất lượng thấp",
                        c.Name + " chiếm " + P(c.Share) + " phiên nhưng tỷ lệ tương tác chỉ " + P(c.EngagementRate) + " (trung bình site " + P(er) + ").",
                        c.Key.StartsWith("Paid", StringComparison.Ordinal) || c.Key == "Display" || c.Key == "Cross-network"
                            ? "Thu hẹp tệp quảng cáo, loại vị trí hiển thị kém (app, game), kiểm tra nội dung trang đích có khớp mẫu quảng cáo."
                            : "Rà soát nguồn trong kênh (tab Nguồn & UTM) để tìm nguồn kéo chất lượng xuống; cải thiện trang đích.");
                if (_o.HasConversions && c.Conversions >= 5 && c.ConversionRate >= Math.Max(cr * 1.5, 0.005) && c.Share < 0.35)
                    Add("opportunity", "channel", "acquisition", c.ConversionRate / Math.Max(cr, 0.0001) * 10,
                        "Kênh " + c.Name + " chuyển đổi tốt - nên tăng đầu tư",
                        "Tỷ lệ chuyển đổi " + P(c.ConversionRate) + " (gấp " + (c.ConversionRate / Math.Max(cr, 0.0001)).ToString("0.#", Vi) + " lần trung bình) nhưng mới chiếm " + P(c.Share) + " phiên.",
                        "Tăng ngân sách / tần suất cho kênh này, nhân bản nội dung và tệp đối tượng đang hiệu quả.");
            }
            MkRow organic = _o.Channels.FirstOrDefault(x => x.Key == "Organic Search");
            double orgShare = organic?.Share ?? 0;
            if (s.Sessions >= 300 && orgShare < 0.2)
                Add("opportunity", "seo", "content", (0.2 - orgShare) * 100,
                    "Tìm kiếm tự nhiên mới chiếm " + P(orgShare) + " truy cập",
                    "Kênh SEO bền vững và miễn phí nhưng đang đóng góp thấp" + (organic != null && organic.Prev != null ? " (" + Chg(organic.Sessions, organic.Prev.Sessions) + " so với kỳ trước)." : "."),
                    "Lập kế hoạch từ khóa theo chủ đề có cầu cao (tab Nội dung › chuyên mục cầu > cung), tối ưu title / meta description / heading, liên kết nội bộ, sitemap & Search Console.");
            MkRow direct = _o.Channels.FirstOrDefault(x => x.Key == "Direct");
            if (direct != null && direct.Share >= 0.45 && s.Sessions >= 300)
                Add("info", "utm", "acquisition", direct.Share * 50,
                    "Truy cập trực tiếp cao bất thường (" + P(direct.Share) + ")",
                    "Một phần lớn có thể là \"dark social\": link chia sẻ qua Zalo, Messenger, email, tin nhắn không gắn UTM nên GA4 không biết nguồn.",
                    "Gắn UTM cho mọi link phát hành (bài Zalo OA, email, SMS, QR code, bio mạng xã hội) bằng công cụ tạo UTM ở tab Nguồn & UTM.");
            MkRow un = _o.Channels.FirstOrDefault(x => x.Key == "Unassigned");
            if (un != null && un.Share >= 0.03)
                Add("warning", "utm", "acquisition", un.Share * 100,
                    P(un.Share) + " phiên không phân loại được kênh (Unassigned)",
                    N(un.Sessions) + " phiên có utm_source / utm_medium không theo quy ước GA4.",
                    "Chuẩn hóa utm_medium (cpc, paid-social, social, email, display, referral, sms, qr). Xem danh sách giá trị sai ở tab Nguồn & UTM.");
            MkRow social = _o.Channels.FirstOrDefault(x => x.Key == "Organic Social");
            if (social != null && social.Sessions >= 50 && social.Share < 0.15 && social.EngagementRate >= er)
                Add("opportunity", "channel", "acquisition", 20,
                    "Mạng xã hội tương tác tốt nhưng mới chiếm " + P(social.Share) + " truy cập",
                    "Tỷ lệ tương tác từ mạng xã hội " + P(social.EngagementRate) + " (trung bình " + P(er) + ").",
                    "Tăng tần suất đăng Fanpage / Zalo OA / TikTok, đăng lại bài ngôi sao, chạy boost nhỏ cho bài có tỷ lệ đọc cao.");
        }

        // ------------------------------------------------------------------ khán giả
        private void Audience()
        {
            MkMetrics s = _o.Summary;
            MkRow mobile = _o.Devices.FirstOrDefault(x => x.Key == "mobile"), desktop = _o.Devices.FirstOrDefault(x => x.Key == "desktop");
            if (mobile != null && desktop != null && mobile.Share >= 0.5 && desktop.Sessions >= 30 && mobile.EngagementRate < desktop.EngagementRate - 0.1)
                Add("warning", "audience", "audience", mobile.Share * 60,
                    "Trải nghiệm trên điện thoại kém hơn máy tính",
                    P(mobile.Share) + " phiên đến từ điện thoại nhưng tỷ lệ tương tác chỉ " + P(mobile.EngagementRate) + " so với " + P(desktop.EngagementRate) + " trên máy tính.",
                    "Kiểm tra tốc độ tải (PageSpeed Insights) và bố cục mobile: ảnh nặng, popup che nội dung, chữ nhỏ, nút CTA khó bấm.");
            MkRow ret = _o.UserTypes.FirstOrDefault(x => x.Key == "returning");
            long users = _o.UserTypes.Sum(x => x.Users);
            double retShare = users > 0 && ret != null ? (double)ret.Users / users : 0;
            if (users >= 200 && retShare < 0.2)
                Add("opportunity", "audience", "audience", (0.2 - retShare) * 100,
                    "Ít khách quay lại (" + P(retShare) + " người dùng)",
                    "Phần lớn người đọc chỉ ghé một lần - chi phí thu hút mới cao, khó nuôi dưỡng thành khách hàng.",
                    "Xây kênh sở hữu: đăng ký nhận tin (email / Zalo OA), thông báo đẩy, remarketing tệp đã đọc bài; thêm \"bài liên quan\" và chuỗi nội dung nhiều kỳ.");
            if (ret != null && ret.Sessions >= 50)
            {
                MkRow nw = _o.UserTypes.FirstOrDefault(x => x.Key == "new");
                if (nw != null && nw.Sessions >= 50 && ret.ConversionRate >= nw.ConversionRate * 2 && ret.Conversions >= 5)
                    Add("info", "audience", "audience", 10, "Khách quay lại chuyển đổi gấp " + (ret.ConversionRate / Math.Max(nw.ConversionRate, 0.0001)).ToString("0.#", Vi) + " lần khách mới",
                        "Tỷ lệ chuyển đổi khách quay lại " + P(ret.ConversionRate) + ", khách mới " + P(nw.ConversionRate) + ".",
                        "Ưu tiên ngân sách remarketing và nội dung nuôi dưỡng (newsletter, chuỗi bài) hơn là chỉ kéo traffic mới.");
            }
            // giờ vàng: ô (thứ, giờ) đông nhất + khung 3 giờ liên tiếp đông nhất trong tuần
            if (_o.Heat.Count == 7 && s.Sessions >= 200)
            {
                long[] byHour = new long[24];
                long best = -1; int bd = 0, bh = 0;
                for (int d = 0; d < 7; d++)
                    for (int h = 0; h < 24; h++)
                    {
                        byHour[h] += _o.Heat[d][h];
                        if (_o.Heat[d][h] > best) { best = _o.Heat[d][h]; bd = d; bh = h; }
                    }
                int wh = 0; long wv = -1;
                for (int h = 0; h < 22; h++) { long v = byHour[h] + byHour[h + 1] + byHour[h + 2]; if (v > wv) { wv = v; wh = h; } }
                string[] days = { "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy", "Chủ nhật" };
                long total = byHour.Sum();
                Add("info", "audience", "audience", 5, "Khung giờ vàng: " + wh + "h–" + (wh + 3) + "h",
                    "Khung " + wh + "h–" + (wh + 3) + "h chiếm " + P(total > 0 ? (double)wv / total : 0) + " phiên trong ngày; đông nhất vào " + days[bd] + " lúc " + bh + "h.",
                    "Lên lịch đăng bài / gửi email / đẩy thông báo trước khung này 30–60 phút; tập trung ngân sách quảng cáo vào khung giờ vàng.");
            }
        }

        // ------------------------------------------------------------------ nội dung
        private void Content()
        {
            List<MkContent> arts = _c.ByArticle.Where(x => x.NewsId > 0).ToList();
            if (arts.Count < 5) arts = _c.ByArticle;
            List<MkContent> stars = arts.Where(x => x.Segment == "star").OrderByDescending(x => x.Views).ToList();
            if (stars.Count > 0)
                Add("success", "content", "content", stars.Take(5).Sum(x => x.Share) * 100,
                    stars.Count + " bài \"ngôi sao\" (xem nhiều + đọc sâu)",
                    "Nổi bật: " + string.Join("; ", stars.Take(3).Select(x => Q(x.Title) + " " + N(x.Views) + " lượt xem, đọc TB " + Sec(x.AvgEngagementSec))) + ".",
                    "Ghim trang chủ / đầu chuyên mục, chia sẻ lại trên mạng xã hội, viết bài tiếp nối cùng chủ đề, gắn CTA (đăng ký tư vấn / nhận tin) vào các bài này.");
            List<MkContent> potential = arts.Where(x => x.Segment == "potential").OrderByDescending(x => x.AvgEngagementSec).ToList();
            if (potential.Count > 0)
                Add("opportunity", "content", "content", potential.Count,
                    potential.Count + " bài đọc rất sâu nhưng ít người xem",
                    "Người đã vào đọc rất lâu (≥ " + Sec(_c.E75) + ") nhưng lượt xem thấp: " + string.Join("; ", potential.Take(3).Select(x => Q(x.Title))) + ".",
                    "Đẩy phân phối: chia sẻ Fanpage / Zalo OA / email, liên kết nội bộ từ bài top, tối ưu tiêu đề & ảnh đại diện cho hấp dẫn hơn, chạy boost nhỏ.");
            List<MkContent> optimize = arts.Where(x => x.Segment == "optimize").OrderByDescending(x => x.Views).ToList();
            if (optimize.Count > 0)
                Add("warning", "content", "content", optimize.Take(5).Sum(x => x.Share) * 100,
                    optimize.Count + " bài nhiều lượt xem nhưng người đọc rời nhanh",
                    "Đọc TB dưới " + Sec(_c.E25) + ": " + string.Join("; ", optimize.Take(3).Select(x => Q(x.Title) + " " + N(x.Views) + " lượt xem")) + ".",
                    "Tối ưu: tiêu đề khớp nội dung (tránh giật tít), sapo trả lời ngay câu hỏi, chia đoạn + ảnh minh họa, thêm bài liên quan & CTA cuối bài.");
            List<MkContent> down = arts.Where(x => x.Trend == "down").OrderByDescending(x => x.PrevViews - x.Views).ToList();
            if (down.Count > 0)
                Add("warning", "content", "content", down.Take(5).Sum(x => x.PrevViews - x.Views) / Math.Max(1.0, _c.PrevTotalViews) * 100,
                    down.Count + " bài từng hút khách đang giảm mạnh",
                    string.Join("; ", down.Take(3).Select(x => Q(x.Title) + " " + N(x.PrevViews) + " → " + N(x.Views))) + ".",
                    "Làm mới nội dung (cập nhật số liệu, năm, hình ảnh), kiểm tra thứ hạng từ khóa, chia sẻ lại; với bài theo mùa - lên lịch đẩy lại trước mùa.");
            List<MkContent> up = arts.Where(x => x.Trend == "up").OrderByDescending(x => x.Views - x.PrevViews).ToList();
            if (up.Count > 0)
                Add("opportunity", "content", "content", up.Take(5).Sum(x => x.Views - x.PrevViews) / Math.Max(1.0, _c.TotalViews) * 100,
                    up.Count + " bài đang lên (lượt xem tăng gấp đôi)",
                    string.Join("; ", up.Take(3).Select(x => Q(x.Title) + " " + N(x.PrevViews) + " → " + N(x.Views))) + ".",
                    "Tận dụng xu hướng: viết thêm bài góc nhìn khác cùng chủ đề, cập nhật bài, gắn liên kết sang bài chuyển đổi.");
            if (_o.HasConversions)
            {
                List<MkContent> conv = arts.Where(x => x.Conversions >= 3).OrderByDescending(x => x.Conversions).ToList();
                if (conv.Count > 0)
                    Add("success", "content", "content", conv.Take(3).Sum(x => x.Conversions),
                        "Bài mang về chuyển đổi nhiều nhất",
                        string.Join("; ", conv.Take(3).Select(x => Q(x.Title) + " " + N(x.Conversions) + " chuyển đổi")) + ".",
                        "Dùng làm trang đích cho quảng cáo, ghim trang chủ, viết thêm bài cùng ý định tìm kiếm.");
            }
        }

        private void Categories()
        {
            List<MkCategory> cats = _o.Categories.Where(x => x.Name != null).ToList();
            if (cats.Sum(x => x.Published) < 5) return;
            foreach (MkCategory c in cats.Where(x => x.Balance == "under").OrderByDescending(x => x.ViewShare).Take(3))
                Add("opportunity", "content", "content", c.ViewShare * 100,
                    "Chuyên mục " + Q(c.Name) + ": cầu lớn hơn cung",
                    "Chiếm " + P(c.ViewShare) + " lượt xem nhưng chỉ " + P(c.PublishShare) + " số bài xuất bản trong kỳ (" + c.Published + " bài).",
                    "Tăng sản lượng bài cho chuyên mục này, ưu tiên các chủ đề đang có bài ngôi sao.");
            foreach (MkCategory c in cats.Where(x => x.Balance == "over").OrderByDescending(x => x.PublishShare).Take(2))
                Add("warning", "content", "content", c.PublishShare * 60,
                    "Chuyên mục " + Q(c.Name) + ": sản xuất nhiều, ít người đọc",
                    c.Published + " bài (" + P(c.PublishShare) + " sản lượng) nhưng chỉ " + P(c.ViewShare) + " lượt xem.",
                    "Giảm số lượng, tập trung chất lượng; đổi góc tiếp cận / định dạng (infographic, video ngắn) hoặc dồn nguồn lực sang chuyên mục cầu cao.");
            MkCategory grow = cats.Where(x => x.PrevViews >= 100 && x.Growth >= 0.3).OrderByDescending(x => x.Views - x.PrevViews).FirstOrDefault();
            if (grow != null)
                Add("info", "content", "content", 5, "Chuyên mục tăng trưởng mạnh: " + grow.Name,
                    "Lượt xem " + N(grow.Views) + " (" + Chg(grow.Views, grow.PrevViews) + " so với kỳ trước).",
                    "Đây là chủ đề đang được quan tâm - đưa vào kế hoạch nội dung và quảng cáo kỳ tới.");
        }

        // ------------------------------------------------------------------ chiến dịch
        private void Campaigns()
        {
            MkMetrics s = _o.Summary;
            List<MkRow> camps = _camps.Where(x => x.Sessions >= 30).ToList();
            if (camps.Count == 0)
            {
                if (s.Sessions >= 500 && _camps.Sum(x => x.Sessions) < s.Sessions * 0.02)
                    Add("info", "campaign", "campaigns", 5, "Hầu như không có truy cập gắn chiến dịch (utm_campaign)",
                        "Không thể so sánh hiệu quả từng đợt truyền thông / quảng cáo.",
                        "Đặt utm_campaign cho mọi đợt (VD: tuyen-sinh-thang10, hoc-bong-my-2026) bằng công cụ tạo UTM ở tab Nguồn & UTM.");
                return;
            }
            if (_o.HasConversions)
            {
                foreach (MkRow c in camps.Where(x => x.Conversions == 0).OrderByDescending(x => x.Sessions).Take(3))
                    Add("warning", "campaign", "campaigns", c.Sessions / (double)Math.Max(1, s.Sessions) * 100,
                        "Chiến dịch " + Q(c.Name) + " có traffic nhưng 0 chuyển đổi",
                        N(c.Sessions) + " phiên, tỷ lệ tương tác " + P(c.EngagementRate) + ".",
                        "Kiểm tra trang đích (CTA, form), sự phù hợp giữa thông điệp quảng cáo và nội dung; cân nhắc dừng / chuyển ngân sách nếu kéo dài.");
                MkRow best = camps.Where(x => x.Conversions >= 3).OrderByDescending(x => x.ConversionRate).FirstOrDefault();
                if (best != null && best.ConversionRate > s.ConversionRate)
                    Add("success", "campaign", "campaigns", best.ConversionRate * 100,
                        "Chiến dịch hiệu quả nhất: " + best.Name,
                        "Tỷ lệ chuyển đổi " + P(best.ConversionRate) + " (" + N(best.Conversions) + " chuyển đổi / " + N(best.Sessions) + " phiên), trung bình site " + P(s.ConversionRate) + ".",
                        "Tăng ngân sách, nhân bản thông điệp / mẫu quảng cáo / tệp đối tượng sang các chiến dịch khác.");
            }
            foreach (MkRow c in camps.Where(x => x.EngagementRate < s.EngagementRate - 0.2 && x.Sessions >= 50).OrderByDescending(x => x.Sessions).Take(2))
                Add("warning", "campaign", "campaigns", c.Sessions / (double)Math.Max(1, s.Sessions) * 80,
                    "Chiến dịch " + Q(c.Name) + " mang về truy cập kém chất lượng",
                    "Tỷ lệ tương tác " + P(c.EngagementRate) + " (trung bình site " + P(s.EngagementRate) + "), " + N(c.Sessions) + " phiên.",
                    "Xem chi tiết chiến dịch theo utm_content để loại mẫu quảng cáo / vị trí kém; thu hẹp tệp đối tượng.");
            MkRow fall = camps.Where(x => x.Prev != null && x.Prev.Sessions >= 100 && x.Sessions < x.Prev.Sessions * 0.5).OrderByDescending(x => x.Prev.Sessions - x.Sessions).FirstOrDefault();
            if (fall != null)
                Add("info", "campaign", "campaigns", 5, "Chiến dịch " + Q(fall.Name) + " giảm mạnh",
                    N(fall.Prev.Sessions) + " → " + N(fall.Sessions) + " phiên so với kỳ trước.",
                    "Nếu chiến dịch còn chạy: kiểm tra ngân sách / mẫu quảng cáo hết hạn; nếu đã kết thúc - lên kế hoạch đợt kế tiếp.");
        }

        private void Landing()
        {
            MkMetrics s = _o.Summary;
            foreach (MkRow l in _o.Landing.Where(x => x.Share >= 0.02 && x.Sessions >= 50 && x.EngagementRate < s.EngagementRate - 0.2).Take(2))
                Add("warning", "content", "overview", l.Share * 80,
                    "Trang đích " + Q(l.Name) + " giữ chân kém",
                    P(l.Share) + " phiên bắt đầu ở trang này nhưng tỷ lệ tương tác chỉ " + P(l.EngagementRate) + " (trung bình " + P(s.EngagementRate) + ").",
                    "Cải thiện màn hình đầu tiên: thông điệp rõ, tải nhanh, CTA nổi bật, liên kết sang nội dung liên quan.");
        }

        /// <summary>Loại trang (theo trang vào đầu tiên): loại nào chuyển đổi tốt nhất / giữ chân kém.</summary>
        private void PageTypes()
        {
            MkMetrics s = _o.Summary;
            List<MkRow> types = _o.PageTypes.Where(x => x.Sessions >= 50 && x.Key != "lead" && x.Key != "other").ToList();
            if (types.Count < 2) return;
            if (_o.HasConversions)
            {
                MkRow best = types.Where(x => x.Conversions >= 3).OrderByDescending(x => x.ConversionRate).FirstOrDefault();
                if (best != null && best.ConversionRate > s.ConversionRate * 1.3)
                    Add("opportunity", "content", "content", best.ConversionRate * 100,
                        "Khách vào từ \"" + best.Name + "\" chuyển đổi tốt nhất",
                        "Tỷ lệ chuyển đổi " + P(best.ConversionRate) + " (trung bình " + P(s.ConversionRate) + "), chiếm " + P(best.Share) + " phiên.",
                        "Dùng loại trang này làm trang đích cho quảng cáo / email, tăng liên kết nội bộ dẫn về và đặt form đăng ký tư vấn ngay trong trang.");
            }
            MkRow weak = types.Where(x => x.Share >= 0.05 && x.EngagementRate < s.EngagementRate - 0.15).OrderByDescending(x => x.Sessions).FirstOrDefault();
            if (weak != null)
                Add("warning", "content", "overview", weak.Share * 60,
                    "Trang \"" + weak.Name + "\" giữ chân kém",
                    P(weak.Share) + " phiên bắt đầu ở loại trang này nhưng tỷ lệ tương tác chỉ " + P(weak.EngagementRate) + " (trung bình " + P(s.EngagementRate) + ").",
                    "Rà soát nội dung màn hình đầu, tốc độ tải và lời kêu gọi (CTA) của nhóm trang này; thêm liên kết sang bài / trường / sự kiện liên quan.");
        }

        private void Utm()
        {
            foreach (MkUtmIssue u in _utm.Where(x => x.Sessions >= 10).Take(4))
                Add(u.Kind == "nonstandard" || u.Kind == "case" ? "warning" : "info", "utm", "acquisition", u.Sessions / (double)Math.Max(1, _o.Summary.Sessions) * 100,
                    u.Kind == "case" ? u.Field + " ghi không thống nhất" : u.Kind == "nonstandard" ? "utm_medium sai quy ước" : u.Kind == "notset" ? "Link gắn UTM thiếu utm_campaign" : u.Field + " có khoảng trắng",
                    u.Message + " Giá trị: " + string.Join(", ", u.Values.Take(5)) + " (" + N(u.Sessions) + " phiên).",
                    "Ban hành quy ước UTM chung (chữ thường, gạch ngang, medium chuẩn) và dùng công cụ tạo UTM ở tab Nguồn & UTM.");
        }

        private void Goal()
        {
            if (_s.MonthlyTarget <= 0) return;
            MkMetrics s = _o.Summary;
            double perDay = _s.MonthlyTarget / 30.4375;
            double pace;
            string text;
            if (_r.Key == "month")
            {
                int dim = DateTime.DaysInMonth(_r.From.Year, _r.From.Month), elapsed = Math.Min(dim, Math.Max(1, (int)(DateTime.Now - _r.From).TotalDays));
                double projected = (double)s.Sessions / elapsed * dim;
                pace = projected / _s.MonthlyTarget;
                text = "Dự kiến cả tháng đạt " + N(projected) + " / " + N(_s.MonthlyTarget) + " phiên (" + P(pace) + " mục tiêu).";
            }
            else
            {
                double expected = perDay * _r.Days;
                pace = expected > 0 ? s.Sessions / expected : 0;
                text = N(s.Sessions) + " phiên so với mức cần có " + N(expected) + " (quy từ mục tiêu " + N(_s.MonthlyTarget) + " phiên / tháng).";
            }
            if (pace < 0.9)
                Add(pace < 0.7 ? "danger" : "warning", "goal", "overview", (1 - pace) * 100, "Đang chậm so với mục tiêu traffic (" + P(pace) + ")", text,
                    "Bù thiếu hụt: tăng tần suất đăng bài vào giờ vàng, đẩy lại bài ngôi sao, tăng ngân sách kênh có chi phí / phiên thấp nhất.");
            else
                Add("success", "goal", "overview", pace * 10, "Đúng / vượt tiến độ mục tiêu traffic (" + P(pace) + ")", text,
                    "Cân nhắc nâng mục tiêu hoặc chuyển trọng tâm sang tối ưu chuyển đổi.");
        }
    }
}
