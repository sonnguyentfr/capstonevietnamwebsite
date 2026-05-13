Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.HeThong
'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class FollowUpTrangThaiNhomController
        '------------------------------------------'
        Public Sub Follow_TrangThaiNhom_CRUD(ByVal Action As String, ByVal id As Integer, ByVal TenNhom As String, ByVal Descreption As String, ByVal Ordernumber As Integer, ByVal Createddate As DateTime, ByVal Userid As Integer)
            DataProvider.Instance.Follow_TrangThaiNhom_CRUD(Action, id, TenNhom, Descreption, Ordernumber, Createddate, Userid)
            DataCache.ClearCache(nvcmsBL.cacheLibFollow_TrangThaiNhom)
        End Sub
        '------------------------------------------'
        Public Function Follow_TrangThaiNhom_GetAll() As ArrayList
            Dim stringcache = nvcmsBL.cacheLibFollow_TrangThaiNhom
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Follow_TrangThaiNhom_GetAll(), GetType(FollowUpTrangThaiNhomInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        '------------------------------------------'
        Public Function Follow_TrangThaiNhom_GetByID(ByVal id As Integer) As FollowUpTrangThaiNhomInfo
            Return CType(CBO.FillObject(Of FollowUpTrangThaiNhomInfo)(DataProvider.Instance.Follow_TrangThaiNhom_GetById(id), True), FollowUpTrangThaiNhomInfo)
        End Function
        '------------------------------------------'

    End Class

End Namespace