using Microsoft.Extensions.Logging.Abstractions;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Security;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Services;
using Xunit;

namespace NVCMS.API.ReadGoogleSheet.Tests;

public class ZaloTokenTests
{
    private const string NewTokenJson = "{\"access_token\":\"new-access\",\"refresh_token\":\"new-refresh\",\"expires_in\":\"90000\"}";

    private static (ZaloService Service, InMemoryTokenRepository Repo, FakeHttpHandler Http, FakeRefreshLock Lock, IZaloTokenProtector Protector, FakeColumnInspector Columns)
        Create(Func<HttpRequestMessage, string?, HttpResponseMessage>? respond = null, Action<ZaloSettings>? configure = null)
    {
        var repo = new InMemoryTokenRepository();
        var http = new FakeHttpHandler(respond ?? ((_, _) => FakeHttpHandler.Json(NewTokenJson)));
        var @lock = new FakeRefreshLock();
        var protector = TestOptions.Protector();
        var columns = new FakeColumnInspector();
        var service = new ZaloService(FakeHttpHandler.CreateApi(http), repo, null!, null!, TestOptions.Zalo(configure),
            protector, @lock, columns, NullLogger<ZaloService>.Instance);
        return (service, repo, http, @lock, protector, columns);
    }

    private static Zalo_Token Token(string access, string refresh, DateTime createdAtUtc, string expiresIn = "90000")
        => new() { AccessToken = access, RefreshToken = refresh, ExpiresIn = expiresIn, CreatedAt = createdAtUtc };

    [Fact]
    public async Task ValidToken_IsReturned_WithoutCallingZalo()
    {
        var t = Create();
        await t.Repo.AddAsync(Token("valid-access", "valid-refresh", DateTime.UtcNow.AddHours(-1)));

        var token = await t.Service.GetValidAccessTokenAsync();

        Assert.Equal("valid-access", token);
        Assert.Empty(t.Http.Calls);
        Assert.Equal(0, t.Lock.Acquired);
    }

    [Fact]
    public async Task ExpiredToken_IsRefreshed_AndNewTokenSavedEncrypted()
    {
        var t = Create();
        await t.Repo.AddAsync(Token("old-access", "old-refresh", DateTime.UtcNow.AddHours(-26)));

        var token = await t.Service.GetValidAccessTokenAsync();

        Assert.Equal("new-access", token);
        var call = Assert.Single(t.Http.Calls);
        Assert.Contains("grant_type=refresh_token", call.Body);
        Assert.Contains("refresh_token=old-refresh", call.Body);
        Assert.Equal("app-secret", call.Request.Headers.GetValues("secret_key").Single());

        var saved = t.Repo.Rows.Last();
        Assert.StartsWith(ZaloTokenProtector.Prefix, saved.AccessToken);   // lưu DB đã mã hoá
        Assert.StartsWith(ZaloTokenProtector.Prefix, saved.RefreshToken);
        var last = await t.Service.GetLastTokenAsync();
        Assert.Equal("new-access", last.AccessToken);                      // đọc ra đã giải mã
        Assert.Equal("new-refresh", last.RefreshToken);
        Assert.Equal(1, t.Lock.Acquired);
    }

    [Fact]
    public async Task TokenNearExpiry_WithinMargin_IsRefreshed()
    {
        var t = Create(configure: s => s.TokenRefreshMarginMinutes = 30);
        await t.Repo.AddAsync(Token("a", "r", DateTime.UtcNow.AddSeconds(-90000 + 10 * 60)));   // còn 10 phút

        Assert.Equal("new-access", await t.Service.GetValidAccessTokenAsync());
    }

    [Fact]
    public async Task ForceRefresh_RefreshesEvenIfNotExpired()
    {
        var t = Create();
        await t.Repo.AddAsync(Token("valid-access", "valid-refresh", DateTime.UtcNow));

        Assert.Equal("new-access", await t.Service.GetValidAccessTokenAsync(forceRefresh: true));
        Assert.Single(t.Http.Calls);
    }

    [Fact]
    public async Task RefreshFailure_ThrowsTokenUnavailable_AndKeepsOldToken()
    {
        var t = Create((_, _) => FakeHttpHandler.Json("{\"error\":-14014,\"error_name\":\"Invalid refresh token\",\"error_description\":\"expired\"}"));
        await t.Repo.AddAsync(Token("old-access", "old-refresh", DateTime.UtcNow.AddHours(-30)));

        await Assert.ThrowsAsync<ZaloTokenUnavailableException>(() => t.Service.GetValidAccessTokenAsync());
        Assert.Single(t.Repo.Rows);   // không lưu token rỗng
    }

    [Fact]
    public async Task NoToken_ThrowsTokenUnavailable()
    {
        var t = Create();
        await Assert.ThrowsAsync<ZaloTokenUnavailableException>(() => t.Service.GetValidAccessTokenAsync());
    }

    [Fact]
    public async Task LegacyPlaintextToken_IsStillReadable()
    {
        var t = Create();
        await t.Repo.AddAsync(Token("plain-access", "plain-refresh", DateTime.UtcNow));

        var last = await t.Service.GetLastTokenAsync();

        Assert.Equal("plain-access", last.AccessToken);
        Assert.Equal("plain-refresh", last.RefreshToken);
    }

    [Fact]
    public async Task GetLastToken_DoesNotMutateTrackedEntity()
    {
        var t = Create();
        await t.Repo.AddAsync(Token("x", "y", DateTime.UtcNow.AddHours(-26)));
        await t.Service.GetValidAccessTokenAsync();   // lưu token mã hoá

        var stored = t.Repo.Rows.Last();
        var before = stored.AccessToken;
        await t.Service.GetLastTokenAsync();

        Assert.Equal(before, stored.AccessToken);     // entity trong repo vẫn là bản mã hoá
    }

    [Fact]
    public async Task ColumnTooShort_SavesPlaintext_InsteadOfLosingToken()
    {
        var t = Create();
        t.Columns.MaxChars = 50;
        await t.Repo.AddAsync(Token("old", "old-r", DateTime.UtcNow.AddHours(-26)));

        await t.Service.GetValidAccessTokenAsync();

        Assert.Equal("new-access", t.Repo.Rows.Last().AccessToken);
    }

    [Fact]
    public async Task RefreshAndSave_SkipsZaloCall_WhenAnotherProcessAlreadyRotated()
    {
        var t = Create();
        await t.Repo.AddAsync(Token("fresh-access", "fresh-refresh", DateTime.UtcNow));

        // Job cầm refresh token cũ (đã bị thay) → không được gọi Zalo với token đã huỷ
        var result = await t.Service.RefreshAndSaveTokenAsync("stale-refresh");

        Assert.Empty(t.Http.Calls);
        Assert.Equal("fresh-access", result.access_token);
    }

    [Fact]
    public async Task TokenStatus_DoesNotExposeTokenValues()
    {
        var t = Create();
        await t.Repo.AddAsync(Token("secret-access", "secret-refresh", DateTime.UtcNow));

        var status = await t.Service.GetTokenStatusAsync();

        Assert.NotNull(status);
        Assert.False(status!.IsExpired);
        var json = System.Text.Json.JsonSerializer.Serialize(status);
        Assert.DoesNotContain("secret", json);
    }
}
