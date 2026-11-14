namespace NVCMS.API.ReadGoogleSheet.Models.Config
{
    /// <summary>
    /// Cấu hình cho 2 job đồng bộ CRM (thay cho DNN Scheduler cũ).
    /// Các giá trị mặc định lấy đúng theo hằng số hard-code trong
    /// NVCMS.Modules.Scheduler (PortalId 50, mail no-reply, BCC nội bộ...).
    /// </summary>
    public class CrmSyncSettings
    {
        /// <summary>PortalId của site CRM. Job VB cũ hard-code 50.</summary>
        public int PortalId { get; set; } = 50;

        /// <summary>Địa chỉ gửi mail xác nhận đăng ký.</summary>
        public string FromEmail { get; set; } = "no-reply@capstonevietnam.com";

        public string FromName { get; set; } = "Capstone Vietnam";

        /// <summary>BCC nội bộ, phân tách bằng dấu phẩy hoặc chấm phẩy.</summary>
        public string BccEmails { get; set; } = "it@capstonevietnam.com; marketing.hn@capstonevietnam.com";

        /// <summary>Domain dùng để sinh QR check-in trong mail.</summary>
        public string UrlDomain { get; set; } = "http://crm.capstonevietnam.com";

        /// <summary>
        /// PortalId website (DB CapstoneVietNamWeb) để đọc tên site / logo qua SP
        /// WebView_GetSiteSettings — phải trùng SiteSettings:PortalId của Capstone.View.
        /// </summary>
        public int WebsitePortalId { get; set; } = 0;

        /// <summary>Dự phòng khi không đọc được WebView_GetSiteSettings.</summary>
        public string SiteName { get; set; } = "Capstone Vietnam";

        /// <summary>Dự phòng khi không đọc được WebView_GetSiteSettings.</summary>
        public string SiteUrl { get; set; } = "https://capstonevietnam.com";

        /// <summary>Dự phòng khi không đọc được WebView_GetSiteSettings (rỗng = hiện chữ SiteName).</summary>
        public string LogoUrl { get; set; } = "";

        /// <summary>Nghỉ giữa 2 lần gửi mail (ms) để không vượt rate limit SES.</summary>
        public int SendMailThrottleMs { get; set; } = 1000;

        /// <summary>Số bản ghi ladipage xử lý tối đa mỗi lần chạy (0 = không giới hạn).</summary>
        public int MaxRecordsPerRun { get; set; } = 0;

        /// <summary>Bật/tắt mail cảnh báo khi job lỗi.</summary>
        public bool AlertEnabled { get; set; } = true;

        /// <summary>Nơi nhận mail cảnh báo lỗi job, phân tách bằng dấu phẩy.</summary>
        public string AlertEmails { get; set; } = "it@capstonevietnam.com";

        /// <summary>CC cho mọi mail cảnh báo lỗi job, phân tách bằng dấu phẩy hoặc chấm phẩy.</summary>
        public string AlertCcEmails { get; set; } = "";

        /// <summary>
        /// Người nhận THÊM theo từng job (key = tên job: ImportCrmDataJob, CopyStudentFromLadiJob,
        /// BirthdayMailJob), cộng với AlertEmails. Ví dụ lỗi đọc Google Sheet gửi thêm cho marketing.
        /// Biến môi trường: CrmSync__AlertEmailsByJob__ImportCrmDataJob
        /// </summary>
        public Dictionary<string, string> AlertEmailsByJob { get; set; }
            = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Số lỗi được mô tả chi tiết trong 1 mail cảnh báo. Phần vượt quá chỉ
        /// đếm số lượng, tránh mail quá dài khi lỗi hệ thống làm hỏng mọi bản ghi.
        /// </summary>
        public int MaxAlertDetails { get; set; } = 20;
    }
}
