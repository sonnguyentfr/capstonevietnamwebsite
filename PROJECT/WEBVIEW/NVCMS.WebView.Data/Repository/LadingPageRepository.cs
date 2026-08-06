using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Repository;

public class LadingPageRepository : ILadingPageRepository
{
    private readonly string _connectionString;

    public LadingPageRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    public async Task<IEnumerable<NVCMS_LadingPageModel>> GetAllAsync(int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<NVCMS_LadingPageModel>(
            "NVCMS_LadingPage_SelectAll",
            new { PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<NVCMS_LadingPageModel>> GetAllByParentIdAsync(int parentId, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<NVCMS_LadingPageModel>(
            "NVCMS_LadingPage_SelectAllByParentId",
            new { ParentId = parentId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<NVCMS_LadingPageModel?> GetByIdAsync(int id, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<NVCMS_LadingPageModel>(
            "NVCMS_LadingPage_SelectByID",
            new { id = id, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }
}
