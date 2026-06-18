namespace NVCMS.API.ReadGoogleSheet.Models.Job
{
    public class HangfireJobOptions
    {
        public JobOption ZnsRefreshToken { get; set; }

        public JobOption CampaignBatch { get; set; }
    }
    public class JobOption
    {
        public bool Enabled { get; set; }

        public string Cron { get; set; }

        public string TimeZone { get; set; }
    }
}
