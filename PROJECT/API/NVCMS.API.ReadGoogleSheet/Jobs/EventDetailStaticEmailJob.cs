using System.Net;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    /// <summary>
    /// Hangfire job gửi mail xác nhận tham dự sự kiện (màn hình thống kê sự kiện).
    /// Khác CampaignBatchJob: body render riêng cho từng học sinh từ EmailTemplates/event-register-succes.html
    /// (tên, mã QR check-in...). Chỉ gửi các send-log còn Queued → retry không gửi trùng.
    /// </summary>
    public class EventDetailStaticEmailJob
    {
        public const string TemplateName = "event-register-succes.html";

        private const int BatchSize = 100;
        private static readonly TimeSpan BatchDelay = TimeSpan.FromSeconds(2);

        /// <summary>href tạm của nút "Xem chi tiết", thay bằng link click-tracking theo từng Send_Log.</summary>
        private const string ShortlinkHrefToken = "__TRACKED_SHORTLINK_HREF__";

        private const string QrHandlerBase  = "https://crm.capstonevietnam.com/Services/QrcodeHandler.ashx";
        private const string BarHandlerBase = "https://crm.capstonevietnam.com/Services/BarcodeHandler.ashx";

        private readonly CRMDbContext                       _crmDb;
        private readonly IMarketingSendLogRepository        _sendLogRepo;
        private readonly IMailAccountRepository             _mailAccountRepo;
        private readonly ISESService                        _sesService;
        private readonly EmailTemplateRenderer              _renderer;
        private readonly IConfiguration                     _config;
        private readonly ILogger<EventDetailStaticEmailJob> _logger;

        public EventDetailStaticEmailJob(
            CRMDbContext                       crmDb,
            IMarketingSendLogRepository        sendLogRepo,
            IMailAccountRepository             mailAccountRepo,
            ISESService                        sesService,
            EmailTemplateRenderer              renderer,
            IConfiguration                     config,
            ILogger<EventDetailStaticEmailJob> logger)
        {
            _crmDb           = crmDb;
            _sendLogRepo     = sendLogRepo;
            _mailAccountRepo = mailAccountRepo;
            _sesService      = sesService;
            _renderer        = renderer;
            _config          = config;
            _logger          = logger;
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 600 })]
        public async Task ExecuteAsync(
            int campaignSendId,
            int eventId,
            int eventCatId,
            string subject,
            int emailAccountId,
            List<EventDetailStaticRecipient> recipients,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("EventDetailStaticEmailJob: start campaignSendId={CampaignSendId}, eventId={EventId}, recipients={Count}",
                campaignSendId, eventId, recipients.Count);

            var eventRow = await _crmDb.NV_Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eventId, cancellationToken);
            var catRow   = await _crmDb.NV_EventsCats.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eventCatId, cancellationToken);
            if (eventRow is null || catRow is null)
            {
                _logger.LogWarning("EventDetailStaticEmailJob: event {EventId} / cat {EventCatId} not found", eventId, eventCatId);
                return;
            }

            var mailAccount = emailAccountId > 0 ? await _mailAccountRepo.GetByIdAsync(emailAccountId) : null;
            var fromEmail   = mailAccount?.Mail ?? string.Empty;   // rỗng → SESService dùng SesSettings.FromEmail
            var apiBaseUrl  = (_config["ApiSelfBaseUrl"] ?? string.Empty).TrimEnd('/');
            var bcc         = _config["Marketing:SendBCC"];   // nhiều địa chỉ: phân cách , hoặc ;

            // Chỉ các log còn Queued (lần retry bỏ qua log đã Sent/Failed)
            var byLogId    = recipients.ToDictionary(x => x.LogId);
            var queuedLogs = (await _sendLogRepo.GetQueuedByCampaignIdAsync(campaignSendId))
                .Where(l => byLogId.ContainsKey(l.Id))
                .ToList();

            var studentIds = queuedLogs.Select(l => byLogId[l.Id].StudentId).Distinct().ToList();
            var students = await _crmDb.StudentInfos
                .AsNoTracking()
                .Where(x => studentIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            var eventValues = BuildEventValues(eventRow, catRow, out var shortlink);

            int totalSent = 0, totalFailed = 0;
            for (int i = 0; i < queuedLogs.Count; i += BatchSize)
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (var log in queuedLogs.Skip(i).Take(BatchSize))
                {
                    try
                    {
                        if (!students.TryGetValue(byLogId[log.Id].StudentId, out var student))
                            throw new InvalidOperationException($"Student_Info {byLogId[log.Id].StudentId} not found");

                        var body = RenderBody(eventValues, student);

                        // Click nút "Xem chi tiết" → /api/EmailTracking/click ghi ClickedTime rồi redirect tới shortlink
                        if (shortlink is not null)
                        {
                            var trackedHref = $"{apiBaseUrl}/api/EmailTracking/click?id={log.Id}&url={Uri.EscapeDataString(shortlink)}";
                            body = body.Replace(ShortlinkHrefToken, H(trackedHref), StringComparison.Ordinal);
                        }

                        // Pixel 1x1 → /api/EmailTracking/open ghi OpenedTime (đã đọc mail).
                        // Không dùng display:none: một số mail client bỏ qua ảnh ẩn → không ghi nhận được lượt mở.
                        var trackingPixel = $"<img src=\"{apiBaseUrl}/api/EmailTracking/open?id={log.Id}\" width=\"1\" height=\"1\" alt=\"\" border=\"0\" style=\"display:block; width:1px; height:1px; border:0; margin:0; padding:0;\">";
                        var unsub = (_config["Marketing:MailUnsubContent"] ?? string.Empty)
                            .Replace("{unsubmail}", UltilHelper.Encrypt(log.Email ?? string.Empty), StringComparison.OrdinalIgnoreCase);
                        body = InjectBeforeBodyClose(body, unsub + trackingPixel);

                        var toName = $"{student.Hotendem} {student.Ten}".Trim();
                        var sesMessageId = await _sesService.SendBodyEmailAsync(fromEmail, log.Email!, toName, subject, body, bcc);
                        await _sendLogRepo.UpdateStatusAsync(log.Id, MailSendStatus.Sent, sesMessageId: sesMessageId);
                        totalSent++;
                    }
                    catch (Exception ex)
                    {
                        await _sendLogRepo.UpdateStatusAsync(log.Id, MailSendStatus.Failed, errorMessage: ex.Message);
                        totalFailed++;
                        _logger.LogError(ex, "EventDetailStaticEmailJob: failed {Email} logId={LogId}", log.Email, log.Id);
                    }
                }

                if (i + BatchSize < queuedLogs.Count)
                    await Task.Delay(BatchDelay, cancellationToken);
            }

            _logger.LogInformation("EventDetailStaticEmailJob: done campaignSendId={CampaignSendId} sent={Sent} failed={Failed}",
                campaignSendId, totalSent, totalFailed);
        }

        /// <summary>Các giá trị chung cho mọi học sinh (lấy 1 lần).</summary>
        private Dictionary<string, string> BuildEventValues(NV_Event eventRow, NV_Events_Cat catRow, out string? shortlink)
        {
            var eventName = eventRow.Title ?? catRow.CatName ?? string.Empty;

            var eventTime = catRow.FromDate?.ToString("HH:mm dd/MM/yyyy") ?? catRow.DateShow ?? string.Empty;
            if (catRow.FromDate.HasValue && catRow.EndDate.HasValue)
                eventTime += " - đến: " + catRow.EndDate.Value.ToString("HH:mm dd/MM/yyyy");

            shortlink = string.IsNullOrWhiteSpace(catRow.Link_pr) ? null : catRow.Link_pr.Trim();
            if (shortlink is not null && !shortlink.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                shortlink = "https://" + shortlink;
            var shortlinkSection = shortlink is null
                ? string.Empty
                : $"""
                    <table role="presentation" cellpadding="0" cellspacing="0" border="0" align="center" style="margin-top:20px;">
                        <tr>
                            <td align="center" style="border-radius:6px; background-color:#c8102e;">
                                <a href="{ShortlinkHrefToken}" target="_blank" style="display:inline-block; padding:13px 30px; font-size:15px; font-weight:700; color:#ffffff; text-decoration:none; border-radius:6px;">Xem chi tiết sự kiện &rarr;</a>
                            </td>
                        </tr>
                    </table>
                    """;

            // ContentMail là HTML soạn sẵn trong CRM → không encode; không có thì dùng sendzalo_content
            var content = !string.IsNullOrWhiteSpace(catRow.ContentMail)
                ? catRow.ContentMail!
                : H(catRow.sendzalo_content);
            var contentSection = string.IsNullOrWhiteSpace(content)
                ? string.Empty
                : $"""
                    <tr>
                        <td class="px" style="padding:24px 32px 0;">
                            <h2 style="margin:0 0 14px; font-size:13px; font-weight:700; letter-spacing:1.2px; text-transform:uppercase; color:#0b5cad;">Lưu ý quan trọng</h2>
                            <div style="padding:16px 18px; background-color:#fffbea; border-left:4px solid #f5a623; border-radius:6px; font-size:14px; line-height:22px; color:#3d3d3d;">
                                {content}
                            </div>
                        </td>
                    </tr>
                    """;

            string Info(string key) => H(_config[$"CapstoneInfo:{key}"]);

            return new Dictionary<string, string>
            {
                ["TITLE_MAIL"]        = H(catRow.TitleMail ?? catRow.CatName),
                ["EVENT_NAME"]        = H(eventName),
                ["EVENT_LOCATION"]    = H(eventRow.Diadiem ?? eventName),
                ["EVENT_CAT_NAME"]    = H(catRow.CatName),
                ["EVENT_TIME"]        = H(eventTime),
                ["SHORTLINK_SECTION"] = shortlinkSection,
                ["CONTENT_SECTION"]   = contentSection,
                // Thông tin công ty: appsettings CapstoneInfo
                ["HOTLINE"]                = Info("Hotline"),
                ["CAPSTONE_EMAIL"]         = Info("Email"),
                ["CAPSTONE_WEBSITE"]       = Info("Website"),
                ["CAPSTONE_FACEBOOK"]      = Info("Facebook"),
                ["CAPSTONE_HANOI_ADDRESS"] = Info("HaNoiDiaChi"),
                ["CAPSTONE_HANOI_PHONE"]   = Info("HaNoiPhone"),
                // appsettings đang đặt tên key "HCMiaChi" (thiếu D) → đọc cả 2
                ["CAPSTONE_HCM_ADDRESS"]   = H(_config["CapstoneInfo:HCMDiaChi"] ?? _config["CapstoneInfo:HCMiaChi"]),
                ["CAPSTONE_HCM_PHONE"]     = Info("HCMPhone")
            };
        }

        private string RenderBody(Dictionary<string, string> eventValues, Student_Info student)
        {
            var code = student.Code?.Trim();
            var values = new Dictionary<string, string>(eventValues)
            {
                ["STUDENT_FULLNAME"] = OrDash($"{student.Hotendem} {student.Ten}"),
                ["STUDENT_CODE"]     = OrDash(code),
                ["STUDENT_PHONE"]    = OrDash(student.Sodienthoai),
                ["STUDENT_EMAIL"]    = OrDash(student.Email),
                ["QR_SECTION"]       = BuildQrSection(code)
            };
            return _renderer.Render(TemplateName, values);
        }

        /// <summary>QR = link check-in theo mã học sinh (giống template ZNS 627608) + barcode mã học sinh.</summary>
        private static string BuildQrSection(string? studentCode)
        {
            if (string.IsNullOrWhiteSpace(studentCode))
                return string.Empty;

            var checkinUrl = $"http://crm.capstonevietnam.com/quantri/partner/checkin-eventm.html?studentcode={Uri.EscapeDataString(studentCode)}";
            var qrUrl  = $"{QrHandlerBase}?data={Uri.EscapeDataString(checkinUrl)}&width=200&height=200";
            var barUrl = $"{BarHandlerBase}?data={Uri.EscapeDataString(studentCode)}&type=barcode&width=400&height=100";

            return $"""
                <tr>
                    <td class="px" style="padding:24px 32px 0;">
                        <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="border:2px dashed #0b5cad; border-radius:10px; background-color:#f3f8fe;">
                            <tr>
                                <td align="center" style="padding:22px 18px; font-size:14px; color:#1f2d3d;">
                                    <div style="font-size:13px; font-weight:700; letter-spacing:1.2px; text-transform:uppercase; color:#0b5cad; margin-bottom:6px;">Mã check-in của bạn</div>
                                    <div style="font-size:13px; color:#5b6b7f; margin-bottom:14px;">Vui lòng xuất trình mã này tại quầy lễ tân khi đến sự kiện</div>
                                    <img src="{qrUrl}" width="170" height="170" alt="QR check-in" style="display:block; margin:0 auto; background-color:#ffffff; padding:8px; border:1px solid #d7e3f1; border-radius:6px;">
                                    <img src="{barUrl}" width="260" height="65" alt="Barcode" style="display:block; margin:14px auto 0;">
                                    <div style="margin-top:10px; font-size:13px; color:#5b6b7f;">Mã đăng ký: <strong style="font-size:16px; color:#c8102e; letter-spacing:1px;">{H(studentCode)}</strong></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                """;
        }

        private static string InjectBeforeBodyClose(string body, string html)
        {
            const string closeBody = "</body>";
            var idx = body.LastIndexOf(closeBody, StringComparison.OrdinalIgnoreCase);
            return idx >= 0 ? body.Insert(idx, html) : body + html;
        }

        private static string H(string? s) => WebUtility.HtmlEncode(s ?? string.Empty);

        private static string OrDash(string? s) => string.IsNullOrWhiteSpace(s) ? "—" : H(s.Trim());
    }
}
