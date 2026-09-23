using Microsoft.Extensions.Logging.Abstractions;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Services;
using Xunit;

namespace NVCMS.API.ReadGoogleSheet.Tests;

public class ZaloOAChatServiceTests
{
    private static (ZaloOAChatService Service, FakeChatRepository Repo, FakeZaloOAClient Client, long ConversationId) Create()
    {
        var repo = new FakeChatRepository();
        var client = new FakeZaloOAClient();
        var customers = new ZaloOACustomerService(repo, client, TestOptions.Chat(), NullLogger<ZaloOACustomerService>.Instance);
        var service = new ZaloOAChatService(repo, client, customers, TestOptions.Zalo(), NullLogger<ZaloOAChatService>.Instance);

        var customer = repo.UpsertCustomerAsync("OA1", "U1", null, DateTime.UtcNow).Result;
        var conv = repo.GetOrCreateConversationAsync("OA1", customer.Id, false).Result;
        return (service, repo, client, conv.Id);
    }

    private static ZaloOASendMessageRequest Text(long conversationId, string text = "Chào bạn", Guid? clientId = null)
        => new() { ConversationId = conversationId, ClientMessageId = clientId ?? Guid.NewGuid(), AgentUserId = 7, Text = text };

    [Fact]
    public async Task Send_Success_SavesSentWithZaloMessageId()
    {
        var t = Create();

        var r = await t.Service.SendMessageAsync(Text(t.ConversationId));

        Assert.True(r.Success);
        Assert.Equal("SENT", r.Data!.Status);
        Assert.Equal("zmsg-1", r.Data.ZaloMessageId);
        Assert.Equal(7, r.Data.AgentUserId);
        Assert.Equal(new[] { "Chào bạn" }, t.Client.SentTexts);
    }

    [Fact]
    public async Task Send_ZaloRejects_UserNotInteracted_SavesFailed()
    {
        var t = Create();
        t.Client.SendResult = () => ZaloOAApiResult<ZaloOASendResult>.Fail(-230, "User has not interacted", "{\"error\":-230}");

        var r = await t.Service.SendMessageAsync(Text(t.ConversationId));

        Assert.False(r.Success);
        Assert.Equal(ZaloOAErrorCodes.ZaloUserNotInteracted, r.ErrorCode);
        Assert.Equal(422, r.HttpStatus);
        Assert.Equal("FAILED", r.Data!.Status);
        Assert.Equal(-230, r.Data.ErrorCode);
    }

    [Fact]
    public async Task Send_Timeout_SavesFailed_WithTimeoutCode()
    {
        var t = Create();
        t.Client.SendResult = () => ZaloOAApiResult<ZaloOASendResult>.Fail(ZaloApiErrorCodes.Timeout, "timeout");

        var r = await t.Service.SendMessageAsync(Text(t.ConversationId));

        Assert.Equal(ZaloOAErrorCodes.ZaloTimeout, r.ErrorCode);
        Assert.Equal("FAILED", t.Repo.Messages.Single().Status);
    }

    [Fact]
    public async Task Send_TokenUnavailable_Returns503()
    {
        var t = Create();
        t.Client.SendResult = () => ZaloOAApiResult<ZaloOASendResult>.Fail(ZaloApiErrorCodes.TokenUnavailable, "none");

        var r = await t.Service.SendMessageAsync(Text(t.ConversationId));

        Assert.Equal(ZaloOAErrorCodes.ZaloTokenUnavailable, r.ErrorCode);
        Assert.Equal(503, r.HttpStatus);
    }

    [Fact]
    public async Task Send_InvalidConversation_NotFound_NoZaloCall()
    {
        var t = Create();

        var r = await t.Service.SendMessageAsync(Text(999));

        Assert.Equal(ZaloOAErrorCodes.NotFound, r.ErrorCode);
        Assert.Empty(t.Client.SentTexts);
    }

