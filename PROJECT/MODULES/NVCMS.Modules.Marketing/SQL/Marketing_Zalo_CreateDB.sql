-- =============================================
-- Marketing Zalo Campaign - Database Script
-- Tables: Marketing_Zalo_Campaign, Marketing_Zalo_ListSdt
-- Connection: SiteSqlServerV1
-- =============================================

-- ===========================================
-- TABLE: Marketing_Zalo_Campaign
-- ===========================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Marketing_Zalo_Campaign' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Marketing_Zalo_Campaign] (
        [Id]          INT           IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Title]       NVARCHAR(500) NOT NULL,
        [Description] NVARCHAR(MAX) NULL,
        [Status]      TINYINT       NOT NULL DEFAULT 0,   -- 0=Draft, 1=Active, 2=Sent
        [CreatedDate] DATETIME      NOT NULL DEFAULT GETDATE(),
        [UserId]      INT           NOT NULL DEFAULT 0,
        [PortalId]    INT           NOT NULL DEFAULT 0
    )
    PRINT 'Created table: Marketing_Zalo_Campaign'
END
ELSE
    PRINT 'Table already exists: Marketing_Zalo_Campaign'
GO

-- ===========================================
-- TABLE: Marketing_Zalo_ListSdt
-- ===========================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Marketing_Zalo_ListSdt' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Marketing_Zalo_ListSdt] (
        [Id]                        INT           IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Marketing_Zalo_CampaignId] INT           NOT NULL,
        [PhoneRaw]                  NVARCHAR(20)  NULL,        -- SDT goc khi nhap
        [Phone]                     NVARCHAR(20)  NOT NULL,    -- SDT da chuan hoa (84xxx)
        [Status]                    TINYINT       NOT NULL DEFAULT 0,  -- 0=Pending, 1=Sent, 2=Failed
        [SendCount]                 INT           NOT NULL DEFAULT 0,
        [CreatedDate]               DATETIME      NOT NULL DEFAULT GETDATE(),
        [UserId]                    INT           NOT NULL DEFAULT 0,
        [PortalId]                  INT           NOT NULL DEFAULT 0,
        CONSTRAINT [FK_Marketing_Zalo_ListSdt_Campaign]
            FOREIGN KEY ([Marketing_Zalo_CampaignId])
            REFERENCES [dbo].[Marketing_Zalo_Campaign]([Id])
            ON DELETE CASCADE
    )
    PRINT 'Created table: Marketing_Zalo_ListSdt'
END
ELSE
    PRINT 'Table already exists: Marketing_Zalo_ListSdt'
GO

-- INDEX
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Marketing_Zalo_ListSdt_CampaignId')
    CREATE INDEX [IX_Marketing_Zalo_ListSdt_CampaignId]
        ON [dbo].[Marketing_Zalo_ListSdt] ([Marketing_Zalo_CampaignId])
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='IX_Marketing_Zalo_ListSdt_Phone')
    CREATE INDEX [IX_Marketing_Zalo_ListSdt_Phone]
        ON [dbo].[Marketing_Zalo_ListSdt] ([Marketing_Zalo_CampaignId], [Phone])
GO

-- =============================================
-- SP: Marketing_Zalo_Campaign_Insert
-- =============================================
IF OBJECT_ID('Marketing_Zalo_Campaign_Insert','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_Campaign_Insert
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_Campaign_Insert]
    @Title       NVARCHAR(500),
    @Description NVARCHAR(MAX),
    @Status      TINYINT,
    @CreatedDate DATETIME,
    @UserId      INT,
    @PortalId    INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO [dbo].[Marketing_Zalo_Campaign]
        ([Title],[Description],[Status],[CreatedDate],[UserId],[PortalId])
    VALUES
        (@Title, @Description, @Status, @CreatedDate, @UserId, @PortalId)
    SELECT SCOPE_IDENTITY() AS NewId
END
GO

-- =============================================
-- SP: Marketing_Zalo_Campaign_Update
-- =============================================
IF OBJECT_ID('Marketing_Zalo_Campaign_Update','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_Campaign_Update
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_Campaign_Update]
    @Id          INT,
    @Title       NVARCHAR(500),
    @Description NVARCHAR(MAX),
    @Status      TINYINT,
    @UserId      INT,
    @PortalId    INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[Marketing_Zalo_Campaign]
    SET [Title]       = @Title,
        [Description] = @Description,
        [Status]      = @Status,
        [UserId]      = @UserId
    WHERE [Id] = @Id AND [PortalId] = @PortalId
END
GO

-- =============================================
-- SP: Marketing_Zalo_Campaign_Delete
-- =============================================
IF OBJECT_ID('Marketing_Zalo_Campaign_Delete','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_Campaign_Delete
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_Campaign_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[Marketing_Zalo_Campaign] WHERE [Id] = @Id
END
GO

-- =============================================
-- SP: Marketing_Zalo_Campaign_SelectByID
-- =============================================
IF OBJECT_ID('Marketing_Zalo_Campaign_SelectByID','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_Campaign_SelectByID
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_Campaign_SelectByID]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        c.*,
        (SELECT COUNT(*) FROM [dbo].[Marketing_Zalo_ListSdt]
         WHERE [Marketing_Zalo_CampaignId] = c.[Id]) AS TotalSdt
    FROM [dbo].[Marketing_Zalo_Campaign] c
    WHERE c.[Id] = @Id
END
GO

-- =============================================
-- SP: Marketing_Zalo_Campaign_SelectAll
-- =============================================
IF OBJECT_ID('Marketing_Zalo_Campaign_SelectAll','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_Campaign_SelectAll
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_Campaign_SelectAll]
    @PortalId INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        c.*,
        (SELECT COUNT(*) FROM [dbo].[Marketing_Zalo_ListSdt]
         WHERE [Marketing_Zalo_CampaignId] = c.[Id]) AS TotalSdt
    FROM [dbo].[Marketing_Zalo_Campaign] c
    WHERE c.[PortalId] = @PortalId
    ORDER BY c.[Id] DESC
END
GO

-- =============================================
-- SP: Marketing_Zalo_ListSdt_Insert
-- Kiem tra trung truoc khi insert
-- Tra ve: Result = NewId hoac -1 neu trung
-- =============================================
IF OBJECT_ID('Marketing_Zalo_ListSdt_Insert','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_ListSdt_Insert
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_ListSdt_Insert]
    @Marketing_Zalo_CampaignId INT,
    @PhoneRaw                  NVARCHAR(20),
    @Phone                     NVARCHAR(20),   -- Da chuan hoa (84xxx)
    @Status                    TINYINT,
    @CreatedDate               DATETIME,
    @UserId                    INT,
    @PortalId                  INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiem tra trung SDT trong cung campaign
    IF EXISTS (
        SELECT 1 FROM [dbo].[Marketing_Zalo_ListSdt]
        WHERE [Marketing_Zalo_CampaignId] = @Marketing_Zalo_CampaignId
          AND [Phone] = @Phone
    )
    BEGIN
        SELECT -1 AS Result, N'So dien thoai da ton tai trong chien dich nay' AS Message
        RETURN
    END

    INSERT INTO [dbo].[Marketing_Zalo_ListSdt]
        ([Marketing_Zalo_CampaignId],[PhoneRaw],[Phone],[Status],[SendCount],[CreatedDate],[UserId],[PortalId])
    VALUES
        (@Marketing_Zalo_CampaignId, @PhoneRaw, @Phone, @Status, 0, @CreatedDate, @UserId, @PortalId)

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Result, N'Them thanh cong' AS Message
END
GO

-- =============================================
-- SP: Marketing_Zalo_ListSdt_InsertBulk
-- Insert nhieu SDT (comma-separated), bo qua trung
-- Tra ve: InsertCount, DupCount
-- =============================================
IF OBJECT_ID('Marketing_Zalo_ListSdt_InsertBulk','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_ListSdt_InsertBulk
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_ListSdt_InsertBulk]
    @Marketing_Zalo_CampaignId INT,
    @PhoneList                 NVARCHAR(MAX),   -- Danh sach SDT chuan hoa, cach nhau dau phay
    @UserId                    INT,
    @PortalId                  INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME = GETDATE()
    DECLARE @InsertCount INT = 0

    ;WITH SplitPhones AS (
        SELECT LTRIM(RTRIM(value)) AS Phone
        FROM STRING_SPLIT(@PhoneList, ',')
        WHERE LTRIM(RTRIM(value)) <> ''
    )
    INSERT INTO [dbo].[Marketing_Zalo_ListSdt]
        ([Marketing_Zalo_CampaignId],[PhoneRaw],[Phone],[Status],[SendCount],[CreatedDate],[UserId],[PortalId])
    SELECT
        @Marketing_Zalo_CampaignId,
        sp.Phone,   -- PhoneRaw = Phone vi da duoc chuan hoa truoc khi truyen vao
        sp.Phone,
        0,
        0,
        @Now,
        @UserId,
        @PortalId
    FROM SplitPhones sp
    WHERE NOT EXISTS (
        SELECT 1 FROM [dbo].[Marketing_Zalo_ListSdt]
        WHERE [Marketing_Zalo_CampaignId] = @Marketing_Zalo_CampaignId
          AND [Phone] = sp.Phone
    )

    SET @InsertCount = @@ROWCOUNT

    SELECT
        @InsertCount AS InsertCount,
        (
            SELECT COUNT(DISTINCT LTRIM(RTRIM(value)))
            FROM STRING_SPLIT(@PhoneList, ',')
            WHERE LTRIM(RTRIM(value)) <> ''
        ) - @InsertCount AS DupCount
END
GO

-- =============================================
-- SP: Marketing_Zalo_ListSdt_Delete
-- =============================================
IF OBJECT_ID('Marketing_Zalo_ListSdt_Delete','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_ListSdt_Delete
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_ListSdt_Delete]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[Marketing_Zalo_ListSdt] WHERE [Id] = @Id
END
GO

-- =============================================
-- SP: Marketing_Zalo_ListSdt_DeleteByCampaignId
-- =============================================
IF OBJECT_ID('Marketing_Zalo_ListSdt_DeleteByCampaignId','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_ListSdt_DeleteByCampaignId
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_ListSdt_DeleteByCampaignId]
    @Marketing_Zalo_CampaignId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[Marketing_Zalo_ListSdt]
    WHERE [Marketing_Zalo_CampaignId] = @Marketing_Zalo_CampaignId
END
GO

-- =============================================
-- SP: Marketing_Zalo_ListSdt_SelectByID
-- =============================================
IF OBJECT_ID('Marketing_Zalo_ListSdt_SelectByID','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_ListSdt_SelectByID
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_ListSdt_SelectByID]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM [dbo].[Marketing_Zalo_ListSdt] WHERE [Id] = @Id
END
GO

-- =============================================
-- SP: Marketing_Zalo_ListSdt_SelectAll
-- Co phan trang, filter theo phone/status
-- =============================================
IF OBJECT_ID('Marketing_Zalo_ListSdt_SelectAll','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_ListSdt_SelectAll
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_ListSdt_SelectAll]
    @Marketing_Zalo_CampaignId INT,
    @KeySearch                  NVARCHAR(100) = '',
    @Status                     INT           = -1,  -- -1 = tat ca
    @PageIndex                  INT           = 0,
    @PageSize                   INT           = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = @PageIndex * @PageSize

    SELECT
        s.*,
        COUNT(*) OVER() AS TotalRecords
    FROM [dbo].[Marketing_Zalo_ListSdt] s
    WHERE s.[Marketing_Zalo_CampaignId] = @Marketing_Zalo_CampaignId
      AND (@KeySearch = '' OR s.[Phone] LIKE '%' + @KeySearch + '%'
                           OR s.[PhoneRaw] LIKE '%' + @KeySearch + '%')
      AND (@Status = -1 OR s.[Status] = @Status)
    ORDER BY s.[Id] DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO

-- =============================================
-- SP: Marketing_Zalo_ListSdt_UpdateStatus
-- =============================================
IF OBJECT_ID('Marketing_Zalo_ListSdt_UpdateStatus','P') IS NOT NULL DROP PROCEDURE Marketing_Zalo_ListSdt_UpdateStatus
GO
CREATE PROCEDURE [dbo].[Marketing_Zalo_ListSdt_UpdateStatus]
    @Id     INT,
    @Status TINYINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[Marketing_Zalo_ListSdt]
    SET [Status]    = @Status,
        [SendCount] = [SendCount] + CASE WHEN @Status = 1 THEN 1 ELSE 0 END
    WHERE [Id] = @Id
END
GO

PRINT 'Done! All Zalo Campaign objects created successfully.'
