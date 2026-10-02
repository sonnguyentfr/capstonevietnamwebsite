using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Services;

/// <summary>
/// Gửi mail thông báo nội bộ khi có đăng ký tư vấn mới từ /dang-ky-tu-van.
/// Người nhận cấu hình tại appsettings "TuVan:NotifyEmails".
/// </summary>
public class TuVanDangKyMailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly bool _enableSsl;
    private readonly string _fromAddress;
    private readonly string _user;
    private readonly string _password;
    private readonly string _displayName;
    private readonly ILogger<TuVanDangKyMailService> _logger;

    public TuVanDangKyMailService(IConfiguration config, ILogger<TuVanDangKyMailService> logger)
    {
        var sec = config.GetSection("Email");
        _host        = sec["Host"] ?? "localhost";
        _port        = int.TryParse(sec["Port"], out var p) ? p : 587;
        _enableSsl   = bool.TryParse(sec["EnableSsl"], out var s) && s;
        _fromAddress = sec["FromEmailAddress"] ?? string.Empty;
        _user        = sec["UserMail"] ?? string.Empty;
        _password    = sec["Password"] ?? string.Empty;
        _displayName = sec["DisplayName"] ?? "Capstone Vietnam";
        _logger      = logger;
    }

    public async Task SendNotifyAsync(TuVanDangKyResult r, string notifyEmails, CancellationToken ct = default)
    {
        var recipients = (notifyEmails ?? string.Empty)
            .Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(EmailHelper.IsValid)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (recipients.Count == 0)
        {
            _logger.LogWarning("TuVan:NotifyEmails chưa cấu hình → bỏ qua mail thông báo (StudentId={StudentId})", r.StudentId);
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_displayName, _fromAddress));
        foreach (var addr in recipients)
            message.To.Add(MailboxAddress.Parse(addr));

        var contact = string.Join(" - ", new[] { r.SoDienThoai, r.Email }
            .Where(x => x is not ("" or "0" or "NA")));
        var tag = string.IsNullOrEmpty(r.NguonTrang) ? "WEBSITE" : "WEBSITE - SIDEBAR";
        message.Subject = $"[TƯ VẤN MỚI - {tag}] {r.HoVaTen} - {contact}";
        message.Body = new TextPart(TextFormat.Html) { Text = BuildBody(r) };

        using var smtp = new SmtpClient();
        var secOpt = _enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
        await smtp.ConnectAsync(_host, _port, secOpt, ct);
        await smtp.AuthenticateAsync(_user, _password, ct);
        await smtp.SendAsync(message, ct);
        await smtp.DisconnectAsync(true, ct);
    }

    private static string H(string? s) => WebUtility.HtmlEncode(s ?? string.Empty);

    private static string Row(string label, string? value) => $"""
        <tr>
          <td style="padding:9px 12px;border-bottom:1px solid #eee;width:190px;color:#555;"><b>{H(label)}</b></td>
          <td style="padding:9px 12px;border-bottom:1px solid #eee;">{(string.IsNullOrWhiteSpace(value) ? "<i style=\"color:#999\">—</i>" : H(value).Replace("\n", "<br/>"))}</td>
        </tr>
        """;

    private static string BuildBody(TuVanDangKyResult r)
    {
        var status = r.IsExisting
            ? """<span style="background:#f0ad4e;color:#fff;padding:4px 10px;border-radius:12px;font-size:12px;">Khách đã có trên CRM – chỉ cập nhật "Tư vấn khác"</span>"""
            : """<span style="background:#28a745;color:#fff;padding:4px 10px;border-radius:12px;font-size:12px;">Khách hàng mới</span>""";

        return $"""
<!DOCTYPE html>
<html lang="vi">
<head><meta charset="UTF-8"></head>
<body style="margin:0;padding:0;background:#f4f6f9;font-family:Arial,Helvetica,sans-serif;font-size:14px;color:#222;">
<table width="100%" cellpadding="0" cellspacing="0" style="padding:24px 0;">
<tr><td align="center">
<table width="650" cellpadding="0" cellspacing="0" style="background:#fff;border-radius:8px;overflow:hidden;">
  <tr>
    <td style="background:#0056b3;padding:20px 24px;color:#fff;">
      <div style="font-size:20px;font-weight:bold;">Có 1 đăng ký tư vấn mới</div>
      <div style="color:#d8e8ff;margin-top:4px;">Nguồn: Website{(string.IsNullOrEmpty(r.NguonTrang) ? " – capstonevietnam.com/dang-ky-tu-van" : " – form tư vấn nhanh")}</div>
    </td>
  </tr>
  <tr>
    <td style="padding:20px 24px;">
      <p style="margin:0 0 14px;">{status}</p>
      <table width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #eee;border-collapse:collapse;">
        {Row("Mã khách hàng", r.StudentCode)}
        {Row("Họ và tên", r.HoVaTen)}
        {Row("Ngày sinh", r.NgaySinh)}
        {Row("Giới tính", r.GioiTinh)}
        {Row("Điện thoại", r.SoDienThoai)}
        {Row("Email", r.Email)}
        {Row("Tỉnh/Thành", r.Tinh)}
        {(string.IsNullOrEmpty(r.NguonTrang) ? Row("Bậc học mong muốn", r.BacHoc) : "")}
        {(string.IsNullOrEmpty(r.NguonTrang) ? Row("Năm dự định đi", r.NamDi) : "")}
        {(string.IsNullOrEmpty(r.NguonTrang) ? Row("Ngành học dự kiến", r.NganhHoc) : "")}
        {(string.IsNullOrEmpty(r.NguonTrang) ? Row("Khả năng chi trả", r.KhaNangChiTra) : "")}
        {Row("Quốc gia", r.QuocGia)}
        {Row("Tư vấn khác", r.TuVanKhac)}
        {Row("Thời gian đăng ký", r.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))}
      </table>
    </td>
  </tr>
</table>
</td></tr>
</table>
</body>
</html>
""";
    }
}
