using System;

namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class ZaloToken
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}