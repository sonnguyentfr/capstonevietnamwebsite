using System;
using System.Collections.Generic;
namespace NVCMS.API.SendMail.Domain
{
    public class Campaign
    {
        public long      Id              { get; set; }
        public string    Name            { get; set; }
        public string    Subject         { get; set; }
        public string    HtmlContent     { get; set; }
        public int       Status          { get; set; }
        public string    FromEmail       { get; set; }
        public string    FromName        { get; set; }
        public DateTime  CreatedDate     { get; set; }
        public DateTime? ScheduledDate   { get; set; }
        public int       TotalRecipients { get; set; }
        public int       SentCount       { get; set; }
        public int       FailedCount     { get; set; }
        public int       OpenCount       { get; set; }
        public int       ClickCount      { get; set; }
    }
    public class MailQueueItem
    {
        public long           Id            { get; set; }
        public long           CampaignId    { get; set; }
        public long           RecipientId   { get; set; }
        public string         Email         { get; set; }
        public string         Subject       { get; set; }
        public string         Body          { get; set; }
        public MailQueueStatus Status        { get; set; }
        public int            RetryCount    { get; set; }
        public string         LastError     { get; set; }
        public DateTime       CreatedDate   { get; set; }
        public DateTime?      SentDate      { get; set; }
        public DateTime?      NextRetryDate { get; set; }
        public string         LockedBy      { get; set; }
    }
    public class CampaignStats
    {
        public long   CampaignId { get; set; }
        public string Name       { get; set; }
        public int    Total      { get; set; }
        public int    Sent       { get; set; }
        public int    Failed     { get; set; }
        public int    Pending    { get; set; }
        public int    Opens      { get; set; }
        public int    Clicks     { get; set; }
        public double OpenRate   => Total > 0 ? Math.Round((double)Opens  / Total * 100, 2) : 0;
        public double ClickRate  => Total > 0 ? Math.Round((double)Clicks / Total * 100, 2) : 0;
        public double SentRate   => Total > 0 ? Math.Round((double)Sent   / Total * 100, 2) : 0;
    }
    public class MailSendResult
    {
        public bool   Success      { get; set; }
        public string SmtpResponse { get; set; }
        public string Error        { get; set; }
        public bool   ShouldRetry  { get; set; }
        public static MailSendResult Ok(string r)   => new MailSendResult { Success = true,  SmtpResponse = r };
        public static MailSendResult Fail(string e, bool retry) => new MailSendResult { Success = false, Error = e, ShouldRetry = retry };
    }
    public class OutgoingMailMessage
    {
        public string From     { get; set; }
        public string FromName { get; set; }
        public string To       { get; set; }
        public string Subject  { get; set; }
        public string HtmlBody { get; set; }
    }
    public class CreateCampaignRequest
    {
        public string    Name        { get; set; }
        public string    Subject     { get; set; }
        public string    HtmlContent { get; set; }
        public string    FromEmail   { get; set; }
        public string    FromName    { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public List<RecipientItem> Recipients { get; set; } = new List<RecipientItem>();
    }
    public class RecipientItem
    {
        public long   Id       { get; set; }
        public string Email    { get; set; }
        public string FullName { get; set; }
    }
}
