using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Repository;

public class TruongRepository : ITruongRepository
{
    private readonly string _cs;
    public TruongRepository(string connectionString) => _cs = connectionString;
    private SqlConnection CreateConn() => new(_cs);

    // Country Id mapping — matches Cap_Location.LocationId in CapstoneVietnam_old
    // 1=Úc, 3=Canada, 23=Thụy Sĩ, 28=Anh, 38=Mỹ, 99=New Zealand, 401=Ireland
    private static readonly Dictionary<int, string> CountryNames = new()
    {
        {1,  "Úc"},
        {3,  "Canada"},
        {23, "Thụy Sĩ"},
        {28, "Anh"},
        {38, "Mỹ"},
        {99, "New Zealand"},
        {401,"Ireland"},
    };

    // ----------------------------------------------------------------
    // SP: WebView_Truong_Search
    // Result set 1 = TotalCount, Result set 2 = paged rows
    // ----------------------------------------------------------------
    public async Task<(IEnumerable<TruongModel> Items, int Total)> SearchAsync(TruongSearchFilterViewModel f)
    {
        await using var conn = CreateConn();

        // Khi có Letter (lọc chữ cái đầu tên trường) thì dùng inline SQL
        // vì SP dùng LIKE '%'+@Ten+'%' — không thể lọc prefix đúng.
        if (!string.IsNullOrWhiteSpace(f.Letter) && string.IsNullOrWhiteSpace(f.Ten))
        {
            return await SearchByLetterAsync(conn, f);
        }

        var p = new DynamicParameters();
        p.Add("Ten",        string.IsNullOrWhiteSpace(f.Ten) ? (object?)null : f.Ten);
        p.Add("QuocGia",    f.QuocGia);
        p.Add("Loai",       string.IsNullOrWhiteSpace(f.Loai) ? (object?)null : f.Loai);
        p.Add("IsPartner",  f.IsPartner);
        p.Add("MajorId",    f.MajorId);
        p.Add("TuitionMax", f.TuitionMax);
        p.Add("Page",       f.Page);
        p.Add("PageSize",   f.PageSize);

        using var multi = await conn.QueryMultipleAsync(
            "WebView_Truong_Search", p, commandType: CommandType.StoredProcedure);

        var total = await multi.ReadSingleAsync<int>();
        var items = await multi.ReadAsync<TruongModel>();
        return (items, total);
    }

