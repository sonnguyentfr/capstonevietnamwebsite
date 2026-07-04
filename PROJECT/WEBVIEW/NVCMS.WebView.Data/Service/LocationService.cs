using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Data;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Service;

public class LocationService : ILocationService
{
    private readonly IDbContextFactory<LocationDbContext> _factory;
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(12);

    public LocationService(IDbContextFactory<LocationDbContext> factory, IMemoryCache cache)
    {
        _factory = factory;
        _cache   = cache;
    }

    public async Task<IReadOnlyList<CapLocationModel>> GetProvincesAsync(int parentId)
    {
        var key = $"loc:provinces:{parentId}";
        if (_cache.TryGetValue(key, out IReadOnlyList<CapLocationModel>? cached) && cached is not null)
            return cached;

        await using var db = await _factory.CreateDbContextAsync();
        var list = await db.Locations
            .Where(x => x.ParentId == parentId && (x.Status == null || x.Status == true))
            .OrderBy(x => x.Ordernumber ?? int.MaxValue)
            .ThenBy(x => x.Name)
            .AsNoTracking()
            .ToListAsync();

        IReadOnlyList<CapLocationModel> result = list;
        _cache.Set(key, result, CacheTtl);
        return result;
    }
}
