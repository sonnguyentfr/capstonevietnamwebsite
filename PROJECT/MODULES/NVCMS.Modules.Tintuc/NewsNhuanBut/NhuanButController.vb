'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.TinTuc

    Public Class NhuanButController

        Public Sub NhuanBut_Insert(ByVal NewId As Integer, ByVal Type As Integer, ByVal UserId As Integer, ByVal Credit As Integer, ByVal Createdate As DateTime, ByVal CreateUser As Integer, ByVal UserChamNhuanBut As Integer, ByVal PortalId As Integer, KieuNhuanBut As Integer)
            DataProvider.Instance.News_NhuanBut_Insert(NewId, Type, UserId, Credit, Createdate, CreateUser, UserChamNhuanBut, PortalId, KieuNhuanBut)
        End Sub

        '------------------------------------------'
        Public Sub NhuanBut_Update(ByVal id As Integer, ByVal NewId As Integer, ByVal Type As Integer, ByVal UserId As Integer, ByVal Credit As Integer, ByVal Createdate As DateTime, ByVal CreateUser As Integer, ByVal UserChamNhuanBut As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.News_NhuanBut_Update(id, NewId, Type, UserId, Credit, Createdate, CreateUser, UserChamNhuanBut, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub NhuanBut_UpdateNhuan(ByVal id As Integer, ByVal Credit As Integer, ByVal UserChamNhuanBut As Integer)
            DataProvider.Instance.News_NhuanBut_UpdateNhuan(id, Credit, UserChamNhuanBut)
        End Sub
        '------------------------------------------'
        Public Sub NhuanBut_UpdateNhuanXuatBan(ByVal NewId As Integer, ByVal UserChamNhuanBut As Integer, UserChamNhuanButDate As DateTime, XuatBan As Boolean, KieuNhuanBut As Integer)
            DataProvider.Instance.News_NhuanBut_UpdateNhuanXuatBan(NewId, UserChamNhuanBut, UserChamNhuanButDate, XuatBan, KieuNhuanBut)
        End Sub
        '------------------------------------------'
        Public Sub NhuanBut_Delete(ByVal id As Integer)
            DataProvider.Instance.News_NhuanBut_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function NhuanBut_GetByID(ByVal id As Integer) As NhuanButInfo
            Return CType(CBO.FillObject(Of NhuanButInfo)(DataProvider.Instance.News_NhuanBut_GetByID(id), True), NhuanButInfo)
        End Function

        '------------------------------------------'
        Public Function NhuanBut_GetAll(NewId As Integer, KieuNhuanBut As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_NhuanBut_GetAll(NewId, KieuNhuanBut), GetType(NhuanButInfo))
        End Function
        '------------------------------------------'
        Public Function NhuanBut_GetCount(NewId As Integer, KieuNhuanBut As Integer) As Integer
            Return CType(DataProvider.Instance.News_NhuanBut_GetCount(NewId, KieuNhuanBut), Integer)
        End Function
        '------------------------------------------'
        Public Function NhuanBut_GetTongTien(NewId As Integer, KieuNhuanBut As Integer) As Integer
            Return CType(DataProvider.Instance.News_NhuanBut_GetTongTien(NewId, KieuNhuanBut), Integer)
        End Function
        '------------------------------------------'
        Public Function NhuanBut_Find_Count(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, KieuNhuanBut As Integer) As Integer
            Return DataProvider.Instance.News_NhuanBut_Find_Count(datefrom, dateto, UserId, type, PortalId, KieuNhuanBut)
        End Function
        Public Function NhuanBut_Find_Index(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, KieuNhuanBut As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_NhuanBut_Find_Index(datefrom, dateto, UserId, type, PortalId, PageIndex, PageSize, KieuNhuanBut), GetType(NhuanButInfo))
        End Function
        Public Function NhuanBut_User_GetTongTien(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer) As Integer
            Return DataProvider.Instance.NhuanBut_User_GetTongTien(datefrom, dateto, UserId)
        End Function
    End Class

End Namespace