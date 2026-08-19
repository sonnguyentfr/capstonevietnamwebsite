-- =============================================
-- Marketing Mail Campaign Resend Feature
-- Database Schema Updates
-- =============================================

USE [YourDatabaseName]  -- Thay thế bằng tên database thực tế
GO

-- =============================================
-- 1. ALTER TABLE: Thêm ResendCount và SenderEmailId
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Marketing_Mail_Send_Log]') AND name = 'ResendCount')
BEGIN
    ALTER TABLE [dbo].[Marketing_Mail_Send_Log]
    ADD ResendCount INT NOT NULL DEFAULT 0
    PRINT 'Column ResendCount added successfully'
END
ELSE
BEGIN
    PRINT 'Column ResendCount already exists'
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Marketing_Mail_Send_Log]') AND name = 'SenderEmailId')
BEGIN
    ALTER TABLE [dbo].[Marketing_Mail_Send_Log]
    ADD SenderEmailId INT NULL  -- Nullable vì records cũ không có thông tin này
    PRINT 'Column SenderEmailId added successfully'
END
ELSE
BEGIN
    PRINT 'Column SenderEmailId already exists'
END
GO

-- =============================================
-- 2. CREATE INDEX: Tăng performance query theo ResendCount
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Marketing_Mail_Send_Log_ResendCount' AND object_id = OBJECT_ID(N'[dbo].[Marketing_Mail_Send_Log]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Marketing_Mail_Send_Log_ResendCount
    ON [dbo].[Marketing_Mail_Send_Log] (CampaignSendId, ResendCount)
    INCLUDE (Email, Status, SentTime, OpenedTime)
    PRINT 'Index IX_Marketing_Mail_Send_Log_ResendCount created successfully'
END
ELSE
BEGIN
    PRINT 'Index IX_Marketing_Mail_Send_Log_ResendCount already exists'
END
GO

-- =============================================
-- 3. STORED PROCEDURE: Update ResendCount
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Marketing_Mail_Send_Log_UpdateResendCount]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[Marketing_Mail_Send_Log_UpdateResendCount]
END
GO

CREATE PROCEDURE [dbo].[Marketing_Mail_Send_Log_UpdateResendCount]
    @SendLogId INT,
    @SenderEmailId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [dbo].[Marketing_Mail_Send_Log]
    SET 
        ResendCount = ResendCount + 1,
        SenderEmailId = ISNULL(@SenderEmailId, SenderEmailId)
    WHERE 
        Id = @SendLogId
    
    SELECT 
        Id,
        CampaignSendId,
        Email,
        ResendCount,
        SenderEmailId,
        Status,
        SentTime
    FROM 
        [dbo].[Marketing_Mail_Send_Log]
    WHERE 
        Id = @SendLogId
END
GO

-- =============================================
-- 4. STORED PROCEDURE: Get Resend Statistics
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Marketing_Mail_Send_Log_GetResendStatistics]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetResendStatistics]
END
GO

CREATE PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetResendStatistics]
    @CampaignSendId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalEmails,
        SUM(CASE WHEN ResendCount > 0 THEN 1 ELSE 0 END) AS TotalResent,
        AVG(CAST(ResendCount AS FLOAT)) AS AverageResendCount,
        MAX(ResendCount) AS MaxResendCount
    FROM 
        [dbo].[Marketing_Mail_Send_Log]
    WHERE 
        CampaignSendId = @CampaignSendId
END
GO

-- =============================================
-- 5. STORED PROCEDURE: Get Unopened Emails for Resend
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Marketing_Mail_Send_Log_GetUnopenedForResend]') AND type in (N'P', N'PC'))
BEGIN
    DROP PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetUnopenedForResend]
END
GO

CREATE PROCEDURE [dbo].[Marketing_Mail_Send_Log_GetUnopenedForResend]
    @CampaignSendId INT,
    @MaxResendCount INT = 3
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        msl.Id,
        msl.CampaignSendId,
        msl.ListMailId,
        msl.Email,
        msl.Status,
        msl.SentTime,
        msl.OpenedTime,
        msl.ResendCount,
        msl.SenderEmailId,
        mma.Mail AS SenderEmail,
        mma.Name AS SenderName
    FROM 
        [dbo].[Marketing_Mail_Send_Log] msl
        LEFT JOIN [dbo].[Marketing_Mail_Account] mma ON msl.SenderEmailId = mma.Id
    WHERE 
        msl.CampaignSendId = @CampaignSendId
        AND (msl.OpenedTime IS NULL OR msl.OpenedTime = '')
        AND msl.Status IN ('Sent', 'Delivered')
        AND msl.ResendCount < @MaxResendCount
    ORDER BY 
        msl.SentTime DESC
END
GO

PRINT 'Marketing Mail Resend Schema Updates Completed Successfully!'
GO
