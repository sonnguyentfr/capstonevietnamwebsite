using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class CrmDataService : ICrmDataService
    {
        private readonly ICrmDataLadingRepository _repository;
        private readonly IGoogleSheetService _googleSheetService;
        private readonly ILogger<CrmDataService> _logger;

        public CrmDataService(
            ICrmDataLadingRepository repository,
            IGoogleSheetService googleSheetService,
            ILogger<CrmDataService> logger)
        {
            _repository = repository;
            _googleSheetService = googleSheetService;
            _logger = logger;
        }

        public async Task<int> ImportFromGoogleSheetAsync(string spreadsheetId, string range, int eventCat_id)
        {
            try
            {
                // Read data from Google Sheet
                var sheetData = await _googleSheetService.ReadSheetDataAsync(spreadsheetId, range);

                if (!sheetData.Any())
                {
                    _logger.LogInformation("No data to import from spreadsheet {SpreadsheetId}", spreadsheetId);
                    return 0;
                }

                // Get existing records to avoid duplicates
                var existingKeys = new HashSet<string>();

                foreach (var item in sheetData)
                {
                    var existingRecords = await _repository.GetByEmailOrPhoneAsync(item.email, item.so_dien_thoai);

                    foreach (var record in existingRecords)
                    {
                        existingKeys.Add($"{record.email}|{record.so_dien_thoai}");
                    }
                }

                // Filter out existing records
                var newRecords = sheetData
                    .Where(d => !existingKeys.Contains($"{d.email}|{d.so_dien_thoai}"))
                    .ToList();
                //Bat dau phan tich lai du lieu
                var listRecords = new List<student_from_ladipage>();
                foreach (var item in newRecords)
                {
                    string hoDem, ten;
                    UltilHelper.SplitHoTen(item.hotendem, out hoDem, out ten);
                    var record = new student_from_ladipage
                    {
                        hotendem = hoDem,
                        ten = ten,
                        gioi_tinh = item.gioi_tinh ?? false,
                        ngay_sinh = item.ngay_sinh,
                        so_dien_thoai = item.so_dien_thoai,
                        email = item.email,
                        truong_dang_hoc = item.truong_dang_hoc,
                        event_dia_diem = item.event_dia_diem,
                        event_id = eventCat_id,
                        event_dia_diem_id = int.TryParse( UltilHelper.ExtractLeadingId(item.event_dia_diem ?? string.Empty), out var idVal) ? idVal : (int?)null,
                        source = item.source,
                        medium = item.medium,
                        link = item.link,
                        ladi_page_id = item.ladi_page_id,
                        client_ip = item.client_ip,
                        thong_tin_khac = item.thong_tin_khac,
                        created_date = item.created_date
                    };
                    listRecords.Add(record);
                }
                if (!listRecords.Any())
                {
                    _logger.LogInformation("All records already exist in database");
                    return 0;
                }

                // Insert new records
                await _repository.AddRangeAsync(listRecords);
                _logger.LogInformation("Imported {Count} new records from spreadsheet {SpreadsheetId}",
                    listRecords.Count, spreadsheetId);

                return listRecords.Count;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing data from Google Sheet {SpreadsheetId}", spreadsheetId);
                throw;
            }
        }

        public async Task<IEnumerable<student_from_ladipage>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}