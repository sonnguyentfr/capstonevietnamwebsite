# Performance Audit — Capstone Vietnam Website

> Audited: ASP.NET Core 10 MVC · Dapper · SQL Server · .NET 10

---

## Executive Summary

| Area | Severity | Issues Found |
|---|---|---|
| N+1 DB queries | 🔴 HIGH | 3 patterns |
| Missing caching on hot paths | 🔴 HIGH | 6 services |
| Response compression disabled | 🔴 HIGH | — |
| Static file cache headers missing | 🔴 HIGH | — |
| Repeated `GetAllCategories` calls | 🟠 MEDIUM | 4 call sites |
| `EventsService` sequential awaits | 🟠 MEDIUM | GetCatWithEventsAsync |
| `IConfiguration` read per request | 🟡 LOW | 4 ViewComponents |
| Missing `async/await` on fire-and-forget | 🟡 LOW | IncrementViewCount |

---

## Bottlenecks Found

### B1 — N+1: `NewsService.GetAllPagedAsync`
**File**: `NewsService.cs:93`
For each news item, a separate `GetCategoryByIdAsync` is fired via `Task.WhenAll`.
With `pageSize=27`, that's 27 parallel DB connections opened each request.
**Fix**: Load all categories once (`GetAllCategoriesAsync`) and join in memory.

### B2 — N+1: `NewsService.GetFeaturedAsync`
**File**: `NewsService.cs:147`
Same pattern — fetches categories individually per featured item.
**Fix**: Already patched in previous session to use `GetAllCategoriesAsync`. Now wrap in cache.

### B3 — Repeated `GetAllCategoriesAsync` — every category page
**File**: `NewsController.CategoryBySlug` calls both `GetCategoryBySlugAsync` AND `GetCategoriesWithCountAsync`, each of which calls `GetAllCategoriesAsync` internally. That's **2 full category table scans per request**.
**Fix**: Cache the category list with 6h TTL.

### B4 — No caching on `BannerService.GetAllShowAsync`
Banners are called on every page via `BannerViewComponent`. Data never changes in production. Zero cache.
**Fix**: `IMemoryCache` with 30 min TTL, keyed by `(portalId, vitriid)`.

### B5 — No caching on `EventsService` (homepage + index)
`EventsViewComponent` calls `GetActiveCatsWithEventsAsync(50)` on every homepage hit. This runs multiple SQL queries (one per cat to fetch events).
**Fix**: Cache with 30 min TTL.

### B6 — No caching on `NewsFeatured` / `DoiNguCoVan`
Both ViewComponents call the DB on every request, including bot crawls.
**Fix**: Cache with 30 min TTL.

### B7 — `DoiNguCoVanViewComponent` makes 2 sequential DB calls
`GetByCategoryIdAsync` + `GetCategoryByIdAsync` — the second is a separate round-trip just for the category name.
**Fix**: Reuse the category info already returned from the first call.

### B8 — Response Compression not enabled
No `app.UseResponseCompression()` or Brotli/Gzip in `Program.cs`.
HTML pages of ~80KB would compress to ~15KB (5x). CSS/JS similarly.
**Fix**: Add `AddResponseCompression` with Brotli + Gzip.

### B9 — Static file cache headers missing
`app.UseStaticFiles()` has no `CacheControl`. Browsers re-validate every asset on every page load.
**Fix**: Set `Cache-Control: public, max-age=31536000` for immutable assets (JS, CSS, fonts, images).

### B10 — `IConfiguration.GetValue` per request in ViewComponents
`BannerViewComponent`, `EventsViewComponent`, `DoiNguCoVanViewComponent`, `NewsFeaturedViewComponent` all call `config.GetValue<int>("SiteSettings:PortalId")` per invocation.
**Fix**: Read once in constructor or use `IOptions<SiteSettings>`.

### B11 — `IncrementViewCountAsync` blocks response
News detail waits for a fire-and-forget UPDATE before rendering. This adds ~5-10ms latency with no user benefit.
**Fix**: Fire without `await` using `_ = Task.Run(...)`.

### B12 — EventsService: sequential per-event school/org resolution
`GetCatWithEventsAsync` iterates events sequentially with `await` inside `foreach`, causing serial DB roundtrips proportional to event count.
**Fix**: Parallelize with `Task.WhenAll` after collecting all IDs.

---

## Prioritized Checklist

### 🔴 High Impact / Low Risk (implement now)
- [x] Enable Response Compression (Brotli + Gzip)
- [x] Add static file Cache-Control headers
- [x] Cache `GetAllCategoriesAsync` — 6h TTL
- [x] Cache `BannerService.GetAllShowAsync` — 30 min
- [x] Cache `NewsFeaturedViewComponent` result — 30 min
- [x] Cache `EventsViewComponent` result — 30 min
- [x] Cache `DoiNguCoVanViewComponent` result — 30 min
- [x] Fix N+1 in `GetAllPagedAsync`
- [x] Parallelize EventsService school/org resolution

### 🟠 Medium Impact
- [x] Fire-and-forget `IncrementViewCountAsync`
- [x] Fix IConfiguration read per request
- [x] Cache news detail pages — 30 min
- [x] Cache news list pages — 10 min
- [x] Add request timing middleware

### 🟡 Optional / Low Risk
- [ ] Add DB indexes (see Index Recommendations below)
- [ ] Add `width`/`height` to images in views
- [ ] Add `loading="lazy"` to below-fold images (partially done)
- [ ] Defer non-critical JS

---

## Index Recommendations (SQL Server)

Run these in production DB to speed up hot queries:

```sql
-- News list by category + portal + date
CREATE INDEX IX_News_CategoryId_PortalId_PublishedDate
  ON News (CategoryId, PortalId, PublishedDate DESC)
  INCLUDE (NewId, Title, MetaUrl, ImagePath, Summary, IsActive);

-- News by ID + portal (detail page)
CREATE INDEX IX_News_NewId_PortalId
  ON News (NewId, PortalId)
  WHERE IsActive = 1;

-- News_Settings ordering
CREATE INDEX IX_NewsSettings_PortalId_Order
  ON News_Settings (PortalId, OrderNumber)
  INCLUDE (NewId);

-- NewsCategories slug lookup
CREATE INDEX IX_NewsCategories_Slug_PortalId
  ON NewsCategories (Slug, PortalId)
  INCLUDE (CategoryId, CategoryName, ParentId, Description);

-- NV_Events by CatId + PortalId
CREATE INDEX IX_NVEvents_CatId_Portalid
  ON NV_Events (CatId, Portalid)
  INCLUDE (Id, Title, Diadiem, Fromdatetime, Enddatetime, School, Org, Avatar);

-- NV_Truong partner lookup
CREATE INDEX IX_NVTruong_isPartner_Country
  ON NV_Truong (isPartner, Country)
  INCLUDE (Id, NameofSchool, Logo, Conver, Loai, Slug);
```
