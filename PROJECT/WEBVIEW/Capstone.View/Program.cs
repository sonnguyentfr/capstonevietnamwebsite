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
builder.Services.AddScoped<Capstone.View.Helpers.NewsUrlHelper>();

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
builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebsite", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7208",
                "https://capstonevietnam.com",
                "https://v3.capstonevietnam.com",
                "https://www.v3.capstonevietnam.com",
                "https://capstonevietnam-fileserver.nvcms.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();
app.UseCors("AllowWebsite");
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
    context.Response.Headers.Append(
        "X-Content-Type-Options",
        "nosniff");

    context.Response.Headers.Append(
        "X-Frame-Options",
        "DENY");

    context.Response.Headers.Append(
        "Referrer-Policy",
        "strict-origin-when-cross-origin");

    context.Response.Headers.Append(
        "Content-Security-Policy",
        "default-src 'self'; script-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none';");
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

// ── Event Registration route ──────────────────────────────────────────────────
app.MapControllerRoute(name: "event-registration", pattern: "dang-ky-su-kien",
    defaults: new { controller = "EventRegistration", action = "Index" });

app.MapControllerRoute(name: "gioi-thieu-ve-capstone", pattern: "gioi-thieu/ve-capstone", defaults: new { controller = "GioiThieu", action = "VeCapstone" });
app.MapControllerRoute(name: "gioi-thieu", pattern: "gioi-thieu", defaults: new { controller = "GioiThieu", action = "Index" });
app.MapControllerRoute(name: "quy-trinh-tu-van", pattern: "gioi-thieu/quy-trinh-tu-van", defaults: new { controller = "GioiThieu", action = "QuyTrinhTuVan" });

// FairGuide
app.MapControllerRoute(name: "fairguide-detail", pattern: "gioi-thieu/fairguide/{slug}-{id:int}", defaults: new { controller = "FairGuide", action = "Detail" });
app.MapControllerRoute(name: "fairguide-index",  pattern: "gioi-thieu/fairguide",                 defaults: new { controller = "FairGuide", action = "Index" });

// Video Library
app.MapControllerRoute(name: "video-load-more", pattern: "gioi-thieu/thu-vien-anh-video/load-more", defaults: new { controller = "Video", action = "LoadMore" });
app.MapControllerRoute(name: "video-detail",    pattern: "gioi-thieu/thu-vien-anh-video/detail/{id:int}", defaults: new { controller = "Video", action = "Detail" });
app.MapControllerRoute(name: "video-index",     pattern: "gioi-thieu/thu-vien-anh-video",          defaults: new { controller = "Video", action = "Index" });
// Sự kiện
app.MapControllerRoute(name: "su-kien-past-paged", pattern: "su-kien/past-paged",     defaults: new { controller = "SuKien", action = "PastPaged" });
app.MapControllerRoute(name: "su-kien-detail",     pattern: "su-kien/{slug}-{id:int}", defaults: new { controller = "SuKien", action = "Detail" });
app.MapControllerRoute(name: "su-kien-index",      pattern: "su-kien",                 defaults: new { controller = "SuKien", action = "Index" });

// Các dịch vụ Capstone – canonical: /dich-vu/*
app.MapControllerRoute(name: "dich-vu-index",            pattern: "dich-vu",                                      defaults: new { controller = "DichVu", action = "Index" });
app.MapControllerRoute(name: "tu-van-du-hoc-cac-nuoc",   pattern: "dich-vu/tu-van-du-hoc-cac-nuoc",               defaults: new { controller = "DichVu", action = "TuVanDuHocCacNuoc" });
app.MapControllerRoute(name: "tu-van-du-hoc-truong-top", pattern: "dich-vu/tu-van-du-hoc-truong-top",             defaults: new { controller = "DichVu", action = "TuVanDuHocTruongTop" });
app.MapControllerRoute(name: "tu-van-du-hoc-cao-hoc",    pattern: "dich-vu/tu-van-du-hoc-cao-hoc",                defaults: new { controller = "DichVu", action = "TuVanDuHocCaoHoc" });
app.MapControllerRoute(name: "tu-van-nganh-nghe",        pattern: "dich-vu/tu-van-nganh-nghe",                    defaults: new { controller = "DichVu", action = "TuVanNganhNghe" });
app.MapControllerRoute(name: "tu-van-visa",              pattern: "dich-vu/tu-van-visa-du-hoc-tham-than",          defaults: new { controller = "DichVu", action = "TuVanVisa" });
app.MapControllerRoute(name: "chuyen-tien-du-hoc",       pattern: "dich-vu/dich-vu-chuyen-tien-du-hoc",           defaults: new { controller = "DichVu", action = "ChuyenTienDuHoc" });
app.MapControllerRoute(name: "tim-nha",                  pattern: "dich-vu/dich-vu-tim-nha",                      defaults: new { controller = "DichVu", action = "TimNha" });
app.MapControllerRoute(name: "tu-van-dinh-cu-sub",       pattern: "tu-van-dinh-cu/{pageSlug}",                    defaults: new { controller = "DichVu", action = "TuVanDinhCuSubPage" });
app.MapControllerRoute(name: "tu-van-dinh-cu",           pattern: "tu-van-dinh-cu",                               defaults: new { controller = "DichVu", action = "TuVanDinhCu" });

