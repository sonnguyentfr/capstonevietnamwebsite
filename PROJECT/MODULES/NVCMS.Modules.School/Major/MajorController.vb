Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.School

    Public Class MajorController

        'Public Sub Major_Insert(ByVal Title As String, ByVal TitleVN As String, ByVal UserId As Integer, ByVal PortalId As Integer)
        '    DataProvider.Instance.Major_Insert(Title, TitleVN, UserId, PortalId)
        'End Sub

        ''------------------------------------------'
        'Public Sub Major_Update(ByVal id As Integer, ByVal Title As String, ByVal TitleVN As String, ByVal UserId As Integer, ByVal PortalId As Integer)
        '    DataProvider.Instance.Major_Update(id, Title, TitleVN, UserId, PortalId)
        'End Sub

        ''------------------------------------------'
        'Public Sub Major_Delete(ByVal id As Integer)
        '    DataProvider.Instance.Major_Delete(id)
        'End Sub

        ''------------------------------------------'
        'Public Function Major_GetByID(ByVal id As Integer) As MajorInfo
        '    Return CType(CBO.FillObject(DataProvider.Instance.Major_GetByID(id), GetType(MajorInfo)), MajorInfo)
        'End Function

        ''------------------------------------------'
        Public Function Major_GetAll() As ArrayList
            Dim stringcache = nvcmsBL.cacheLibMajor
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Major_GetAll(), GetType(MajorInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Major_GetAll(), GetType(MajorInfo))
        End Function
        Public Function Major_GetAllByAlphaBet(ByVal key As String) As ArrayList
            Dim stringcache = nvcmsBL.cacheLibMajor
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Major_GetAllByAlphaBet(key), GetType(MajorInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Major_GetAll(), GetType(MajorInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace