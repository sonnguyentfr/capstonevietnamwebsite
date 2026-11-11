-- ============================================================
-- Kiểm tra luồng ngày sinh: Google Sheet → student_from_ladipage → Student_Info
-- Database: CapstoneVietNamCRM (chỉ SELECT, không sửa dữ liệu)
-- ============================================================
USE [CapstoneVietNamCRM];
GO

-- 1. SP job CopyStudentFromLadi dùng để đọc ladipage có trả cột ngay_sinh không?
--    (Nếu SELECT liệt kê cột mà thiếu ngay_sinh → Student_Info luôn nhận 01/01/1970)
SELECT CASE WHEN OBJECT_DEFINITION(OBJECT_ID(N'dbo.sp_student_from_ladipage_select_by_event_id')) LIKE N'%ngay_sinh%'
                 OR OBJECT_DEFINITION(OBJECT_ID(N'dbo.sp_student_from_ladipage_select_by_event_id')) LIKE N'%*%'
            THEN N'OK: có ngay_sinh (hoặc SELECT *)' ELSE N'THIẾU ngay_sinh' END AS sp_select_ladipage;

-- 2. SP Student_Info_Insert có ghi @Ngaysinh vào bảng không?
SELECT CASE WHEN OBJECT_DEFINITION(OBJECT_ID(N'dbo.Student_Info_Insert')) LIKE N'%@Ngaysinh%'
            THEN N'OK: có @Ngaysinh' ELSE N'KHÔNG thấy @Ngaysinh' END AS sp_student_insert;
-- Xem toàn văn nếu cần:
-- EXEC sp_helptext N'dbo.Student_Info_Insert';

-- 3. Bao nhiêu bản ghi ladipage đã bị gán ngày sinh giả 01/01/1970 (lỗi parse MM/dd cũ)
SELECT event_id,
       COUNT(*)                                                        AS tong,
       SUM(CASE WHEN ngay_sinh = '1970-01-01' THEN 1 ELSE 0 END)      AS ngay_1970,
       SUM(CASE WHEN ngay_sinh IS NULL THEN 1 ELSE 0 END)             AS ngay_null,
       SUM(CASE WHEN ngay_sinh NOT IN ('1970-01-01') THEN 1 ELSE 0 END) AS ngay_hop_le
FROM dbo.student_from_ladipage
GROUP BY event_id
ORDER BY event_id DESC;
GO
