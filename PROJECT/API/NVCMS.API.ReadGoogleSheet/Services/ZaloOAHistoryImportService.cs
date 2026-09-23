using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    /// <summary>
    /// Nhập lịch sử chat có sẵn trên Zalo OA:
    ///   1. v2.0/oa/listrecentchat (10/lần) → danh sách khách đã chat gần đây.
    ///   2. v2.0/oa/conversation (10/lần) cho từng khách → tin nhắn.
    /// Idempotent: tin trùng ZaloMessageId (đã có từ webhook hoặc lần nhập trước) được bỏ qua.
    /// Không tăng số tin chưa đọc, không mở lại hội thoại đã đóng.
    /// </summary>
    public interface IZaloOAHistoryImportService
    {
        Task<ZaloOAHistoryImportResult> ImportAsync(ZaloOAHistoryImportRequest request, CancellationToken cancellationToken = default);
    }

    public class ZaloOAHistoryImportService : IZaloOAHistoryImportService
    {
        private readonly IZaloOAClient _client;
        private readonly IZaloOAChatRepository _repo;
        private readonly IZaloOACustomerService _customers;
        private readonly ZaloSettings _zalo;
        private readonly ZaloOAChatSettings _settings;
        private readonly ILogger<ZaloOAHistoryImportService> _logger;

        public ZaloOAHistoryImportService(IZaloOAClient client, IZaloOAChatRepository repo, IZaloOACustomerService customers,
            IOptions<ZaloSettings> zalo, IOptions<ZaloOAChatSettings> settings, ILogger<ZaloOAHistoryImportService> logger)
        {
            _client = client;
            _repo = repo;
            _customers = customers;
            _zalo = zalo.Value;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<ZaloOAHistoryImportResult> ImportAsync(ZaloOAHistoryImportRequest request, CancellationToken cancellationToken = default)
        {
            var result = new ZaloOAHistoryImportResult();
            var maxConversations = Math.Clamp(request.MaxConversations ?? _settings.HistoryImportMaxConversations, 1, 10000);
            var maxMessages = Math.Clamp(request.MaxMessagesPerUser ?? _settings.HistoryImportMaxMessagesPerUser, 1, 5000);

            var oaId = await ResolveOAIdAsync(cancellationToken);
            if (string.IsNullOrEmpty(oaId))
                throw new InvalidOperationException("Không xác định được OA ID (cấu hình ZaloSettings:OAId hoặc quyền gọi getoa).");

            // 1. Danh sách khách từ các hội thoại gần đây
            var users = new Dictionary<string, (string? Name, string? Avatar)>();
            for (var offset = 0; offset < maxConversations; offset += ZaloOAClient.MaxHistoryPageSize)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var page = await _client.GetRecentChatsAsync(offset, ZaloOAClient.MaxHistoryPageSize, cancellationToken);
                if (!page.Success)
                {
                    _logger.LogWarning("Zalo history: listrecentchat offset={Offset} lỗi {ErrorCode} {Message}", offset, page.ErrorCode, page.ErrorMessage);
                    result.Errors++;
                    break;
                }

                var items = page.Data ?? new();
                foreach (var m in items)
                {
                    var (userId, name, avatar) = ResolveCustomerSide(m, oaId);
                    if (!string.IsNullOrEmpty(userId) && !users.ContainsKey(userId))
                        users[userId] = (name, avatar);
                }
                result.ConversationsScanned += items.Count;

                if (items.Count < ZaloOAClient.MaxHistoryPageSize)
                    break;
                await Delay(cancellationToken);
            }

            _logger.LogInformation("Zalo history: {Count} khách cần nhập lịch sử", users.Count);

            // 2. Tin nhắn từng khách
            foreach (var (userId, info) in users)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    await ImportUserAsync(oaId, userId, info.Name, info.Avatar, maxMessages, result, cancellationToken);
                    result.Customers++;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    result.Errors++;
                    _logger.LogError(ex, "Zalo history: lỗi khi nhập khách ZaloUserId={ZaloUserId}", userId);
                }
            }

            _logger.LogInformation(
                "Zalo history xong: Scanned={Scanned} Customers={Customers} Inserted={Inserted} Duplicate={Duplicate} Errors={Errors} RequestedBy={UserId}",
                result.ConversationsScanned, result.Customers, result.MessagesInserted, result.MessagesDuplicate, result.Errors, request.RequestedByUserId);
            return result;
        }

        private async Task ImportUserAsync(string oaId, string userId, string? name, string? avatar, int maxMessages,
            ZaloOAHistoryImportResult result, CancellationToken ct)
        {
            var customer = await _customers.EnsureCustomerAsync(oaId, userId, null, null, name, avatar, syncProfile: true, cancellationToken: ct);
            var conversation = await _repo.GetOrCreateConversationAsync(oaId, customer.Id, reopenIfClosed: false);

            for (var offset = 0; offset < maxMessages; offset += ZaloOAClient.MaxHistoryPageSize)
            {
                await Delay(ct);
                var page = await _client.GetConversationAsync(userId, offset, ZaloOAClient.MaxHistoryPageSize, ct);
                if (!page.Success)
                {
                    _logger.LogWarning("Zalo history: conversation user={ZaloUserId} offset={Offset} lỗi {ErrorCode} {Message}",
                        userId, offset, page.ErrorCode, page.ErrorMessage);
                    result.Errors++;
                    return;
                }

                var items = page.Data ?? new();
                DateTime? latestCustomerMessage = null;
                foreach (var m in items)
                {
                    if (string.IsNullOrEmpty(m.MessageId))
                        continue;

                    var fromCustomer = m.Src == 1;
                    var type = MapHistoryType(m.Type);
                    var content = !string.IsNullOrWhiteSpace(m.Message) ? m.Message : (m.Url ?? m.Thumb);
                    var sentAt = ZaloOAClient.ParseEpochMs(m.Time.ToString()) ?? DateTime.UtcNow;
                    if (fromCustomer && (latestCustomerMessage == null || sentAt > latestCustomerMessage))
                        latestCustomerMessage = sentAt;

                    var upsert = await _repo.UpsertMessageAsync(new ZaloOAMessageUpsert
                    {
                        ConversationId = conversation.Id,
                        CustomerId = customer.Id,
                        Direction = fromCustomer ? ZaloOADirection.In : ZaloOADirection.Out,
                        SenderType = fromCustomer ? ZaloOASenderType.Customer : ZaloOASenderType.OA,
                        SenderId = m.FromId,
                        ReceiverId = m.ToId,
                        MessageType = type,
                        Content = content,
                        AttachmentsJson = m.Url != null || m.Thumb != null
                            ? System.Text.Json.JsonSerializer.Serialize(new { url = m.Url, thumb = m.Thumb })
                            : null,
                        ZaloMessageId = m.MessageId,
                        Status = fromCustomer ? ZaloOAMessageStatus.Received : ZaloOAMessageStatus.Sent,
                        SentAt = sentAt,
                        RawPayload = m.RawJson,
                        Preview = ZaloOAWebhookParser.BuildPreview(type, content),
                        IncrementUnread = false
                    });

                    if (upsert.IsDuplicate) result.MessagesDuplicate++;
                    else result.MessagesInserted++;
                }

                if (latestCustomerMessage.HasValue)
                    await _repo.UpsertCustomerAsync(oaId, userId, null, latestCustomerMessage);

                if (items.Count < ZaloOAClient.MaxHistoryPageSize)
                    return;
            }
        }

        private async Task<string?> ResolveOAIdAsync(CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(_zalo.OAId))
                return _zalo.OAId;

            var info = await _client.GetOAInfoAsync(ct);
            if (!info.Success)
                _logger.LogWarning("Zalo history: getoa lỗi {ErrorCode} {Message}", info.ErrorCode, info.ErrorMessage);
            return info.Data?.OAId;
        }

        /// <summary>src=1: khách → OA (khách là from); src=0: OA → khách (khách là to).</summary>
        internal static (string? UserId, string? Name, string? Avatar) ResolveCustomerSide(ZaloOAHistoryMessage m, string oaId)
        {
            if (m.Src == 1 || (m.ToId == oaId && m.FromId != oaId))
                return (m.FromId, m.FromDisplayName, m.FromAvatar);
            return (m.ToId, m.ToDisplayName, m.ToAvatar);
        }

        /// <summary>type của API lịch sử: text | voice | photo | GIF | link | links | sticker | location.</summary>
        internal static string MapHistoryType(string? type) => (type ?? "").ToLowerInvariant() switch
        {
            "text" => ZaloOAMessageType.Text,
            "photo" or "gif" or "image" => ZaloOAMessageType.Image,
            "voice" or "audio" => ZaloOAMessageType.Audio,
            "video" => ZaloOAMessageType.Video,
            "sticker" => ZaloOAMessageType.Sticker,
            "location" => ZaloOAMessageType.Location,
            "link" or "links" => ZaloOAMessageType.Link,
            "file" => ZaloOAMessageType.File,
            _ => ZaloOAMessageType.Other
        };

        private Task Delay(CancellationToken ct)
            => _settings.HistoryImportDelayMs > 0 ? Task.Delay(_settings.HistoryImportDelayMs, ct) : Task.CompletedTask;
    }
}
