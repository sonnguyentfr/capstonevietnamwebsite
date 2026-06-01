-- =============================================
-- SP: Lay danh sach TIN TUC lien quan den 1 truong
-- Truyen vao: @SchoolId
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[WebView_NewsBySchool_GetNewsBySchool]
    @SchoolId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT n.*
    FROM   dbo.NV_News n
    INNER JOIN dbo.NewsBySchool nbs ON nbs.NewId = n.NewId
    WHERE  nbs.SchoolId = @SchoolId
      AND  n.IsActive = 1
    ORDER BY n.PublishedDate DESC;
END
