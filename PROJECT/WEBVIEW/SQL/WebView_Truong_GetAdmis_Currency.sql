-- ================================================================
-- Admis 4Year / BF / ESL: LEFT JOIN Cap_Currency để lấy ký hiệu, viết tắt tiền tệ.
-- currency = 0 (hoặc không khớp) -> các cột currency* = NULL, WebView mặc định USD / $.
-- ================================================================

ALTER PROCEDURE [dbo].[WebView_Truong_GetAdmis4Year]
    @TruongId INT,
    @PortalId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 a.*,
           currency.kyhieu  AS currencyKyHieu,
           currency.viettat AS currencyVietTat,
           currency.Title   AS currencyName
    FROM Cap_Truong_Admis_4Year a
    INNER JOIN Cap_Truong_TruongAdmission ta ON ta.AdmissionID = a.id AND ta.TruongId = @TruongId
    LEFT JOIN Cap_Currency currency ON currency.id = a.currency
    WHERE (@PortalId IS NULL OR a.PortalId = @PortalId);
END
GO

ALTER PROCEDURE [dbo].[WebView_Truong_GetAdmisBF]
    @TruongId INT,
    @PortalId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 a.*,
           currency.kyhieu  AS currencyKyHieu,
           currency.viettat AS currencyVietTat,
           currency.Title   AS currencyName
    FROM Cap_Truong_Admis_BF a
    INNER JOIN Cap_Truong_TruongAdmissionBF ta ON ta.AdmissionID = a.id AND ta.TruongId = @TruongId
    LEFT JOIN Cap_Currency currency ON currency.id = a.currency
    WHERE (@PortalId IS NULL OR a.PortalId = @PortalId);
END
GO

ALTER PROCEDURE [dbo].[WebView_Truong_GetAdmisESL]
    @TruongId INT,
    @PortalId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 a.*,
           currency.kyhieu  AS currencyKyHieu,
           currency.viettat AS currencyVietTat,
           currency.Title   AS currencyName
    FROM Cap_Truong_Admis_ESL a
    INNER JOIN Cap_Truong_TruongAdmissionESL ta ON ta.AdmissionID = a.id AND ta.TruongId = @TruongId
    LEFT JOIN Cap_Currency currency ON currency.id = a.currency
    WHERE (@PortalId IS NULL OR a.PortalId = @PortalId);
END
GO
