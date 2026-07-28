using System.Data;
using Dapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Data.SqlClient;
using MimeKit;
using MimeKit.Text;
using NCapstone.View.Helpers;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Services;

/// <summary>
/// Gửi email xác nhận đăng ký sự kiện trực tiếp qua SMTP (MailKit).
/// Lưu nội dung vào Marketing_Mail_Campaign_Send và trạng thái vào Marketing_Mail_Send_Log.
/// </summary>
public class EventRegistrationMailService
{
    // ── Config ─────────────────────────────────────────────────────────────────
    private readonly string _host;
    private readonly int    _port;
    private readonly bool   _enableSsl;
    private readonly string _fromAddress;
    private readonly string _user;
    private readonly string _password;
    private readonly string _displayName;
    private readonly string _crmConnStr;
    private readonly string _siteBaseUrl;
    private readonly ILogger<EventRegistrationMailService> _logger;

    // ── Template cache ─────────────────────────────────────────────────────────
    private static readonly object _cacheLock = new();
    private static readonly Dictionary<string, string> _templateCache = new();
    private readonly string _templateRoot;

    public EventRegistrationMailService(
        IConfiguration config,
        IWebHostEnvironment env,
        ILogger<EventRegistrationMailService> logger)
    {
        var sec = config.GetSection("Email");
        _host        = sec["Host"]             ?? "localhost";
        _port        = int.TryParse(sec["Port"], out var p) ? p : 587;
        _enableSsl   = bool.TryParse(sec["EnableSsl"], out var s) && s;
        _fromAddress = sec["FromEmailAddress"] ?? string.Empty;
        _user        = sec["UserMail"]         ?? string.Empty;
        _password    = sec["Password"]         ?? string.Empty;
        _displayName = sec["DisplayName"]      ?? "No Reply";
        _crmConnStr  = config.GetConnectionString("CRMConnection")
                       ?? throw new InvalidOperationException("CRMConnection not configured");
        _siteBaseUrl = (config["SiteBaseUrl"] ?? config["SiteSettings:SiteWeb"] ?? string.Empty)
                       .TrimEnd('/');
        var templateFolder = sec["TemplateFolder"] ?? "wwwroot/template/EmailTemplates";
        _templateRoot = Path.IsPathRooted(templateFolder)
            ? templateFolder
            : Path.Combine(env.ContentRootPath, templateFolder.Replace('/', Path.DirectorySeparatorChar));
        _logger = logger;
    }

    // ── Public entry point ────────────────────────────────────────────────────

