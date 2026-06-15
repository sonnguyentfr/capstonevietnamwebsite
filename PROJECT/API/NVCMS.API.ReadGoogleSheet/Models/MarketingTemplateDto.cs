namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class CreateTemplateRequest
    {
        public string TemplateName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int PortalId { get; set; }
    }

    public class TemplateResponse
    {
        public int Id { get; set; }
        public string? TemplateName { get; set; }
        public string? FilePath { get; set; }
        public int? PortalId { get; set; }
    }
}
