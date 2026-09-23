using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NVCMS.API.ReadGoogleSheet.Infrastructure.Locking
{
    /// <summary>
    /// Refresh token Zalo chỉ dùng được 1 lần: 2 tiến trình refresh cùng lúc thì tiến trình sau
    /// dùng refresh token đã bị huỷ và làm đứt chuỗi token. Khoá này bảo đảm tại một thời điểm
    /// chỉ 1 nơi refresh (trong process + giữa các process/app pool qua sp_getapplock).
    /// </summary>
    public interface IZaloTokenRefreshLock
    {
        Task<IAsyncDisposable> AcquireAsync(CancellationToken cancellationToken = default);
    }

    public class SqlZaloTokenRefreshLock : IZaloTokenRefreshLock
    {
        private const string Resource = "NVCMS_ZaloToken_Refresh";
        private const int LockTimeoutMs = 60_000;
        private static readonly SemaphoreSlim LocalLock = new(1, 1);

        private readonly string _connStr;
        private readonly ILogger<SqlZaloTokenRefreshLock> _logger;

        public SqlZaloTokenRefreshLock(IConfiguration config, ILogger<SqlZaloTokenRefreshLock> logger)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("DefaultConnection not configured");
            _logger = logger;
        }

        public async Task<IAsyncDisposable> AcquireAsync(CancellationToken cancellationToken = default)
        {
            if (!await LocalLock.WaitAsync(LockTimeoutMs, cancellationToken))
                throw new TimeoutException("Hết thời gian chờ khoá refresh Zalo token (trong process).");

            SqlConnection? conn = null;
            try
            {
                conn = new SqlConnection(_connStr);
                await conn.OpenAsync(cancellationToken);

                var p = new DynamicParameters();
                p.Add("@Resource", Resource);
                p.Add("@LockMode", "Exclusive");
                p.Add("@LockOwner", "Session");
                p.Add("@LockTimeout", LockTimeoutMs);
                p.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await conn.ExecuteAsync("sp_getapplock", p, commandType: CommandType.StoredProcedure,
                    commandTimeout: LockTimeoutMs / 1000 + 5);

                var result = p.Get<int>("@Result");
                if (result < 0)
                    throw new TimeoutException($"Không lấy được khoá refresh Zalo token (sp_getapplock={result}).");

                return new Releaser(conn);
            }
            catch
            {
                if (conn != null) await conn.DisposeAsync();
                LocalLock.Release();
                throw;
            }
        }

        private sealed class Releaser : IAsyncDisposable
        {
            private readonly SqlConnection _conn;
            private int _disposed;

            public Releaser(SqlConnection conn) => _conn = conn;

            public async ValueTask DisposeAsync()
            {
                if (Interlocked.Exchange(ref _disposed, 1) == 1) return;
                try
                {
                    // Đóng connection cũng tự nhả session lock; gọi release cho rõ ràng.
                    await _conn.ExecuteAsync("sp_releaseapplock",
                        new { Resource, LockOwner = "Session" }, commandType: CommandType.StoredProcedure);
                }
                catch
                {
                    // bỏ qua - DisposeAsync bên dưới vẫn nhả lock
                }
                finally
                {
                    await _conn.DisposeAsync();
                    LocalLock.Release();
                }
            }
        }
    }
}
