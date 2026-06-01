-- =============================================
-- Bảng liên kết: Tin tức <-> Trường
-- =============================================
CREATE TABLE [dbo].[NewsBySchool] (
    [id]       INT IDENTITY(1,1) NOT NULL,
    [NewId]    INT NULL,
    [SchoolId] INT NULL,
    CONSTRAINT [PK_NewsBySchool] PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE INDEX [IX_NewsBySchool_NewId]    ON [dbo].[NewsBySchool] ([NewId]);
CREATE INDEX [IX_NewsBySchool_SchoolId] ON [dbo].[NewsBySchool] ([SchoolId]);
