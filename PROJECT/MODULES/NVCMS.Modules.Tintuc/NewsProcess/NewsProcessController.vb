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

    Public Class NewsProcessController

        Public Function Insert(ByVal objInfo As NewsProcessInfo) As Integer
            Return DataProvider.Instance.News_Process_Insert(objInfo)
        End Function

        Public Sub Update(ByVal objInfo As NewsProcessInfo)
            DataProvider.Instance.News_Process_Update(objInfo)
        End Sub

        Public Sub Delete(ByVal ID As Integer)
            DataProvider.Instance.News_Process_Delete(ID)
        End Sub

        Public Function GetById(ByVal ID As Integer) As NewsProcessInfo
            Return CType(CBO.FillObject(Of NewsProcessInfo)(DataProvider.Instance.News_Process_GetById(ID), True), NewsProcessInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_Process_GetAll(), GetType(NewsProcessInfo))
        End Function

        Public Function GetByNewsId(ByVal newsID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_Process_GetByNewsId(newsID), GetType(NewsProcessInfo))
        End Function

        Public Function GetCurrentProcess(ByVal newsID As Integer) As NewsProcessInfo
            Return CType(CBO.FillObject(Of NewsProcessInfo)(DataProvider.Instance.News_Process_GetCurrentProcess(newsID), True), NewsProcessInfo)
        End Function

        Public Function GetLastProcessByStatus(ByVal newsId As Integer, ByVal status As Integer) As NewsProcessInfo
            Return CType(CBO.FillObject(Of NewsProcessInfo)(DataProvider.Instance.News_Process_GetLastProcessByStatus(newsId, status), True), NewsProcessInfo)
        End Function

        Public Sub DeleteByNewsID(ByVal newsID As Integer)
            DataProvider.Instance.News_Process_DeleteByNewsID(newsID)
        End Sub
    End Class

End Namespace