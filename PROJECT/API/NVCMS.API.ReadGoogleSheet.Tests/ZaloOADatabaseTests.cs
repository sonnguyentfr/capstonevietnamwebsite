using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Repositories;
using System.Text.RegularExpressions;
using Xunit;

namespace NVCMS.API.ReadGoogleSheet.Tests;

/// <summary>
/// Tạo DB tạm trên SQL LocalDB, chạy migration thật (Sql/Migrations/20260923_001_ZaloOAChat.sql) HAI lần
/// (kiểm tra script chạy lại được), dùng ZaloOAChatRepository thật, xoá DB khi xong.
/// Đổi server bằng biến môi trường ZALOOA_TEST_SQLSERVER. Không có LocalDB → test tự bỏ qua.
/// KHÔNG bao giờ trỏ vào DB production: tên DB luôn là ZaloOAChat_Test_{guid}.
/// </summary>
public sealed class LocalDbFixture : IAsyncLifetime
{
    public string? ConnectionString { get; private set; }
    public string? SkipReason { get; private set; }
    private string _master = "";
    private readonly string _dbName = "ZaloOAChat_Test_" + Guid.NewGuid().ToString("N")[..12];

    public async Task InitializeAsync()
    {
        var server = Environment.GetEnvironmentVariable("ZALOOA_TEST_SQLSERVER") ?? @"(localdb)\MSSQLLocalDB";
        _master = $"Server={server};Database=master;Integrated Security=true;TrustServerCertificate=True;Connect Timeout=15";
        try
        {
            await using var conn = new SqlConnection(_master);
            await conn.OpenAsync();
            await conn.ExecuteAsync($"CREATE DATABASE [{_dbName}]");
        }
        catch (Exception ex)
        {
            SkipReason = "SQL LocalDB không khả dụng: " + ex.Message;
            return;
        }

        ConnectionString = $"Server={server};Database={_dbName};Integrated Security=true;TrustServerCertificate=True";
        var script = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Sql", "20260923_001_ZaloOAChat.sql"));
        await RunScriptAsync(script);
        await RunScriptAsync(script);   // chạy lại phải không lỗi
    }

    private async Task RunScriptAsync(string script)
    {
        await using var conn = new SqlConnection(ConnectionString);
        await conn.OpenAsync();
        foreach (var batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            if (!string.IsNullOrWhiteSpace(batch))
                await conn.ExecuteAsync(batch);
    }

    public async Task DisposeAsync()
    {
        if (ConnectionString == null) return;
        SqlConnection.ClearAllPools();
        await using var conn = new SqlConnection(_master);
        await conn.OpenAsync();
        await conn.ExecuteAsync($"ALTER DATABASE [{_dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{_dbName}];");
    }

    public ZaloOAChatRepository CreateRepository()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = ConnectionString })
            .Build();
        return new ZaloOAChatRepository(config);
    }
}

public class ZaloOADatabaseTests : IClassFixture<LocalDbFixture>
{
    private readonly LocalDbFixture _db;

    public ZaloOADatabaseTests(LocalDbFixture db) => _db = db;

    private ZaloOAChatRepository Repo()
    {
        Skip.If(_db.SkipReason != null, _db.SkipReason);
        return _db.CreateRepository();
    }

    private static string U() => "U" + Guid.NewGuid().ToString("N")[..10];

    [SkippableFact]
    public async Task Customer_Create_Then_Update()
    {
        var repo = Repo();
        var uid = U();

        var created = await repo.UpsertCustomerAsync("OA1", uid, "app-" + uid, new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), "Tên từ lịch sử");
        var again = await repo.UpsertCustomerAsync("OA1", uid, null, new DateTime(2026, 9, 20, 0, 0, 0, DateTimeKind.Utc));
        await repo.UpdateCustomerProfileAsync(created.Id, new ZaloOAUserDetail { DisplayName = "Tên Zalo", SharedPhone = "84900000000", TagNames = { "VIP" } });
        var loaded = await repo.GetCustomerByIdAsync(created.Id);

