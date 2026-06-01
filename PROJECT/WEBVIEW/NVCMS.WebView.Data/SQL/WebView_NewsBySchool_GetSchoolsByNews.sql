-- =============================================
-- SP: Lay danh sach TRUONG lien quan den 1 tin
-- Truyen vao: @NewId
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[WebView_NewsBySchool_GetSchoolsByNews]
    @NewId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.*
    FROM   dbo.Cap_Truong t
    INNER JOIN dbo.NewsBySchool nbs ON nbs.SchoolId = t.Id
    WHERE  nbs.NewId = @NewId;
END
