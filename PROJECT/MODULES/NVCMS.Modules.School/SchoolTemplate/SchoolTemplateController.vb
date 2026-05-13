'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace NVCMS.Modules.School

    Public Class MarketingSchoolTemplateController
        Public Function GetTemplate(ByVal PortalID As Integer, ByVal TemplateId As Integer) As MarketingSchoolTemplateInfo
            Return CType(CBO.FillObject(Of MarketingSchoolTemplateInfo)(DataProvider.Instance.MarketingSchoolTemplate_Get(PortalID, TemplateId), True), MarketingSchoolTemplateInfo)
        End Function

        Public Function GetTemplates(ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.MarketingSchoolTemplate_GetAll(PortalID), GetType(MarketingSchoolTemplateInfo))
        End Function
        Public Sub InsertTemplate(ByVal TempalteName As String, ByVal FilePath As String, ByVal PortalID As Integer)
            DataProvider.Instance.MarketingSchoolTemplate_Insert(TempalteName, FilePath, PortalID)
        End Sub
        Public Sub UpdateTemplate(ByVal TemplateID As Integer, ByVal TemplateName As String, ByVal FilePath As String)
            DataProvider.Instance.MarketingSchoolTemplate_Update(TemplateID, TemplateName, FilePath)
        End Sub
        Public Sub DeleteTemplate(ByVal TemplateID As Integer)
            DataProvider.Instance.MarketingSchoolTemplate_Delete(TemplateID)
        End Sub
    End Class

End Namespace