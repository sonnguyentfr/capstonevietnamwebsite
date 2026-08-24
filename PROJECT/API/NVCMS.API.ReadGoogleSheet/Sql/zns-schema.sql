/* ZNS schema - run on DefaultConnection: [CapstoneVietNamWeb] */

IF OBJECT_ID('dbo.ZNS_Template', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZNS_Template
    (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        TemplateId BIGINT NOT NULL,
        TemplateName NVARCHAR(500) NOT NULL,
        CreatedTime BIGINT NOT NULL,
        Status NVARCHAR(100) NULL,
        TemplateQuality NVARCHAR(100) NULL,
        TemplateTag NVARCHAR(100) NULL,
        Timeout BIGINT NULL,
        PreviewUrl NVARCHAR(1000) NULL,
        Price DECIMAL(18,2) NULL,
        PriceUid DECIMAL(18,2) NULL,
        PriceSdt DECIMAL(18,2) NULL,
        ApplyTemplateQuota BIT NOT NULL DEFAULT(0),
        Reason NVARCHAR(2000) NULL,
        IsActive BIT NOT NULL DEFAULT(1),
        DetailJson NVARCHAR(MAX) NULL,
        LastSyncedAt DATETIME NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE()),
        UpdatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE())
    );

    CREATE UNIQUE INDEX UX_ZNS_Template_TemplateId ON dbo.ZNS_Template(TemplateId);
    CREATE INDEX IX_ZNS_Template_Status ON dbo.ZNS_Template(Status);
    CREATE INDEX IX_ZNS_Template_IsActive ON dbo.ZNS_Template(IsActive);
    CREATE INDEX IX_ZNS_Template_TemplateTag ON dbo.ZNS_Template(TemplateTag);
END
GO

IF OBJECT_ID('dbo.ZNS_Template_Param', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZNS_Template_Param
    (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        ZnsTemplateId BIGINT NOT NULL,
        ParamName NVARCHAR(200) NOT NULL,
        IsRequired BIT NOT NULL,
        ParamType NVARCHAR(50) NOT NULL,
        MaxLength INT NULL,
        MinLength INT NULL,
        AcceptNull BIT NOT NULL,
        SortOrder INT NOT NULL,
        DisplayName NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE()),
        UpdatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE()),
        CONSTRAINT FK_ZNS_Template_Param_ZNS_Template FOREIGN KEY (ZnsTemplateId) REFERENCES dbo.ZNS_Template(Id) ON DELETE CASCADE
    );

    CREATE UNIQUE INDEX UX_ZNS_Template_Param_Template_ParamName ON dbo.ZNS_Template_Param(ZnsTemplateId, ParamName);
    CREATE INDEX IX_ZNS_Template_Param_ZnsTemplateId ON dbo.ZNS_Template_Param(ZnsTemplateId);
END
GO

IF OBJECT_ID('dbo.ZNS_Template_Button', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZNS_Template_Button
    (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        ZnsTemplateId BIGINT NOT NULL,
        ButtonType INT NOT NULL,
        Title NVARCHAR(500) NULL,
        Content NVARCHAR(2000) NULL,
        SortOrder INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE()),
        UpdatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE()),
        CONSTRAINT FK_ZNS_Template_Button_ZNS_Template FOREIGN KEY (ZnsTemplateId) REFERENCES dbo.ZNS_Template(Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_ZNS_Template_Button_ZnsTemplateId ON dbo.ZNS_Template_Button(ZnsTemplateId);
END
GO

IF OBJECT_ID('dbo.ZNS_Send_Log', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZNS_Send_Log
    (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        ZnsTemplateId BIGINT NULL,
        ZaloTemplateId BIGINT NOT NULL,
        Phone NVARCHAR(30) NOT NULL,
        ParamsJson NVARCHAR(MAX) NULL,
        RequestJson NVARCHAR(MAX) NULL,
        ResponseJson NVARCHAR(MAX) NULL,
        Status NVARCHAR(50) NOT NULL,
        ZaloMessageId NVARCHAR(200) NULL,
        SentTime DATETIME NULL,
        SendingMode NVARCHAR(50) NULL,
        RemainingQuota INT NULL,
        DailyQuota INT NULL,
        ErrorCode INT NULL,
        ErrorMessage NVARCHAR(2000) NULL,
        CampaignId INT NULL,
        EventCatId INT NULL,
        EventId INT NULL,
        ContextType NVARCHAR(100) NULL,
        CreatedBy NVARCHAR(200) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE()),
        UpdatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE())
    );

    CREATE INDEX IX_ZNS_Send_Log_ZaloTemplateId ON dbo.ZNS_Send_Log(ZaloTemplateId);
    CREATE INDEX IX_ZNS_Send_Log_Status ON dbo.ZNS_Send_Log(Status);
    CREATE INDEX IX_ZNS_Send_Log_CreatedAt ON dbo.ZNS_Send_Log(CreatedAt);
END
GO

IF OBJECT_ID('dbo.ZNS_Send_Queue', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ZNS_Send_Queue
    (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        TemplateId BIGINT NOT NULL,
        Phone NVARCHAR(30) NOT NULL,
        TemplateDataJson NVARCHAR(MAX) NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        RetryCount INT NOT NULL DEFAULT(0),
        ScheduledAt DATETIME NULL,
        StartedAt DATETIME NULL,
        CompletedAt DATETIME NULL,
        ErrorCode INT NULL,
        ErrorMessage NVARCHAR(2000) NULL,
        MsgId NVARCHAR(200) NULL,
        CampaignId INT NULL,
        EventCatId INT NULL,
        EventId INT NULL,
        ContextType NVARCHAR(100) NULL,
        CreatedBy NVARCHAR(200) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE()),
        UpdatedAt DATETIME NOT NULL DEFAULT(GETUTCDATE())
    );

    CREATE INDEX IX_ZNS_Send_Queue_Status ON dbo.ZNS_Send_Queue(Status);
    CREATE INDEX IX_ZNS_Send_Queue_CreatedAt ON dbo.ZNS_Send_Queue(CreatedAt);
END
GO
