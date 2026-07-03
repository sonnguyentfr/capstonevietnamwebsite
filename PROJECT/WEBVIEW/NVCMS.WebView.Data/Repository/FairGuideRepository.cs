using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;
using System.Data;

namespace NVCMS.WebView.Data.Repository;

public class FairGuideRepository : IFairGuideRepository
{
    private readonly string _connectionString;

    public FairGuideRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    public async Task<IEnumerable<FairGuideModel>> GetAllActiveAsync(int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<FairGuideModel>(
            "WebView_NVCMS_Fairguide_SelectAllView",
            new { PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<FairGuideModel?> GetByIdAsync(int id, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<FairGuideModel>(
            "WebView_NVCMS_Fairguide_SelectById",
            new { Id = id, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
        
    }

    public async Task<IEnumerable<FairGuideMediaModel>> GetMediaAsync(int fairGuideId, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<FairGuideMediaModel>(
            "WebView_NVCMS_Fairguide_SelectMediaById",
            new { FairGuideId = fairGuideId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
       
    }
}