    [Fact]
    public async Task Send_InvalidCustomer_NotFound()
    {
        var t = Create();
        t.Repo.Customers.Clear();   // hội thoại còn nhưng không còn khách

        var r = await t.Service.SendMessageAsync(Text(t.ConversationId));

        Assert.Equal(ZaloOAErrorCodes.NotFound, r.ErrorCode);
        Assert.Empty(t.Client.SentTexts);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Send_EmptyText_ValidationError(string text)
    {
        var t = Create();
        var r = await t.Service.SendMessageAsync(Text(t.ConversationId, text));
        Assert.Equal(ZaloOAErrorCodes.ValidationError, r.ErrorCode);
    }

    [Fact]
    public async Task Send_TooLong_ValidationError()
    {
        var t = Create();
        var r = await t.Service.SendMessageAsync(Text(t.ConversationId, new string('a', 2001)));
        Assert.Equal(ZaloOAErrorCodes.ValidationError, r.ErrorCode);
    }

    [Fact]
    public async Task Send_SameClientMessageId_Twice_SendsOnce()
    {
        var t = Create();
        var id = Guid.NewGuid();

        await t.Service.SendMessageAsync(Text(t.ConversationId, clientId: id));
        var second = await t.Service.SendMessageAsync(Text(t.ConversationId, clientId: id));

        Assert.True(second.Success);
        Assert.Single(t.Client.SentTexts);
        Assert.Single(t.Repo.Messages);
    }

    [Fact]
    public async Task Retry_FailedMessage_ResendsSameRow()
    {
        var t = Create();
        t.Client.SendResult = () => ZaloOAApiResult<ZaloOASendResult>.Fail(-200, "Send message failed");
        var failed = await t.Service.SendMessageAsync(Text(t.ConversationId));

        t.Client.SendResult = () => new ZaloOAApiResult<ZaloOASendResult> { Data = new ZaloOASendResult { MessageId = "zmsg-retry" } };
        var retry = await t.Service.RetryMessageAsync(failed.Data!.Id, 7);

        Assert.True(retry.Success);
        Assert.Equal("zmsg-retry", retry.Data!.ZaloMessageId);
        Assert.Single(t.Repo.Messages);
    }

    [Fact]
    public async Task Retry_SentMessage_Rejected()
    {
        var t = Create();
        var sent = await t.Service.SendMessageAsync(Text(t.ConversationId));

        var retry = await t.Service.RetryMessageAsync(sent.Data!.Id, 7);

        Assert.Equal(ZaloOAErrorCodes.ValidationError, retry.ErrorCode);
    }

    [Fact]
    public async Task SendAttachment_RejectsWrongTypeAndSize()
    {
        var t = Create();
        using var s = new MemoryStream(new byte[10]);

        var exe = await t.Service.SendAttachmentAsync(t.ConversationId, Guid.NewGuid(), 7, s, "virus.exe", "application/octet-stream", 10, null);
        var bigImage = await t.Service.SendAttachmentAsync(t.ConversationId, Guid.NewGuid(), 7, s, "a.png", "image/png", 2 * 1024 * 1024, null);

        Assert.Equal(ZaloOAErrorCodes.ValidationError, exe.ErrorCode);
        Assert.Equal(ZaloOAErrorCodes.ValidationError, bigImage.ErrorCode);
    }

    [Fact]
    public async Task SendAttachment_Image_UploadsThenSends()
    {
        var t = Create();
        using var s = new MemoryStream(new byte[100]);

        var r = await t.Service.SendAttachmentAsync(t.ConversationId, Guid.NewGuid(), 7, s, "anh.png", "image/png", 100, "Ảnh sản phẩm");

        Assert.True(r.Success);
        Assert.Equal("IMAGE", r.Data!.MessageType);
        Assert.Contains("att-1", r.Data.AttachmentsJson);
    }

    [Fact]
    public async Task SetStatus_Invalid_ValidationError()
    {
        var t = Create();
        var r = await t.Service.SetStatusAsync(t.ConversationId, "DELETED", 1);
        Assert.Equal(ZaloOAErrorCodes.ValidationError, r.ErrorCode);
    }

    [Fact]
    public async Task SetStatus_Close_Then_Reopen()
    {
        var t = Create();
        Assert.True((await t.Service.SetStatusAsync(t.ConversationId, "closed", 1)).Success);
        Assert.Equal("CLOSED", t.Repo.Conversations.Single().Status);
        Assert.True((await t.Service.SetStatusAsync(t.ConversationId, "OPEN", 1)).Success);
        Assert.Equal("OPEN", t.Repo.Conversations.Single().Status);
    }

    [Fact]
    public void ToUtc_ConvertsLocalAndKeepsUnspecifiedAsUtc()
    {
        var local = new DateTime(2026, 9, 23, 7, 0, 0, DateTimeKind.Local);
        Assert.Equal(local.ToUniversalTime(), ZaloOAChatService.ToUtc(local));
        var unspecified = new DateTime(2026, 9, 23, 0, 0, 0, DateTimeKind.Unspecified);
        Assert.Equal(DateTimeKind.Utc, ZaloOAChatService.ToUtc(unspecified)!.Value.Kind);
        Assert.Null(ZaloOAChatService.ToUtc(null));
    }
}
