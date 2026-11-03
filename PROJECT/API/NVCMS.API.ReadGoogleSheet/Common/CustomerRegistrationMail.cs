using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Common;

/// <summary>Dữ liệu điền vào template customer-registration.html.</summary>
public sealed class CustomerRegistrationMailModel
{
    public string StudentName { get; init; } = string.Empty;
    public string StudentCode { get; init; } = string.Empty;
    public string StudentPhone { get; init; } = string.Empty;
    public string EventName { get; init; } = string.Empty;
    public string EventLocation { get; init; } = string.Empty;
    public string EventDate { get; init; } = string.Empty;
    public string EventTime { get; init; } = string.Empty;
    /// <summary>NV_Events_Cat.ContentMail (HTML) → khối "Lưu ý quan trọng".</summary>
    public string Notes { get; init; } = string.Empty;
    /// <summary>NV_Events_Cat.sendcode → có hiện QR/barcode hay không.</summary>
    public bool SendCode { get; init; }
    public string SiteName { get; init; } = string.Empty;
    public string SiteUrl { get; init; } = string.Empty;
    public string LogoUrl { get; init; } = string.Empty;
    /// <summary>Domain CRM sinh link check-in trong QR (CrmSync:UrlDomain).</summary>
    public string CheckinDomain { get; init; } = "http://crm.capstonevietnam.com";
}

/// <summary>
/// Mail xác nhận đăng ký sự kiện cho khách — dùng chung template
/// EmailTemplates/customer-registration.html với trang /dang-ky-su-kien
/// (Capstone.View/Services/EventRegistrationMailService.BuildCustomerBody).
/// Sửa giao diện mail thì sửa file template ở CẢ HAI project để 2 nơi gửi giống nhau.
/// </summary>
public static class CustomerRegistrationMail
{
    public const string TemplateName = "customer-registration.html";

    private const string QrHandlerBase = "https://crm.capstonevietnam.com/Services/QrcodeHandler.ashx";
    private const string BarHandlerBase = "https://crm.capstonevietnam.com/Services/BarcodeHandler.ashx";

    /// <summary>Tiêu đề giống trang đăng ký: "[SiteName] Xác nhận đăng ký - EventName".</summary>
    public static string BuildSubject(string siteName, string eventName)
        => $"[{siteName}] Xác nhận đăng ký - {eventName}";

    public static string BuildBody(EmailTemplateRenderer renderer, CustomerRegistrationMailModel m)
    {
        var logoHtml = string.IsNullOrWhiteSpace(m.LogoUrl)
            ? $"<span style=\"color:#fff;font-size:20px;font-weight:700;\">{H(m.SiteName)}</span>"
            : $"<img src=\"{H(m.LogoUrl)}\" alt=\"{H(m.SiteName)}\" style=\"max-height:60px;\">";

        var checkinUrl = $"{m.CheckinDomain.TrimEnd('/')}/quantri/partner/checkin-eventm.html?studentcode={Uri.EscapeDataString(m.StudentCode)}";
        var qrUrl = $"{QrHandlerBase}?data={Uri.EscapeDataString(checkinUrl)}&width=200&height=200";
        var barUrl = $"{BarHandlerBase}?data={Uri.EscapeDataString(m.StudentCode)}&type=barcode&width=400&height=100";

        var qrSection = m.SendCode
            ? $"""
              <tr>
                <td style="padding:16px 0 8px; text-align: center;">
                    <p style="font-size:20px;font-weight:700;color:#333;margin:0 0 10px;">
                        Mã đăng ký của bạn:
                    </p>
                    <p style="font-size:14px;color:#333;margin:0 0 8px; text-align: center;">
                        <center><img src="{qrUrl}" width="300" height="300" alt="QR Code" style="display:block;border:1px solid #e0e0e0;border-radius:4px;"></center>
                    </p>
                    <p style="font-size:14px;color:#333;margin:0 0 8px; text-align: center;">
                        <center><img src="{barUrl}" width="220" height="60" alt="Barcode" style="display:block;border:1px solid #e0e0e0;border-radius:4px;"></center>
                    </p>
                    <div style="font-size:28px;font-weight:700;color:#003087;letter-spacing:4px; background:#f0f4ff;padding:12px 24px;border-radius:6px; display:inline-block;">
                        {H(m.StudentCode)}
                    </div>
                </td>
              </tr>
              """
            : string.Empty;

        var notesSection = !string.IsNullOrWhiteSpace(m.Notes)
            ? $"""
              <tr>
                <td style="background:#fffbea;border-left:4px solid #f5a623;
                           padding:12px 16px;border-radius:4px;margin-top:16px;">
                  <strong style="color:#b8820a;">Lưu ý quan trọng:</strong>
                  <div style="font-size:14px;color:#555;margin-top:6px;">{m.Notes}</div>
                </td>
              </tr>
              """
            : string.Empty;

        var phoneRow = !string.IsNullOrWhiteSpace(m.StudentPhone)
            ? $"""
              <tr>
                <td style="padding:6px 0;border-top:1px solid #dde4f5;">
                  <span style="color:#666;font-size:13px;">Điện thoại liên hệ</span><br>
                  <strong style="color:#333;font-size:14px;">{H(m.StudentPhone)}</strong>
                </td>
              </tr>
              """
            : string.Empty;

        return renderer.Render(TemplateName, new Dictionary<string, string>
        {
            ["LOGO_HTML"] = logoHtml,
            ["STUDENT_NAME"] = H(m.StudentName),
            ["EVENT_NAME"] = H(m.EventName),
            ["EVENT_LOCATION"] = H(m.EventLocation),
            ["EVENT_DATE"] = H(m.EventDate),
            ["EVENT_TIME"] = H(m.EventTime),
            ["ADDRESS_ROW"] = string.Empty,
            ["PHONE_ROW"] = phoneRow,
            ["QR_SECTION"] = qrSection,
            ["NOTES_SECTION"] = notesSection,
            ["SITE_URL"] = H(m.SiteUrl),
            ["SITE_NAME"] = H(m.SiteName),
            ["YEAR"] = DateTime.Now.Year.ToString(),
            // Job không ghi Marketing_Mail_Send_Log nên không có tracking pixel
            ["TRACKING_PIXEL"] = string.Empty,
        });
    }

    private static string H(string? s) => System.Net.WebUtility.HtmlEncode(s ?? string.Empty);
}
