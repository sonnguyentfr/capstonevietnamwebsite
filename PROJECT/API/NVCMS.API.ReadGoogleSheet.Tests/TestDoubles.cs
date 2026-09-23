using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Http;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Locking;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Security;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;
using System.Net;
using System.Text;

namespace NVCMS.API.ReadGoogleSheet.Tests;

/// <summary>HttpMessageHandler giả: trả response theo hàm, ghi lại request.</summary>
public class FakeHttpHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, string?, HttpResponseMessage> _respond;
    public List<(HttpRequestMessage Request, string? Body)> Calls { get; } = new();

    public FakeHttpHandler(Func<HttpRequestMessage, string?, HttpResponseMessage> respond) => _respond = respond;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        Calls.Add((request, body));
        return _respond(request, body);
    }

    public static HttpResponseMessage Json(string json, HttpStatusCode status = HttpStatusCode.OK)
        => new(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    public static BaseApi CreateApi(FakeHttpHandler handler) => new(new HttpClient(handler), NullLogger<BaseApi>.Instance);
}

public class InMemoryTokenRepository : IZaloTokenRepository
{
    public List<Zalo_Token> Rows { get; } = new();

    public Task AddAsync(Zalo_Token token)
    {
        if (!Rows.Contains(token))
        {
            token.Id = Rows.Count + 1;
            Rows.Add(token);
        }
        return Task.CompletedTask;
    }

    public Task<Zalo_Token> GetLastAsync()
    {
        var last = Rows.OrderByDescending(r => r.Id).FirstOrDefault()
                   ?? throw new InvalidOperationException("No ZaloToken found.");
        return Task.FromResult(last);
    }
}

public class FakeRefreshLock : IZaloTokenRefreshLock
{
    public int Acquired { get; private set; }

    public Task<IAsyncDisposable> AcquireAsync(CancellationToken cancellationToken = default)
    {
        Acquired++;
        return Task.FromResult<IAsyncDisposable>(new Noop());
    }

    private sealed class Noop : IAsyncDisposable
    {
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

public class FakeColumnInspector : IZaloTokenColumnInspector
{
    public int? MaxChars { get; set; }
    public Task<int?> GetMaxCharsAsync(string columnName) => Task.FromResult(MaxChars);
}

/// <summary>IZaloService giả cho test ZaloOAClient: trả token cố định, ghi nhận forceRefresh.</summary>
public class FakeTokenService : IZaloService
{
    public List<bool> ForceRefreshCalls { get; } = new();
    public Exception? ThrowOnGet { get; set; }

    public Task<string> GetValidAccessTokenAsync(bool forceRefresh = false, CancellationToken cancellationToken = default)
    {
        ForceRefreshCalls.Add(forceRefresh);
        if (ThrowOnGet != null) throw ThrowOnGet;
        return Task.FromResult(forceRefresh ? "token-2" : "token-1");
    }

    public Task<ZaloTokenResponse> GetAndSaveTokenAsync(string code) => throw new NotSupportedException();
    public Task<ZaloTokenResponse> RefreshAndSaveTokenAsync(string refresh_token) => throw new NotSupportedException();
    public Task<Zalo_Token> GetLastTokenAsync() => throw new NotSupportedException();
    public Task<ZaloMessageResponse> SendTemplateAsync<T>(ZaloMessageRequest<T> request) => throw new NotSupportedException();
    public Task<ZaloTokenStatus?> GetTokenStatusAsync() => Task.FromResult<ZaloTokenStatus?>(null);
}

/// <summary>IZaloOAClient giả: mỗi hàm cấu hình được kết quả.</summary>
public class FakeZaloOAClient : IZaloOAClient
{
    public Func<ZaloOAApiResult<ZaloOASendResult>> SendResult { get; set; } =
        () => new ZaloOAApiResult<ZaloOASendResult> { Data = new ZaloOASendResult { MessageId = "zmsg-1", SentTime = DateTime.UtcNow } };
    public Func<string, ZaloOAApiResult<ZaloOAUserDetail>> UserDetailResult { get; set; } =
        id => new ZaloOAApiResult<ZaloOAUserDetail> { Data = new ZaloOAUserDetail { UserId = id, DisplayName = "Khách " + id } };

    public List<string> SentTexts { get; } = new();
    public List<string> UserDetailCalls { get; } = new();

    public Task<ZaloOAApiResult<ZaloOASendResult>> SendTextAsync(string userId, string text, CancellationToken cancellationToken = default)
    {
        SentTexts.Add(text);
        return Task.FromResult(SendResult());
    }

    public Task<ZaloOAApiResult<ZaloOASendResult>> SendImageAsync(string userId, string? attachmentId, string? imageUrl, string? caption, CancellationToken cancellationToken = default)
        => Task.FromResult(SendResult());

    public Task<ZaloOAApiResult<ZaloOASendResult>> SendFileAsync(string userId, string fileToken, CancellationToken cancellationToken = default)
        => Task.FromResult(SendResult());

    public Task<ZaloOAApiResult<ZaloOASendResult>> SendMessageAsync(string userId, object message, CancellationToken cancellationToken = default)
        => Task.FromResult(SendResult());

    public Task<ZaloOAApiResult<string>> UploadImageAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
        => Task.FromResult(new ZaloOAApiResult<string> { Data = "att-1" });

    public Task<ZaloOAApiResult<string>> UploadFileAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
        => Task.FromResult(new ZaloOAApiResult<string> { Data = "file-token-1" });

    public Task<ZaloOAApiResult<ZaloOAUserDetail>> GetUserDetailAsync(string userId, CancellationToken cancellationToken = default)
    {
        UserDetailCalls.Add(userId);
        return Task.FromResult(UserDetailResult(userId));
    }

    public Task<ZaloOAApiResult<ZaloOAInfo>> GetOAInfoAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new ZaloOAApiResult<ZaloOAInfo> { Data = new ZaloOAInfo { OAId = "OA1" } });

    public Task<ZaloOAApiResult<List<ZaloOAHistoryMessage>>> GetRecentChatsAsync(int offset, int count, CancellationToken cancellationToken = default)
        => Task.FromResult(new ZaloOAApiResult<List<ZaloOAHistoryMessage>> { Data = new() });

    public Task<ZaloOAApiResult<List<ZaloOAHistoryMessage>>> GetConversationAsync(string userId, int offset, int count, CancellationToken cancellationToken = default)
        => Task.FromResult(new ZaloOAApiResult<List<ZaloOAHistoryMessage>> { Data = new() });
}

/// <summary>
/// Repository giả trong bộ nhớ, mô phỏng đúng các quy tắc chống trùng của stored procedure
/// (UX ZaloMessageId, UX ClientMessageId, UX DedupKey, 1 hội thoại / khách).
/// </summary>
public class FakeChatRepository : IZaloOAChatRepository
{
    public List<ZaloOACustomer> Customers { get; } = new();
    public List<ZaloOAConversation> Conversations { get; } = new();
    public List<ZaloOAMessage> Messages { get; } = new();
    public List<ZaloOAWebhookEventRow> Events { get; } = new();
    public List<(long Id, string Status, string? Error)> EventStatusChanges { get; } = new();
    public List<ZaloOAMessageUpsert> Upserts { get; } = new();

    public Task<ZaloOACustomer> UpsertCustomerAsync(string oaId, string zaloUserId, string? zaloUserIdByApp, DateTime? interactionAt,
        string? displayName = null, string? avatarUrl = null)
    {
        var c = Customers.FirstOrDefault(x => x.OAId == oaId && x.ZaloUserId == zaloUserId);
        var isNew = c == null;
        if (c == null)
        {
            c = new ZaloOACustomer { Id = Customers.Count + 1, OAId = oaId, ZaloUserId = zaloUserId, DisplayName = displayName, Status = "ACTIVE" };
            Customers.Add(c);
        }
        if (interactionAt.HasValue && (c.LastInteractionAt == null || interactionAt > c.LastInteractionAt))
            c.LastInteractionAt = interactionAt;
        c.IsNew = isNew;
        return Task.FromResult(c);
    }

