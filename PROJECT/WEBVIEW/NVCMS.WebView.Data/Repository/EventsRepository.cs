using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Repository;

public class EventsRepository : IEventsRepository
{
    private readonly string _connectionString;

    public EventsRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    public async Task<IEnumerable<EventsCatModel>> GetActiveCatsAsync(int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<EventsCatModel>(
            "NV_Events_Cat_SelectAllOnlineViewWebsite",
            new { PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<EventsCatModel>> GetAllCatsAsync()
    {
        const string sql = @"
            SELECT *
            FROM   NV_Events_Cat
            WHERE  Isactive = 1
            AND  is_show_website = 1
            ORDER BY Ordernumber, FromDate";

        await using var conn = CreateConn();
        return await conn.QueryAsync<EventsCatModel>(sql);
    }

    public async Task<EventsCatModel?> GetCatByIdAsync(int id)
    {
        const string sql = "SELECT * FROM NV_Events_Cat WHERE id = @id";
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<EventsCatModel>(sql, new { id });
    }

    public async Task<IEnumerable<EventsModel>> GetEventsByCatAsync(int catId, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<EventsModel>(
            "NV_Events_SelectAllByCat",
            new { CatId = catId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<EventsModel?> GetEventByIdAsync(int id)
    {
        const string sql = "SELECT * FROM NV_Events WHERE id = @id";
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<EventsModel>(sql, new { id });
    }

    public async Task<IEnumerable<EventsCatModel>> GetPastCatsAsync(int portalid)
    {
        const string sql = @"
            SELECT * FROM NV_Events_Cat
            WHERE EndDate < GETDATE()
              AND Isactive = 1
              AND PortalId = @portalid
              AND is_show_website = 1
            ORDER BY EndDate DESC";
        await using var conn = CreateConn();
        return await conn.QueryAsync<EventsCatModel>(sql, new { portalid });
    }

    public async Task<(IEnumerable<EventsCatModel> Items, int Total)> GetPastCatsPagedAsync(int portalid, int page, int pageSize)
    {
        const string sql = @"
            SELECT COUNT(*) FROM NV_Events_Cat
            WHERE EndDate < GETDATE() AND Isactive = 1 AND PortalId = @portalid AND is_show_website = 1;

            SELECT * FROM NV_Events_Cat
            WHERE EndDate < GETDATE() AND Isactive = 1 AND PortalId = @portalid AND is_show_website = 1
            ORDER BY EndDate DESC
            OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;";

        await using var conn = CreateConn();
        await using var multi = await conn.QueryMultipleAsync(sql, new { portalid, offset = (page - 1) * pageSize, pageSize });
        var total = await multi.ReadSingleAsync<int>();
        var items = await multi.ReadAsync<EventsCatModel>();
        return (items, total);
    }
}
