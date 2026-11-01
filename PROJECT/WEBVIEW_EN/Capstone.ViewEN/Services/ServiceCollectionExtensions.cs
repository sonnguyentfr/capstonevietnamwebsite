using Capstone.Content;
using Capstone.Content.Json;

namespace Capstone.ViewEN.Services;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IContentApi"/>:
    /// - ContentApi:UseMock = false (default): typed HttpClient <see cref="ContentApiClient"/> → Capstone.ApiEN.
    /// - ContentApi:UseMock = true: <see cref="JsonContentStore"/> reading ../MockData directly (no API needed).
    /// </summary>
    public static IServiceCollection AddContentApi(this IServiceCollection services, IConfiguration config)
    {
        var section = config.GetSection(ContentApiOptions.SectionName);
        services.Configure<ContentApiOptions>(section);
        services.Configure<SiteOptions>(config.GetSection(SiteOptions.SectionName));
        services.AddMemoryCache();

        var opts = section.Get<ContentApiOptions>() ?? new ContentApiOptions();
        if (opts.UseMock)
        {
            services.AddSingleton<IContentApi>(sp => new JsonContentStore(
                JsonContentStore.ResolveDirectory(sp.GetRequiredService<IWebHostEnvironment>().ContentRootPath, opts.MockDataPath),
                sp.GetRequiredService<ILogger<JsonContentStore>>()));
            return services;
        }

        services.AddHttpClient<IContentApi, ContentApiClient>(client =>
        {
            var baseUrl = opts.BaseUrl.EndsWith('/') ? opts.BaseUrl : opts.BaseUrl + "/";
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(Math.Max(1, opts.TimeoutSeconds));
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en");
            if (!string.IsNullOrWhiteSpace(opts.ApiKey))
                client.DefaultRequestHeaders.Add("X-Api-Key", opts.ApiKey);
        });

        return services;
    }
}
