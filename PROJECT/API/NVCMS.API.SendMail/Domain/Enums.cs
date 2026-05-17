namespace NVCMS.API.SendMail.Domain
{
    public enum MailQueueStatus { Pending=0, Processing=1, Sent=2, Failed=3, Retry=4, Bounce=5 }
    public enum CampaignStatus  { Draft=0, Queued=1, Sending=2, Done=3, Paused=4 }
    public enum TrackingType    { Open=0, Click=1 }
}