    private static async Task<(IEnumerable<TruongModel> Items, int Total)> SearchByLetterAsync(
        SqlConnection conn, TruongSearchFilterViewModel f)
    {
        var p = new DynamicParameters();
        p.Add("Letter",     f.Letter!.Trim().ToUpper()[0].ToString());
        p.Add("QuocGia",    f.QuocGia);
        p.Add("Loai",       string.IsNullOrWhiteSpace(f.Loai) ? (object?)null : f.Loai);
        p.Add("IsPartner",  f.IsPartner);
        p.Add("MajorId",    f.MajorId);
        p.Add("TuitionMax", f.TuitionMax);
        p.Add("Page",       f.Page);
        p.Add("PageSize",   f.PageSize);

        using var multi = await conn.QueryMultipleAsync(
            "WebView_Truong_SearchByLetter", p, commandType: CommandType.StoredProcedure);

        var total = await multi.ReadSingleAsync<int>();
        var items = await multi.ReadAsync<TruongModel>();
        return (items, total);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_RandomPartners
    // ----------------------------------------------------------------
    public async Task<IEnumerable<TruongModel>> GetRandomPartnersAsync(int count, int? portalId = null)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<TruongModel>(
            "WebView_Truong_RandomPartners",
            new { Count = count, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetByCountry
    // Result set 1 = TotalCount, Result set 2 = paged rows
    // ----------------------------------------------------------------
    public async Task<IEnumerable<TruongModel>> GetByCountryAsync(
        int countryId, string? loai = null, int? portalId = null)
    {
        // Default: lay het (pageSize lon) de dung nhu truoc, khong can phan trang o Service layer
        await using var conn = CreateConn();

        var p = new DynamicParameters();
        p.Add("CountryId", countryId);
        p.Add("Loai",      string.IsNullOrWhiteSpace(loai) ? (object?)null : loai);
        p.Add("PortalId",  portalId);
        p.Add("Page",      1);
        p.Add("PageSize",  500); // lay toan bo, Controller co the truyen PageSize nho hon neu can

        using var multi = await conn.QueryMultipleAsync(
            "WebView_Truong_GetByCountry", p, commandType: CommandType.StoredProcedure);

        await multi.ReadSingleAsync<int>(); // bo qua TotalCount
        return await multi.ReadAsync<TruongModel>();
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetById
    // ----------------------------------------------------------------
    public async Task<TruongModel?> GetByIdAsync(int id)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<TruongModel>(
            "WebView_Truong_GetById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // Inline SQL: lấy nhiều trường theo danh sách ID (dùng cho sự kiện)
    // ----------------------------------------------------------------
    public async Task<IEnumerable<TruongModel>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.ToList();
        if (idList.Count == 0) return [];
        var inClause = string.Join(",", idList);
        var sql = $"SELECT * FROM Cap_Truong WHERE Id IN ({inClause})";
        await using var conn = CreateConn();
        return await conn.QueryAsync<TruongModel>(sql);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetAdmis4Year
    // ----------------------------------------------------------------
    public async Task<TruongAdmis4YearModel?> GetAdmis4YearAsync(int truongId, int? portalId = null)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<TruongAdmis4YearModel>(
            "WebView_Truong_GetAdmis4Year",
            new { TruongId = truongId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetAdmisBF
    // ----------------------------------------------------------------
    public async Task<TruongAdmisBFModel?> GetAdmisBFAsync(int truongId, int? portalId = null)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<TruongAdmisBFModel>(
            "WebView_Truong_GetAdmisBF",
            new { TruongId = truongId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetAdmisESL
    // ----------------------------------------------------------------
    public async Task<TruongAdmisESLModel?> GetAdmisESLAsync(int truongId, int? portalId = null)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<TruongAdmisESLModel>(
            "WebView_Truong_GetAdmisESL",
            new { TruongId = truongId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetMajorsByTruong
    // ----------------------------------------------------------------
    public async Task<IEnumerable<TruongMajorModel>> GetMajorsByTruongAsync(int truongId)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<TruongMajorModel>(
            "WebView_Truong_GetMajorsByTruong",
            new { TruongId = truongId },
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetAllMajors
    // ----------------------------------------------------------------
    public async Task<IEnumerable<TruongMajorModel>> GetAllMajorsAsync()
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<TruongMajorModel>(
            "WebView_Truong_GetAllMajors",
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetMajorsWithCount
    // ----------------------------------------------------------------
    public async Task<IEnumerable<TruongMajorModel>> GetMajorsWithCountAsync(int? quocGiaId, string? loai)
    {
        await using var conn = CreateConn();
        return await conn.QueryAsync<TruongMajorModel>(
            "WebView_Truong_GetMajorsWithCount",
            new
            {
                QuocGiaId = quocGiaId,
                Loai      = string.IsNullOrWhiteSpace(loai) ? (object?)null : loai
            },
            commandType: CommandType.StoredProcedure);
    }

    // ----------------------------------------------------------------
    // SP: WebView_Truong_GetCountriesWithCount
    // ----------------------------------------------------------------
    public async Task<IEnumerable<(int Id, string Ten, int Count)>> GetCountriesWithCountAsync(bool? isPartner = null)
    {
        await using var conn = CreateConn();

        var rows = await conn.QueryAsync<(int Id, int TruongCount)>(
            "WebView_Truong_GetCountriesWithCount",
            new { IsPartner = isPartner.HasValue ? (object)(isPartner.Value ? 1 : 0) : null },
            commandType: CommandType.StoredProcedure);

        return rows.Select(r => (
            r.Id,
            CountryNames.TryGetValue(r.Id, out var n) ? n : $"Quốc gia {r.Id}",
            r.TruongCount));
    }
}
