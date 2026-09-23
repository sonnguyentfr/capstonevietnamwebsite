using Dapper;
using Microsoft.Data.SqlClient;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    /// <summary>
    /// Kiểm tra độ dài cột AccessToken/RefreshToken của bảng Zalo_Token (schema có sẵn, không ALTER).
    /// Token mã hoá dài hơn plaintext; nếu cột không đủ chỗ thì ZaloService lưu plaintext và cảnh báo,
    /// tránh trường hợp refresh token mới (Zalo đã xoay vòng) không lưu được.
    /// </summary>
    public interface IZaloTokenColumnInspector
    {
        /// <summary>Số ký tự tối đa của cột; null = NVARCHAR(MAX)/VARCHAR(MAX)/TEXT hoặc không xác định.</summary>
        Task<int?> GetMaxCharsAsync(string columnName);
    }

    public class ZaloTokenColumnInspector : IZaloTokenColumnInspector
    {
        private static readonly Dictionary<string, int?> Cache = new(StringComparer.OrdinalIgnoreCase);
        private static readonly SemaphoreSlim Gate = new(1, 1);
        private readonly string _connStr;

        public ZaloTokenColumnInspector(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("DefaultConnection not configured");
        }

        public async Task<int?> GetMaxCharsAsync(string columnName)
        {
            await Gate.WaitAsync();
            try
            {
                if (Cache.TryGetValue(columnName, out var cached))
                    return cached;

                using var conn = new SqlConnection(_connStr);
                var row = await conn.QueryFirstOrDefaultAsync<(short MaxLength, string TypeName)>(
                    @"SELECT c.max_length, t.name
                      FROM sys.columns c
                      INNER JOIN sys.types t ON t.user_type_id = c.user_type_id
                      WHERE c.object_id = OBJECT_ID('dbo.Zalo_Token') AND c.name = @columnName",
                    new { columnName });

                int? chars = row.TypeName switch
                {
                    null => null,
                    _ when row.MaxLength == -1 => null,
                    "nvarchar" or "nchar" => row.MaxLength / 2,
                    "varchar" or "char" => row.MaxLength,
                    _ => null
                };

                Cache[columnName] = chars;
                return chars;
            }
            finally
            {
                Gate.Release();
            }
        }
    }
}
