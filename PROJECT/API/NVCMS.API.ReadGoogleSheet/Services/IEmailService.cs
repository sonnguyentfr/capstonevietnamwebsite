namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body, string? ccEmail = null, string? bccEmail = null);
        Task SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string body, string? ccEmail = null, string? bccEmail = null);
    }
}