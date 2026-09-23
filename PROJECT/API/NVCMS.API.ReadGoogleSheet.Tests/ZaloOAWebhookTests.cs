using Microsoft.Extensions.Logging.Abstractions;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Services;
using Xunit;

namespace NVCMS.API.ReadGoogleSheet.Tests;

public class ZaloOAWebhookTests
{
    private const string UserText =
        "{\"app_id\":\"app-123\",\"sender\":{\"id\":\"246845883529197922\"},\"user_id_by_app\":\"552177279717587730\"," +
        "\"recipient\":{\"id\":\"OA1\"},\"event_name\":\"user_send_text\",\"message\":{\"text\":\"Xin chào shop\",\"msg_id\":\"96d3cdf3af150460909\"}," +
        "\"timestamp\":\"1727000000000\"}";

    private static string Sign(string body, string timestamp = "1727000000000")
        => "mac=" + ZaloOAWebhookSignature.Compute(TestOptions.AppId, body, timestamp, TestOptions.OASecret);

    private static (ZaloOAWebhookService Service, FakeChatRepository Repo, FakeJobClient Jobs, FakeZaloOAClient Client) Create(
        Action<NVCMS.API.ReadGoogleSheet.Models.Config.ZaloSettings>? zalo = null)
    {
        var repo = new FakeChatRepository();
        var jobs = new FakeJobClient();
        var client = new FakeZaloOAClient();
        var customers = new ZaloOACustomerService(repo, client, TestOptions.Chat(), NullLogger<ZaloOACustomerService>.Instance);
        var service = new ZaloOAWebhookService(repo, customers, jobs, TestOptions.Zalo(zalo), TestOptions.Chat(), NullLogger<ZaloOAWebhookService>.Instance);
        return (service, repo, jobs, client);
    }

    // ── Chữ ký ─────────────────────────────────────────────────────────────

    [Fact]
    public void Signature_Valid_WithAndWithoutMacPrefix()
    {
        var mac = ZaloOAWebhookSignature.Compute("app", "{\"a\":1}", "123", "secret");
        Assert.True(ZaloOAWebhookSignature.Verify("mac=" + mac, "app", "{\"a\":1}", "123", "secret"));
        Assert.True(ZaloOAWebhookSignature.Verify(mac.ToUpperInvariant(), "app", "{\"a\":1}", "123", "secret"));
    }

    [Fact]
    public void Signature_Invalid_WhenBodyOrSecretChanges()
    {
        var mac = "mac=" + ZaloOAWebhookSignature.Compute("app", "{\"a\":1}", "123", "secret");
        Assert.False(ZaloOAWebhookSignature.Verify(mac, "app", "{\"a\":2}", "123", "secret"));
        Assert.False(ZaloOAWebhookSignature.Verify(mac, "app", "{\"a\":1}", "123", "other"));
        Assert.False(ZaloOAWebhookSignature.Verify(null, "app", "{\"a\":1}", "123", "secret"));
    }

    // ── Parser ─────────────────────────────────────────────────────────────

    [Fact]
    public void Parse_UserSendText()
    {
        var e = ZaloOAWebhookParser.Parse(UserText);
        Assert.Equal(ZaloOAWebhookKind.UserMessage, e.Kind);
        Assert.Equal("246845883529197922", e.CustomerUserId);
        Assert.Equal("OA1", e.OAId);
        Assert.Equal("96d3cdf3af150460909", e.MessageId);
        Assert.Equal(ZaloOAMessageType.Text, ZaloOAWebhookParser.MapMessageType(e));
        Assert.Equal(new DateTime(2024, 9, 22, 10, 13, 20, DateTimeKind.Utc), e.OccurredAtUtc);
    }

    [Fact]
    public void Parse_OASendText_CustomerIsRecipient()
    {
        var e = ZaloOAWebhookParser.Parse("{\"event_name\":\"oa_send_text\",\"sender\":{\"id\":\"OA1\",\"admin_id\":\"9\"},\"recipient\":{\"id\":\"U1\"},\"message\":{\"msg_id\":\"m1\",\"text\":\"hi\"},\"timestamp\":\"1\"}");
        Assert.Equal(ZaloOAWebhookKind.OAMessage, e.Kind);
        Assert.Equal("U1", e.CustomerUserId);
        Assert.Equal("OA1", e.OAId);
        Assert.Equal("9", e.SenderAdminId);
    }

