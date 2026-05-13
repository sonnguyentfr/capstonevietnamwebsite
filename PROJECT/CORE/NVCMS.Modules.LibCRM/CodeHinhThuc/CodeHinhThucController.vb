Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class CodeHinhThucController
        '------------------------------------------'
        Public Function Code_HinhThuc_GetByID(ByVal id As Integer) As CodeHinhThucInfo
            Return CType(CBO.FillObject(Of CodeHinhThucInfo)(DataProvider.Instance.Code_HinhThuc_GetById(id), True), CodeHinhThucInfo)
        End Function

        '------------------------------------------'
        Public Sub Code_HinhThuc_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Title As String, ByVal Code As String, ParentId As Integer, ByVal CreatedDate As DateTime, ByVal UserId As Integer)
            DataProvider.Instance.Code_HinhThuc_CRUD(Action, id, Title, Code, ParentId, CreatedDate, UserId)
            DataCache.ClearCache(nvcmsBL.cacheLibCodeHinhThuc)
        End Sub
        '------------------------------------------'
        Public Function Code_HinhThuc_GetAll() As ArrayList
            Dim stringcache = nvcmsBL.cacheLibCodeHinhThuc
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Code_HinhThuc_GetAll(), GetType(CodeHinhThucInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
        End Function
        '------------------------------------------'
        Public Function Code_HinhThuc_FindCount(ByVal Title As String) As Integer
            Dim stringcache = nvcmsBL.cacheLibCodeHinhThuc & "count"
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim totalCount = DataProvider.Instance.Code_HinhThuc_FindCount(Title)
                DataCache.SetCache(stringcache, totalCount)
            End If
            Return DataCache.GetCache(stringcache)
            'Return DataProvider.Instance.Code_HinhThuc_FindCount(Title)
        End Function
        ''------------------------------------------'
        Public Function Code_HinhThuc_FindIndex(Title As String, PageIndex As Integer, PageSize As Integer) As ArrayList

            Dim stringcache = nvcmsBL.cacheLibCodeHinhThuc & Title & PageIndex & PageSize
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Code_HinhThuc_FindIndex(Title, PageIndex, PageSize), GetType(CodeHinhThucInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Code_HinhThuc_FindIndex(Title, PageIndex, PageSize), GetType(Code_HinhThucInfo))
        End Function
    End Class

End Namespace