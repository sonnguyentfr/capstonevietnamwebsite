using System.Data;
using System.Net;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Repository;

public class GioiThieuRepository : IGioiThieuRepository
{
    private readonly string _connectionString;

    public GioiThieuRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    public async Task<GioiThieuModel?> GetByIdAsync(int id, int portalId)
    {
        const string sql = "NVCMS_PageGioiThieu_SelectByID";
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<GioiThieuModel>(
            sql,
            new { id, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<GioiThieuModel>> GetAllAsync(int portalId)
    {
        const string sql = "NVCMS_PageGioiThieu_SelectAll";
        await using var conn = CreateConn();
        return await conn.QueryAsync<GioiThieuModel>(
            sql,
            new { PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<GioiThieuModel>> GetAllByParentIdAsync(int parentId, int portalId)
    {
        const string sql = "NVCMS_PageGioiThieu_SelectAllByParentId";
        await using var conn = CreateConn();
        return await conn.QueryAsync<GioiThieuModel>(
            sql,
            new { ParentId = parentId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }
}
