namespace NVCMS.API.ReadGoogleSheet.Models.Config
{
    /// <summary>
    /// FB-SYNC 19/11/2026: cấu hình job tự lấy bài từ Facebook Page về module Tin tức (section "FacebookSync").
    /// </summary>
    public class FacebookSyncSettings
    {
        /// <summary>Phiên bản Graph API, ví dụ "v25.0".</summary>
        public string GraphVersion { get; set; } = "v25.0";

        /// <summary>ID (số) của Facebook Page cần quét.</summary>
        public string PageId { get; set; } = string.Empty;

        /// <summary>Page Access Token dài hạn (quyền pages_read_engagement).</summary>
        public string PageAccessToken { get; set; } = string.Empty;

        /// <summary>App Secret - chỉ cần khi app bật "Require App Secret" (gửi kèm appsecret_proof).</summary>
        public string AppSecret { get; set; } = string.Empty;

        /// <summary>Portal của website nhận bài.</summary>
        public int PortalId { get; set; }

        /// <summary>Chuyên mục Tin tức nhận bài (bắt buộc &gt; 0).</summary>
        public int CategoryId { get; set; }

        /// <summary>UserId (DNN) đứng tên người tạo / người xuất bản bài.</summary>
        public int UserId { get; set; } = 1;

        public string LanguageId { get; set; } = "vi-VN";

        /// <summary>Thể loại bài (ddl "Chọn thể loại" ở màn soạn tin). 0 = TT - Tin tức tổng hợp.</summary>
        public int NewsKind { get; set; }

        /// <summary>Bút danh hiển thị trên bài.</summary>
        public string ButDanh { get; set; } = string.Empty;

        /// <summary>true: xuất bản luôn. false: chỉ tạo bài ở trạng thái "Đang biên soạn".</summary>
        public bool AutoPublish { get; set; } = true;

        /// <summary>Số bài tối đa xử lý mỗi lần chạy.</summary>
        public int MaxPostsPerRun { get; set; } = 25;

        /// <summary>Lần chạy đầu tiên lấy lùi về bao nhiêu ngày.</summary>
        public int FirstRunLookbackDays { get; set; } = 7;

        /// <summary>Bỏ qua bài có nội dung chữ ngắn hơn số ký tự này (ảnh bìa, status ngắn...).</summary>
        public int MinMessageLength { get; set; } = 80;

        /// <summary>Bỏ qua bài Page chia sẻ lại từ nơi khác.</summary>
        public bool SkipSharedPosts { get; set; } = true;

        /// <summary>Bỏ qua bài không có ảnh.</summary>
        public bool SkipPostsWithoutImage { get; set; }

        /// <summary>
        /// Nếu có giá trị: chỉ lấy bài chứa ÍT NHẤT một hashtag trong danh sách (không phân biệt hoa thường,
        /// có hay không có dấu #). Để trống = lấy mọi bài.
        /// </summary>
        public string[] RequiredHashtags { get; set; } = Array.Empty<string>();

        /// <summary>Thêm dòng "Xem bài gốc trên Facebook" ở cuối bài.</summary>
        public bool AppendSourceLink { get; set; } = true;

        /// <summary>Nhúng trình phát video Facebook vào bài khi bài có video.</summary>
        public bool EmbedVideo { get; set; } = true;

        /// <summary>Tiêu đề viết HOA toàn bộ thì đổi thành Viết Hoa Chữ Đầu Mỗi Từ.</summary>
        public bool NormalizeUppercaseTitle { get; set; } = true;

        /// <summary>Từ viết tắt giữ nguyên chữ hoa trong tiêu đề, bổ sung cho danh sách có sẵn (IELTS, SAT, USA...).</summary>
        public string[] KeepUppercaseWords { get; set; } = Array.Empty<string>();

        /// <summary>Số ảnh tối đa tải về cho mỗi bài.</summary>
        public int MaxImagesPerPost { get; set; } = 10;

        /// <summary>
        /// Thư mục vật lý lưu ảnh (gốc, chưa có yyyy\MM\dd). Để trống = đọc PortalSettings "settingAnhLuuTruPhysical".
        /// API phải ghi được vào thư mục này (cùng máy chủ với website hoặc ổ mạng).
        /// </summary>
        public string ImagePhysicalPath { get; set; } = string.Empty;

        /// <summary>Đường dẫn ảo tương ứng. Để trống = đọc PortalSettings "settingAnhLuuTruVirtual".</summary>
        public string ImageVirtualPath { get; set; } = string.Empty;
    }
}
