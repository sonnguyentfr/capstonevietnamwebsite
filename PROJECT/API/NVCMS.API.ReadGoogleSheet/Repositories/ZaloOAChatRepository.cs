using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    /// <summary>
    /// Truy cập bảng ZaloOA_* trên DefaultConnection (CapstoneVietNamWeb) qua stored procedure
    /// (Sql/Migrations/20260923_001_ZaloOAChat.sql). Mọi thao tác chống trùng nằm trong SP.
    /// </summary>
    public interface IZaloOAChatRepository
    {
        // Customer
        Task<ZaloOACustomer> UpsertCustomerAsync(string oaId, string zaloUserId, string? zaloUserIdByApp, DateTime? interactionAt,
            string? displayName = null, string? avatarUrl = null);
        Task UpdateCustomerProfileAsync(long id, ZaloOAUserDetail detail);
        Task UpdateCustomerSharedInfoAsync(long id, string? name, string? phone, string? address, string? dob, string? gender);
        Task SetCustomerFollowerAsync(long id, bool isFollower);
        Task<ZaloOACustomer?> GetCustomerByIdAsync(long id);
        Task<ZaloOAPagedResult<ZaloOACustomer>> GetCustomersAsync(ZaloOACustomerQuery query);

        // Conversation
        Task<ZaloOAConversation> GetOrCreateConversationAsync(string oaId, long customerId, bool reopenIfClosed);
        Task<ZaloOAConversation?> GetConversationByIdAsync(long id);
        Task<ZaloOAPagedResult<ZaloOAConversation>> GetConversationsAsync(ZaloOAConversationQuery query);
        Task<bool> MarkConversationReadAsync(long id, int userId);
        Task<bool> SetConversationStatusAsync(long id, string status, int userId);
        Task<ZaloOAUnreadSummary> GetUnreadSummaryAsync(long afterMessageId, int top);

        // Message
        Task<ZaloOAUpsertResult> UpsertMessageAsync(ZaloOAMessageUpsert message);
        Task<ZaloOAMessage> InsertPendingMessageAsync(ZaloOAPendingMessage message);
        Task<ZaloOAMessage?> MarkMessageSentAsync(long id, string zaloMessageId, DateTime? sentAt, string? responseJson);
        Task<ZaloOAMessage?> MarkMessageFailedAsync(long id, int? errorCode, string? errorMessage, string? responseJson);
        Task<ZaloOAMessage?> ResetMessageForRetryAsync(long id, int? agentUserId);
        Task<int> UpdateMessageStatusByZaloIdAsync(string zaloMessageId, string status, DateTime at);
        Task<ZaloOAMessage?> GetMessageByIdAsync(long id);
        Task<List<ZaloOAMessage>> GetMessagesAsync(long conversationId, long beforeId, int pageSize);
        Task<List<ZaloOAMessage>> GetMessageChangesAsync(long conversationId, DateTime since, int top);

        // Webhook
        Task<ZaloOAUpsertResult> InsertWebhookEventAsync(string dedupKey, string eventName, string? oaId, string? zaloUserId,
            string? zaloMessageId, long? eventTimestamp, string rawPayload, bool signatureValid);
        Task<ZaloOAWebhookEventRow?> GetWebhookEventAsync(long id);
        Task SetWebhookEventStatusAsync(long id, string status, string? errorMessage);
        Task<List<long>> GetWebhookEventsForReprocessAsync(int staleMinutes, int maxRetry, int top);
    }

    public class ZaloOAChatRepository : IZaloOAChatRepository
    {
        private readonly string _connStr;

        public ZaloOAChatRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("DefaultConnection not configured");
        }

        private SqlConnection Open() => new SqlConnection(_connStr);

        private const CommandType Sp = CommandType.StoredProcedure;

        // ── Customer ────────────────────────────────────────────────────────

        public async Task<ZaloOACustomer> UpsertCustomerAsync(string oaId, string zaloUserId, string? zaloUserIdByApp, DateTime? interactionAt,
            string? displayName = null, string? avatarUrl = null)
        {
            using var conn = Open();
            return await conn.QuerySingleAsync<ZaloOACustomer>("ZaloOA_Customer_Upsert",
                new
                {
                    OAId = oaId,
                    ZaloUserId = zaloUserId,
                    ZaloUserIdByApp = zaloUserIdByApp,
                    InteractionAt = interactionAt,
                    DisplayName = Truncate(displayName, 250),
                    AvatarUrl = Truncate(avatarUrl, 1000)
                },
                commandType: Sp);
        }

        public async Task UpdateCustomerProfileAsync(long id, ZaloOAUserDetail d)
        {
            using var conn = Open();
            await conn.ExecuteAsync("ZaloOA_Customer_UpdateProfile",
                new
                {
                    Id = id,
                    ZaloUserIdByApp = d.UserIdByApp,
                    DisplayName = Truncate(d.DisplayName, 250),
                    UserAlias = Truncate(d.UserAlias, 250),
                    AvatarUrl = Truncate(d.Avatar, 1000),
                    d.IsFollower,
                    d.IsSensitive,
                    SharedName = Truncate(d.SharedName, 250),
                    SharedPhone = Truncate(d.SharedPhone, 30),
                    SharedAddress = Truncate(d.SharedAddress, 500),
                    SharedDob = Truncate(d.SharedDob, 20),
                    TagsJson = d.TagNames.Count > 0 ? System.Text.Json.JsonSerializer.Serialize(d.TagNames) : null,
                    NotesJson = d.Notes.Count > 0 ? System.Text.Json.JsonSerializer.Serialize(d.Notes) : null,
                    ProfileRawJson = d.RawJson
                },
                commandType: Sp);
        }

        public async Task UpdateCustomerSharedInfoAsync(long id, string? name, string? phone, string? address, string? dob, string? gender)
        {
            using var conn = Open();
            await conn.ExecuteAsync("ZaloOA_Customer_UpdateSharedInfo",
                new
                {
                    Id = id,
                    SharedName = Truncate(name, 250),
                    SharedPhone = Truncate(phone, 30),
                    SharedAddress = Truncate(address, 500),
                    SharedDob = Truncate(dob, 20),
                    Gender = Truncate(gender, 20)
                },
                commandType: Sp);
        }

        public async Task SetCustomerFollowerAsync(long id, bool isFollower)
        {
            using var conn = Open();
            await conn.ExecuteAsync("ZaloOA_Customer_SetFollower", new { Id = id, IsFollower = isFollower }, commandType: Sp);
        }

        public async Task<ZaloOACustomer?> GetCustomerByIdAsync(long id)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<ZaloOACustomer>("ZaloOA_Customer_GetById", new { Id = id }, commandType: Sp);
        }

        public async Task<ZaloOAPagedResult<ZaloOACustomer>> GetCustomersAsync(ZaloOACustomerQuery q)
        {
            using var conn = Open();
            var rows = (await conn.QueryAsync<CustomerRow>("ZaloOA_Customer_GetList",
                new
                {
                    Keyword = EscapeLike(q.Keyword),
                    q.IsFollower,
                    q.FromDate,
                    q.ToDate,
                    SortBy = q.SortBy,
                    SortDir = q.SortDir,
                    q.PageIndex,
                    q.PageSize
                },
                commandType: Sp)).ToList();

            return new ZaloOAPagedResult<ZaloOACustomer>
            {
                Items = rows.Cast<ZaloOACustomer>().ToList(),
                TotalRecords = rows.FirstOrDefault()?.TotalRecords ?? 0
            };
        }

        // ── Conversation ────────────────────────────────────────────────────

        public async Task<ZaloOAConversation> GetOrCreateConversationAsync(string oaId, long customerId, bool reopenIfClosed)
        {
            using var conn = Open();
            return await conn.QuerySingleAsync<ZaloOAConversation>("ZaloOA_Conversation_GetOrCreate",
                new { OAId = oaId, CustomerId = customerId, ReopenIfClosed = reopenIfClosed }, commandType: Sp);
        }

        public async Task<ZaloOAConversation?> GetConversationByIdAsync(long id)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<ZaloOAConversation>("ZaloOA_Conversation_GetById", new { Id = id }, commandType: Sp);
        }

        public async Task<ZaloOAPagedResult<ZaloOAConversation>> GetConversationsAsync(ZaloOAConversationQuery q)
        {
            using var conn = Open();
            var rows = (await conn.QueryAsync<ConversationRow>("ZaloOA_Conversation_GetList",
                new
                {
                    Keyword = EscapeLike(q.Keyword),
                    Status = q.Status ?? "",
                    AssignedToUserId = q.AssignedTo,
                    q.UnreadOnly,
                    q.FromDate,
                    q.ToDate,
                    SortBy = q.SortBy,
                    SortDir = q.SortDir,
                    q.PageIndex,
                    q.PageSize
                },
                commandType: Sp)).ToList();

            return new ZaloOAPagedResult<ZaloOAConversation>
            {
                Items = rows.Cast<ZaloOAConversation>().ToList(),
                TotalRecords = rows.FirstOrDefault()?.TotalRecords ?? 0
            };
        }

        public async Task<bool> MarkConversationReadAsync(long id, int userId)
        {
            using var conn = Open();
            return await conn.ExecuteScalarAsync<int>("ZaloOA_Conversation_MarkRead", new { Id = id, UserId = userId }, commandType: Sp) > 0;
        }

        public async Task<bool> SetConversationStatusAsync(long id, string status, int userId)
        {
            using var conn = Open();
            return await conn.ExecuteScalarAsync<int>("ZaloOA_Conversation_SetStatus",
                new { Id = id, Status = status, UserId = userId }, commandType: Sp) > 0;
        }

        public async Task<ZaloOAUnreadSummary> GetUnreadSummaryAsync(long afterMessageId, int top)
        {
            using var conn = Open();
            using var multi = await conn.QueryMultipleAsync("ZaloOA_Conversation_GetUnreadSummary",
                new { AfterMessageId = afterMessageId, Top = top }, commandType: Sp);

            var summary = await multi.ReadSingleAsync<ZaloOAUnreadSummary>();
            if (afterMessageId > 0 && !multi.IsConsumed)
                summary.NewMessages = (await multi.ReadAsync<ZaloOANewMessageNotice>()).ToList();
            return summary;
        }

        // ── Message ─────────────────────────────────────────────────────────

        public async Task<ZaloOAUpsertResult> UpsertMessageAsync(ZaloOAMessageUpsert m)
        {
            using var conn = Open();
            return await conn.QuerySingleAsync<ZaloOAUpsertResult>("ZaloOA_Message_Upsert",
                new
                {
                    m.ConversationId,
                    m.CustomerId,
                    m.Direction,
                    m.SenderType,
                    m.SenderId,
                    m.ReceiverId,
                    m.MessageType,
                    m.Content,
                    m.AttachmentsJson,
                    m.QuoteZaloMessageId,
                    m.ZaloMessageId,
                    m.Status,
                    m.SentAt,
                    m.ReceivedAt,
                    m.WebhookEventId,
                    m.RawPayload,
                    Preview = Truncate(m.Preview, 500),
                    m.IncrementUnread
                },
                commandType: Sp);
        }

        public async Task<ZaloOAMessage> InsertPendingMessageAsync(ZaloOAPendingMessage m)
        {
            using var conn = Open();
            return await conn.QuerySingleAsync<ZaloOAMessage>("ZaloOA_Message_InsertPending",
                new
                {
                    m.ConversationId,
                    m.CustomerId,
                    m.SenderId,
                    m.ReceiverId,
                    m.AgentUserId,
                    m.MessageType,
                    m.Content,
                    m.AttachmentsJson,
                    m.ClientMessageId,
                    Preview = Truncate(m.Preview, 500)
                },
                commandType: Sp);
        }

        public async Task<ZaloOAMessage?> MarkMessageSentAsync(long id, string zaloMessageId, DateTime? sentAt, string? responseJson)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<ZaloOAMessage>("ZaloOA_Message_MarkSent",
                new { Id = id, ZaloMessageId = zaloMessageId, SentAt = sentAt, ResponseJson = responseJson }, commandType: Sp);
        }

        public async Task<ZaloOAMessage?> MarkMessageFailedAsync(long id, int? errorCode, string? errorMessage, string? responseJson)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<ZaloOAMessage>("ZaloOA_Message_MarkFailed",
                new { Id = id, ErrorCode = errorCode, ErrorMessage = Truncate(errorMessage, 2000), ResponseJson = responseJson },
                commandType: Sp);
        }

        public async Task<ZaloOAMessage?> ResetMessageForRetryAsync(long id, int? agentUserId)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<ZaloOAMessage>("ZaloOA_Message_ResetForRetry",
                new { Id = id, AgentUserId = agentUserId }, commandType: Sp);
        }

        public async Task<int> UpdateMessageStatusByZaloIdAsync(string zaloMessageId, string status, DateTime at)
        {
            using var conn = Open();
            return await conn.ExecuteScalarAsync<int>("ZaloOA_Message_UpdateStatusByZaloId",
                new { ZaloMessageId = zaloMessageId, Status = status, At = at }, commandType: Sp);
        }

        public async Task<ZaloOAMessage?> GetMessageByIdAsync(long id)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<ZaloOAMessage>("ZaloOA_Message_GetById", new { Id = id }, commandType: Sp);
        }

        public async Task<List<ZaloOAMessage>> GetMessagesAsync(long conversationId, long beforeId, int pageSize)
        {
            using var conn = Open();
            return (await conn.QueryAsync<ZaloOAMessage>("ZaloOA_Message_GetByConversation",
                new { ConversationId = conversationId, BeforeId = beforeId, PageSize = pageSize }, commandType: Sp)).ToList();
        }

        public async Task<List<ZaloOAMessage>> GetMessageChangesAsync(long conversationId, DateTime since, int top)
        {
            using var conn = Open();
            return (await conn.QueryAsync<ZaloOAMessage>("ZaloOA_Message_GetChanges",
                new { ConversationId = conversationId, Since = since, Top = top }, commandType: Sp)).ToList();
        }

        // ── Webhook ─────────────────────────────────────────────────────────

        public async Task<ZaloOAUpsertResult> InsertWebhookEventAsync(string dedupKey, string eventName, string? oaId, string? zaloUserId,
            string? zaloMessageId, long? eventTimestamp, string rawPayload, bool signatureValid)
        {
            using var conn = Open();
            return await conn.QuerySingleAsync<ZaloOAUpsertResult>("ZaloOA_WebhookEvent_Insert",
                new
                {
                    DedupKey = dedupKey,
                    EventName = Truncate(eventName, 100),
                    OAId = Truncate(oaId, 50),
                    ZaloUserId = Truncate(zaloUserId, 50),
                    ZaloMessageId = Truncate(zaloMessageId, 100),
                    EventTimestamp = eventTimestamp,
                    RawPayload = rawPayload,
                    SignatureValid = signatureValid
                },
                commandType: Sp);
        }

        public async Task<ZaloOAWebhookEventRow?> GetWebhookEventAsync(long id)
        {
            using var conn = Open();
            return await conn.QueryFirstOrDefaultAsync<ZaloOAWebhookEventRow>("ZaloOA_WebhookEvent_GetById", new { Id = id }, commandType: Sp);
        }

        public async Task SetWebhookEventStatusAsync(long id, string status, string? errorMessage)
        {
            using var conn = Open();
            await conn.ExecuteAsync("ZaloOA_WebhookEvent_SetStatus",
                new { Id = id, Status = status, ErrorMessage = Truncate(errorMessage, 2000) }, commandType: Sp);
        }

        public async Task<List<long>> GetWebhookEventsForReprocessAsync(int staleMinutes, int maxRetry, int top)
        {
            using var conn = Open();
            return (await conn.QueryAsync<long>("ZaloOA_WebhookEvent_GetForReprocess",
                new { StaleMinutes = staleMinutes, MaxRetry = maxRetry, Top = top }, commandType: Sp)).ToList();
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        /// <summary>Escape ký tự đặc biệt của LIKE để từ khoá tìm kiếm được hiểu nguyên văn.</summary>
        internal static string EscapeLike(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return "";
            var k = keyword.Trim();
            if (k.Length > 200) k = k[..200];
            return k.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
        }

        private static string? Truncate(string? s, int max) => s == null || s.Length <= max ? s : s[..max];

        private class CustomerRow : ZaloOACustomer { public int TotalRecords { get; set; } }
        private class ConversationRow : ZaloOAConversation { public int TotalRecords { get; set; } }
    }
}
