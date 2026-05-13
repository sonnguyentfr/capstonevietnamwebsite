Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class LibLocationController

        Public Sub Location_Insert(ByVal Name As String, ByVal ShortName As String, currency As String, currencyName As String, currencycode As String, PostCode As String, ByVal ParentId As Integer, ByVal Status As Boolean, Ordernumber As Integer, ByVal mapLatitude As String, ByVal mapLongitude As String, ByVal Info As String, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            DataProvider.Instance.Location_Insert(Name, ShortName, currency, currencyName, currencycode, PostCode, ParentId, Status, Ordernumber, mapLatitude, mapLongitude, Info, PortalId, CreatedDate)
        End Sub

        '------------------------------------------'
        Public Sub Location_Update(ByVal id As Integer, ByVal Name As String, ByVal ShortName As String, currency As String, currencyName As String, currencycode As String, PostCode As String, ByVal ParentId As Integer, ByVal Status As Boolean, Ordernumber As Integer, ByVal mapLatitude As String, ByVal mapLongitude As String, ByVal Info As String, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            DataProvider.Instance.Location_Update(id, Name, ShortName, currency, currencyName, currencycode, PostCode, ParentId, Status, Ordernumber, mapLatitude, mapLongitude, Info, PortalId, CreatedDate)
        End Sub
        '------------------------------------------'
        Public Sub Location_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Location_UpdateOrdernumber(id, Ordernumber, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub Location_Delete(ByVal id As Integer, Portalid As Integer)
            DataProvider.Instance.Location_Delete(id, Portalid)
        End Sub

        '------------------------------------------'
        Public Function Location_GetByID(ByVal id As Integer, Portalid As Integer) As LibLocationInfo
            Return CType(CBO.FillObject(Of LibLocationInfo)(DataProvider.Instance.Location_GetByID(id, Portalid), True), LibLocationInfo)
        End Function

        '------------------------------------------'
        Public Function Location_GetAll(Portalid As Integer) As ArrayList

            Dim stringcache = "Location_GetAll" & Portalid
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Location_GetAll(Portalid), GetType(LibLocationInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)

            'Return CBO.FillCollection(DataProvider.Instance.Location_GetAll(Portalid), GetType(LibLocationInfo))
        End Function
        '------------------------------------------'
        Public Function Location_SelectByParentId(Parentid As Integer, Portalid As Integer) As ArrayList
            Dim stringcache = "CacheName_DM_Tinh_" & Parentid & Portalid
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Location_SelectByParentId(Parentid, Portalid), GetType(LibLocationInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Location_SelectByParentId(Parentid, Portalid), GetType(LibLocationInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace