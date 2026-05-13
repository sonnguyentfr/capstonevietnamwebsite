'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace NVCMS.Modules.Video

    Public Class VideoProcessController

        Public Function Insert(ByVal objInfo As VideoProcessInfo) As Integer
            Return DataProvider.Instance.Video_Process_Insert(objInfo)
        End Function

        Public Sub Update(ByVal objInfo As VideoProcessInfo)
            DataProvider.Instance.Video_Process_Update(objInfo)
        End Sub

        Public Sub Delete(ByVal ID As Integer)
            DataProvider.Instance.Video_Process_Delete(ID)
        End Sub

        Public Function GetById(ByVal ID As Integer) As VideoProcessInfo
            Return CType(CBO.FillObject(Of VideoProcessInfo)(DataProvider.Instance.Video_Process_GetById(ID), True), VideoProcessInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Video_Process_GetAll(), GetType(VideoProcessInfo))
        End Function

        Public Function GetByNewsId(ByVal newsID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Video_Process_GetByNewsId(newsID), GetType(VideoProcessInfo))
        End Function

        Public Function GetCurrentProcess(ByVal newsID As Integer) As VideoProcessInfo
            Return CType(CBO.FillObject(Of VideoProcessInfo)(DataProvider.Instance.Video_Process_GetCurrentProcess(newsID), True), VideoProcessInfo)
        End Function

        Public Function GetLastProcessByStatus(ByVal newsId As Integer, ByVal status As Integer) As VideoProcessInfo
            Return CType(CBO.FillObject(Of VideoProcessInfo)(DataProvider.Instance.Video_Process_GetLastProcessByStatus(newsId, status), True), VideoProcessInfo)
        End Function

        Public Sub DeleteByNewsID(ByVal newsID As Integer)
            DataProvider.Instance.Video_Process_DeleteByNewsID(newsID)
        End Sub
    End Class

End Namespace