Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :SonNguyen 
'Created Date   :23/07/2016
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Form

    Public Class Form_Controller
#Region "Form"
        Public Sub Form_Insert(ByVal objform As Form_Info)
            DataProvider.Instance.Form_Insert(objform)
        End Sub

        '------------------------------------------'
        Public Sub Form_Update(ByVal objform As Form_Info)
            DataProvider.Instance.Form_Update(objform)
        End Sub
        '------------------------------------------'
        Public Sub Form_Update_Traloi(ByVal id As Integer, ByVal status As String, ByVal repuserid As Integer, ByVal repcreateddate As DateTime, ByVal reptitle As String, ByVal repnoidung As String)
            DataProvider.Instance.Form_Update_Traloi(id, status, repuserid, repcreateddate, reptitle, repnoidung)
        End Sub
        '------------------------------------------'
        Public Sub Form_Delete(ByVal id As Integer)
            DataProvider.Instance.Form_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function Form_GetByID(ByVal id As Integer) As Form_Info
            Return CType(CBO.FillObject(Of Form_Info)(DataProvider.Instance.Form_GetByID(id), True), Form_Info)
        End Function

        '------------------------------------------'
        Public Function Form_GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Form_GetAll(), GetType(Form_Info))
        End Function
        Public Function _Find_Count(subtractIds As String, Type As String, datefrom As DateTime, dateto As DateTime, ByVal noidung As String, ByVal Status As String, ByVal PortalId As Integer) As Integer
            Return DataProvider.Instance.Form_Find_Count(subtractIds, Type, datefrom, dateto, noidung, Status, PortalId)
        End Function
        '------------------------------------------'
        Public Function _Find_Index(subtractIds As String, Type As String, datefrom As DateTime, dateto As DateTime, ByVal noidung As String, ByVal Status As String, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Form_Find_Index(subtractIds, Type, datefrom, dateto, noidung, Status, PortalId, PageIndex, PageSize), GetType(Form_Info))
        End Function
        '------------------------------------------'
#End Region
#Region "Form_Rep"
        '------------------------------------------'
        Public Sub Form_Rep_Insert(ByVal FormId As Integer, ByVal repuserid As Integer, ByVal repcreateddate As DateTime, ByVal reptitle As String, ByVal repnoidung As String, ByVal portalid As Integer)
            DataProvider.Instance.Form_Rep_Insert(FormId, repuserid, repcreateddate, reptitle, repnoidung, portalid)
        End Sub
        '------------------------------------------'
        Public Function Form_Rep_GetAll(FormId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Form_Rep_GetAll(FormId), GetType(Form_Info))
        End Function
#End Region

    End Class

End Namespace