/* =============================================================================
   Zalo OA Chat - schema + stored procedures
   Database : [CapstoneVietNamWeb]  (ReadGoogleSheet: ConnectionStrings:DefaultConnection
                                     DNN: SiteSqlServer)
   Script chạy lại nhiều lần được (idempotent):
     - Bảng / index chỉ tạo khi chưa có.
     - Chỉ DROP + CREATE lại các stored procedure ZaloOA_* do chính script này tạo.
     - KHÔNG đụng tới bảng/procedure đang có (Zalo_Token, ZNS_*, Marketing_*...).
   Tất cả thời gian lưu theo UTC (GETUTCDATE()).
   ============================================================================= */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* =============================================================================
   TABLE: ZaloOA_Customer - người dùng Zalo đã tương tác với OA
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_Customer', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZaloOA_Customer
    (
        Id                  BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ZaloOA_Customer PRIMARY KEY,
        OAId                VARCHAR(50)    NOT NULL,
        ZaloUserId          VARCHAR(50)    NOT NULL,          -- user_id theo OA (chuỗi số 16-19 ký tự)
        ZaloUserIdByApp     VARCHAR(50)    NULL,
        DisplayName         NVARCHAR(250)  NULL,
        UserAlias           NVARCHAR(250)  NULL,
        AvatarUrl           NVARCHAR(1000) NULL,
        IsFollower          BIT            NULL,
        IsSensitive         BIT            NULL,              -- Zalo: tài khoản dưới 18 tuổi
        SharedName          NVARCHAR(250)  NULL,              -- shared_info / user_submit_info (chỉ có khi user tự chia sẻ)
        SharedPhone         VARCHAR(30)    NULL,
        SharedAddress       NVARCHAR(500)  NULL,
        SharedDob           NVARCHAR(20)   NULL,              -- giữ nguyên chuỗi Zalo trả về (dd/MM/yyyy)
        Gender              NVARCHAR(20)   NULL,              -- chỉ có từ user_submit_info
        TagsJson            NVARCHAR(MAX)  NULL,
        NotesJson           NVARCHAR(MAX)  NULL,
        Status              VARCHAR(20)    NOT NULL CONSTRAINT DF_ZaloOA_Customer_Status DEFAULT('ACTIVE'),
        FirstInteractionAt  DATETIME       NULL,
        LastInteractionAt   DATETIME       NULL,              -- lần cuối KHÁCH chủ động tương tác
        LastProfileSyncAt   DATETIME       NULL,
        ProfileRawJson      NVARCHAR(MAX)  NULL,
        IsDeleted           BIT            NOT NULL CONSTRAINT DF_ZaloOA_Customer_IsDeleted DEFAULT(0),
        CreatedAt           DATETIME       NOT NULL CONSTRAINT DF_ZaloOA_Customer_CreatedAt DEFAULT(GETUTCDATE()),
        UpdatedAt           DATETIME       NOT NULL CONSTRAINT DF_ZaloOA_Customer_UpdatedAt DEFAULT(GETUTCDATE())
    );
    PRINT 'Created table: ZaloOA_Customer';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_ZaloOA_Customer_OAId_ZaloUserId' AND object_id = OBJECT_ID('dbo.ZaloOA_Customer'))
    CREATE UNIQUE INDEX UX_ZaloOA_Customer_OAId_ZaloUserId ON dbo.ZaloOA_Customer(OAId, ZaloUserId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Customer_LastInteractionAt' AND object_id = OBJECT_ID('dbo.ZaloOA_Customer'))
    CREATE INDEX IX_ZaloOA_Customer_LastInteractionAt ON dbo.ZaloOA_Customer(LastInteractionAt DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Customer_DisplayName' AND object_id = OBJECT_ID('dbo.ZaloOA_Customer'))
    CREATE INDEX IX_ZaloOA_Customer_DisplayName ON dbo.ZaloOA_Customer(DisplayName);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Customer_SharedPhone' AND object_id = OBJECT_ID('dbo.ZaloOA_Customer'))
    CREATE INDEX IX_ZaloOA_Customer_SharedPhone ON dbo.ZaloOA_Customer(SharedPhone) WHERE SharedPhone IS NOT NULL;
GO

/* =============================================================================
   TABLE: ZaloOA_Conversation - mỗi khách đúng 1 hội thoại (đóng/mở bằng tay)
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_Conversation', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZaloOA_Conversation
    (
        Id                     BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ZaloOA_Conversation PRIMARY KEY,
        OAId                   VARCHAR(50)   NOT NULL,
        CustomerId             BIGINT        NOT NULL,
        Status                 VARCHAR(20)   NOT NULL CONSTRAINT DF_ZaloOA_Conversation_Status DEFAULT('OPEN'),  -- OPEN / PENDING / CLOSED
        AssignedToUserId       INT           NULL,               -- DNN UserId (mở rộng: assign CSKH)
        UnreadCount            INT           NOT NULL CONSTRAINT DF_ZaloOA_Conversation_UnreadCount DEFAULT(0),
        LastMessageId          BIGINT        NULL,
        LastMessageAt          DATETIME      NULL,
        LastMessagePreview     NVARCHAR(500) NULL,
        LastMessageSenderType  VARCHAR(20)   NULL,
        LastCustomerMessageAt  DATETIME      NULL,               -- dùng để cảnh báo cửa sổ 48h / 7 ngày của Zalo
        LastReadAt             DATETIME      NULL,
        LastReadByUserId       INT           NULL,
        StartedAt              DATETIME      NOT NULL CONSTRAINT DF_ZaloOA_Conversation_StartedAt DEFAULT(GETUTCDATE()),
        ClosedAt               DATETIME      NULL,
        ClosedByUserId         INT           NULL,
        RowVer                 ROWVERSION    NOT NULL,
        CreatedAt              DATETIME      NOT NULL CONSTRAINT DF_ZaloOA_Conversation_CreatedAt DEFAULT(GETUTCDATE()),
        UpdatedAt              DATETIME      NOT NULL CONSTRAINT DF_ZaloOA_Conversation_UpdatedAt DEFAULT(GETUTCDATE()),
        CONSTRAINT FK_ZaloOA_Conversation_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.ZaloOA_Customer(Id)
    );
    PRINT 'Created table: ZaloOA_Conversation';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_ZaloOA_Conversation_CustomerId' AND object_id = OBJECT_ID('dbo.ZaloOA_Conversation'))
    CREATE UNIQUE INDEX UX_ZaloOA_Conversation_CustomerId ON dbo.ZaloOA_Conversation(CustomerId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Conversation_LastMessageAt' AND object_id = OBJECT_ID('dbo.ZaloOA_Conversation'))
    CREATE INDEX IX_ZaloOA_Conversation_LastMessageAt ON dbo.ZaloOA_Conversation(LastMessageAt DESC) INCLUDE (CustomerId, Status, UnreadCount);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Conversation_Status_LastMessageAt' AND object_id = OBJECT_ID('dbo.ZaloOA_Conversation'))
    CREATE INDEX IX_ZaloOA_Conversation_Status_LastMessageAt ON dbo.ZaloOA_Conversation(Status, LastMessageAt DESC) INCLUDE (CustomerId, UnreadCount);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Conversation_AssignedTo' AND object_id = OBJECT_ID('dbo.ZaloOA_Conversation'))
    CREATE INDEX IX_ZaloOA_Conversation_AssignedTo ON dbo.ZaloOA_Conversation(AssignedToUserId, LastMessageAt DESC) WHERE AssignedToUserId IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Conversation_Unread' AND object_id = OBJECT_ID('dbo.ZaloOA_Conversation'))
    CREATE INDEX IX_ZaloOA_Conversation_Unread ON dbo.ZaloOA_Conversation(UnreadCount) WHERE UnreadCount > 0;
GO

/* =============================================================================
   TABLE: ZaloOA_WebhookEvent - lưu nguyên event trước khi xử lý (chống trùng)
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_WebhookEvent', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZaloOA_WebhookEvent
    (
        Id              BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ZaloOA_WebhookEvent PRIMARY KEY,
        DedupKey        CHAR(64)       NOT NULL,           -- SHA-256 hex
        EventName       VARCHAR(100)   NOT NULL,
        OAId            VARCHAR(50)    NULL,
        ZaloUserId      VARCHAR(50)    NULL,
        ZaloMessageId   VARCHAR(100)   NULL,
        EventTimestamp  BIGINT         NULL,               -- ms, lấy từ payload
        RawPayload      NVARCHAR(MAX)  NOT NULL,
        SignatureValid  BIT            NOT NULL,
        ProcessStatus   VARCHAR(20)    NOT NULL CONSTRAINT DF_ZaloOA_WebhookEvent_ProcessStatus DEFAULT('PENDING'), -- PENDING / PROCESSED / FAILED / IGNORED
        RetryCount      INT            NOT NULL CONSTRAINT DF_ZaloOA_WebhookEvent_RetryCount DEFAULT(0),
        ErrorMessage    NVARCHAR(2000) NULL,
        ReceivedAt      DATETIME       NOT NULL CONSTRAINT DF_ZaloOA_WebhookEvent_ReceivedAt DEFAULT(GETUTCDATE()),
        ProcessedAt     DATETIME       NULL,
        UpdatedAt       DATETIME       NOT NULL CONSTRAINT DF_ZaloOA_WebhookEvent_UpdatedAt DEFAULT(GETUTCDATE())
    );
    PRINT 'Created table: ZaloOA_WebhookEvent';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_ZaloOA_WebhookEvent_DedupKey' AND object_id = OBJECT_ID('dbo.ZaloOA_WebhookEvent'))
    CREATE UNIQUE INDEX UX_ZaloOA_WebhookEvent_DedupKey ON dbo.ZaloOA_WebhookEvent(DedupKey);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_WebhookEvent_ProcessStatus' AND object_id = OBJECT_ID('dbo.ZaloOA_WebhookEvent'))
    CREATE INDEX IX_ZaloOA_WebhookEvent_ProcessStatus ON dbo.ZaloOA_WebhookEvent(ProcessStatus, ReceivedAt) WHERE ProcessStatus IN ('PENDING', 'FAILED');
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_WebhookEvent_ReceivedAt' AND object_id = OBJECT_ID('dbo.ZaloOA_WebhookEvent'))
    CREATE INDEX IX_ZaloOA_WebhookEvent_ReceivedAt ON dbo.ZaloOA_WebhookEvent(ReceivedAt);
GO

/* =============================================================================
   TABLE: ZaloOA_Message
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_Message', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZaloOA_Message
    (
        Id                  BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ZaloOA_Message PRIMARY KEY,
        ConversationId      BIGINT           NOT NULL,
        CustomerId          BIGINT           NOT NULL,
        Direction           VARCHAR(3)       NOT NULL,     -- IN (khách -> OA) / OUT (OA -> khách) / SYS
        SenderType          VARCHAR(20)      NOT NULL,     -- CUSTOMER / OA / AGENT / SYSTEM
        SenderId            VARCHAR(50)      NULL,
        ReceiverId          VARCHAR(50)      NULL,
        AgentUserId         INT              NULL,         -- DNN UserId khi nhân viên gửi từ website
        MessageType         VARCHAR(20)      NOT NULL,     -- TEXT / IMAGE / FILE / VIDEO / AUDIO / STICKER / LOCATION / LINK / CONTACT / EVENT / OTHER
        Content             NVARCHAR(MAX)    NULL,
        AttachmentsJson     NVARCHAR(MAX)    NULL,
        QuoteZaloMessageId  VARCHAR(100)     NULL,
        ZaloMessageId       VARCHAR(100)     NULL,
        ClientMessageId     UNIQUEIDENTIFIER NULL,         -- chống gửi 2 lần từ UI
        Status              VARCHAR(20)      NOT NULL,     -- PENDING / RECEIVED / SENT / DELIVERED / SEEN / FAILED / UNKNOWN
        ErrorCode           INT              NULL,
        ErrorMessage        NVARCHAR(2000)   NULL,
        SentAt              DATETIME         NOT NULL,     -- thời điểm tin nhắn phát sinh (UTC)
        ReceivedAt          DATETIME         NULL,         -- thời điểm hệ thống nhận (webhook)
        DeliveredAt         DATETIME         NULL,
        SeenAt              DATETIME         NULL,
        WebhookEventId      BIGINT           NULL,
        RawPayload          NVARCHAR(MAX)    NULL,         -- JSON webhook / lịch sử gốc
        ResponseJson        NVARCHAR(MAX)    NULL,         -- JSON Zalo trả về khi gửi
        CreatedAt           DATETIME         NOT NULL CONSTRAINT DF_ZaloOA_Message_CreatedAt DEFAULT(GETUTCDATE()),
        UpdatedAt           DATETIME         NOT NULL CONSTRAINT DF_ZaloOA_Message_UpdatedAt DEFAULT(GETUTCDATE()),
        CONSTRAINT FK_ZaloOA_Message_Conversation FOREIGN KEY (ConversationId) REFERENCES dbo.ZaloOA_Conversation(Id),
        CONSTRAINT FK_ZaloOA_Message_Customer     FOREIGN KEY (CustomerId)     REFERENCES dbo.ZaloOA_Customer(Id)
    );
    PRINT 'Created table: ZaloOA_Message';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_ZaloOA_Message_ZaloMessageId' AND object_id = OBJECT_ID('dbo.ZaloOA_Message'))
    CREATE UNIQUE INDEX UX_ZaloOA_Message_ZaloMessageId ON dbo.ZaloOA_Message(ZaloMessageId) WHERE ZaloMessageId IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_ZaloOA_Message_ClientMessageId' AND object_id = OBJECT_ID('dbo.ZaloOA_Message'))
    CREATE UNIQUE INDEX UX_ZaloOA_Message_ClientMessageId ON dbo.ZaloOA_Message(ClientMessageId) WHERE ClientMessageId IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Message_Conversation_SentAt' AND object_id = OBJECT_ID('dbo.ZaloOA_Message'))
    CREATE INDEX IX_ZaloOA_Message_Conversation_SentAt ON dbo.ZaloOA_Message(ConversationId, SentAt DESC, Id DESC);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Message_Conversation_UpdatedAt' AND object_id = OBJECT_ID('dbo.ZaloOA_Message'))
    CREATE INDEX IX_ZaloOA_Message_Conversation_UpdatedAt ON dbo.ZaloOA_Message(ConversationId, UpdatedAt);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Message_CustomerId' AND object_id = OBJECT_ID('dbo.ZaloOA_Message'))
    CREATE INDEX IX_ZaloOA_Message_CustomerId ON dbo.ZaloOA_Message(CustomerId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Message_Direction_Id' AND object_id = OBJECT_ID('dbo.ZaloOA_Message'))
    CREATE INDEX IX_ZaloOA_Message_Direction_Id ON dbo.ZaloOA_Message(Direction, Id DESC) INCLUDE (ConversationId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ZaloOA_Message_Status' AND object_id = OBJECT_ID('dbo.ZaloOA_Message'))
    CREATE INDEX IX_ZaloOA_Message_Status ON dbo.ZaloOA_Message(Status) WHERE Status IN ('PENDING', 'FAILED');
GO

/* =============================================================================
   STORED PROCEDURES - Customer
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_Customer_Upsert', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Customer_Upsert;
GO
CREATE PROCEDURE dbo.ZaloOA_Customer_Upsert
    @OAId            VARCHAR(50),
    @ZaloUserId      VARCHAR(50),
    @ZaloUserIdByApp VARCHAR(50) = NULL,
    @InteractionAt   DATETIME    = NULL,     -- NULL khi sự kiện không do khách chủ động (vd oa_send_*)
    @DisplayName     NVARCHAR(250)  = NULL,  -- chỉ điền khi đang trống (vd từ lịch sử chat)
    @AvatarUrl       NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Now DATETIME = GETUTCDATE();
    DECLARE @Id BIGINT;
    DECLARE @IsNew BIT = 0;

    BEGIN TRAN;

    SELECT @Id = Id
    FROM dbo.ZaloOA_Customer WITH (UPDLOCK, HOLDLOCK)
    WHERE OAId = @OAId AND ZaloUserId = @ZaloUserId;

    IF @Id IS NULL
    BEGIN
        INSERT INTO dbo.ZaloOA_Customer
            (OAId, ZaloUserId, ZaloUserIdByApp, DisplayName, AvatarUrl, FirstInteractionAt, LastInteractionAt, CreatedAt, UpdatedAt)
        VALUES
            (@OAId, @ZaloUserId, @ZaloUserIdByApp, @DisplayName, @AvatarUrl, @InteractionAt, @InteractionAt, @Now, @Now);

        SET @Id = CAST(SCOPE_IDENTITY() AS BIGINT);
        SET @IsNew = 1;
    END
    ELSE
    BEGIN
        UPDATE dbo.ZaloOA_Customer
        SET ZaloUserIdByApp    = COALESCE(@ZaloUserIdByApp, ZaloUserIdByApp),
            DisplayName        = COALESCE(DisplayName, @DisplayName),
            AvatarUrl          = COALESCE(AvatarUrl, @AvatarUrl),
            FirstInteractionAt = CASE WHEN @InteractionAt IS NOT NULL AND (FirstInteractionAt IS NULL OR @InteractionAt < FirstInteractionAt)
                                      THEN @InteractionAt ELSE FirstInteractionAt END,
            LastInteractionAt  = CASE WHEN @InteractionAt IS NOT NULL AND (LastInteractionAt IS NULL OR @InteractionAt > LastInteractionAt)
                                      THEN @InteractionAt ELSE LastInteractionAt END,
            IsDeleted          = 0,
            UpdatedAt          = @Now
        WHERE Id = @Id;
    END

    COMMIT;

    SELECT c.*, @IsNew AS IsNew
    FROM dbo.ZaloOA_Customer c
    WHERE c.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Customer_UpdateProfile', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Customer_UpdateProfile;
GO
CREATE PROCEDURE dbo.ZaloOA_Customer_UpdateProfile
    @Id              BIGINT,
    @ZaloUserIdByApp VARCHAR(50)    = NULL,
    @DisplayName     NVARCHAR(250)  = NULL,
    @UserAlias       NVARCHAR(250)  = NULL,
    @AvatarUrl       NVARCHAR(1000) = NULL,
    @IsFollower      BIT            = NULL,
    @IsSensitive     BIT            = NULL,
    @SharedName      NVARCHAR(250)  = NULL,
    @SharedPhone     VARCHAR(30)    = NULL,
    @SharedAddress   NVARCHAR(500)  = NULL,
    @SharedDob       NVARCHAR(20)   = NULL,
    @TagsJson        NVARCHAR(MAX)  = NULL,
    @NotesJson       NVARCHAR(MAX)  = NULL,
    @ProfileRawJson  NVARCHAR(MAX)  = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME = GETUTCDATE();

    UPDATE dbo.ZaloOA_Customer
    SET ZaloUserIdByApp   = COALESCE(@ZaloUserIdByApp, ZaloUserIdByApp),
        DisplayName       = COALESCE(@DisplayName, DisplayName),
        UserAlias         = COALESCE(@UserAlias, UserAlias),
        AvatarUrl         = COALESCE(@AvatarUrl, AvatarUrl),
        IsFollower        = COALESCE(@IsFollower, IsFollower),
        IsSensitive       = COALESCE(@IsSensitive, IsSensitive),
        SharedName        = COALESCE(@SharedName, SharedName),
        SharedPhone       = COALESCE(@SharedPhone, SharedPhone),
        SharedAddress     = COALESCE(@SharedAddress, SharedAddress),
        SharedDob         = COALESCE(@SharedDob, SharedDob),
        TagsJson          = COALESCE(@TagsJson, TagsJson),
        NotesJson         = COALESCE(@NotesJson, NotesJson),
        ProfileRawJson    = COALESCE(@ProfileRawJson, ProfileRawJson),
        LastProfileSyncAt = @Now,
        UpdatedAt         = @Now
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS Affected;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Customer_UpdateSharedInfo', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Customer_UpdateSharedInfo;
GO
CREATE PROCEDURE dbo.ZaloOA_Customer_UpdateSharedInfo
    @Id            BIGINT,
    @SharedName    NVARCHAR(250) = NULL,
    @SharedPhone   VARCHAR(30)   = NULL,
    @SharedAddress NVARCHAR(500) = NULL,
    @SharedDob     NVARCHAR(20)  = NULL,
    @Gender        NVARCHAR(20)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ZaloOA_Customer
    SET SharedName    = COALESCE(@SharedName, SharedName),
        SharedPhone   = COALESCE(@SharedPhone, SharedPhone),
        SharedAddress = COALESCE(@SharedAddress, SharedAddress),
        SharedDob     = COALESCE(@SharedDob, SharedDob),
        Gender        = COALESCE(@Gender, Gender),
        UpdatedAt     = GETUTCDATE()
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS Affected;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Customer_SetFollower', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Customer_SetFollower;
GO
CREATE PROCEDURE dbo.ZaloOA_Customer_SetFollower
    @Id         BIGINT,
    @IsFollower BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ZaloOA_Customer
    SET IsFollower = @IsFollower,
        UpdatedAt  = GETUTCDATE()
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS Affected;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Customer_GetById', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Customer_GetById;
GO
CREATE PROCEDURE dbo.ZaloOA_Customer_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.*, cv.Id AS ConversationId
    FROM dbo.ZaloOA_Customer c
    LEFT JOIN dbo.ZaloOA_Conversation cv ON cv.CustomerId = c.Id
    WHERE c.Id = @Id AND c.IsDeleted = 0;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Customer_GetList', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Customer_GetList;
GO
CREATE PROCEDURE dbo.ZaloOA_Customer_GetList
    @Keyword    NVARCHAR(700) = N'',     -- đã escape ký tự LIKE ở tầng ứng dụng
    @IsFollower INT           = -1,      -- -1 tất cả, 0 / 1
    @FromDate   DATETIME      = NULL,    -- lọc LastInteractionAt
    @ToDate     DATETIME      = NULL,
    @SortBy     VARCHAR(30)   = 'LastInteractionAt',   -- LastInteractionAt / DisplayName / CreatedAt
    @SortDir    VARCHAR(4)    = 'DESC',
    @PageIndex  INT           = 0,
    @PageSize   INT           = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = @PageIndex * @PageSize;
    DECLARE @Like NVARCHAR(710) = N'%' + ISNULL(@Keyword, N'') + N'%';

    SELECT c.*, cv.Id AS ConversationId, COUNT(*) OVER() AS TotalRecords
    FROM dbo.ZaloOA_Customer c
    LEFT JOIN dbo.ZaloOA_Conversation cv ON cv.CustomerId = c.Id
    WHERE c.IsDeleted = 0
      AND (ISNULL(@Keyword, N'') = N''
           OR c.DisplayName LIKE @Like OR c.UserAlias LIKE @Like OR c.SharedName LIKE @Like
           OR c.SharedPhone LIKE @Like OR c.ZaloUserId LIKE @Like)
      AND (@IsFollower = -1 OR c.IsFollower = @IsFollower)
      AND (@FromDate IS NULL OR c.LastInteractionAt >= @FromDate)
      AND (@ToDate   IS NULL OR c.LastInteractionAt <  @ToDate)
    ORDER BY
        CASE WHEN @SortBy = 'DisplayName'       AND @SortDir = 'ASC'  THEN c.DisplayName END ASC,
        CASE WHEN @SortBy = 'DisplayName'       AND @SortDir = 'DESC' THEN c.DisplayName END DESC,
        CASE WHEN @SortBy = 'CreatedAt'         AND @SortDir = 'ASC'  THEN c.CreatedAt END ASC,
        CASE WHEN @SortBy = 'CreatedAt'         AND @SortDir = 'DESC' THEN c.CreatedAt END DESC,
        CASE WHEN @SortBy = 'LastInteractionAt' AND @SortDir = 'ASC'  THEN c.LastInteractionAt END ASC,
        CASE WHEN @SortBy = 'LastInteractionAt' AND @SortDir = 'DESC' THEN c.LastInteractionAt END DESC,
        c.Id DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

/* =============================================================================
   STORED PROCEDURES - Conversation
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_Conversation_GetOrCreate', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Conversation_GetOrCreate;
GO
CREATE PROCEDURE dbo.ZaloOA_Conversation_GetOrCreate
    @OAId           VARCHAR(50),
    @CustomerId     BIGINT,
    @ReopenIfClosed BIT = 0          -- 1 khi khách nhắn tin mới vào hội thoại đã đóng
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Now DATETIME = GETUTCDATE();
    DECLARE @Id BIGINT;
    DECLARE @Status VARCHAR(20);
    DECLARE @IsNew BIT = 0;

    BEGIN TRAN;

    SELECT @Id = Id, @Status = Status
    FROM dbo.ZaloOA_Conversation WITH (UPDLOCK, HOLDLOCK)
    WHERE CustomerId = @CustomerId;

    IF @Id IS NULL
    BEGIN
        INSERT INTO dbo.ZaloOA_Conversation (OAId, CustomerId, Status, StartedAt, CreatedAt, UpdatedAt)
        VALUES (@OAId, @CustomerId, 'OPEN', @Now, @Now, @Now);

        SET @Id = CAST(SCOPE_IDENTITY() AS BIGINT);
        SET @IsNew = 1;
    END
    ELSE IF @ReopenIfClosed = 1 AND @Status = 'CLOSED'
    BEGIN
        UPDATE dbo.ZaloOA_Conversation
        SET Status = 'OPEN', ClosedAt = NULL, ClosedByUserId = NULL, UpdatedAt = @Now
        WHERE Id = @Id;
    END

    COMMIT;

    SELECT cv.*, @IsNew AS IsNew
    FROM dbo.ZaloOA_Conversation cv
    WHERE cv.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Conversation_TouchLastMessage', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Conversation_TouchLastMessage;
GO
/* Dùng nội bộ bởi các SP message: cập nhật tin cuối / số chưa đọc. */
CREATE PROCEDURE dbo.ZaloOA_Conversation_TouchLastMessage
    @ConversationId  BIGINT,
    @MessageId       BIGINT,
    @SentAt          DATETIME,
    @Preview         NVARCHAR(500),
    @SenderType      VARCHAR(20),
    @IncrementUnread BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ZaloOA_Conversation
    SET LastMessageId         = CASE WHEN LastMessageAt IS NULL OR @SentAt >= LastMessageAt THEN @MessageId  ELSE LastMessageId END,
        LastMessagePreview    = CASE WHEN LastMessageAt IS NULL OR @SentAt >= LastMessageAt THEN @Preview    ELSE LastMessagePreview END,
        LastMessageSenderType = CASE WHEN LastMessageAt IS NULL OR @SentAt >= LastMessageAt THEN @SenderType ELSE LastMessageSenderType END,
        LastMessageAt         = CASE WHEN LastMessageAt IS NULL OR @SentAt >= LastMessageAt THEN @SentAt     ELSE LastMessageAt END,
        LastCustomerMessageAt = CASE WHEN @SenderType = 'CUSTOMER' AND (LastCustomerMessageAt IS NULL OR @SentAt > LastCustomerMessageAt)
                                     THEN @SentAt ELSE LastCustomerMessageAt END,
        UnreadCount           = UnreadCount + CASE WHEN @IncrementUnread = 1 THEN 1 ELSE 0 END,
        UpdatedAt             = GETUTCDATE()
    WHERE Id = @ConversationId;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Conversation_GetById', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Conversation_GetById;
GO
CREATE PROCEDURE dbo.ZaloOA_Conversation_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT cv.Id, cv.OAId, cv.CustomerId, cv.Status, cv.AssignedToUserId, cv.UnreadCount,
           cv.LastMessageId, cv.LastMessageAt, cv.LastMessagePreview, cv.LastMessageSenderType,
           cv.LastCustomerMessageAt, cv.LastReadAt, cv.LastReadByUserId, cv.StartedAt, cv.ClosedAt,
           cv.ClosedByUserId, cv.CreatedAt, cv.UpdatedAt,
           c.ZaloUserId, c.DisplayName, c.UserAlias, c.AvatarUrl, c.IsFollower, c.SharedName, c.SharedPhone
    FROM dbo.ZaloOA_Conversation cv
    INNER JOIN dbo.ZaloOA_Customer c ON c.Id = cv.CustomerId
    WHERE cv.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Conversation_GetList', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Conversation_GetList;
GO
CREATE PROCEDURE dbo.ZaloOA_Conversation_GetList
    @Keyword          NVARCHAR(700) = N'',    -- đã escape ký tự LIKE ở tầng ứng dụng
    @Status           VARCHAR(20)   = '',     -- '' tất cả
    @AssignedToUserId INT           = -1,     -- -1 tất cả, 0 chưa assign
    @UnreadOnly       BIT           = 0,
    @FromDate         DATETIME      = NULL,   -- lọc LastMessageAt
    @ToDate           DATETIME      = NULL,
    @SortBy           VARCHAR(30)   = 'LastMessageAt',   -- LastMessageAt / UnreadCount / CreatedAt / DisplayName
    @SortDir          VARCHAR(4)    = 'DESC',
    @PageIndex        INT           = 0,
    @PageSize         INT           = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = @PageIndex * @PageSize;
    DECLARE @Like NVARCHAR(710) = N'%' + ISNULL(@Keyword, N'') + N'%';

    SELECT cv.Id, cv.OAId, cv.CustomerId, cv.Status, cv.AssignedToUserId, cv.UnreadCount,
           cv.LastMessageId, cv.LastMessageAt, cv.LastMessagePreview, cv.LastMessageSenderType,
           cv.LastCustomerMessageAt, cv.LastReadAt, cv.LastReadByUserId, cv.StartedAt, cv.ClosedAt,
           cv.ClosedByUserId, cv.CreatedAt, cv.UpdatedAt,
           c.ZaloUserId, c.DisplayName, c.UserAlias, c.AvatarUrl, c.IsFollower, c.SharedName, c.SharedPhone,
           COUNT(*) OVER() AS TotalRecords
    FROM dbo.ZaloOA_Conversation cv
    INNER JOIN dbo.ZaloOA_Customer c ON c.Id = cv.CustomerId
    WHERE c.IsDeleted = 0
      AND (ISNULL(@Keyword, N'') = N''
           OR c.DisplayName LIKE @Like OR c.UserAlias LIKE @Like OR c.SharedName LIKE @Like
           OR c.SharedPhone LIKE @Like OR c.ZaloUserId LIKE @Like OR cv.LastMessagePreview LIKE @Like)
      AND (ISNULL(@Status, '') = '' OR cv.Status = @Status)
      AND (@AssignedToUserId = -1
           OR (@AssignedToUserId = 0 AND cv.AssignedToUserId IS NULL)
           OR cv.AssignedToUserId = @AssignedToUserId)
      AND (@UnreadOnly = 0 OR cv.UnreadCount > 0)
      AND (@FromDate IS NULL OR cv.LastMessageAt >= @FromDate)
      AND (@ToDate   IS NULL OR cv.LastMessageAt <  @ToDate)
    ORDER BY
        CASE WHEN @SortBy = 'UnreadCount'   AND @SortDir = 'ASC'  THEN cv.UnreadCount END ASC,
        CASE WHEN @SortBy = 'UnreadCount'   AND @SortDir = 'DESC' THEN cv.UnreadCount END DESC,
        CASE WHEN @SortBy = 'CreatedAt'     AND @SortDir = 'ASC'  THEN cv.CreatedAt END ASC,
        CASE WHEN @SortBy = 'CreatedAt'     AND @SortDir = 'DESC' THEN cv.CreatedAt END DESC,
        CASE WHEN @SortBy = 'DisplayName'   AND @SortDir = 'ASC'  THEN c.DisplayName END ASC,
        CASE WHEN @SortBy = 'DisplayName'   AND @SortDir = 'DESC' THEN c.DisplayName END DESC,
        CASE WHEN @SortBy = 'LastMessageAt' AND @SortDir = 'ASC'  THEN cv.LastMessageAt END ASC,
        CASE WHEN @SortBy = 'LastMessageAt' AND @SortDir = 'DESC' THEN cv.LastMessageAt END DESC,
        cv.Id DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Conversation_MarkRead', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Conversation_MarkRead;
GO
CREATE PROCEDURE dbo.ZaloOA_Conversation_MarkRead
    @Id     BIGINT,
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ZaloOA_Conversation
    SET UnreadCount = 0, LastReadAt = GETUTCDATE(), LastReadByUserId = @UserId, UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS Affected;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Conversation_SetStatus', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Conversation_SetStatus;
GO
CREATE PROCEDURE dbo.ZaloOA_Conversation_SetStatus
    @Id     BIGINT,
    @Status VARCHAR(20),     -- OPEN / PENDING / CLOSED (đã validate ở tầng ứng dụng)
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME = GETUTCDATE();

    UPDATE dbo.ZaloOA_Conversation
    SET Status         = @Status,
        ClosedAt       = CASE WHEN @Status = 'CLOSED' THEN @Now    ELSE NULL END,
        ClosedByUserId = CASE WHEN @Status = 'CLOSED' THEN @UserId ELSE NULL END,
        UpdatedAt      = @Now
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS Affected;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Conversation_GetUnreadSummary', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Conversation_GetUnreadSummary;
GO
CREATE PROCEDURE dbo.ZaloOA_Conversation_GetUnreadSummary
    @AfterMessageId BIGINT = 0,     -- > 0: trả thêm danh sách tin khách mới (Id > @AfterMessageId) để hiện thông báo
    @Top            INT    = 20
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ISNULL(SUM(cv.UnreadCount), 0)                          AS TotalUnread,
           ISNULL(SUM(CASE WHEN cv.UnreadCount > 0 THEN 1 ELSE 0 END), 0) AS UnreadConversations,
           (SELECT ISNULL(MAX(m.Id), 0) FROM dbo.ZaloOA_Message m WHERE m.Direction = 'IN') AS LatestInboundMessageId
    FROM dbo.ZaloOA_Conversation cv
    WHERE cv.UnreadCount > 0;

    IF @AfterMessageId > 0
    BEGIN
        SELECT TOP (@Top)
               m.Id, m.ConversationId, m.CustomerId, m.MessageType, m.Content, m.SentAt,
               c.DisplayName, c.AvatarUrl
        FROM dbo.ZaloOA_Message m
        INNER JOIN dbo.ZaloOA_Customer c ON c.Id = m.CustomerId
        WHERE m.Direction = 'IN' AND m.Id > @AfterMessageId
        ORDER BY m.Id DESC;
    END
END
GO

/* =============================================================================
   STORED PROCEDURES - Message
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_Message_Upsert', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_Upsert;
GO
/* Lưu tin từ webhook / lịch sử. Idempotent theo ZaloMessageId. */
CREATE PROCEDURE dbo.ZaloOA_Message_Upsert
    @ConversationId     BIGINT,
    @CustomerId         BIGINT,
    @Direction          VARCHAR(3),
    @SenderType         VARCHAR(20),
    @SenderId           VARCHAR(50)    = NULL,
    @ReceiverId         VARCHAR(50)    = NULL,
    @MessageType        VARCHAR(20),
    @Content            NVARCHAR(MAX)  = NULL,
    @AttachmentsJson    NVARCHAR(MAX)  = NULL,
    @QuoteZaloMessageId VARCHAR(100)   = NULL,
    @ZaloMessageId      VARCHAR(100)   = NULL,
    @Status             VARCHAR(20),
    @SentAt             DATETIME,
    @ReceivedAt         DATETIME       = NULL,
    @WebhookEventId     BIGINT         = NULL,
    @RawPayload         NVARCHAR(MAX)  = NULL,
    @Preview            NVARCHAR(500)  = NULL,
    @IncrementUnread    BIT            = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Now DATETIME = GETUTCDATE();
    DECLARE @Id BIGINT;
    DECLARE @IsDuplicate BIT = 0;

    BEGIN TRAN;

    IF @ZaloMessageId IS NOT NULL
    BEGIN
        SELECT @Id = Id
        FROM dbo.ZaloOA_Message WITH (UPDLOCK, HOLDLOCK)
        WHERE ZaloMessageId = @ZaloMessageId;
    END

    IF @Id IS NOT NULL
    BEGIN
        SET @IsDuplicate = 1;

        UPDATE dbo.ZaloOA_Message
        SET RawPayload     = COALESCE(RawPayload, @RawPayload),
            WebhookEventId = COALESCE(WebhookEventId, @WebhookEventId),
            ReceivedAt     = COALESCE(ReceivedAt, @ReceivedAt),
            UpdatedAt      = @Now
        WHERE Id = @Id;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.ZaloOA_Message
            (ConversationId, CustomerId, Direction, SenderType, SenderId, ReceiverId, MessageType, Content,
             AttachmentsJson, QuoteZaloMessageId, ZaloMessageId, Status, SentAt, ReceivedAt, WebhookEventId,
             RawPayload, CreatedAt, UpdatedAt)
        VALUES
            (@ConversationId, @CustomerId, @Direction, @SenderType, @SenderId, @ReceiverId, @MessageType, @Content,
             @AttachmentsJson, @QuoteZaloMessageId, @ZaloMessageId, @Status, @SentAt, @ReceivedAt, @WebhookEventId,
             @RawPayload, @Now, @Now);

        SET @Id = CAST(SCOPE_IDENTITY() AS BIGINT);

        EXEC dbo.ZaloOA_Conversation_TouchLastMessage
             @ConversationId = @ConversationId, @MessageId = @Id, @SentAt = @SentAt,
             @Preview = @Preview, @SenderType = @SenderType, @IncrementUnread = @IncrementUnread;
    END

    COMMIT;

    SELECT @Id AS Id, @IsDuplicate AS IsDuplicate;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_InsertPending', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_InsertPending;
GO
/* Tin nhân viên gửi từ website - tạo trước khi gọi Zalo. Idempotent theo ClientMessageId. */
CREATE PROCEDURE dbo.ZaloOA_Message_InsertPending
    @ConversationId  BIGINT,
    @CustomerId      BIGINT,
    @SenderId        VARCHAR(50)      = NULL,
    @ReceiverId      VARCHAR(50)      = NULL,
    @AgentUserId     INT              = NULL,
    @MessageType     VARCHAR(20),
    @Content         NVARCHAR(MAX)    = NULL,
    @AttachmentsJson NVARCHAR(MAX)    = NULL,
    @ClientMessageId UNIQUEIDENTIFIER,
    @Preview         NVARCHAR(500)    = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Now DATETIME = GETUTCDATE();
    DECLARE @Id BIGINT;
    DECLARE @IsDuplicate BIT = 0;

    BEGIN TRAN;

    SELECT @Id = Id
    FROM dbo.ZaloOA_Message WITH (UPDLOCK, HOLDLOCK)
    WHERE ClientMessageId = @ClientMessageId;

    IF @Id IS NOT NULL
        SET @IsDuplicate = 1;
    ELSE
    BEGIN
        INSERT INTO dbo.ZaloOA_Message
            (ConversationId, CustomerId, Direction, SenderType, SenderId, ReceiverId, AgentUserId, MessageType,
             Content, AttachmentsJson, ClientMessageId, Status, SentAt, CreatedAt, UpdatedAt)
        VALUES
            (@ConversationId, @CustomerId, 'OUT', 'AGENT', @SenderId, @ReceiverId, @AgentUserId, @MessageType,
             @Content, @AttachmentsJson, @ClientMessageId, 'PENDING', @Now, @Now, @Now);

        SET @Id = CAST(SCOPE_IDENTITY() AS BIGINT);

        EXEC dbo.ZaloOA_Conversation_TouchLastMessage
             @ConversationId = @ConversationId, @MessageId = @Id, @SentAt = @Now,
             @Preview = @Preview, @SenderType = 'AGENT', @IncrementUnread = 0;
    END

    COMMIT;

    SELECT m.*, @IsDuplicate AS IsDuplicate
    FROM dbo.ZaloOA_Message m
    WHERE m.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_MarkSent', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_MarkSent;
GO
/* Gửi thành công. Nếu webhook oa_send_* về TRƯỚC và đã tạo 1 dòng cùng ZaloMessageId
   thì gộp dòng đó vào dòng PENDING (giữ AgentUserId) và xoá dòng thừa. */
CREATE PROCEDURE dbo.ZaloOA_Message_MarkSent
    @Id            BIGINT,
    @ZaloMessageId VARCHAR(100),
    @SentAt        DATETIME      = NULL,
    @ResponseJson  NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Now DATETIME = GETUTCDATE();
    DECLARE @DupId BIGINT, @DupRaw NVARCHAR(MAX), @DupEventId BIGINT, @DupReceivedAt DATETIME, @DupStatus VARCHAR(20);
    DECLARE @ConversationId BIGINT;

    BEGIN TRAN;

    SELECT @ConversationId = ConversationId
    FROM dbo.ZaloOA_Message WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @Id;

    SELECT @DupId = Id, @DupRaw = RawPayload, @DupEventId = WebhookEventId,
           @DupReceivedAt = ReceivedAt, @DupStatus = Status
    FROM dbo.ZaloOA_Message WITH (UPDLOCK, HOLDLOCK)
    WHERE ZaloMessageId = @ZaloMessageId AND Id <> @Id;

    IF @DupId IS NOT NULL
    BEGIN
        UPDATE dbo.ZaloOA_Conversation
        SET LastMessageId = @Id
        WHERE Id = @ConversationId AND LastMessageId = @DupId;

        DELETE FROM dbo.ZaloOA_Message WHERE Id = @DupId;
    END

    UPDATE dbo.ZaloOA_Message
    SET ZaloMessageId  = @ZaloMessageId,
        Status         = CASE WHEN @DupStatus IN ('DELIVERED', 'SEEN') THEN @DupStatus
                              WHEN Status IN ('PENDING', 'FAILED', 'UNKNOWN') THEN 'SENT'
                              ELSE Status END,
        SentAt         = COALESCE(@SentAt, SentAt),
        ErrorCode      = NULL,
        ErrorMessage   = NULL,
        ResponseJson   = @ResponseJson,
        RawPayload     = COALESCE(RawPayload, @DupRaw),
        WebhookEventId = COALESCE(WebhookEventId, @DupEventId),
        ReceivedAt     = COALESCE(ReceivedAt, @DupReceivedAt),
        UpdatedAt      = @Now
    WHERE Id = @Id;

    COMMIT;

    SELECT m.* FROM dbo.ZaloOA_Message m WHERE m.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_MarkFailed', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_MarkFailed;
GO
CREATE PROCEDURE dbo.ZaloOA_Message_MarkFailed
    @Id           BIGINT,
    @ErrorCode    INT            = NULL,
    @ErrorMessage NVARCHAR(2000) = NULL,
    @ResponseJson NVARCHAR(MAX)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ZaloOA_Message
    SET Status       = 'FAILED',
        ErrorCode    = @ErrorCode,
        ErrorMessage = @ErrorMessage,
        ResponseJson = @ResponseJson,
        UpdatedAt    = GETUTCDATE()
    WHERE Id = @Id AND Status IN ('PENDING', 'FAILED', 'UNKNOWN');

    SELECT m.* FROM dbo.ZaloOA_Message m WHERE m.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_ResetForRetry', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_ResetForRetry;
GO
/* Chỉ tin FAILED mới được gửi lại. Trả về dòng nếu chuyển được sang PENDING. */
CREATE PROCEDURE dbo.ZaloOA_Message_ResetForRetry
    @Id          BIGINT,
    @AgentUserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ZaloOA_Message
    SET Status       = 'PENDING',
        AgentUserId  = COALESCE(@AgentUserId, AgentUserId),
        ErrorCode    = NULL,
        ErrorMessage = NULL,
        UpdatedAt    = GETUTCDATE()
    WHERE Id = @Id AND Status = 'FAILED' AND Direction = 'OUT' AND ZaloMessageId IS NULL;

    IF @@ROWCOUNT > 0
        SELECT m.* FROM dbo.ZaloOA_Message m WHERE m.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_UpdateStatusByZaloId', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_UpdateStatusByZaloId;
GO
/* Chỉ nâng trạng thái: SENT -> DELIVERED -> SEEN (không lùi). */
CREATE PROCEDURE dbo.ZaloOA_Message_UpdateStatusByZaloId
    @ZaloMessageId VARCHAR(100),
    @Status        VARCHAR(20),     -- DELIVERED / SEEN
    @At            DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ZaloOA_Message
    SET Status      = @Status,
        DeliveredAt = CASE WHEN @Status IN ('DELIVERED', 'SEEN') THEN COALESCE(DeliveredAt, @At) ELSE DeliveredAt END,
        SeenAt      = CASE WHEN @Status = 'SEEN' THEN COALESCE(SeenAt, @At) ELSE SeenAt END,
        UpdatedAt   = GETUTCDATE()
    WHERE ZaloMessageId = @ZaloMessageId
      AND Direction = 'OUT'
      AND (CASE Status WHEN 'SEEN' THEN 3 WHEN 'DELIVERED' THEN 2 WHEN 'SENT' THEN 1 ELSE 0 END)
        < (CASE @Status WHEN 'SEEN' THEN 3 WHEN 'DELIVERED' THEN 2 ELSE 0 END);

    SELECT @@ROWCOUNT AS Affected;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_GetById', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_GetById;
GO
CREATE PROCEDURE dbo.ZaloOA_Message_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.* FROM dbo.ZaloOA_Message m WHERE m.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_GetByConversation', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_GetByConversation;
GO
/* Phân trang dạng cursor: @BeforeId = Id tin cũ nhất đang hiển thị (0 = trang mới nhất).
   Sắp theo (SentAt, Id) vì tin nhập từ lịch sử có Id lớn nhưng thời gian cũ. */
CREATE PROCEDURE dbo.ZaloOA_Message_GetByConversation
    @ConversationId BIGINT,
    @BeforeId       BIGINT = 0,
    @PageSize       INT    = 30
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @BeforeSentAt DATETIME = NULL;
    IF @BeforeId > 0
        SELECT @BeforeSentAt = SentAt FROM dbo.ZaloOA_Message WHERE Id = @BeforeId AND ConversationId = @ConversationId;

    SELECT TOP (@PageSize)
           m.Id, m.ConversationId, m.CustomerId, m.Direction, m.SenderType, m.SenderId, m.ReceiverId,
           m.AgentUserId, m.MessageType, m.Content, m.AttachmentsJson, m.QuoteZaloMessageId, m.ZaloMessageId,
           m.ClientMessageId, m.Status, m.ErrorCode, m.ErrorMessage, m.SentAt, m.ReceivedAt, m.DeliveredAt,
           m.SeenAt, m.CreatedAt, m.UpdatedAt
    FROM dbo.ZaloOA_Message m
    WHERE m.ConversationId = @ConversationId
      AND (@BeforeSentAt IS NULL
           OR m.SentAt < @BeforeSentAt
           OR (m.SentAt = @BeforeSentAt AND m.Id < @BeforeId))
    ORDER BY m.SentAt DESC, m.Id DESC;
END
GO

IF OBJECT_ID('dbo.ZaloOA_Message_GetChanges', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_Message_GetChanges;
GO
/* Polling: tin mới hoặc tin vừa đổi trạng thái kể từ @Since (UTC). */
CREATE PROCEDURE dbo.ZaloOA_Message_GetChanges
    @ConversationId BIGINT,
    @Since          DATETIME,
    @Top            INT = 200
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Top)
           m.Id, m.ConversationId, m.CustomerId, m.Direction, m.SenderType, m.SenderId, m.ReceiverId,
           m.AgentUserId, m.MessageType, m.Content, m.AttachmentsJson, m.QuoteZaloMessageId, m.ZaloMessageId,
           m.ClientMessageId, m.Status, m.ErrorCode, m.ErrorMessage, m.SentAt, m.ReceivedAt, m.DeliveredAt,
           m.SeenAt, m.CreatedAt, m.UpdatedAt
    FROM dbo.ZaloOA_Message m
    WHERE m.ConversationId = @ConversationId
      AND m.UpdatedAt >= @Since
    ORDER BY m.UpdatedAt ASC, m.Id ASC;
END
GO

/* =============================================================================
   STORED PROCEDURES - Webhook event
   ============================================================================= */
IF OBJECT_ID('dbo.ZaloOA_WebhookEvent_Insert', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_WebhookEvent_Insert;
GO
CREATE PROCEDURE dbo.ZaloOA_WebhookEvent_Insert
    @DedupKey       CHAR(64),
    @EventName      VARCHAR(100),
    @OAId           VARCHAR(50)   = NULL,
    @ZaloUserId     VARCHAR(50)   = NULL,
    @ZaloMessageId  VARCHAR(100)  = NULL,
    @EventTimestamp BIGINT        = NULL,
    @RawPayload     NVARCHAR(MAX),
    @SignatureValid BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Id BIGINT;
    DECLARE @IsDuplicate BIT = 0;
    DECLARE @Status VARCHAR(20);

    BEGIN TRAN;

    SELECT @Id = Id, @Status = ProcessStatus
    FROM dbo.ZaloOA_WebhookEvent WITH (UPDLOCK, HOLDLOCK)
    WHERE DedupKey = @DedupKey;

    IF @Id IS NOT NULL
        SET @IsDuplicate = 1;
    ELSE
    BEGIN
        INSERT INTO dbo.ZaloOA_WebhookEvent
            (DedupKey, EventName, OAId, ZaloUserId, ZaloMessageId, EventTimestamp, RawPayload, SignatureValid)
        VALUES
            (@DedupKey, @EventName, @OAId, @ZaloUserId, @ZaloMessageId, @EventTimestamp, @RawPayload, @SignatureValid);

        SET @Id = CAST(SCOPE_IDENTITY() AS BIGINT);
        SET @Status = 'PENDING';
    END

    COMMIT;

    SELECT @Id AS Id, @IsDuplicate AS IsDuplicate, @Status AS ProcessStatus;
END
GO

IF OBJECT_ID('dbo.ZaloOA_WebhookEvent_GetById', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_WebhookEvent_GetById;
GO
CREATE PROCEDURE dbo.ZaloOA_WebhookEvent_GetById
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.* FROM dbo.ZaloOA_WebhookEvent e WHERE e.Id = @Id;
END
GO

IF OBJECT_ID('dbo.ZaloOA_WebhookEvent_SetStatus', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_WebhookEvent_SetStatus;
GO
CREATE PROCEDURE dbo.ZaloOA_WebhookEvent_SetStatus
    @Id           BIGINT,
    @Status       VARCHAR(20),          -- PROCESSED / FAILED / IGNORED
    @ErrorMessage NVARCHAR(2000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME = GETUTCDATE();

    UPDATE dbo.ZaloOA_WebhookEvent
    SET ProcessStatus = @Status,
        ErrorMessage  = @ErrorMessage,
        RetryCount    = RetryCount + CASE WHEN @Status = 'FAILED' THEN 1 ELSE 0 END,
        ProcessedAt   = CASE WHEN @Status IN ('PROCESSED', 'IGNORED') THEN @Now ELSE ProcessedAt END,
        UpdatedAt     = @Now
    WHERE Id = @Id;

    SELECT @@ROWCOUNT AS Affected;
END
GO

IF OBJECT_ID('dbo.ZaloOA_WebhookEvent_GetForReprocess', 'P') IS NOT NULL DROP PROCEDURE dbo.ZaloOA_WebhookEvent_GetForReprocess;
GO
/* Event FAILED còn lượt retry, hoặc PENDING quá lâu (vd enqueue Hangfire thất bại). */
CREATE PROCEDURE dbo.ZaloOA_WebhookEvent_GetForReprocess
    @StaleMinutes INT = 10,
    @MaxRetry     INT = 5,
    @Top          INT = 100
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StaleBefore DATETIME = DATEADD(MINUTE, -@StaleMinutes, GETUTCDATE());

    SELECT TOP (@Top) e.Id
    FROM dbo.ZaloOA_WebhookEvent e
    WHERE e.ProcessStatus IN ('PENDING', 'FAILED')
      AND e.UpdatedAt < @StaleBefore
      AND e.RetryCount < @MaxRetry
    ORDER BY e.Id;
END
GO

PRINT 'Zalo OA Chat migration completed.';
GO
