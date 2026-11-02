using System.Globalization;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    /// <summary>
    /// BIRTHDAY-MAIL 02/11/2026: nghiệp vụ thư chúc mừng sinh nhật — 1 lượt gửi / portal / ngày, xem trước, gửi thử.
    /// Giờ dùng: giờ Việt Nam (SE Asia Standard Time / Asia/Ho_Chi_Minh), không phụ thuộc múi giờ server.
    /// </summary>
    public class BirthdayMailService
    {
        public const string ApiPath = "/api/BirthdayMail";

        private readonly IBirthdayMailRepository _repo;
        private readonly ISESService _ses;
        private readonly BirthdayMailRenderer _renderer;
        private readonly BirthdayMailSettings _settings;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<BirthdayMailService> _logger;

        public BirthdayMailService(IBirthdayMailRepository repo, ISESService ses, BirthdayMailRenderer renderer,
            IOptions<BirthdayMailSettings> settings, IConfiguration config, IWebHostEnvironment env, ILogger<BirthdayMailService> logger)
        {
            _repo = repo;
            _ses = ses;
            _renderer = renderer;
            _settings = settings.Value;
            _config = config;
            _env = env;
            _logger = logger;
        }

        // ── thời gian VN ────────────────────────────────────────────────────

        private static readonly TimeZoneInfo VnZone = ResolveVn();

        private static TimeZoneInfo ResolveVn()
        {
            foreach (var id in new[] { "SE Asia Standard Time", "Asia/Ho_Chi_Minh", "Asia/Bangkok" })
            {
                try { return TimeZoneInfo.FindSystemTimeZoneById(id); } catch (Exception) { }
            }
            return TimeZoneInfo.CreateCustomTimeZone("UTC+7", TimeSpan.FromHours(7), "UTC+7", "UTC+7");
        }

        public static DateTime VietnamNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VnZone);

        /// <summary>"08:30" -> 08:30; sai định dạng -> 08:00.</summary>
        public static TimeSpan ParseTime(string? hhmm, TimeSpan fallback)
        {
            return TimeSpan.TryParseExact((hhmm ?? string.Empty).Trim(), new[] { @"hh\:mm", @"h\:mm" }, CultureInfo.InvariantCulture, out var t)
                   && t >= TimeSpan.Zero && t < TimeSpan.FromDays(1) ? t : fallback;
        }

        public string TrackingBase
        {
            get
            {
                var b = string.IsNullOrWhiteSpace(_settings.TrackingBaseUrl) ? _config["ApiSelfBaseUrl"] : _settings.TrackingBaseUrl;
                return string.IsNullOrWhiteSpace(b) ? string.Empty : b.Trim().TrimEnd('/') + ApiPath;
            }
        }

        /// <summary>Link tracking chỉ có ý nghĩa khi API là địa chỉ công khai.</summary>
        public bool TrackingIsPublic
        {
            get
            {
                var b = TrackingBase;
                return b.Length > 0 && !b.Contains("localhost", StringComparison.OrdinalIgnoreCase) && !b.Contains("127.0.0.1", StringComparison.Ordinal);
            }
        }

        // ── job ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Tick định kỳ (manual = false): chỉ chạy khi đã tới giờ gửi và chưa quá LatestSendTime.
        /// "Gửi ngay" (manual = true): bỏ qua giờ gửi nhưng vẫn 1 ngày 1 lượt (lượt lỗi thì gửi nốt).
        /// </summary>
        public async Task<BirthdayRunResult> RunAsync(BirthdayMailConfigRow cfg, bool manual, string triggeredBy, CancellationToken ct)
        {
            var now = VietnamNow;
            var today = now.Date;
            var result = new BirthdayRunResult { PortalId = cfg.PortalId };

            if (!manual)
            {
                if (now.TimeOfDay < ParseTime(cfg.SendTime, new TimeSpan(8, 0, 0)))
                    return Done(result, "wait", "Chưa tới giờ gửi " + cfg.SendTime + ".");
                if (!string.IsNullOrWhiteSpace(_settings.LatestSendTime) && now.TimeOfDay > ParseTime(_settings.LatestSendTime, TimeSpan.FromHours(23)))
                    return Done(result, "late", "Đã quá " + _settings.LatestSendTime + " — hôm nay không gửi tự động nữa.");
            }

            var begin = await _repo.BeginRunAsync(cfg.PortalId, today, manual, manual ? triggeredBy : "Hangfire", Math.Max(5, _settings.StaleMinutes), now);
            result.Action = begin.Action;
            if (begin.RunId <= 0)
                return Done(result, begin.Action, begin.Action switch
                {
                    "busy" => "Đang có tiến trình khác gửi thư hôm nay.",
                    "failed" => "Lượt hôm nay bị lỗi — dùng \"Gửi ngay\" trong CRM để gửi nốt.",
                    _ => "Hôm nay đã gửi thư sinh nhật."
                });
            result.RunId = begin.RunId;

            try
            {
                await SendRunAsync(cfg, begin.RunId, today, result, ct);
                await _repo.FinishRunAsync(begin.RunId, "Done", $"Gửi {result.Sent}/{result.Candidates}" + (result.Failed > 0 ? $", lỗi {result.Failed}" : ""), VietnamNow);
                result.Message = $"Đã gửi {result.Sent} thư" + (result.Failed > 0 ? $", lỗi {result.Failed}" : "") + ".";
                _logger.LogInformation("BirthdayMail portal {Portal} run {Run}: sent {Sent}, failed {Failed}, candidates {Cand}",
                    cfg.PortalId, begin.RunId, result.Sent, result.Failed, result.Candidates);
                return result;
            }
            catch (OperationCanceledException)
            {
                // server tắt / Hangfire hủy: để nguyên "Running", tick sau (quá StaleMinutes) gửi tiếp phần còn lại
                _logger.LogWarning("BirthdayMail run {Run} bị hủy giữa chừng — sẽ gửi tiếp ở lần chạy sau.", begin.RunId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BirthdayMail run {Run} lỗi", begin.RunId);
                try { await _repo.FinishRunAsync(begin.RunId, "Failed", "Lỗi: " + ex.Message, VietnamNow); } catch (Exception) { }
                throw;
            }
        }

        private async Task SendRunAsync(BirthdayMailConfigRow cfg, int runId, DateTime today, BirthdayRunResult result, CancellationToken ct)
        {
            var (templateId, templateName, subject, body) = await LoadTemplateAsync(cfg.PortalId, cfg.TemplateId ?? 0, cfg.Subject);
            await _repo.SetRunInfoAsync(runId, templateId, templateName, subject, VietnamNow);

            var site = Site(cfg.HeroImageUrl);
            var items = await _repo.GetActiveItemsAsync(cfg.PortalId, today);
            var tracking = TrackingIsPublic ? TrackingBase : null;
            if (tracking == null)
                _logger.LogWarning("BirthdayMail: TrackingBaseUrl chưa cấu hình địa chỉ công khai -> thư gửi KHÔNG có đếm mở / click.");

            var baseHtml = await _renderer.PrepareBase(body, items, site, today, tracking,
                tracking == null ? null : (url, title, itemId, type) => _repo.UpsertLinkAsync(runId, itemId, type, url, title));

            var done = await _repo.GetRunByDateAsync(cfg.PortalId, today);
            var limit = Math.Max(0, (cfg.MaxPerDay <= 0 ? int.MaxValue : cfg.MaxPerDay) - (done?.Total ?? 0));
            if (limit == 0) { result.Message = "Đã đạt giới hạn thư / ngày."; return; }

            var recipients = await _repo.GetRecipientsAsync(cfg.PortalId, today, runId, cfg.OnlyConsented, limit == int.MaxValue ? 0 : limit);
            result.Candidates = recipients.Count;
            var fromEmail = FirstNonEmpty(cfg.FromEmail, _settings.FromEmail);
            var bcc = string.IsNullOrWhiteSpace(cfg.BccEmail) ? null : cfg.BccEmail.Trim();
            var n = 0;

            foreach (var r in recipients)
            {
                ct.ThrowIfCancellationRequested();
                if (!IsValidEmail(r.Email)) { result.Skipped++; continue; }

                var token = Guid.NewGuid().ToString("N");
                var logId = await _repo.InsertLogAsync(runId, cfg.PortalId, r.StudentId, r.Email, r.FullName, token, VietnamNow);
                if (logId <= 0) { result.Skipped++; continue; }   // đã có trong lượt

                var html = _renderer.Personalize(baseHtml, r, today, site, token, UltilHelper.Encrypt);
                var subj = _renderer.PersonalizeSubject(subject, r, today);
                var error = await SendWithRetryAsync(fromEmail, r.Email, r.FullName, subj, html, bcc, ct);
                await _repo.SetLogResultAsync(logId, error == null ? MailSendStatus.Sent : MailSendStatus.Failed, error ?? string.Empty, VietnamNow);
                if (error == null) result.Sent++; else result.Failed++;

                if (++n % 20 == 0) await _repo.HeartbeatAsync(runId, VietnamNow);
                if (_settings.SendThrottleMs > 0) await Task.Delay(_settings.SendThrottleMs, ct);
            }
        }

        private async Task<string?> SendWithRetryAsync(string fromEmail, string to, string toName, string subject, string html, string? bcc, CancellationToken ct)
        {
            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    await _ses.SendBodyEmailAsync(fromEmail, to, toName, subject, html, bcc);
                    return null;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    if (attempt >= 2 || IsPermanent(ex))
                    {
                        _logger.LogWarning(ex, "BirthdayMail gửi lỗi tới {Email}", to);
                        return ex.GetBaseException().Message;
                    }
                    await Task.Delay(Math.Max(500, _settings.RetryDelayMs), ct);
                }
            }
        }

        /// <summary>Lỗi địa chỉ (5xx mailbox) không thử lại.</summary>
        private static bool IsPermanent(Exception ex) =>
            ex is FormatException || ex is SmtpFailedRecipientException ||
            (ex is SmtpException se && ((int)se.StatusCode >= 550 && (int)se.StatusCode <= 554));

        // ── xem trước / gửi thử (CRM gọi qua JWT) ──────────────────────────

        public async Task<(string Subject, string Html, int Items)> PreviewAsync(BirthdayPreviewRequest req)
        {
            var cfg = await _repo.GetConfigAsync(req.PortalId) ?? new BirthdayMailConfigRow { PortalId = req.PortalId };
            var today = VietnamNow.Date;
            var (_, _, subject, body) = await ResolveForEditorAsync(req, cfg);
            var items = await _repo.GetActiveItemsAsync(req.PortalId, today);
            var site = Site(cfg.HeroImageUrl);
            var baseHtml = await _renderer.PrepareBase(body, items, site, today, null, null);
            var r = BirthdayMailRenderer.SampleRecipient(req.FullName, "khachhang@example.com", today);
            return (_renderer.PersonalizeSubject(subject, r, today), _renderer.Personalize(baseHtml, r, today, site, null, UltilHelper.Encrypt), items.Count);
        }

        /// <summary>Gửi thử: link có gắn tracking (lượt 0) để kiểm tra chuyển hướng, nhưng KHÔNG tính vào thống kê.</summary>
        public async Task<string> SendTestAsync(BirthdayTestRequest req)
        {
            if (!IsValidEmail(req.Email)) throw new ArgumentException("Email nhận thử không hợp lệ.");
            var cfg = await _repo.GetConfigAsync(req.PortalId) ?? new BirthdayMailConfigRow { PortalId = req.PortalId };
            var today = VietnamNow.Date;
            var (_, _, subject, body) = await ResolveForEditorAsync(req, cfg);
            var items = await _repo.GetActiveItemsAsync(req.PortalId, today);
            var site = Site(cfg.HeroImageUrl);
            var tracking = TrackingIsPublic ? TrackingBase : null;
            var baseHtml = await _renderer.PrepareBase(body, items, site, today, tracking,
                tracking == null ? null : (url, title, itemId, type) => _repo.UpsertLinkAsync(0, itemId, type, url, title));
            var r = BirthdayMailRenderer.SampleRecipient(req.FullName, req.Email.Trim(), today);
            var html = _renderer.Personalize(baseHtml, r, today, site, Guid.NewGuid().ToString("N"), UltilHelper.Encrypt);
            var subj = "[THỬ] " + _renderer.PersonalizeSubject(subject, r, today);
            var error = await SendWithRetryAsync(FirstNonEmpty(cfg.FromEmail, _settings.FromEmail), req.Email.Trim(), r.FullName, subj, html, null, CancellationToken.None);
            if (error != null) throw new InvalidOperationException("Gửi thử lỗi: " + error);
            _logger.LogInformation("BirthdayMail gửi thử tới {Email} bởi {By}", req.Email, req.RequestedBy);
            return subj;
        }

        public async Task<(string Subject, string Html)> GetDefaultTemplateAsync()
        {
            var html = await ReadDefaultAsync();
            return (_settings.DefaultSubject, html);
        }

        // ── nạp mẫu ─────────────────────────────────────────────────────────

        private async Task<(int Id, string Name, string Subject, string Body)> ResolveForEditorAsync(BirthdayPreviewRequest req, BirthdayMailConfigRow cfg)
        {
            if (!string.IsNullOrWhiteSpace(req.BodyHtml))
                return (0, "Bản đang soạn", FirstNonEmpty(req.Subject, cfg.Subject, _settings.DefaultSubject), req.BodyHtml!);
            if (req.TemplateId > 0)
            {
                var t = await _repo.GetTemplateAsync(req.PortalId, req.TemplateId);
                if (t == null) throw new ArgumentException("Không tìm thấy mẫu thư hoặc mẫu đang tắt.");
                return (t.Id, t.Name, FirstNonEmpty(req.Subject, t.Subject, cfg.Subject, _settings.DefaultSubject), t.BodyHtml);
            }
            return await LoadTemplateAsync(req.PortalId, 0, FirstNonEmpty(req.Subject, cfg.Subject));
        }

        /// <summary>Mẫu đang dùng của Marketing; không có / bị xóa / trống -> thiệp mặc định.</summary>
        private async Task<(int Id, string Name, string Subject, string Body)> LoadTemplateAsync(int portalId, int templateId, string? configSubject)
        {
            var t = templateId > 0 ? await _repo.GetTemplateAsync(portalId, templateId) : null;
            if (t != null && !string.IsNullOrWhiteSpace(t.BodyHtml))
                return (t.Id, t.Name, FirstNonEmpty(t.Subject, configSubject, _settings.DefaultSubject), t.BodyHtml);
            return (0, "Thiệp mặc định", FirstNonEmpty(configSubject, _settings.DefaultSubject), await ReadDefaultAsync());
        }

        private async Task<string> ReadDefaultAsync()
        {
            var path = _settings.DefaultTemplatePath;
            var full = Path.IsPathRooted(path) ? path : Path.Combine(_env.ContentRootPath, path);
            if (!File.Exists(full))
            {
                var alt = Path.Combine(AppContext.BaseDirectory, path);
                if (File.Exists(alt)) full = alt;
                else throw new FileNotFoundException("Không thấy thiệp mặc định: " + path);
            }
            return await File.ReadAllTextAsync(full);
        }

        private BirthdayMailRenderer.SiteInfo Site(string heroUrl)
        {
            string C(string key) => (_config["CapstoneInfo:" + key] ?? string.Empty).Trim();
            var website = FirstNonEmpty(C("Website"), "www.capstonevietnam.com");
            var unsubHtml = (_config["Marketing:MailUnsubContent"] ?? string.Empty).Trim();
            return new BirthdayMailRenderer.SiteInfo
            {
                WebsiteUrl = FirstNonEmpty(_settings.WebsiteUrl, "https://" + website.Replace("https://", "").Replace("http://", "")),
                Website = website,
                LogoUrl = _settings.LogoUrl,
                Hotline = C("Hotline"),
                ContactEmail = C("Email"),
                Facebook = C("Facebook"),
                HaNoiAddress = StripLabel(C("HaNoiDiaChi")),
                HaNoiPhone = C("HaNoiPhone"),
                HCMAddress = StripLabel(FirstNonEmpty(C("HCMDiaChi"), C("HCMiaChi"))),
                HCMPhone = C("HCMPhone"),
                HeroImageUrl = heroUrl ?? string.Empty,
                UnsubscribeHtml = unsubHtml,
                UnsubscribeUrl = _settings.UnsubscribeUrl
            };

            // "Hà Nội: 2 Lê Quý Đôn" -> "2 Lê Quý Đôn" (tiêu đề văn phòng đã có trong mẫu)
            static string StripLabel(string s)
            {
                var i = s.IndexOf(':');
                return i > 0 && i < 20 ? s[(i + 1)..].Trim() : s;
            }
        }

        // ── tiện ích ────────────────────────────────────────────────────────

        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > 256) return false;
            try
            {
                var a = new MailAddress(email.Trim());
                return string.Equals(a.Address, email.Trim(), StringComparison.OrdinalIgnoreCase) && a.Host.Contains('.');
            }
            catch (FormatException) { return false; }
        }

        private static string FirstNonEmpty(params string?[] values) =>
            values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v))?.Trim() ?? string.Empty;

        private static BirthdayRunResult Done(BirthdayRunResult r, string action, string message)
        {
            r.Action = action;
            r.Message = message;
            return r;
        }
    }
}
