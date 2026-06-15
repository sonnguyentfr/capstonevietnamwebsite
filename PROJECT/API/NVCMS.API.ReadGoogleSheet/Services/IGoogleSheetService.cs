using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface IGoogleSheetService
    {
        Task<IEnumerable<student_from_ladipage>> ReadSheetDataAsync(string spreadsheetId, string range);
    }
}