        Assert.True(created.IsNew);
        Assert.False(again.IsNew);
        Assert.Equal(created.Id, again.Id);
        Assert.Equal("app-" + uid, again.ZaloUserIdByApp);                 // không bị ghi đè bằng null
        Assert.Equal(new DateTime(2026, 9, 1), loaded!.FirstInteractionAt);
        Assert.Equal(new DateTime(2026, 9, 20), loaded.LastInteractionAt);
        Assert.Equal("Tên Zalo", loaded.DisplayName);
        Assert.Equal("84900000000", loaded.SharedPhone);
        Assert.Equal("[\"VIP\"]", loaded.TagsJson);
        Assert.NotNull(loaded.LastProfileSyncAt);
    }

    [SkippableFact]
    public async Task Conversation_OnePerCustomer_ReopenOnlyWhenRequested()
    {
        var repo = Repo();
        var c = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow);

        var first = await repo.GetOrCreateConversationAsync("OA1", c.Id, false);
        var second = await repo.GetOrCreateConversationAsync("OA1", c.Id, false);
        await repo.SetConversationStatusAsync(first.Id, "CLOSED", 5);
        var stillClosed = await repo.GetOrCreateConversationAsync("OA1", c.Id, false);
        var reopened = await repo.GetOrCreateConversationAsync("OA1", c.Id, true);

        Assert.True(first.IsNew);
        Assert.Equal(first.Id, second.Id);
        Assert.Equal("CLOSED", stillClosed.Status);
        Assert.Equal("OPEN", reopened.Status);
        Assert.Null(reopened.ClosedAt);
    }

    [SkippableFact]
    public async Task Message_Save_And_DuplicatePrevention()
    {
        var repo = Repo();
        var c = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow);
        var conv = await repo.GetOrCreateConversationAsync("OA1", c.Id, true);
        var zaloMsgId = "zm-" + Guid.NewGuid().ToString("N");

        ZaloOAMessageUpsert Inbound() => new()
        {
            ConversationId = conv.Id, CustomerId = c.Id, Direction = "IN", SenderType = "CUSTOMER", MessageType = "TEXT",
            Content = "Xin chào 'quote' ; DROP TABLE x --", ZaloMessageId = zaloMsgId, Status = "RECEIVED",
            SentAt = DateTime.UtcNow, Preview = "Xin chào", IncrementUnread = true, RawPayload = "{\"a\":1}"
        };

        var r1 = await repo.UpsertMessageAsync(Inbound());
        var r2 = await repo.UpsertMessageAsync(Inbound());
        var loaded = await repo.GetConversationByIdAsync(conv.Id);
        var messages = await repo.GetMessagesAsync(conv.Id, 0, 30);

        Assert.False(r1.IsDuplicate);
        Assert.True(r2.IsDuplicate);
        Assert.Equal(r1.Id, r2.Id);
        Assert.Single(messages);
        Assert.Equal("Xin chào 'quote' ; DROP TABLE x --", messages[0].Content);   // tham số hoá, không bị SQL injection
        Assert.Equal(1, loaded!.UnreadCount);                                      // retry không tăng chưa đọc
        Assert.Equal(r1.Id, loaded.LastMessageId);
        Assert.Equal("CUSTOMER", loaded.LastMessageSenderType);
    }

    [SkippableFact]
    public async Task PendingMessage_IdempotentByClientId_And_MergesWebhookRowOnMarkSent()
    {
        var repo = Repo();
        var c = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow);
        var conv = await repo.GetOrCreateConversationAsync("OA1", c.Id, true);
        var clientId = Guid.NewGuid();
        var zaloMsgId = "zm-" + Guid.NewGuid().ToString("N");

        var pending = new ZaloOAPendingMessage
        {
            ConversationId = conv.Id, CustomerId = c.Id, AgentUserId = 42, MessageType = "TEXT", Content = "Chào bạn",
            ClientMessageId = clientId, Preview = "Chào bạn"
        };
        var p1 = await repo.InsertPendingMessageAsync(pending);
        var p2 = await repo.InsertPendingMessageAsync(pending);

        // Webhook oa_send_text về TRƯỚC khi API trả kết quả
        await repo.UpsertMessageAsync(new ZaloOAMessageUpsert
        {
            ConversationId = conv.Id, CustomerId = c.Id, Direction = "OUT", SenderType = "OA", MessageType = "TEXT",
            Content = "Chào bạn", ZaloMessageId = zaloMsgId, Status = "SENT", SentAt = DateTime.UtcNow, RawPayload = "{\"oa\":1}"
        });
        var sent = await repo.MarkMessageSentAsync(p1.Id, zaloMsgId, null, "{\"error\":0}");
        var all = await repo.GetMessagesAsync(conv.Id, 0, 30);

        Assert.False(p1.IsDuplicate);
        Assert.True(p2.IsDuplicate);
        Assert.Equal(p1.Id, p2.Id);
        Assert.Single(all);                                   // dòng webhook đã được gộp
        Assert.Equal("SENT", sent!.Status);
        Assert.Equal(42, sent.AgentUserId);                   // giữ thông tin nhân viên gửi
        Assert.Equal("{\"oa\":1}", sent.RawPayload);          // giữ payload webhook
        Assert.Equal(p1.Id, (await repo.GetConversationByIdAsync(conv.Id))!.LastMessageId);
    }

    [SkippableFact]
    public async Task MessageStatus_OnlyMovesForward()
    {
        var repo = Repo();
        var c = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow);
        var conv = await repo.GetOrCreateConversationAsync("OA1", c.Id, true);
        var p = await repo.InsertPendingMessageAsync(new ZaloOAPendingMessage
        { ConversationId = conv.Id, CustomerId = c.Id, MessageType = "TEXT", Content = "x", ClientMessageId = Guid.NewGuid() });
        var zid = "zm-" + Guid.NewGuid().ToString("N");
        await repo.MarkMessageSentAsync(p.Id, zid, null, null);

        Assert.Equal(1, await repo.UpdateMessageStatusByZaloIdAsync(zid, "SEEN", DateTime.UtcNow));
        Assert.Equal(0, await repo.UpdateMessageStatusByZaloIdAsync(zid, "DELIVERED", DateTime.UtcNow));
        Assert.Equal("SEEN", (await repo.GetMessageByIdAsync(p.Id))!.Status);
    }

    [SkippableFact]
    public async Task FailedMessage_Retry_Allowed_OnlyWhenFailed()
    {
        var repo = Repo();
        var c = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow);
        var conv = await repo.GetOrCreateConversationAsync("OA1", c.Id, true);
        var p = await repo.InsertPendingMessageAsync(new ZaloOAPendingMessage
        { ConversationId = conv.Id, CustomerId = c.Id, MessageType = "TEXT", Content = "x", ClientMessageId = Guid.NewGuid() });

        Assert.Null(await repo.ResetMessageForRetryAsync(p.Id, 1));             // PENDING: không retry
        var failed = await repo.MarkMessageFailedAsync(p.Id, -230, "not interacted", "{}");
        Assert.Equal("FAILED", failed!.Status);
        var reset = await repo.ResetMessageForRetryAsync(p.Id, 1);
        Assert.Equal("PENDING", reset!.Status);
        Assert.Null(reset.ErrorCode);
    }

    [SkippableFact]
    public async Task WebhookEvent_DuplicateDedupKey_Rejected()
    {
        var repo = Repo();
        var key = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

        var e1 = await repo.InsertWebhookEventAsync(key, "user_send_text", "OA1", "U1", "m1", 1, "{}", true);
        var e2 = await repo.InsertWebhookEventAsync(key, "user_send_text", "OA1", "U1", "m1", 1, "{}", true);
        await repo.SetWebhookEventStatusAsync(e1.Id, "FAILED", "boom");
        var row = await repo.GetWebhookEventAsync(e1.Id);

        Assert.False(e1.IsDuplicate);
        Assert.True(e2.IsDuplicate);
        Assert.Equal(e1.Id, e2.Id);
        Assert.Equal("FAILED", row!.ProcessStatus);
        Assert.Equal(1, row.RetryCount);
    }

    [SkippableFact]
    public async Task ConversationList_Search_Paging_And_LikeEscaping()
    {
        var repo = Repo();
        var tag = Guid.NewGuid().ToString("N")[..8];
        var c1 = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow, "Nguyễn 100% " + tag);
        var c2 = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow, "Trần 1000 " + tag);
        await repo.GetOrCreateConversationAsync("OA1", c1.Id, true);
        await repo.GetOrCreateConversationAsync("OA1", c2.Id, true);

        var all = await repo.GetConversationsAsync(new ZaloOAConversationQuery { Keyword = tag, PageSize = 1, SortBy = "LastMessageAt", SortDir = "DESC" });
        var percent = await repo.GetConversationsAsync(new ZaloOAConversationQuery { Keyword = "100% " + tag, PageSize = 20, SortBy = "LastMessageAt", SortDir = "DESC" });

        Assert.Equal(2, all.TotalRecords);
        Assert.Single(all.Items);                     // phân trang
        Assert.Single(percent.Items);                 // "%" được hiểu nguyên văn
        Assert.StartsWith("Nguyễn", percent.Items[0].DisplayName);
    }

    [SkippableFact]
    public async Task UnreadSummary_ReturnsNewInboundMessages_AndMarkReadResets()
    {
        var repo = Repo();
        var c = await repo.UpsertCustomerAsync("OA1", U(), null, DateTime.UtcNow, "Khách unread");
        var conv = await repo.GetOrCreateConversationAsync("OA1", c.Id, true);
        var before = await repo.GetUnreadSummaryAsync(0, 20);

        var r = await repo.UpsertMessageAsync(new ZaloOAMessageUpsert
        {
            ConversationId = conv.Id, CustomerId = c.Id, Direction = "IN", SenderType = "CUSTOMER", MessageType = "TEXT",
            Content = "Có ai không?", ZaloMessageId = "zm-" + Guid.NewGuid().ToString("N"), Status = "RECEIVED",
            SentAt = DateTime.UtcNow, IncrementUnread = true
        });
        var after = await repo.GetUnreadSummaryAsync(before.LatestInboundMessageId, 20);
        await repo.MarkConversationReadAsync(conv.Id, 9);
        var read = await repo.GetConversationByIdAsync(conv.Id);

        Assert.True(after.TotalUnread >= 1);
        Assert.Equal(r.Id, after.LatestInboundMessageId);
        var notice = Assert.Single(after.NewMessages, m => m.Id == r.Id);
        Assert.Equal("Khách unread", notice.DisplayName);
        Assert.Equal(0, read!.UnreadCount);
        Assert.Equal(9, read.LastReadByUserId);
    }
}
