using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface ISESService
    {
        /// <summary>
        /// Gửi email với HTML body đã render sẵn (DNN cung cấp).
        /// Không đọc template từ DB.
        /// </summary>
        Task<string> SendBodyEmailAsync(
            string fromEmail,
            string toEmail,
            string toName,
            string subject,
            string htmlBody);

        /// <summary>
        /// Đọc HTML từ Marketing_Mail_Template.FilePath, replace placeholder, gửi qua SES.
        /// Chỉ dùng cho luồng cũ – không gọi trong flow mới.
        /// </summary>
        Task<string> SendTemplatedEmailAsync(
            Marketing_Mail_Template    template,
            string                   fromEmail,
            string                   toEmail,
            string                   toName,
            Dictionary<string, string> placeholders);

        /// <summary>
        /// Gửi cho một recipient trong ListMail (luồng cũ).
        /// </summary>
        Task SendToRecipientAsync(
            Marketing_Mail_Template    template,
            Marketing_Mail_ListMail    recipient,
            Dictionary<string, string> extraPlaceholders);
    }
}
