using System.Data;
using System.Net;
using System.Net.Mail;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public sealed class TuVanFormService : ITuVanFormService
{
    private readonly string _connectionString;
    private readonly IConfiguration _config;

    public TuVanFormService(string connectionString, IConfiguration config)
    {
        _connectionString = connectionString;
        _config = config;
    }

    public async Task SubmitAsync(TuVanFormInputViewModel input, int portalId, CancellationToken ct = default)
    {
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

        var subject = "Capstone Vietnam - Đã tiếp nhận đăng ký tư vấn";
        var body = $"""
                    <p>Xin chào {WebUtility.HtmlEncode(input.HoVaTen)},</p>
                    <p>Capstone đã tiếp nhận đăng ký tư vấn của bạn.</p>
                    <p>Hình thức: {WebUtility.HtmlEncode(input.HinhThuc)}<br/>
                       Văn phòng: {WebUtility.HtmlEncode(input.VanPhong)}<br/>
                       SĐT: {WebUtility.HtmlEncode(input.SoDienThoai)}<br/>
                       Email: {WebUtility.HtmlEncode(input.Email)}</p>
                    <p>Trạng thái: <b>Vừa tiếp nhận</b></p>
                    """;

        // Nếu có email -> gửi cho khách
        if (!string.IsNullOrWhiteSpace(input.Email))
            await SendMailAsync(input.Email!, subject, body, ct);

        // Luôn gửi nội bộ
        await SendMailAsync("info@capstonevietnam.com", "[TƯ VẤN] Đăng ký mới từ trang chủ", body, ct);
    }

    private async Task SendMailAsync(string to, string subject, string htmlBody, CancellationToken ct)
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
}