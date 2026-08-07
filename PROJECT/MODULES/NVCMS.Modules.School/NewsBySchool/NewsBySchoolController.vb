'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.School

    Public Class NewsBySchool_Controller
        Public Sub ClearCache()
            'Clear cache
            DataCache.ClearCache("NewsBySchool_Controller_GetShowNewID")
            DataCache.ClearCache(nvcmsBL.cacheMarketingSchoolDetail)
        End Sub
        Public Sub _Insert(ByVal obj As NewsBySchoolInfo)
            DataProvider.Instance.NewsBySchool_Insert(obj)
            ClearCache()
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal obj As NewsBySchoolInfo)
            DataProvider.Instance.NewsBySchool_Update(obj)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Function _GetByNewID(ByVal NewId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsBySchool_GetByNewID(NewId), GetType(NewsBySchoolInfo))
        End Function
        '------------------------------------------'
        Public Function _GetShowNewID(ByVal Count As Integer) As ArrayList
            Dim stringcache = "NewsBySchool_Controller_GetShowNewID" & Count
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.NewsBySchool_GetShowNewID(Count), GetType(NV_NewsInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.NewsBySchool_GetShowNewID(Count), GetType(NV_NewsInfo))
        End Function
        '------------------------------------------'
        Public Function _GetByID(ByVal Id As Integer) As NewsBySchoolInfo
            Return CType(CBO.FillObject(Of NewsBySchoolInfo)(DataProvider.Instance.NewsBySchool_GetByID(Id), True), NewsBySchoolInfo)
        End Function
        '------------------------------------------'
        Public Sub _Delete(ByVal Id As Integer)
            DataProvider.Instance.NewsBySchool_Delete(Id)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteByNewId(ByVal NewId As Integer)
            DataProvider.Instance.NewsBySchool_DeleteByNewId(NewId)
        End Sub

        '------------------------------------------'
    End Class

End Namespace