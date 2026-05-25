using Microsoft.Extensions.DependencyInjection;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Repository;
using NVCMS.WebView.Data.Service;
using NVCMS.WebView.Data.ViewModels;

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
        string crmConnectionString,
        string webRootPath,
        string serverFilesBaseUrl)
    {
        var rewriter = new ContentUrlRewriter(serverFilesBaseUrl);

        // Repository - DefaultConnection
        services.AddScoped<INewsRepository>(_ => new NewsRepository(connectionString));
        services.AddScoped<IBannerRepository>(_ => new BannerRepository(connectionString));
        services.AddScoped<IGioiThieuRepository>(_ => new GioiThieuRepository(connectionString));

        // Repository - CRMConnection
        services.AddScoped<IEventsRepository>(_ => new EventsRepository(crmConnectionString));

        // Repository - CRMConnection (school data)
        services.AddScoped<ITruongRepository>(_ => new TruongRepository(crmConnectionString));

        // Service
        services.AddScoped<INewsService>(sp =>
            new NewsService(sp.GetRequiredService<INewsRepository>(), rewriter));
        services.AddScoped<IBannerService>(sp =>
            new BannerService(sp.GetRequiredService<IBannerRepository>(), rewriter));
        services.AddScoped<IGioiThieuService>(sp =>
            new GioiThieuService(sp.GetRequiredService<IGioiThieuRepository>(), rewriter));
        services.AddScoped<IEventsService>(sp =>
            new EventsService(sp.GetRequiredService<IEventsRepository>(), rewriter));
        services.AddScoped<ITruongService>(sp =>
            new TruongService(sp.GetRequiredService<ITruongRepository>(), rewriter));
        services.AddSingleton<IMenuService>(_ => new MenuService(webRootPath));

        return services;
    }
}
