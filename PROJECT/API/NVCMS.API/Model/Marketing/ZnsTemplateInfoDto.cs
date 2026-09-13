using System;

namespace NVCMS.API.Model.Marketing
{
    public class ZnsTemplateInfoDto
    {
        public long Id { get; set; }
        public long TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string Status { get; set; }
        public string PreviewUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