    [Fact]
    public void Parse_ImageAttachment_And_SeenMsgIds()
    {
        var img = ZaloOAWebhookParser.Parse("{\"event_name\":\"user_send_image\",\"sender\":{\"id\":\"U1\"},\"recipient\":{\"id\":\"OA1\"},\"message\":{\"msg_id\":\"m1\",\"attachments\":[{\"type\":\"image\",\"payload\":{\"thumbnail\":\"https://t\",\"url\":\"https://u\"}}]},\"timestamp\":\"1\"}");
        Assert.Equal(ZaloOAMessageType.Image, ZaloOAWebhookParser.MapMessageType(img));
        Assert.Equal("https://u", ZaloOAWebhookParser.BuildContent(img, ZaloOAMessageType.Image));

        var seen = ZaloOAWebhookParser.Parse("{\"event_name\":\"user_seen_message\",\"sender\":{\"id\":\"U1\"},\"recipient\":{\"id\":\"OA1\"},\"message\":{\"msg_ids\":[\"a\",\"b\"]},\"timestamp\":\"1\"}");
        Assert.Equal(ZaloOAWebhookKind.UserSeen, seen.Kind);
        Assert.Equal(new[] { "a", "b" }, seen.MessageIds);
    }

    [Fact]
    public void Parse_Follow()
    {
        var e = ZaloOAWebhookParser.Parse("{\"event_name\":\"follow\",\"oa_id\":\"OA1\",\"follower\":{\"id\":\"U9\"},\"source\":\"oa_profile\",\"timestamp\":\"1\"}");
        Assert.Equal(ZaloOAWebhookKind.Follow, e.Kind);
        Assert.Equal("U9", e.CustomerUserId);
        Assert.Equal("OA1", e.OAId);
    }

    [Fact]
    public void DedupKey_SameMessage_SameKey_EvenIfBodyDiffers()
    {
        var a = ZaloOAWebhookParser.Parse(UserText);
        var retried = UserText.Replace("1727000000000", "1727000009999");
        var b = ZaloOAWebhookParser.Parse(retried);

        Assert.Equal(ZaloOAWebhookParser.ComputeDedupKey(a, UserText), ZaloOAWebhookParser.ComputeDedupKey(b, retried));

        var oa = ZaloOAWebhookParser.Parse(UserText.Replace("user_send_text", "oa_send_text"));
        Assert.NotEqual(ZaloOAWebhookParser.ComputeDedupKey(a, UserText), ZaloOAWebhookParser.ComputeDedupKey(oa, UserText));
    }

