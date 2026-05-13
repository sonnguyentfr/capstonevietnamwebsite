'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/27/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NV_NewsFeedbackController

        Public Sub Insert(ByVal obj As NV_NewsFeedbackInfo)
            DataProvider.Instance.NV_NewsFeedback_Insert(obj.NewsId, obj.FullName, obj.Email, obj.CreateDate, obj.PhoneNumber, obj.Title, obj.Content, obj.Address, obj.IPTrack, obj.Status)
        End Sub

        '------------------------------------------'
        Public Sub Update(ByVal obj As NV_NewsFeedbackInfo)
            DataProvider.Instance.NV_NewsFeedback_Update(obj.NewsFeedbackId, obj.NewsId, obj.FullName, obj.Email, obj.CreateDate, obj.PhoneNumber, obj.Title, obj.Content, obj.Address, obj.IPTrack, obj.Status)
        End Sub

        '------------------------------------------'
        Public Sub Delete(ByVal NewsFeedbackId As Integer)
            DataProvider.Instance.NV_NewsFeedback_Delete(NewsFeedbackId)
        End Sub

        '------------------------------------------'
        Public Function GetByID(ByVal NewsFeedbackId As Integer) As NV_NewsFeedbackInfo
            Return CType(CBO.FillObject(Of NV_NewsFeedbackInfo)(DataProvider.Instance.NV_NewsFeedback_GetByID(NewsFeedbackId), True), NV_NewsFeedbackInfo)
        End Function

        '------------------------------------------'
        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsFeedback_GetAll(), GetType(NV_NewsFeedbackInfo))
        End Function

        Public Function GetByNewsId(ByVal NewsId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsFeedback_GetByNewsId(NewsId), GetType(NV_NewsFeedbackInfo))
        End Function

        Public Function GetByPortalId(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsFeedback_GetByPortalId(PortalId), GetType(NV_NewsFeedbackInfo))
        End Function

        '------------------------------------------'
        Public Function GetByNewsID_Count(ByVal NewsId As Integer, ByVal Status As Integer) As Integer
            Return DataProvider.Instance.NV_NewsFeedback_GetByNewsId_Count(NewsId, Status)
        End Function
        Public Function GetByNewsID_Index(ByVal NewsId As Integer, ByVal Status As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsFeedback_GetByNewsId_Index(NewsId, Status, PageIndex, PageSize), GetType(NV_NewsFeedbackInfo))
        End Function
    End Class

End Namespace