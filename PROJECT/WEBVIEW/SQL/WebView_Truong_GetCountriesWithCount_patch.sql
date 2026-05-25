-- SP update: WebView_Truong_GetCountriesWithCount add @IsPartner
IF OBJECT_ID(N'dbo.WebView_Truong_GetCountriesWithCount', N'P') IS NOT NULL DROP PROCEDURE dbo.WebView_Truong_GetCountriesWithCount;
GO
CREATE PROCEDURE dbo.WebView_Truong_GetCountriesWithCount @IsPartner BIT = NULL AS BEGIN SET NOCOUNT ON; SELECT Country AS Id, COUNT(*) AS TruongCount FROM Cap_Truong WHERE Status = 1 AND Country IS NOT NULL AND (@IsPartner IS NULL OR isPartner = @IsPartner) GROUP BY Country ORDER BY Country; END
GO
