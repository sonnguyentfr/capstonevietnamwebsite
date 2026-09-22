-- =====================================================================================
-- Marketing Zalo Campaign - ANALYTICS
-- SP     : sp_Marketing_Zalo_Campaign_Analytics
-- Nguon  : ZNS_Send_Queue (hang doi gui)  -> trang thai vong doi ban tin
--          ZNS_Send_Log   (log goi Zalo API) -> ket qua ky thuat, error code, quota
--          Zalo_Message_Log (log ban tin)    -> ket qua giao ban tin tu phia Zalo
-- Connection: SiteSqlServerV1 ([CapstoneVietNam_old])
--
-- LUU Y TUONG THICH (DB compatibility_level = 100):
--   - KHONG dung: STRING_SPLIT, TRY_CONVERT, OPENJSON  (khong ho tro)
--   - Duoc dung : JSON_VALUE, IIF, CONCAT, FORMAT, OFFSET-FETCH, window functions
--
-- SP tra ve 10 RESULT SET (xem chi tiet tai moi muc ben duoi)
-- =====================================================================================

IF OBJECT_ID('dbo.sp_Marketing_Zalo_Campaign_Analytics', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Marketing_Zalo_Campaign_Analytics
GO

CREATE PROCEDURE [dbo].[sp_Marketing_Zalo_Campaign_Analytics]
    @CampaignId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- =================================================================================
    -- BUOC 1: LAY HANG DOI GUI CUA CHIEN DICH (ZNS_Send_Queue)
    --         Day la "su that goc" ve so luot gui: moi dong = 1 luot gui toi 1 SDT
    -- =================================================================================
    CREATE TABLE #Q
    (
        QueueId        BIGINT        NOT NULL,
        TemplateId     BIGINT        NOT NULL,
        Phone          NVARCHAR(60)  NOT NULL,
        QueueStatus    NVARCHAR(100) NOT NULL,
        RetryCount     INT           NOT NULL,
        ScheduledAt    DATETIME      NULL,
        StartedAt      DATETIME      NULL,
        CompletedAt    DATETIME      NULL,
        QErrorCode     INT           NULL,
        QErrorMessage  NVARCHAR(4000) NULL,
        MsgId          NVARCHAR(400) NULL,
        EventCatId     INT           NULL,
        EventId        INT           NULL,
        ContextType    NVARCHAR(200) NULL,
        CreatedBy      NVARCHAR(400) NULL,
        CreatedAt      DATETIME      NOT NULL,
        SendTime       DATETIME      NULL,   -- moc thoi gian dai dien cho luot gui
        WaitSeconds    INT           NULL,   -- ScheduledAt -> StartedAt (do tre hang doi)
        ProcessSeconds INT           NULL,   -- StartedAt   -> CompletedAt (thoi gian xu ly)
        TotalSeconds   INT           NULL    -- CreatedAt   -> CompletedAt (end-to-end)
    );

    INSERT INTO #Q
    (
        QueueId, TemplateId, Phone, QueueStatus, RetryCount,
        ScheduledAt, StartedAt, CompletedAt, QErrorCode, QErrorMessage, MsgId,
        EventCatId, EventId, ContextType, CreatedBy, CreatedAt,
        SendTime, WaitSeconds, ProcessSeconds, TotalSeconds
    )
    SELECT
        q.Id, q.TemplateId, q.Phone, q.Status, q.RetryCount,
        q.ScheduledAt, q.StartedAt, q.CompletedAt, q.ErrorCode, q.ErrorMessage, q.MsgId,
        q.EventCatId, q.EventId, q.ContextType, q.CreatedBy, q.CreatedAt,
        ISNULL(q.CompletedAt, ISNULL(q.StartedAt, q.CreatedAt)),
        CASE WHEN q.ScheduledAt IS NOT NULL AND q.StartedAt   IS NOT NULL
                  AND q.StartedAt   >= q.ScheduledAt THEN DATEDIFF(SECOND, q.ScheduledAt, q.StartedAt)   END,
        CASE WHEN q.StartedAt   IS NOT NULL AND q.CompletedAt IS NOT NULL
                  AND q.CompletedAt >= q.StartedAt   THEN DATEDIFF(SECOND, q.StartedAt,   q.CompletedAt) END,
        CASE WHEN q.CompletedAt IS NOT NULL AND q.CompletedAt >= q.CreatedAt
                  THEN DATEDIFF(SECOND, q.CreatedAt, q.CompletedAt) END
    FROM dbo.ZNS_Send_Queue q
    WHERE q.CampaignId = @CampaignId;

    -- =================================================================================
    -- BUOC 2: LAY LOG GOI ZALO API (ZNS_Send_Log)
    --         TrackingId nam trong ParamsJson -> dung de noi sang Zalo_Message_Log
    -- =================================================================================
    CREATE TABLE #L
    (
        LogId          BIGINT        NOT NULL,
        ZaloTemplateId BIGINT        NOT NULL,
        Phone          NVARCHAR(60)  NOT NULL,
        LogStatus      NVARCHAR(100) NOT NULL,
        LErrorCode     INT           NULL,
        LErrorMessage  NVARCHAR(4000) NULL,
        ZaloMessageId  NVARCHAR(400) NULL,
        SentTime       DATETIME      NULL,
        SendingMode    NVARCHAR(100) NULL,
        RemainingQuota INT           NULL,
        DailyQuota     INT           NULL,
        EventCatId     INT           NULL,
        EventId        INT           NULL,
        LogCreatedAt   DATETIME      NOT NULL,
        TrackingId     NVARCHAR(200) NULL,
        StudentName    NVARCHAR(400) NULL
    );

    INSERT INTO #L
    (
        LogId, ZaloTemplateId, Phone, LogStatus, LErrorCode, LErrorMessage,
        ZaloMessageId, SentTime, SendingMode, RemainingQuota, DailyQuota,
        EventCatId, EventId, LogCreatedAt, TrackingId, StudentName
    )
    SELECT
        sl.Id, sl.ZaloTemplateId, sl.Phone, sl.Status, sl.ErrorCode, sl.ErrorMessage,
        sl.ZaloMessageId, sl.SentTime, sl.SendingMode, sl.RemainingQuota, sl.DailyQuota,
        sl.EventCatId, sl.EventId, sl.CreatedAt,
        JSON_VALUE(sl.ParamsJson, '$.tracking_id'),
        JSON_VALUE(sl.ParamsJson, '$.student_fullname')
    FROM dbo.ZNS_Send_Log sl
    WHERE sl.CampaignId = @CampaignId;

    CREATE INDEX IX_L_Msg   ON #L (ZaloMessageId);
    CREATE INDEX IX_L_Phone ON #L (Phone, ZaloTemplateId, LogCreatedAt);

    -- =================================================================================
    -- BUOC 2B: ANH XA 1-1 GIUA HANG DOI VA LOG API  (#MAP)
    --   Moi dong queue chi duoc gan DUNG 1 dong log, va nguoc lai. Neu de LEFT JOIN
    --   theo cua so thoi gian thi 1 log se bi dem lai nhieu lan -> Delivered > Sent.
    --
    --   Vong 1: khop tuyet doi qua MsgId (queue.MsgId = log.ZaloMessageId).
    --   Vong 2: cac dong con lai khop theo thu tu thoi gian trong tung (Phone, Template).
    --           Bo qua nhung dong KHONG THE co log: con nam trong hang doi, hoac loi
    --           validate xay ra TRUOC khi API ghi log (-108/-400/-404/-1121).
    -- =================================================================================
    CREATE TABLE #MAP
    (
        QueueId BIGINT NOT NULL PRIMARY KEY,
        LogId   BIGINT NULL
    );

    -- Vong 1: khop tuyet doi theo MsgId
    INSERT INTO #MAP (QueueId, LogId)
    SELECT q.QueueId, l.LogId
    FROM #Q q
    CROSS APPLY
    (
        SELECT TOP 1 x.LogId
        FROM #L x
        WHERE x.ZaloMessageId = q.MsgId
        ORDER BY x.LogId
    ) l
    WHERE q.MsgId IS NOT NULL AND q.MsgId <> '';

    -- Vong 2: khop theo thu tu thoi gian trong tung cap (Phone, Template)
    ;WITH qn AS
    (
        SELECT
            q.QueueId, q.Phone, q.TemplateId,
            ROW_NUMBER() OVER (PARTITION BY q.Phone, q.TemplateId
                               ORDER BY q.CreatedAt, q.QueueId) AS rn
        FROM #Q q
        WHERE NOT EXISTS (SELECT 1 FROM #MAP m WHERE m.QueueId = q.QueueId)
          AND q.QueueStatus IN ('Sent', 'Failed', 'Retry')
          AND (q.QErrorCode IS NULL OR q.QErrorCode NOT IN (-108, -400, -404, -1121))
    ),
    ln AS
    (
        SELECT
            l.LogId, l.Phone, l.ZaloTemplateId,
            ROW_NUMBER() OVER (PARTITION BY l.Phone, l.ZaloTemplateId
                               ORDER BY l.LogCreatedAt, l.LogId) AS rn
        FROM #L l
        WHERE NOT EXISTS (SELECT 1 FROM #MAP m WHERE m.LogId = l.LogId)
    )
    INSERT INTO #MAP (QueueId, LogId)
    SELECT qn.QueueId, ln.LogId
    FROM qn
    LEFT JOIN ln
           ON ln.Phone          = qn.Phone
          AND ln.ZaloTemplateId = qn.TemplateId
          AND ln.rn             = qn.rn;

    -- =================================================================================
    -- BUOC 3: GOP 3 BANG THANH 1 BANG SU KIEN DUY NHAT (#A - Attempts)
    --   Moi dong = 1 luot gui, kem ket qua ky thuat (log) va ket qua giao (message log).
    --   Zalo_Message_Log noi qua TrackingId -> cho biet Zalo CO GIAO duoc hay khong.
    -- =================================================================================
    CREATE TABLE #A
    (
        QueueId        BIGINT        NOT NULL,
        TemplateId     BIGINT        NOT NULL,
        Phone          NVARCHAR(60)  NOT NULL,
        FullName       NVARCHAR(510) NULL,
        QueueStatus    NVARCHAR(100) NOT NULL,
        RetryCount     INT           NOT NULL,
        ScheduledAt    DATETIME      NULL,
        StartedAt      DATETIME      NULL,
        CompletedAt    DATETIME      NULL,
        SendTime       DATETIME      NULL,
        WaitSeconds    INT           NULL,
        ProcessSeconds INT           NULL,
        TotalSeconds   INT           NULL,
        EventCatId     INT           NULL,
        EventId        INT           NULL,
        MsgId          NVARCHAR(400) NULL,
        LogId          BIGINT        NULL,
        LogStatus      NVARCHAR(100) NULL,
        SentTime       DATETIME      NULL,
        SendingMode    NVARCHAR(100) NULL,
        RemainingQuota INT           NULL,
        DailyQuota     INT           NULL,
        TrackingId     NVARCHAR(200) NULL,
        DeliveryStatus INT           NULL,   -- Zalo_Message_Log.Status: 1=OK, 0=cho, <0=loi
        DeliveryMessage NVARCHAR(1000) NULL,
        ErrorCode      INT           NULL,   -- error code hop nhat
        ErrorMessage   NVARCHAR(4000) NULL   -- error message hop nhat
    );

    INSERT INTO #A
    (
        QueueId, TemplateId, Phone, FullName, QueueStatus, RetryCount,
        ScheduledAt, StartedAt, CompletedAt, SendTime, WaitSeconds, ProcessSeconds, TotalSeconds,
        EventCatId, EventId, MsgId, LogId, LogStatus, SentTime, SendingMode,
        RemainingQuota, DailyQuota, TrackingId, DeliveryStatus, DeliveryMessage,
        ErrorCode, ErrorMessage
    )
    SELECT
        q.QueueId, q.TemplateId, q.Phone,
        ISNULL(NULLIF(LTRIM(RTRIM(m.FullName)), ''),
               ISNULL(NULLIF(LTRIM(RTRIM(l.StudentName)), ''), sdt.FullName)),
        q.QueueStatus, q.RetryCount,
        q.ScheduledAt, q.StartedAt, q.CompletedAt, q.SendTime,
        q.WaitSeconds, q.ProcessSeconds, q.TotalSeconds,
        q.EventCatId, q.EventId, q.MsgId,
        l.LogId, l.LogStatus, l.SentTime, l.SendingMode,
        l.RemainingQuota, l.DailyQuota, l.TrackingId,
        m.Status, m.Message,
        ISNULL(q.QErrorCode, ISNULL(l.LErrorCode, CASE WHEN m.Status < 0 THEN m.Status END)),
        ISNULL(NULLIF(q.QErrorMessage, ''), ISNULL(NULLIF(l.LErrorMessage, ''), m.Message))
    FROM #Q q
    LEFT JOIN #MAP map ON map.QueueId = q.QueueId
    LEFT JOIN #L   l   ON l.LogId     = map.LogId
    OUTER APPLY
    (
        SELECT TOP 1 ml.Status, ml.Message, ml.FullName
        FROM dbo.Zalo_Message_Log ml
        WHERE ml.TrackingId = l.TrackingId
          AND l.TrackingId IS NOT NULL
        ORDER BY ml.Id DESC
    ) m
    OUTER APPLY
    (
        SELECT TOP 1 s.FullName
        FROM dbo.Marketing_Zalo_ListSdt s
        WHERE s.Marketing_Zalo_CampaignId = @CampaignId
          AND s.Phone = q.Phone
        ORDER BY s.Id
    ) sdt;

    -- Bien dem dung chung cho cac ty le
    DECLARE @Total INT = (SELECT COUNT(*) FROM #A);

    -- =================================================================================
    -- RESULT SET 1: SUMMARY - cac con so KPI tong quan cua chien dich
    -- =================================================================================
    SELECT
        c.Id                                                      AS CampaignId,
        c.Title                                                   AS Title,
        ISNULL(c.Description, '')                                 AS Description,
        c.Status                                                  AS CampaignStatus,
        c.CreatedDate                                             AS CampaignCreatedDate,

        -- Quy mo
        ISNULL(lst.TotalPhoneInList, 0)                           AS TotalPhoneInList,
        @Total                                                    AS TotalSend,
        ISNULL(a.TotalPhoneTargeted, 0)                           AS TotalPhoneTargeted,

        -- Trang thai hang doi
        ISNULL(a.TotalQueued, 0)                                  AS TotalQueued,
        ISNULL(a.TotalProcessing, 0)                              AS TotalProcessing,
        ISNULL(a.TotalSent, 0)                                    AS TotalSent,
        ISNULL(a.TotalFailed, 0)                                  AS TotalFailed,
        ISNULL(a.TotalRetry, 0)                                   AS TotalRetry,
        ISNULL(a.TotalRetryCount, 0)                              AS TotalRetryCount,

        -- Ket qua giao ban tin phia Zalo
        ISNULL(a.TotalDelivered, 0)                               AS TotalDelivered,
        ISNULL(a.TotalRejected, 0)                                AS TotalRejected,
        ISNULL(a.TotalPendingDelivery, 0)                         AS TotalPendingDelivery,

        -- Ty le (%)
        CAST(ISNULL(a.TotalSent, 0)      * 100.0 / NULLIF(@Total, 0) AS DECIMAL(9,2)) AS SuccessRate,
        CAST(ISNULL(a.TotalFailed, 0)    * 100.0 / NULLIF(@Total, 0) AS DECIMAL(9,2)) AS FailRate,
        CAST((ISNULL(a.TotalQueued,0) + ISNULL(a.TotalProcessing,0) + ISNULL(a.TotalRetry,0))
                                         * 100.0 / NULLIF(@Total, 0) AS DECIMAL(9,2)) AS PendingRate,
        CAST(ISNULL(a.TotalDelivered, 0) * 100.0 / NULLIF(@Total, 0) AS DECIMAL(9,2)) AS DeliveryRate,
        CAST(ISNULL(a.TotalPhoneTargeted, 0) * 1.0 / NULLIF(ISNULL(lst.TotalPhoneInList,0), 0) * 100 AS DECIMAL(9,2)) AS CoverageRate,
        CAST(@Total * 1.0 / NULLIF(ISNULL(a.TotalPhoneTargeted, 0), 0) AS DECIMAL(9,2)) AS AvgSendPerPhone,

        -- Do phu
        ISNULL(a.TotalTemplateUsed, 0)                            AS TotalTemplateUsed,
        ISNULL(a.TotalEventCat, 0)                                AS TotalEventCat,
        ISNULL(a.TotalEvent, 0)                                   AS TotalEvent,

        -- Hieu nang duong ong gui
        ISNULL(a.AvgWaitSeconds, 0)                               AS AvgWaitSeconds,
        ISNULL(a.AvgProcessSeconds, 0)                            AS AvgProcessSeconds,
        ISNULL(a.AvgTotalSeconds, 0)                              AS AvgTotalSeconds,
        ISNULL(a.MaxProcessSeconds, 0)                            AS MaxProcessSeconds,

        -- Moc thoi gian
        a.FirstQueuedTime                                         AS FirstQueuedTime,
        a.FirstSentTime                                           AS FirstSentTime,
        a.LastCompletedTime                                       AS LastCompletedTime,

        -- Chi phi uoc tinh (VND) - theo bang gia ZNS_Template
        ISNULL(cost.EstimatedCostSent, 0)                         AS EstimatedCostSent,
        ISNULL(cost.EstimatedCostAll, 0)                          AS EstimatedCostAll,
        CAST(ISNULL(cost.EstimatedCostSent, 0) * 1.0
             / NULLIF(ISNULL(a.TotalDelivered, 0), 0) AS DECIMAL(18,2))  AS CostPerDelivered,

        -- Quota con lai ghi nhan gan nhat
        quota.RemainingQuota                                      AS RemainingQuota,
        quota.DailyQuota                                          AS DailyQuota
    FROM dbo.Marketing_Zalo_Campaign c
    OUTER APPLY
    (
        SELECT
            COUNT(DISTINCT x.Phone)                                              AS TotalPhoneTargeted,
            SUM(CASE WHEN x.QueueStatus = 'Queued'     THEN 1 ELSE 0 END)        AS TotalQueued,
            SUM(CASE WHEN x.QueueStatus = 'Processing' THEN 1 ELSE 0 END)        AS TotalProcessing,
            SUM(CASE WHEN x.QueueStatus = 'Sent'       THEN 1 ELSE 0 END)        AS TotalSent,
            SUM(CASE WHEN x.QueueStatus = 'Failed'     THEN 1 ELSE 0 END)        AS TotalFailed,
            SUM(CASE WHEN x.QueueStatus = 'Retry'      THEN 1 ELSE 0 END)        AS TotalRetry,
            SUM(x.RetryCount)                                                    AS TotalRetryCount,
            SUM(CASE WHEN x.DeliveryStatus  = 1 THEN 1 ELSE 0 END)               AS TotalDelivered,
            SUM(CASE WHEN x.DeliveryStatus  < 0 THEN 1 ELSE 0 END)               AS TotalRejected,
            SUM(CASE WHEN x.DeliveryStatus  = 0 THEN 1 ELSE 0 END)               AS TotalPendingDelivery,
            COUNT(DISTINCT x.TemplateId)                                         AS TotalTemplateUsed,
            COUNT(DISTINCT x.EventCatId)                                         AS TotalEventCat,
            COUNT(DISTINCT x.EventId)                                            AS TotalEvent,
            CAST(AVG(x.WaitSeconds * 1.0)    AS DECIMAL(18,2))                   AS AvgWaitSeconds,
            CAST(AVG(x.ProcessSeconds * 1.0) AS DECIMAL(18,2))                   AS AvgProcessSeconds,
            CAST(AVG(x.TotalSeconds * 1.0)   AS DECIMAL(18,2))                   AS AvgTotalSeconds,
            MAX(x.ProcessSeconds)                                                AS MaxProcessSeconds,
            MIN(x.ScheduledAt)                                                   AS FirstQueuedTime,
            MIN(x.SentTime)                                                      AS FirstSentTime,
            MAX(x.CompletedAt)                                                   AS LastCompletedTime
        FROM #A x
    ) a
    OUTER APPLY
    (
        SELECT COUNT(*) AS TotalPhoneInList
        FROM dbo.Marketing_Zalo_ListSdt s
        WHERE s.Marketing_Zalo_CampaignId = @CampaignId
    ) lst
    OUTER APPLY
    (
        SELECT
            SUM(CASE WHEN x.QueueStatus = 'Sent' THEN ISNULL(t.Price, 0) ELSE 0 END) AS EstimatedCostSent,
            SUM(ISNULL(t.Price, 0))                                                  AS EstimatedCostAll
        FROM #A x
        LEFT JOIN dbo.ZNS_Template t ON t.TemplateId = x.TemplateId
    ) cost
    OUTER APPLY
    (
        SELECT TOP 1 x.RemainingQuota, x.DailyQuota
        FROM #L x
        WHERE x.RemainingQuota IS NOT NULL
        ORDER BY x.LogId DESC
    ) quota
    WHERE c.Id = @CampaignId;

    -- =================================================================================
    -- RESULT SET 2: PHAN BO TRANG THAI HANG DOI  (Doughnut chart)
    -- =================================================================================
    SELECT
        x.QueueStatus                                                        AS Status,
        CASE x.QueueStatus
            WHEN 'Queued'     THEN N'Cho gui'
            WHEN 'Processing' THEN N'Dang xu ly'
            WHEN 'Sent'       THEN N'Gui thanh cong'
            WHEN 'Failed'     THEN N'That bai'
            WHEN 'Retry'      THEN N'Gui lai'
            ELSE x.QueueStatus
        END                                                                  AS StatusLabel,
        COUNT(*)                                                             AS Quantity,
        CAST(COUNT(*) * 100.0 / NULLIF(@Total, 0) AS DECIMAL(9,2))           AS Percentage
    FROM #A x
    GROUP BY x.QueueStatus
    ORDER BY Quantity DESC;

    -- =================================================================================
    -- RESULT SET 3: THONG KE THEO TEMPLATE ZNS  (Bar chart + bang)
    -- =================================================================================
    SELECT
        x.TemplateId                                                         AS TemplateId,
        ISNULL(t.TemplateName, CONCAT(N'Template #', CAST(x.TemplateId AS NVARCHAR(30)))) AS TemplateName,
        ISNULL(t.TemplateQuality, N'UNDEFINED')                              AS TemplateQuality,
        ISNULL(t.Status, '')                                                 AS TemplateStatus,
        ISNULL(t.Price, 0)                                                   AS Price,
        COUNT(*)                                                             AS TotalSend,
        COUNT(DISTINCT x.Phone)                                              AS DistinctPhone,
        SUM(CASE WHEN x.QueueStatus = 'Sent'       THEN 1 ELSE 0 END)        AS Sent,
        SUM(CASE WHEN x.QueueStatus = 'Failed'     THEN 1 ELSE 0 END)        AS Failed,
        SUM(CASE WHEN x.QueueStatus = 'Retry'      THEN 1 ELSE 0 END)        AS Retry,
        SUM(CASE WHEN x.QueueStatus = 'Queued'     THEN 1 ELSE 0 END)        AS Queued,
        SUM(CASE WHEN x.QueueStatus = 'Processing' THEN 1 ELSE 0 END)        AS Processing,
        SUM(CASE WHEN x.DeliveryStatus = 1         THEN 1 ELSE 0 END)        AS Delivered,
        CAST(SUM(CASE WHEN x.QueueStatus = 'Sent' THEN 1 ELSE 0 END) * 100.0
             / NULLIF(COUNT(*), 0) AS DECIMAL(9,2))                          AS SuccessRate,
        SUM(CASE WHEN x.QueueStatus = 'Sent' THEN ISNULL(t.Price, 0) ELSE 0 END) AS EstimatedCost
    FROM #A x
    LEFT JOIN dbo.ZNS_Template t ON t.TemplateId = x.TemplateId
    GROUP BY x.TemplateId, t.TemplateName, t.TemplateQuality, t.Status, t.Price
    ORDER BY TotalSend DESC;

    -- =================================================================================
    -- RESULT SET 4: THONG KE THEO NHOM SU KIEN (EventCatId)
    -- =================================================================================
    SELECT
        ISNULL(x.EventCatId, 0)                                              AS EventCatId,
        ISNULL(ec.CatName, N'(Khong xac dinh)')                              AS EventCatName,
        ISNULL(ec.DateShow, '')                                              AS DateShow,
        COUNT(*)                                                             AS TotalSend,
        COUNT(DISTINCT x.Phone)                                              AS DistinctPhone,
        SUM(CASE WHEN x.QueueStatus = 'Sent'   THEN 1 ELSE 0 END)            AS Sent,
        SUM(CASE WHEN x.QueueStatus = 'Failed' THEN 1 ELSE 0 END)            AS Failed,
        SUM(CASE WHEN x.QueueStatus NOT IN ('Sent','Failed') THEN 1 ELSE 0 END) AS Pending,
        SUM(CASE WHEN x.DeliveryStatus = 1     THEN 1 ELSE 0 END)            AS Delivered,
        CAST(SUM(CASE WHEN x.QueueStatus = 'Sent' THEN 1 ELSE 0 END) * 100.0
             / NULLIF(COUNT(*), 0) AS DECIMAL(9,2))                          AS SuccessRate
    FROM #A x
    LEFT JOIN dbo.NV_Events_Cat ec ON ec.id = x.EventCatId
    GROUP BY x.EventCatId, ec.CatName, ec.DateShow
    ORDER BY TotalSend DESC;

    -- =================================================================================
    -- RESULT SET 5: THONG KE THEO SU KIEN / DIA DIEM (EventId)
    -- =================================================================================
    SELECT
        ISNULL(x.EventId, 0)                                                 AS EventId,
        ISNULL(ev.Title, N'(Khong xac dinh)')                                AS EventName,
        ISNULL(x.EventCatId, 0)                                              AS EventCatId,
        ISNULL(ec.CatName, N'(Khong xac dinh)')                              AS EventCatName,
        ISNULL(ev.diadiem, '')                                               AS Location,
        ev.fromdatetime                                                      AS FromDate,
        COUNT(*)                                                             AS TotalSend,
        COUNT(DISTINCT x.Phone)                                              AS DistinctPhone,
        SUM(CASE WHEN x.QueueStatus = 'Sent'   THEN 1 ELSE 0 END)            AS Sent,
        SUM(CASE WHEN x.QueueStatus = 'Failed' THEN 1 ELSE 0 END)            AS Failed,
        SUM(CASE WHEN x.QueueStatus NOT IN ('Sent','Failed') THEN 1 ELSE 0 END) AS Pending,
        SUM(CASE WHEN x.DeliveryStatus = 1     THEN 1 ELSE 0 END)            AS Delivered,
        CAST(SUM(CASE WHEN x.QueueStatus = 'Sent' THEN 1 ELSE 0 END) * 100.0
             / NULLIF(COUNT(*), 0) AS DECIMAL(9,2))                          AS SuccessRate
    FROM #A x
    LEFT JOIN dbo.NV_Events     ev ON ev.id = x.EventId
    LEFT JOIN dbo.NV_Events_Cat ec ON ec.id = x.EventCatId
    GROUP BY x.EventId, ev.Title, x.EventCatId, ec.CatName, ev.diadiem, ev.fromdatetime
    ORDER BY TotalSend DESC;

    -- =================================================================================
    -- RESULT SET 6: SO LUOT GUI THEO SO DIEN THOAI
    --   Phat hien SDT bi gui trung lap nhieu lan / SDT luon that bai
    -- =================================================================================
    SELECT
        x.Phone                                                              AS Phone,
        ISNULL(MAX(x.FullName), N'Khach hang')                               AS FullName,
        COUNT(*)                                                             AS TotalSend,
        COUNT(DISTINCT x.TemplateId)                                         AS TemplateCount,
        SUM(CASE WHEN x.QueueStatus = 'Sent'   THEN 1 ELSE 0 END)            AS Sent,
        SUM(CASE WHEN x.QueueStatus = 'Failed' THEN 1 ELSE 0 END)            AS Failed,
        SUM(CASE WHEN x.QueueStatus = 'Retry'  THEN 1 ELSE 0 END)            AS Retry,
        SUM(CASE WHEN x.QueueStatus IN ('Queued','Processing') THEN 1 ELSE 0 END) AS Pending,
        SUM(x.RetryCount)                                                    AS TotalRetryCount,
        SUM(CASE WHEN x.DeliveryStatus = 1     THEN 1 ELSE 0 END)            AS Delivered,
        MIN(x.SendTime)                                                      AS FirstSendTime,
        MAX(x.SendTime)                                                      AS LastSendTime,
        MAX(last.QueueStatus)                                                AS LastStatus,
        MAX(last.ErrorCode)                                                  AS LastErrorCode,
        MAX(last.ErrorMessage)                                               AS LastErrorMessage,
        CAST(SUM(CASE WHEN x.QueueStatus = 'Sent' THEN 1 ELSE 0 END) * 100.0
             / NULLIF(COUNT(*), 0) AS DECIMAL(9,2))                          AS SuccessRate
    FROM #A x
    OUTER APPLY
    (
        SELECT TOP 1 y.QueueStatus, y.ErrorCode, y.ErrorMessage
        FROM #A y
        WHERE y.Phone = x.Phone
        ORDER BY y.QueueId DESC
    ) last
    GROUP BY x.Phone
    ORDER BY TotalSend DESC, Failed DESC;

    -- =================================================================================
    -- RESULT SET 7: DIEN BIEN THEO NGAY  (Line chart)
    -- =================================================================================
    SELECT
        CAST(x.SendTime AS DATE)                                             AS SendDate,
        COUNT(*)                                                             AS TotalSend,
        SUM(CASE WHEN x.QueueStatus = 'Sent'   THEN 1 ELSE 0 END)            AS Sent,
        SUM(CASE WHEN x.QueueStatus = 'Failed' THEN 1 ELSE 0 END)            AS Failed,
        SUM(CASE WHEN x.QueueStatus NOT IN ('Sent','Failed') THEN 1 ELSE 0 END) AS Pending,
        SUM(CASE WHEN x.DeliveryStatus = 1     THEN 1 ELSE 0 END)            AS Delivered,
        CAST(SUM(CASE WHEN x.QueueStatus = 'Sent' THEN 1 ELSE 0 END) * 100.0
             / NULLIF(COUNT(*), 0) AS DECIMAL(9,2))                          AS SuccessRate
    FROM #A x
    WHERE x.SendTime IS NOT NULL
    GROUP BY CAST(x.SendTime AS DATE)
    ORDER BY SendDate;

    -- =================================================================================
    -- RESULT SET 8: PHAN BO THEO KHUNG GIO GUI  (Bar chart - toi uu gio gui)
    -- =================================================================================
    SELECT
        DATEPART(HOUR, x.SendTime)                                           AS SendHour,
        COUNT(*)                                                             AS TotalSend,
        SUM(CASE WHEN x.QueueStatus = 'Sent'   THEN 1 ELSE 0 END)            AS Sent,
        SUM(CASE WHEN x.QueueStatus = 'Failed' THEN 1 ELSE 0 END)            AS Failed,
        CAST(SUM(CASE WHEN x.QueueStatus = 'Sent' THEN 1 ELSE 0 END) * 100.0
             / NULLIF(COUNT(*), 0) AS DECIMAL(9,2))                          AS SuccessRate
    FROM #A x
    WHERE x.SendTime IS NOT NULL
    GROUP BY DATEPART(HOUR, x.SendTime)
    ORDER BY SendHour;

    -- =================================================================================
    -- RESULT SET 9: PHAN TICH NGUYEN NHAN LOI  (Bar chart - chat luong gui)
    -- =================================================================================
    SELECT
        ISNULL(x.ErrorCode, 0)                                               AS ErrorCode,
        ISNULL(NULLIF(LTRIM(RTRIM(x.ErrorMessage)), ''), N'(Khong co mo ta)') AS ErrorMessage,
        COUNT(*)                                                             AS Quantity,
        COUNT(DISTINCT x.Phone)                                              AS DistinctPhone,
        CAST(COUNT(*) * 100.0 / NULLIF(@Total, 0) AS DECIMAL(9,2))           AS Percentage,
        MAX(x.SendTime)                                                      AS LastOccurredTime
    FROM #A x
    WHERE x.QueueStatus <> 'Sent'
       OR x.ErrorCode IS NOT NULL
       OR x.DeliveryStatus < 0
    GROUP BY x.ErrorCode, ISNULL(NULLIF(LTRIM(RTRIM(x.ErrorMessage)), ''), N'(Khong co mo ta)')
    ORDER BY Quantity DESC;

    -- =================================================================================
    -- RESULT SET 10: CHI TIET TUNG LUOT GUI  (bang chi tiet + xuat Excel)
    -- =================================================================================
    SELECT
        x.QueueId                                                            AS QueueId,
        x.LogId                                                              AS LogId,
        x.Phone                                                              AS Phone,
        ISNULL(x.FullName, N'Khach hang')                                    AS FullName,
        x.TemplateId                                                         AS TemplateId,
        ISNULL(t.TemplateName, CONCAT(N'Template #', CAST(x.TemplateId AS NVARCHAR(30)))) AS TemplateName,
        x.QueueStatus                                                        AS QueueStatus,
        ISNULL(x.LogStatus, '')                                              AS LogStatus,
        x.DeliveryStatus                                                     AS DeliveryStatus,
        ISNULL(x.DeliveryMessage, '')                                        AS DeliveryMessage,
        x.RetryCount                                                         AS RetryCount,
        x.ErrorCode                                                          AS ErrorCode,
        ISNULL(x.ErrorMessage, '')                                           AS ErrorMessage,
        ISNULL(x.MsgId, '')                                                  AS MsgId,
        ISNULL(x.TrackingId, '')                                             AS TrackingId,
        ISNULL(x.SendingMode, '')                                            AS SendingMode,
        ISNULL(x.EventCatId, 0)                                              AS EventCatId,
        ISNULL(ec.CatName, '')                                               AS EventCatName,
        ISNULL(x.EventId, 0)                                                 AS EventId,
        ISNULL(ev.Title, '')                                                 AS EventName,
        x.ScheduledAt                                                        AS ScheduledAt,
        x.StartedAt                                                          AS StartedAt,
        x.CompletedAt                                                        AS CompletedAt,
        x.SentTime                                                           AS SentTime,
        x.WaitSeconds                                                        AS WaitSeconds,
        x.ProcessSeconds                                                     AS ProcessSeconds,
        x.TotalSeconds                                                       AS TotalSeconds,
        ISNULL(t.Price, 0)                                                   AS Price
    FROM #A x
    LEFT JOIN dbo.ZNS_Template  t  ON t.TemplateId = x.TemplateId
    LEFT JOIN dbo.NV_Events_Cat ec ON ec.id        = x.EventCatId
    LEFT JOIN dbo.NV_Events     ev ON ev.id        = x.EventId
    ORDER BY x.QueueId DESC;

    DROP TABLE #A;
    DROP TABLE #MAP;
    DROP TABLE #L;
    DROP TABLE #Q;
END
GO

PRINT 'Done! sp_Marketing_Zalo_Campaign_Analytics created successfully.'
GO
