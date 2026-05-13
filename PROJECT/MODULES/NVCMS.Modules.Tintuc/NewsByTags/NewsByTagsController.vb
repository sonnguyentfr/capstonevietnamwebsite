'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NewsByTagsController

        Public Sub NewsByTags_Insert(ByVal NewId As Integer, ByVal Tags As String, TagsTitle As String, ByVal PortalId As Integer)
            DataProvider.Instance.NewsByTags_Insert(NewId, Tags, TagsTitle, PortalId)
        End Sub

        Public Sub NewsByTags_DeleteByNewId(ByVal NewId As Integer)
            DataProvider.Instance.NewsByTags_DeleteByNewId(NewId)
        End Sub
        '------------------------------------------'
        Public Function NewsByTags_GetByTags_Index(ByVal Tags As String, PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsByTags_GetByTags_Index(Tags, PortalId, PageIndex, PageSize), GetType(NV_NewsInfo))
        End Function
        '------------------------------------------'
        Public Function NewsByTags_GetByTags_Count(ByVal Tags As String, PortalId As Integer) As Integer
            Return DataProvider.Instance.NewsByTags_GetByTags_Count(Tags, PortalId)
        End Function
        '------------------------------------------'
        Public Function NewsByTags_GetByNewID(ByVal NewId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsByTags_GetByNewId(NewId), GetType(NewsByTagsInfo))
        End Function
        '------------------------------------------'
        Public Function NewsByTags_GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsByTags_GetAll(), GetType(NewsByTagsInfo))
        End Function
        '------------------------------------------'
        Public Function NewsByTags_GetAllAutoComplate() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsByTags_GetAllAutoComplate(), GetType(NewsByTagsInfo))
        End Function
        '------------------------------------------'
        Public Function NewsByTags_GetByTags(ByVal subtractIds As String, Tags As String, Count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsByTags_GetByTags(subtractIds, Tags, Count), GetType(NewsByTagsInfo))
        End Function

    End Class

End Namespace