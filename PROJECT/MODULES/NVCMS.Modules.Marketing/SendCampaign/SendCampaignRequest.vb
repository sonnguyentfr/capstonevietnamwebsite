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

        Public Property EmailAccountId As Integer

        Public Property Subject As String

        Public Property Body As String
    End Class
End Namespace