    public Task UpdateCustomerProfileAsync(long id, ZaloOAUserDetail detail)
    {
        var c = Customers.Single(x => x.Id == id);
        c.DisplayName = detail.DisplayName ?? c.DisplayName;
        c.LastProfileSyncAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    public Task UpdateCustomerSharedInfoAsync(long id, string? name, string? phone, string? address, string? dob, string? gender)
    {
        var c = Customers.Single(x => x.Id == id);
        c.SharedName = name ?? c.SharedName; c.SharedPhone = phone ?? c.SharedPhone; c.Gender = gender ?? c.Gender;
        return Task.CompletedTask;
    }

    public Task SetCustomerFollowerAsync(long id, bool isFollower)
    {
        Customers.Single(x => x.Id == id).IsFollower = isFollower;
        return Task.CompletedTask;
    }

    public Task<ZaloOACustomer?> GetCustomerByIdAsync(long id) => Task.FromResult(Customers.FirstOrDefault(x => x.Id == id));

    public Task<ZaloOAPagedResult<ZaloOACustomer>> GetCustomersAsync(ZaloOACustomerQuery query)
        => Task.FromResult(new ZaloOAPagedResult<ZaloOACustomer> { Items = Customers.ToList(), TotalRecords = Customers.Count });

    public Task<ZaloOAConversation> GetOrCreateConversationAsync(string oaId, long customerId, bool reopenIfClosed)
    {
        var c = Conversations.FirstOrDefault(x => x.CustomerId == customerId);
        if (c == null)
        {
            c = new ZaloOAConversation { Id = Conversations.Count + 1, OAId = oaId, CustomerId = customerId, Status = "OPEN", IsNew = true };
            Conversations.Add(c);
        }
        else if (reopenIfClosed && c.Status == "CLOSED")
        {
            c.Status = "OPEN";
        }
        return Task.FromResult(c);
    }

    public Task<ZaloOAConversation?> GetConversationByIdAsync(long id)
    {
        var c = Conversations.FirstOrDefault(x => x.Id == id);
        if (c != null)
            c.ZaloUserId = Customers.FirstOrDefault(x => x.Id == c.CustomerId)?.ZaloUserId;
        return Task.FromResult(c);
    }

    public Task<ZaloOAPagedResult<ZaloOAConversation>> GetConversationsAsync(ZaloOAConversationQuery query)
        => Task.FromResult(new ZaloOAPagedResult<ZaloOAConversation> { Items = Conversations.ToList(), TotalRecords = Conversations.Count });

    public Task<bool> MarkConversationReadAsync(long id, int userId)
    {
        var c = Conversations.FirstOrDefault(x => x.Id == id);
        if (c != null) c.UnreadCount = 0;
        return Task.FromResult(c != null);
    }

    public Task<bool> SetConversationStatusAsync(long id, string status, int userId)
    {
        var c = Conversations.FirstOrDefault(x => x.Id == id);
        if (c != null) c.Status = status;
        return Task.FromResult(c != null);
    }

    public Task<ZaloOAUnreadSummary> GetUnreadSummaryAsync(long afterMessageId, int top)
        => Task.FromResult(new ZaloOAUnreadSummary { TotalUnread = Conversations.Sum(x => x.UnreadCount) });

    public Task<ZaloOAUpsertResult> UpsertMessageAsync(ZaloOAMessageUpsert m)
    {
        Upserts.Add(m);
        var existing = m.ZaloMessageId == null ? null : Messages.FirstOrDefault(x => x.ZaloMessageId == m.ZaloMessageId);
        if (existing != null)
            return Task.FromResult(new ZaloOAUpsertResult { Id = existing.Id, IsDuplicate = true });

        var msg = new ZaloOAMessage
        {
            Id = Messages.Count + 1, ConversationId = m.ConversationId, CustomerId = m.CustomerId, Direction = m.Direction,
            SenderType = m.SenderType, MessageType = m.MessageType, Content = m.Content, ZaloMessageId = m.ZaloMessageId,
            Status = m.Status, SentAt = m.SentAt
        };
        Messages.Add(msg);
        if (m.IncrementUnread)
            Conversations.Single(x => x.Id == m.ConversationId).UnreadCount++;
        return Task.FromResult(new ZaloOAUpsertResult { Id = msg.Id });
    }

    public Task<ZaloOAMessage> InsertPendingMessageAsync(ZaloOAPendingMessage m)
    {
        var existing = Messages.FirstOrDefault(x => x.ClientMessageId == m.ClientMessageId);
        if (existing != null)
        {
            existing.IsDuplicate = true;
            return Task.FromResult(existing);
        }
        var msg = new ZaloOAMessage
        {
            Id = Messages.Count + 1, ConversationId = m.ConversationId, CustomerId = m.CustomerId, Direction = "OUT",
            SenderType = "AGENT", AgentUserId = m.AgentUserId, MessageType = m.MessageType, Content = m.Content,
            AttachmentsJson = m.AttachmentsJson, ClientMessageId = m.ClientMessageId, Status = "PENDING", SentAt = DateTime.UtcNow
        };
        Messages.Add(msg);
        return Task.FromResult(msg);
    }

    public Task<ZaloOAMessage?> MarkMessageSentAsync(long id, string zaloMessageId, DateTime? sentAt, string? responseJson)
    {
        var m = Messages.Single(x => x.Id == id);
        m.ZaloMessageId = zaloMessageId; m.Status = "SENT"; m.ErrorCode = null; m.ErrorMessage = null;
        return Task.FromResult<ZaloOAMessage?>(m);
    }

    public Task<ZaloOAMessage?> MarkMessageFailedAsync(long id, int? errorCode, string? errorMessage, string? responseJson)
    {
        var m = Messages.Single(x => x.Id == id);
        m.Status = "FAILED"; m.ErrorCode = errorCode; m.ErrorMessage = errorMessage;
        return Task.FromResult<ZaloOAMessage?>(m);
    }

    public Task<ZaloOAMessage?> ResetMessageForRetryAsync(long id, int? agentUserId)
    {
        var m = Messages.FirstOrDefault(x => x.Id == id && x.Status == "FAILED" && x.Direction == "OUT");
        if (m != null) m.Status = "PENDING";
        return Task.FromResult(m);
    }

    public Task<int> UpdateMessageStatusByZaloIdAsync(string zaloMessageId, string status, DateTime at)
    {
        var m = Messages.FirstOrDefault(x => x.ZaloMessageId == zaloMessageId);
        if (m != null) m.Status = status;
        return Task.FromResult(m == null ? 0 : 1);
    }

    public Task<ZaloOAMessage?> GetMessageByIdAsync(long id) => Task.FromResult(Messages.FirstOrDefault(x => x.Id == id));

    public Task<List<ZaloOAMessage>> GetMessagesAsync(long conversationId, long beforeId, int pageSize)
        => Task.FromResult(Messages.Where(x => x.ConversationId == conversationId).OrderByDescending(x => x.SentAt).Take(pageSize).ToList());

    public Task<List<ZaloOAMessage>> GetMessageChangesAsync(long conversationId, DateTime since, int top)
        => Task.FromResult(Messages.Where(x => x.ConversationId == conversationId).ToList());

    public Task<ZaloOAUpsertResult> InsertWebhookEventAsync(string dedupKey, string eventName, string? oaId, string? zaloUserId,
        string? zaloMessageId, long? eventTimestamp, string rawPayload, bool signatureValid)
    {
        var e = Events.FirstOrDefault(x => x.DedupKey == dedupKey);
        if (e != null)
            return Task.FromResult(new ZaloOAUpsertResult { Id = e.Id, IsDuplicate = true, ProcessStatus = e.ProcessStatus });

        e = new ZaloOAWebhookEventRow
        {
            Id = Events.Count + 1, DedupKey = dedupKey, EventName = eventName, OAId = oaId, ZaloUserId = zaloUserId,
            ZaloMessageId = zaloMessageId, EventTimestamp = eventTimestamp, RawPayload = rawPayload,
            SignatureValid = signatureValid, ProcessStatus = "PENDING", ReceivedAt = DateTime.UtcNow
        };
        Events.Add(e);
        return Task.FromResult(new ZaloOAUpsertResult { Id = e.Id, ProcessStatus = "PENDING" });
    }

    public Task<ZaloOAWebhookEventRow?> GetWebhookEventAsync(long id) => Task.FromResult(Events.FirstOrDefault(x => x.Id == id));

    public Task SetWebhookEventStatusAsync(long id, string status, string? errorMessage)
    {
        EventStatusChanges.Add((id, status, errorMessage));
        var e = Events.Single(x => x.Id == id);
        e.ProcessStatus = status;
        return Task.CompletedTask;
    }

    public Task<List<long>> GetWebhookEventsForReprocessAsync(int staleMinutes, int maxRetry, int top)
        => Task.FromResult(Events.Where(x => x.ProcessStatus is "PENDING" or "FAILED").Select(x => x.Id).ToList());
}

/// <summary>IBackgroundJobClient giả: ghi lại job được enqueue.</summary>
public class FakeJobClient : IBackgroundJobClient
{
    public List<Job> Created { get; } = new();
    public bool Throw { get; set; }

