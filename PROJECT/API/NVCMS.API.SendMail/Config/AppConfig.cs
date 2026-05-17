using System.Configuration;
namespace NVCMS.API.SendMail.Config
{
    public static class AppConfig
    {
        public static string ConnectionString   => Get("ConnectionString");
        public static string SmtpHost           => Get("Smtp.Host");
        public static int    SmtpPort           => int.Parse(Get("Smtp.Port", "587"));
        public static string SmtpUsername       => Get("Smtp.Username");
        public static string SmtpPassword       => Get("Smtp.Password");
        public static int    BatchSize          => int.Parse(Get("Worker.BatchSize",        "100"));
        public static int    MaxConcurrent      => int.Parse(Get("Worker.MaxConcurrent",    "10"));
        public static int    MaxRetries         => int.Parse(Get("Worker.MaxRetries",       "5"));
        public static int    IdleDelaySeconds   => int.Parse(Get("Worker.IdleDelaySeconds", "5"));
        public static string DefaultFromEmail   => Get("Worker.DefaultFromEmail");
        public static string DefaultFromName    => Get("Worker.DefaultFromName");
        public static int    RateLimitPerMinute => int.Parse(Get("RateLimit.PerMinute",     "840"));
        public static string AppDomain          => Get("App.Domain", "https://yourdomain.com");
        private static string Get(string key, string fallback = "")
        {
            var v = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(v) ? fallback : v;
        }
    }
}
