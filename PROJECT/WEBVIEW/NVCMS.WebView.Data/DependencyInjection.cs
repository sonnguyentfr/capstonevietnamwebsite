using Microsoft.Extensions.DependencyInjection;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Repository;
using NVCMS.WebView.Data.Service;
using NVCMS.WebView.Data.SiteSettings;
using NVCMS.WebView.Data.ViewModels;
using Microsoft.Extensions.Caching.Memory;

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
        services.AddScoped<IShortyUrlRepository>(_ => new ShortyUrlRepository(connectionString));

        // Repository - CRMConnection
        services.AddScoped<IEventsRepository>(_ => new EventsRepository(crmConnectionString));

        // Repository - CRMConnection (school data)
        services.AddScoped<ITruongRepository>(_ => new TruongRepository(crmConnectionString));

        // Repository - CRMConnection (org data)
        services.AddScoped<IOrganizationRepository>(_ => new OrganizationRepository(crmConnectionString));

        // Service
        services.AddScoped<INewsService>(sp =>
            new NewsService(
                sp.GetRequiredService<INewsRepository>(),
                rewriter,
                sp.GetRequiredService<IMemoryCache>()));
        services.AddScoped<INewsUrlService>(sp =>
            new NewsUrlService(sp.GetRequiredService<INewsRepository>()));
        services.AddScoped<IBannerService>(sp =>
            new BannerService(
                sp.GetRequiredService<IBannerRepository>(),
                rewriter,
                sp.GetRequiredService<IMemoryCache>()));
        services.AddScoped<IGioiThieuService>(sp =>
            new GioiThieuService(sp.GetRequiredService<IGioiThieuRepository>(), rewriter));
        services.AddScoped<IEventsService>(sp =>
            new EventsService(
                sp.GetRequiredService<IEventsRepository>(),
                sp.GetRequiredService<ITruongRepository>(),
                sp.GetRequiredService<IOrganizationRepository>(),
                rewriter,
                sp.GetRequiredService<IMemoryCache>()));
        services.AddScoped<ITruongService>(sp =>
            new TruongService(
                sp.GetRequiredService<ITruongRepository>(),
                sp.GetRequiredService<INewsRepository>(),
                rewriter));
        services.AddSingleton<IMenuService>(_ => new MenuService(webRootPath));

        // Event Registration
        services.AddScoped<IEventRegistrationRepository>(_ =>
            new EventRegistrationRepository(crmConnectionString));
        services.AddScoped<IEventRegistrationService>(sp =>
            new EventRegistrationService(
                sp.GetRequiredService<IEventRegistrationRepository>(),
                sp.GetRequiredService<IEventsRepository>()));

        // SiteSettings — Singleton, backed by WebView_GetSiteSettings SP, cached per portalId
        services.AddMemoryCache();
        services.AddSingleton<ISiteSettingsHelper>(sp =>
            new SiteSettingsHelper(connectionString, sp.GetRequiredService<IMemoryCache>()));

        return services;
    }
}