    public string Create(Job job, IState state)
    {
        if (Throw) throw new InvalidOperationException("Hangfire storage down");
        Created.Add(job);
        return Created.Count.ToString();
    }

    public bool ChangeState(string jobId, IState state, string expectedState) => true;
}

public static class TestOptions
{
    public const string AppId = "app-123";
    public const string OASecret = "oa-secret-xyz";

    public static IOptions<ZaloSettings> Zalo(Action<ZaloSettings>? configure = null)
    {
        var s = new ZaloSettings
        {
            TokenEndpoint = "https://oauth.zaloapp.com/v4/oa/access_token",
            AppId = AppId,
            AppSecret = "app-secret",
            OASecretKey = OASecret,
            OAId = "OA1",
            OpenApiBaseUrl = "https://openapi.zalo.me",
            EncryptTokens = true,
            TokenRefreshMarginMinutes = 30
        };
        configure?.Invoke(s);
        return Options.Create(s);
    }

    public static IOptions<ZaloOAChatSettings> Chat(Action<ZaloOAChatSettings>? configure = null)
    {
        var s = new ZaloOAChatSettings();
        configure?.Invoke(s);
        return Options.Create(s);
    }

    public static IZaloTokenProtector Protector() => new ZaloTokenProtector(new EphemeralDataProtectionProvider());
}
