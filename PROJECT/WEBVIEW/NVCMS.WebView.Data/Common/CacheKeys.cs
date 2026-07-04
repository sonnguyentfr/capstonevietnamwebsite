namespace NVCMS.WebView.Data.Common;

/// <summary>
/// Centralised cache key factory.
/// All keys include portalId so multi-tenant deployments are safe.
/// </summary>
public static class CacheKeys
{
    // ── News ──────────────────────────────────────────────────────────────────
    public static string AllCategories(int portalId)
        => $"news:cats:all:{portalId}";

    public static string CategoryBySlug(int portalId, string slug)
        => $"news:cat:slug:{portalId}:{slug}";

    public static string CategoryById(int categoryId)
        => $"news:cat:id:{categoryId}";

    public static string NewsDetail(int newId, int portalId)
        => $"news:detail:{portalId}:{newId}";

    public static string NewsList(int portalId, int categoryId, int page, int pageSize)
        => $"news:list:{portalId}:{categoryId}:{page}:{pageSize}";

    public static string NewsListByIds(int portalId, string catIds, int page, int pageSize)
        => $"news:listids:{portalId}:{catIds}:{page}:{pageSize}";

    public static string NewsFeatured(int portalId, int count)
        => $"news:featured:{portalId}:{count}";

    public static string CategoryCounts(int portalId)
        => $"news:catcounts:{portalId}";

    // ── Banners ───────────────────────────────────────────────────────────────
    public static string BannerVitri(int portalId, int vitriId)
        => $"banner:vitri:{portalId}:{vitriId}";

    // ── Events ────────────────────────────────────────────────────────────────
    public static string EventsActive(int portalId)
        => $"events:active:{portalId}";

    public static string EventsCatDetail(int catId, int portalId)
        => $"events:cat:{portalId}:{catId}";

    // ── DoiNgu ────────────────────────────────────────────────────────────────
    public static string DoiNguList(int portalId, int categoryId, int count)
        => $"doingu:list:{portalId}:{categoryId}:{count}";

    // ── Location ──────────────────────────────────────────────────────────────
    public static string Provinces(int parentId)
        => $"loc:provinces:{parentId}";

    // ── TTLs (read-only constants) ────────────────────────────────────────────
    public static readonly TimeSpan TtlCategories  = TimeSpan.FromHours(6);
    public static readonly TimeSpan TtlHomepage    = TimeSpan.FromMinutes(30);
    public static readonly TimeSpan TtlNewsDetail  = TimeSpan.FromMinutes(30);
    public static readonly TimeSpan TtlNewsList    = TimeSpan.FromMinutes(10);
    public static readonly TimeSpan TtlBanner      = TimeSpan.FromMinutes(30);
}
