using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class SESService : ISESService
    {
        private readonly SmtpSettings                 _smtpSettings;
        private readonly SesSettings                  _sesSettings;
        private readonly IMarketingListMailRepository _listMailRepo;
        private readonly IWebHostEnvironment          _env;
        private readonly ILogger<SESService>          _logger;

        public SESService(
            IOptions<SmtpSettings>          smtpSettings,
            IOptions<SesSettings>           sesSettings,
            IMarketingListMailRepository     listMailRepo,
            IWebHostEnvironment              env,
            ILogger<SESService>              logger)
        {
            _smtpSettings = smtpSettings.Value;
            _sesSettings  = sesSettings.Value;
            _listMailRepo = listMailRepo;
            _env          = env;
            _logger       = logger;
        }

        // ── SendBodyEmailAsync (NEW – body-only, no template) ─────────────────
        public async Task<string> SendBodyEmailAsync(
            string toEmail,
            string toName,
            string subject,
            string htmlBody)
        {
            var messageId = Guid.NewGuid().ToString("N");

            await UltilHelper.SendMailAsync(
                _smtpSettings.Host,
                _smtpSettings.Port,
                _smtpSettings.EnableSsl,
                _smtpSettings.Username,
                _smtpSettings.Password,
                _sesSettings.FromEmail,
                toEmail,
                ccEmail:  null,
                bccEmail: null,
                subject,
                htmlBody,
                isBodyHtml: true,
                fromName: _sesSettings.FromName);

            _logger.LogInformation("SMTP sent to {Email} | MessageId={MessageId}", toEmail, messageId);
            return messageId;
        }

        // ── SendTemplatedEmailAsync ───────────────────────────────────────────
        public async Task<string> SendTemplatedEmailAsync(
            Marketing_Mail_Template      template,
            string                     toEmail,
            string                     toName,
            Dictionary<string, string> placeholders)
        {
            var htmlBody = await LoadAndRenderTemplateAsync(template, placeholders);
            var subject  = ReplacePlaceholders(template.TemplateName ?? "(no subject)", placeholders);

            return await SendBodyEmailAsync(toEmail, toName, subject, htmlBody);
        }

        // ── SendToRecipientAsync ──────────────────────────────────────────────
        public async Task SendToRecipientAsync(
            Marketing_Mail_Template      template,
            Marketing_Mail_ListMail      recipient,
            Dictionary<string, string> extraPlaceholders)
        {
            // Merge built-in placeholders với extra
            var placeholders = BuildRecipientPlaceholders(recipient);
            foreach (var kv in extraPlaceholders)
                placeholders[kv.Key] = kv.Value;

            string messageId;
            try
            {
                messageId = await SendTemplatedEmailAsync(
                    template,
                    recipient.Email ?? throw new InvalidOperationException("Recipient email is null"),
                    string.Empty,
                    placeholders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP failed for recipient {Id} <{Email}>",
                    recipient.id, recipient.Email);
                throw;
            }

            _logger.LogInformation("SMTP sent to recipient {Id} <{Email}> messageId={MsgId}",
                recipient.id, recipient.Email, messageId);
        }

        // ── Private helpers ───────────────────────────────────────────────────

        /// <summary>Đọc file HTML từ FilePath và thay thế placeholder.</summary>
        private async Task<string> LoadAndRenderTemplateAsync(
            Marketing_Mail_Template      template,
            Dictionary<string, string> placeholders)
        {
            if (string.IsNullOrWhiteSpace(template.FilePath))
                throw new InvalidOperationException(
                    $"Template {template.Id} '{template.TemplateName}' has no FilePath");

            // Resolve đường dẫn: nếu absolute thì dùng luôn, ngược lại ghép với wwwroot/TemplateBasePath
            string fullPath;
            if (Path.IsPathRooted(template.FilePath))
            {
                fullPath = template.FilePath;
            }
            else
            {
                fullPath = Path.Combine(
                    _env.WebRootPath,
                    _sesSettings.TemplateBasePath.TrimEnd('/', '\\'),
                    template.FilePath.TrimStart('/', '\\'));
            }

            if (!File.Exists(fullPath))
                throw new FileNotFoundException(
                    $"Email template file not found: {fullPath}", fullPath);

            var html = await File.ReadAllTextAsync(fullPath);
            return ReplacePlaceholders(html, placeholders);
        }

        /// <summary>Thay thế {{Key}} bằng value tương ứng.</summary>
        private static string ReplacePlaceholders(
            string template, Dictionary<string, string> placeholders)
        {
            foreach (var (key, value) in placeholders)
                template = template.Replace($"{{{{{key}}}}}", value,
                    StringComparison.OrdinalIgnoreCase);
            return template;
        }

        /// <summary>Placeholder mặc định từ thông tin recipient.</summary>
        private static Dictionary<string, string> BuildRecipientPlaceholders(
            Marketing_Mail_ListMail recipient) => new()
        {
            ["FullName"]   = string.Empty,
            ["Email"]      = recipient.Email ?? string.Empty,
            ["FirstName"]  = string.Empty,
        };


    }
}
