-- ============================================================
-- Lỗi ImportCrmDataJob: SqlException 2628 "String or binary data would be truncated"
-- khi insert student_from_ladipage (sheet thêm cột yeu_cau_tu_van → JSON thong_tin_khac dài ra).
-- Database: CapstoneVietNamCRM
-- ============================================================
USE [CapstoneVietNamCRM];
GO

-- 1. Độ rộng hiện tại các cột (-1 = MAX)
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'student_from_ladipage'
ORDER BY ORDINAL_POSITION;
GO

-- 2. Nới thong_tin_khac (JSON các cột phát sinh của sheet, gồm yeu_cau_tu_van) lên NVARCHAR(MAX)
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
           WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'student_from_ladipage'
             AND COLUMN_NAME = 'thong_tin_khac' AND CHARACTER_MAXIMUM_LENGTH <> -1)
BEGIN
    ALTER TABLE dbo.student_from_ladipage ALTER COLUMN thong_tin_khac NVARCHAR(MAX) NULL;
    PRINT N'Đã nới thong_tin_khac → NVARCHAR(MAX)';
END
ELSE
    PRINT N'thong_tin_khac đã là MAX, không cần sửa';
GO
