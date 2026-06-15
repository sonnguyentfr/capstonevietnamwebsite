namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.student_from_ladipage (by convention or fluent config)
    public class student_from_ladipage
    {
        public int id { get; set; }                             // INT IDENTITY(1,1) NOT NULL
        public string? hotendem { get; set; }                   // NVARCHAR(200) NULL
        public string? ten { get; set; }                        // NVARCHAR(50) NULL
        public bool? gioi_tinh { get; set; }                    // BIT NULL
        public DateTime? ngay_sinh { get; set; } = new DateTime(1970, 1, 1);              // DATE NULL
        public string? so_dien_thoai { get; set; }              // NVARCHAR(30) NULL
        public string? email { get; set; }                      // NVARCHAR(50) NULL
        public string? truong_dang_hoc { get; set; }            // NVARCHAR(500) NULL
        public string? event_dia_diem { get; set; }             // NVARCHAR(500) NULL
        public int? event_id { get; set; }                      // INT NULL
        public int? event_dia_diem_id { get; set; }             // INT NULL
        public string? source { get; set; }                     // NVARCHAR(500) NULL
        public string? medium { get; set; }                     // NVARCHAR(500) NULL
        public string? link { get; set; }                       // NVARCHAR(500) NULL
        public string? ladi_page_id { get; set; }               // NVARCHAR(500) NULL
        public string? client_ip { get; set; }                  // NVARCHAR(50) NULL
        public string? thong_tin_khac { get; set; }                  // NVARCHAR(50) NULL
        public bool? is_update_crm { get; set; } = false;                 // NVARCHAR(50) NULL
        public DateTime? created_date { get; set; } = DateTime.Now;
    }
}