-- ============================================================
-- SP: WebView_EventRegistration_Upsert
-- Database: CapstoneVietnam_old
-- Version: 4 — Unified logic with EventCat-based code for new students
--              StudentCode format: {CatCode}{YY}{MM}{StudentId}
--              Example: HE2607150993
--                HE     = NV_Events_Cat.Code (from EventCatId)
--                26     = year (2 digits)
--                07     = month (2 digits)
--                150993 = Student_Info.id
--              
--              For existing students: keep existing Code
--              For duplicate registrations: update NV_Events_Student record
-- ============================================================
IF OBJECT_ID('dbo.WebView_EventRegistration_Upsert', 'P') IS NOT NULL
	DROP PROCEDURE dbo.WebView_EventRegistration_Upsert;
GO

CREATE PROCEDURE [dbo].[WebView_EventRegistration_Upsert]
	-- ── Student Information ──────────────────────────────────────────────
	@Hotendem       NVARCHAR(200),
	@Ten            NVARCHAR(100),
	@Sodienthoai    NVARCHAR(30),
	@Email          NVARCHAR(200),
	@Diachi         NVARCHAR(500),
	@Ngaysinh       DATETIME      = NULL,
	@TinhId         INT           = NULL,
	@PortalId       INT,
	-- ── Event Information ─────────────────────────────────────────────────
	@EventId        INT,
	@EventCatId     INT,
	-- ── Output ────────────────────────────────────────────────────────────
	@StudentId      INT           OUTPUT,
	@StudentCode    NVARCHAR(50)  OUTPUT,
	@IsDuplicate    BIT           OUTPUT    -- 1 = already registered for this event
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRANSACTION;

	BEGIN TRY

		-- ══════════════════════════════════════════════════════════════════
		-- 0. Get CatCode from NV_Events_Cat
		--    Fallback to 'C' if Code is NULL or empty
		-- ══════════════════════════════════════════════════════════════════
		DECLARE @CatCode NVARCHAR(20);
		SELECT @CatCode = NULLIF(LTRIM(RTRIM(ISNULL(Code, ''))), '')
		FROM   NV_Events_Cat
		WHERE  id = @EventCatId;

		IF @CatCode IS NULL SET @CatCode = 'C';

		-- ══════════════════════════════════════════════════════════════════
		-- 1. Find student by phone (priority) or email
		-- ══════════════════════════════════════════════════════════════════
		DECLARE @ExistingStudentId INT = NULL;
		DECLARE @ExistingCode NVARCHAR(50) = NULL;

		SELECT TOP 1 
			@ExistingStudentId = id,
			@ExistingCode = Code
		FROM   Student_Info
		WHERE  (Sodienthoai = @Sodienthoai AND @Sodienthoai <> '')
			OR (Email = @Email AND @Email <> '')
		ORDER BY
			CASE WHEN Sodienthoai = @Sodienthoai THEN 0 ELSE 1 END,
			id ASC;

		-- ══════════════════════════════════════════════════════════════════
		-- 2a. Student exists → UPDATE basic info
		--     - Text fields: only fill if currently empty
		--     - Ngaysinh / Tinh: only fill if NULL/0
		--     - Diachi: always update if user provides value
		-- ══════════════════════════════════════════════════════════════════
		IF @ExistingStudentId IS NOT NULL
		BEGIN
			UPDATE Student_Info
			SET
				Hotendem    = CASE WHEN ISNULL(Hotendem,    '') = '' THEN @Hotendem    ELSE Hotendem    END,
				Ten         = CASE WHEN ISNULL(Ten,         '') = '' THEN @Ten         ELSE Ten         END,
				Sodienthoai = CASE WHEN ISNULL(Sodienthoai, '') = '' THEN @Sodienthoai ELSE Sodienthoai END,
				Email       = CASE WHEN ISNULL(Email,       '') = '' THEN @Email       ELSE Email       END,
				Diachi      = CASE WHEN @Diachi <> ''                THEN @Diachi      ELSE Diachi      END,
				Ngaysinh    = CASE WHEN @Ngaysinh IS NOT NULL
										AND Ngaysinh IS NULL         THEN @Ngaysinh    ELSE Ngaysinh    END,
				Tinh        = CASE WHEN @TinhId IS NOT NULL
										AND ISNULL(Tinh, 0) = 0      THEN @TinhId      ELSE Tinh        END
			WHERE id = @ExistingStudentId;

			SET @StudentId = @ExistingStudentId;
			SET @StudentCode = ISNULL(@ExistingCode, 'C' + RIGHT('000000' + CAST(@ExistingStudentId AS VARCHAR(10)), 6));
		END

		-- ══════════════════════════════════════════════════════════════════
		-- 2b. Student does not exist → INSERT new with EventCat-based code
		-- ══════════════════════════════════════════════════════════════════
		ELSE
		BEGIN
			INSERT INTO Student_Info
				(Hotendem, Ten, Sodienthoai, Email, Diachi, Ngaysinh, Tinh,
				 VP, Type, Sex, Kieungaysinh,
				 FollowPhuongThuc, FollowKetQua, FollowUpStatus,
				 FollowUpDateUpdate, TuVanEditDate, TuVanApproveDate,
				 HocVanEditDate, HocVanApproveDate,
				 CreatedDate, UserId, PortalId, Xoa,
				 isspy, dongyguithongtin, Indirect)
			VALUES
				(@Hotendem, @Ten, @Sodienthoai, @Email, @Diachi, @Ngaysinh, @TinhId,
				 0, 0, 0, 0,
				 0, 0, 0,
				 GETDATE(), GETDATE(), GETDATE(),
				 GETDATE(), GETDATE(),
				 GETDATE(), 0, @PortalId, 0,
				 0, 0, 0);

			SET @StudentId = SCOPE_IDENTITY();

			-- Generate code: {CatCode}{YY}{MM}{StudentId}
			-- Example: HE2607150993
			SET @StudentCode =
				@CatCode
				+ RIGHT(CAST(YEAR(GETDATE()) AS VARCHAR(4)), 2)
				+ RIGHT('0' + CAST(MONTH(GETDATE()) AS VARCHAR(2)), 2)
				+ CAST(@StudentId AS VARCHAR(20));

			-- Save Code to Student_Info
			UPDATE Student_Info SET Code = @StudentCode WHERE id = @StudentId;
		END

		-- ══════════════════════════════════════════════════════════════════
		-- 3. Check for duplicate event registration
		-- ══════════════════════════════════════════════════════════════════
		DECLARE @ExistingRegistrationId INT = NULL;

		SELECT @ExistingRegistrationId = id
		FROM   NV_Events_Student
		WHERE  StudentId  = @StudentId
		  AND  EventId    = @EventId
		  AND  EventCatId = @EventCatId;

		IF @ExistingRegistrationId IS NOT NULL
		BEGIN
			-- ══════════════════════════════════════════════════════════════
			-- 4a. Registration exists → UPDATE it
			-- ══════════════════════════════════════════════════════════════
			UPDATE NV_Events_Student
			SET
				StudentCode = @StudentCode,
				CreatedDate = GETDATE(),
				PortalId    = @PortalId
			WHERE id = @ExistingRegistrationId;

			SET @IsDuplicate = 1;
		END
		ELSE
		BEGIN
			-- ══════════════════════════════════════════════════════════════
			-- 4b. Registration does not exist → INSERT new
			-- ══════════════════════════════════════════════════════════════
			INSERT INTO NV_Events_Student
				(EventId, EventCatId, StudentId, StudentCode,
				 Source, Nguon, CreatedDate, PortalId, nguontutao)
			VALUES
				(@EventId, @EventCatId, @StudentId, @StudentCode,
				 8, N'WEBSITE', GETDATE(), @PortalId, N'WEBSITE');

			SET @IsDuplicate = 0;
		END

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO
