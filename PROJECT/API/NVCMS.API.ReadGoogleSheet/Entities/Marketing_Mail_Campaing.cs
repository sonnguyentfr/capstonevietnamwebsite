namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Campaing
    // IMPORTANT: Bảng DB chỉ có 6 cột gốc – KHÔNG thêm cột mới vào đây
    public class Marketing_Mail_Campaing
    {
        public int id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UserId { get; set; }
        public int? PortalId { get; set; }
    }
}
