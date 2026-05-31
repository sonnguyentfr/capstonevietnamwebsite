using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;

namespace NVCMS.WebView.Data.Repository;

public class ShortyUrlRepository : IShortyUrlRepository
{
    private readonly string _cs;
    public ShortyUrlRepository(string connectionString) => _cs = connectionString;
    private SqlConnection CreateConn() => new(_cs);

    public async Task<string?> GetRealUrlAsync(string shortUrl)
    {
        await using var conn = CreateConn();
        var row = await conn.QueryFirstOrDefaultAsync(
            "NVCMS_ShortyUrls_GetUrl",
            new { short_url = shortUrl },
            commandType: CommandType.StoredProcedure);

        return row is null ? null : (string?)row.real_url;
    }

    public async Task IncrementClickAsync(string shortUrl)
    {
        await using var conn = CreateConn();
        await conn.ExecuteAsync(
            "NVCMS_ShortyUrls_Update_Click",
            new { short_url = shortUrl },
            commandType: CommandType.StoredProcedure);
    }
}
