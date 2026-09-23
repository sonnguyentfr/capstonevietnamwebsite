namespace NVCMS.API.ReadGoogleSheet.Models.Config
{
    public class ZaloSettings
    {
        public string TokenEndpoint { get; set; }

        public string ApiSendMessage { get; set; }

        public string TemplateListEndpoint { get; set; }

        public string TemplateDetailEndpoint { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string AccessToken { get; set; }

        // ── Zalo OA Chat ─────────────────────────────────────────────────────

        /// <summary>OA ID. Để trống thì lấy từ API getoa / webhook.</summary>
        public string? OAId { get; set; }

        /// <summary>"OA Secret Key" trong cấu hình Webhook của app (khác AppSecret) - dùng để kiểm tra X-ZEvent-Signature.</summary>
        public string? OASecretKey { get; set; }

        /// <summary>Gốc Zalo OA Open API.</summary>
        public string OpenApiBaseUrl { get; set; } = "https://openapi.zalo.me";

        /// <summary>Mã hoá access/refresh token trong bảng Zalo_Token (ASP.NET Data Protection).</summary>
        public bool EncryptTokens { get; set; } = true;

        /// <summary>Refresh access token khi còn ít hơn N phút là hết hạn (token Zalo sống 25h).</summary>
        public int TokenRefreshMarginMinutes { get; set; } = 30;
    }
}
