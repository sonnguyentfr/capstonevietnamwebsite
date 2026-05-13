'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace NVCMS.Modules.TinTuc

    Public Class NV_NewsTemplateController
        Public Function GetTemplate(ByVal PortalID As Integer, ByVal TemplateId As Integer) As NV_NewsTemplateInfo
            Return CType(CBO.FillObject(Of NV_NewsTemplateInfo)(DataProvider.Instance.News_Template_Get(PortalID, TemplateId), True), NV_NewsTemplateInfo)
        End Function

        Public Function GetTemplates(ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_Template_GetAll(PortalID), GetType(NV_NewsTemplateInfo))
        End Function
        Public Sub InsertTemplate(ByVal TempalteName As String, ByVal FilePath As String, ByVal PortalID As Integer)
            DataProvider.Instance.News_Template_Insert(TempalteName, FilePath, PortalID)
        End Sub
        Public Sub UpdateTemplate(ByVal TemplateID As Integer, ByVal TemplateName As String, ByVal FilePath As String)
            DataProvider.Instance.News_Template_Update(TemplateID, TemplateName, FilePath)
        End Sub
        Public Sub DeleteTemplate(ByVal TemplateID As Integer)
            DataProvider.Instance.News_Template_Delete(TemplateID)
        End Sub
    End Class

End Namespace