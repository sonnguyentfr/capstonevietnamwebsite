using Capstone.View.Helpers;
using Capstone.View.Middleware;
using Capstone.View.Options;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;
using NVCMS.WebView.Data;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(SiteSettings.SectionName));
builder.Services.AddSingleton<ContentUrlHelper>();

// ── Response Compression (Brotli first, then Gzip) ────────────────────────────
builder.Services.AddResponseCompression(opts =>
{
    opts.EnableForHttps = true;
    opts.Providers.Add<BrotliCompressionProvider>();
    opts.Providers.Add<GzipCompressionProvider>();
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    [
        "text/html", "text/css", "application/javascript",
        "application/json", "image/svg+xml", "font/woff2",
        "text/plain", "application/xml"
    ]);
});
builder.Services.Configure<BrotliCompressionProviderOptions>(o =>
    o.Level = System.IO.Compression.CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(o =>
    o.Level = System.IO.Compression.CompressionLevel.Fastest);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
var crmConnStr = builder.Configuration.GetConnectionString("CRMConnection")!;
var webRootPath = builder.Environment.WebRootPath;
var serverFilesBaseUrl = builder.Configuration["SiteSettings:ServerFilesBaseUrl"] ?? string.Empty;
builder.Services.AddWebViewData(connStr, crmConnStr, webRootPath, serverFilesBaseUrl);

builder.Services.AddScoped<ITuVanFormService>(sp =>
    new TuVanFormService(connStr, sp.GetRequiredService<IConfiguration>()));

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost/");
});

var app = builder.Build();

// ── Response Compression — must be first ─────────────────────────────────────
app.UseResponseCompression();

// ── Request Timing (logs slow requests > 500ms, adds Server-Timing header) ───
app.UseMiddleware<RequestTimingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/404");

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;
    if (path.EndsWith(".html", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".htm", StringComparison.OrdinalIgnoreCase))
    {
        var newPath = path[..path.LastIndexOf('.')];
        context.Response.Redirect(newPath + context.Request.QueryString, permanent: true);
        return;
    }
    await next();
});

// ── Static Files with aggressive Cache-Control ────────────────────────────────
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = false,
    OnPrepareResponse = ctx =>
    {
        var ext = Path.GetExtension(ctx.File.Name).ToLowerInvariant();
        var headers = ctx.Context.Response.GetTypedHeaders();

        // Immutable assets (fingerprinted by build tools or rarely change)
        if (ext is ".css" or ".js" or ".woff" or ".woff2" or ".ttf" or ".eot" or ".otf")
        {
            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(365),
                // Uncomment below if assets use content-hash filenames:
                // Extensions = { "immutable" }
            };
        }
        // Images & SVG — 30 days
        else if (ext is ".png" or ".jpg" or ".jpeg" or ".gif" or ".svg" or ".webp" or ".ico" or ".avif")
        {
            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(30)
            };
        }
        // Everything else — 1 hour
        else
        {
            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromHours(1)
            };
        }
    }
});

// ShortUrl: chạy trước UseRouting để ưu tiên short_url hơn bất kỳ route nào
app.UseMiddleware<ShortUrlMiddleware>();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "gioi-thieu-ve-capstone", pattern: "gioi-thieu/ve-capstone", defaults: new { controller = "GioiThieu", action = "VeCapstone" });
app.MapControllerRoute(name: "gioi-thieu", pattern: "gioi-thieu", defaults: new { controller = "GioiThieu", action = "Index" });
app.MapControllerRoute(name: "quy-trinh-tu-van", pattern: "gioi-thieu/quy-trinh-tu-van", defaults: new { controller = "GioiThieu", action = "QuyTrinhTuVan" });

app.MapControllerRoute(name: "su-kien-past-paged", pattern: "su-kien/past-paged", defaults: new { controller = "SuKien", action = "PastPaged" });
app.MapControllerRoute(name: "su-kien-detail", pattern: "su-kien/{slug}-{id:int}", defaults: new { controller = "SuKien", action = "Detail" });
app.MapControllerRoute(name: "su-kien", pattern: "su-kien", defaults: new { controller = "SuKien", action = "Index" });

// Các dịch vụ Capstone
app.MapControllerRoute(name: "dich-vu-index", pattern: "gioi-thieu/cac-dich-vu-capstone", defaults: new { controller = "DichVu", action = "Index" });
app.MapControllerRoute(name: "tu-van-dinh-cu", pattern: "tu-van-dinh-cu", defaults: new { controller = "DichVu", action = "TuVanDinhCu" });
app.MapControllerRoute(name: "tu-van-du-hoc-cac-nuoc", pattern: "gioi-thieu/cac-dich-vu-capstone/tu-van-du-hoc-cac-nuoc", defaults: new { controller = "DichVu", action = "TuVanDuHocCacNuoc" });
app.MapControllerRoute(name: "tu-van-du-hoc-truong-top", pattern: "gioi-thieu/cac-dich-vu-capstone/tu-van-du-hoc-truong-top", defaults: new { controller = "DichVu", action = "TuVanDuHocTruongTop" });
app.MapControllerRoute(name: "tu-van-du-hoc-cao-hoc", pattern: "gioi-thieu/cac-dich-vu-capstone/tu-van-du-hoc-cao-hoc", defaults: new { controller = "DichVu", action = "TuVanDuHocCaoHoc" });

