using Dapper;
using Hangfire;
using Microsoft.Data.SqlClient;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Hangfire job: gá»­i email xÃ¡c nháº­n Ä‘Äƒng kÃ½ sá»± kiá»‡n cho khÃ¡ch hÃ ng + thÃ´ng bÃ¡o admin.
    ///
    /// Flow:
    ///   1. INSERT Marketing_Mail_Campaign_Send  â†’ campaignSendId (Ä‘áº¡i diá»‡n cho "Ä‘á»£t gá»­i" nÃ y)
    ///   2. INSERT Marketing_Mail_Send_Log       â†’ logId (1 row / email)
    ///   3. Gá»­i SMTP kÃ¨m tracking pixel /api/EmailTracking/open?id={logId}
    ///   4. UPDATE Send_Log  Status=Sent / Failed
    ///   5. UPDATE Campaign_Send TotalSent / TotalFailed counter
    /// </summary>
    public class EventRegistrationEmailJob
    {
        private readonly IEmailService _email;
        private readonly string _crmConnStr;
        private readonly string _apiBaseUrl;
        private readonly EmailTemplateRenderer _renderer;
        private readonly ILogger<EventRegistrationEmailJob> _logger;

        // CRM QrCode handler (already used by the legacy system)
        private const string QrHandlerBase =
            "https://crm.capstonevietnam.com/Services/QrcodeHandler.ashx";
        private const string BarHandlerBase =
           "https://crm.capstonevietnam.com/Services/BarcodeHandler.ashx";

        public EventRegistrationEmailJob(
            IEmailService email,
            IConfiguration config,
            EmailTemplateRenderer renderer,
            ILogger<EventRegistrationEmailJob> logger)
        {
            _email      = email;
            _crmConnStr = config.GetConnectionString("DefaultCRMConnection")
                          ?? throw new InvalidOperationException("DefaultCRMConnection not configured");
            _apiBaseUrl = (config["ApiSelfBaseUrl"] ?? string.Empty).TrimEnd('/');
            _renderer   = renderer;
            _logger     = logger;
        }

        // â”€â”€ Called by Hangfire â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
        public async Task SendAsync(EventRegistrationEmailRequest request)
        {
            _logger.LogInformation(
                "EventRegistrationEmailJob.SendAsync start: StudentId={Id} EventCatId={Cat}",
                request.StudentId, request.EventCatId);

            // â”€â”€ 1. Táº¡o Campaign_Send Ä‘á»ƒ nhÃ³m toÃ n bá»™ emails trong láº§n gá»­i nÃ y â”€â”€
            var subject        = $"Đăng ký thành công - {request.EventName}";
            var adminSubject   = $"[ĐĂNG KÝ SỰ KIỆN] {request.StudentName} - {request.EventName}";
            int totalRecipient = (!string.IsNullOrWhiteSpace(request.StudentEmail) ? 1 : 0)
                               + request.AdminEmails.Count;

            var campaignSendId = await CreateCampaignSendAsync(subject, request, totalRecipient);
            _logger.LogInformation("CampaignSendId={CampaignSendId}", campaignSendId);

            int totalSent   = 0;
            int totalFailed = 0;

            // â”€â”€ 2. Gá»­i email khÃ¡ch hÃ ng â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
            if (!string.IsNullOrWhiteSpace(request.StudentEmail))
            {
                var logId = await CreateSendLogAsync(campaignSendId, request.StudentEmail);
                try
                {
                    var body = BuildCustomerBody(request, logId);
                    var bcc  = request.BccEmails.Count > 0
                        ? string.Join(",", request.BccEmails) : null;
                    await _email.SendEmailAsync(request.StudentEmail, subject, body, bccEmail: bcc);
                    await UpdateSendLogSentAsync(logId);
                    totalSent++;
                    _logger.LogInformation(
                        "CustomerEmailSent: StudentId={Id} LogId={LogId}", request.StudentId, logId);
                }
                catch (Exception ex)
                {
                    await UpdateSendLogFailedAsync(logId, ex.Message);
                    totalFailed++;
                    _logger.LogError(ex,
                        "CustomerEmailFailed: StudentId={Id} LogId={LogId}", request.StudentId, logId);
                }
            }

            // â”€â”€ 3. Gá»­i email admin â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
            foreach (var adminEmail in request.AdminEmails)
            {
                var logId = await CreateSendLogAsync(campaignSendId, adminEmail);
                try
                {
                    var body = BuildAdminBody(request);
                    await _email.SendEmailAsync(adminEmail, adminSubject, body);
                    await UpdateSendLogSentAsync(logId);
                    totalSent++;
                    _logger.LogInformation(
                        "AdminEmailSent: {Email} LogId={LogId}", adminEmail, logId);
                }
                catch (Exception ex)
                {
                    await UpdateSendLogFailedAsync(logId, ex.Message);
                    totalFailed++;
                    _logger.LogError(ex,
                        "AdminEmailFailed: {Email} LogId={LogId}", adminEmail, logId);
                }
            }

            // â”€â”€ 4. Cáº­p nháº­t Campaign_Send counters â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
            await UpdateCampaignSendCountersAsync(campaignSendId, totalSent, totalFailed);

            _logger.LogInformation(
                "EventRegistrationEmailJob.SendAsync done: CampaignSendId={Id} Sent={S} Failed={F}",
                campaignSendId, totalSent, totalFailed);
        }

        // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
        // DB helpers â€” Dapper trá»±c tiáº¿p, khÃ´ng qua EF scope (trÃ¡nh DI conflict)
        // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•

        /// <summary>
        /// INSERT Marketing_Mail_Campaign_Send â†’ tráº£ vá» Id má»›i.
        /// CampaignId=0 (transactional, khÃ´ng thuá»™c campaign marketing).
        /// TemplateId=0 (inline HTML, khÃ´ng dÃ¹ng template).
        /// Status=1 (Sending).
        /// </summary>
        private async Task<int> CreateCampaignSendAsync(
            string subject, EventRegistrationEmailRequest r, int totalRecipient)
        {
            const string sql = """
                INSERT INTO Marketing_Mail_Campaign_Send
                    (CampaignId, TemplateId, Subject, Body,
                     Status, TotalRecipient, TotalSent, TotalDelivered,
                     TotalOpened, TotalClicked, TotalBounced, TotalComplaint, TotalUnsubscribed,
                     StartedTime, CreatedDate)
                VALUES
                    (0, 0, @Subject, @Body,
                     1, @TotalRecipient, 0, 0,
                     0, 0, 0, 0, 0,
                     GETDATE(), GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;
            try
            {
                // LÆ°u body cá»§a customer email (ná»™i dung chÃ­nh) vÃ o Campaign_Send.Body
                var previewBody = BuildCustomerBody(r, 0);
                await using var conn = new SqlConnection(_crmConnStr);
                var id = await conn.ExecuteScalarAsync<int>(sql, new
                {
                    Subject        = subject,
                    Body           = previewBody,
                    TotalRecipient = totalRecipient,
                });
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CreateCampaignSend failed â€” continuing without CampaignSendId");
                return 0;
            }
        }

        /// <summary>
        /// INSERT Marketing_Mail_Send_Log â†’ tráº£ vá» Id.
        /// CampaignSendId liÃªn káº¿t vá»›i Campaign_Send.
        /// ListMailId=0 (Ä‘Äƒng kÃ½ trá»±c tiáº¿p, khÃ´ng qua list mail).
        /// </summary>
        private async Task<long> CreateSendLogAsync(int campaignSendId, string toEmail)
        {
            const string sql = """
                INSERT INTO Marketing_Mail_Send_Log
                    (CampaignSendId, ListMailId, Email, Status, CreatedDate)
                VALUES
                    (@CampaignSendId, 0, @Email, @Status, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS bigint);
                """;
            try
            {
                await using var conn = new SqlConnection(_crmConnStr);
                var id = await conn.ExecuteScalarAsync<long>(sql, new
                {
                    CampaignSendId = campaignSendId,
                    Email          = toEmail,
                    Status         = MailSendStatus.Queued,
                });
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CreateSendLog failed for {Email}", toEmail);
                return 0;
            }
        }

        private async Task UpdateSendLogSentAsync(long logId)
        {
            if (logId <= 0) return;
            try
            {
                await using var conn = new SqlConnection(_crmConnStr);
                await conn.ExecuteAsync(
                    "UPDATE Marketing_Mail_Send_Log SET Status=@S, SentTime=GETDATE() WHERE Id=@Id",
                    new { S = MailSendStatus.Sent, Id = logId });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "UpdateSendLogSent failed LogId={Id}", logId);
            }
        }

        private async Task UpdateSendLogFailedAsync(long logId, string errorMessage)
        {
            if (logId <= 0) return;
            try
            {
                await using var conn = new SqlConnection(_crmConnStr);
                await conn.ExecuteAsync(
                    "UPDATE Marketing_Mail_Send_Log SET Status=@S, ErrorMessage=@E WHERE Id=@Id",
                    new { S = MailSendStatus.Failed, E = errorMessage, Id = logId });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "UpdateSendLogFailed failed LogId={Id}", logId);
            }
        }

        /// <summary>
        /// Cáº­p nháº­t TotalSent, TotalBounced vÃ  CompletedTime cho Campaign_Send.
        /// </summary>
        private async Task UpdateCampaignSendCountersAsync(int campaignSendId, int sent, int failed)
        {
            if (campaignSendId <= 0) return;
            try
            {
                await using var conn = new SqlConnection(_crmConnStr);
                await conn.ExecuteAsync("""
                    UPDATE Marketing_Mail_Campaign_Send
                    SET TotalSent    = @Sent,
                        TotalBounced = @Failed,
                        Status       = 2,
                        CompletedTime = GETDATE()
                    WHERE Id = @Id
                    """,
                    new { Sent = sent, Failed = failed, Id = campaignSendId });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "UpdateCampaignSendCounters failed Id={Id}", campaignSendId);
            }
        }

        // â”€â”€ HTML builders â€” dÃ¹ng file template trong /EmailTemplates/ â”€â”€â”€â”€â”€â”€â”€â”€â”€

        private string BuildCustomerBody(EventRegistrationEmailRequest r, long logId)
        {
            // QR / Barcode section
            var qrSection = string.Empty;
            if (r.SendCode && !string.IsNullOrWhiteSpace(r.StudentCode))
            {
                var checkinUrl = $"http://crm.capstonevietnam.com/quantri/partner/checkin-eventm.html?studentcode={Uri.EscapeDataString(r.StudentCode)}";
                var qrUrl  = $"{QrHandlerBase}?data={Uri.EscapeDataString(checkinUrl)}&width=200&height=200";
                var barUrl = $"{BarHandlerBase}?data={Uri.EscapeDataString(r.StudentCode)}&type=barcode&width=400&height=100";
                qrSection = $"""
                    <tr>
                      <td style="padding:16px 0 8px;">
                        <p style="font-size:14px;color:#555;margin:0 0 8px;">
                          Vui lòng trình mã dưới đây khi check-in tại sự kiện:
                        </p>
                        <table cellpadding="0" cellspacing="0"><tr>
                          <td style="padding-right:20px;text-align:center;">
                            <p style="margin:0 0 4px;font-size:12px;color:#888;">QR Code</p>
                            <img src="{qrUrl}" width="160" height="160" alt="QR Code"
                                 style="display:block;border:1px solid #e0e0e0;border-radius:4px;">
                          </td>
                          <td style="text-align:center;">
                            <p style="margin:0 0 4px;font-size:12px;color:#888;">Barcode</p>
                            <img src="{barUrl}" width="220" height="60" alt="Barcode"
                                 style="display:block;border:1px solid #e0e0e0;border-radius:4px;">
                          </td>
                        </tr></table>
                        <p style="font-size:12px;color:#888;margin-top:6px;">
                          Mã đăng ký: <strong>{H(r.StudentCode)}</strong>
                        </p>
                      </td>
                    </tr>
                    """;
            }

            // Notes section
            var notesSection = string.Empty;
            if (!string.IsNullOrWhiteSpace(r.ImportantNotes))
            {
                notesSection = $"""
                    <tr><td>
                      <div style="background:#fffbea;border-left:4px solid #f5a623;
                                  padding:12px 16px;border-radius:4px;margin-top:16px;">
                        <p style="margin:0 0 4px;font-size:13px;font-weight:600;color:#7c5800;">
                          Lưu ý quan trọng
                        </p>
                        <div style="font-size:13px;color:#555;">{r.ImportantNotes}</div>
                      </div>
                    </td></tr>
                    """;
            }

            // Logo
            var logoHtml = string.IsNullOrWhiteSpace(r.CompanyLogoUrl)
                ? $"<span style=\"color:#fff;font-size:22px;font-weight:700;\">{H(r.SiteName)}</span>"
                : $"<img src=\"{r.CompanyLogoUrl}\" alt=\"{H(r.SiteName)}\" height=\"48\" style=\"max-height:48px;object-fit:contain;\">";

            // Optional rows
            var addressRow = string.IsNullOrEmpty(r.StudentAddress) ? "" : $"""
                <tr><td style="padding:6px 0;border-top:1px solid #dde4f5;">
                  <span style="color:#666;font-size:13px;">Địa chỉ</span><br>
                  <span style="color:#333;font-size:14px;">{H(r.StudentAddress)}</span>
                </td></tr>
                """;

            var phoneRow = string.IsNullOrEmpty(r.StudentPhone) ? "" : $"""
                <tr><td style="padding:6px 0;border-top:1px solid #dde4f5;">
                  <span style="color:#666;font-size:13px;">Số điện thoại</span><br>
                  <span style="color:#333;font-size:14px;">{H(r.StudentPhone)}</span>
                </td></tr>
                """;

            // Tracking pixel
            var trackingPixel = logId > 0 && !string.IsNullOrEmpty(_apiBaseUrl)
                ? $"<img src=\"{_apiBaseUrl}/api/EmailTracking/open?id={logId}\" width=\"1\" height=\"1\" alt=\"\" style=\"display:none;\">"
                : "";

            return _renderer.Render("customer-registration.html", new Dictionary<string, string>
            {
                ["EVENT_NAME"]      = H(r.EventName),
                ["LOGO_HTML"]       = logoHtml,
                ["STUDENT_NAME"]    = H(r.StudentName),
                ["EVENT_LOCATION"]  = H(r.EventLocation),
                ["EVENT_DATE"]      = H(r.EventDate),
                ["EVENT_TIME"]      = H(r.EventTime),
                ["ADDRESS_ROW"]     = addressRow,
                ["PHONE_ROW"]       = phoneRow,
                ["QR_SECTION"]      = qrSection,
                ["NOTES_SECTION"]   = notesSection,
                ["SITE_URL"]        = r.SiteUrl,
                ["SITE_NAME"]       = H(r.SiteName),
                ["YEAR"]            = DateTime.Now.Year.ToString(),
                ["TRACKING_PIXEL"]  = trackingPixel,
            });
        }

        private string BuildAdminBody(EventRegistrationEmailRequest r)
        {
            var emailLink = string.IsNullOrEmpty(r.StudentEmail)
                ? "<span style=\"color:#aaa;\">â€”</span>"
                : $"<a href=\"mailto:{H(r.StudentEmail)}\" style=\"color:#0051b4;\">{H(r.StudentEmail)}</a>";

            return _renderer.Render("admin-notification.html", new Dictionary<string, string>
            {
                ["SITE_NAME"]         = H(r.SiteName),
                ["STUDENT_NAME"]      = H(r.StudentName),
                ["STUDENT_PHONE"]     = H(r.StudentPhone),
                ["STUDENT_EMAIL_LINK"]= emailLink,
                ["STUDENT_ADDRESS"]   = H(r.StudentAddress),
                ["EVENT_NAME"]        = H(r.EventName),
                ["EVENT_LOCATION"]    = H(r.EventLocation),
                ["EVENT_DATE"]        = H(r.EventDate),
                ["EVENT_TIME"]        = H(r.EventTime),
                ["REGISTRATION_TIME"] = H(r.RegistrationTime),
            });
        }

        // HTML encode shorthand
        private static string H(string? s) =>
            System.Net.WebUtility.HtmlEncode(s ?? string.Empty);
    }

    // â”€â”€ Payload DTO â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    public class EventRegistrationEmailRequest
    {
        public int      StudentId      { get; set; }
        public string   StudentCode    { get; set; } = string.Empty;
        public string   StudentName    { get; set; } = string.Empty;
        public string   StudentPhone   { get; set; } = string.Empty;
        public string   StudentEmail   { get; set; } = string.Empty;
        public string   StudentAddress { get; set; } = string.Empty;

        public int    EventCatId      { get; set; }
        public int    EventId         { get; set; }
        public string EventName       { get; set; } = string.Empty;
        public string EventLocation   { get; set; } = string.Empty;
        public string EventDate       { get; set; } = string.Empty;
        public string EventTime       { get; set; } = string.Empty;
        public string RegistrationTime { get; set; } = string.Empty;

        public bool    SendCode        { get; set; }
        public string? ImportantNotes  { get; set; }

        public string   CompanyLogoUrl { get; set; } = string.Empty;
        public string   SiteUrl        { get; set; } = string.Empty;
        public string   SiteName       { get; set; } = string.Empty;

        public List<string> AdminEmails { get; set; } = [];

        /// <summary>BCC recipients for the customer confirmation email (FixedEmail + cat.Email).</summary>
        public List<string> BccEmails { get; set; } = [];
    }
}

