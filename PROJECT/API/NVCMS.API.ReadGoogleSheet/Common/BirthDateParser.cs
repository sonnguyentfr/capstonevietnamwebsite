using System.Globalization;

namespace NVCMS.API.ReadGoogleSheet.Common;

/// <summary>
/// Đọc ngày sinh nhập trên Google Sheet / Ladipage theo kiểu Việt Nam: ngày trước, tháng sau
/// (dd/MM/yyyy). Chấp nhận thiếu số 0 (5/3/2008), dấu "-" hoặc ".", kèm phần giờ
/// ("05/03/2008 00:00:00"), dạng ISO yyyy-MM-dd và số serial ngày của Google Sheet.
/// Giá trị không hợp lệ (sai định dạng, trước 1900, sau hôm nay) → null, KHÔNG gán 01/01/1970.
/// </summary>
public static class BirthDateParser
{
    private static readonly string[] Formats =
    {
        "d/M/yyyy", "d-M-yyyy", "d.M.yyyy",
        "yyyy-M-d", "yyyy/M/d",
    };

    private static readonly DateTime MinDate = new(1900, 1, 1);

    public static DateTime? Parse(string? text, DateTime? today = null)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;

        var s = text.Trim();

        // Bỏ phần giờ nếu có: "05/03/2008 00:00:00", "2008-03-05T00:00:00"
        var cut = s.IndexOfAny(new[] { ' ', 'T' });
        if (cut > 0) s = s[..cut];

        DateTime value;
        if (DateTime.TryParseExact(s, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            value = parsed;
        }
        else if (int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var serial)
                 && serial is >= 10000 and < 2958466)
        {
            // Ô định dạng Number → Sheets trả số ngày tính từ 30/12/1899 (giống OADate).
            // Chặn dưới 10000 (~1927) để số năm gõ tay như "2008" không bị hiểu thành ngày.
            value = DateTime.FromOADate(serial);
        }
        else
        {
            return null;
        }

        var max = (today ?? DateTime.Today).Date;
        if (value < MinDate || value.Date > max) return null;

        return value.Date;
    }
}
