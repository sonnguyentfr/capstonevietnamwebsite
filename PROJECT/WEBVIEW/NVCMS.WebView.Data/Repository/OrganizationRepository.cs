using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Repository;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly string _cs;
    public OrganizationRepository(string connectionString) => _cs = connectionString;
    private SqlConnection CreateConn() => new(_cs);

    public async Task<IEnumerable<OrganizationModel>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.ToList();
        if (idList.Count == 0) return [];
        var inClause = string.Join(",", idList);
        var sql = $@"
            SELECT Id, Name, Logo, Website, Email, Phone, Diachi, quocgia
            FROM   Cap_Organization
            WHERE  Id IN ({inClause})";
        await using var conn = CreateConn();
        return await conn.QueryAsync<OrganizationModel>(sql);
    }
}
