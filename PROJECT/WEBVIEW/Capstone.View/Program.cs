using Capstone.View.Helpers;
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
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

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
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "gioi-thieu-ve-capstone", pattern: "gioi-thieu/ve-capstone", defaults: new { controller = "GioiThieu", action = "VeCapstone" });
app.MapControllerRoute(name: "gioi-thieu", pattern: "gioi-thieu", defaults: new { controller = "GioiThieu", action = "Index" });

app.MapControllerRoute(name: "su-kien-past-paged", pattern: "su-kien/past-paged",   defaults: new { controller = "SuKien", action = "PastPaged" });
app.MapControllerRoute(name: "su-kien-detail",     pattern: "su-kien/{slug}-{id:int}", defaults: new { controller = "SuKien", action = "Detail" });
app.MapControllerRoute(name: "su-kien",        pattern: "su-kien",                  defaults: new { controller = "SuKien", action = "Index" });

// Các dịch vụ Capstone
app.MapControllerRoute(name: "dich-vu-index", pattern: "capstone-vietnam/cac-dich-vu-capstone", defaults: new { controller = "DichVu", action = "Index" });
app.MapControllerRoute(name: "tu-van-dinh-cu", pattern: "tu-van-dinh-cu", defaults: new { controller = "DichVu", action = "TuVanDinhCu" });
app.MapControllerRoute(name: "tu-van-du-hoc-cac-nuoc", pattern: "capstone-vietnam/cac-dich-vu-capstone/tu-van-du-hoc-cac-nuoc", defaults: new { controller = "DichVu", action = "TuVanDuHocCacNuoc" });
app.MapControllerRoute(name: "tu-van-du-hoc-truong-top", pattern: "capstone-vietnam/cac-dich-vu-capstone/tu-van-du-hoc-truong-top", defaults: new { controller = "DichVu", action = "TuVanDuHocTruongTop" });
app.MapControllerRoute(name: "tu-van-du-hoc-cao-hoc", pattern: "capstone-vietnam/cac-dich-vu-capstone/tu-van-du-hoc-cao-hoc", defaults: new { controller = "DichVu", action = "TuVanDuHocCaoHoc" });

// Trường đối tác
app.MapControllerRoute(name: "truong-search-json",    pattern: "truong-doi-tac/search-json",           defaults: new { controller = "Truong", action = "SearchJson" });
app.MapControllerRoute(name: "truong-detail",         pattern: "truong-doi-tac/{slug}-{id:int}",        defaults: new { controller = "Truong", action = "Detail" });
app.MapControllerRoute(name: "truong-quocgia",        pattern: "truong-doi-tac/{countrySlug}",          defaults: new { controller = "Truong", action = "QuocGia" });
app.MapControllerRoute(name: "truong-index",          pattern: "truong-doi-tac",                        defaults: new { controller = "Truong", action = "Index" });
app.MapControllerRoute(name: "tim-truong",            pattern: "tim-truong",                            defaults: new { controller = "Truong", action = "TimTruong" });
app.MapControllerRoute(name: "tim-nganh-hoc",         pattern: "tim-nganh-hoc",                         defaults: new { controller = "Truong", action = "TimNganhHoc" });

// Thông tin du học – danh sách trường theo quốc gia
app.MapControllerRoute(name: "thong-tin-du-hoc-danh-sach-truong",
    pattern:  "thong-tin-du-hoc/{countrySlug}/danh-sach-truong",
    defaults: new { controller = "ThongTinDuHoc", action = "DanhSachTruong" });
app.MapControllerRoute(name: "tu-van-nganh-nghe", pattern: "capstone-vietnam/cac-dich-vu-capstone/tu-van-nganh-nghe", defaults: new { controller = "DichVu", action = "TuVanNganhNghe" });
app.MapControllerRoute(name: "tu-van-visa", pattern: "capstone-vietnam/cac-dich-vu-capstone/tu-van-visa-du-hoc-tham-than", defaults: new { controller = "DichVu", action = "TuVanVisa" });
app.MapControllerRoute(name: "chuyen-tien-du-hoc", pattern: "capstone-vietnam/cac-dich-vu-capstone/dich-vu-chuyen-tien-du-hoc", defaults: new { controller = "DichVu", action = "ChuyenTienDuHoc" });
app.MapControllerRoute(name: "tim-nha", pattern: "capstone-vietnam/cac-dich-vu-capstone/dich-vu-tim-nha", defaults: new { controller = "DichVu", action = "TimNha" });

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();