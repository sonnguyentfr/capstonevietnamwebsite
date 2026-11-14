namespace NVCMS.API.ReadGoogleSheet.Services;

/// <summary>Một dòng Google Sheet không import được, kèm giá trị để hiển thị trong mail cảnh báo.</summary>
public sealed class RejectedSheetRow
{
    /// <summary>Số dòng trên Google Sheet (đếm từ 1, như Sheets hiển thị).</summary>
    public int? SheetRow { get; init; }
    /// <summary>Vì sao bị bỏ, ví dụ "client_ip dài 120 > 50".</summary>
    public string Problems { get; init; } = string.Empty;
    /// <summary>Giá trị các cột của dòng (tên cột → giá trị) theo dòng tiêu đề hiện tại.</summary>
    public IReadOnlyList<KeyValuePair<string, string?>> Values { get; init; } = [];
}

/// <summary>
/// Ném ra SAU KHI đã import các dòng hợp lệ, để job gửi mail cảnh báo liệt kê
/// những dòng bị bỏ (thường do sheet lệch cột so với dòng tiêu đề).
/// </summary>
public sealed class SheetRowsRejectedException : Exception
{
    public IReadOnlyList<RejectedSheetRow> Rows { get; }
    public int ImportedCount { get; }

    public SheetRowsRejectedException(string spreadsheetId, IReadOnlyList<RejectedSheetRow> rows, int importedCount)
        : base($"Sheet {spreadsheetId}: {rows.Count} dòng KHÔNG import được, đã bỏ qua " +
               $"(đã import {importedCount} dòng hợp lệ, job vẫn chạy tiếp). Các dòng này chưa vào DB " +
               "nên sẽ được thử lại ở lần chạy sau — xem lý do và giá trị từng dòng bên dưới.")
    {
        Rows = rows;
        ImportedCount = importedCount;
    }
}
