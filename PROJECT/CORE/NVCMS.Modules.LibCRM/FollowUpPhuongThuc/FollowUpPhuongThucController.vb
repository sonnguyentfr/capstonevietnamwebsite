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

    Public Class FollowUpPhuongThucController
        '------------------------------------------'
        Public Sub FollowUpPhuongThuc_CRUD(ByVal Action As String, ByVal id As Integer, ByVal PhuongThuc As String, ByVal ParentId As Integer, isShow As Boolean, IsActive As Boolean, ByVal UserId As Integer)
            DataProvider.Instance.FollowUpPhuongThuc_CRUD(Action, id, PhuongThuc, ParentId, isShow, IsActive, UserId)
            DataCache.ClearCache(nvcmsBL.cacheLibFollowUpPhuongThuc)
        End Sub
        '------------------------------------------'
        Public Function FollowUpPhuongThuc_GetAll() As ArrayList
            Dim stringcache = nvcmsBL.cacheLibFollowUpPhuongThuc
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.FollowUpPhuongThuc_GetAll(), GetType(FollowUpPhuongThucInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        '------------------------------------------'
        Public Function FollowUpPhuongThuc_GetByID(ByVal id As Integer) As FollowUpPhuongThucInfo
            Return CType(CBO.FillObject(Of FollowUpPhuongThucInfo)(DataProvider.Instance.FollowUpPhuongThuc_GetById(id), True), FollowUpPhuongThucInfo)
        End Function
        '------------------------------------------'
        Public Function FollowUpPhuongThuc_GetByParentID(ByVal Parentid As Integer) As ArrayList
            Dim stringcache = nvcmsBL.cacheLibFollowUpPhuongThuc & Parentid
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.FollowUpPhuongThuc_GetById(Parentid), GetType(FollowUpPhuongThucInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        '------------------------------------------'
        Public Function FollowUpPhuongThuc_FindCount(ByVal Title As String) As Integer
            Dim stringcache = nvcmsBL.cacheLibFollowUpPhuongThuc & "count"
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim totalCount = DataProvider.Instance.FollowUpPhuongThuc_FindCount(Title)
                DataCache.SetCache(stringcache, totalCount)
            End If
            Return DataCache.GetCache(stringcache)
            'Return DataProvider.Instance.FollowUpPhuongThuc_FindCount(Title)
        End Function
        ''------------------------------------------'
        Public Function FollowUpPhuongThuc_FindIndex(Title As String, PageIndex As Integer, PageSize As Integer) As ArrayList

            Dim stringcache = nvcmsBL.cacheLibFollowUpPhuongThuc & Title & PageIndex & PageSize
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.FollowUpPhuongThuc_FindIndex(Title, PageIndex, PageSize), GetType(FollowUpPhuongThucInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.FollowUpPhuongThuc_FindIndex(Title, PageIndex, PageSize), GetType(FollowUpPhuongThucInfo))
        End Function
    End Class

End Namespace