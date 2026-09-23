using Hangfire;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Repositories;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public enum ZaloOAWebhookReceiveOutcome
    {
        Accepted,
        Duplicate,
        InvalidPayload,
        InvalidSignature,
        NotConfigured
    }

    public class ZaloOAWebhookReceiveResult
    {
        public ZaloOAWebhookReceiveOutcome Outcome { get; set; }
        public long? EventId { get; set; }
    }

    /// <summary>
    /// Webhook Zalo OA theo nguyên tắc Receive → Validate → Persist → Process:
    ///   ReceiveAsync  : chạy trong request (phải trả 200 trong 2 giây) - kiểm tra chữ ký, lưu event, enqueue Hangfire.
    ///   ProcessAsync  : chạy trong Hangfire job - tạo/cập nhật khách, hội thoại, tin nhắn (idempotent).
    /// </summary>
    public interface IZaloOAWebhookService
    {
        Task<ZaloOAWebhookReceiveResult> ReceiveAsync(string rawBody, string? signatureHeader, CancellationToken cancellationToken = default);
        Task ProcessAsync(long eventId, CancellationToken cancellationToken = default);
    }

    public class ZaloOAWebhookService : IZaloOAWebhookService
    {
        private readonly IZaloOAChatRepository _repo;
        private readonly IZaloOACustomerService _customers;
        private readonly IBackgroundJobClient _jobs;
        private readonly ZaloSettings _zalo;
        private readonly ZaloOAChatSettings _settings;
        private readonly ILogger<ZaloOAWebhookService> _logger;

        public ZaloOAWebhookService(
            IZaloOAChatRepository repo,
            IZaloOACustomerService customers,
            IBackgroundJobClient jobs,
            IOptions<ZaloSettings> zalo,
            IOptions<ZaloOAChatSettings> settings,
            ILogger<ZaloOAWebhookService> logger)
        {
            _repo = repo;
            _customers = customers;
            _jobs = jobs;
            _zalo = zalo.Value;
            _settings = settings.Value;
            _logger = logger;
        }

        // ── Receive ─────────────────────────────────────────────────────────

        public async Task<ZaloOAWebhookReceiveResult> ReceiveAsync(string rawBody, string? signatureHeader, CancellationToken cancellationToken = default)
        {
            ZaloOAWebhookEvent evt;
            try
            {
                evt = ZaloOAWebhookParser.Parse(rawBody);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Zalo webhook: payload không phải JSON hợp lệ ({Length} ký tự): {Error}", rawBody.Length, ex.Message);
                return new() { Outcome = ZaloOAWebhookReceiveOutcome.InvalidPayload };
            }

            if (string.IsNullOrWhiteSpace(evt.EventName))
            {
                _logger.LogWarning("Zalo webhook: thiếu event_name.");
                return new() { Outcome = ZaloOAWebhookReceiveOutcome.InvalidPayload };
            }

            var signatureValid = false;
            if (_settings.VerifyWebhookSignature)
            {
                if (string.IsNullOrWhiteSpace(_zalo.OASecretKey) || string.IsNullOrWhiteSpace(_zalo.AppId))
                {
                    _logger.LogError("Zalo webhook: chưa cấu hình ZaloSettings:OASecretKey / AppId - không thể xác thực chữ ký, từ chối event {EventName}.",
                        evt.EventName);
                    return new() { Outcome = ZaloOAWebhookReceiveOutcome.NotConfigured };
                }

                signatureValid = ZaloOAWebhookSignature.Verify(signatureHeader, _zalo.AppId, rawBody, evt.RawTimestamp, _zalo.OASecretKey);
                if (!signatureValid)
                {
                    _logger.LogWarning("Zalo webhook: sai chữ ký X-ZEvent-Signature. EventName={EventName} AppId={AppId} HasHeader={HasHeader}",
                        evt.EventName, evt.AppId, !string.IsNullOrEmpty(signatureHeader));
                    return new() { Outcome = ZaloOAWebhookReceiveOutcome.InvalidSignature };
                }
            }
            else
            {
                _logger.LogWarning("Zalo webhook: ĐANG TẮT kiểm tra chữ ký (ZaloOAChat:VerifyWebhookSignature=false). Không dùng cấu hình này trên production.");
            }

            var dedupKey = ZaloOAWebhookParser.ComputeDedupKey(evt, rawBody);
            var inserted = await _repo.InsertWebhookEventAsync(
                dedupKey, evt.EventName, evt.OAId ?? _zalo.OAId, evt.CustomerUserId, evt.MessageId, evt.TimestampMs, rawBody, signatureValid);

            if (inserted.IsDuplicate)
            {
                _logger.LogInformation("Zalo webhook trùng (retry): EventId={EventId} EventName={EventName} Status={Status}",
                    inserted.Id, evt.EventName, inserted.ProcessStatus);
                return new() { Outcome = ZaloOAWebhookReceiveOutcome.Duplicate, EventId = inserted.Id };
            }

            try
            {
                _jobs.Enqueue<ZaloOAWebhookProcessJob>(j => j.ExecuteAsync(inserted.Id, CancellationToken.None));
            }
            catch (Exception ex)
            {
                // Event đã lưu PENDING - job ZaloOAWebhookReprocessJob sẽ xử lý lại, không mất event.
                _logger.LogError(ex, "Zalo webhook: enqueue Hangfire thất bại EventId={EventId}, sẽ xử lý lại bởi job định kỳ.", inserted.Id);
            }

            _logger.LogInformation("Zalo webhook nhận: EventId={EventId} EventName={EventName}", inserted.Id, evt.EventName);
            return new() { Outcome = ZaloOAWebhookReceiveOutcome.Accepted, EventId = inserted.Id };
        }

        // ── Process ─────────────────────────────────────────────────────────

        public async Task ProcessAsync(long eventId, CancellationToken cancellationToken = default)
        {
            var row = await _repo.GetWebhookEventAsync(eventId);
            if (row == null)
            {
                _logger.LogWarning("Zalo webhook process: không tìm thấy EventId={EventId}", eventId);
                return;
            }
            if (row.ProcessStatus is ZaloOAWebhookStatus.Processed or ZaloOAWebhookStatus.Ignored)
                return;

            using var scope = _logger.BeginScope(new Dictionary<string, object> { ["ZaloWebhookEventId"] = eventId, ["ZaloEventName"] = row.EventName });

            try
            {
                var evt = ZaloOAWebhookParser.Parse(row.RawPayload);
                var status = await HandleAsync(evt, row, cancellationToken);
                await _repo.SetWebhookEventStatusAsync(eventId, status, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Zalo webhook process lỗi EventId={EventId} EventName={EventName}", eventId, row.EventName);
                await _repo.SetWebhookEventStatusAsync(eventId, ZaloOAWebhookStatus.Failed, ex.Message);
                throw;
            }
        }

        private async Task<string> HandleAsync(ZaloOAWebhookEvent evt, ZaloOAWebhookEventRow row, CancellationToken ct)
        {
            if (evt.Kind == ZaloOAWebhookKind.Unknown || evt.Kind == ZaloOAWebhookKind.Anonymous)
            {
                _logger.LogInformation("Zalo webhook: bỏ qua event {EventName} (chưa hỗ trợ).", evt.EventName);
                return ZaloOAWebhookStatus.Ignored;
            }

            if (string.IsNullOrWhiteSpace(evt.CustomerUserId))
            {
                _logger.LogWarning("Zalo webhook: event {EventName} không xác định được user_id khách.", evt.EventName);
                return ZaloOAWebhookStatus.Ignored;
            }

            var oaId = evt.OAId ?? _zalo.OAId ?? "";

            switch (evt.Kind)
            {
                case ZaloOAWebhookKind.UserMessage:
                    await HandleUserMessageAsync(evt, row, oaId, ct);
                    return ZaloOAWebhookStatus.Processed;

                case ZaloOAWebhookKind.OAMessage:
                    await HandleOAMessageAsync(evt, row, oaId, ct);
                    return ZaloOAWebhookStatus.Processed;

                case ZaloOAWebhookKind.Follow:
                case ZaloOAWebhookKind.Unfollow:
                    await HandleFollowAsync(evt, row, oaId, ct);
                    return ZaloOAWebhookStatus.Processed;

                case ZaloOAWebhookKind.UserSubmitInfo:
                    await HandleSubmitInfoAsync(evt, row, oaId, ct);
                    return ZaloOAWebhookStatus.Processed;

                case ZaloOAWebhookKind.UserSeen:
                case ZaloOAWebhookKind.UserReceived:
                    var status = evt.Kind == ZaloOAWebhookKind.UserSeen ? ZaloOAMessageStatus.Seen : ZaloOAMessageStatus.Delivered;
                    foreach (var msgId in evt.MessageIds.Distinct())
                        await _repo.UpdateMessageStatusByZaloIdAsync(msgId, status, evt.OccurredAtUtc);
                    return ZaloOAWebhookStatus.Processed;

                case ZaloOAWebhookKind.Reaction:
                    // Phase 1: chỉ lưu event gốc (ZaloOA_WebhookEvent), chưa hiển thị reaction.
                    return ZaloOAWebhookStatus.Processed;

                default:
                    return ZaloOAWebhookStatus.Ignored;
            }
        }

        private async Task HandleUserMessageAsync(ZaloOAWebhookEvent evt, ZaloOAWebhookEventRow row, string oaId, CancellationToken ct)
        {
            var customer = await _customers.EnsureCustomerAsync(oaId, evt.CustomerUserId!, evt.UserIdByApp, evt.OccurredAtUtc, cancellationToken: ct);
            var conversation = await _repo.GetOrCreateConversationAsync(oaId, customer.Id, reopenIfClosed: true);

            var type = ZaloOAWebhookParser.MapMessageType(evt);
            var content = ZaloOAWebhookParser.BuildContent(evt, type);

            var result = await _repo.UpsertMessageAsync(new ZaloOAMessageUpsert
            {
                ConversationId = conversation.Id,
                CustomerId = customer.Id,
                Direction = ZaloOADirection.In,
                SenderType = ZaloOASenderType.Customer,
                SenderId = evt.SenderId,
                ReceiverId = evt.RecipientId,
                MessageType = type,
                Content = content,
                AttachmentsJson = evt.AttachmentsJson,
                QuoteZaloMessageId = evt.QuoteMessageId,
                ZaloMessageId = evt.MessageId,
                Status = ZaloOAMessageStatus.Received,
                SentAt = evt.OccurredAtUtc,
                ReceivedAt = row.ReceivedAt,
                WebhookEventId = row.Id,
                RawPayload = row.RawPayload,
                Preview = ZaloOAWebhookParser.BuildPreview(type, content),
                IncrementUnread = true
            });

            _logger.LogInformation("Zalo webhook: lưu tin khách MessageId={MessageId} ConversationId={ConversationId} CustomerId={CustomerId} Duplicate={Duplicate}",
                result.Id, conversation.Id, customer.Id, result.IsDuplicate);
        }

        private async Task HandleOAMessageAsync(ZaloOAWebhookEvent evt, ZaloOAWebhookEventRow row, string oaId, CancellationToken ct)
        {
            // Tin OA gửi (qua API của hệ thống này, hoặc nhân viên chat trực tiếp trên OA Admin).
            var customer = await _customers.EnsureCustomerAsync(oaId, evt.CustomerUserId!, null, null, cancellationToken: ct);
            var conversation = await _repo.GetOrCreateConversationAsync(oaId, customer.Id, reopenIfClosed: false);

            var type = ZaloOAWebhookParser.MapMessageType(evt);
            var content = ZaloOAWebhookParser.BuildContent(evt, type);

            await _repo.UpsertMessageAsync(new ZaloOAMessageUpsert
            {
                ConversationId = conversation.Id,
                CustomerId = customer.Id,
                Direction = ZaloOADirection.Out,
                SenderType = ZaloOASenderType.OA,
                SenderId = evt.SenderAdminId ?? evt.SenderId,
                ReceiverId = evt.RecipientId,
                MessageType = type,
                Content = content,
                AttachmentsJson = evt.AttachmentsJson,
                QuoteZaloMessageId = evt.QuoteMessageId,
                ZaloMessageId = evt.MessageId,
                Status = ZaloOAMessageStatus.Sent,
                SentAt = evt.OccurredAtUtc,
                ReceivedAt = row.ReceivedAt,
                WebhookEventId = row.Id,
                RawPayload = row.RawPayload,
                Preview = ZaloOAWebhookParser.BuildPreview(type, content),
                IncrementUnread = false
            });
        }

        private async Task HandleFollowAsync(ZaloOAWebhookEvent evt, ZaloOAWebhookEventRow row, string oaId, CancellationToken ct)
        {
            var isFollow = evt.Kind == ZaloOAWebhookKind.Follow;
            var customer = await _customers.EnsureCustomerAsync(oaId, evt.CustomerUserId!, evt.UserIdByApp,
                isFollow ? evt.OccurredAtUtc : null, cancellationToken: ct);
            await _repo.SetCustomerFollowerAsync(customer.Id, isFollow);

            var conversation = await _repo.GetOrCreateConversationAsync(oaId, customer.Id, reopenIfClosed: false);
            await SaveSystemMessageAsync(row, conversation, customer,
                isFollow ? "Khách đã quan tâm OA" : "Khách đã bỏ quan tâm OA", evt.OccurredAtUtc, incrementUnread: false);
        }

        private async Task HandleSubmitInfoAsync(ZaloOAWebhookEvent evt, ZaloOAWebhookEventRow row, string oaId, CancellationToken ct)
        {
            var customer = await _customers.EnsureCustomerAsync(oaId, evt.CustomerUserId!, evt.UserIdByApp, evt.OccurredAtUtc,
                syncProfile: false, cancellationToken: ct);

            string? name = null, phone = null, dob = null, gender = null, address = null;
            if (evt.Info is { ValueKind: JsonValueKind.Object } info)
            {
                name = ZaloOAClient.GetString(info, "name");
                phone = ZaloOAClient.GetString(info, "phone");
                dob = ZaloOAClient.GetString(info, "user_dob");
                gender = ZaloOAClient.GetString(info, "gender");
                address = ZaloOAClient.GetString(info, "address");
                if (info.TryGetProperty("full_address", out var fa) && fa.ValueKind == JsonValueKind.Object)
                {
                    var parts = new[] { ZaloOAClient.GetString(fa, "user_address"), ZaloOAClient.GetString(fa, "user_ward"), ZaloOAClient.GetString(fa, "user_city") }
                        .Where(x => !string.IsNullOrWhiteSpace(x));
                    var full = string.Join(", ", parts);
                    if (!string.IsNullOrWhiteSpace(full)) address = full;
                }
            }

            await _repo.UpdateCustomerSharedInfoAsync(customer.Id, name, phone, address, dob, gender);

            var conversation = await _repo.GetOrCreateConversationAsync(oaId, customer.Id, reopenIfClosed: true);
            var lines = new[] { ("Tên", name), ("SĐT", phone), ("Ngày sinh", dob), ("Giới tính", gender), ("Địa chỉ", address) }
                .Where(x => !string.IsNullOrWhiteSpace(x.Item2))
                .Select(x => $"{x.Item1}: {x.Item2}");
            await SaveSystemMessageAsync(row, conversation, customer,
                "Khách đã gửi thông tin. " + string.Join("; ", lines), evt.OccurredAtUtc, incrementUnread: true);
        }

        /// <summary>Tin hệ thống dùng ZaloMessageId giả "evt:{eventId}" để xử lý lại event không tạo tin trùng.</summary>
        private Task<ZaloOAUpsertResult> SaveSystemMessageAsync(ZaloOAWebhookEventRow row, ZaloOAConversation conversation,
            ZaloOACustomer customer, string text, DateTime at, bool incrementUnread)
            => _repo.UpsertMessageAsync(new ZaloOAMessageUpsert
            {
                ConversationId = conversation.Id,
                CustomerId = customer.Id,
                Direction = ZaloOADirection.System,
                SenderType = ZaloOASenderType.System,
                MessageType = ZaloOAMessageType.Event,
                Content = text,
                ZaloMessageId = "evt:" + row.Id,
                Status = ZaloOAMessageStatus.Received,
                SentAt = at,
                ReceivedAt = row.ReceivedAt,
                WebhookEventId = row.Id,
                Preview = ZaloOAWebhookParser.BuildPreview(ZaloOAMessageType.Event, text),
                IncrementUnread = incrementUnread
            });
    }
}
