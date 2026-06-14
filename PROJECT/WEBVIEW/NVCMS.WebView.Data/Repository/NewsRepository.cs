using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Repository;

public class NewsRepository : INewsRepository
{
    private readonly string _connectionString;

    public NewsRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    public async Task<PaginatedList<NewsModel>> GetAllPagedAsync(int portalId, int page, int pageSize)
    {
        await using var conn = CreateConn();
        var param = new { PortalId = portalId, PageIndex = page, PageSize = pageSize };
        using var multi = await conn.QueryMultipleAsync(
            "WebView_NVCMS_News_SelectAll",
            param,
            commandType: CommandType.StoredProcedure);
        var total = await multi.ReadFirstOrDefaultAsync<int>();
        var items = await multi.ReadAsync<NewsModel>();
        return new PaginatedList<NewsModel>(items, total, page, pageSize);
    }

    public async Task<IEnumerable<(int CategoryId, int Count)>> GetCategoryCountsAsync(int portalId)
    {
        await using var conn = CreateConn();
        var rows = await conn.QueryAsync(
            "WebView_NVCMS_NewsCategory_SelectWithCount",
            new { PortalId = portalId },
            commandType: CommandType.StoredProcedure);
        return rows.Select(r => ((int)r.CategoryID, (int)r.NewsCount));
    }

    public async Task<PaginatedList<NewsModel>> GetByCategoryIdsAsync(
        IEnumerable<int> categoryIds, int portalId, int page, int pageSize)
    {
        await using var conn = CreateConn();
        var param = new
        {
            CategoryIds = string.Join(",", categoryIds),
            PortalId    = portalId,
            PageIndex   = page,
            PageSize    = pageSize
        };
        using var multi = await conn.QueryMultipleAsync(
            "WebView_NVCMS_News_SelectByCategoryIds",
            param,
            commandType: CommandType.StoredProcedure);
        var total = await multi.ReadFirstOrDefaultAsync<int>();
        var items = await multi.ReadAsync<NewsModel>();
        return new PaginatedList<NewsModel>(items, total, page, pageSize);
    }

    public async Task<PaginatedList<NewsModel>> GetByCategoryAsync(
        int categoryId, int portalId, int page, int pageSize)
    {
        await using var conn = CreateConn();
        var param = new
        {
            CategoryId = categoryId,
            PortalId = portalId,
            PageIndex = page,
            PageSize = pageSize
        };
        using var multi = await conn.QueryMultipleAsync(
            "WebView_NVCMS_News_SelectByCategory",
            param,
            commandType: CommandType.StoredProcedure);
        var total = await multi.ReadFirstOrDefaultAsync<int>();
        var items = await multi.ReadAsync<NewsModel>();
        return new PaginatedList<NewsModel>(items, total, page, pageSize);
    }

    public async Task<NewsModel?> GetByIdAsync(int newId, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<NewsModel>(
            "WebView_NVCMS_News_SelectByID",
            new { NewId = newId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<NewsModel>> GetRelatedAsync(
        int categoryId, int excludeId, int portalId, int top = 5)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<NewsModel>(
            "WebView_NVCMS_News_SelectRelated",
            new { CategoryId = categoryId, ExcludeId = excludeId, PortalId = portalId, Top = top },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<NewsCategoryModel>> GetAllCategoriesAsync(int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<NewsCategoryModel>(
            "WebView_NVCMS_NewsCategory_SelectAll",
            new { PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<NewsCategoryModel?> GetCategoryByIdAsync(int categoryId)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<NewsCategoryModel>(
            "WebView_NVCMS_NewsCategory_SelectByID",
            new { CategoryId = categoryId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<NewsCategoryModel?> GetCategoryBySlugAsync(string slug, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<NewsCategoryModel>(
            "WebView_NVCMS_NewsCategory_SelectBySlug",
            new { Slug = slug, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task IncrementViewCountAsync(int newId)
    {
        await using var conn = CreateConn();
        await conn.ExecuteAsync(
            "WebView_NVCMS_News_UpdateView",
            new { NewId = newId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<NewsModel>> GetFeaturedAsync(int portalId, int top)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<NewsModel>(
            "WebView_NVCMS_NewsSettings_Select",
            new { PortalId = portalId, Top = top },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<TruongModel>> GetSchoolsByNewsAsync(int newId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<TruongModel>(
            "WebView_NewsBySchool_GetSchoolsByNews",
            new { NewId = newId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<NewsModel>> GetNewsBySchoolAsync(int schoolId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<NewsModel>(
            "WebView_NewsBySchool_GetNewsBySchool",
            new { SchoolId = schoolId },
            commandType: CommandType.StoredProcedure);
    }
}