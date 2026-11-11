using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.API.ReadGoogleSheet.Jobs;

namespace NVCMS.API.ReadGoogleSheet.Services;

/// <summary>Một dòng ngày sinh sẽ được sửa.</summary>
public sealed class BirthDateFix
{
    public int EventCatId { get; init; }
    public int LadipageId { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    /// <summary>Ngày sinh đọc đúng từ Google Sheet (dd/MM/yyyy).</summary>
    public DateTime NewDate { get; init; }
    public DateTime? OldLadipageDate { get; init; }
    public bool UpdateLadipage { get; init; }
    public int? StudentId { get; init; }
    public DateTime? OldStudentInfoDate { get; init; }
    public bool UpdateStudentInfo { get; set; }
    /// <summary>Vì sao không sửa Student_Info (nếu không sửa).</summary>
    public string? StudentInfoNote { get; set; }
}

public sealed class BirthDateBackfillResult
{
    public bool Applied { get; init; }
    public int SheetRows { get; set; }
    public int LadipageRows { get; set; }
    public int LadipageMatched { get; set; }
    public int LadipageUpdated { get; set; }
    public int StudentInfoUpdated { get; set; }
    public List<string> Warnings { get; } = new();
    public List<BirthDateFix> Fixes { get; } = new();
}

public interface IBirthDateBackfillService
{
    Task<BirthDateBackfillResult> RunAsync(IReadOnlyCollection<int> eventCatIds, bool apply, CancellationToken ct = default);
}

/// <summary>
/// Sửa ngày sinh bị lỗi do job import cũ parse nhầm MM/dd/yyyy (sheet nhập dd/MM/yyyy):
/// ngày &gt; 12 bị gán 01/01/1970, ngày ≤ 12 bị đảo ngày/tháng.
///
/// Đọc lại Google Sheet của từng nhóm sự kiện bằng BirthDateParser, khớp với
/// student_from_ladipage theo (email, số điện thoại) như lúc import, rồi:
///   • student_from_ladipage.ngay_sinh  ← ngày đúng trên sheet (sheet là nguồn gốc)
///   • Student_Info.Ngaysinh            ← ngày đúng, CHỈ KHI đang NULL, 01/01/1970
///     hoặc trùng đúng giá trị sai cũ của ladipage — ngày tư vấn viên đã sửa tay giữ nguyên.
/// apply = false: chỉ trả danh sách sẽ sửa, không ghi DB.
/// </summary>
public class BirthDateBackfillService : IBirthDateBackfillService
{
    private static readonly DateTime Placeholder = new(1970, 1, 1);

    private readonly IGoogleSheetService _sheets;
    private readonly string _crmConnStr;
    private readonly ILogger<BirthDateBackfillService> _logger;

    public BirthDateBackfillService(
        IGoogleSheetService sheets,
        IConfiguration config,
        ILogger<BirthDateBackfillService> logger)
    {
        _sheets = sheets;
        _crmConnStr = config.GetConnectionString("DefaultCRMConnection")
                      ?? throw new InvalidOperationException("DefaultCRMConnection not configured");
        _logger = logger;
    }

    private sealed class CatRow
    {
        public int id { get; set; }
        public string? CatName { get; set; }
        public string? link_data_google_sheet { get; set; }
        public string? link_data_google_sheet_range { get; set; }
    }

    private sealed class LadiRow
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? so_dien_thoai { get; set; }
        public DateTime? ngay_sinh { get; set; }
    }

    private sealed class StudentRow
    {
        public int id { get; set; }
        public string? Email { get; set; }
        public string? Sodienthoai { get; set; }
        public DateTime? Ngaysinh { get; set; }
    }

    public async Task<BirthDateBackfillResult> RunAsync(
        IReadOnlyCollection<int> eventCatIds, bool apply, CancellationToken ct = default)
    {
        var result = new BirthDateBackfillResult { Applied = apply };

        await using var conn = new SqlConnection(_crmConnStr);
        await conn.OpenAsync(ct);

        var cats = (await conn.QueryAsync<CatRow>(
            """
            SELECT Id AS id, CatName, link_data_google_sheet, link_data_google_sheet_range
            FROM   NV_Events_Cat
            WHERE  Id IN @Ids
            """, new { Ids = eventCatIds })).ToList();

        foreach (var missing in eventCatIds.Except(cats.Select(c => c.id)))
            result.Warnings.Add($"EventCat {missing}: không tồn tại trong NV_Events_Cat");

        // ── 1. Tính các dòng ladipage cần sửa ────────────────────────────────
        foreach (var cat in cats)
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(cat.link_data_google_sheet))
            {
                result.Warnings.Add($"EventCat {cat.id} ({cat.CatName}): không có Google Sheet");
                continue;
            }

            var sheetRows = (await _sheets.ReadSheetDataAsync(
                cat.link_data_google_sheet, cat.link_data_google_sheet_range ?? string.Empty)).ToList();
            result.SheetRows += sheetRows.Count;

            // Khoá giống lúc import (CrmDataService): email|số điện thoại nguyên văn
            var sheetDates = new Dictionary<string, DateTime>();
            var ambiguous = new HashSet<string>();
            foreach (var r in sheetRows.Where(r => r.ngay_sinh.HasValue))
            {
                var key = Key(r.email, r.so_dien_thoai);
                if (sheetDates.TryGetValue(key, out var existing) && existing != r.ngay_sinh!.Value)
                    ambiguous.Add(key);
                else
                    sheetDates[key] = r.ngay_sinh!.Value;
            }
            foreach (var key in ambiguous)
            {
                sheetDates.Remove(key);
                result.Warnings.Add($"EventCat {cat.id}: '{key}' có nhiều ngày sinh khác nhau trên sheet → bỏ qua");
            }

            var ladiRows = (await conn.QueryAsync<LadiRow>(
                "SELECT id, email, so_dien_thoai, ngay_sinh FROM student_from_ladipage WHERE event_id = @EventId",
                new { EventId = cat.id })).ToList();
            result.LadipageRows += ladiRows.Count;

            foreach (var row in ladiRows)
            {
                if (!sheetDates.TryGetValue(Key(row.email, row.so_dien_thoai), out var newDate))
                    continue;

                result.LadipageMatched++;
                result.Fixes.Add(new BirthDateFix
                {
                    EventCatId = cat.id,
                    LadipageId = row.id,
                    Email = row.email,
                    Phone = row.so_dien_thoai,
                    NewDate = newDate,
                    OldLadipageDate = row.ngay_sinh,
                    UpdateLadipage = row.ngay_sinh?.Date != newDate,
                });
            }
        }

        // ── 2. Tìm Student_Info tương ứng (email trước, rồi số điện thoại — như job copy) ──
        var emails = result.Fixes.Select(f => f.Email?.Trim()).Where(IsUsableEmail)
                                 .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var phones = result.Fixes.SelectMany(f => new[] { f.Phone?.Trim(), CopyStudentFromLadiJob.NormalizeVnPhone(f.Phone) })
                                 .Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();

        var students = new List<StudentRow>();
        foreach (var chunk in emails.Chunk(1000))
            students.AddRange(await conn.QueryAsync<StudentRow>(
                "SELECT id, Email, Sodienthoai, Ngaysinh FROM Student_Info WHERE Email IN @V", new { V = chunk }));
        foreach (var chunk in phones.Chunk(1000))
            students.AddRange(await conn.QueryAsync<StudentRow>(
                "SELECT id, Email, Sodienthoai, Ngaysinh FROM Student_Info WHERE Sodienthoai IN @V", new { V = chunk }));

        var byEmail = students.Where(s => !string.IsNullOrWhiteSpace(s.Email))
            .GroupBy(s => s.Email!.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.OrderBy(s => s.id).First(), StringComparer.OrdinalIgnoreCase);
        var byPhone = students.Where(s => !string.IsNullOrWhiteSpace(s.Sodienthoai))
            .GroupBy(s => s.Sodienthoai!.Trim())
            .ToDictionary(g => g.Key, g => g.OrderBy(s => s.id).First());

        var studentFixes = new Dictionary<int, (StudentRow Student, DateTime NewDate, List<BirthDateFix> Rows)>();
        var conflicted = new HashSet<int>();

        foreach (var fix in result.Fixes.ToList())
        {
            StudentRow? st = null;
            if (IsUsableEmail(fix.Email))
                byEmail.TryGetValue(fix.Email!.Trim(), out st);
            if (st == null && !string.IsNullOrWhiteSpace(fix.Phone))
            {
                if (!byPhone.TryGetValue(fix.Phone.Trim(), out st))
                    byPhone.TryGetValue(CopyStudentFromLadiJob.NormalizeVnPhone(fix.Phone), out st);
            }

            if (st == null)
            {
                fix.StudentInfoNote = "Chưa có trong Student_Info";
                continue;
            }

            var current = st.Ngaysinh?.Date;
            var replaceable = current == null
                              || current == Placeholder
                              || (fix.OldLadipageDate.HasValue && current == fix.OldLadipageDate.Value.Date);

            var replaced = new BirthDateFix
            {
                EventCatId = fix.EventCatId, LadipageId = fix.LadipageId, Email = fix.Email, Phone = fix.Phone,
                NewDate = fix.NewDate, OldLadipageDate = fix.OldLadipageDate, UpdateLadipage = fix.UpdateLadipage,
                StudentId = st.id, OldStudentInfoDate = st.Ngaysinh,
            };
            result.Fixes[result.Fixes.IndexOf(fix)] = replaced;

            if (current == fix.NewDate)
            {
                replaced.StudentInfoNote = "Student_Info đã đúng";
                continue;
            }
            if (!replaceable)
            {
                replaced.StudentInfoNote = "Student_Info có ngày sinh khác (có thể đã sửa tay) → giữ nguyên";
                continue;
            }

            if (studentFixes.TryGetValue(st.id, out var prev) && prev.NewDate != fix.NewDate)
            {
                conflicted.Add(st.id);
                replaced.StudentInfoNote = "Khách đăng ký nhiều lần với ngày sinh khác nhau → giữ nguyên";
                continue;
            }

            if (!studentFixes.ContainsKey(st.id))
                studentFixes[st.id] = (st, fix.NewDate, new List<BirthDateFix>());
            studentFixes[st.id].Rows.Add(replaced);
        }

        foreach (var id in conflicted)
        {
            foreach (var r in studentFixes[id].Rows)
                r.StudentInfoNote = "Khách đăng ký nhiều lần với ngày sinh khác nhau → giữ nguyên";
            studentFixes.Remove(id);
        }

        // Mỗi Student_Info chỉ đánh dấu cập nhật ở 1 dòng
        foreach (var (_, v) in studentFixes)
            v.Rows[0].UpdateStudentInfo = true;

        // Chỉ trả về những dòng có thay đổi
        result.Fixes.RemoveAll(f => !f.UpdateLadipage && !f.UpdateStudentInfo);

        var ladiToUpdate = result.Fixes.Where(f => f.UpdateLadipage).ToList();
        var studentToUpdate = studentFixes.Values.ToList();

        if (!apply)
        {
            result.LadipageUpdated = ladiToUpdate.Count;
            result.StudentInfoUpdated = studentToUpdate.Count;
            return result;
        }

        // ── 3. Ghi DB trong 1 transaction ────────────────────────────────────
        await using var tx = await conn.BeginTransactionAsync(ct);
        try
        {
            foreach (var f in ladiToUpdate)
                result.LadipageUpdated += await conn.ExecuteAsync(
                    "UPDATE student_from_ladipage SET ngay_sinh = @NewDate WHERE id = @Id",
                    new { f.NewDate, Id = f.LadipageId }, tx);

            foreach (var (st, newDate, _) in studentToUpdate)
                // Điều kiện Ngaysinh cũ: tránh ghi đè nếu ai đó vừa sửa tay sau lúc chạy thử
                result.StudentInfoUpdated += await conn.ExecuteAsync(
                    """
                    UPDATE Student_Info SET Ngaysinh = @NewDate
                    WHERE  id = @Id AND (Ngaysinh = @Old OR (Ngaysinh IS NULL AND @Old IS NULL))
                    """,
                    new { NewDate = newDate, Id = st.id, Old = st.Ngaysinh }, tx);

            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(CancellationToken.None);
            throw;
        }

        _logger.LogInformation(
            "BirthDateBackfill EventCats={Cats}: sửa {Ladi} dòng student_from_ladipage, {Student} Student_Info",
            string.Join(",", eventCatIds), result.LadipageUpdated, result.StudentInfoUpdated);

        return result;
    }

    private static string Key(string? email, string? phone) => $"{email}|{phone}";

    /// <summary>Bỏ email rác kiểu "na", "-" để không khớp nhầm sang khách khác.</summary>
    private static bool IsUsableEmail(string? email)
        => !string.IsNullOrWhiteSpace(email) && email.Contains('@');
}
