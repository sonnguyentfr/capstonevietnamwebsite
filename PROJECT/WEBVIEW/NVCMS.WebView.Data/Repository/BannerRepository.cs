using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Repository;

public class BannerRepository : IBannerRepository
{
    private readonly string _connectionString;

    public BannerRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    public async Task<IEnumerable<BannerModel>> GetAllAsync(int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<BannerModel>(
            "NVCMS_Banner_SelectAll",
            new { PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<BannerModel>> GetAllShowAsync(int portalId, int vitri)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<BannerModel>(
            "NVCMS_Banner_SelectAllShow",
            new { PortalId = portalId, Vitri = vitri },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<BannerModel>> GetByVitriAsync(int vitri, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<BannerModel>(
            "NVCMS_Banner_SelectAllVitri",
            new { Vitri = vitri, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<BannerModel?> GetByIdAsync(int bannerId)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<BannerModel>(
            "NVCMS_Banner_SelectByID",
            new { BannerId = bannerId },
            commandType: CommandType.StoredProcedure);
    }

    // 2 SP này của module DNN cũ được gọi bằng SqlHelper (truyền tham số theo VỊ TRÍ),
    // tên tham số trong SP không phải @BannerId → truyền theo tên sẽ lỗi.
    // Dùng EXEC positional để không phụ thuộc tên tham số.
    public async Task UpdateClickAsync(int bannerId)
    {
        await using var conn = CreateConn();
        await conn.ExecuteAsync("EXEC NVCMS_Banner_UpdateClick @Id", new { Id = bannerId });
    }

    public async Task UpdateViewAsync(IEnumerable<int> bannerIds)
    {
        var ids = bannerIds.Select(id => new { Id = id }).ToList();
        if (ids.Count == 0) return;

        await using var conn = CreateConn();
        await conn.ExecuteAsync("EXEC NVCMS_Banner_UpdateView @Id", ids);
    }
}