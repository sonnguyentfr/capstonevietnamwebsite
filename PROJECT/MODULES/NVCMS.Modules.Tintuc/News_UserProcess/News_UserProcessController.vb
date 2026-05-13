'******************************************
'Author         :DuongNQ
'Created Date   :3/25/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Data
Namespace NVCMS.Modules.TinTuc
    Public Class News_UserProcessController
        Public Function Insert(ByVal objInfo As News_UserProcessInfo) As Integer
            Return DataProvider.Instance.News_UserProcess_Insert(objInfo)
        End Function

        Public Sub Update(ByVal objInfo As News_UserProcessInfo)
            DataProvider.Instance.News_UserProcess_Update(objInfo)
        End Sub

        Public Sub Delete(ByVal ID As Integer)
            DataProvider.Instance.News_UserProcess_Delete(ID)
        End Sub

        Public Function GetById(ByVal ID As Integer) As News_UserProcessInfo
            Return CType(CBO.FillObject(Of News_UserProcessInfo)(DataProvider.Instance.News_UserProcess_GetById(ID), True), News_UserProcessInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_UserProcess_GetAll(), GetType(News_UserProcessInfo))
        End Function

        Public Function GetByUserId(ByVal UserId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_UserProcess_GetByUserId(UserId), GetType(News_UserProcessInfo))
        End Function

        Public Sub DeleteByNewsID(ByVal NewsID As Integer)
            DataProvider.Instance.News_UserProcess_DeleteByNewsID(NewsID)
        End Sub
    End Class
End Namespace
