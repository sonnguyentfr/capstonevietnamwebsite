using NVCMS.API.ReadGoogleSheet.Common;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    /// <summary>
    /// Event webhook Zalo OA đã parse. Payload gốc (tài liệu webhook Zalo OA):
    /// { "app_id", "user_id_by_app", "event_name", "timestamp": "ms",
    ///   "sender": { "id", "admin_id"? }, "recipient": { "id" },
    ///   "message": { "msg_id", "text", "attachments": [ { "type", "payload": {...} } ], "quote_msg_id"?, "msg_ids"? },
    ///   "follower": { "id" }, "oa_id", "info": {...} }
    /// </summary>
    public class ZaloOAWebhookEvent
    {
        public string EventName { get; set; } = "";
        public string? AppId { get; set; }
        public string? OAId { get; set; }
        /// <summary>user_id (theo OA) của khách liên quan tới event.</summary>
        public string? CustomerUserId { get; set; }
        public string? UserIdByApp { get; set; }
        public string? SenderId { get; set; }
        public string? SenderAdminId { get; set; }
        public string? RecipientId { get; set; }
        public string? MessageId { get; set; }
        public List<string> MessageIds { get; set; } = new();
        public string? Text { get; set; }
        public string? QuoteMessageId { get; set; }
        public string? AttachmentsJson { get; set; }
        public string? FirstAttachmentType { get; set; }
        public JsonElement? FirstAttachmentPayload { get; set; }
        public string? RawTimestamp { get; set; }
        public long? TimestampMs { get; set; }
        public JsonElement? Info { get; set; }
        public ZaloOAWebhookKind Kind { get; set; }

        public DateTime OccurredAtUtc =>
            TimestampMs is > 0 ? DateTimeOffset.FromUnixTimeMilliseconds(TimestampMs.Value).UtcDateTime : DateTime.UtcNow;
    }

    public enum ZaloOAWebhookKind
    {
        Unknown,
        UserMessage,      // user_send_*
        OAMessage,        // oa_send_*
        Follow,
        Unfollow,
        UserSubmitInfo,
        UserSeen,         // user_seen_message
        UserReceived,     // user_received_message
        Reaction,         // user_reacted_message / oa_reacted_message
        Anonymous         // anonymous_* (chưa hỗ trợ)
    }

    public static class ZaloOAWebhookParser
    {
        /// <summary>Parse raw body. Ném JsonException nếu không phải JSON object hợp lệ.</summary>
        public static ZaloOAWebhookEvent Parse(string rawBody)
        {
            using var doc = JsonDocument.Parse(rawBody);
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                throw new JsonException("Webhook payload không phải JSON object.");

            var e = new ZaloOAWebhookEvent
            {
                EventName = ZaloOAClient.GetString(root, "event_name") ?? "",
                AppId = ZaloOAClient.GetString(root, "app_id"),
                UserIdByApp = ZaloOAClient.GetString(root, "user_id_by_app"),
                RawTimestamp = ZaloOAClient.GetString(root, "timestamp"),
                TimestampMs = ZaloOAClient.GetLong(root, "timestamp")
            };

            if (root.TryGetProperty("sender", out var sender) && sender.ValueKind == JsonValueKind.Object)
            {
                e.SenderId = ZaloOAClient.GetString(sender, "id");
                e.SenderAdminId = ZaloOAClient.GetString(sender, "admin_id");
            }
            if (root.TryGetProperty("recipient", out var recipient) && recipient.ValueKind == JsonValueKind.Object)
                e.RecipientId = ZaloOAClient.GetString(recipient, "id");

            if (root.TryGetProperty("message", out var msg) && msg.ValueKind == JsonValueKind.Object)
            {
                e.MessageId = ZaloOAClient.GetString(msg, "msg_id");
                e.Text = ZaloOAClient.GetString(msg, "text");
                e.QuoteMessageId = ZaloOAClient.GetString(msg, "quote_msg_id");

                if (msg.TryGetProperty("msg_ids", out var ids) && ids.ValueKind == JsonValueKind.Array)
                    foreach (var id in ids.EnumerateArray())
                        if (id.ValueKind is JsonValueKind.String or JsonValueKind.Number)
                            e.MessageIds.Add(id.ValueKind == JsonValueKind.String ? id.GetString()! : id.GetRawText());

                if (msg.TryGetProperty("attachments", out var atts) && atts.ValueKind == JsonValueKind.Array)
                {
                    e.AttachmentsJson = atts.GetRawText();
                    var first = atts.EnumerateArray().FirstOrDefault();
                    if (first.ValueKind == JsonValueKind.Object)
                    {
                        e.FirstAttachmentType = ZaloOAClient.GetString(first, "type");
                        if (first.TryGetProperty("payload", out var payload))
                            e.FirstAttachmentPayload = payload.Clone();
                    }
                }
            }
            if (!string.IsNullOrEmpty(e.MessageId) && e.MessageIds.Count == 0)
                e.MessageIds.Add(e.MessageId);

            if (root.TryGetProperty("info", out var info) && info.ValueKind == JsonValueKind.Object)
                e.Info = info.Clone();

            var name = e.EventName;
            if (name.StartsWith("user_send_", StringComparison.Ordinal))
            {
                e.Kind = ZaloOAWebhookKind.UserMessage;
                e.CustomerUserId = e.SenderId;
                e.OAId = e.RecipientId;
            }
            else if (name.StartsWith("oa_send_", StringComparison.Ordinal))
            {
                e.Kind = ZaloOAWebhookKind.OAMessage;
                e.CustomerUserId = e.RecipientId;
                e.OAId = e.SenderId;
            }
            else if (name is "follow" or "unfollow")
            {
                e.Kind = name == "follow" ? ZaloOAWebhookKind.Follow : ZaloOAWebhookKind.Unfollow;
                e.OAId = ZaloOAClient.GetString(root, "oa_id");
                if (root.TryGetProperty("follower", out var follower) && follower.ValueKind == JsonValueKind.Object)
                    e.CustomerUserId = ZaloOAClient.GetString(follower, "id");
            }
            else if (name == "user_submit_info")
            {
                e.Kind = ZaloOAWebhookKind.UserSubmitInfo;
                e.CustomerUserId = e.SenderId;
                e.OAId = e.RecipientId;
            }
            else if (name is "user_seen_message" or "user_received_message")
            {
                e.Kind = name == "user_seen_message" ? ZaloOAWebhookKind.UserSeen : ZaloOAWebhookKind.UserReceived;
                e.CustomerUserId = e.SenderId;
                e.OAId = e.RecipientId;
            }
            else if (name.EndsWith("_reacted_message", StringComparison.Ordinal))
            {
                e.Kind = ZaloOAWebhookKind.Reaction;
                e.CustomerUserId = name.StartsWith("user_", StringComparison.Ordinal) ? e.SenderId : e.RecipientId;
                e.OAId = name.StartsWith("user_", StringComparison.Ordinal) ? e.RecipientId : e.SenderId;
            }
            else if (name.StartsWith("anonymous_", StringComparison.Ordinal))
            {
                e.Kind = ZaloOAWebhookKind.Anonymous;
            }

            return e;
        }

        /// <summary>
        /// Khoá chống trùng: event có msg_id → theo (event_name, msg_id) vì Zalo retry gửi lại cùng tin;
        /// event khác → SHA-256 của nguyên body.
        /// </summary>
        public static string ComputeDedupKey(ZaloOAWebhookEvent e, string rawBody)
        {
            var source = !string.IsNullOrEmpty(e.MessageId) && e.Kind is ZaloOAWebhookKind.UserMessage or ZaloOAWebhookKind.OAMessage
                ? $"msg|{e.EventName}|{e.MessageId}"
                : "raw|" + rawBody;
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(source))).ToLowerInvariant();
        }

        /// <summary>Map event_name → MessageType nội bộ.</summary>
        public static string MapMessageType(ZaloOAWebhookEvent e)
        {
            var suffix = e.EventName;
            var idx = suffix.IndexOf("_send_", StringComparison.Ordinal);
            suffix = idx >= 0 ? suffix[(idx + 6)..] : e.FirstAttachmentType ?? "";

            return suffix switch
            {
                "text" => ZaloOAMessageType.Text,
                "image" or "gif" => ZaloOAMessageType.Image,
                "file" => ZaloOAMessageType.File,
                "video" => ZaloOAMessageType.Video,
                "audio" => ZaloOAMessageType.Audio,
                "sticker" => ZaloOAMessageType.Sticker,
                "location" => ZaloOAMessageType.Location,
                "link" => ZaloOAMessageType.Link,
                "business_card" => ZaloOAMessageType.Contact,
                _ => ZaloOAMessageType.Other
            };
        }

        /// <summary>Nội dung hiển thị: text, hoặc url/tên file/toạ độ lấy từ attachment đầu tiên.</summary>
        public static string? BuildContent(ZaloOAWebhookEvent e, string messageType)
        {
            if (!string.IsNullOrWhiteSpace(e.Text))
                return e.Text;

            if (e.FirstAttachmentPayload is not { ValueKind: JsonValueKind.Object } p)
                return null;

            if (messageType == ZaloOAMessageType.Location
                && p.TryGetProperty("coordinates", out var c) && c.ValueKind == JsonValueKind.Object)
                return $"{ZaloOAClient.GetString(c, "latitude")},{ZaloOAClient.GetString(c, "longitude")}";

            if (messageType == ZaloOAMessageType.File)
                return ZaloOAClient.GetString(p, "name") ?? ZaloOAClient.GetString(p, "url");

            return ZaloOAClient.GetString(p, "url") ?? ZaloOAClient.GetString(p, "description");
        }

        public static string BuildPreview(string messageType, string? content)
        {
            var label = messageType switch
            {
                ZaloOAMessageType.Text => null,
                ZaloOAMessageType.Image => "[Hình ảnh]",
                ZaloOAMessageType.File => "[Tệp] ",
                ZaloOAMessageType.Video => "[Video]",
                ZaloOAMessageType.Audio => "[Âm thanh]",
                ZaloOAMessageType.Sticker => "[Sticker]",
                ZaloOAMessageType.Location => "[Vị trí]",
                ZaloOAMessageType.Link => "[Liên kết] ",
                ZaloOAMessageType.Contact => "[Danh thiếp]",
                ZaloOAMessageType.Event => null,
                _ => "[Tin nhắn]"
            };

            string text = label switch
            {
                null => content ?? "",
                _ when label.EndsWith(' ') => label + (content ?? ""),
                _ => label
            };
            text = text.Replace('\r', ' ').Replace('\n', ' ').Trim();
            return text.Length <= 200 ? text : text[..200];
        }
    }

    public static class ZaloOAWebhookSignature
    {
        /// <summary>
        /// Zalo: header X-ZEvent-Signature = "mac=" + sha256(appId + data + timeStamp + OAsecretKey),
        /// data = raw body, timeStamp = field "timestamp" của payload, OAsecretKey = "OA Secret Key" của webhook.
        /// Chấp nhận header có hoặc không có tiền tố "mac=".
        /// </summary>
        public static bool Verify(string? header, string appId, string rawBody, string? timestamp, string oaSecretKey)
        {
            if (string.IsNullOrWhiteSpace(header) || string.IsNullOrEmpty(oaSecretKey) || string.IsNullOrEmpty(appId))
                return false;

            var provided = header.Trim();
            if (provided.StartsWith("mac=", StringComparison.OrdinalIgnoreCase))
                provided = provided[4..];

            var expected = Compute(appId, rawBody, timestamp, oaSecretKey);
            var a = Encoding.ASCII.GetBytes(expected);
            var b = Encoding.ASCII.GetBytes(provided.ToLowerInvariant());
            return a.Length == b.Length && CryptographicOperations.FixedTimeEquals(a, b);
        }

        public static string Compute(string appId, string rawBody, string? timestamp, string oaSecretKey)
            => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(appId + rawBody + (timestamp ?? "") + oaSecretKey)))
                      .ToLowerInvariant();
    }
}
