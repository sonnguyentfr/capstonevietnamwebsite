using System;

namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class Zalo_Token
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}