Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.Marketing
    Public Class Marketing_Zalo_ListSdt_Controller
        Public Function _Insert(ByVal CampaignId As Integer, ByVal PhoneRaw As String, ByVal Phone As String, ByVal Status As Integer, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer) As Integer
            Using dr As System.Data.IDataReader = DataProvider.Instance.Marketing_Zalo_ListSdt_Insert(CampaignId, PhoneRaw, Phone, Status, CreatedDate, UserId, PortalId)
                If dr.Read() Then
                    Return CInt(dr("Result"))
                End If
            End Using
            Return -1
        End Function

        Public Function _InsertBulk(ByVal CampaignId As Integer, ByVal phoneCSV As String, ByVal UserId As Integer, ByVal PortalId As Integer) As Marketing_Zalo_ListSdt_BulkResult
            Dim res As New Marketing_Zalo_ListSdt_BulkResult()
            Using dr As System.Data.IDataReader = DataProvider.Instance.Marketing_Zalo_ListSdt_InsertBulk(CampaignId, phoneCSV, UserId, PortalId)
                If dr.Read() Then
                    res.InsertCount = CInt(dr("InsertCount"))
                    res.DupCount = CInt(dr("DupCount"))
                End If
            End Using
            Return res
        End Function

        Public Sub _Delete(ByVal Id As Integer)
            DataProvider.Instance.Marketing_Zalo_ListSdt_Delete(Id)
        End Sub
        Public Sub _DeleteByCampaignId(ByVal CampaignId As Integer)
            DataProvider.Instance.Marketing_Zalo_ListSdt_DeleteByCampaignId(CampaignId)
        End Sub
        Public Sub _UpdateStatus(ByVal Id As Integer, ByVal Status As Integer)
            DataProvider.Instance.Marketing_Zalo_ListSdt_UpdateStatus(Id, Status)
        End Sub
        Public Function _GetByID(ByVal Id As Integer) As Marketing_Zalo_ListSdtInfo
            Return CType(CBO.FillObject(Of Marketing_Zalo_ListSdtInfo)(DataProvider.Instance.Marketing_Zalo_ListSdt_GetByID(Id), True), Marketing_Zalo_ListSdtInfo)
        End Function
        Public Function _GetAll(ByVal CampaignId As Integer, Optional ByVal KeySearch As String = "", Optional ByVal Status As Integer = -1, Optional ByVal PageIndex As Integer = 0, Optional ByVal PageSize As Integer = 50) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Zalo_ListSdt_GetAll(CampaignId, KeySearch, Status, PageIndex, PageSize), GetType(Marketing_Zalo_ListSdtInfo))
        End Function
        Public Function API_GetAll(ByVal CampaignId As Integer, Optional ByVal KeySearch As String = "", Optional ByVal Status As Integer = -1, Optional ByVal PageIndex As Integer = 0, Optional ByVal PageSize As Integer = 50) As List(Of Marketing_Zalo_ListSdtInfo)
            Dim arr As ArrayList = CBO.FillCollection(DataProvider.Instance.Marketing_Zalo_ListSdt_GetAll(CampaignId, KeySearch, Status, PageIndex, PageSize), GetType(Marketing_Zalo_ListSdtInfo))
            Return arr.Cast(Of Marketing_Zalo_ListSdtInfo).ToList()
        End Function
    End Class
End Namespace