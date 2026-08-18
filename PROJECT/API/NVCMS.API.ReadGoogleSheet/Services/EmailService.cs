using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Models.Config;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
            _logger = logger;
        }

        public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body, string? ccEmail = null, string? bccEmail = null)
        {
            await SendEmailAsync(
                string.IsNullOrWhiteSpace(fromEmail) ? _smtpSettings.DefaultFromEmail : fromEmail,
                _smtpSettings.DefaultFromName,
                toEmail,
                subject,
                body,
                ccEmail,
                bccEmail);
        }

        public async Task SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string body, string? ccEmail = null, string? bccEmail = null)
        {
            try
            {
                _logger.LogInformation("Sending email to {ToEmail} with subject: {Subject}", toEmail, subject);

                await UltilHelper.SendMailAsync(
                    _smtpSettings.Host,
                    _smtpSettings.Port,
                    _smtpSettings.EnableSsl,
                    _smtpSettings.Username,
                    _smtpSettings.Password,
                    string.IsNullOrWhiteSpace(fromEmail) ? _smtpSettings.DefaultFromEmail : fromEmail,
                    toEmail,
                    ccEmail,
                    bccEmail,
                    subject,
                    body,
                    isBodyHtml: true,
                    fromName: fromName);

                _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                throw;
            }
        }
    }
}