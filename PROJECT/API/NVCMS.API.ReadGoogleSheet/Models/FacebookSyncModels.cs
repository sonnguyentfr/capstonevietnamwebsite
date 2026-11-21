using System.Text.Json.Serialization;

namespace NVCMS.API.ReadGoogleSheet.Models
{
    // FB-SYNC 19/11/2026: DTO đọc Graph API /{page-id}/published_posts

    public class FbPagedResponse<T>
    {
        [JsonPropertyName("data")] public List<T> Data { get; set; } = new();
        [JsonPropertyName("paging")] public FbPaging? Paging { get; set; }
    }

    public class FbPaging
    {
        [JsonPropertyName("next")] public string? Next { get; set; }
    }

    public class FbPost
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("message")] public string? Message { get; set; }
        [JsonPropertyName("created_time")] public string? CreatedTimeRaw { get; set; }
        [JsonPropertyName("permalink_url")] public string? PermalinkUrl { get; set; }
        [JsonPropertyName("full_picture")] public string? FullPicture { get; set; }
        [JsonPropertyName("status_type")] public string? StatusType { get; set; }
        [JsonPropertyName("attachments")] public FbPagedResponse<FbAttachment>? Attachments { get; set; }

        /// <summary>created_time (UTC). Graph trả dạng 2026-11-19T03:15:00+0000.</summary>
        [JsonIgnore]
        public DateTime? CreatedTimeUtc =>
            DateTimeOffset.TryParse(CreatedTimeRaw, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal, out var d)
                ? d.UtcDateTime
                : null;
    }

    public class FbAttachment
    {
        [JsonPropertyName("media_type")] public string? MediaType { get; set; }
        [JsonPropertyName("type")] public string? Type { get; set; }
        [JsonPropertyName("url")] public string? Url { get; set; }
        [JsonPropertyName("media")] public FbMedia? Media { get; set; }
        [JsonPropertyName("subattachments")] public FbPagedResponse<FbAttachment>? Subattachments { get; set; }
    }

    public class FbMedia
    {
        [JsonPropertyName("image")] public FbImage? Image { get; set; }
    }

    public class FbImage
    {
        [JsonPropertyName("src")] public string? Src { get; set; }
    }

    /// <summary>Kết quả một lần chạy đồng bộ.</summary>
    public class FacebookSyncResult
    {
        public int Fetched { get; set; }
        public int Imported { get; set; }
        public int Skipped { get; set; }
        public int Failed { get; set; }
        public List<string> Messages { get; } = new();

        public override string ToString() =>
            $"Đọc {Fetched} bài, tạo mới {Imported}, bỏ qua {Skipped}, lỗi {Failed}";
    }

    public enum FacebookSyncStatus : byte
    {
        Processing = 0,
        Imported = 1,
        Skipped = 2,
        Failed = 3
    }
}
