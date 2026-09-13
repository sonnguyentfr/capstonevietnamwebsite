IF OBJECT_ID(N'dbo.Marketing_ZNS_Template_SelectAll', N'P') IS NOT NULL
    DROP PROCEDURE dbo.Marketing_ZNS_Template_SelectAll
GO
CREATE PROCEDURE dbo.Marketing_ZNS_Template_SelectAll
    @OnlyActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        TemplateId,
        TemplateName,
        CreatedTime,
        Status,
        TemplateQuality,
        TemplateTag,
        Timeout,
        PreviewUrl,
        Price,
        PriceUid,
        PriceSdt,
        ApplyTemplateQuota,
        Reason,
        IsActive,
        LastSyncedAt,
        CreatedAt,
        UpdatedAt
    FROM dbo.ZNS_Template
    WHERE (@OnlyActive = 0 OR IsActive = 1)
    ORDER BY UpdatedAt DESC, Id DESC
END
GO

IF OBJECT_ID(N'dbo.Marketing_ZNS_Template_SelectByTemplateId', N'P') IS NOT NULL
    DROP PROCEDURE dbo.Marketing_ZNS_Template_SelectByTemplateId
GO
CREATE PROCEDURE dbo.Marketing_ZNS_Template_SelectByTemplateId
    @TemplateId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        Id,
        TemplateId,
        TemplateName,
        CreatedTime,
        Status,
        TemplateQuality,
        TemplateTag,
        Timeout,
        PreviewUrl,
        Price,
        PriceUid,
        PriceSdt,
        ApplyTemplateQuota,
        Reason,
        IsActive,
        LastSyncedAt,
        CreatedAt,
        UpdatedAt
    FROM dbo.ZNS_Template
    WHERE TemplateId = @TemplateId
END
GO