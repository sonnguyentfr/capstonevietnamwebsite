Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Mail_Campaing
        Public Sub _Insert(ByVal Title As String, ByVal Description As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_Campaing_Insert(Title, Description, CreatedDate, UserId, PortalId)
            DataCache.ClearCache("Marketing_Mail_Campaing_GetAll")
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal Title As String, ByVal Description As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_Campaing_Update(id, Title, Description, CreatedDate, UserId, PortalId)
            DataCache.ClearCache("Marketing_Mail_Campaing_GetAll")
        End Sub


        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.Marketing_Mail_Campaing_Delete(id)
            DataCache.ClearCache("Marketing_Mail_Campaing_GetAll")
        End Sub
        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer) As Marketing_Mail_CampaingInfo
            Return CType(CBO.FillObject(Of Marketing_Mail_CampaingInfo)(DataProvider.Instance.Marketing_Mail_Campaing_GetByID(id), True), Marketing_Mail_CampaingInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAll() As ArrayList
            Dim stringcache = "Marketing_Mail_Campaing_GetAll"
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Marketing_Mail_Campaing_GetAll(), GetType(Marketing_Mail_Campaing_ViewInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)


            'Return CBO.FillCollection(DataProvider.Instance.Marketing_Mail_Campaing_GetAll(), GetType(Marketing_Mail_CampaingInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace