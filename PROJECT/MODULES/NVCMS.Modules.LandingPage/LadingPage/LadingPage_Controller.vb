Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :SonNguyen 
'Created Date   :23/07/2016
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LadingPage

    Public Class LadingPage_Controller

        Public Sub _Insert(ByVal obj As LadingPage_Info)
            DataProvider.Instance.LadingPage_Insert(obj)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal obj As LadingPage_Info)
            DataProvider.Instance.LadingPage_Update(obj)
        End Sub

        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.LadingPage_Delete(id, PortalId)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer, ByVal PortalId As Integer) As LadingPage_Info
            Return CType(CBO.FillObject(Of LadingPage_Info)(DataProvider.Instance.LadingPage_GetByID(id, PortalId), True), LadingPage_Info)
        End Function

        '------------------------------------------'
        Public Function _GetAll(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.LadingPage_GetAll(PortalId), GetType(LadingPage_Info))
        End Function
        '------------------------------------------'
        Public Function _GetAllByParentId(ByVal ParentId As Integer, ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.LadingPage_GetAllByParentId(ParentId, PortalId), GetType(LadingPage_Info))
        End Function
        '------------------------------------------'
    End Class

End Namespace