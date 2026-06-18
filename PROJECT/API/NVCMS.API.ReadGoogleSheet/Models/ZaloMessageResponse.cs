namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class ZaloMessageResponse
    {
        public int Error { get; set; }

        public string Message { get; set; }

        public ZaloMessageData Data { get; set; }
    }

    public class ZaloMessageData
    {
        public string SentTime { get; set; }

        public string SendingMode { get; set; }

        public ZaloQuota Quota { get; set; }

        public string MsgId { get; set; }
    }

    public class ZaloQuota
    {
        public string RemainingQuota { get; set; }

        public string DailyQuota { get; set; }
    }
}