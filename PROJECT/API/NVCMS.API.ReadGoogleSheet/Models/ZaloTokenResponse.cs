namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class ZaloTokenResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }

        // Zalo trả HTTP 200 kèm các field dưới khi lỗi (vd refresh token hết hạn)
        public int? error { get; set; }
        public string? error_name { get; set; }
        public string? error_description { get; set; }
    }
}
