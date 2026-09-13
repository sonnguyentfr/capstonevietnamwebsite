Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Marketing

    Public Class Marketing_ZNS_TemplateController

        Public Function _GetByTemplateId(ByVal templateId As Long) As Marketing_ZNS_TemplateInfo
            Return CType(CBO.FillObject(Of Marketing_ZNS_TemplateInfo)(DataProvider.Instance.Marketing_ZNS_Template_GetByTemplateId(templateId), True), Marketing_ZNS_TemplateInfo)
        End Function

        Public Function _GetAll(Optional ByVal onlyActive As Boolean = True) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Marketing_ZNS_Template_SelectAll(onlyActive), GetType(Marketing_ZNS_TemplateInfo))
        End Function

    End Class

End Namespace