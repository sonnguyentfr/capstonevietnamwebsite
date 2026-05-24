'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace NVCMS.Modules.TrangGioiThieu

    Public Class GioiThieuTemplateController
        Public Sub _Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            DataProvider.Instance.NVCMS_PageGioiThieu_Template_Insert(TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            DataProvider.Instance.NVCMS_PageGioiThieu_Template_Update(Id, TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Delete(ByVal Id As Integer, Portalid As Integer)
            DataProvider.Instance.NVCMS_PageGioiThieu_Template_Delete(Id, Portalid)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal Id As Integer, Portalid As Integer) As GioiThieuTemplateInfo
            Return CType(CBO.FillObject(Of GioiThieuTemplateInfo)(DataProvider.Instance.NVCMS_PageGioiThieu_Template_GetByID(Id, Portalid), True), GioiThieuTemplateInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAll(ByVal Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NVCMS_PageGioiThieu_Template_SelectAll(Portalid), GetType(GioiThieuTemplateInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace