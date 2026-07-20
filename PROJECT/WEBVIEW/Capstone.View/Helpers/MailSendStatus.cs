namespace NCapstone.View.Helpers
{
    /// <summary>
    /// Hằng số string status cho cột Status [nvarchar] trong Marketing_Mail_Send_Log.
    /// </summary>
    public static class MailSendStatus
    {
        public const string Queued    = "Queued";
        public const string Sent      = "Sent";
        public const string Failed    = "Failed";
        public const string Delivered = "Delivered";
        public const string Opened    = "Opened";
        public const string Clicked   = "Clicked";
    }
}
