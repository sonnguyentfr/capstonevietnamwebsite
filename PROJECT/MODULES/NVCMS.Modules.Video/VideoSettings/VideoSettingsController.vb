'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.Video

    Public Class VideoSettingsController


        Public Sub Insert(ByVal VideoId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Video_Settings_Insert(VideoId, OrderNumber, Type, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub Update(ByVal id As Integer, ByVal VideoId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Video_Settings_Update(id, VideoId, OrderNumber, Type, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub UpdateOrder(ByVal id As Integer, ByVal OrderNumber As Integer)
            DataProvider.Instance.Video_Settings_UpdateOrder(id, OrderNumber)
        End Sub
        '------------------------------------------'
        Public Sub Delete(ByVal Type As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Video_Settings_Delete(Type, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub DeleteByVideoId(ByVal VideoId As Integer, ByVal Type As Integer, ByVal Portalid As Integer)
            DataProvider.Instance.Video_Settings_DeleteByVideoId(VideoId, Type, Portalid)
        End Sub
        '------------------------------------------'
        Public Sub DeleteById(ByVal Id As Integer, PortalId As Integer)
            DataProvider.Instance.Video_Settings_DeleteById(Id, PortalId)
        End Sub
        '------------------------------------------'
        Public Function GetByID(ByVal id As Integer) As VideoSettingsInfo
            Return CType(CBO.FillObject(Of VideoSettingsInfo)(DataProvider.Instance.Video_Settings_GetByID(id), True), VideoSettingsInfo)
        End Function

        '------------------------------------------'
        Public Function GetAll(Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Video_Settings_GetAll(Portalid), GetType(VideoSettingsInfo))
        End Function
        '------------------------------------------'
        Public Function GetAllByType(Type As Integer, Count As Integer, PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Video_Settings_GetAllByType(Type, Count, PortalId), GetType(VideoSettingsInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace