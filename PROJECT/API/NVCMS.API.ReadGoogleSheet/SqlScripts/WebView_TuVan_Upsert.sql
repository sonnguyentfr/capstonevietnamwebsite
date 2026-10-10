-- ============================================================
-- SP: WebView_TuVan_Upsert
-- Database: CapstoneVietnam_old (CRM)
-- Dùng cho form /dang-ky-tu-van trên website.
--
--   1. Tìm khách theo SĐT (ưu tiên) rồi tới Email.
--      Bỏ qua giá trị giữ chỗ: SĐT = '0', Email = 'NA'.
--      SĐT so khớp cả dạng 84xxxxxxxxx lẫn 0xxxxxxxxx (dữ liệu cũ).
--   2a. Đã có → chỉ cập nhật TuVanKhac (chèn nội dung mới lên đầu,
--       giữ nội dung cũ) + ghi Student_Follow_Log.
--   2b. Chưa có → INSERT Student_Info đầy đủ thông tin tư vấn,
--       HinhThuc = nguồn Website, Code = {CodePrefix}{YY}{MM}{id}
--       (ví dụ WEB2610151800), ghi Student_StudentHinhThuc + Follow_Log.
-- ============================================================
IF OBJECT_ID('dbo.WebView_TuVan_Upsert', 'P') IS NOT NULL
	DROP PROCEDURE dbo.WebView_TuVan_Upsert;
GO

CREATE PROCEDURE [dbo].[WebView_TuVan_Upsert]
	-- ── Thông tin cá nhân ─────────────────────────────────────────────────
	@Hotendem             NVARCHAR(200),
	@Ten                  NVARCHAR(100),
	@Sex                  BIT,
	@Ngaysinh             DATETIME      = NULL,
	@Sodienthoai          NVARCHAR(30),           -- '0' nếu không có
	@Email                NVARCHAR(200),          -- 'NA' nếu không có
	@Diachi               NVARCHAR(500),
	@TinhId               INT           = NULL,
	-- ── Thông tin tư vấn ──────────────────────────────────────────────────
	@TuVanHocVanmongmuon  NVARCHAR(100),          -- id Cap_CapGiaoduc, dạng '2,4,'
	@TuVanNamdi           NVARCHAR(100),
	@TuVanNganhhoc        NVARCHAR(400),
	@TuVanKhanangchitra   INT,                    -- id Student_TuVanInfo_ChiTra, 0 = chưa chọn
	@TuVanQuocgia         NVARCHAR(100),          -- id Cap_Location, dạng '1,38,'
	@TuVanKhac            NVARCHAR(1000),
	-- ── Cấu hình ──────────────────────────────────────────────────────────
	@HinhThuc             INT,                    -- Student_Code_Hinhthuc.id (7 = Website)
	@CodePrefix           NVARCHAR(10),
	@PortalId             INT,
	-- ── Output ────────────────────────────────────────────────────────────
	@StudentId            INT           OUTPUT,
	@StudentCode          NVARCHAR(50)  OUTPUT,
	@IsExisting           BIT           OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @Now DATETIME = GETDATE();
	DECLARE @HasPhone BIT = CASE WHEN ISNULL(@Sodienthoai, '') NOT IN ('', '0') THEN 1 ELSE 0 END;
	DECLARE @HasEmail BIT = CASE WHEN ISNULL(@Email, '') NOT IN ('', 'NA') THEN 1 ELSE 0 END;
	DECLARE @PhoneLocal NVARCHAR(30) =
		CASE WHEN @Sodienthoai LIKE '84%' THEN '0' + SUBSTRING(@Sodienthoai, 3, 30) ELSE @Sodienthoai END;

	BEGIN TRANSACTION;

	BEGIN TRY

		-- ══════════════════════════════════════════════════════════════════
		-- 1. Tìm khách hàng đã có (khóa để tránh tạo trùng khi submit đôi)
		-- ══════════════════════════════════════════════════════════════════
		DECLARE @ExistingId INT = NULL;
		DECLARE @ExistingCode NVARCHAR(50) = NULL;

		IF @HasPhone = 1
			SELECT TOP 1 @ExistingId = id, @ExistingCode = Code
			FROM   Student_Info WITH (UPDLOCK, HOLDLOCK)
			WHERE  Sodienthoai IN (@Sodienthoai, @PhoneLocal)
			  AND  ISNULL(Xoa, 0) = 0
			ORDER BY id ASC;

		IF @ExistingId IS NULL AND @HasEmail = 1
			SELECT TOP 1 @ExistingId = id, @ExistingCode = Code
			FROM   Student_Info WITH (UPDLOCK, HOLDLOCK)
			WHERE  Email = @Email
			  AND  ISNULL(Xoa, 0) = 0
			ORDER BY id ASC;

		IF @ExistingId IS NOT NULL
		BEGIN
			-- ══════════════════════════════════════════════════════════════
			-- 2a. Đã có trên CRM → chỉ cập nhật "Tư vấn khác"
			-- ══════════════════════════════════════════════════════════════
			IF ISNULL(@TuVanKhac, '') <> ''
				UPDATE Student_Info
				SET    TuVanKhac = LEFT(
							N'[' + CONVERT(NVARCHAR(10), @Now, 103) + N' - Website] ' + @TuVanKhac
							+ CASE WHEN ISNULL(TuVanKhac, '') = '' THEN N'' ELSE CHAR(13) + CHAR(10) + TuVanKhac END,
							1000),
					   TuVanEditDate = @Now
				WHERE  id = @ExistingId;

			EXEC Student_Follow_Log_Insert
				@StudentId   = @ExistingId,
				@Noidung     = N'KHÁCH HÀNG ĐĂNG KÝ TƯ VẤN LẠI TỪ WEBSITE',
				@CreatedDate = @Now,
				@PortalId    = @PortalId;

			SET @StudentId   = @ExistingId;
			SET @StudentCode = ISNULL(@ExistingCode, '');
			SET @IsExisting  = 1;
		END
		ELSE
		BEGIN
			-- ══════════════════════════════════════════════════════════════
			-- 2b. Khách mới → INSERT
			-- ══════════════════════════════════════════════════════════════
			INSERT INTO Student_Info
				(Hotendem, Ten, Sex, Ngaysinh, Kieungaysinh,
				 Sodienthoai, Email, Diachi, Tinh, Huyen,
				 VP, Type, HinhThuc,
				 FollowPhuongThuc, FollowKetQua, FollowUpStatus, FollowUpDateUpdate,
				 TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien,
				 TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac,
				 TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate,
				 HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate,
				 CreatedDate, UserId, PortalId, Xoa,
				 isspy, dongyguithongtin, Indirect)
			VALUES
				(@Hotendem, @Ten, @Sex, @Ngaysinh, 0,
				 @Sodienthoai, @Email, @Diachi, @TinhId, 0,
				 0, 1, @HinhThuc,
				 15, 0, 1, @Now,
				 @TuVanHocVanmongmuon, @TuVanNamdi, '0', @TuVanNganhhoc, '',
				 @TuVanQuocgia, 0, ISNULL(@TuVanKhanangchitra, 0), @TuVanKhac,
				 0, @Now, 0, @Now,
				 0, @Now, 0, @Now,
				 @Now, 0, @PortalId, 0,
				 0, 0, 0);

			SET @StudentId = SCOPE_IDENTITY();

			SET @StudentCode =
				@CodePrefix
				+ RIGHT(CAST(YEAR(@Now) AS VARCHAR(4)), 2)
				+ RIGHT('0' + CAST(MONTH(@Now) AS VARCHAR(2)), 2)
				+ CAST(@StudentId AS VARCHAR(20));

			UPDATE Student_Info SET Code = @StudentCode WHERE id = @StudentId;

			EXEC Student_StudentHinhThuc_Insert
				@StudentId   = @StudentId,
				@HinhThuc    = @HinhThuc,
				@CreatedDate = @Now,
				@UserId      = -1,
				@PortalId    = @PortalId;

			DECLARE @LogText NVARCHAR(MAX) =
				N'KHÁCH HÀNG: [' + LTRIM(RTRIM(ISNULL(@Hotendem, '') + ' ' + ISNULL(@Ten, ''))) + N'] - ĐĂNG KÝ TƯ VẤN TỪ WEBSITE';

			EXEC Student_Follow_Log_Insert
				@StudentId   = @StudentId,
				@Noidung     = @LogText,
				@CreatedDate = @Now,
				@PortalId    = @PortalId;

			SET @IsExisting = 0;
		END

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO
