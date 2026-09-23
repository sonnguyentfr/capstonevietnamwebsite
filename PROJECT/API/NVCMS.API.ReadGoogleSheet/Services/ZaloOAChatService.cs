using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Repositories;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface IZaloOAChatService
    {
        Task<ZaloOAServiceResult<ZaloOAPagedResult<ZaloOAConversation>>> GetConversationsAsync(ZaloOAConversationQuery query);
        Task<ZaloOAServiceResult<ZaloOAConversation>> GetConversationAsync(long id);
        Task<ZaloOAServiceResult<List<ZaloOAMessage>>> GetMessagesAsync(long conversationId, long beforeId, int pageSize);
        Task<ZaloOAServiceResult<List<ZaloOAMessage>>> GetMessageChangesAsync(long conversationId, DateTime since);
        Task<ZaloOAServiceResult<bool>> MarkReadAsync(long conversationId, int userId);
        Task<ZaloOAServiceResult<bool>> SetStatusAsync(long conversationId, string status, int userId);
        Task<ZaloOAServiceResult<ZaloOAUnreadSummary>> GetUnreadSummaryAsync(long afterMessageId);

        Task<ZaloOAServiceResult<ZaloOAPagedResult<ZaloOACustomer>>> GetCustomersAsync(ZaloOACustomerQuery query);
        Task<ZaloOAServiceResult<ZaloOACustomer>> GetCustomerAsync(long id);
        Task<ZaloOAServiceResult<ZaloOACustomer>> SyncCustomerAsync(long id, CancellationToken cancellationToken = default);

        Task<ZaloOAServiceResult<ZaloOAMessage>> SendMessageAsync(ZaloOASendMessageRequest request, CancellationToken cancellationToken = default);
        Task<ZaloOAServiceResult<ZaloOAMessage>> SendAttachmentAsync(long conversationId, Guid clientMessageId, int? agentUserId,
            Stream content, string fileName, string contentType, long length, string? caption, CancellationToken cancellationToken = default);
        Task<ZaloOAServiceResult<ZaloOAMessage>> RetryMessageAsync(long messageId, int? agentUserId, CancellationToken cancellationToken = default);
    }

    public class ZaloOAChatService : IZaloOAChatService
    {
        public const int MaxPageSize = 100;
        public const long MaxImageBytes = 1 * 1024 * 1024;   // Zalo: ảnh jpg/png ≤ 1MB
        public const long MaxFileBytes = 5 * 1024 * 1024;    // Zalo: PDF/DOC/DOCX/CSV ≤ 5MB

        private static readonly string[] ConversationSorts = ["LastMessageAt", "UnreadCount", "CreatedAt", "DisplayName"];
        private static readonly string[] CustomerSorts = ["LastInteractionAt", "DisplayName", "CreatedAt"];
        private static readonly Dictionary<string, string> ImageTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = "image/jpeg", [".jpeg"] = "image/jpeg", [".png"] = "image/png"
        };
        private static readonly Dictionary<string, string> FileTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = "application/pdf",
            [".doc"] = "application/msword",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            [".csv"] = "text/csv"
        };

        private readonly IZaloOAChatRepository _repo;
        private readonly IZaloOAClient _client;
        private readonly IZaloOACustomerService _customers;
        private readonly ZaloSettings _zalo;
        private readonly ILogger<ZaloOAChatService> _logger;

        public ZaloOAChatService(IZaloOAChatRepository repo, IZaloOAClient client, IZaloOACustomerService customers,
            IOptions<ZaloSettings> zalo, ILogger<ZaloOAChatService> logger)
        {
            _repo = repo;
            _client = client;
            _customers = customers;
            _zalo = zalo.Value;
            _logger = logger;
        }

        // ── Conversation ────────────────────────────────────────────────────

        public async Task<ZaloOAServiceResult<ZaloOAPagedResult<ZaloOAConversation>>> GetConversationsAsync(ZaloOAConversationQuery q)
        {
            q.PageSize = Math.Clamp(q.PageSize <= 0 ? 20 : q.PageSize, 1, MaxPageSize);
            q.PageIndex = Math.Max(0, q.PageIndex);
            q.SortBy = ConversationSorts.FirstOrDefault(s => s.Equals(q.SortBy, StringComparison.OrdinalIgnoreCase)) ?? "LastMessageAt";
            q.SortDir = NormalizeDir(q.SortDir);
            q.FromDate = ToUtc(q.FromDate);
            q.ToDate = ToUtc(q.ToDate);
            if (!string.IsNullOrWhiteSpace(q.Status))
            {
                var status = q.Status.Trim().ToUpperInvariant();
                if (!ZaloOAConversationStatus.All.Contains(status))
                    return Invalid<ZaloOAPagedResult<ZaloOAConversation>>("Trạng thái hội thoại không hợp lệ.");
                q.Status = status;
            }
            if (q.FromDate.HasValue && q.ToDate.HasValue && q.FromDate > q.ToDate)
                return Invalid<ZaloOAPagedResult<ZaloOAConversation>>("Khoảng thời gian không hợp lệ.");

            return ZaloOAServiceResult<ZaloOAPagedResult<ZaloOAConversation>>.Ok(await _repo.GetConversationsAsync(q));
        }

        public async Task<ZaloOAServiceResult<ZaloOAConversation>> GetConversationAsync(long id)
        {
            var c = await _repo.GetConversationByIdAsync(id);
            return c == null ? NotFound<ZaloOAConversation>("Không tìm thấy hội thoại.") : ZaloOAServiceResult<ZaloOAConversation>.Ok(c);
        }

        public async Task<ZaloOAServiceResult<List<ZaloOAMessage>>> GetMessagesAsync(long conversationId, long beforeId, int pageSize)
        {
            if (await _repo.GetConversationByIdAsync(conversationId) == null)
                return NotFound<List<ZaloOAMessage>>("Không tìm thấy hội thoại.");

            pageSize = Math.Clamp(pageSize <= 0 ? 30 : pageSize, 1, MaxPageSize);
            var list = await _repo.GetMessagesAsync(conversationId, Math.Max(0, beforeId), pageSize);
            list.Reverse(); // SP trả mới → cũ; UI hiển thị cũ → mới
            return ZaloOAServiceResult<List<ZaloOAMessage>>.Ok(list);
        }

        public async Task<ZaloOAServiceResult<List<ZaloOAMessage>>> GetMessageChangesAsync(long conversationId, DateTime since)
        {
            var utc = ToUtc(since)!.Value;
            if (utc < DateTime.UtcNow.AddDays(-2))
                utc = DateTime.UtcNow.AddDays(-2); // polling chỉ cần thay đổi gần đây
            return ZaloOAServiceResult<List<ZaloOAMessage>>.Ok(await _repo.GetMessageChangesAsync(conversationId, utc, 200));
        }

        public async Task<ZaloOAServiceResult<bool>> MarkReadAsync(long conversationId, int userId)
        {
            var ok = await _repo.MarkConversationReadAsync(conversationId, userId);
            return ok ? ZaloOAServiceResult<bool>.Ok(true) : NotFound<bool>("Không tìm thấy hội thoại.");
        }

        public async Task<ZaloOAServiceResult<bool>> SetStatusAsync(long conversationId, string status, int userId)
        {
            var s = (status ?? "").Trim().ToUpperInvariant();
            if (!ZaloOAConversationStatus.All.Contains(s))
                return Invalid<bool>("Trạng thái hội thoại không hợp lệ (OPEN / PENDING / CLOSED).");

            var ok = await _repo.SetConversationStatusAsync(conversationId, s, userId);
            if (ok)
                _logger.LogInformation("Zalo OA: hội thoại {ConversationId} → {Status} bởi UserId={UserId}", conversationId, s, userId);
            return ok ? ZaloOAServiceResult<bool>.Ok(true) : NotFound<bool>("Không tìm thấy hội thoại.");
        }

        public async Task<ZaloOAServiceResult<ZaloOAUnreadSummary>> GetUnreadSummaryAsync(long afterMessageId)
            => ZaloOAServiceResult<ZaloOAUnreadSummary>.Ok(await _repo.GetUnreadSummaryAsync(Math.Max(0, afterMessageId), 20));

        // ── Customer ────────────────────────────────────────────────────────

        public async Task<ZaloOAServiceResult<ZaloOAPagedResult<ZaloOACustomer>>> GetCustomersAsync(ZaloOACustomerQuery q)
        {
            q.PageSize = Math.Clamp(q.PageSize <= 0 ? 20 : q.PageSize, 1, MaxPageSize);
            q.PageIndex = Math.Max(0, q.PageIndex);
            q.SortBy = CustomerSorts.FirstOrDefault(s => s.Equals(q.SortBy, StringComparison.OrdinalIgnoreCase)) ?? "LastInteractionAt";
            q.SortDir = NormalizeDir(q.SortDir);
            q.FromDate = ToUtc(q.FromDate);
            q.ToDate = ToUtc(q.ToDate);
            if (q.IsFollower is < -1 or > 1) q.IsFollower = -1;
            return ZaloOAServiceResult<ZaloOAPagedResult<ZaloOACustomer>>.Ok(await _repo.GetCustomersAsync(q));
        }

        public async Task<ZaloOAServiceResult<ZaloOACustomer>> GetCustomerAsync(long id)
        {
            var c = await _repo.GetCustomerByIdAsync(id);
            return c == null ? NotFound<ZaloOACustomer>("Không tìm thấy khách hàng.") : ZaloOAServiceResult<ZaloOACustomer>.Ok(c);
        }

        public async Task<ZaloOAServiceResult<ZaloOACustomer>> SyncCustomerAsync(long id, CancellationToken cancellationToken = default)
        {
            var c = await _repo.GetCustomerByIdAsync(id);
            if (c == null)
                return NotFound<ZaloOACustomer>("Không tìm thấy khách hàng.");

            var result = await _customers.SyncProfileAsync(c, cancellationToken);
            if (!result.Success)
                return ZaloFail<ZaloOACustomer>(result.ErrorCode, result.ErrorMessage);

            return ZaloOAServiceResult<ZaloOACustomer>.Ok((await _repo.GetCustomerByIdAsync(id))!);
        }

        // ── Gửi tin ─────────────────────────────────────────────────────────

        public async Task<ZaloOAServiceResult<ZaloOAMessage>> SendMessageAsync(ZaloOASendMessageRequest r, CancellationToken cancellationToken = default)
        {
            var type = string.IsNullOrWhiteSpace(r.MessageType) ? ZaloOAMessageType.Text : r.MessageType.Trim().ToUpperInvariant();
            var text = r.Text?.Trim();

            if (type == ZaloOAMessageType.Text)
            {
                if (string.IsNullOrEmpty(text))
                    return Invalid<ZaloOAMessage>("Nội dung tin nhắn không được để trống.");
                if (text.Length > ZaloOAClient.MaxTextLength)
                    return Invalid<ZaloOAMessage>($"Tin nhắn tối đa {ZaloOAClient.MaxTextLength} ký tự.");
            }
            else if (type == ZaloOAMessageType.Image)
            {
                if (!Uri.TryCreate(r.ImageUrl, UriKind.Absolute, out var u) || u.Scheme != Uri.UriSchemeHttps)
                    return Invalid<ZaloOAMessage>("URL ảnh phải là https hợp lệ.");
                if (text?.Length > ZaloOAClient.MaxTextLength)
                    return Invalid<ZaloOAMessage>($"Chú thích tối đa {ZaloOAClient.MaxTextLength} ký tự.");
            }
            else
            {
                return Invalid<ZaloOAMessage>("Loại tin chỉ hỗ trợ TEXT hoặc IMAGE (file dùng send-attachment).");
            }

            var conv = await _repo.GetConversationByIdAsync(r.ConversationId);
            if (conv == null || string.IsNullOrWhiteSpace(conv.ZaloUserId))
                return NotFound<ZaloOAMessage>("Không tìm thấy hội thoại / khách hàng.");

            var attachments = type == ZaloOAMessageType.Image
                ? JsonSerializer.Serialize(new OutboundAttachment { Url = r.ImageUrl })
                : null;

            var pending = await _repo.InsertPendingMessageAsync(new ZaloOAPendingMessage
            {
                ConversationId = conv.Id,
                CustomerId = conv.CustomerId,
                SenderId = conv.OAId,
                ReceiverId = conv.ZaloUserId,
                AgentUserId = r.AgentUserId,
                MessageType = type,
                Content = type == ZaloOAMessageType.Text ? text : (string.IsNullOrEmpty(text) ? r.ImageUrl : text),
                AttachmentsJson = attachments,
                ClientMessageId = r.ClientMessageId == Guid.Empty ? Guid.NewGuid() : r.ClientMessageId,
                Preview = ZaloOAWebhookParser.BuildPreview(type, text)
            });

            if (pending.IsDuplicate)
            {
                _logger.LogInformation("Zalo OA send: ClientMessageId đã tồn tại → trả tin cũ MessageId={MessageId} Status={Status}",
                    pending.Id, pending.Status);
                return ZaloOAServiceResult<ZaloOAMessage>.Ok(pending, "Tin nhắn đã được gửi trước đó.");
            }

            return await DispatchAsync(pending, conv.ZaloUserId!, cancellationToken);
        }

        public async Task<ZaloOAServiceResult<ZaloOAMessage>> SendAttachmentAsync(long conversationId, Guid clientMessageId, int? agentUserId,
            Stream content, string fileName, string contentType, long length, string? caption, CancellationToken cancellationToken = default)
        {
            var ext = Path.GetExtension(fileName ?? "");
            var isImage = ImageTypes.ContainsKey(ext);
            var isFile = FileTypes.ContainsKey(ext);
            if (!isImage && !isFile)
                return Invalid<ZaloOAMessage>("Chỉ hỗ trợ ảnh JPG/PNG hoặc tệp PDF/DOC/DOCX/CSV.");
            if (length <= 0)
                return Invalid<ZaloOAMessage>("Tệp rỗng.");
            if (isImage && length > MaxImageBytes)
                return Invalid<ZaloOAMessage>("Ảnh tối đa 1MB (giới hạn Zalo).");
            if (isFile && length > MaxFileBytes)
                return Invalid<ZaloOAMessage>("Tệp tối đa 5MB (giới hạn Zalo).");
            if (caption?.Length > ZaloOAClient.MaxTextLength)
                return Invalid<ZaloOAMessage>($"Chú thích tối đa {ZaloOAClient.MaxTextLength} ký tự.");

            var conv = await _repo.GetConversationByIdAsync(conversationId);
            if (conv == null || string.IsNullOrWhiteSpace(conv.ZaloUserId))
                return NotFound<ZaloOAMessage>("Không tìm thấy hội thoại / khách hàng.");

            // Loại MIME lấy theo phần mở rộng đã kiểm tra, không tin Content-Type của client.
            var mime = isImage ? ImageTypes[ext] : FileTypes[ext];
            var safeName = Path.GetFileName(fileName)!;

            var upload = isImage
                ? await _client.UploadImageAsync(content, safeName, mime, cancellationToken)
                : await _client.UploadFileAsync(content, safeName, mime, cancellationToken);
            if (!upload.Success || string.IsNullOrEmpty(upload.Data))
                return ZaloFail<ZaloOAMessage>(upload.ErrorCode, upload.ErrorMessage);

            var type = isImage ? ZaloOAMessageType.Image : ZaloOAMessageType.File;
            var attachment = isImage
                ? new OutboundAttachment { AttachmentId = upload.Data, FileName = safeName }
                : new OutboundAttachment { FileToken = upload.Data, FileName = safeName };

            var pending = await _repo.InsertPendingMessageAsync(new ZaloOAPendingMessage
            {
                ConversationId = conv.Id,
                CustomerId = conv.CustomerId,
                SenderId = conv.OAId,
                ReceiverId = conv.ZaloUserId,
                AgentUserId = agentUserId,
                MessageType = type,
                Content = string.IsNullOrWhiteSpace(caption) ? safeName : caption.Trim(),
                AttachmentsJson = JsonSerializer.Serialize(attachment),
                ClientMessageId = clientMessageId == Guid.Empty ? Guid.NewGuid() : clientMessageId,
                Preview = ZaloOAWebhookParser.BuildPreview(type, safeName)
            });

            if (pending.IsDuplicate)
                return ZaloOAServiceResult<ZaloOAMessage>.Ok(pending, "Tin nhắn đã được gửi trước đó.");

            return await DispatchAsync(pending, conv.ZaloUserId!, cancellationToken);
        }

        public async Task<ZaloOAServiceResult<ZaloOAMessage>> RetryMessageAsync(long messageId, int? agentUserId, CancellationToken cancellationToken = default)
        {
            var msg = await _repo.GetMessageByIdAsync(messageId);
            if (msg == null)
                return NotFound<ZaloOAMessage>("Không tìm thấy tin nhắn.");

            var conv = await _repo.GetConversationByIdAsync(msg.ConversationId);
            if (conv == null || string.IsNullOrWhiteSpace(conv.ZaloUserId))
                return NotFound<ZaloOAMessage>("Không tìm thấy hội thoại / khách hàng.");

            var reset = await _repo.ResetMessageForRetryAsync(messageId, agentUserId);
            if (reset == null)
                return Invalid<ZaloOAMessage>("Chỉ gửi lại được tin nhắn đi đang ở trạng thái FAILED.");

            return await DispatchAsync(reset, conv.ZaloUserId!, cancellationToken);
        }

        /// <summary>Gọi Zalo cho tin PENDING rồi cập nhật SENT / FAILED.</summary>
        private async Task<ZaloOAServiceResult<ZaloOAMessage>> DispatchAsync(ZaloOAMessage msg, string zaloUserId, CancellationToken ct)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["ZaloMessageRowId"] = msg.Id,
                ["ConversationId"] = msg.ConversationId,
                ["CustomerId"] = msg.CustomerId
            });

            var att = string.IsNullOrEmpty(msg.AttachmentsJson) ? null : JsonSerializer.Deserialize<OutboundAttachment>(msg.AttachmentsJson);
            var caption = att?.FileName != null && msg.Content == att.FileName ? null : msg.Content;

            ZaloOAApiResult<ZaloOASendResult> result;
            try
            {
                result = msg.MessageType switch
                {
                    ZaloOAMessageType.Text => await _client.SendTextAsync(zaloUserId, msg.Content ?? "", ct),
                    ZaloOAMessageType.Image => await _client.SendImageAsync(zaloUserId, att?.AttachmentId, att?.Url,
                        att?.Url != null && caption == att.Url ? null : caption, ct),
                    ZaloOAMessageType.File => await _client.SendFileAsync(zaloUserId, att?.FileToken ?? "", ct),
                    _ => ZaloOAApiResult<ZaloOASendResult>.Fail(ZaloApiErrorCodes.InvalidResponse, "Loại tin không hỗ trợ gửi.")
                };
            }
            catch (Exception ex)
            {
                // Lỗi ngoài dự kiến: vẫn đánh dấu FAILED để tin không kẹt PENDING.
                _logger.LogError(ex, "Zalo OA send: lỗi ngoài dự kiến MessageId={MessageId}", msg.Id);
                await _repo.MarkMessageFailedAsync(msg.Id, ZaloApiErrorCodes.HttpError, "Lỗi hệ thống khi gửi: " + ex.Message, null);
                throw;
            }

            if (result.Success && !string.IsNullOrEmpty(result.Data?.MessageId))
            {
                var sent = await _repo.MarkMessageSentAsync(msg.Id, result.Data.MessageId!, result.Data.SentTime, result.RawJson);
                _logger.LogInformation("Zalo OA send OK MessageId={MessageId} ZaloMessageId={ZaloMessageId} AgentUserId={AgentUserId}",
                    msg.Id, result.Data.MessageId, msg.AgentUserId);
                return ZaloOAServiceResult<ZaloOAMessage>.Ok(sent ?? msg, "Đã gửi.");
            }

            var code = result.Success ? ZaloApiErrorCodes.InvalidResponse : result.ErrorCode;
            var error = result.Success ? "Zalo không trả message_id - chưa xác nhận được tin đã gửi." : result.ErrorMessage;
            var failed = await _repo.MarkMessageFailedAsync(msg.Id, code, error, result.RawJson);
            _logger.LogWarning("Zalo OA send FAILED MessageId={MessageId} ErrorCode={ErrorCode} Error={Error}", msg.Id, code, error);

            var fail = ZaloFail<ZaloOAMessage>(code, error);
            fail.Data = failed ?? msg;
            return fail;
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        /// <summary>
        /// ASP.NET Core bind "…Z" thành DateTimeKind.Local (giờ server) → đổi lại UTC để so với cột UTC.
        /// Chuỗi không có múi giờ (Unspecified) được hiểu là UTC.
        /// </summary>
        internal static DateTime? ToUtc(DateTime? value) => value switch
        {
            null => null,
            { Kind: DateTimeKind.Local } v => v.ToUniversalTime(),
            { } v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        };

        private static string NormalizeDir(string? dir) => string.Equals(dir, "ASC", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";

        private static ZaloOAServiceResult<T> Invalid<T>(string message)
            => ZaloOAServiceResult<T>.Fail(ZaloOAErrorCodes.ValidationError, message, StatusCodes.Status400BadRequest);

        private static ZaloOAServiceResult<T> NotFound<T>(string message)
            => ZaloOAServiceResult<T>.Fail(ZaloOAErrorCodes.NotFound, message, StatusCodes.Status404NotFound);

        /// <summary>Map lỗi Zalo → mã lỗi + thông báo cho nhân viên.</summary>
        internal static ZaloOAServiceResult<T> ZaloFail<T>(int zaloCode, string? zaloMessage)
        {
            if (zaloCode == ZaloApiErrorCodes.TokenUnavailable)
                return ZaloOAServiceResult<T>.Fail(ZaloOAErrorCodes.ZaloTokenUnavailable,
                    "Chưa có Zalo access token hợp lệ. Liên hệ quản trị để cấp lại token.", StatusCodes.Status503ServiceUnavailable);
            if (zaloCode == ZaloApiErrorCodes.Timeout)
                return ZaloOAServiceResult<T>.Fail(ZaloOAErrorCodes.ZaloTimeout,
                    "Zalo không phản hồi, vui lòng thử lại.", StatusCodes.Status504GatewayTimeout);
            if (ZaloApiErrorCodes.IsInteractionWindowError(zaloCode))
                return ZaloOAServiceResult<T>.Fail(ZaloOAErrorCodes.ZaloUserNotInteracted,
                    "Khách chưa tương tác với OA trong 7 ngày gần nhất nên Zalo không cho gửi tin tư vấn.", StatusCodes.Status422UnprocessableEntity);
            return ZaloOAServiceResult<T>.Fail(ZaloOAErrorCodes.ZaloApiError,
                $"Zalo từ chối yêu cầu: {zaloMessage} (mã {zaloCode}).", StatusCodes.Status502BadGateway);
        }

        /// <summary>Thông tin đính kèm của tin gửi đi - lưu trong AttachmentsJson để gửi lại được.</summary>
        internal class OutboundAttachment
        {
            public string? Url { get; set; }
            public string? AttachmentId { get; set; }
            public string? FileToken { get; set; }
            public string? FileName { get; set; }
        }
    }
}
