using Capstone.View.Helpers;
using Capstone.View.Options;
using NVCMS.WebView.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(SiteSettings.SectionName));
builder.Services.AddSingleton<ContentUrlHelper>();

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
var webRootPath = builder.Environment.WebRootPath;
var serverFilesBaseUrl = builder.Configuration["SiteSettings:ServerFilesBaseUrl"] ?? string.Empty;
builder.Services.AddWebViewData(connStr, webRootPath, serverFilesBaseUrl);

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
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();