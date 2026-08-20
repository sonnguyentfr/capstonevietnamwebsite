namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Send_Log
    public class MarketingMailSendLog
    {
        public long     Id             { get; set; }
        public int      CampaignSendId { get; set; }
        public int      ListMailId     { get; set; }
        public string   Email          { get; set; } = string.Empty;
        public string?  SesMessageId   { get; set; }
        public string   Status         { get; set; } = string.Empty;
        public string?  ErrorMessage   { get; set; }
        public DateTime? SentTime      { get; set; }
        public DateTime? DeliveredTime { get; set; }
        public DateTime? OpenedTime    { get; set; }
        public DateTime? ClickedTime   { get; set; }
        public DateTime  CreatedDate   { get; set; }
        public int SenderEmailId { get; set; }
    }
}