// Trường đối tác
app.MapControllerRoute(name: "truong-search-json", pattern: "truong-doi-tac/search-json", defaults: new { controller = "Truong", action = "SearchJson" });
app.MapControllerRoute(name: "truong-detail", pattern: "truong-doi-tac/{slug}-{id:int}", defaults: new { controller = "Truong", action = "Detail" });
app.MapControllerRoute(name: "truong-quocgia", pattern: "truong-doi-tac/{countrySlug}", defaults: new { controller = "Truong", action = "QuocGia" });
app.MapControllerRoute(name: "truong-index", pattern: "truong-doi-tac", defaults: new { controller = "Truong", action = "Index" });
app.MapControllerRoute(name: "tim-truong", pattern: "tim-truong", defaults: new { controller = "Truong", action = "TimTruong" });
app.MapControllerRoute(name: "tim-nganh-hoc", pattern: "tim-nganh-hoc", defaults: new { controller = "Truong", action = "TimNganhHoc" });

// Thông tin du học – danh sách trường theo quốc gia
app.MapControllerRoute(name: "thong-tin-du-hoc-danh-sach-truong",
    pattern: "thong-tin-du-hoc/{countrySlug}/danh-sach-truong",
    defaults: new { controller = "ThongTinDuHoc", action = "DanhSachTruong" });
app.MapControllerRoute(name: "tu-van-nganh-nghe", pattern: "gioi-thieu/cac-dich-vu-capstone/tu-van-nganh-nghe", defaults: new { controller = "DichVu", action = "TuVanNganhNghe" });
app.MapControllerRoute(name: "tu-van-visa", pattern: "gioi-thieu/cac-dich-vu-capstone/tu-van-visa-du-hoc-tham-than", defaults: new { controller = "DichVu", action = "TuVanVisa" });
app.MapControllerRoute(name: "chuyen-tien-du-hoc", pattern: "gioi-thieu/cac-dich-vu-capstone/dich-vu-chuyen-tien-du-hoc", defaults: new { controller = "DichVu", action = "ChuyenTienDuHoc" });
app.MapControllerRoute(name: "tim-nha", pattern: "gioi-thieu/cac-dich-vu-capstone/dich-vu-tim-nha", defaults: new { controller = "DichVu", action = "TimNha" });

// Tin tức – index, all, category, detail
app.MapControllerRoute(name: "news-all",      pattern: "tin-tuc",              defaults: new { controller = "News", action = "All" });
app.MapControllerRoute(name: "news-detail",   pattern: "tin-tuc/{slug}-{id:int}", defaults: new { controller = "News", action = "Detail" });

