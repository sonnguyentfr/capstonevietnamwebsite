Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Mail_ListMail
#Region "Webform"
        Public Sub _Insert(ByVal CampaingId As Integer, ByVal Email As String, ByVal Status As Boolean, ByVal Datetime As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_ListMail_Insert(CampaingId, Email, Status, Datetime, UserId, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal CampaingId As Integer, ByVal Email As String, ByVal Status As Boolean, ByVal Datetime As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_ListMail_Update(id, CampaingId, Email, Status, Datetime, UserId, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub _UpdateCountSend(ByVal id As String)
            DataProvider.Instance.Marketing_Mail_ListMail_UpdateCountSend(id)
        End Sub
        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.Marketing_Mail_ListMail_Delete(id)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteCampaingId(ByVal CampaingId As Integer)
            DataProvider.Instance.Marketing_Mail_Campaing_DeleteCampaing(CampaingId)
        End Sub
        '------------------------------------------'
        Public Function _ListMail_GetByID(ByVal id As Integer) As Marketing_Mail_ListMailInfo
            Return CType(CBO.FillObject(Of Marketing_Mail_ListMailInfo)(DataProvider.Instance.Marketing_Mail_ListMail_GetByID(id), True), Marketing_Mail_ListMailInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAll(CampaingId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Mail_ListMail_GetAll(CampaingId), GetType(Marketing_Mail_ListMailInfo))
        End Function

        '------------------------------------------'
#End Region
#Region "API"
        '------------------------------------------'
        Public Function API_GetAll(CampaingId As Integer) As List(Of Marketing_Mail_ListMailInfo)
            'Return CBO.FillCollection(DataProvider.Instance.Marketing_Mail_ListMail_GetAll(CampaingId), GetType(Marketing_Mail_ListMailInfo))
            Dim result As ArrayList = CBO.FillCollection(DataProvider.Instance.Marketing_Mail_ListMail_GetAll(CampaingId), GetType(Marketing_Mail_ListMailInfo))
            Return result.Cast(Of Marketing_Mail_ListMailInfo).ToList()
        End Function
#End Region
    End Class

End Namespace