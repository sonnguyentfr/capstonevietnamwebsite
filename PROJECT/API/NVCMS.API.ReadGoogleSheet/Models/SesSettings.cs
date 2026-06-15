namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class SesSettings
    {
        public string AccessKeyId { get; set; } = string.Empty;
        public string SecretAccessKey { get; set; } = string.Empty;
        public string Region { get; set; } = "ap-southeast-1";
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        /// <summary>Thư mục gốc chứa các file HTML template trên server.</summary>
        public string TemplateBasePath { get; set; } = "Templates/Email";
    }
}
