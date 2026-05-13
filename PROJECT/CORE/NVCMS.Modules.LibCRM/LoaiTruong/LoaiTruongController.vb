Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class LoaiTruongController
#Region "Propertice"

#End Region
        '------------------------------------------'
        Public Sub LoaiTruong_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            DataProvider.Instance.LoaiTruong_CRUD(Action, id, Loaitruong, Descreption, IsActive, Ordernumber, PortalId, CreatedDate)
            DataCache.RemoveCache(nvcmsBL.cacheLibLoaiTruongAll)
            DataCache.RemoveCache(nvcmsBL.cacheLibLoaiTruongAllShow)
        End Sub
        '------------------------------------------'
        Public Sub LoaiTruong_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
            DataProvider.Instance.LoaiTruong_UpdateOrdernumber(id, Ordernumber)
        End Sub
        Public Function LoaiTruong_GetAll(PortalId As Integer) As ArrayList
            Dim stringcache = nvcmsBL.cacheLibLoaiTruongAll
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.LoaiTruong_GetAll(PortalId), GetType(LoaiTruongInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        Public Function LoaiTruong_GetAllShow(PortalId As Integer) As ArrayList
            Dim stringcache = nvcmsBL.cacheLibLoaiTruongAllShow & PortalId
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.LoaiTruong_GetAllShow(PortalId), GetType(LoaiTruongInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        '------------------------------------------'
        Public Function LoaiTruong_GetById(Id As Integer) As LoaiTruongInfo
            Return CType(CBO.FillObject(Of LoaiTruongInfo)(DataProvider.Instance.LoaiTruong_GetById(Id), True), LoaiTruongInfo)
        End Function
        '------------------------------------------'


    End Class

End Namespace