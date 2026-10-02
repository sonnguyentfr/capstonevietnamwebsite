using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Repository;

public class TuVanDangKyRepository : ITuVanDangKyRepository
{
    private readonly string _connectionString;

    public TuVanDangKyRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    // ── Danh mục ──────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<TuVanOption>> GetBacHocAsync(int portalId)
    {
        const string sql = @"
            SELECT id AS Id, LTRIM(RTRIM(Title)) AS Title
            FROM   Cap_CapGiaoduc
            WHERE  ISNULL(IsActive, 1) = 1 AND PortalId = @portalId
            ORDER BY ISNULL(Ordernumber, 0), id";

        await using var conn = CreateConn();
        var rows = await conn.QueryAsync<TuVanOption>(sql, new { portalId });
        return rows.AsList();
    }

    public async Task<IReadOnlyList<string>> GetNganhHocAsync()
    {
        const string sql = @"
            SELECT DISTINCT LTRIM(RTRIM(TitleVN))
            FROM   Cap_Truong_Major
            WHERE  ISNULL(LTRIM(RTRIM(TitleVN)), '') <> ''
            ORDER BY 1";

        await using var conn = CreateConn();
        var rows = await conn.QueryAsync<string>(sql);
        return rows.AsList();
    }

    public async Task<IReadOnlyList<TuVanOption>> GetKhaNangChiTraAsync(int portalId)
    {
        // Kieudulieu dạng "10000;15000" → sắp theo mức dưới
        const string sql = @"
            SELECT id AS Id, LTRIM(RTRIM(Title)) AS Title
            FROM   Student_TuVanInfo_ChiTra
            WHERE  PortalId = @portalId
            ORDER BY TRY_CAST(LEFT(Kieudulieu, CHARINDEX(';', Kieudulieu + ';') - 1) AS INT), id";

        await using var conn = CreateConn();
        var rows = await conn.QueryAsync<TuVanOption>(sql, new { portalId });
        return rows.AsList();
    }

    // ── Upsert Student_Info ───────────────────────────────────────────────────

    public async Task<(int StudentId, string StudentCode, bool IsExisting)> UpsertAsync(
        TuVanUpsertArgs a, CancellationToken ct = default)
    {
        var p = new DynamicParameters();
        p.Add("Hotendem",            a.Hotendem);
        p.Add("Ten",                 a.Ten);
        p.Add("Sex",                 a.Sex);
        p.Add("Ngaysinh",            a.Ngaysinh, dbType: DbType.DateTime);
        p.Add("Sodienthoai",         a.Sodienthoai);
        p.Add("Email",               a.Email);
        p.Add("Diachi",              a.Diachi);
        p.Add("TinhId",              a.TinhId, dbType: DbType.Int32);
        p.Add("TuVanHocVanmongmuon", a.TuVanHocVanmongmuon);
        p.Add("TuVanNamdi",          a.TuVanNamdi);
        p.Add("TuVanNganhhoc",       a.TuVanNganhhoc);
        p.Add("TuVanKhanangchitra",  a.TuVanKhanangchitra);
        p.Add("TuVanQuocgia",        a.TuVanQuocgia);
        p.Add("TuVanKhac",           a.TuVanKhac);
        p.Add("HinhThuc",            a.HinhThuc);
        p.Add("CodePrefix",          a.CodePrefix);
        p.Add("PortalId",            a.PortalId);
        p.Add("StudentId",   dbType: DbType.Int32,   direction: ParameterDirection.Output);
        p.Add("StudentCode", dbType: DbType.String,  direction: ParameterDirection.Output, size: 50);
        p.Add("IsExisting",  dbType: DbType.Boolean, direction: ParameterDirection.Output);

        await using var conn = CreateConn();
        await conn.ExecuteAsync(new CommandDefinition(
            "WebView_TuVan_Upsert", p,
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));

        return (p.Get<int>("StudentId"),
                p.Get<string>("StudentCode") ?? string.Empty,
                p.Get<bool>("IsExisting"));
    }
}
