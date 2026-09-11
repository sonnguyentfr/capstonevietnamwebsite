Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Zalo_Campaign_Controller

        Public Function _Insert(ByVal Title As String, ByVal Description As String,
                                ByVal Status As Integer, ByVal CreatedDate As DateTime,
                                ByVal UserId As Integer, ByVal PortalId As Integer) As Integer
            Dim newId As Integer = 0
            Using dr As System.Data.IDataReader = DataProvider.Instance.Marketing_Zalo_Campaign_Insert(Title, Description, Status, CreatedDate, UserId, PortalId)
                If dr.Read() Then newId = CInt(dr("NewId"))
            End Using
            DataCache.ClearCache("Marketing_Zalo_Campaign_GetAll_" & PortalId)
            Return newId
        End Function

        Public Sub _Update(ByVal Id As Integer, ByVal Title As String, ByVal Description As String,
                           ByVal Status As Integer, ByVal UserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Zalo_Campaign_Update(Id, Title, Description, Status, UserId, PortalId)
            DataCache.ClearCache("Marketing_Zalo_Campaign_GetAll_" & PortalId)
        End Sub

        Public Sub _Delete(ByVal Id As Integer)
            DataProvider.Instance.Marketing_Zalo_Campaign_Delete(Id)
        End Sub

        Public Function _GetByID(ByVal Id As Integer) As Marketing_Zalo_CampaignInfo
            Return CType(CBO.FillObject(Of Marketing_Zalo_CampaignInfo)(DataProvider.Instance.Marketing_Zalo_Campaign_GetByID(Id), True), Marketing_Zalo_CampaignInfo)
        End Function

        Public Function _GetAll(ByVal PortalId As Integer) As ArrayList
            Dim cacheKey As String = "Marketing_Zalo_Campaign_GetAll_" & PortalId
            If DataCache.GetCache(cacheKey) Is Nothing Then
                Dim arr = CBO.FillCollection(DataProvider.Instance.Marketing_Zalo_Campaign_GetAll(PortalId), GetType(Marketing_Zalo_CampaignInfo))
                DataCache.SetCache(cacheKey, arr)
            End If
            Return DataCache.GetCache(cacheKey)
        End Function

        Public Function API_GetAll(ByVal PortalId As Integer) As List(Of Marketing_Zalo_CampaignInfo)
            Dim arr As ArrayList = CBO.FillCollection(DataProvider.Instance.Marketing_Zalo_Campaign_GetAll(PortalId), GetType(Marketing_Zalo_CampaignInfo))
            Return arr.Cast(Of Marketing_Zalo_CampaignInfo).ToList()
        End Function

    End Class

End Namespace

