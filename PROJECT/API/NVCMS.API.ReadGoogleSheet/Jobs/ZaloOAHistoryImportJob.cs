using Hangfire;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Jobs;

/// <summary>Nhập lịch sử chat Zalo OA (chạy tay qua POST /api/zalo-oa/history/import).</summary>
public class ZaloOAHistoryImportJob
{
    private readonly IZaloOAHistoryImportService _import;

    public ZaloOAHistoryImportJob(IZaloOAHistoryImportService import)
    {
        _import = import;
    }

    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    [AutomaticRetry(Attempts = 0)]
    public Task<ZaloOAHistoryImportResult> ExecuteAsync(ZaloOAHistoryImportRequest request, CancellationToken cancellationToken)
        => _import.ImportAsync(request, cancellationToken);
}
