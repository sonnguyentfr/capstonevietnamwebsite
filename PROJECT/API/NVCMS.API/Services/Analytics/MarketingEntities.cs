using System;
using System.Collections.Generic;

namespace NVCMS.API.Analytics
{
    // =====================================================================================================
    // Phân tích Marketing (GA4) - trang chủ quản trị (skin _admin/controls/index/U_NhuanBut.ascx)
    // =====================================================================================================

    /// <summary>Bộ chỉ số chuẩn của một dòng (cả site, một kênh, một chiến dịch…). Tỷ lệ tính ở server, 0..1.</summary>
    public class MkMetrics
    {
        public long Users { get; set; }
        public long NewUsers { get; set; }
        public long Sessions { get; set; }
        public long EngagedSessions { get; set; }
        public long Views { get; set; }
        /// <summary>Tổng thời gian tương tác (giây).</summary>
        public double EngagementSec { get; set; }
        /// <summary>Số chuyển đổi (key event GA4 hoặc các sự kiện cấu hình ở NVCMS.Analytics.KeyEvents).</summary>
        public double Conversions { get; set; }

        public double EngagementRate => Sessions > 0 ? Math.Round((double)EngagedSessions / Sessions, 4) : 0;
        public double BounceRate => Sessions > 0 ? Math.Round(1 - (double)EngagedSessions / Sessions, 4) : 0;
        /// <summary>Thời gian tương tác trung bình / người dùng (giây).</summary>
        public double AvgEngagementSec => Users > 0 ? Math.Round(EngagementSec / Users, 1) : 0;
        public double ViewsPerSession => Sessions > 0 ? Math.Round((double)Views / Sessions, 2) : 0;
        /// <summary>Chuyển đổi / phiên.</summary>
        public double ConversionRate => Sessions > 0 ? Math.Round(Conversions / Sessions, 4) : 0;
        public double NewUserRate => Users > 0 ? Math.Round((double)NewUsers / Users, 4) : 0;

        internal void Add(MkMetrics o)
        {
            if (o == null) return;
            Users += o.Users; NewUsers += o.NewUsers; Sessions += o.Sessions; EngagedSessions += o.EngagedSessions;
            Views += o.Views; EngagementSec += o.EngagementSec; Conversions += o.Conversions;
        }
    }

    /// <summary>Một dòng của bảng phân tích: chỉ số kỳ này + kỳ trước (cùng độ dài, liền trước).</summary>
    public class MkRow : MkMetrics
    {
        public string Key { get; set; }
        public string Name { get; set; }
        /// <summary>Thông tin phụ (VD: nguồn / phương tiện chính của chiến dịch).</summary>
        public string Sub { get; set; }
        /// <summary>Tỷ trọng phiên (hoặc lượt xem với trang) trong tổng, 0..1.</summary>
        public double Share { get; set; }
        public MkMetrics Prev { get; set; }
    }

    public class MkPoint
    {
        public DateTime Date { get; set; }
        public long Users { get; set; }
        public long Sessions { get; set; }
        public long Views { get; set; }
        public double Conversions { get; set; }
        /// <summary>Ngày tương ứng của kỳ trước (cùng thứ tự ngày).</summary>
        public long PrevSessions { get; set; }
        public long PrevViews { get; set; }
    }

    /// <summary>Trang / bài viết: lượt xem, người đọc, thời gian đọc, phân loại hiệu quả.</summary>
    public class MkContent
    {
        /// <summary>"n{NewId}" khi gộp theo bài, ngược lại là đường dẫn.</summary>
        public string Key { get; set; }
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public int Paths { get; set; }
        public long Views { get; set; }
        public long PrevViews { get; set; }
        public long Users { get; set; }
        public double EngagementSec { get; set; }
        public double Conversions { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string AuthorName { get; set; }
        public DateTime? PublishedDate { get; set; }
        /// <summary>star | potential | optimize | normal | low (ít dữ liệu).</summary>
        public string Segment { get; set; }
        /// <summary>up | down | new | null.</summary>
        public string Trend { get; set; }

        public double AvgEngagementSec => Users > 0 ? Math.Round(EngagementSec / Users, 1) : 0;
        public double ViewsPerUser => Users > 0 ? Math.Round((double)Views / Users, 2) : 0;
        public double? Growth => PrevViews > 0 ? Math.Round((double)(Views - PrevViews) / PrevViews, 4) : (double?)null;
        public double Share { get; set; }

        internal List<string> AllPaths { get; } = new List<string>();
        internal string Host { get; set; }
    }

    /// <summary>Chuyên mục: cầu (lượt xem) so với cung (số bài xuất bản trong kỳ).</summary>
    public class MkCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Views { get; set; }
        public long PrevViews { get; set; }
        public long Users { get; set; }
        public double EngagementSec { get; set; }
        public int Articles { get; set; }
        public int Published { get; set; }
        public double ViewShare { get; set; }
        public double PublishShare { get; set; }
        /// <summary>Tỷ trọng xem / tỷ trọng xuất bản: &gt; 1 cầu vượt cung.</summary>
        public double? DemandIndex { get; set; }
        public double AvgEngagementSec => Users > 0 ? Math.Round(EngagementSec / Users, 1) : 0;
        public double? Growth => PrevViews > 0 ? Math.Round((double)(Views - PrevViews) / PrevViews, 4) : (double?)null;
        public double ViewsPerPublished => Published > 0 ? Math.Round((double)Views / Published, 1) : 0;
        /// <summary>under (cầu &gt; cung) | over (cung &gt; cầu) | balanced.</summary>
        public string Balance { get; set; }
    }

    public class MkSlice
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Share { get; set; }
    }

    /// <summary>Gợi ý hành động cho Marketing.</summary>
    public class MkInsight
    {
        /// <summary>danger | warning | opportunity | success | info.</summary>
        public string Level { get; set; }
        /// <summary>traffic | channel | content | campaign | utm | audience | seo | goal | setup.</summary>
        public string Area { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Action { get; set; }
        /// <summary>Tab liên quan để mở (overview | content | acquisition | campaigns | audience).</summary>
        public string Tab { get; set; }
        /// <summary>Mức ảnh hưởng ước tính để xếp thứ tự trong cùng mức.</summary>
        public double Impact { get; set; }
    }

    public class MkUtmIssue
    {
        /// <summary>case | nonstandard | notset | spaces | unknownSource.</summary>
        public string Kind { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public long Sessions { get; set; }
    }

    public class MkRange
    {
        public string Key { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime PrevFrom { get; set; }
        public DateTime PrevTo { get; set; }
        public int Days => (int)(To - From).TotalDays + 1;
    }
}