    public async Task SendRegistrationEmailAsync(
        EventRegistrationInputViewModel input,
        int studentId,
        string studentCode,
        DateTime registeredAt,
        EventsCatViewModel cat,
        NVCMS.WebView.Data.SiteSettings.WebSiteSettings site,
        List<string> adminEmails,
        List<string> bccEmails,
        CancellationToken ct = default)
    {
        var ev = cat.Events.FirstOrDefault(e => e.Id == input.EventId);
        if (ev is null) return;

        var eventDate = ev.Fromdatetime.HasValue
            ? ev.Fromdatetime.Value.ToString("dd/MM/yyyy")
            : cat.FromDate?.ToString("dd/MM/yyyy") ?? string.Empty;

        var eventTime = ev.Fromdatetime.HasValue
            ? ev.Fromdatetime.Value.ToString("HH:mm")
            : cat.FromDate?.ToString("HH:mm") ?? string.Empty;

        var studentName  = input.HoVaTen.Trim();
        var studentPhone = PhoneHelper.Normalize(input.SoDienThoai);
        var studentEmail = input.Email ?? string.Empty;
        var eventName    = cat.CatName ?? string.Empty;
        var eventLoc     = ev.Diadiem  ?? string.Empty;
        var regTime      = registeredAt.ToString("HH:mm dd/MM/yyyy");
        var logoUrl      = site.Logo.HeaderLogo ?? string.Empty;
        var siteUrl      = site.General.SiteWeb ?? string.Empty;
        var siteName     = site.General.SiteName ?? _displayName;
        var sendCode     = cat.Sendcode == true;
        var notes        = cat.ContentMail ?? string.Empty;

        // ── 1. Lưu Campaign_Send (nội dung mail customer) ──────────────────────
        var customerSubject = $"[{siteName}] Xác nhận đăng ký - {eventName}";

        // Build customer body (chưa có tracking pixel — chèn sau khi có logId)
        string customerBody = BuildCustomerBody(
            studentName, studentCode, eventName, eventLoc,
            eventDate, eventTime, studentPhone,
            logoUrl, siteUrl, siteName,
            notes, sendCode, trackingLogId: 0);

        await using var conn = new SqlConnection(_crmConnStr);
        await conn.OpenAsync(ct);

        var campaignSendId = await conn.ExecuteScalarAsync<int>(
            """
            INSERT INTO Marketing_Mail_Campaign_Send
                (CampaignId, TemplateId, Subject, Body, Status,
                 TotalRecipient, TotalSent, TotalDelivered,
                 TotalOpened, TotalClicked, TotalBounced,
                 TotalComplaint, TotalUnsubscribed,
                 StartedTime, CreatedDate)
            VALUES
                (0, 0, @Subject, @Body, 1,
                 1, 0, 0,
                 0, 0, 0,
                 0, 0,
                 GETDATE(), GETDATE());
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """,
            new { Subject = customerSubject, Body = customerBody });

        // ── 2. Lưu Send_Log Queued cho customer ────────────────────────────────
        var logId = await conn.ExecuteScalarAsync<long>(
            """
            INSERT INTO Marketing_Mail_Send_Log
                (CampaignSendId, ListMailId, Email, Status, CreatedDate)
            VALUES
                (@CampaignSendId, 0, @Email, @Status, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
            """,
            new { CampaignSendId = campaignSendId, Email = studentEmail, Status = MailSendStatus.Queued });

        // Rebuild với tracking pixel đúng logId
        customerBody = BuildCustomerBody(
            studentName, studentCode, eventName, eventLoc,
            eventDate, eventTime, studentPhone,
            logoUrl, siteUrl, siteName,
            notes, sendCode, trackingLogId: logId);

        // Update body với tracking pixel
        await conn.ExecuteAsync(
            "UPDATE Marketing_Mail_Campaign_Send SET Body = @Body WHERE Id = @Id",
            new { Body = customerBody, Id = campaignSendId });

        // ── 3. Gửi email cho customer ─────────────────────────────────────────
        if (EmailHelper.IsValid(studentEmail))
        {
            await SendSmtpAsync(
                to: [(studentEmail, studentName)],
                bcc: bccEmails.Where(EmailHelper.IsValid).Select(e => (e, (string?)null)).ToList(),
                subject: customerSubject,
                htmlBody: customerBody,
                ct: ct);

            await conn.ExecuteAsync(
                """
                UPDATE Marketing_Mail_Send_Log
                SET Status = @Status, SentTime = GETDATE()
                WHERE Id = @Id
                """,
                new { Status = MailSendStatus.Sent, Id = logId });

            await conn.ExecuteAsync(
                """
                UPDATE Marketing_Mail_Campaign_Send
                SET TotalSent = TotalSent + 1, CompletedTime = GETDATE()
                WHERE Id = @Id
                """,
                new { Id = campaignSendId });

            _logger.LogInformation(
                "RegistrationMail sent to {Email} StudentId={StudentId} LogId={LogId}",
                studentEmail, studentId, logId);
        }
        else
        {
            await conn.ExecuteAsync(
                "UPDATE Marketing_Mail_Send_Log SET Status = @Status, ErrorMessage = @Msg WHERE Id = @Id",
                new { Status = MailSendStatus.Failed, Msg = "Invalid email address", Id = logId });
        }

        // ── 4. Gửi email admin notification ───────────────────────────────────
        if (adminEmails.Count > 0)
        {
            var adminSubject = $"[Đăng ký sự kiện] {studentName} – {eventName}";
            var adminBody    = BuildAdminBody(
                studentName, studentPhone, studentEmail, input.TinhThanh ?? string.Empty,
                eventName, eventLoc, eventDate, eventTime, regTime, siteName);

            foreach (var adminEmail in adminEmails.Where(EmailHelper.IsValid))
            {
                try
                {
                    var adminLogId = await conn.ExecuteScalarAsync<long>(
                        """
                        INSERT INTO Marketing_Mail_Send_Log
                            (CampaignSendId, ListMailId, Email, Status, CreatedDate)
                        VALUES
                            (@CampaignSendId, 0, @Email, @Status, GETDATE());
                        SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
                        """,
                        new { CampaignSendId = campaignSendId, Email = adminEmail, Status = MailSendStatus.Queued });

                    await SendSmtpAsync(
                        to: [(adminEmail, null)],
                        bcc: [],
                        subject: adminSubject,
                        htmlBody: adminBody,
                        ct: ct);

                    await conn.ExecuteAsync(
                        "UPDATE Marketing_Mail_Send_Log SET Status = @Status, SentTime = GETDATE() WHERE Id = @Id",
                        new { Status = MailSendStatus.Sent, Id = adminLogId });

                    await conn.ExecuteAsync(
                        "UPDATE Marketing_Mail_Campaign_Send SET TotalSent = TotalSent + 1 WHERE Id = @Id",
                        new { Id = campaignSendId });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Admin notification failed to {AdminEmail}", adminEmail);
                }
            }
        }
    }

