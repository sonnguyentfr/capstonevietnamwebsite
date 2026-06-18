namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class ZaloMessageRequest<T>
    {
        public string phone { get; set; }

        public long template_id { get; set; }

        public T template_data { get; set; }

        public string tracking_id { get; set; } = Guid.NewGuid().ToString();
    }
    public class ZaloMessage_DangKyThanhCongSK_Request
    {
        public string student_fullname { get; set; }

        public string student_code { get; set; }

        public string event_cat_description { get; set; }

        public string event_time { get; set; }

        public string event_cat_name { get; set; }

        public string event_name { get; set; }

        public string event_cat_shortlink { get; set; }

        public string hotline { get; set; }
    }
}