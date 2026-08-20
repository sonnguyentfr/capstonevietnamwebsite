namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Campaign_Send
    public class MarketingMailCampaignSend
    {
        public int      Id             { get; set; }
        public int      CampaignId     { get; set; }
        public int TemplateId { get; set; }
        public string   Subject        { get; set; } = string.Empty;
        public string?  Body           { get; set; }
        public int      Status         { get; set; }
        public int      TotalRecipient { get; set; }
        public DateTime CreatedDate    { get; set; }
    }
}