    // ── SMTP sender ───────────────────────────────────────────────────────────

    private async Task SendSmtpAsync(
        List<(string Address, string? Name)> to,
        List<(string Address, string? Name)> bcc,
        string subject,
        string htmlBody,
        CancellationToken ct)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_displayName, _fromAddress));

        foreach (var (addr, name) in to)
            message.To.Add(string.IsNullOrWhiteSpace(name)
                ? new MailboxAddress(addr, addr)
                : new MailboxAddress(name, addr));

        foreach (var (addr, name) in bcc)
            message.Bcc.Add(string.IsNullOrWhiteSpace(name)
                ? new MailboxAddress(addr, addr)
                : new MailboxAddress(name, addr));

        message.Subject = subject;
        message.Body    = new TextPart(TextFormat.Html) { Text = htmlBody };

        using var smtp = new SmtpClient();
        var secOpt = _enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
        await smtp.ConnectAsync(_host, _port, secOpt, ct);
        await smtp.AuthenticateAsync(_user, _password, ct);
        await smtp.SendAsync(message, ct);
        await smtp.DisconnectAsync(true, ct);
    }

    // ── Template builder: Customer ────────────────────────────────────────────

    private string BuildCustomerBody(
        string studentName, string studentCode,
        string eventName, string eventLoc,
        string eventDate, string eventTime,
        string studentPhone,
        string logoUrl, string siteUrl, string siteName,
        string notes, bool sendCode,
        long trackingLogId)
    {
        var logoHtml = string.IsNullOrWhiteSpace(logoUrl)
            ? $"<span style=\"color:#fff;font-size:20px;font-weight:700;\">{H(siteName)}</span>"
            : $"<img src=\"{H(logoUrl)}\" alt=\"{H(siteName)}\" style=\"max-height:60px;\">";

        var qrSection = sendCode
            ? $"""
              <tr>
                <td style="padding:16px 0 8px;">
                  <p style="font-size:14px;color:#333;margin:0 0 8px;">
                    Mã đăng ký của bạn:
                  </p>
                  <div style="font-size:28px;font-weight:700;color:#003087;letter-spacing:4px;
                              background:#f0f4ff;padding:12px 24px;border-radius:6px;
                              display:inline-block;">
                    {H(studentCode)}
                  </div>
                </td>
              </tr>
              """
            : string.Empty;

        var notesSection = !string.IsNullOrWhiteSpace(notes)
            ? $"""
              <tr>
                <td style="background:#fffbea;border-left:4px solid #f5a623;
                           padding:12px 16px;border-radius:4px;margin-top:16px;">
                  <strong style="color:#b8820a;">Lưu ý quan trọng:</strong>
                  <div style="font-size:14px;color:#555;margin-top:6px;">{notes}</div>
                </td>
              </tr>
              """
            : string.Empty;

        var phoneRow = !string.IsNullOrWhiteSpace(studentPhone)
            ? $"""
              <tr>
                <td style="padding:6px 0;border-top:1px solid #dde4f5;">
                  <span style="color:#666;font-size:13px;">Điện thoại liên hệ</span><br>
                  <strong style="color:#333;font-size:14px;">{H(studentPhone)}</strong>
                </td>
              </tr>
              """
            : string.Empty;

        var trackingPixel = trackingLogId > 0
            ? $"<img src=\"{_siteBaseUrl}/api/EmailTracking/open?id={trackingLogId}\" width=\"1\" height=\"1\" style=\"display:none;\" alt=\"\">"
            : string.Empty;

        var template = LoadTemplate("customer-registration.html");

        return template
            .Replace("{{LOGO_HTML}}",       logoHtml)
            .Replace("{{STUDENT_NAME}}",    H(studentName))
            .Replace("{{EVENT_NAME}}",      H(eventName))
            .Replace("{{EVENT_LOCATION}}",  H(eventLoc))
            .Replace("{{EVENT_DATE}}",      H(eventDate))
            .Replace("{{EVENT_TIME}}",      H(eventTime))
            .Replace("{{ADDRESS_ROW}}",     string.Empty)
            .Replace("{{PHONE_ROW}}",       phoneRow)
            .Replace("{{QR_SECTION}}",      qrSection)
            .Replace("{{NOTES_SECTION}}",   notesSection)
            .Replace("{{SITE_URL}}",        H(siteUrl))
            .Replace("{{SITE_NAME}}",       H(siteName))
            .Replace("{{YEAR}}",            DateTime.Now.Year.ToString())
            .Replace("{{TRACKING_PIXEL}}",  trackingPixel);
    }

    // ── Template builder: Admin ───────────────────────────────────────────────

    private string BuildAdminBody(
        string studentName, string studentPhone, string studentEmail,
        string studentAddress,
        string eventName, string eventLoc,
        string eventDate, string eventTime,
        string regTime, string siteName)
    {
        var emailLink = EmailHelper.IsValid(studentEmail)
            ? $"<a href=\"mailto:{H(studentEmail)}\">{H(studentEmail)}</a>"
            : H(studentEmail);

        var template = LoadTemplate("admin-notification.html");

        return template
            .Replace("{{SITE_NAME}}",         H(siteName))
            .Replace("{{STUDENT_NAME}}",      H(studentName))
            .Replace("{{STUDENT_PHONE}}",     H(studentPhone))
            .Replace("{{STUDENT_EMAIL_LINK}}", emailLink)
            .Replace("{{STUDENT_ADDRESS}}",   H(studentAddress))
            .Replace("{{EVENT_NAME}}",        H(eventName))
            .Replace("{{EVENT_LOCATION}}",    H(eventLoc))
            .Replace("{{EVENT_DATE}}",        H(eventDate))
            .Replace("{{EVENT_TIME}}",        H(eventTime))
            .Replace("{{REGISTRATION_TIME}}", H(regTime));
    }

    // ── Template loader ───────────────────────────────────────────────────────

    private string LoadTemplate(string fileName)
    {
        lock (_cacheLock)
        {
            if (_templateCache.TryGetValue(fileName, out var cached))
                return cached;

            var path    = Path.Combine(_templateRoot, fileName);
            var content = File.Exists(path)
                ? File.ReadAllText(path)
                : $"<p>Template '{fileName}' not found.</p>";

            _templateCache[fileName] = content;
            return content;
        }
    }

    private static string H(string? s) =>
        System.Net.WebUtility.HtmlEncode(s ?? string.Empty);
}
