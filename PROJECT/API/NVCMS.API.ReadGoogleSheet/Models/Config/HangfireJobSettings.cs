namespace NVCMS.API.ReadGoogleSheet.Models.Config
{
    public class HangfireJobSettings
    {
        public JobSetting ZnsRefreshToken { get; set; } = new();

        public JobSetting CampaignBatch { get; set; } = new();

        public JobSetting ZnsTemplateSync { get; set; } = new();
    }

    public class JobSetting
    {
        public bool Enabled { get; set; }

        public string Cron { get; set; }

        public string TimeZone { get; set; }
    }
}