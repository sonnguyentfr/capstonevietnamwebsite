-- ============================================================
-- SP: sp_student_from_ladipage_update_crm
-- Database: CapstoneVietNamCRM
-- Gọi từ: CrmSyncRepository.MarkLadipageSyncedAsync (Dapper, param @Id)
--         (trước đây: DNN SqlDataProvider.StudentFromLadipage_Info_Update_Crm)
-- Mục đích: đánh dấu 1 bản ghi student_from_ladipage đã đẩy sang CRM.
-- Lỗi khắc phục: SqlException 2812 "Could not find stored procedure
--                'sp_student_from_ladipage_update_crm'" trong CopyStudentFromLadiJob.
-- ============================================================
USE [CapstoneVietNamCRM];
GO

-- (Tuỳ chọn) Kiểm tra xem SP có đang nằm ở schema khác dbo không
SELECT s.name AS [schema], p.name, p.create_date, p.modify_date
FROM sys.procedures p
JOIN sys.schemas s ON s.schema_id = p.schema_id
WHERE p.name LIKE N'%student_from_ladipage%';
GO

IF OBJECT_ID(N'dbo.sp_student_from_ladipage_update_crm', N'P') IS NULL
    EXEC (N'CREATE PROCEDURE dbo.sp_student_from_ladipage_update_crm AS RETURN 0;');
GO

ALTER PROCEDURE [dbo].[sp_student_from_ladipage_update_crm]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.student_from_ladipage
    SET    is_update_crm = 1
    WHERE  id = @Id;
END
GO

-- Sau khi tạo: xem các bản ghi đang chờ (39 bản ghi lỗi sẽ được job xử lý lại ở lần chạy kế tiếp)
SELECT id, event_id, event_dia_diem_id, hotendem, ten, email, so_dien_thoai, created_date, is_update_crm
FROM   dbo.student_from_ladipage
WHERE  ISNULL(is_update_crm, 0) = 0
ORDER BY id;
GO
