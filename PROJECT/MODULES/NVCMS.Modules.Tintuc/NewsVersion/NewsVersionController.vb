'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace NVCMS.Modules.TinTuc

    Public Class NewsVersionController

        Public Function Insert(ByVal objInfo As NewsVersionInfo) As Integer
            Return DataProvider.Instance.News_Version_Insert(objInfo)
        End Function

        Public Sub Update(ByVal objInfo As NewsVersionInfo)
            DataProvider.Instance.News_Version_Update(objInfo)
        End Sub

        Public Sub Delete(ByVal Id As Integer)
            DataProvider.Instance.News_Version_Delete(Id)
        End Sub

        Public Function GetById(ByVal Id As Integer) As NewsVersionInfo
            Return CType(CBO.FillObject(Of NewsVersionInfo)(DataProvider.Instance.News_Version_GetById(Id), True), NewsVersionInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_Version_GetAll(), GetType(NewsVersionInfo))
        End Function

        Public Sub DeleteByNewsID(ByVal newsId As Integer)
            DataProvider.Instance.News_Version_DeleteByNewsID(newsId)
        End Sub
    End Class

End Namespace