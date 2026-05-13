Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class TrinhDoController
#Region "Propertice"

#End Region
        '------------------------------------------'
        Public Sub TrinhDo_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String)
            DataProvider.Instance.TrinhDo_CRUD(Action, id, Title, TitleEN)
            DataCache.RemoveCache(nvcmsBL.cacheLibTrinhDoAll)
            DataCache.RemoveCache(nvcmsBL.cacheLibTrinhDoAll & "count")
            DataCache.ClearCache(nvcmsBL.cacheLibTrinhDoAll)
        End Sub
        '------------------------------------------'
        Public Function TrinhDo_GetAll() As ArrayList
            Dim stringcache = nvcmsBL.cacheLibTrinhDoAll
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.TrinhDo_GetAll(), GetType(TrinhDoInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        '------------------------------------------'
        Public Function TrinhDo_GetById(Id As Integer) As TrinhDoInfo
            Return CType(CBO.FillObject(Of TrinhDoInfo)(DataProvider.Instance.TrinhDo_GetById(Id), True), TrinhDoInfo)
        End Function
        '------------------------------------------'
        Public Function TrinhDo_GetAllByChoose(ids As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.TrinhDo_GetAllByChoose(ids), GetType(TrinhDoInfo))
        End Function
        '------------------------------------------'
        Public Function TrinhDo_FindCount(ByVal Title As String) As Integer
            Dim stringcache = nvcmsBL.cacheLibTrinhDoAll & "count"
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim totalCount = DataProvider.Instance.TrinhDo_FindCount(Title)
                DataCache.SetCache(stringcache, totalCount)
            End If
            Return DataCache.GetCache(stringcache)
            'Return DataProvider.Instance.TrinhDo_FindCount(Title)
        End Function
        '------------------------------------------'
        Public Function TrinhDo_FindIndex(Title As String, PageIndex As Integer, PageSize As Integer) As ArrayList

            Dim stringcache = nvcmsBL.cacheLibTrinhDoAll & Title & PageIndex & PageSize
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.TrinhDo_FindIndex(Title, PageIndex, PageSize), GetType(TrinhDoInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.TrinhDo_FindIndex(Title, PageIndex, PageSize), GetType(TrinhDoInfo))
        End Function
    End Class

End Namespace