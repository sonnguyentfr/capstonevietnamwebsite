using Hangfire;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

/// <summary>
/// Thay cho DNN job NVCMS.Modules.Scheduler.ImportCrmDataScheduledJob.
///
/// Flow: lấy toàn bộ nhóm sự kiện đang mở (NV_Events_Cat_SelectAllOnline)
///       → với nhóm nào có link_data_google_sheet thì đọc Google Sheet
///       và ghi bản ghi mới vào student_from_ladipage.
///
/// Khác job cũ: job VB gọi vòng qua HTTP tới chính API này (kèm JWT).
/// Ở đây gọi thẳng ICrmDataService trong process → bỏ được HTTP hop,
/// token, và timeout 30s.
///
/// Mọi exception đều được gom lại và gửi mail cảnh báo cho đội IT
/// (xem IJobAlertService, cấu hình ở CrmSync:AlertEmails).
/// </summary>
public class ImportCrmDataJob
{
    public const string JobName = nameof(ImportCrmDataJob);

    private readonly ICrmSyncRepository _crm;
    private readonly ICrmDataService _crmDataService;
    private readonly IJobAlertService _alert;
    private readonly CrmSyncSettings _settings;
    private readonly ILogger<ImportCrmDataJob> _logger;

    public ImportCrmDataJob(
        ICrmSyncRepository crm,
        ICrmDataService crmDataService,
        IJobAlertService alert,
        IOptions<CrmSyncSettings> settings,
        ILogger<ImportCrmDataJob> logger)
    {
        _crm = crm;
        _crmDataService = crmDataService;
        _alert = alert;
        _settings = settings.Value;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 2, DelaysInSeconds = new[] { 120, 600 })]
    [DisableConcurrentExecution(timeoutInSeconds: 600)]
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var failures = new List<JobFailure>();

        try
        {
            await RunAsync(failures, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;   // Hangfire dừng job - không phải lỗi, không cảnh báo
        }
        catch (Exception ex)
        {
            // Lỗi thoát ra khỏi vòng lặp → job dừng hẳn
            failures.Add(new JobFailure
            {
                Step = "Chạy job ImportCrmDataJob",
                FallbackSource = $"{JobName}.RunAsync",
                Exception = ex
            });

            await _alert.ReportAsync(JobName, failures, aborted: true);
            throw;   // để Hangfire ghi nhận job failed và retry
        }
    }

    private async Task RunAsync(List<JobFailure> failures, CancellationToken cancellationToken)
    {
        var portalId = _settings.PortalId;
        _logger.LogInformation("ImportCrmDataJob bắt đầu (PortalId={PortalId})", portalId);

        var eventCats = await _crm.GetOnlineEventCatsAsync(portalId);
        _logger.LogInformation("Tìm thấy {Count} nhóm sự kiện đang mở", eventCats.Count);

        var totalImported = 0;
        var skipped = 0;

        foreach (var cat in eventCats)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(cat.link_data_google_sheet))
            {
                skipped++;
                _logger.LogInformation(
                    "EventCat {Id} ({Name}) KHÔNG có Google Sheet ID → bỏ qua",
                    cat.id, cat.CatName);
                continue;
            }

            try
            {
                var imported = await _crmDataService.ImportFromGoogleSheetAsync(
                    cat.link_data_google_sheet,
                    cat.link_data_google_sheet_range ?? string.Empty,
                    cat.id);

                totalImported += imported;
                _logger.LogInformation(
                    "EventCat {Id} ({Name}) → import {Imported} bản ghi mới",
                    cat.id, cat.CatName, imported);
            }
            catch (Exception ex)
            {
                // Một sheet lỗi không được làm hỏng các sheet còn lại
                var context = new Dictionary<string, string?>
                {
                    ["EventCatId"] = cat.id.ToString(),
                    ["Tên nhóm sự kiện"] = cat.CatName,
                    ["SpreadsheetId"] = cat.link_data_google_sheet,
                    ["Range"] = cat.link_data_google_sheet_range
                };

                if (ex is SheetRowsRejectedException rejected)
                {
                    // Các dòng hợp lệ đã import; liệt kê từng dòng bị bỏ kèm giá trị các cột
                    totalImported += rejected.ImportedCount;
                    AddRejectedRows(context, rejected);
                }

                failures.Add(new JobFailure
                {
                    Step = ex is SheetRowsRejectedException r
                        ? $"Import Google Sheet của nhóm sự kiện \"{cat.CatName}\": {r.Rows.Count} dòng bị bỏ"
                        : $"Đọc Google Sheet của nhóm sự kiện \"{cat.CatName}\"",
                    FallbackSource = $"{JobName} → ICrmDataService.ImportFromGoogleSheetAsync",
                    Context = context,
                    Exception = ex
                });

                _logger.LogError(ex,
                    "EventCat {Id} ({Name}) - lỗi import Google Sheet {SheetId}",
                    cat.id, cat.CatName, cat.link_data_google_sheet);
            }
        }

        var summary =
            $"{totalImported} bản ghi mới, {skipped} nhóm bỏ qua (không có sheet), {failures.Count} nhóm lỗi";

        _logger.LogInformation("ImportCrmDataJob xong: {Summary}", summary);

        await _alert.ReportAsync(JobName, failures, aborted: false, summary);
    }

    /// <summary>Số dòng bị bỏ tối đa hiển thị chi tiết trong 1 mail.</summary>
    private const int MaxRejectedRowsInMail = 30;

    /// <summary>Mỗi dòng sheet bị bỏ → 1 mục "Dòng N trên sheet": lý do + giá trị từng cột.</summary>
    private static void AddRejectedRows(Dictionary<string, string?> context, SheetRowsRejectedException rejected)
    {
        context["Đã import"] = $"{rejected.ImportedCount} dòng hợp lệ";
        context["Số dòng bị bỏ"] = rejected.Rows.Count.ToString();

        var n = 0;
        foreach (var row in rejected.Rows.Take(MaxRejectedRowsInMail))
        {
            n++;
            var label = row.SheetRow.HasValue ? $"Dòng {row.SheetRow} trên sheet" : $"Dòng bị bỏ #{n}";

            var lines = new List<string> { $"LỖI: {row.Problems}" };
            lines.AddRange(row.Values.Select(v =>
                $"{v.Key}: {(string.IsNullOrEmpty(v.Value) ? "(trống)" : v.Value)}"));

            context[label] = string.Join("\n", lines);
        }

        if (rejected.Rows.Count > MaxRejectedRowsInMail)
            context["…"] = $"Còn {rejected.Rows.Count - MaxRejectedRowsInMail} dòng nữa, xem log ứng dụng";
    }
}
