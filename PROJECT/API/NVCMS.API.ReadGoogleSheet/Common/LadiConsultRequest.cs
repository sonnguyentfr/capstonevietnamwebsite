using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NVCMS.API.ReadGoogleSheet.Common;

/// <summary>
/// Cột "yeu_cau_tu_van" trên Google Sheet Ladipage. Cột này không nằm trong danh sách cột cố định
/// nên GoogleSheetService gom vào JSON student_from_ladipage.thong_tin_khac; ở đây lấy ra để ghi
/// vào Student_Info.TuVanKhac (mục "Tư vấn - Khác", giống form /dang-ky-tu-van của website).
/// </summary>
public static class LadiConsultRequest
{
    /// <summary>WebView_TuVan_Upsert cũng cắt TuVanKhac ở 1000 ký tự.</summary>
    public const int TuVanKhacMaxLength = 1000;

    /// <summary>Lấy nội dung yêu cầu tư vấn từ JSON thong_tin_khac; không có / rỗng → null.</summary>
    public static string? Extract(string? thongTinKhacJson)
    {
        if (string.IsNullOrWhiteSpace(thongTinKhacJson)) return null;

        JObject obj;
        try
        {
            obj = JObject.Parse(thongTinKhacJson);
        }
        catch (JsonException)
        {
            return null;
        }

        foreach (var prop in obj.Properties())
        {
            // Header sheet đã ToLower + Trim; chấp nhận thêm "yeu cau tu van", "yeucautuvan"
            var name = prop.Name.Replace("_", "").Replace(" ", "");
            if (!name.Equals("yeucautuvan", StringComparison.OrdinalIgnoreCase)) continue;

            var text = prop.Value.Type == JTokenType.Null ? null : prop.Value.ToString().Trim();
            return string.IsNullOrEmpty(text) ? null : text;
        }

        return null;
    }

    /// <summary>TuVanKhac cho khách MỚI: câu mô tả nguồn đăng ký + yêu cầu tư vấn (nếu có).</summary>
    public static string BuildNewStudentTuVanKhac(string? catName, string? request)
    {
        var text = $"<mark>Khách đăng ký Online tại: {catName}</mark>.";
        if (!string.IsNullOrWhiteSpace(request))
            text += $"<br/>Yêu cầu tư vấn: {Encode(request)}";

        return Truncate(text);
    }

    /// <summary>
    /// Dòng chèn lên đầu TuVanKhac của khách ĐÃ CÓ (giữ nội dung cũ), cùng kiểu với
    /// WebView_TuVan_Upsert: "[dd/MM/yyyy - Ladi: tên sự kiện] Yêu cầu tư vấn: ...".
    /// </summary>
    public static string BuildExistingStudentLine(string? catName, string request, DateTime now)
        => Truncate($"[{now:dd/MM/yyyy} - Ladi: {catName}] Yêu cầu tư vấn: {Encode(request)}");

    /// <summary>Nội dung khách nhập được HtmlEncode vì CRM hiển thị TuVanKhac dạng HTML.</summary>
    public static string Encode(string request) => WebUtility.HtmlEncode(request.Trim());

    private static string Truncate(string s)
        => s.Length <= TuVanKhacMaxLength ? s : s[..TuVanKhacMaxLength];
}
