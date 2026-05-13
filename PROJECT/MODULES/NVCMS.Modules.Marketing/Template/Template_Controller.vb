Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Mail_TemplateController

        Public Sub _Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_Template_Insert(TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_Template_Update(Id, TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Delete(ByVal Id As Integer, Portalid As Integer)
            DataProvider.Instance.Marketing_Mail_Template_Delete(Id, Portalid)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal Id As Integer, Portalid As Integer) As Marketing_Mail_TemplateInfo
            Return CType(CBO.FillObject(Of Marketing_Mail_TemplateInfo)(DataProvider.Instance.Marketing_Mail_Template_GetByID(Id, Portalid), True), Marketing_Mail_TemplateInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAll(ByVal Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Mail_Template_SelectAll(Portalid), GetType(Marketing_Mail_TemplateInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace