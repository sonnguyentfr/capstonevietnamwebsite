using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
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

// Register Services
builder.Services.AddScoped<IGoogleSheetService, GoogleSheetService>();
builder.Services.AddScoped<ICrmDataService, CrmDataService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IZaloTokenService, ZaloTokenService>();

// Marketing DbContext (DefaultCRMConnection)
builder.Services.AddDbContext<MarketingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultCRMConnection")));

// Marketing Repositories
builder.Services.AddScoped<IMarketingCampaignRepository,    MarketingCampaignRepository>();
builder.Services.AddScoped<IMarketingListMailRepository,    MarketingListMailRepository>();
builder.Services.AddScoped<IMarketingTemplateRepository,    MarketingTemplateRepository>();
builder.Services.AddScoped<IMarketingEventRepository,       MarketingEventRepository>();
builder.Services.AddScoped<IMarketingHangfireLogRepository, MarketingHangfireLogRepository>();
builder.Services.AddScoped<IMarketingUnsubRepository,       MarketingUnsubRepository>();
builder.Services.AddScoped<IMarketingClickRepository,       MarketingClickRepository>();
builder.Services.AddScoped<IMarketingCampaignSendRepository, MarketingCampaignSendRepository>();
builder.Services.AddScoped<IMarketingSendLogRepository,     MarketingSendLogRepository>();
builder.Services.AddScoped<IEmailMarketingService,          EmailMarketingService>();
builder.Services.AddScoped<ISESService,                     SESService>();

// ── Hangfire ──────────────────────────────────────────────────────────────────
var hangfireConn = builder.Configuration.GetConnectionString("DefaultCRMConnection")!;
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(hangfireConn, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout       = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout   = TimeSpan.FromMinutes(5),
        QueuePollInterval            = TimeSpan.FromSeconds(15),
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks           = true
    }));

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 2;
    options.Queues      = ["default"];
});

// Register Jobs as transient (Hangfire activator tự resolve qua DI)
builder.Services.AddTransient<CampaignSchedulerJob>();
builder.Services.AddTransient<CampaignBatchJob>();

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

builder.Services.AddHttpClient();

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

// ── Hangfire Dashboard (/hangfire) ────────────────────────────────────────────
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    // Cho phép tất cả truy cập trong dev; production nên dùng DashboardAuthorizationFilter
    Authorization = []
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ── Recurring Jobs ────────────────────────────────────────────────────────────
// CampaignSchedulerJob chạy mỗi phút để quét campaign Queued đến hạn
RecurringJob.AddOrUpdate<CampaignSchedulerJob>(
    recurringJobId: "campaign-scheduler",
    methodCall:     job => job.ExecuteAsync(),
    cronExpression: Cron.Minutely(),
    options:        new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Utc
    });

app.Run();