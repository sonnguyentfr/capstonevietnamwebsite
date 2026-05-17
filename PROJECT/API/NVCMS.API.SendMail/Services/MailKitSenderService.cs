using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Domain;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Services
{
    public class MailKitSenderService : IMailSenderService
    {
        private static readonly HashSet<string> RetryPfx   = new HashSet<string> { "421","450","451" };
        private static readonly HashSet<string> PermFailPfx = new HashSet<string> { "550","551","552","553","554" };
        public async Task<MailSendResult> SendAsync(OutgoingMailMessage msg, CancellationToken ct)
        {
            try
            {
                var mime = new MimeMessage();
                mime.From.Add(new MailboxAddress(msg.FromName, msg.From));
                mime.To.Add(MailboxAddress.Parse(msg.To));
                mime.Subject = msg.Subject;
                mime.Body = new BodyBuilder { HtmlBody = msg.HtmlBody }.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(AppConfig.SmtpHost, AppConfig.SmtpPort, SecureSocketOptions.StartTls, ct);
                    await client.AuthenticateAsync(AppConfig.SmtpUsername, AppConfig.SmtpPassword, ct);
                    var response = await client.SendAsync(mime, ct);
                    await client.DisconnectAsync(true, ct);
                    return MailSendResult.Ok(response);
                }
            }
            catch (SmtpCommandException ex)
            {
                var code = ((int)ex.StatusCode).ToString();
                foreach (var p in PermFailPfx) if (code.StartsWith(p)) return MailSendResult.Fail(ex.Message, false);
                foreach (var p in RetryPfx)    if (code.StartsWith(p)) return MailSendResult.Fail(ex.Message, true);
                return MailSendResult.Fail(ex.Message, false);
            }
            catch (Exception ex) { return MailSendResult.Fail(ex.Message, true); }
        }
    }
}
