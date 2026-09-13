'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Marketing
    Public Class SendCampaignRequest
        Public Property CampaignId As Integer
        Public Property TemplateId As Integer

        Public Property EmailAccountId As Integer

        Public Property Subject As String

        Public Property Body As String
    End Class

    Public Class SendZnsRequest
        Public Property TemplateId As Long
        Public Property Phone As String
        Public Property TemplateData As Dictionary(Of String, Object)
        Public Property Type As String
        Public Property CampaignId As Integer?
        Public Property ContextType As String
        Public Property CreatedBy As String
    End Class
End Namespace