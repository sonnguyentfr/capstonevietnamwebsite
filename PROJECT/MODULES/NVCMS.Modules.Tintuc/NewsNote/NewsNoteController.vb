'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NewsNoteController

        Public Sub News_Note_Insert(ByVal objInfo As NewsNoteInfo)
            DataProvider.Instance.News_Note_Insert(objInfo)
            DataCache.ClearCache(BL.cacheNewsNoteByNewId)
        End Sub
        Public Function News_Note_GetByNewIdTop1(ByVal NewId As Integer) As NewsNoteInfo
            Return CType(CBO.FillObject(Of NewsNoteInfo)(DataProvider.Instance.News_Note_GetbyNewIdTop1(NewId), True), NewsNoteInfo)
        End Function
        '------------------------------------------'
        Public Function News_Note_GetByNewId(ByVal NewId As Integer) As ArrayList
            Dim stringcache = BL.cacheNewsNoteByNewId & NewId
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.News_Note_GetbyNewId(NewId), GetType(NewsNoteInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.News_Note_GetbyNewId(NewId), GetType(NewsNoteInfo))
        End Function
    End Class
End Namespace