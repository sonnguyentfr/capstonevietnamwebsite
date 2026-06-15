using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface ICrmDataService
    {
        Task<int> ImportFromGoogleSheetAsync(string spreadsheetId, string range, int eventCat_id);
        Task<IEnumerable<student_from_ladipage>> GetAllAsync();
    }
}