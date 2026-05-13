'******************************************
'Author         :SonNguyen
'Created Date   :3/21/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Data
Namespace NVCMS.Modules.Student

    Public Class StudentFollow_LogController
        Public Sub _Follow_Log_Insert(ByVal StudentId As Integer, ByVal Noidung As String, ByVal CreatedDate As DateTime, ByVal PortalId As String)
            DataProvider.Instance._Follow_Log_Insert(StudentId, Noidung, CreatedDate, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Follow_Log_Update(ByVal id As Integer, ByVal StudentId As Integer, ByVal Noidung As String, ByVal CreatedDate As DateTime, ByVal PortalId As String)
            DataProvider.Instance._Follow_Log_Update(id, StudentId, Noidung, CreatedDate, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Follow_Log_Delete(ByVal id As Integer)
            DataProvider.Instance._Follow_Log_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function _Follow_Log_GetByID(ByVal id As Integer) As StudentFollow_LogInfo
            Return CType(CBO.FillObject(Of StudentFollow_LogInfo)(DataProvider.Instance._Follow_Log_GetByID(id), True), StudentFollow_LogInfo)
        End Function
        '------------------------------------------'
        Public Function _Follow_Log_GetByStudentID(ByVal Studentid As Integer) As StudentFollow_LogInfo
            Return CType(CBO.FillObject(Of StudentFollow_LogInfo)(DataProvider.Instance._Follow_Log_GetByStudentID(Studentid), True), StudentFollow_LogInfo)
        End Function
        '------------------------------------------'
        Public Function _Follow_Log_GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Follow_Log_GetAll(), GetType(StudentFollow_LogInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace