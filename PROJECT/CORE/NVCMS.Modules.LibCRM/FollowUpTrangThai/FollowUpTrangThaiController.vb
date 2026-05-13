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

    Public Class FollowUpTrangThaiController

        '=====
        Public Sub FollowUpTrangThai_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Title As String, ByVal ParentId As Integer, isShow As Boolean, isActive As Boolean, ByVal Kyhopdong As Boolean, ByVal UserId As Integer, ByVal CreatedDate As DateTime, Student_NhomId As Integer)
            DataProvider.Instance.Follow_TrangThai_CRUD(Action, id, Title, ParentId, isShow, isActive, Kyhopdong, UserId, CreatedDate, Student_NhomId)
            DataCache.ClearCache(nvcmsBL.cacheLibFollowUpTrangThai)
        End Sub
        '------------------------------------------'
        Public Function Follow_TrangThaI_GetAll(ByVal Kyhopdong As Boolean) As ArrayList
            Dim stringcache = nvcmsBL.cacheLibFollowUpTrangThai & Kyhopdong
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Follow_TrangThaI_GetAll(Kyhopdong), GetType(FollowUpTrangThaiInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        '------------------------------------------'
        Public Function Follow_TrangThai_GetById(ByVal id As Integer) As FollowUpTrangThaiInfo
            Return CType(CBO.FillObject(Of FollowUpTrangThaiInfo)(DataProvider.Instance.Follow_TrangThai_GetById(id), True), FollowUpTrangThaiInfo)
        End Function
        '------------------------------------------'

    End Class

End Namespace