namespace NVCMS.API.ReadGoogleSheet.Models;

/// <summary>Một dòng NV_Events_Cat đang mở (SP NV_Events_Cat_SelectAllOnline).</summary>
public class EventCatRow
{
    public int id { get; set; }
    public string? CatName { get; set; }
    public string? Code { get; set; }
    public string? Email { get; set; }
    public string? ContentMail { get; set; }
    public string? titleMail { get; set; }
    public bool? sendmail { get; set; }
    public bool? sendcode { get; set; }
    public string? link_data_google_sheet { get; set; }
    public string? link_data_google_sheet_range { get; set; }
}

/// <summary>Một dòng NV_Events (SP NV_Events_SelectByID).</summary>
public class EventRow
{
    public int id { get; set; }
    public string? Title { get; set; }
    public string? diadiem { get; set; }
    public DateTime? fromdatetime { get; set; }
    public DateTime? enddatetime { get; set; }
    public int? Vanphong { get; set; }
    public int CatId { get; set; }
}

/// <summary>Một dòng student_from_ladipage chờ đẩy sang CRM.</summary>
public class LadipageRow
{
    public int id { get; set; }
    public string? hotendem { get; set; }
    public string? ten { get; set; }
    public bool? gioi_tinh { get; set; }
    public DateTime? ngay_sinh { get; set; }
    public string? so_dien_thoai { get; set; }
    public string? email { get; set; }
    public string? truong_dang_hoc { get; set; }
    public string? event_dia_diem { get; set; }
    public int? event_id { get; set; }
    public int? event_dia_diem_id { get; set; }
    public string? source { get; set; }
    public string? medium { get; set; }
    public string? link { get; set; }
    public string? thong_tin_khac { get; set; }
    public bool? is_update_crm { get; set; }
    public DateTime? created_date { get; set; }
}

/// <summary>Student_Info rút gọn - chỉ các cột job cần.</summary>
public class StudentInfoRow
{
    public int id { get; set; }
    public string? Code { get; set; }
    public string? Hotendem { get; set; }
    public string? Ten { get; set; }
    public string? Email { get; set; }
    public string? Sodienthoai { get; set; }
}

/// <summary>
/// Tham số insert Student_Info - map 1-1 với SP Student_Info_Insert (44 tham số).
///
/// Giá trị mặc định ở đây bám sát job VB cũ: các thuộc tính mà
/// StudentInfoInfo không gán sẽ là Nothing/0/False, và thực tế trong DB
/// các cột đó đang là NULL (đã kiểm chứng trên Student_Info). Vì vậy các
/// chuỗi không dùng để mặc định là null, KHÔNG phải chuỗi rỗng.
/// </summary>
public class StudentInsertArgs
{
    public int VP { get; set; }
    public int Type { get; set; } = 1;
    public string? Hotendem { get; set; }
    public string? Ten { get; set; }
    public bool Sex { get; set; }
    public DateTime Ngaysinh { get; set; } = new DateTime(1970, 1, 1);
    public int Kieungaysinh { get; set; }
    public string? Sodienthoai { get; set; }
    public string? Email { get; set; }
    public string? Diachi { get; set; }              // VB không gán → NULL
    public int Tinh { get; set; }
    public int Huyen { get; set; }
    public bool EB5 { get; set; }
    public string? PermissionUser { get; set; }      // VB không gán → NULL
    public int FollowPhuongThuc { get; set; } = 15;
    public int FollowKetQua { get; set; }
    public string? FollowNoiDung { get; set; }       // VB không gán → NULL
    public int FollowUpStatus { get; set; } = 1;
    public DateTime FollowUpDateUpdate { get; set; }
    public string? TuVanHocVanmongmuon { get; set; } = string.Empty;
    public string? TuVanNamdi { get; set; } = string.Empty;
    public string? TuVanKyhoc { get; set; } = "0";
    public string? TuVanNganhhoc { get; set; } = string.Empty;
    public string? TuVanTruongdukien { get; set; } = string.Empty;
    public string? TuVanQuocgia { get; set; } = "0";
    public int TuVanDiadiem { get; set; }
    public int TuVanKhanangchitra { get; set; }
    public string? TuVanKhac { get; set; }
    public int TuVanEditUserId { get; set; }
    public DateTime TuVanEditDate { get; set; }
    public int TuVanApproveUserId { get; set; }
    public DateTime TuVanApproveDate { get; set; }
    public string? HocVanDanghoc { get; set; }              // VB không gán → NULL
    public string? HocVanTruongdanghoc { get; set; }
    public string? HocVanDiemtrungbinh { get; set; }        // VB không gán → NULL
    public string? HocVanDiemsobaithichuanhoa { get; set; } // VB không gán → NULL
    public string? HocVanLuuy { get; set; }                 // VB không gán → NULL
    public int HocVanEditUserId { get; set; }
    public DateTime HocVanEditDate { get; set; }
    public int HocVanApproveUserId { get; set; }
    public DateTime HocVanApproveDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; } = 1;
    public int PortalId { get; set; }
    public bool Xoa { get; set; }
}
