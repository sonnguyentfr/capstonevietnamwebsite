namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class Zalo_Message_Log
    {
        public long Id { get; set; }

        public string Phone { get; set; }

        public string FullName { get; set; }

        public long TemplateId { get; set; }

        public string TrackingId { get; set; }

        public int Status { get; set; }

        public string Message { get; set; }

        public string RequestJson { get; set; }

        public string ResponseJson { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}