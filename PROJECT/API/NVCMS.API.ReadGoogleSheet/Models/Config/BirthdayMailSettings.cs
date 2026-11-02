namespace NVCMS.API.ReadGoogleSheet.Models.Config
{
    /// <summary>
    /// BIRTHDAY-MAIL 02/11/2026: cấu hình kỹ thuật cho job "birthday-mail" (appsettings mục "BirthdayMail").
    /// Phần nghiệp vụ (bật/tắt, giờ gửi, mẫu thư, tin/sự kiện) do Marketing chỉnh trong CRM -> bảng NVCore_BirthdayMail_Config.
    /// Lịch tick của job: HangfireJobs:BirthdayMail (Cron mặc định 10 phút/lần).
    /// </summary>
    public class BirthdayMailSettings
    {
        /// <summary>
        /// Domain công khai của API này để dựng link mở / click (vd https://api-data.capstonevietnam.com).
        /// Trống = ApiSelfBaseUrl. PHẢI là địa chỉ khách truy cập được từ Internet, không phải localhost.
        /// </summary>
        public string TrackingBaseUrl { get; set; } = string.Empty;

        /// <summary>Website Capstone: [WebsiteUrl], và là trang chuyển tới khi link click không hợp lệ.</summary>
        public string WebsiteUrl { get; set; } = "https://capstonevietnam.com";

        /// <summary>Logo đầu thư: [LogoUrl].</summary>
        public string LogoUrl { get; set; } = "https://crm.capstonevietnam.com/Portals/_default/Skins/Public/images/logo.png";

        /// <summary>Thiệp mặc định (đường dẫn tính từ thư mục chạy API).</summary>
        public string DefaultTemplatePath { get; set; } = "EmailTemplates/birthday-default.html";

        /// <summary>Tiêu đề khi cả mẫu lẫn cấu hình CRM đều để trống. Có thể dùng [FirstName], [Fullname].</summary>
        public string DefaultSubject { get; set; } = "Chúc mừng sinh nhật [FirstName]! 🎂 Capstone Vietnam gửi lời chúc tới bạn";

        /// <summary>Email gửi đi khi CRM để trống (trống nữa = SesSettings.FromEmail). Phải là địa chỉ đã xác minh trên SES.</summary>
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>Link hủy nhận thư: {unsubmail} = email đã mã hóa (UltilHelper.Encrypt), giống mail marketing hiện có.</summary>
        public string UnsubscribeUrl { get; set; } = "https://capstonevietnam.com/mail-unsubscribe?email={unsubmail}";

        /// <summary>Nghỉ giữa 2 thư (ms) để không vượt giới hạn gửi của SES.</summary>
        public int SendThrottleMs { get; set; } = 300;

        /// <summary>Lượt đang "Running" mà quá N phút không cập nhật (server tắt giữa chừng) thì tick sau được gửi tiếp.</summary>
        public int StaleMinutes { get; set; } = 20;

        /// <summary>Không gửi sau giờ này trong ngày (HH:mm, giờ VN) — tránh thư sinh nhật đến lúc nửa đêm nếu server chết cả ngày. Trống = không chặn.</summary>
        public string LatestSendTime { get; set; } = "21:00";

        /// <summary>Gửi lỗi tạm thời (mạng / SMTP) thì thử lại 1 lần sau N ms.</summary>
        public int RetryDelayMs { get; set; } = 2000;
    }
}
