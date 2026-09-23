namespace NVCMS.API.ReadGoogleSheet.Models.Config
{
    /// <summary>Section "ZaloOAChat" trong appsettings.json. Mọi giá trị đều có mặc định.</summary>
    public class ZaloOAChatSettings
    {
        /// <summary>Tắt chỉ để debug local. Production luôn phải bật.</summary>
        public bool VerifyWebhookSignature { get; set; } = true;

        /// <summary>Gọi lại user/detail nếu hồ sơ khách cũ hơn N giờ.</summary>
        public int ProfileRefreshHours { get; set; } = 24;

        /// <summary>Số lần xử lý lại tối đa cho 1 webhook event lỗi.</summary>
        public int WebhookMaxRetry { get; set; } = 5;

        /// <summary>Event PENDING/FAILED không đổi trong N phút thì job định kỳ xử lý lại.</summary>
        public int WebhookReprocessStaleMinutes { get; set; } = 10;

        /// <summary>Số hội thoại tối đa quét khi nhập lịch sử (listrecentchat, 10/lần).</summary>
        public int HistoryImportMaxConversations { get; set; } = 500;

        /// <summary>Số tin tối đa mỗi khách khi nhập lịch sử (conversation, 10/lần).</summary>
        public int HistoryImportMaxMessagesPerUser { get; set; } = 200;

        /// <summary>Nghỉ giữa các request khi nhập lịch sử (giới hạn Zalo 4000 req/phút).</summary>
        public int HistoryImportDelayMs { get; set; } = 150;
    }
}
