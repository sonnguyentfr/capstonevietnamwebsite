namespace NVCMS.API.ReadGoogleSheet.Models.Config
{
    public class HangfireJobSettings
    {
        public JobSetting ZnsRefreshToken { get; set; } = new();

        public JobSetting CampaignBatch { get; set; } = new();

        public JobSetting ZnsTemplateSync { get; set; } = new();

        /// <summary>Đọc Google Sheet → student_from_ladipage (thay DNN ImportCrmDataScheduledJob).</summary>
        public JobSetting ImportCrmData { get; set; } = new();

        /// <summary>student_from_ladipage → Student_Info / NV_Events_Student (thay DNN CopyDataStudentFromLadiScheduledJob).</summary>
        public JobSetting CopyStudentFromLadi { get; set; } = new();
    }

    public class JobSetting
    {
        public bool Enabled { get; set; }

        public string Cron { get; set; }

        public string TimeZone { get; set; }
    }
}