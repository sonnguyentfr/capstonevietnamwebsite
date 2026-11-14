using Dapper;
using Microsoft.Data.SqlClient;
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
        private readonly string? _crmConnStr;

        public CrmDataService(
            ICrmDataLadingRepository repository,
            IGoogleSheetService googleSheetService,
            IConfiguration config,
            ILogger<CrmDataService> logger)
        {
            _repository = repository;
            _googleSheetService = googleSheetService;
            _crmConnStr = config.GetConnectionString("DefaultCRMConnection");
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
                        created_date = item.created_date,
                        SheetRow = item.SheetRow
                    };
                    listRecords.Add(record);
                }
                if (!listRecords.Any())
                {
                    _logger.LogInformation("All records already exist in database");
                    return 0;
                }

                // Dòng có giá trị dài hơn cột DB thường là dấu hiệu sheet bị lệch cột so với
                // dòng tiêu đề (vd nội dung tư vấn rơi vào cột client_ip). KHÔNG cắt bớt (sẽ ghi
                // dữ liệu sai mà không ai biết): bỏ riêng các dòng đó, insert các dòng hợp lệ,
                // rồi ném lỗi để job gửi mail cảnh báo. Dòng bị bỏ chưa vào DB nên lần chạy
                // sau (khi đã sửa sheet) sẽ được import lại.
                var overflows = await FindColumnOverflowsAsync(listRecords);
                var rejected = overflows
                    .Select(kv => new RejectedSheetRow
                    {
                        SheetRow = kv.Key.SheetRow,
                        Problems = kv.Value + " (thường do dòng tiêu đề lệch cột dữ liệu)",
                        Values = DescribeRow(kv.Key),
                    })
                    .ToList();

                var toInsert = listRecords.Where(r => !overflows.ContainsKey(r)).ToList();

                // Insert; dòng nào lỗi DB (kiểu dữ liệu, ràng buộc...) thì bỏ riêng dòng đó
                var insertFailures = await _repository.AddRangeSkipFailuresAsync(toInsert);
                foreach (var (record, error) in insertFailures)
                {
                    _logger.LogWarning(error, "Bỏ qua dòng sheet {Row} (email={Email}, sđt={Phone}): lỗi insert",
                        record.SheetRow, record.email, record.so_dien_thoai);

                    rejected.Add(new RejectedSheetRow
                    {
                        SheetRow = record.SheetRow,
                        Problems = "Lỗi insert DB: " + error.GetBaseException().Message,
                        Values = DescribeRow(record),
                    });
                }

                var imported = toInsert.Count - insertFailures.Count;
                _logger.LogInformation("Imported {Count} new records from spreadsheet {SpreadsheetId}",
                    imported, spreadsheetId);

                // Các dòng hợp lệ đã vào DB; báo lại các dòng bị bỏ để job gửi mail và chạy tiếp
                if (rejected.Count > 0)
                {
                    throw new SheetRowsRejectedException(
                        spreadsheetId,
                        rejected.OrderBy(r => r.SheetRow ?? int.MaxValue).ToList(),
                        imported);
                }

                return imported;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing data from Google Sheet {SpreadsheetId}", spreadsheetId);
                throw;
            }
        }

        /// <summary>
        /// Đọc độ rộng thật các cột chuỗi của student_from_ladipage (INFORMATION_SCHEMA), trả về
        /// các dòng có giá trị vượt quá kèm mô tả. Không đọc được độ rộng → coi như không có dòng lỗi.
        /// </summary>
        private async Task<Dictionary<student_from_ladipage, string>> FindColumnOverflowsAsync(
            List<student_from_ladipage> records)
        {
            var result = new Dictionary<student_from_ladipage, string>(ReferenceEqualityComparer.Instance);
            if (string.IsNullOrWhiteSpace(_crmConnStr)) return result;

            Dictionary<string, int> limits;
            try
            {
                await using var conn = new SqlConnection(_crmConnStr);
                var rows = await conn.QueryAsync<(string Name, int Len)>(
                    """
                    SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH
                    FROM   INFORMATION_SCHEMA.COLUMNS
                    WHERE  TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'student_from_ladipage'
                      AND  CHARACTER_MAXIMUM_LENGTH > 0
                    """);
                limits = rows.ToDictionary(r => r.Name, r => r.Len, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Không đọc được độ rộng cột student_from_ladipage → bỏ qua bước kiểm tra");
                return result;
            }

            var props = typeof(student_from_ladipage).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanRead)
                .ToList();

            foreach (var record in records)
            {
                var problems = new List<string>();
                foreach (var prop in props)
                {
                    if (!limits.TryGetValue(prop.Name, out var max)) continue;
                    if (prop.GetValue(record) is not string value || value.Length <= max) continue;

                    problems.Add($"{prop.Name} dài {value.Length} ký tự > cột DB {max}");
                }

                if (problems.Count > 0)
                {
                    result[record] = string.Join("; ", problems);
                    _logger.LogWarning("Bỏ qua dòng sheet {Row} (email={Email}, sđt={Phone}): {Problems}",
                        record.SheetRow, record.email, record.so_dien_thoai, result[record]);
                }
            }

            return result;
        }

        /// <summary>Giá trị các cột đọc từ sheet của 1 dòng (theo dòng tiêu đề hiện tại) để đưa vào mail lỗi.</summary>
        private static List<KeyValuePair<string, string?>> DescribeRow(student_from_ladipage r) =>
        [
            new("hotendem (họ tên)", $"{r.hotendem} {r.ten}".Trim()),
            new("email", r.email),
            new("so_dien_thoai", r.so_dien_thoai),
            new("truong_dang_hoc", r.truong_dang_hoc),
            new("event_dia_diem", r.event_dia_diem),
            new("ngay_sinh", r.ngay_sinh?.ToString("dd/MM/yyyy")),
            new("source", r.source),
            new("medium", r.medium),
            new("link", r.link),
            new("client_ip", r.client_ip),
            new("created_date", r.created_date?.ToString("dd/MM/yyyy HH:mm:ss")),
            new("cột khác (thong_tin_khac)", r.thong_tin_khac),
        ];

        public async Task<IEnumerable<student_from_ladipage>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}