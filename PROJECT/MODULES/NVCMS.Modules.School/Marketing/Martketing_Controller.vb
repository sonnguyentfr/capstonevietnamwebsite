Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.School
    Public Class MarketingSchoolController
#Region "Info"


        Public Sub Marketing_Truong_Insert(ByVal objInfo As MarketingSchoolInfo)
            DataProvider.Instance.Marketing_Truong_Insert(objInfo)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheMarketingSchool)
            DataCache.ClearCache(nvcmsBL.cacheMarketingSchoolDetail)
        End Sub

        '------------------------------------------'
        Public Sub Marketing_Truong_Update(ByVal objInfo As MarketingSchoolInfo)
            DataProvider.Instance.Marketing_Truong_Update(objInfo)
            'Clear cache
            DataCache.ClearCache(nvcmsBL.cacheMarketingSchool)
            DataCache.ClearCache(nvcmsBL.cacheMarketingSchoolDetail)
        End Sub

        '------------------------------------------'
        Public Sub Marketing_Truong_Delete(ByVal id As Integer)
            DataProvider.Instance.Marketing_Truong_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function Marketing_Truong_GetByID(ByVal id As Integer) As MarketingSchoolInfo
            Dim stringcache = nvcmsBL.cacheMarketingSchoolDetail & id
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CType(CBO.FillObject(Of MarketingSchoolInfo)(DataProvider.Instance.Marketing_Truong_GetByID(id), True), MarketingSchoolInfo)
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CType(CBO.FillObject(DataProvider.Instance.Marketing_Truong_GetByID(id), GetType(MarketingSchoolInfo)), MarketingSchoolInfo)
        End Function
        '------------------------------------------'
        Public Function Marketing_Truong_Search_Find_Count(ByVal NameofSchool As String, Country As Integer, Loai As Integer, ChiPhi As String, Major As Integer) As Integer
            Return DataProvider.Instance.Marketing_Truong_Search_Find_Count(NameofSchool, Country, Loai, ChiPhi, Major)
        End Function

        '------------------------------------------'
        Public Function Marketing_Truong_Search_Find_Index(ByVal NameofSchool As String, Country As Integer, Loai As Integer, ChiPhi As String, Major As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            'Dim stringcache = nvcmsBL.cacheMarketingSchool & Country & Loai & ChiPhi & Major & PageIndex & PageSize
            'If DataCache.GetCache(stringcache) Is Nothing Then
            '    Dim arrtop = CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Search_Find_Index(Country, Loai, ChiPhi, Major, PageIndex, PageSize), GetType(MarketingSchoolInfo))
            '    DataCache.SetCache(stringcache, arrtop)
            'End If
            'Return DataCache.GetCache(stringcache)
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Search_Find_Index(NameofSchool, Country, Loai, ChiPhi, Major, PageIndex, PageSize), GetType(MarketingSchoolInfo))
        End Function
        '------------------------------------------'
        Public Function Marketing_Truong_Find_Count(ByVal NameofSchool As String, ByVal Website As String, Loai As Integer, Country As Integer, StateCity As Integer) As Integer
            Return DataProvider.Instance.Marketing_Truong_Find_Count(NameofSchool, Website, Loai, Country, StateCity)
        End Function

        '------------------------------------------'
        Public Function Marketing_Truong_Find_Index(ByVal NameofSchool As String, ByVal Website As String, Loai As Integer, Country As Integer, StateCity As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            'Dim stringcache = nvcmsBL.cacheMarketingSchool & NameofSchool & Website & Loai & Country & StateCity & PageIndex & PageSize
            'If DataCache.GetCache(stringcache) Is Nothing Then
            '    Dim arrtop = CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Find_Index(NameofSchool, Website, Loai, Country, StateCity, PageIndex, PageSize), GetType(MarketingSchoolInfo))
            '    DataCache.SetCache(stringcache, arrtop)
            'End If
            'Return DataCache.GetCache(stringcache)
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Find_Index(NameofSchool, Website, Loai, Country, StateCity, PageIndex, PageSize), GetType(MarketingSchoolInfo))
        End Function
        '------------------------------------------'
        Public Function Marketing_Truong_Find_RandomByLoaiCountry(ByVal Top As Integer, Loai As Integer, Country As String) As ArrayList
            'Dim stringcache = nvcmsBL.cacheMarketingSchool & "Random" & Top & Loai & Country
            'If DataCache.GetCache(stringcache) Is Nothing Then
            '    Dim arrtop = CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Find_RandomByLoaiCountry(Top, Loai, Country), GetType(MarketingSchoolInfo))
            '    DataCache.SetCache(stringcache, arrtop)
            'End If
            'Return DataCache.GetCache(stringcache)
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Find_RandomByLoaiCountry(Top, Loai, Country), GetType(MarketingSchoolInfo))
        End Function
        '------------------------------------------'
        Public Sub Marketing_Truong_Version_Insert(ByVal objInfo As MarketingSchoolInfo)
            DataProvider.Instance.Marketing_Truong_Version_Insert(objInfo)
            DataCache.ClearCache(nvcmsBL.cacheMarketingSchool_VerSion)

        End Sub
        Public Function Marketing_Truong_Version_GetAllByTruong(ByVal TruongId As Integer) As ArrayList
            Dim stringcache = nvcmsBL.cacheMarketingSchool_VerSion & TruongId
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Version_GetAllByTruong(TruongId), GetType(MarketingSchoolInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Marketing_Truong_Version_GetAllByTruong(TruongId), GetType(Marketing_Truong_VersionInfo))
        End Function
        Public Function Marketing_Truong_Version_GetByID(ByVal id As Integer) As MarketingSchoolInfo
            Dim stringcache = nvcmsBL.cacheMarketingSchoolDetail & "Verson" & id
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CType(CBO.FillObject(Of MarketingSchoolInfo)(DataProvider.Instance.Marketing_Truong_Version_GetByID(id), True), MarketingSchoolInfo)
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CType(CBO.FillObject(DataProvider.Instance.Marketing_Truong_GetByID(id), GetType(Marketing_Truong_VersionInfo)), Marketing_Truong_VersionInfo)
        End Function
#End Region

    End Class

End Namespace