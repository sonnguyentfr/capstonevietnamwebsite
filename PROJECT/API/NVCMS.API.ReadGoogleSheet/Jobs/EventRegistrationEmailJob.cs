using Hangfire;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Hangfire job: sends customer confirmation email + admin notification
    /// after a successful event registration from Capstone.View.
    ///
    /// Enqueued by the API's EventRegistrationEmailController.
    /// Runs inside the existing Hangfire worker — never blocks the web request.
    /// </summary>
    public class EventRegistrationEmailJob
    {
        private readonly IEmailService _email;
        private readonly ILogger<EventRegistrationEmailJob> _logger;

        // CRM QrCode handler (already used by the legacy system)
        private const string QrHandlerBase =
            "https://crm.capstonevietnam.com/Services/QrcodeHandler.ashx";

        public EventRegistrationEmailJob(
            IEmailService email,
            ILogger<EventRegistrationEmailJob> logger)
        {
            _email  = email;
            _logger = logger;
        }

        // ── Called by Hangfire ────────────────────────────────────────────────

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
        public async Task SendAsync(EventRegistrationEmailRequest request)
        {
            // ── Customer email ────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(request.StudentEmail))
            {
                try
                {
                    var body    = BuildCustomerBody(request);
                    var subject = $"Đăng ký thành công - {request.EventName}";
                    await _email.SendEmailAsync(request.StudentEmail, subject, body);

                    _logger.LogInformation(
                        "CustomerEmailSuccess: StudentId={StudentId} EventCatId={EventCatId}",
                        request.StudentId, request.EventCatId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "CustomerEmailFailure: StudentId={StudentId} EventCatId={EventCatId}",
                        request.StudentId, request.EventCatId);
                    // Do NOT rethrow — email failure must not rollback registration
                }
            }

            // ── Admin emails ──────────────────────────────────────────────────
            foreach (var adminEmail in request.AdminEmails)
            {
                try
                {
                    var body    = BuildAdminBody(request);
                    var subject = $"[ĐĂNG KÝ SỰ KIỆN] {request.StudentName} – {request.EventName}";
                    await _email.SendEmailAsync(adminEmail, subject, body);

                    _logger.LogInformation(
                        "AdminEmailSuccess: AdminEmail={AdminEmail} EventCatId={EventCatId}",
                        adminEmail, request.EventCatId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "AdminEmailFailure: AdminEmail={AdminEmail} EventCatId={EventCatId}",
                        adminEmail, request.EventCatId);
                }
            }
        }

        // ── HTML builders ─────────────────────────────────────────────────────

        private static string BuildCustomerBody(EventRegistrationEmailRequest r)
        {
            var qrSection = string.Empty;
            if (r.SendCode && !string.IsNullOrWhiteSpace(r.StudentCode))
            {
                var qrUrl  = $"{QrHandlerBase}?data={Uri.EscapeDataString(r.StudentCode)}&width=200&height=200";
                var barUrl = $"{QrHandlerBase}?data={Uri.EscapeDataString(r.StudentCode)}&type=barcode&width=300&height=80";
                qrSection = $"""
                    <tr>
                      <td style="padding:16px 0 8px;">
                        <p style="font-size:14px;color:#555;margin:0 0 8px;">
                          Vui lòng trình mã dưới đây khi check-in tại sự kiện:
                        </p>
                        <table cellpadding="0" cellspacing="0">
                          <tr>
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
                          </tr>
                        </table>
                        <p style="font-size:12px;color:#888;margin-top:6px;">
                          Mã đăng ký: <strong>{System.Net.WebUtility.HtmlEncode(r.StudentCode)}</strong>
                        </p>
                      </td>
                    </tr>
                """;
            }

            var notesSection = string.Empty;
            if (!string.IsNullOrWhiteSpace(r.ImportantNotes))
            {
                notesSection = $"""
                    <tr>
                      <td>
                        <div style="background:#fffbea;border-left:4px solid #f5a623;
                                    padding:12px 16px;border-radius:4px;margin-top:16px;">
                          <p style="margin:0 0 4px;font-size:13px;font-weight:600;color:#7c5800;">
                            Lưu ý quan trọng
                          </p>
                          <div style="font-size:13px;color:#555;">{r.ImportantNotes}</div>
                        </div>
                      </td>
                    </tr>
                """;
            }

            var logoHtml = string.IsNullOrWhiteSpace(r.CompanyLogoUrl)
                ? $"<span style=\"color:#fff;font-size:22px;font-weight:700;\">{System.Net.WebUtility.HtmlEncode(r.SiteName)}</span>"
                : $"<img src=\"{r.CompanyLogoUrl}\" alt=\"{System.Net.WebUtility.HtmlEncode(r.SiteName)}\" height=\"48\" style=\"max-height:48px;object-fit:contain;\">";

            return $"""
                <!DOCTYPE html><html lang="vi"><head><meta charset="UTF-8">
                <title>Đăng ký thành công – {System.Net.WebUtility.HtmlEncode(r.EventName)}</title></head>
                <body style="margin:0;padding:0;background:#f4f6f9;font-family:'Segoe UI',Arial,sans-serif;">
                <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f6f9;padding:30px 0;">
                  <tr><td align="center">
                    <table width="600" cellpadding="0" cellspacing="0"
                           style="background:#fff;border-radius:8px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,.08);">
                      <tr>
                        <td style="background:#003087;padding:24px 32px;text-align:center;">
                          {logoHtml}
                        </td>
                      </tr>
                      <tr>
                        <td style="background:#0051b4;padding:18px 32px;text-align:center;">
                          <h1 style="margin:0;color:#fff;font-size:19px;font-weight:600;">
                            ✅ Đăng ký thành công!
                          </h1>
                        </td>
                      </tr>
                      <tr>
                        <td style="padding:28px 32px;">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td>
                                <p style="margin:0 0 16px;font-size:15px;color:#333;">
                                  Xin chào <strong>{System.Net.WebUtility.HtmlEncode(r.StudentName)}</strong>,
                                </p>
                                <p style="margin:0 0 20px;font-size:15px;color:#333;">
                                  Capstone Vietnam xác nhận bạn đã đăng ký thành công tham dự sự kiện.
                                </p>
                                <table width="100%" cellpadding="0" cellspacing="0"
                                       style="background:#f0f4ff;border-radius:6px;padding:16px;margin-bottom:8px;">
                                  <tr>
                                    <td style="padding:6px 0;">
                                      <span style="color:#666;font-size:13px;">Sự kiện</span><br>
                                      <strong style="color:#003087;font-size:15px;">{System.Net.WebUtility.HtmlEncode(r.EventName)}</strong>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td style="padding:6px 0;border-top:1px solid #dde4f5;">
                                      <span style="color:#666;font-size:13px;">Địa điểm</span><br>
                                      <strong style="color:#333;font-size:14px;">{System.Net.WebUtility.HtmlEncode(r.EventLocation)}</strong>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td style="padding:6px 0;border-top:1px solid #dde4f5;">
                                      <span style="color:#666;font-size:13px;">Ngày</span>&nbsp;
                                      <strong style="color:#333;font-size:14px;">{System.Net.WebUtility.HtmlEncode(r.EventDate)}</strong>
                                      &nbsp;&nbsp;
                                      <span style="color:#666;font-size:13px;">Giờ</span>&nbsp;
                                      <strong style="color:#333;font-size:14px;">{System.Net.WebUtility.HtmlEncode(r.EventTime)}</strong>
                                    </td>
                                  </tr>
                                  {(string.IsNullOrEmpty(r.StudentAddress) ? "" : $"""
                                  <tr>
                                    <td style="padding:6px 0;border-top:1px solid #dde4f5;">
                                      <span style="color:#666;font-size:13px;">Địa chỉ</span><br>
                                      <span style="color:#333;font-size:14px;">{System.Net.WebUtility.HtmlEncode(r.StudentAddress)}</span>
                                    </td>
                                  </tr>
                                  """)}
                                  {(string.IsNullOrEmpty(r.StudentPhone) ? "" : $"""
                                  <tr>
                                    <td style="padding:6px 0;border-top:1px solid #dde4f5;">
                                      <span style="color:#666;font-size:13px;">Số điện thoại</span><br>
                                      <span style="color:#333;font-size:14px;">{System.Net.WebUtility.HtmlEncode(r.StudentPhone)}</span>
                                    </td>
                                  </tr>
                                  """)}
                                </table>
                              </td>
                            </tr>
                            {qrSection}
                            {notesSection}
                            <tr>
                              <td>
                                <p style="font-size:14px;color:#555;margin-top:24px;">
                                  Nếu có thắc mắc, vui lòng liên hệ qua
                                  <a href="{r.SiteUrl}" style="color:#0051b4;">{System.Net.WebUtility.HtmlEncode(r.SiteUrl)}</a>.
                                </p>
                                <p style="font-size:14px;color:#555;">
                                  Trân trọng,<br><strong>{System.Net.WebUtility.HtmlEncode(r.SiteName)}</strong>
                                </p>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <tr>
                        <td style="background:#f0f0f0;padding:14px 32px;text-align:center;
                                   font-size:12px;color:#999;">
                          © {DateTime.Now.Year} {System.Net.WebUtility.HtmlEncode(r.SiteName)}.
                          Email này được gửi tự động, vui lòng không trả lời.
                        </td>
                      </tr>
                    </table>
                  </td></tr>
                </table>
                </body></html>
            """;
        }

        private static string BuildAdminBody(EventRegistrationEmailRequest r)
        {
            return $"""
                <!DOCTYPE html><html lang="vi"><head><meta charset="UTF-8">
                <title>Đăng ký sự kiện mới – {System.Net.WebUtility.HtmlEncode(r.EventName)}</title></head>
                <body style="margin:0;padding:0;background:#f4f6f9;font-family:'Segoe UI',Arial,sans-serif;">
                <table width="100%" cellpadding="0" cellspacing="0" style="background:#f4f6f9;padding:30px 0;">
                  <tr><td align="center">
                    <table width="600" cellpadding="0" cellspacing="0"
                           style="background:#fff;border-radius:8px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,.08);">
                      <tr>
                        <td style="background:#003087;padding:18px 32px;text-align:center;">
                          <h2 style="margin:0;color:#fff;font-size:16px;font-weight:600;">
                            [ĐĂNG KÝ SỰ KIỆN] Thông báo nội bộ
                          </h2>
                        </td>
                      </tr>
                      <tr>
                        <td style="padding:26px 32px;">
                          <p style="margin:0 0 16px;font-size:14px;color:#333;">
                            Có học sinh vừa đăng ký tham dự sự kiện qua website
                            <strong>{System.Net.WebUtility.HtmlEncode(r.SiteName)}</strong>.
                          </p>
                          <table width="100%" cellpadding="0" cellspacing="0"
                                 style="border-collapse:collapse;font-size:14px;">
                            <tr style="background:#f5f7ff;">
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#666;width:40%;">Họ và tên</td>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#333;font-weight:600;">
                                {System.Net.WebUtility.HtmlEncode(r.StudentName)}
                              </td>
                            </tr>
                            <tr>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#666;">Số điện thoại</td>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#333;">
                                {System.Net.WebUtility.HtmlEncode(r.StudentPhone)}
                              </td>
                            </tr>
                            <tr style="background:#f5f7ff;">
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#666;">Email</td>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#333;">
                                {(string.IsNullOrEmpty(r.StudentEmail)
                                    ? "<span style=\"color:#aaa;\">—</span>"
                                    : $"<a href=\"mailto:{System.Net.WebUtility.HtmlEncode(r.StudentEmail)}\" style=\"color:#0051b4;\">{System.Net.WebUtility.HtmlEncode(r.StudentEmail)}</a>")}
                              </td>
                            </tr>
                            <tr>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#666;">Sự kiện</td>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#333;font-weight:600;">
                                {System.Net.WebUtility.HtmlEncode(r.EventName)}
                              </td>
                            </tr>
                            <tr style="background:#f5f7ff;">
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#666;">Địa điểm đăng ký</td>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#333;">
                                {System.Net.WebUtility.HtmlEncode(r.EventLocation)}
                              </td>
                            </tr>
                            <tr>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#666;">Thời điểm đăng ký</td>
                              <td style="padding:10px 12px;border:1px solid #e0e6f0;color:#333;">
                                {System.Net.WebUtility.HtmlEncode(r.RegistrationTime)}
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <tr>
                        <td style="background:#f0f0f0;padding:14px 32px;text-align:center;
                                   font-size:12px;color:#999;">
                          Email tự động từ hệ thống {System.Net.WebUtility.HtmlEncode(r.SiteName)}.
                          Vui lòng không trả lời email này.
                        </td>
                      </tr>
                    </table>
                  </td></tr>
                </table>
                </body></html>
            """;
        }
    }

    // ── Payload DTO ─────────────────────────────────────────────────────────────

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
    }
}
