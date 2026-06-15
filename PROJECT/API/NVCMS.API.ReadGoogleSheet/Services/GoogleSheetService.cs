using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;
using NVCMS.API.ReadGoogleSheet.Entities;
using System.Globalization;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class GoogleSheetService : IGoogleSheetService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleSheetService> _logger;

        public GoogleSheetService(IConfiguration configuration, ILogger<GoogleSheetService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<student_from_ladipage>> ReadSheetDataAsync(string spreadsheetId, string range)
        {
            try
            {
                var credential = await GetCredentialAsync();
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "NVCMS Google Sheet Reader"
                });

                var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
                var response = await request.ExecuteAsync();

                var values = response.Values;
                if (values == null || values.Count < 4)
                {
                    _logger.LogWarning("Not enough data in spreadsheet {SpreadsheetId}", spreadsheetId);
                    return Enumerable.Empty<student_from_ladipage>();
                }

                // --- FIXED COLUMNS ---
                var fixedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "hotendem", "ten", "ngay_sinh",
                    "so_dien_thoai", "email", "truong_dang_hoc",
                    "event_dia_diem", "source", "medium", "link",
                    "client_ip", "created_date"
                };

                var dataList = new List<student_from_ladipage>();

                // --- HEADER LÀ DÒNG SỐ 3 (INDEX = 2) ---
                var header = values[2].Select(x => x?.ToString()?.Trim().ToLower() ?? "").ToList();

                // --- DATA START TỪ DÒNG SỐ 4 (INDEX = 3) ---
                for (int i = 3; i < values.Count; i++)
                {
                    var row = values[i];
                    var extraDict = new Dictionary<string, object>();
                    var student = new student_from_ladipage();

                    for (int col = 0; col < header.Count; col++)
                    {
                        var colName = header[col];
                        var value = row.Count > col ? row[col] : null;
                        var text = value?.ToString() ?? "";

                        if (fixedColumns.Contains(colName))
                        {
                            switch (colName)
                            {
                                case "hotendem": student.hotendem = text; break;
                                case "ten": student.ten = text; break;
                                //case "gioi_tinh": student.gioi_tinh = bool.TryParse(text, out var gioiTinh) ? gioiTinh : false; break;
                                case "so_dien_thoai": student.so_dien_thoai = text; break;
                                case "email": student.email = text; break;
                                case "truong_dang_hoc": student.truong_dang_hoc = text; break;
                                case "event_dia_diem": student.event_dia_diem = text; break;
                                case "source": student.source = text; break;
                                case "medium": student.medium = text; break;
                                case "link": student.link = text; break;
                                case "client_ip": student.client_ip = text; break;

                                case "ngay_sinh":
                                    student.ngay_sinh =
                                        DateTime.TryParseExact(
                                            text,
                                            "MM/dd/yyyy",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out var dob
                                        )
                                        ? dob
                                        : new DateTime(1970, 1, 1);
                                    break;

                                case "created_date":
                                    student.created_date = DateTime.TryParse(text, out var cd)
                                        ? cd
                                        : DateTime.Now;
                                    break;
                            }
                        }
                        else
                        {
                            // Đây là cột phát sinh → đưa vào JSON
                            extraDict[colName] = text;
                        }
                    }

                    student.thong_tin_khac = JsonConvert.SerializeObject(extraDict);
                    dataList.Add(student);
                }

                return dataList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading Google Sheet {SpreadsheetId}", spreadsheetId);
                throw;
            }
        }


        private async Task<GoogleCredential> GetCredentialAsync()
        {
            var jsonPath = _configuration["Google:ServiceAccountJsonPath"] ?? "capstoneweb-151609-ae0fdb82aef2.json";

            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"Service account file not found: {jsonPath}");
            }

            using var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
            var credential = await Task.Run(() =>
                GoogleCredential.FromStream(stream)
                    .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly));

            return credential;
        }
    }
}