// 301 redirects: old paths → /dich-vu/*
app.MapGet("gioi-thieu/cac-dich-vu-capstone",                                       (HttpContext ctx) => Results.Redirect("/dich-vu",                              permanent: true));
app.MapGet("gioi-thieu/cac-dich-vu-capstone/tu-van-du-hoc-cac-nuoc",                (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-du-hoc-cac-nuoc",      permanent: true));
app.MapGet("gioi-thieu/cac-dich-vu-capstone/tu-van-du-hoc-truong-top",              (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-du-hoc-truong-top",    permanent: true));
app.MapGet("gioi-thieu/cac-dich-vu-capstone/tu-van-du-hoc-cao-hoc",                 (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-du-hoc-cao-hoc",       permanent: true));
app.MapGet("gioi-thieu/cac-dich-vu-capstone/tu-van-nganh-nghe",                     (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-nganh-nghe",           permanent: true));
app.MapGet("gioi-thieu/cac-dich-vu-capstone/tu-van-visa-du-hoc-tham-than",          (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-visa-du-hoc-tham-than",permanent: true));
app.MapGet("gioi-thieu/cac-dich-vu-capstone/dich-vu-chuyen-tien-du-hoc",            (HttpContext ctx) => Results.Redirect("/dich-vu/dich-vu-chuyen-tien-du-hoc", permanent: true));
app.MapGet("gioi-thieu/cac-dich-vu-capstone/dich-vu-tim-nha",                       (HttpContext ctx) => Results.Redirect("/dich-vu/dich-vu-tim-nha",             permanent: true));
app.MapGet("cac-dich-vu-capstone",                                                   (HttpContext ctx) => Results.Redirect("/dich-vu",                              permanent: true));
app.MapGet("cac-dich-vu-capstone/tu-van-du-hoc-cac-nuoc",                           (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-du-hoc-cac-nuoc",      permanent: true));
app.MapGet("cac-dich-vu-capstone/tu-van-du-hoc-truong-top",                         (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-du-hoc-truong-top",    permanent: true));
app.MapGet("cac-dich-vu-capstone/tu-van-du-hoc-cao-hoc",                            (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-du-hoc-cao-hoc",       permanent: true));
app.MapGet("cac-dich-vu-capstone/tu-van-nganh-nghe",                                (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-nganh-nghe",           permanent: true));
app.MapGet("cac-dich-vu-capstone/tu-van-visa-du-hoc-tham-than",                     (HttpContext ctx) => Results.Redirect("/dich-vu/tu-van-visa-du-hoc-tham-than",permanent: true));
app.MapGet("cac-dich-vu-capstone/dich-vu-chuyen-tien-du-hoc",                       (HttpContext ctx) => Results.Redirect("/dich-vu/dich-vu-chuyen-tien-du-hoc", permanent: true));
app.MapGet("cac-dich-vu-capstone/dich-vu-tim-nha",                                  (HttpContext ctx) => Results.Redirect("/dich-vu/dich-vu-tim-nha",             permanent: true));



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

// Thông tin du học – sub-page giới thiệu
app.MapControllerRoute(name: "thong-tin-du-hoc-sub-page",
    pattern: "thong-tin-du-hoc/{countrySlug}/{pageSlug}",
    defaults: new { controller = "ThongTinDuHoc", action = "SubPage" });

// Thông tin du học – country landing page
app.MapControllerRoute(name: "thong-tin-du-hoc-country",
    pattern: "thong-tin-du-hoc/{countrySlug}",
    defaults: new { controller = "ThongTinDuHoc", action = "CountryPage" });

// Tin tức – core routes
app.MapControllerRoute(
    name: "news-dinh-cu-detail",
    pattern: "tu-van-dinh-cu/{slug}-{id:int}",
    defaults: new { controller = "News", action = "DetailDinhCu" });

app.MapControllerRoute(
    name: "news-dau-tu-detail",
    pattern: "tu-van-dau-tu/{slug}-{id:int}",
    defaults: new { controller = "News", action = "DetailDauTu" });

app.MapControllerRoute(
    name: "news-tag",
    pattern: "tin-tuc/tag",
    defaults: new { controller = "News", action = "Tag" });

app.MapControllerRoute(
    name: "news-all",
    pattern: "tin-tuc",
    defaults: new { controller = "News", action = "All" });

app.MapControllerRoute(
    name: "news-detail",
    pattern: "tin-tuc/{slug}-{id:int}",
    defaults: new { controller = "News", action = "Detail" });

// Tìm kiếm
app.MapControllerRoute(
    name: "search",
    pattern: "tim-kiem",
    defaults: new { controller = "Search", action = "Index" });

// ================= DEFAULT (phải đặt trước catch-all) =================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ================= CATCH-ALL CATEGORY & DETAIL BY FULLSLUG =================
app.MapControllerRoute(
    name: "news-category-detail-catchall",
    pattern: "{**path}",
    defaults: new { controller = "News", action = "DetailCatchAll" });


app.Run();