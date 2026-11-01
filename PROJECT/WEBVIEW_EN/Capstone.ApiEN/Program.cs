using System.Threading.RateLimiting;
using Capstone.ApiEN.Data;
using Capstone.ApiEN.Endpoints;
using Capstone.ApiEN.Infrastructure;
using Capstone.ApiEN.Leads;
using Capstone.Content;
using Capstone.Content.Json;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// ── Options ──────────────────────────────────────────────────────────────────
builder.Services.Configure<ContentSourceOptions>(builder.Configuration.GetSection(ContentSourceOptions.SectionName));
builder.Services.Configure<ApiSecurityOptions>(builder.Configuration.GetSection(ApiSecurityOptions.SectionName));
var source = builder.Configuration.GetSection(ContentSourceOptions.SectionName).Get<ContentSourceOptions>() ?? new ContentSourceOptions();

// ── JSON: same camelCase contract as the website ─────────────────────────────
builder.Services.ConfigureHttpJsonOptions(o => ContentJson.Configure(o.SerializerOptions));

// ── Data source ──────────────────────────────────────────────────────────────
// JsonContentStore is always registered: it serves everything in Json mode and the
// CMS blocks (site, home, pages, services, testimonials) in Sql mode.
builder.Services.AddSingleton(sp => new JsonContentStore(
    JsonContentStore.ResolveDirectory(sp.GetRequiredService<IWebHostEnvironment>().ContentRootPath, source.MockDataPath),
    sp.GetRequiredService<ILogger<JsonContentStore>>()));

if (source.UseSql)
{
    builder.Services.AddSingleton<IContentApi, SqlContentStore>();
    builder.Services.AddSingleton<ILeadStore, SqlLeadStore>();
}
else
{
    builder.Services.AddSingleton<IContentApi>(sp => sp.GetRequiredService<JsonContentStore>());
    builder.Services.AddSingleton<ILeadStore, JsonLeadStore>();
}

// ── Cross-cutting ────────────────────────────────────────────────────────────
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.Services.AddOutputCache(o =>
{
    var seconds = builder.Configuration.GetValue("OutputCache:Seconds", 60);
    o.AddPolicy(ContentEndpoints.CachePolicy, p =>
    {
        if (seconds <= 0)
        {
            p.NoCache(); // development: always fresh
            return;
        }
        // The cache key already includes path + query string. Varying by the key header means a
        // request without a valid key never gets a cached response past ApiKeyFilter.
        p.Expire(TimeSpan.FromSeconds(seconds))
         .SetVaryByHeader(ApiKeyFilter.HeaderName)
         .Tag(ContentEndpoints.CacheTag);
    });
});

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    // Leads arrive from the website server, so this is a site-wide ceiling per calling server.
    // The per-visitor limit lives in Capstone.ViewEN (POST rate limiter).
    o.AddPolicy("leads", http => RateLimitPartition.GetFixedWindowLimiter(
        http.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 30, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    // OpenAPI document: /openapi/v1.json (import into Postman / Swagger UI).
    app.MapOpenApi();
}

app.UseRateLimiter();
app.UseOutputCache();

app.MapGet("/", (IOptions<ContentSourceOptions> o) => Results.Ok(new
{
    service = "Capstone Vietnam EN content API",
    dataSource = o.Value.UseSql ? "Sql" : "Json",
    endpoints = "/api/v1/en/*",
    openApi = app.Environment.IsDevelopment() ? "/openapi/v1.json" : null
})).ExcludeFromDescription();

app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTimeOffset.Now })).ExcludeFromDescription();

app.MapContentEndpoints();

app.Run();