    // ── Receive ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Receive_ValidSignature_PersistsAndEnqueues()
    {
        var t = Create();

        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));

        Assert.Equal(ZaloOAWebhookReceiveOutcome.Accepted, r.Outcome);
        var e = Assert.Single(t.Repo.Events);
        Assert.True(e.SignatureValid);
        Assert.Equal("user_send_text", e.EventName);
        Assert.Single(t.Jobs.Created);
    }

    [Fact]
    public async Task Receive_InvalidSignature_Rejected_NotPersisted()
    {
        var t = Create();

        var r = await t.Service.ReceiveAsync(UserText, "mac=deadbeef");

        Assert.Equal(ZaloOAWebhookReceiveOutcome.InvalidSignature, r.Outcome);
        Assert.Empty(t.Repo.Events);
        Assert.Empty(t.Jobs.Created);
    }

    [Fact]
    public async Task Receive_MissingSecret_NotConfigured()
    {
        var t = Create(z => z.OASecretKey = null);

        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));

        Assert.Equal(ZaloOAWebhookReceiveOutcome.NotConfigured, r.Outcome);
        Assert.Empty(t.Repo.Events);
    }

    [Fact]
    public async Task Receive_InvalidJson_InvalidPayload()
    {
        var t = Create();
        var r = await t.Service.ReceiveAsync("not json", "mac=x");
        Assert.Equal(ZaloOAWebhookReceiveOutcome.InvalidPayload, r.Outcome);
    }

    [Fact]
    public async Task Receive_DuplicateEvent_NotEnqueuedTwice()
    {
        var t = Create();

        await t.Service.ReceiveAsync(UserText, Sign(UserText));
        var second = await t.Service.ReceiveAsync(UserText, Sign(UserText));

        Assert.Equal(ZaloOAWebhookReceiveOutcome.Duplicate, second.Outcome);
        Assert.Single(t.Repo.Events);
        Assert.Single(t.Jobs.Created);
    }

    [Fact]
    public async Task Receive_EnqueueFails_EventStillPersisted()
    {
        var t = Create();
        t.Jobs.Throw = true;

        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));

        Assert.Equal(ZaloOAWebhookReceiveOutcome.Accepted, r.Outcome);   // vẫn trả 200 cho Zalo
        Assert.Equal("PENDING", Assert.Single(t.Repo.Events).ProcessStatus);   // job reprocess sẽ xử lý
    }

    // ── Process ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Process_NewCustomer_CreatesCustomerConversationMessage_AndSyncsProfile()
    {
        var t = Create();
        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));

        await t.Service.ProcessAsync(r.EventId!.Value);

        var customer = Assert.Single(t.Repo.Customers);
        Assert.Equal("246845883529197922", customer.ZaloUserId);
        Assert.Equal("Khách 246845883529197922", customer.DisplayName);   // từ user/detail
        Assert.Single(t.Client.UserDetailCalls);
        var conv = Assert.Single(t.Repo.Conversations);
        Assert.Equal(1, conv.UnreadCount);
        var msg = Assert.Single(t.Repo.Messages);
        Assert.Equal("IN", msg.Direction);
        Assert.Equal("CUSTOMER", msg.SenderType);
        Assert.Equal("Xin chào shop", msg.Content);
        Assert.Equal("96d3cdf3af150460909", msg.ZaloMessageId);
        Assert.Equal("PROCESSED", t.Repo.Events.Single().ProcessStatus);
    }

    [Fact]
    public async Task Process_ExistingCustomer_ReusesConversation_NoDuplicateMessage()
    {
        var t = Create();
        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));
        await t.Service.ProcessAsync(r.EventId!.Value);

        // Cùng msg_id nhưng event khác (Zalo retry với body khác) → dedup ở tầng message
        var body2 = UserText.Replace("1727000000000", "1727000005000");
        t.Repo.Events.Single().ProcessStatus = "PROCESSED";
        var e2 = await t.Repo.InsertWebhookEventAsync("other-key", "user_send_text", "OA1", "246845883529197922", "96d3cdf3af150460909", 1, body2, true);
        await t.Service.ProcessAsync(e2.Id);

        Assert.Single(t.Repo.Customers);
        Assert.Single(t.Repo.Conversations);
        Assert.Single(t.Repo.Messages);
        Assert.Equal(1, t.Repo.Conversations.Single().UnreadCount);
        Assert.Single(t.Client.UserDetailCalls);   // hồ sơ còn mới, không gọi lại
    }

    [Fact]
    public async Task Process_ProfileSyncFails_MessageStillSaved()
    {
        var t = Create();
        t.Client.UserDetailResult = _ => ZaloOAApiResult<ZaloOAUserDetail>.Fail(-213, "not follower");
        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));

        await t.Service.ProcessAsync(r.EventId!.Value);

        Assert.Single(t.Repo.Messages);
        Assert.Equal("PROCESSED", t.Repo.Events.Single().ProcessStatus);
    }

    [Fact]
    public async Task Process_UnknownEvent_Ignored()
    {
        var t = Create();
        var body = "{\"app_id\":\"app-123\",\"event_name\":\"shop_has_order\",\"timestamp\":\"1727000000000\"}";
        var r = await t.Service.ReceiveAsync(body, Sign(body));

        await t.Service.ProcessAsync(r.EventId!.Value);

        Assert.Equal("IGNORED", t.Repo.Events.Single().ProcessStatus);
        Assert.Empty(t.Repo.Messages);
    }

    [Fact]
    public async Task Process_AlreadyProcessed_IsNoOp()
    {
        var t = Create();
        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));
        await t.Service.ProcessAsync(r.EventId!.Value);
        await t.Service.ProcessAsync(r.EventId!.Value);

        Assert.Single(t.Repo.Upserts);
    }

    [Fact]
    public async Task Process_ClosedConversation_IsReopenedByCustomerMessage()
    {
        var t = Create();
        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));
        await t.Service.ProcessAsync(r.EventId!.Value);
        t.Repo.Conversations.Single().Status = "CLOSED";

        var body2 = UserText.Replace("96d3cdf3af150460909", "new-msg-2");
        var r2 = await t.Service.ReceiveAsync(body2, Sign(body2));
        await t.Service.ProcessAsync(r2.EventId!.Value);

        Assert.Equal("OPEN", t.Repo.Conversations.Single().Status);
        Assert.Equal(2, t.Repo.Messages.Count);
    }

    [Fact]
    public async Task Process_SeenEvent_UpdatesOutgoingStatus()
    {
        var t = Create();
        var r = await t.Service.ReceiveAsync(UserText, Sign(UserText));
        await t.Service.ProcessAsync(r.EventId!.Value);
        var m = t.Repo.Messages.Single();
        m.Direction = "OUT"; m.Status = "SENT";

        var seen = "{\"app_id\":\"app-123\",\"event_name\":\"user_seen_message\",\"sender\":{\"id\":\"246845883529197922\"},\"recipient\":{\"id\":\"OA1\"},\"message\":{\"msg_ids\":[\"96d3cdf3af150460909\"]},\"timestamp\":\"1727000001000\"}";
        var r2 = await t.Service.ReceiveAsync(seen, Sign(seen, "1727000001000"));
        await t.Service.ProcessAsync(r2.EventId!.Value);

        Assert.Equal("SEEN", m.Status);
    }
}
