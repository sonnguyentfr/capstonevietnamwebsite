using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

public interface IGoogleSheetsService
{
    Task<IList<IList<object>>> ReadRowsAsync(string spreadsheetId, string rangeA1);
}

public class GoogleSheetsService : IGoogleSheetsService
{
    private readonly SheetsService _service;

    public GoogleSheetsService(IConfiguration configuration, IWebHostEnvironment env, ILogger<GoogleSheetsService> logger)
    {
        var jsonRelPath = configuration["Google:ServiceAccountJsonPath"];
        if (string.IsNullOrWhiteSpace(jsonRelPath))
            throw new InvalidOperationException("Google:ServiceAccountJsonPath is missing in configuration.");

        var jsonPath = Path.Combine(env.ContentRootPath, jsonRelPath.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(jsonPath))
        {
            logger.LogError("Service account file not found: {FullPath}", jsonPath);
            throw new FileNotFoundException("Google service account configuration file not found", jsonPath);
        }

        var credential = GoogleCredential
            .FromFile(jsonPath)
            .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

        _service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "NVCMS.API.ReadGoogleSheet"
        });
    }

    public async Task<IList<IList<object>>> ReadRowsAsync(string spreadsheetId, string rangeA1)
    {
        var request = _service.Spreadsheets.Values.Get(spreadsheetId, rangeA1);
        var response = await request.ExecuteAsync();
        return response.Values ?? new List<IList<object>>();
    }
}