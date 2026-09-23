using Microsoft.Extensions.Logging.Abstractions;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Services;
using Xunit;

namespace NVCMS.API.ReadGoogleSheet.Tests;

public class ZaloOAClientTests
{
    private static (ZaloOAClient Client, FakeHttpHandler Http, FakeTokenService Tokens) Create(Func<HttpRequestMessage, string?, HttpResponseMessage> respond)
    {
        var http = new FakeHttpHandler(respond);
        var tokens = new FakeTokenService();
        var client = new ZaloOAClient(FakeHttpHandler.CreateApi(http), tokens, TestOptions.Zalo(), NullLogger<ZaloOAClient>.Instance);
        return (client, http, tokens);
    }

    [Fact]
    public async Task SendText_Success_ParsesMessageId_AndSendsTokenInHeader()
    {
        var t = Create((_, _) => FakeHttpHandler.Json(
            "{\"data\":{\"message_id\":\"63ecf43f0df7dba892e6\",\"user_id\":\"2512523625412515\",\"sent_time\":\"1626926349402\"},\"error\":0,\"message\":\"Success\"}"));

        var r = await t.Client.SendTextAsync("2512523625412515", "Xin chào");

        Assert.True(r.Success);
        Assert.Equal("63ecf43f0df7dba892e6", r.Data!.MessageId);
        Assert.NotNull(r.Data.SentTime);
        var call = Assert.Single(t.Http.Calls);
        Assert.Equal("https://openapi.zalo.me/v3.0/oa/message/cs", call.Request.RequestUri!.ToString());
        Assert.Equal("token-1", call.Request.Headers.GetValues("access_token").Single());
        Assert.Contains("\"recipient\":{\"user_id\":\"2512523625412515\"}", call.Body);
        Assert.Contains("\"message\":{\"text\":\"Xin ch", call.Body);
    }

    [Fact]
    public async Task TokenExpiredError_RefreshesOnce_AndRetries()
    {
        var calls = 0;
        var t = Create((_, _) => ++calls == 1
            ? FakeHttpHandler.Json("{\"error\":-216,\"message\":\"Access token is invalid\"}")
            : FakeHttpHandler.Json("{\"data\":{\"message_id\":\"m2\"},\"error\":0,\"message\":\"Success\"}"));

        var r = await t.Client.SendTextAsync("1", "hi");

        Assert.True(r.Success);
        Assert.Equal(new[] { false, true }, t.Tokens.ForceRefreshCalls);
        Assert.Equal("token-2", t.Http.Calls[1].Request.Headers.GetValues("access_token").Single());
    }

    [Fact]
    public async Task TokenError_Twice_ReturnsError_WithoutLooping()
    {
        var t = Create((_, _) => FakeHttpHandler.Json("{\"error\":-220,\"message\":\"expired\"}"));

        var r = await t.Client.SendTextAsync("1", "hi");

        Assert.False(r.Success);
        Assert.Equal(-220, r.ErrorCode);
        Assert.Equal(2, t.Http.Calls.Count);
    }

    [Fact]
    public async Task Timeout_IsMappedToTimeoutCode()
    {
        var t = Create((_, _) => throw new TaskCanceledException("timeout"));

        var r = await t.Client.SendTextAsync("1", "hi");

        Assert.False(r.Success);
        Assert.Equal(ZaloApiErrorCodes.Timeout, r.ErrorCode);
    }

    [Fact]
    public async Task UserNotInteracted_ReturnsZaloCode()
    {
        var t = Create((_, _) => FakeHttpHandler.Json("{\"error\":-230,\"message\":\"User has not interacted with the OA in the past 7 days\"}"));

        var r = await t.Client.SendTextAsync("1", "hi");

        Assert.Equal(-230, r.ErrorCode);
        Assert.Single(t.Http.Calls);   // không retry lỗi nghiệp vụ
    }

    [Fact]
    public async Task NoToken_ReturnsTokenUnavailable_WithoutHttpCall()
    {
        var t = Create((_, _) => FakeHttpHandler.Json("{}"));
        t.Tokens.ThrowOnGet = new ZaloTokenUnavailableException("none");

        var r = await t.Client.SendTextAsync("1", "hi");

        Assert.Equal(ZaloApiErrorCodes.TokenUnavailable, r.ErrorCode);
        Assert.Empty(t.Http.Calls);
    }

    [Fact]
    public async Task HttpError_IsMapped()
    {
        var t = Create((_, _) => FakeHttpHandler.Json("bad gateway", System.Net.HttpStatusCode.BadGateway));

        var r = await t.Client.SendTextAsync("1", "hi");

        Assert.Equal(ZaloApiErrorCodes.HttpError, r.ErrorCode);
    }

    [Fact]
    public async Task GetUserDetail_ParsesProfile_AndPhoneAsNumber()
    {
        var t = Create((_, _) => FakeHttpHandler.Json(
            "{\"data\":{\"user_id\":\"4572947693969771653\",\"user_id_by_app\":\"46041\",\"display_name\":\"Phạm Khoa\"," +
            "\"user_is_follower\":true,\"avatars\":{\"120\":\"https://a/120.jpg\",\"240\":\"https://a/240.jpg\"}," +
            "\"tags_and_notes_info\":{\"tag_names\":[\"VIP\"],\"notes\":[]}," +
            "\"shared_info\":{\"name\":\"Khoa\",\"phone\":84868602410,\"city\":\"HCM\",\"district\":\"Q7\",\"address\":\"Z06\",\"user_dob\":\"25/12/1999\"}}," +
            "\"error\":0,\"message\":\"Success\"}"));

        var r = await t.Client.GetUserDetailAsync("4572947693969771653");

        Assert.True(r.Success);
        Assert.Equal("Phạm Khoa", r.Data!.DisplayName);
        Assert.Equal("84868602410", r.Data.SharedPhone);
        Assert.Equal("https://a/240.jpg", r.Data.Avatar);
        Assert.Equal("Z06, Q7, HCM", r.Data.SharedAddress);
        Assert.True(r.Data.IsFollower);
        Assert.Equal(new[] { "VIP" }, r.Data.TagNames);
        Assert.Contains("data=%7B%22user_id%22%3A%224572947693969771653%22%7D", t.Http.Calls[0].Request.RequestUri!.AbsoluteUri);
    }

    [Fact]
    public async Task GetConversation_RejectsNonNumericUserId()
    {
        var t = Create((_, _) => FakeHttpHandler.Json("{}"));

        var r = await t.Client.GetConversationAsync("1,\"x\":1", 0, 10);

        Assert.False(r.Success);
        Assert.Empty(t.Http.Calls);
    }
}
