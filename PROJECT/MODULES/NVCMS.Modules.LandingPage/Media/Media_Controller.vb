Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :SonNguyen 
'Created Date   :23/07/2016
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LadingPage

    Public Class Media_Controller

        Public Sub _Insert(ByVal TrangLadingPageId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.NVCMS_LadingPage_Media_Insert(TrangLadingPageId, Title, Descreption, MediaLnk, Ordernumber, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal TrangLadingPageId As Integer, ByVal Title As String, Descreption As String, ByVal MediaLnk As String, Ordernumber As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.NVCMS_LadingPage_Media_Update(id, TrangLadingPageId, Title, Descreption, MediaLnk, Ordernumber, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub _UpdateTitle(ByVal id As Integer, ByVal Title As String, Descreption As String)
            DataProvider.Instance.NVCMS_LadingPage_Media_UpdateTitle(id, Title, Descreption)
        End Sub
        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.NVCMS_LadingPage_Media_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer) As Media_Info
            Return CType(CBO.FillObject(Of Media_Info)(DataProvider.Instance.NVCMS_LadingPage_Media_GetByID(id), True), Media_Info)
        End Function

        '------------------------------------------'
        Public Function _GetAll(ByVal TrangLadingPageId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NVCMS_LadingPage_Media_GetAll(TrangLadingPageId), GetType(Media_Info))
        End Function

        '------------------------------------------'
    End Class

End Namespace