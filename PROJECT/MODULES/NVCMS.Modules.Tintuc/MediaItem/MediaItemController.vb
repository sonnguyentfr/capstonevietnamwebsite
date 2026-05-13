Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Data
Namespace NVCMS.Modules.TinTuc

    Public Class MediaItemController

        Public Function _Insert(ByVal title As String, ByVal filename As String, ByVal forder As String, ByVal MediaUrl As String, ByVal Size As Integer, ByVal extension As String, ByVal createddate As DateTime, ByVal userid As Integer, ByVal portalid As Integer) As Integer
            Return CType(DataProvider.Instance.MediaItem_Insert(title, filename, forder, MediaUrl, Size, extension, createddate, userid, portalid), Integer)
        End Function

        '------------------------------------------'
        Public Sub _UpdateTitle(id As Integer, Title As String)
            DataProvider.Instance.MediaItem_UpdateTitle(id, Title)
        End Sub

        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.MediaItem_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer) As MediaItemInfo
            Return CType(CBO.FillObject(Of MediaItemInfo)(DataProvider.Instance.MediaItem_GetByID(id), True), MediaItemInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAll(PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.MediaItem_GetAll(PortalId), GetType(MediaItemInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace