using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace NVCMS.WebView.Data.Service;

public sealed class TuVanFormService : ITuVanFormService
{
    private readonly string _connectionString;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpFactory;
    private readonly ILogger<TuVanFormService> _logger;
    public TuVanFormService(string connectionString, IConfiguration config, IHttpClientFactory httpFactory,
        ILogger<TuVanFormService> logger)
    {
        _connectionString = connectionString;
        _config = config;
        _httpFactory = httpFactory;
        _logger = logger;
    }

    public async Task SubmitAsync(TuVanFormInputViewModel input, int portalId, CancellationToken ct = default)
    {
        // Verify reCAPTCHA
        if (!string.IsNullOrWhiteSpace(input.RecaptchaToken))
        {
            var isValid = await VerifyRecaptchaAsync(input.RecaptchaToken);
            if (!isValid)
                throw new InvalidOperationException("Xác thực reCAPTCHA không thành công. Vui lòng thử lại.");
        }

        // Save database
        await using (var conn = new SqlConnection(_connectionString))
        {
            await conn.ExecuteAsync(
                "NVCMS_Form_Insert",
                new
                {
                    Type = "TUVAN",
                    hinhthuc = input.HinhThuc,
                    vanphong = input.VanPhong,
                    title = "Đăng ký tư vấn từ trang chủ",
                    noidung = input.NoiDung,
                    hovaten = input.HoVaTen,
                    email = input.Email,
                    sodienthoai = input.SoDienThoai,
                    diachi = (string?)null,
                    status = "VUATIEPNHAN",
                    creatdate = DateTime.Now,
                    portalid = portalId
                },
                commandType: CommandType.StoredProcedure);
        }

        // ==========================
        // Mail khách hàng
        // ==========================

        var customerSubject = "Capstone Vietnam | Xác nhận tiếp nhận đăng ký tư vấn";

        var customerBody = $"""
<!DOCTYPE html>
<html lang="vi">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>

<body style="margin:0;padding:0;background:#f4f6f9;font-family:Arial,Helvetica,sans-serif;">

<table width="100%" cellpadding="0" cellspacing="0" style="padding:30px 0;">
<tr>
<td align="center">

<table width="650" cellpadding="0" cellspacing="0"
style="background:#fff;border-radius:10px;overflow:hidden;">

<tr>
<td style="background:#0056b3;padding:25px;text-align:center;">
<h1 style="color:#fff;margin:0;">Capstone Vietnam</h1>
<div style="color:#d8e8ff;">Xác nhận đăng ký tư vấn</div>
</td>
</tr>

<tr>
<td style="padding:35px;">

<p>Xin chào <strong>{WebUtility.HtmlEncode(input.HoVaTen)}</strong>,</p>

<p>
Cảm ơn bạn đã đăng ký tư vấn tại
<b>Capstone Vietnam</b>.
</p>

<p>
Chúng tôi đã tiếp nhận thông tin và sẽ liên hệ với bạn trong thời gian sớm nhất.
</p>

<table width="100%" cellpadding="10" cellspacing="0"
style="border:1px solid #ddd;">

<tr>
<td width="180"><b>Hình thức</b></td>
<td>{WebUtility.HtmlEncode(input.HinhThuc)}</td>
</tr>

<tr>
<td><b>Văn phòng</b></td>
<td>{WebUtility.HtmlEncode(input.VanPhong)}</td>
</tr>

<tr>
<td><b>Số điện thoại</b></td>
<td>{WebUtility.HtmlEncode(input.SoDienThoai)}</td>
</tr>

<tr>
<td><b>Email</b></td>
<td>{WebUtility.HtmlEncode(input.Email)}</td>
</tr>

<tr>
<td><b>Trạng thái</b></td>
<td>
<span style="background:#28a745;color:white;padding:5px 12px;border-radius:15px;">
Đã tiếp nhận
</span>
</td>
</tr>

</table>

<p style="margin-top:25px;">
Trân trọng,<br/>
<b>Capstone Vietnam</b>
</p>

</td>
</tr>

<tr>
<td style="background:#f7f7f7;padding:20px;text-align:center;font-size:13px;color:#666;">
Website:
<a href="https://capstonevietnam.com">
https://capstonevietnam.com
</a>

<br/>

Email:
<a href="mailto:info@capstonevietnam.com">
info@capstonevietnam.com
</a>

</td>
</tr>

</table>

</td>
</tr>
</table>

</body>
</html>
""";

        // ==========================
        // Mail Admin
        // ==========================

        var adminSubject = $"[TƯ VẤN] {input.HoVaTen} - {input.SoDienThoai}";

        var adminBody = $"""
<!DOCTYPE html>
<html>
<body style="font-family:Arial;">

<h2>Có đăng ký tư vấn mới</h2>

<table border="1" cellpadding="8" cellspacing="0"
style="border-collapse:collapse;">

<tr>
<td><b>Họ tên</b></td>
<td>{WebUtility.HtmlEncode(input.HoVaTen)}</td>
</tr>

<tr>
<td><b>Điện thoại</b></td>
<td>{WebUtility.HtmlEncode(input.SoDienThoai)}</td>
</tr>

<tr>
<td><b>Email</b></td>
<td>{WebUtility.HtmlEncode(input.Email)}</td>
</tr>

<tr>
<td><b>Hình thức</b></td>
<td>{WebUtility.HtmlEncode(input.HinhThuc)}</td>
</tr>

<tr>
<td><b>Văn phòng</b></td>
<td>{WebUtility.HtmlEncode(input.VanPhong)}</td>
</tr>

<tr>
<td><b>Nội dung</b></td>
<td>{WebUtility.HtmlEncode(input.NoiDung)}</td>
</tr>

<tr>
<td><b>Ngày đăng ký</b></td>
<td>{DateTime.Now:dd/MM/yyyy HH:mm:ss}</td>
</tr>

</table>

</body>
</html>
""";

        // Gửi mail (không làm fail đăng ký nếu gửi lỗi)

        if (!string.IsNullOrWhiteSpace(input.Email))
        {
            await TrySendMailAsync(
                input.Email,
                customerSubject,
                customerBody,
                ct);
        }

        await TrySendMailAsync(
            "info@capstonevietnam.com",
            adminSubject,
            adminBody,
            ct);
    }

    private async Task TrySendMailAsync(
        string to,
        string subject,
        string body,
        CancellationToken ct)
    {
        try
        {
            await SendMailAsync(to, subject, body, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to send email to {Email}",
                to);
        }
    }

    private async Task<bool> VerifyRecaptchaAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;

        var secretKey = _config["Google:recaptchav3_secretkey"];
        if (string.IsNullOrWhiteSpace(secretKey)) return true;

        try
        {
            var client = _httpFactory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", secretKey),
                new KeyValuePair<string, string>("response", token)
            });

            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var success = doc.RootElement.TryGetProperty("success", out var s) && s.GetBoolean();
            var score = doc.RootElement.TryGetProperty("score", out var sc) ? sc.GetDouble() : 0.5;
            return success && score >= 0.5;
        }
        catch
        {
            return true;
        }
    }

    private async Task SendMailAsync(string to, string subject, string htmlBody, CancellationToken ct)
    {
        try
        {
            var host = _config["Email:Host"]!;
            var port = int.Parse(_config["Email:Port"] ?? "587");
            var enableSsl = bool.Parse(_config["Email:EnableSsl"] ?? "true");
            var from = _config["Email:FromEmailAddress"]!;
            var displayName = _config["Email:DisplayName"] ?? "Capstone Vietnam";
            var username = _config["Email:UserMail"]!;
            var password = _config["Email:Password"]!;

            using var msg = new MailMessage
            {
                From = new MailAddress(from, displayName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            msg.To.Add(to);

            using var smtp = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                Credentials = new NetworkCredential(username, password)
            };

            ct.ThrowIfCancellationRequested();
            await smtp.SendMailAsync(msg);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

}