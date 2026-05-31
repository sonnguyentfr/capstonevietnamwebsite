-- ============================================================
-- SP: WebView_Truong_SearchByLetter
-- Loc truong theo chu cai dau ten (prefix LIKE 'X%')
-- Tra ve 2 result set:
--   1. Total (INT) -- tong so ban ghi khop
--   2. Paged rows  -- danh sach truong theo Page/PageSize
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[WebView_Truong_SearchByLetter]
    @Letter     NCHAR(1)     = NULL,
    @QuocGia    INT          = NULL,
    @Loai       NVARCHAR(10) = NULL,
    @IsPartner  BIT          = NULL,
    @MajorId    INT          = NULL,
    @TuitionMax INT          = NULL,
    @Page       INT          = 1,
    @PageSize   INT          = 12
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    -- Total count
    SELECT COUNT(*)
    FROM   Cap_School t
    WHERE  t.Status = 1
      AND  (@Letter     IS NULL OR t.NameofSchool LIKE @Letter + N'%')
      AND  (@QuocGia    IS NULL OR t.Country      =  @QuocGia)
      AND  (@Loai       IS NULL OR t.Loai         =  @Loai)
      AND  (@IsPartner  IS NULL OR t.isPartner    =  @IsPartner)
      AND  (@MajorId    IS NULL OR EXISTS (
               SELECT 1 FROM Cap_SchoolMajor sm
               WHERE  sm.SchoolId = t.Id AND sm.MajorId = @MajorId))
      AND  (@TuitionMax IS NULL OR
            t.ECUnder      <= @TuitionMax OR
            t.ECass        <= @TuitionMax OR
            t.ECHighschool <= @TuitionMax);

    -- Paged rows
    SELECT t.*
    FROM   Cap_School t
    WHERE  t.Status = 1
      AND  (@Letter     IS NULL OR t.NameofSchool LIKE @Letter + N'%')
      AND  (@QuocGia    IS NULL OR t.Country      =  @QuocGia)
      AND  (@Loai       IS NULL OR t.Loai         =  @Loai)
      AND  (@IsPartner  IS NULL OR t.isPartner    =  @IsPartner)
      AND  (@MajorId    IS NULL OR EXISTS (
               SELECT 1 FROM Cap_SchoolMajor sm
               WHERE  sm.SchoolId = t.Id AND sm.MajorId = @MajorId))
      AND  (@TuitionMax IS NULL OR
            t.ECUnder      <= @TuitionMax OR
            t.ECass        <= @TuitionMax OR
            t.ECHighschool <= @TuitionMax)
    ORDER  BY t.NameofSchool
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;
GO
