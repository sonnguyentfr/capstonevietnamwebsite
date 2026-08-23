using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Infrastructure;
using NVCMS.API.ReadGoogleSheet.Infrastructure.Http;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure SMTP Settings
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Configure SES Settings
builder.Services.Configure<SesSettings>(builder.Configuration.GetSection("SesSettings"));
// Configure Zalo
builder.Services.Configure<ZaloSettings>(builder.Configuration.GetSection("ZaloSettings"));
// Configure HangfireJobs
builder.Services.Configure<HangfireJobSettings>(builder.Configuration.GetSection("HangfireJobs"));

// Configure Swagger with JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NVCMS Google Sheet Reader API",
        Version = "v1",
        Description = "API for reading Google Sheets and importing to SQL Server"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(); // dev only
    options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICrmDataLadingRepository, CrmDataLadingRepository>();
builder.Services.AddScoped<IZaloTokenRepository, ZaloTokenRepository>();
builder.Services.AddScoped<IZaloZnsClient, ZaloZnsClient>();
builder.Services.AddScoped<IZnsTemplateRepository, ZnsTemplateRepository>();
builder.Services.AddScoped<IZnsSendQueueRepository, ZnsSendQueueRepository>();
builder.Services.AddScoped<IZnsSendLogRepository, ZnsSendLogRepository>();

// Register Services
builder.Services.AddScoped<IGoogleSheetService, GoogleSheetService>();
builder.Services.AddScoped<ICrmDataService, CrmDataService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<EmailTemplateRenderer>();
builder.Services.AddScoped<IZaloService, ZaloService>();
builder.Services.AddScoped<IZnsTemplateService, ZnsTemplateService>();
builder.Services.AddScoped<IZnsSendService, ZnsSendService>();

// Marketing DbContext (DefaultCRMConnection)
builder.Services.AddDbContext<CRMDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultCRMConnection")));

// Marketing Repositories
builder.Services.AddScoped<IMarketingCampaignRepository, MarketingCampaignRepository>();
builder.Services.AddScoped<IMarketingListMailRepository, MarketingListMailRepository>();
builder.Services.AddScoped<IMarketingTemplateRepository, MarketingTemplateRepository>();
builder.Services.AddScoped<IMarketingUnsubRepository, MarketingUnsubRepository>();
builder.Services.AddScoped<IMarketingSendLogRepository, MarketingSendLogRepository>();
builder.Services.AddScoped<IMailAccountRepository, MailAccountRepository>();
builder.Services.AddScoped<IEmailMarketingService, EmailMarketingService>();
builder.Services.AddScoped<ISESService, SESService>();

// ── Hangfire ──────────────────────────────────────────────────────────────────
var hangfireConn = builder.Configuration.GetConnectionString("DefaultCRMConnection")!;
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(hangfireConn, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.FromSeconds(15),
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 2;
    options.Queues = ["default"];
});

// Register Jobs as transient (Hangfire activator tự resolve qua DI)
builder.Services.AddTransient<CampaignBatchJob>();
// ZNS refresh token
builder.Services.AddTransient<ZnsRefreshTokenJob>();
builder.Services.AddTransient<ZnsTemplateSyncJob>();
builder.Services.AddTransient<ZnsSendJob>();
// Event registration confirmation emails (enqueued by Capstone.View)
builder.Services.AddTransient<EventRegistrationEmailJob>();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//builder.Services.AddHttpClient();
builder.Services.AddHttpClient<BaseApi>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NVCMS Google Sheet Reader API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// ── Hangfire Dashboard (/hangfire) ────────────────────────────────────────────
var _hangfireCfg = builder.Configuration.GetSection("HangfireDashboard");
var _cookieSecret = _hangfireCfg["CookieSecret"] ?? string.Empty;

// Middleware chặn /hangfire trước khi Hangfire xử lý.
// Nếu chưa có cookie hợp lệ → redirect sang /hangfire-login (HTTP 302).
// Làm vậy vì IDashboardAuthorizationFilter.Authorize() trả false thì Hangfire
// tự trả 401 và ghi đè mọi redirect ta set trong response.
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/hangfire"))
    {
        var cookie = context.Request.Cookies[HangfireDashboardAuthFilter.CookieName];
        if (!HangfireDashboardAuthFilter.IsValidCookieValue(cookie, _cookieSecret))
        {
            var returnUrl = Uri.EscapeDataString(context.Request.PathBase + context.Request.Path);
            context.Response.Redirect($"/hangfire-login?returnUrl={returnUrl}");
            return;
        }
    }
    await next();
});

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "NVCMS – Hangfire Monitor",
    Authorization = [new HangfireDashboardAuthFilter(_cookieSecret)],
    AppPath = "/swagger"
});

app.MapControllers();
app.RegisterRecurringJobs();
app.Run();