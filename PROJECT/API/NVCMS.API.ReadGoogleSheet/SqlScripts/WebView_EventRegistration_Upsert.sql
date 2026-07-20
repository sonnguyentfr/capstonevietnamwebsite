-- ============================================================
-- SP: WebView_EventRegistration_Upsert
-- Database: CapstoneVietnam_old
-- Version: 3 — StudentCode dạng {CatCode}{YY}{MM}{StudentId}
--              Ví dụ: HE2607150993
--                HE     = NV_Events_Cat.Code (theo EventCatId)
--                26     = năm (2 chữ số)
--                07     = tháng (2 chữ số)
--                150993 = Student_Info.id
-- ============================================================
IF OBJECT_ID('dbo.WebView_EventRegistration_Upsert', 'P') IS NOT NULL
    DROP PROCEDURE dbo.WebView_EventRegistration_Upsert;
GO

CREATE PROCEDURE [dbo].[WebView_EventRegistration_Upsert]
    -- ── Thông tin học sinh ────────────────────────────────────────────────
    @Hotendem       NVARCHAR(200),
    @Ten            NVARCHAR(100),
    @Sodienthoai    NVARCHAR(30),
    @Email          NVARCHAR(200),
    @Diachi         NVARCHAR(500),
    @Ngaysinh       DATETIME      = NULL,
    @TinhId         INT           = NULL,
    @PortalId       INT,
    -- ── Thông tin sự kiện ─────────────────────────────────────────────────
    @EventId        INT,
    @EventCatId     INT,
    -- ── Output ────────────────────────────────────────────────────────────
    @StudentId      INT           OUTPUT,
    @StudentCode    NVARCHAR(50)  OUTPUT,
    @IsDuplicate    BIT           OUTPUT    -- 1 = đã đăng ký sự kiện này trước đó
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        -- ══════════════════════════════════════════════════════════════════
        -- 0. Lấy CatCode từ NV_Events_Cat
        --    Fallback 'C' nếu Code NULL hoặc rỗng
        -- ══════════════════════════════════════════════════════════════════
        DECLARE @CatCode NVARCHAR(20);
        SELECT @CatCode = NULLIF(LTRIM(RTRIM(ISNULL(Code, ''))), '')
        FROM   NV_Events_Cat
        WHERE  id = @EventCatId;

        IF @CatCode IS NULL SET @CatCode = 'C';

        -- ══════════════════════════════════════════════════════════════════
        -- 1. Tìm student theo SĐT (ưu tiên) hoặc Email
        -- ══════════════════════════════════════════════════════════════════
        SET @StudentId = NULL;

        SELECT TOP 1 @StudentId = id
        FROM   Student_Info
        WHERE  (Sodienthoai = @Sodienthoai AND @Sodienthoai <> '')
            OR (Email = @Email AND @Email <> '')
        ORDER BY
            CASE WHEN Sodienthoai = @Sodienthoai THEN 0 ELSE 1 END,
            id ASC;

        -- ══════════════════════════════════════════════════════════════════
        -- 2a. Student đã tồn tại → UPDATE thông tin cơ bản
        --     - Các trường text: chỉ điền vào ô đang trống
        --     - Ngaysinh / Tinh: chỉ điền nếu đang NULL/0
        --     - Diachi: luôn update nếu user truyền giá trị
        -- ══════════════════════════════════════════════════════════════════
        IF @StudentId IS NOT NULL
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
            WHERE id = @StudentId;
        END

        -- ══════════════════════════════════════════════════════════════════
        -- 2b. Student chưa tồn tại → INSERT mới
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
        END

        -- ══════════════════════════════════════════════════════════════════
        -- 3. Sinh StudentCode theo format: {CatCode}{YY}{MM}{StudentId}
        --    Ví dụ: HE2607150993
        --      HE     = NV_Events_Cat.Code
        --      26     = RIGHT(YEAR(GETDATE()), 2)
        --      07     = RIGHT('0'+MONTH, 2)
        --      150993 = Student_Info.id (không padding)
        -- ══════════════════════════════════════════════════════════════════
        SET @StudentCode =
            @CatCode
            + RIGHT(CAST(YEAR(GETDATE()) AS VARCHAR(4)), 2)
            + RIGHT('0' + CAST(MONTH(GETDATE()) AS VARCHAR(2)), 2)
            + CAST(@StudentId AS VARCHAR(20));

        -- Ghi Code vào Student_Info
        UPDATE Student_Info SET Code = @StudentCode WHERE id = @StudentId;

        -- ══════════════════════════════════════════════════════════════════
        -- 4. Kiểm tra duplicate đăng ký sự kiện
        -- ══════════════════════════════════════════════════════════════════
        IF EXISTS (
            SELECT 1
            FROM   NV_Events_Student
            WHERE  StudentId  = @StudentId
              AND  EventId    = @EventId
              AND  EventCatId = @EventCatId
        )
        BEGIN
            SET @IsDuplicate = 1;
        END
        ELSE
        BEGIN
            -- ══════════════════════════════════════════════════════════════
            -- 5. Insert đăng ký sự kiện mới
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

IF OBJECT_ID('dbo.WebView_EventRegistration_Upsert', 'P') IS NOT NULL
    DROP PROCEDURE dbo.WebView_EventRegistration_Upsert;
GO

CREATE PROCEDURE [dbo].[WebView_EventRegistration_Upsert]
    -- ── Thông tin học sinh ────────────────────────────────────────────────
    @Hotendem       NVARCHAR(200),
    @Ten            NVARCHAR(100),
    @Sodienthoai    NVARCHAR(30),
    @Email          NVARCHAR(200),
    @Diachi         NVARCHAR(500),
    @Ngaysinh       DATETIME      = NULL,   -- nullable: truyền NULL nếu không có
    @TinhId         INT           = NULL,   -- FK NVCMS_DM_DVHanhChinh_Tinh.id; NULL nếu không chọn
    @PortalId       INT,
    -- ── Thông tin sự kiện ─────────────────────────────────────────────────
    @EventId        INT,
    @EventCatId     INT,
    -- ── Output ────────────────────────────────────────────────────────────
    @StudentId      INT          OUTPUT,
    @StudentCode    NVARCHAR(50) OUTPUT,
    @IsDuplicate    BIT          OUTPUT    -- 1 = đã đăng ký sự kiện này trước đó
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    BEGIN TRY

        -- ══════════════════════════════════════════════════════════════════
        -- 1. Tìm student theo SĐT (ưu tiên) hoặc Email
        -- ══════════════════════════════════════════════════════════════════
        SET @StudentId = NULL;

        SELECT TOP 1 @StudentId = id
        FROM   Student_Info
        WHERE  (Sodienthoai = @Sodienthoai AND @Sodienthoai <> '')
            OR (Email = @Email AND @Email <> '')
        ORDER BY
            CASE WHEN Sodienthoai = @Sodienthoai THEN 0 ELSE 1 END,
            id ASC;

        -- ══════════════════════════════════════════════════════════════════
        -- 2a. Student đã tồn tại → UPDATE thông tin
        --     - Trường văn bản: chỉ điền vào ô đang trống (không xóa CRM data)
        --     - Ngaysinh : điền nếu đang NULL
        --     - Tinh     : điền nếu đang NULL hoặc = 0
        --     - Diachi   : luôn cập nhật nếu user truyền giá trị mới
        -- ══════════════════════════════════════════════════════════════════
        IF @StudentId IS NOT NULL
        BEGIN
            UPDATE Student_Info
            SET
                Hotendem    = CASE WHEN ISNULL(Hotendem,    '') = '' THEN @Hotendem    ELSE Hotendem    END,
                Ten         = CASE WHEN ISNULL(Ten,         '') = '' THEN @Ten         ELSE Ten         END,
                Sodienthoai = CASE WHEN ISNULL(Sodienthoai, '') = '' THEN @Sodienthoai ELSE Sodienthoai END,
                Email       = CASE WHEN ISNULL(Email,       '') = '' THEN @Email       ELSE Email       END,
                Diachi      = CASE WHEN @Diachi <> ''
                                   THEN @Diachi
                                   ELSE Diachi END,
                Ngaysinh    = CASE WHEN @Ngaysinh IS NOT NULL AND Ngaysinh IS NULL
                                   THEN @Ngaysinh
                                   ELSE Ngaysinh END,
                Tinh        = CASE WHEN @TinhId IS NOT NULL AND ISNULL(Tinh, 0) = 0
                                   THEN @TinhId
                                   ELSE Tinh END
            WHERE id = @StudentId;

            SELECT @StudentCode = ISNULL(Code, 'C' + RIGHT('000000' + CAST(@StudentId AS VARCHAR(10)), 6))
            FROM   Student_Info
            WHERE  id = @StudentId;
        END

        -- ══════════════════════════════════════════════════════════════════
        -- 2b. Student chưa tồn tại → INSERT mới + sinh Code C000001
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

            -- Code dạng: C000001
            SET @StudentCode = 'C' + RIGHT('000000' + CAST(@StudentId AS VARCHAR(10)), 6);

            UPDATE Student_Info SET Code = @StudentCode WHERE id = @StudentId;
        END

        -- ══════════════════════════════════════════════════════════════════
        -- 3. Kiểm tra duplicate đăng ký sự kiện
        -- ══════════════════════════════════════════════════════════════════
        IF EXISTS (
            SELECT 1
            FROM   NV_Events_Student
            WHERE  StudentId  = @StudentId
              AND  EventId    = @EventId
              AND  EventCatId = @EventCatId
        )
        BEGIN
            SET @IsDuplicate = 1;
        END
        ELSE
        BEGIN
            -- ══════════════════════════════════════════════════════════════
            -- 4. Insert đăng ký sự kiện mới
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
