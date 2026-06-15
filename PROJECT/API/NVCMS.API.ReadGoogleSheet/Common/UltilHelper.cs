using System.Net;
using System.Net.Mail;
using System.Text;

namespace NVCMS.API.ReadGoogleSheet.Common
{
    public static class UltilHelper
    {
        // Returns "v{ho_dem}; {ten}"
        public static string FormatHoDemTen(string fullName)
        {
            SplitHoTen(fullName, out var hoDem, out var ten);
            return $"v{hoDem}; {ten}";
        }

        // Splits into ho_dem and ten
        public static void SplitHoTen(string fullName, out string hoDem, out string ten)
        {
            hoDem = string.Empty;
            ten = string.Empty;

            if (string.IsNullOrWhiteSpace(fullName))
                return;

            var cleaned = NormalizeSpaces(fullName.Trim());

            var parts = cleaned.Split(' ');
            if (parts.Length == 1)
            {
                // Only one token -> treat as ten
                ten = parts[0];
                return;
            }

            // Last token is ten, the rest is ho_dem
            ten = parts[^1];
            hoDem = string.Join(" ", parts.Take(parts.Length - 1));
        }

        private static string NormalizeSpaces(string s)
        {
            // Collapse multiple spaces into single space
            var sb = new StringBuilder(s.Length);
            bool lastIsSpace = false;
            foreach (var ch in s)
            {
                if (char.IsWhiteSpace(ch))
                {
                    if (!lastIsSpace)
                    {
                        sb.Append(' ');
                        lastIsSpace = true;
                    }
                }
                else
                {
                    sb.Append(ch);
                    lastIsSpace = false;
                }
            }
            return sb.ToString().Trim();
        }

        // Extract the numeric ID before the first '-' (e.g., "123- - ten" -> "123")
        public static string ExtractLeadingId(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var s = input.Trim();

            // Find position of first hyphen
            var idx = s.IndexOf('-');
            if (idx <= 0) // no hyphen or hyphen at start
                return string.Empty;

            // Take substring before hyphen and trim spaces
            var before = s.Substring(0, idx).Trim();

            // Keep only digits from the prefix
            var digits = new StringBuilder();
            foreach (var ch in before)
            {
                if (char.IsDigit(ch)) digits.Append(ch);
                else if (!char.IsWhiteSpace(ch)) break; // stop at first non-space non-digit
            }

            return digits.Length > 0 ? digits.ToString() : string.Empty;
        }

        // Extract and join multiple IDs from a list of strings -> "id1; id2; id3"
        public static string ExtractLeadingIdsJoined(IEnumerable<string> inputs, string separator = "; ")
        {
            if (inputs == null) return string.Empty;
            var ids = inputs
                .Select(ExtractLeadingId)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .ToList();
            return ids.Count > 0 ? string.Join(separator, ids) : string.Empty;
        }

        // Extract IDs from a multi-line text (each line like "123- something")
        public static string ExtractLeadingIdsFromText(string text, string separator = "; ")
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            var lines = text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim());
            return ExtractLeadingIdsJoined(lines, separator);
        }

        /// <summary>
        /// Send email using SMTP settings
        /// </summary>
        /// <param name="smtpHost">SMTP server host (e.g., email-smtp.ap-southeast-1.amazonaws.com)</param>
        /// <param name="smtpPort">SMTP server port (e.g., 587)</param>
        /// <param name="enableSsl">Enable SSL/TLS</param>
        /// <param name="username">SMTP username/account</param>
        /// <param name="password">SMTP password</param>
        /// <param name="fromEmail">From email address</param>
        /// <param name="toEmail">To email address (comma-separated for multiple)</param>
        /// <param name="ccEmail">CC email address (comma-separated for multiple)</param>
        /// <param name="bccEmail">BCC email address (comma-separated for multiple)</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body (HTML supported)</param>
        /// <param name="isBodyHtml">Is body HTML format (default: true)</param>
        /// <param name="fromName">From display name (optional)</param>
        public static async Task SendMailAsync(
            string smtpHost,
            int smtpPort,
            bool enableSsl,
            string username,
            string password,
            string fromEmail,
            string toEmail,
            string? ccEmail,
            string? bccEmail,
            string subject,
            string body,
            bool isBodyHtml = true,
            string? fromName = null)
        {
            if (string.IsNullOrWhiteSpace(smtpHost))
                throw new ArgumentException("SMTP host cannot be null or empty", nameof(smtpHost));

            if (string.IsNullOrWhiteSpace(fromEmail))
                throw new ArgumentException("From email cannot be null or empty", nameof(fromEmail));

            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("To email cannot be null or empty", nameof(toEmail));

            using var mailMessage = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
                Priority = MailPriority.High
            };

            // Set From address
            mailMessage.From = string.IsNullOrWhiteSpace(fromName)
                ? new MailAddress(fromEmail)
                : new MailAddress(fromEmail, fromName);

            // Add To recipients
            AddEmailAddresses(mailMessage.To, toEmail);

            // Add CC recipients
            if (!string.IsNullOrWhiteSpace(ccEmail))
                AddEmailAddresses(mailMessage.CC, ccEmail);

            // Add BCC recipients
            if (!string.IsNullOrWhiteSpace(bccEmail))
                AddEmailAddresses(mailMessage.Bcc, bccEmail);

            // Configure SMTP client
            using var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password)
            };

            await smtpClient.SendMailAsync(mailMessage);
            
            // Small delay to prevent overwhelming SMTP server
            await Task.Delay(100);
        }

        /// <summary>
        /// Send email synchronously (wrapper for async version)
        /// </summary>
        public static void SendMail(
            string smtpHost,
            int smtpPort,
            bool enableSsl,
            string username,
            string password,
            string fromEmail,
            string toEmail,
            string? ccEmail,
            string? bccEmail,
            string subject,
            string body,
            bool isBodyHtml = true,
            string? fromName = null)
        {
            SendMailAsync(smtpHost, smtpPort, enableSsl, username, password, 
                fromEmail, toEmail, ccEmail, bccEmail, subject, body, isBodyHtml, fromName)
                .GetAwaiter()
                .GetResult();
        }

        private static void AddEmailAddresses(MailAddressCollection collection, string emails)
        {
            if (string.IsNullOrWhiteSpace(emails))
                return;

            var addresses = emails
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrWhiteSpace(e));

            foreach (var address in addresses)
            {
                try
                {
                    collection.Add(address);
                }
                catch (FormatException ex)
                {
                    // Log invalid email format but continue with others
                    System.Diagnostics.Debug.WriteLine($"Invalid email address: {address}. Error: {ex.Message}");
                }
            }
        }
    }
}