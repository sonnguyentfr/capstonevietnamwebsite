using Capstone.View.Helpers;
using Capstone.View.Middleware;
using Capstone.View.Options;
using NVCMS.WebView.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(SiteSettings.SectionName));
builder.Services.AddSingleton<ContentUrlHelper>();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
var crmConnStr = builder.Configuration.GetConnectionString("CRMConnection")!;
var webRootPath = builder.Environment.WebRootPath;
var serverFilesBaseUrl = builder.Configuration["SiteSettings:ServerFilesBaseUrl"] ?? string.Empty;
builder.Services.AddWebViewData(connStr, crmConnStr, webRootPath, serverFilesBaseUrl);

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost/");
});

var app = builder.Build();

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

app.UseStaticFiles(new StaticFileOptions { ServeUnknownFileTypes = false });

// ShortUrl: chạy trước UseRouting để ưu tiên short_url hơn bất kỳ route nào
app.UseMiddleware<ShortUrlMiddleware>();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "gioi-thieu-ve-capstone", pattern: "gioi-thieu/ve-capstone", defaults: new { controller = "GioiThieu", action = "VeCapstone" });
app.MapControllerRoute(name: "gioi-thieu", pattern: "gioi-thieu", defaults: new { controller = "GioiThieu", action = "Index" });

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
app.MapControllerRoute(name: "news-all",      pattern: "tin-tuc",                                  defaults: new { controller = "News", action = "All" });
app.MapControllerRoute(name: "news-category", pattern: "tin-tuc/danh-muc/{slug}-{categoryId:int}", defaults: new { controller = "News", action = "Category" });
app.MapControllerRoute(name: "news-detail",   pattern: "tin-tuc/{slug}-{id:int}",                  defaults: new { controller = "News", action = "Detail" });

// Trang danh sách tin tức theo chuyên mục – route generic dùng chung
// 2-segment: /{section}/{slug}-{categoryId}             VD: /gioi-thieu/doi-ngu-227
app.MapControllerRoute(name: "news-category-page-2", pattern: "{section}/{slug}-{categoryId:int}",       defaults: new { controller = "News", action = "CategoryPage" });
// 3-segment: /{s1}/{s2}/{slug}-{categoryId}             VD: /thong-tin-du-hoc/du-hoc-my/tin-tuc-244
app.MapControllerRoute(name: "news-category-page-3", pattern: "{s1}/{s2}/{slug}-{categoryId:int}",       defaults: new { controller = "News", action = "CategoryPage" });
// 4-segment: /{s1}/{s2}/{s3}/{slug}-{categoryId}        VD: /gioi-thieu/cac-dich-vu-capstone/tu-van-dinh-cu/tin-tuc-dinh-cu-eb5-202
app.MapControllerRoute(name: "news-category-page-4", pattern: "{s1}/{s2}/{s3}/{slug}-{categoryId:int}",  defaults: new { controller = "News", action = "CategoryPage" });

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();