// ── Routes danh mục tin tức theo section ──────────────────────────────────────
// /cam-nang-su-kien-du-hoc               VD: /cam-nang-su-kien-du-hoc  (root – categoryId=67)
app.MapControllerRoute(name: "cam-nang-su-kien-du-hoc-root", pattern: "cam-nang-su-kien-du-hoc", defaults: new { controller = "News", action = "CategoryBySlug", slug = "cam-nang-su-kien-du-hoc" });
// /cam-nang-su-kien-du-hoc/{catSlug}/{slug}-{id}   VD: /cam-nang-su-kien-du-hoc/chia-se-tu-du-hoc-sinh/ten-bai-7261
app.MapControllerRoute(name: "cam-nang-su-kien-du-hoc-detail", pattern: "cam-nang-su-kien-du-hoc/{catSlug}/{slug}-{id:int}", defaults: new { controller = "News", action = "CamNangSuKienDetail" });
// /cam-nang-su-kien-du-hoc/{slug}        VD: /cam-nang-su-kien-du-hoc/tin-tuc-chung
app.MapControllerRoute(name: "news-camnangtintuc", pattern: "cam-nang-su-kien-du-hoc/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// /guong-mat-thanh-cong  (root)
app.MapControllerRoute(name: "guong-mat-thanh-cong-root", pattern: "guong-mat-thanh-cong", defaults: new { controller = "News", action = "CategoryBySlug", slug = "guong-mat-thanh-cong" });
// /guong-mat-thanh-cong/{catSlug}/{slug}-{id}
app.MapControllerRoute(name: "guong-mat-thanh-cong-detail", pattern: "guong-mat-thanh-cong/{catSlug}/{slug}-{id:int}", defaults: new { controller = "News", action = "SectionDetail", section = "guong-mat-thanh-cong" });
// /guong-mat-thanh-cong/{slug}
app.MapControllerRoute(name: "news-guongmat", pattern: "guong-mat-thanh-cong/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// /tu-van-dinh-cu/{slug}             VD: /tu-van-dinh-cu/tin-tuc-dinh-cu
app.MapControllerRoute(name: "news-dinhcu", pattern: "tu-van-dinh-cu/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// /tu-van-du-hoc/{slug}              VD: /tu-van-du-hoc/du-hoc-my
app.MapControllerRoute(name: "news-tuvanduhoc", pattern: "tu-van-du-hoc/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// /hoc-bong-du-hoc/{slug}
app.MapControllerRoute(name: "news-hocbong", pattern: "hoc-bong-du-hoc/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// /huong-nghiep/{slug}
app.MapControllerRoute(name: "news-huongnghiep", pattern: "huong-nghiep/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// /chia-se-tu-du-hoc-sinh            (slug cố định, không có segment cha)
app.MapControllerRoute(name: "news-chiase", pattern: "chia-se-tu-du-hoc-sinh", defaults: new { controller = "News", action = "CategoryBySlug", slug = "chia-se-tu-du-hoc-sinh" });
// /thong-tin-du-hoc/{country}/{slug}  VD: /thong-tin-du-hoc/du-hoc-my/tin-tuc-du-hoc-my
app.MapControllerRoute(name: "news-thongtinduhoc", pattern: "thong-tin-du-hoc/{s1}/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// /gioi-thieu/{slug}                 VD: /gioi-thieu/doi-ngu
app.MapControllerRoute(name: "news-gioithieu", pattern: "gioi-thieu/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });

// ── Section: Đội ngũ (/doi-ngu) ──────────────────────────────────────────────
// /doi-ngu  (root → danh mục đội ngũ)
app.MapControllerRoute(name: "doi-ngu-root", pattern: "doi-ngu", defaults: new { controller = "News", action = "CategoryBySlug", slug = "doi-ngu" });
// /doi-ngu/{slug}-{id}  VD: /doi-ngu/tien-si-mark-a-ashwill-3352
app.MapControllerRoute(name: "doi-ngu-detail", pattern: "doi-ngu/{slug}-{id:int}", defaults: new { controller = "News", action = "DoiNguDetail" });
// /tuyen-dung/{slug}-{id}  VD: /tuyen-dung/tien-si-mark-a-ashwill-3352
app.MapControllerRoute(name: "tuyen-dung-detail", pattern: "tuyen-dung/{slug}-{id:int}", defaults: new { controller = "News", action = "TuyenDungDetail" });
// ── Section: Tuyển dụng (/tuyen-dung) ────────────────────────────────────────
// /tuyen-dung  (root → danh mục tuyển dụng, CategoryID=200)
app.MapControllerRoute(name: "tuyen-dung-root", pattern: "tuyen-dung", defaults: new { controller = "News", action = "CategoryBySlug", slug = "tuyen-dung" });
// /tuyen-dung/{slug}-{id}  VD: /tuyen-dung/ten-bai-viet-7263
app.MapControllerRoute(name: "tuyen-dung-detail", pattern: "tuyen-dung/{slug}-{id:int}", defaults: new { controller = "News", action = "TuyenDungDetail" });

// Backward compat: /tin-tuc/danh-muc/{slug} → redirect vẫn hoạt động
app.MapControllerRoute(name: "news-category-slug", pattern: "tin-tuc/danh-muc/{slug}", defaults: new { controller = "News", action = "CategoryBySlug" });
// Legacy {slug}-{id} → 301 redirect
app.MapControllerRoute(name: "news-category", pattern: "tin-tuc/danh-muc/{slug}-{categoryId:int}", defaults: new { controller = "News", action = "Category" });

// ── Section: Cẩm nang & Tin tức (/cam-nang-va-tin-tuc) ───────────────────────
// /cam-nang-va-tin-tuc/{catSlug}/{slug}-{id}  VD: .../bang-tin-ve-cac-truong/bai-viet-7261
app.MapControllerRoute(name: "cam-nang-va-tin-tuc-detail",
    pattern: "cam-nang-va-tin-tuc/{catSlug}/{slug}-{id:int}",
    defaults: new { controller = "News", action = "CamNangDetail" });
// /cam-nang-va-tin-tuc/{slug}-{categoryId}    VD: .../bang-tin-ve-cac-truong-175
app.MapControllerRoute(name: "cam-nang-va-tin-tuc-cat",
    pattern: "cam-nang-va-tin-tuc/{slug}-{categoryId:int}",
    defaults: new { controller = "News", action = "CamNangCategory" });
// /cam-nang-va-tin-tuc  (root section)
app.MapControllerRoute(name: "cam-nang-va-tin-tuc-root",
    pattern: "cam-nang-va-tin-tuc",
    defaults: new { controller = "News", action = "CamNangSection" });

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();