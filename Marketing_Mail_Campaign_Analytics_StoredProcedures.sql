-- =============================================
-- Email Campaign Analytics Stored Procedures
-- =============================================

-- =============================================
-- 1. Get Campaign Send By ID
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[Marketing_Mail_Campaign_Send_GetByID]'') AND type in (N''P'', N''PC''))
DROP PROCEDURE [dbo].[Marketing_Mail_Campaign_Send_GetByID]
GO

CREATE PROCEDURE [dbo].[Marketing_Mail_Campaign_Send_GetByID]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        CampaignId,
        TemplateId,
        Subject,
        Body,
        Status,
        TotalRecipient,
        TotalSent,
        TotalDelivered,
        TotalOpened,
        TotalClicked,
        TotalBounced,
        TotalComplaint,
        TotalUnsubscribed,
        ScheduleTime,
        StartedTime,
        CompletedTime,
        CreatedDate
    FROM Marketing_Mail_Campaign_Send
    WHERE Id = @Id
END
GO

-- =============================================
-- 2. Get Send Logs By Campaign Send ID (with filters and paging)
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[Marketing_Mail_Send_Log_GetByCampaignSendId]'') AND type in (N''P'', N''PC''))
DROP PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetByCampaignSendId]
GO

CREATE PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetByCampaignSendId]
    @CampaignSendId INT,
    @Status NVARCHAR(50) = NULL,
    @Email NVARCHAR(200) = NULL,
    @PageIndex INT = 0,
    @PageSize INT = 50,
    @SortBy NVARCHAR(50) = ''CreatedDate'',
    @SortDirection NVARCHAR(4) = ''DESC''
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = @PageIndex * @PageSize;
    DECLARE @SQL NVARCHAR(MAX);
    
    SET @SQL = N''
    SELECT 
        Id,
        CampaignSendId,
        ListMailId,
        Email,
        SesMessageId,
        Status,
        ErrorMessage,
        SentTime,
        DeliveredTime,
        OpenedTime,
        ClickedTime,
        CreatedDate,
        CASE 
            WHEN OpenedTime IS NOT NULL AND SentTime IS NOT NULL 
            THEN DATEDIFF(SECOND, SentTime, OpenedTime)
            ELSE NULL
        END AS TimeToOpenSeconds,
        CASE 
            WHEN ClickedTime IS NOT NULL AND OpenedTime IS NOT NULL 
            THEN DATEDIFF(SECOND, OpenedTime, ClickedTime)
            ELSE NULL
        END AS TimeToClickSeconds,
        COUNT(*) OVER() AS TotalCount
    FROM Marketing_Mail_Send_Log
    WHERE CampaignSendId = @CampaignSendId
        AND (@Status IS NULL OR Status = @Status)
        AND (@Email IS NULL OR Email LIKE ''''%'''' + @Email + ''''%'''')
    ORDER BY '' + QUOTENAME(@SortBy) + '' '' + @SortDirection + ''
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY'';
    
    EXEC sp_executesql @SQL, 
        N''@CampaignSendId INT, @Status NVARCHAR(50), @Email NVARCHAR(200), @Offset INT, @PageSize INT'',
        @CampaignSendId, @Status, @Email, @Offset, @PageSize;
END
GO

-- =============================================
-- 3. Get Send Log Statistics
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[Marketing_Mail_Send_Log_GetStatistics]'') AND type in (N''P'', N''PC''))
DROP PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetStatistics]
GO

CREATE PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetStatistics]
    @CampaignSendId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalRecipients,
        SUM(CASE WHEN Status IN (''Sent'', ''Delivered'', ''Opened'', ''Clicked'') THEN 1 ELSE 0 END) AS CountSent,
        SUM(CASE WHEN Status IN (''Delivered'', ''Opened'', ''Clicked'') THEN 1 ELSE 0 END) AS CountDelivered,
        SUM(CASE WHEN OpenedTime IS NOT NULL THEN 1 ELSE 0 END) AS CountOpened,
        SUM(CASE WHEN ClickedTime IS NOT NULL THEN 1 ELSE 0 END) AS CountClicked,
        SUM(CASE WHEN Status = ''Bounced'' THEN 1 ELSE 0 END) AS CountBounced,
        SUM(CASE WHEN Status = ''Complaint'' THEN 1 ELSE 0 END) AS CountComplaint,
        SUM(CASE WHEN Status = ''Unsubscribed'' THEN 1 ELSE 0 END) AS CountUnsubscribed,
        SUM(CASE WHEN Status = ''Failed'' THEN 1 ELSE 0 END) AS CountFailed,
        MIN(SentTime) AS FirstSentTime,
        MAX(SentTime) AS LastSentTime,
        AVG(CASE 
            WHEN OpenedTime IS NOT NULL AND SentTime IS NOT NULL 
            THEN DATEDIFF(SECOND, SentTime, OpenedTime)
            ELSE NULL
        END) AS AvgTimeToOpenSeconds
    FROM Marketing_Mail_Send_Log
    WHERE CampaignSendId = @CampaignSendId
END
GO

-- =============================================
-- 4. Get Status Distribution
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[Marketing_Mail_Send_Log_GetStatusDistribution]'') AND type in (N''P'', N''PC''))
DROP PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetStatusDistribution]
GO

CREATE PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetStatusDistribution]
    @CampaignSendId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Status,
        COUNT(*) AS Count,
        CAST(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER() AS DECIMAL(5,2)) AS Percentage
    FROM Marketing_Mail_Send_Log
    WHERE CampaignSendId = @CampaignSendId
    GROUP BY Status
    ORDER BY Count DESC
END
GO

-- =============================================
-- Index Recommendations for Performance
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = ''IX_Marketing_Mail_Send_Log_CampaignSendId'' AND object_id = OBJECT_ID(''Marketing_Mail_Send_Log''))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Marketing_Mail_Send_Log_CampaignSendId
    ON Marketing_Mail_Send_Log(CampaignSendId)
    INCLUDE (Status, Email, SentTime, OpenedTime, ClickedTime);
    PRINT ''Index IX_Marketing_Mail_Send_Log_CampaignSendId created successfully'';
END
ELSE
BEGIN
    PRINT ''Index IX_Marketing_Mail_Send_Log_CampaignSendId already exists'';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = ''IX_Marketing_Mail_Send_Log_CampaignSendId_Status'' AND object_id = OBJECT_ID(''Marketing_Mail_Send_Log''))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Marketing_Mail_Send_Log_CampaignSendId_Status
    ON Marketing_Mail_Send_Log(CampaignSendId, Status)
    INCLUDE (Email, SentTime, OpenedTime);
    PRINT ''Index IX_Marketing_Mail_Send_Log_CampaignSendId_Status created successfully'';
END
ELSE
BEGIN
    PRINT ''Index IX_Marketing_Mail_Send_Log_CampaignSendId_Status already exists'';
END
GO

PRINT ''All stored procedures and indexes created successfully'';
GO
