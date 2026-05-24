using Microsoft.Extensions.DependencyInjection;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Repository;
using NVCMS.WebView.Data.Service;

namespace NVCMS.WebView.Data;

public static class DependencyInjection
{
    /// <summary>
    /// Dang ky toan bo Repository + Service vao DI container.
    /// Goi tu Program.cs cua Capstone.View.
    /// </summary>
    public static IServiceCollection AddWebViewData(
        this IServiceCollection services,
        string connectionString,
        string webRootPath,
        string serverFilesBaseUrl)
    {
        var rewriter = new ContentUrlRewriter(serverFilesBaseUrl);

        // Repository
        services.AddScoped<INewsRepository>(_ => new NewsRepository(connectionString));
        services.AddScoped<IBannerRepository>(_ => new BannerRepository(connectionString));
        services.AddScoped<IGioiThieuRepository>(_ => new GioiThieuRepository(connectionString));

        // Service
        services.AddScoped<INewsService>(sp =>
            new NewsService(sp.GetRequiredService<INewsRepository>(), rewriter));
        services.AddScoped<IBannerService>(sp =>
            new BannerService(sp.GetRequiredService<IBannerRepository>(), rewriter));
        services.AddScoped<IGioiThieuService>(sp =>
            new GioiThieuService(sp.GetRequiredService<IGioiThieuRepository>(), rewriter));
        services.AddSingleton<IMenuService>(_ => new MenuService(webRootPath));

        return services;
    }
}
