Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.LibCRM

    Public Class LibMajorController
        Private Sub Clearchace()
            DataCache.ClearCache("School_MajorCache")
        End Sub
        '------------------------------------------'
        Public Function Major_GetByID(ByVal id As Integer) As LibMajorInfo
            Return CType(CBO.FillObject(Of LibMajorInfo)(DataProvider.Instance.Major_GetByID(id), True), LibMajorInfo)
        End Function

        '------------------------------------------'
        Public Function Major_GetAll() As ArrayList

            Dim stringcache = "School_MajorCache"
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Major_GetAll(), GetType(LibMajorInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Major_GetAll(), GetType(LibMajorInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace