using System.ComponentModel.DataAnnotations;

namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class GoogleSheetRequest
    {
        [Required]
        public string SpreadsheetId { get; set; } = string.Empty;
        
        public string Range { get; set; } = "Sheet1!A:D";
        public int eventCat_id { get; set; } 
    }
}