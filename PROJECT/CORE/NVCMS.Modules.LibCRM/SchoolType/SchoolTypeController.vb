Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.LibCRM

    Public Class LibSchoolTypeController

        Public Sub Cap_Loaitruong_Insert(ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            DataProvider.Instance.Cap_Loaitruong_Insert(Loaitruong, Descreption, IsActive, Ordernumber, PortalId, CreatedDate)
        End Sub

        '------------------------------------------'
        Public Sub Cap_Loaitruong_Update(ByVal id As Integer, ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            DataProvider.Instance.Cap_Loaitruong_Update(id, Loaitruong, Descreption, IsActive, Ordernumber, PortalId, CreatedDate)
        End Sub
        '------------------------------------------'
        Public Sub Cap_Loaitruong_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
            DataProvider.Instance.Cap_Loaitruong_UpdateOrdernumber(id, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Sub Cap_Loaitruong_Delete(ByVal id As Integer, Portalid As Integer)
            DataProvider.Instance.Cap_Loaitruong_Delete(id, Portalid)
        End Sub

        '------------------------------------------'
        Public Function Cap_Loaitruong_GetByID(ByVal id As Integer, Portalid As Integer) As LibSchoolTypeInfo
            Return CType(CBO.FillObject(Of LibSchoolTypeInfo)(DataProvider.Instance.Cap_Loaitruong_GetByID(id, Portalid), True), LibSchoolTypeInfo)
        End Function

        '------------------------------------------'
        Public Function Cap_Loaitruong_GetAll(Portalid As Integer) As ArrayList
            Dim stringcache = "Cap_Loaitruong_GetAll" & Portalid
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Cap_Loaitruong_GetAll(Portalid), GetType(LibSchoolTypeInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Cap_Loaitruong_GetAll(Portalid), GetType(LibSchoolTypeInfo))
        End Function
        '------------------------------------------'
        Public Function Cap_Loaitruong_GetAllShow(Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Cap_Loaitruong_GetAllShow(Portalid), GetType(LibSchoolTypeInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace