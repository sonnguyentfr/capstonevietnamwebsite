/* =============================================================================
   (TUỲ CHỌN - CHỈ CHẠY KHI ĐÃ ĐƯỢC DUYỆT) Nới cột token của bảng có sẵn dbo.Zalo_Token
   Database : [CapstoneVietNamWeb]

   Lý do: token mã hoá (Data Protection, tiền tố "dp1:") dài hơn token gốc.
   Nếu cột đang ngắn hơn, ZaloService tự lưu plaintext + log cảnh báo (không mất token).
   Script chỉ NỚI RỘNG cột lên NVARCHAR(MAX) khi cột đang giới hạn độ dài - không xoá/đổi dữ liệu.
   ============================================================================= */

-- Xem hiện trạng trước khi chạy:
SELECT c.name AS ColumnName, t.name AS TypeName, c.max_length, c.is_nullable
FROM sys.columns c
INNER JOIN sys.types t ON t.user_type_id = c.user_type_id
WHERE c.object_id = OBJECT_ID('dbo.Zalo_Token');
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Zalo_Token') AND name = 'AccessToken' AND max_length <> -1)
BEGIN
    DECLARE @nullA NVARCHAR(10) =
        CASE WHEN (SELECT is_nullable FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Zalo_Token') AND name = 'AccessToken') = 1
             THEN N'NULL' ELSE N'NOT NULL' END;
    EXEC (N'ALTER TABLE dbo.Zalo_Token ALTER COLUMN AccessToken NVARCHAR(MAX) ' + @nullA);
    PRINT 'Widened Zalo_Token.AccessToken';
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Zalo_Token') AND name = 'RefreshToken' AND max_length <> -1)
BEGIN
    DECLARE @nullR NVARCHAR(10) =
        CASE WHEN (SELECT is_nullable FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Zalo_Token') AND name = 'RefreshToken') = 1
             THEN N'NULL' ELSE N'NOT NULL' END;
    EXEC (N'ALTER TABLE dbo.Zalo_Token ALTER COLUMN RefreshToken NVARCHAR(MAX) ' + @nullR);
    PRINT 'Widened Zalo_Token.RefreshToken';
END
GO
