'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NV_NewsStatusController

        Public Sub NV_NewsStatus_Insert(ByVal StatusName As String, ByVal Description As String)
            DataProvider.Instance.NV_NewsStatus_Insert(StatusName, Description)
        End Sub

        '------------------------------------------'
        Public Sub NV_NewsStatus_Update(ByVal NewsStatusId As Integer, ByVal StatusName As String, ByVal Description As String)
            DataProvider.Instance.NV_NewsStatus_Update(NewsStatusId, StatusName, Description)
        End Sub

        '------------------------------------------'
        Public Sub NV_NewsStatus_Delete(ByVal NewsStatusId As Integer)
            DataProvider.Instance.NV_NewsStatus_Delete(NewsStatusId)
        End Sub

        '------------------------------------------'
        Public Function NV_NewsStatus_GetByID(ByVal NewsStatusId As Integer) As NV_NewsStatusInfo
            Return CType(CBO.FillObject(Of NV_NewsStatusInfo)(DataProvider.Instance.NV_NewsStatus_GetByID(NewsStatusId), True), NV_NewsStatusInfo)
        End Function

        '------------------------------------------'
        Public Function NV_NewsStatus_GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsStatus_GetAll(), GetType(NV_NewsStatusInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace