using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

/// <summary>
/// FB-SYNC 19/11/2026: ghi bài lấy từ Facebook vào module Tin tức của website (DB DefaultConnection).
///
/// Tạo bài bằng ĐÚNG các stored procedure mà màn soạn tin đang gọi (News_add, NewsByCategoryAdd,
/// News_updateStatus, News_updatePublishedDate, News_Process_Insert) nên không phụ thuộc tên bảng
/// và không đổi luồng dữ liệu. Tham số truyền theo thứ tự, giống SqlHelper của DNN.
/// Chỉ thêm 1 bảng mới News_FacebookSync để chống tạo trùng.
/// </summary>
public class FacebookNewsRepository
{
    private const int MaxRetry = 3;

    private readonly string _connStr;

    public FacebookNewsRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("DefaultConnection not configured");
    }

    private SqlConnection Open() => new SqlConnection(_connStr);

    // ── Bảng theo dõi ────────────────────────────────────────────────────────

    public async Task EnsureTableAsync(CancellationToken ct)
    {
        const string sql = @"
IF OBJECT_ID(N'dbo.News_FacebookSync', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.News_FacebookSync
    (
        Id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_News_FacebookSync PRIMARY KEY,
        PageId        NVARCHAR(50)   NOT NULL,
        FbPostId      NVARCHAR(100)  NOT NULL,
        NewId         INT            NULL,
        Permalink     NVARCHAR(500)  NULL,
        FbCreatedTime DATETIME       NULL,
        [Status]      TINYINT        NOT NULL,
        Note          NVARCHAR(1000) NULL,
        RetryCount    INT            NOT NULL CONSTRAINT DF_News_FacebookSync_Retry DEFAULT (0),
        CreatedAt     DATETIME       NOT NULL CONSTRAINT DF_News_FacebookSync_Created DEFAULT (GETDATE()),
        UpdatedAt     DATETIME       NOT NULL CONSTRAINT DF_News_FacebookSync_Updated DEFAULT (GETDATE())
    );
    CREATE UNIQUE INDEX UX_News_FacebookSync_FbPostId ON dbo.News_FacebookSync (FbPostId);
END";
        using var conn = Open();
        await conn.ExecuteAsync(new CommandDefinition(sql, cancellationToken: ct));
    }

    /// <summary>Giờ đăng (UTC) của bài Facebook mới nhất đã ghi nhận; null nếu chưa chạy lần nào.</summary>
    public async Task<DateTime?> GetLastCreatedTimeUtcAsync(string pageId, CancellationToken ct)
    {
        using var conn = Open();
        return await conn.ExecuteScalarAsync<DateTime?>(new CommandDefinition(
            "SELECT MAX(FbCreatedTime) FROM dbo.News_FacebookSync WHERE PageId = @pageId",
            new { pageId }, cancellationToken: ct));
    }

    /// <summary>
    /// Giữ chỗ một bài để xử lý. Trả false nếu bài đã được xử lý (đã tạo / đã bỏ qua / đang chạy / hết lượt thử).
    /// Bài lỗi mà CHƯA tạo được tin thì được thử lại tối đa 3 lần; bài kẹt "đang xử lý" quá 1 giờ cũng vậy.
    /// </summary>
    public async Task<bool> TryClaimAsync(string pageId, FbPost post, CancellationToken ct)
    {
        const string sql = @"
UPDATE dbo.News_FacebookSync
   SET [Status] = 0, RetryCount = RetryCount + 1, UpdatedAt = GETDATE()
 WHERE FbPostId = @postId
   AND NewId IS NULL
   AND RetryCount < @maxRetry
   AND ([Status] = 3 OR ([Status] = 0 AND UpdatedAt < DATEADD(HOUR, -1, GETDATE())));

IF @@ROWCOUNT > 0
BEGIN
    SELECT 1;
    RETURN;
END

IF EXISTS (SELECT 1 FROM dbo.News_FacebookSync WHERE FbPostId = @postId)
BEGIN
    SELECT 0;
    RETURN;
END

INSERT INTO dbo.News_FacebookSync (PageId, FbPostId, Permalink, FbCreatedTime, [Status])
VALUES (@pageId, @postId, @permalink, @created, 0);
SELECT 1;";

        try
        {
            using var conn = Open();
            var claimed = await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, new
            {
                pageId,
                postId = post.Id,
                permalink = Truncate(post.PermalinkUrl, 500),
                created = post.CreatedTimeUtc,
                maxRetry = MaxRetry
            }, cancellationToken: ct));
            return claimed == 1;
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            // Một tiến trình khác vừa giữ chỗ bài này
            return false;
        }
    }

    public async Task SetNewIdAsync(string postId, int newId, CancellationToken ct)
    {
        using var conn = Open();
        await conn.ExecuteAsync(new CommandDefinition(
            "UPDATE dbo.News_FacebookSync SET NewId = @newId, UpdatedAt = GETDATE() WHERE FbPostId = @postId",
            new { postId, newId }, cancellationToken: ct));
    }

    public async Task MarkAsync(string postId, FacebookSyncStatus status, string? note, CancellationToken ct)
    {
        using var conn = Open();
        await conn.ExecuteAsync(new CommandDefinition(
            "UPDATE dbo.News_FacebookSync SET [Status] = @status, Note = @note, UpdatedAt = GETDATE() WHERE FbPostId = @postId",
            new { postId, status = (byte)status, note = Truncate(note, 1000) }, cancellationToken: ct));
    }

    /// <summary>Đọc 1 giá trị PortalSettings của DNN (ví dụ thư mục lưu ảnh).</summary>
    public async Task<string?> GetPortalSettingAsync(int portalId, string settingName, CancellationToken ct)
    {
        using var conn = Open();
        return await conn.ExecuteScalarAsync<string?>(new CommandDefinition(
            "SELECT TOP 1 SettingValue FROM dbo.PortalSettings WHERE PortalID = @portalId AND SettingName = @settingName",
            new { portalId, settingName }, cancellationToken: ct));
    }

    // ── Module Tin tức ───────────────────────────────────────────────────────

    /// <summary>Gọi News_add - thứ tự tham số theo SqlDataProvider.NV_News_add. Trả NewId.</summary>
    public async Task<int> InsertNewsAsync(object?[] newsAddValues, CancellationToken ct)
    {
        var result = await ExecScalarAsync("News_add", newsAddValues, ct);
        if (result == null || result == DBNull.Value)
            throw new InvalidOperationException("News_add không trả về NewId");
        return Convert.ToInt32(result);
    }

    public Task AddCategoryAsync(int newId, int categoryId, bool isMain, CancellationToken ct) =>
        ExecScalarAsync("NewsByCategoryAdd", new object?[] { newId, categoryId, isMain }, ct);

    public Task UpdateStatusAsync(int newId, int status, int userId, CancellationToken ct) =>
        ExecScalarAsync("News_updateStatus", new object?[] { newId, status, userId }, ct);

    public Task UpdatePublishedDateAsync(int newId, DateTime publishedDate, int userId, CancellationToken ct) =>
        ExecScalarAsync("News_updatePublishedDate", new object?[] { newId, publishedDate, userId }, ct);

    /// <summary>Ghi lịch sử xử lý bài (tab "Quá trình xử lý" ở màn soạn tin).</summary>
    public Task InsertProcessAsync(int newId, int statusId, string processName, int userId, CancellationToken ct) =>
        ExecScalarAsync("News_Process_Insert", new object?[]
        {
            newId, statusId, processName, string.Empty, userId, 0, DateTime.Now, 0, "facebook-sync"
        }, ct);

    /// <summary>
    /// Gọi stored procedure với tham số truyền THEO THỨ TỰ (đọc danh sách tham số từ DB như SqlHelper của DNN).
    /// Báo lỗi rõ nếu số tham số của proc trên DB khác với code.
    /// </summary>
    private async Task<object?> ExecScalarAsync(string procName, object?[] values, CancellationToken ct)
    {
        await using var conn = Open();
        await conn.OpenAsync(ct);

        await using var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };
        SqlCommandBuilder.DeriveParameters(cmd);

        var inputs = cmd.Parameters.Cast<SqlParameter>()
            .Where(p => p.Direction != ParameterDirection.ReturnValue)
            .ToList();

        if (inputs.Count != values.Length)
            throw new InvalidOperationException(
                $"Stored procedure {procName} có {inputs.Count} tham số nhưng job truyền {values.Length}. " +
                "Proc trên DB đã khác với mã nguồn NVCMS.Modules.TinTuc - cần cập nhật FacebookPageSyncService.");

        for (var i = 0; i < inputs.Count; i++)
            inputs[i].Value = values[i] ?? DBNull.Value;

        return await cmd.ExecuteScalarAsync(ct);
    }

    private static string? Truncate(string? s, int max) =>
        string.IsNullOrEmpty(s) || s.Length <= max ? s : s[..max];
}
