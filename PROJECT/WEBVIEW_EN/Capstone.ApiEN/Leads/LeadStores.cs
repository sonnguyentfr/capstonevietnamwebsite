using System.Data;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Capstone.ApiEN.Infrastructure;
using Capstone.Content;
using Capstone.Content.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Capstone.ApiEN.Leads;

/// <summary>Where enquiries from the EN site go.</summary>
public interface ILeadStore
{
    Task<LeadResult> SaveAsync(LeadRequest lead, CancellationToken ct);
}

public static class LeadValidator
{
    private static readonly string[] Roles = ["student", "parent", "institution"];

    /// <summary>Field errors in the ValidationProblem format (camelCase keys, same names as the JSON body).</summary>
    public static Dictionary<string, string[]> Validate(LeadRequest r)
    {
        var errors = new Dictionary<string, string[]>();
        void Add(string field, string message) => errors[field] = [message];

        if (string.IsNullOrWhiteSpace(r.FullName)) Add("fullName", "Enter your full name.");
        else if (r.FullName.Length > 120) Add("fullName", "Use 120 characters or fewer.");

        if (string.IsNullOrWhiteSpace(r.Email) || !MailAddress.TryCreate(r.Email.Trim(), out _) || r.Email.Length > 200)
            Add("email", "Enter an email address like name@example.com.");

        if (r.Phone is { Length: > 30 }) Add("phone", "Enter a phone number of 30 characters or fewer.");
        if (r.Message is { Length: > 2000 }) Add("message", "Keep your message under 2,000 characters.");
        if (r.Organisation is { Length: > 200 }) Add("organisation", "Use 200 characters or fewer.");
        if (!Roles.Contains(r.Role)) Add("role", "Choose student, parent or institution.");
        if (!r.Consent) Add("consent", "Consent is required before we can contact you.");
        return errors;
    }
}

/// <summary>Development / Json mode: appends each lead to App_Data/leads.jsonl.</summary>
public sealed class JsonLeadStore(IWebHostEnvironment env, IOptions<ContentSourceOptions> options, ILogger<JsonLeadStore> logger) : ILeadStore
{
    private static readonly SemaphoreSlim Gate = new(1, 1);

    public async Task<LeadResult> SaveAsync(LeadRequest lead, CancellationToken ct)
    {
        var path = Path.Combine(env.ContentRootPath, options.Value.LeadsFile);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var line = JsonSerializer.Serialize(new { receivedAt = DateTimeOffset.Now, lead }, ContentJson.Options);

        await Gate.WaitAsync(ct);
        try
        {
            await File.AppendAllTextAsync(path, line + Environment.NewLine, Encoding.UTF8, ct);
        }
        finally
        {
            Gate.Release();
        }

        logger.LogInformation("Lead saved to {Path}: {Role} {Email}", path, lead.Role, lead.Email);
        return new LeadResult { Success = true, Message = "Thanks, your enquiry has been received." };
    }
}

/// <summary>
/// Sql mode: same CRM stored procedure as the Vietnamese form (WebView_TuVan_Upsert → Student_Info),
/// tagged with its own code prefix so counsellors can tell EN enquiries apart.
/// </summary>
public sealed class SqlLeadStore(IConfiguration config, IOptions<ContentSourceOptions> options, ILogger<SqlLeadStore> logger) : ILeadStore
{
    private readonly string _crmDb = Capstone.ApiEN.Data.SqlContentStore.Required(config, "CrmDb");

    public async Task<LeadResult> SaveAsync(LeadRequest lead, CancellationToken ct)
    {
        var o = options.Value;
        var (hoTenDem, ten) = SplitName(lead.FullName);

        var p = new DynamicParameters();
        p.Add("Hotendem", hoTenDem);
        p.Add("Ten", ten);
        p.Add("Sex", null, DbType.Boolean);
        p.Add("Ngaysinh", null, DbType.DateTime);
        p.Add("Sodienthoai", lead.Phone?.Trim() ?? string.Empty);
        p.Add("Email", lead.Email.Trim());
        p.Add("Diachi", string.Empty);
        p.Add("TinhId", null, DbType.Int32);
        p.Add("TuVanHocVanmongmuon", string.Empty);
        p.Add("TuVanNamdi", string.Empty);
        p.Add("TuVanNganhhoc", string.Empty);
        p.Add("TuVanKhanangchitra", 0);
        p.Add("TuVanQuocgia", string.Empty);
        p.Add("TuVanKhac", Describe(lead));
        p.Add("HinhThuc", o.LeadHinhThucId);
        p.Add("CodePrefix", o.LeadCodePrefix);
        p.Add("PortalId", o.LeadCrmPortalId);
        p.Add("StudentId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("StudentCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
        p.Add("IsExisting", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        await using var conn = new SqlConnection(_crmDb);
        await conn.ExecuteAsync(new CommandDefinition("WebView_TuVan_Upsert", p,
            commandType: CommandType.StoredProcedure, cancellationToken: ct));

        logger.LogInformation("Lead stored in CRM: code {Code}, existing={Existing}, role={Role}",
            p.Get<string?>("StudentCode"), p.Get<bool?>("IsExisting"), lead.Role);
        return new LeadResult { Success = true, Message = "Thanks, your enquiry has been received." };
    }

    /// <summary>Vietnamese order: family and middle names first, given name last.</summary>
    private static (string HoTenDem, string Ten) SplitName(string fullName)
    {
        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length <= 1 ? (string.Empty, fullName.Trim()) : (string.Join(' ', parts[..^1]), parts[^1]);
    }

    /// <summary>Everything the CRM has no column for goes into TuVanKhac, readable by counsellors.</summary>
    private static string Describe(LeadRequest l)
    {
        var sb = new StringBuilder("[EN website]");
        sb.Append(" Role: ").Append(l.Role);
        sb.Append(" | Interest: ").Append(l.Interest);
        if (!string.IsNullOrWhiteSpace(l.Organisation)) sb.Append(" | Organisation: ").Append(l.Organisation.Trim());
        if (!string.IsNullOrWhiteSpace(l.Message)) sb.Append(" | Message: ").Append(l.Message.Trim());
        if (!string.IsNullOrWhiteSpace(l.SourceUrl)) sb.Append(" | Page: ").Append(l.SourceUrl);
        if (!string.IsNullOrWhiteSpace(l.UtmSource) || !string.IsNullOrWhiteSpace(l.UtmCampaign))
            sb.Append(" | UTM: ").Append(l.UtmSource).Append(" / ").Append(l.UtmMedium).Append(" / ").Append(l.UtmCampaign);
        return sb.ToString();
    }
}
