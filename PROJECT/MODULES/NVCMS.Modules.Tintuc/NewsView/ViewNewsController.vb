'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/27/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class ViewNewsController

        Public Sub Insert(ByVal obj As ViewNewsInfo)
            DataProvider.Instance.NV_ViewNews_Insert(obj.UserId, obj.NewsId)
        End Sub

        Public Sub Update(ByVal obj As ViewNewsInfo)
            DataProvider.Instance.NV_ViewNews_Update(obj.Id, obj.UserId, obj.NewsId)
        End Sub

        Public Sub Delete(ByVal Id As Integer)
            DataProvider.Instance.NV_ViewNews_Delete(Id)
        End Sub

        Public Function GetByID(ByVal Id As Integer) As ViewNewsInfo
            Return CType(CBO.FillObject(Of ViewNewsInfo)(DataProvider.Instance.NV_ViewNews_GetByID(Id), True), ViewNewsInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_ViewNews_GetAll(), GetType(ViewNewsInfo))
        End Function

        Public Function GetByNewsId(ByVal NewsId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_ViewNews_GetByNewsId(NewsId), GetType(ViewNewsInfo))
        End Function

        Public Function GetByUserId(ByVal userid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_ViewNews_GetByUserId(userid), GetType(ViewNewsInfo))
        End Function

        Public Function GetByNewsIdAndUserId(ByVal NewsId As Integer, ByVal userid As Integer) As ViewNewsInfo
            Return CType(CBO.FillObject(Of ViewNewsInfo)(DataProvider.Instance.NV_ViewNews_GetByNewsIdAndUserId(NewsId, userid), True), ViewNewsInfo)
        End Function
    End Class
End Namespace