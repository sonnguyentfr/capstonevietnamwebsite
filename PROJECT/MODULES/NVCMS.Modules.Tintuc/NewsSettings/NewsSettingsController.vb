'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NewsSettingsController

        Public Sub ClearCache()
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub
        Public Sub Insert(ByVal NewId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.News_Settings_Insert(NewId, OrderNumber, Type, PortalId)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub

        '------------------------------------------'
        Public Sub Update(ByVal id As Integer, ByVal NewId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.News_Settings_Update(id, NewId, OrderNumber, Type, PortalId)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub
        '------------------------------------------'
        Public Sub UpdateOrder(ByVal id As Integer, ByVal OrderNumber As Integer)
            DataProvider.Instance.News_Settings_UpdateOrder(id, OrderNumber)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheShowBaiMoiDanhMuc)
            DataCache.ClearCache(nvcmsBL.cacheShowGetAllByType)
            DataCache.ClearCache(nvcmsBL.cacheShowIndexNews)
        End Sub
        '------------------------------------------'
        Public Sub Delete(ByVal Type As Integer, PortalId As Integer)
            DataProvider.Instance.News_Settings_Delete(Type, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub DeleteById(ByVal id As Integer, PortalId As Integer)
            DataProvider.Instance.News_Settings_DeleteById(id, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub DeleteByNewId(ByVal NewId As Integer, ByVal Type As Integer, ByVal Portalid As Integer)
            DataProvider.Instance.News_Settings_DeleteByNewId(NewId, Type, Portalid)
        End Sub
        '------------------------------------------'
        Public Function GetByID(ByVal id As Integer) As NewsSettingsInfo
            Return CType(CBO.FillObject(Of NewsSettingsInfo)(DataProvider.Instance.News_Settings_GetByID(id), True), NewsSettingsInfo)
        End Function

        '------------------------------------------'
        Public Function GetAll(Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_Settings_GetAll(Portalid), GetType(NewsSettingsInfo))
        End Function
        '------------------------------------------'
        Public Function GetAllByType(Type As Integer, Count As Integer, PortalId As Integer) As ArrayList
            Dim stringcache = nvcmsBL.cacheShowGetAllByType & Type & Count
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.News_Settings_GetAllByType(Type, Count, PortalId), GetType(NewsSettingsInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.News_Settings_GetAllByType(Type, Count, PortalId), GetType(NewsSettingsInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace