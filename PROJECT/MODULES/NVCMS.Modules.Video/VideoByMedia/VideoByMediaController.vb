'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/27/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.Video

    Public Class VideoByMediaController

        Public Sub _Insert(ByVal videoid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            DataProvider.Instance.VideoByMedia_Insert(videoid, mediaid, createdted, userid, portalid)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal videoid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            DataProvider.Instance.VideoByMedia_Update(id, videoid, mediaid, createdted, userid, portalid)
        End Sub
        '------------------------------------------'
        Public Sub _Updatevideoid(ByVal videoid As Integer, videoidnew As Integer)
            DataProvider.Instance.VideoByMedia_Updatevideoid(videoid, videoidnew)
        End Sub
        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.VideoByMedia_Delete(id)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteByvideoid(ByVal videoid As Integer, Portalid As Integer)
            DataProvider.Instance.VideoByMedia_DeleteByvideoid(videoid, Portalid)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteByMediaId(ByVal Mediaid As Integer)
            DataProvider.Instance.VideoByMedia_DeleteByMediaId(Mediaid)
        End Sub
        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer) As VideoByMediaInfo
            Return CType(CBO.FillObject(Of VideoByMediaInfo)(DataProvider.Instance.VideoByMedia_GetByID(id), True), VideoByMediaInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAllByvideoid(ByVal videoid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.VideoByMedia_GetAllByvideoid(videoid), GetType(VideoByMediaInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace
