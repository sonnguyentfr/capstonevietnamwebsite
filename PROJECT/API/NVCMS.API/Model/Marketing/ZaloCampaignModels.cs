using System.Collections.Generic;

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
        public System.DateTime UpdatedAt { get; set; }
    }

    public class ZaloPhoneValidateResult
    {
        public int ValidCount { get; set; }
        public int InvalidCount { get; set; }
        public int DupCount { get; set; }
        public List<string> ValidList { get; set; }
        public List<string> InvalidList { get; set; }
    }

    public class AddPhoneRequest
    {
        public int CampaignId { get; set; }
        public string FullName { get; set; }
        public string PhoneRaw { get; set; }
    }

    public class ValidatePhoneRequest
    {
        public string PhoneList { get; set; }
    }

    public class AddPhoneBulkRequest
    {
        public int CampaignId { get; set; }
        public string PhoneList { get; set; }
    }

    public class DeletePhoneRequest
    {
        public int Id { get; set; }
    }

    public class DeleteAllPhoneRequest
    {
        public int CampaignId { get; set; }
    }
}
