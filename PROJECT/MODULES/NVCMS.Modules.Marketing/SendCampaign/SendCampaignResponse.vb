'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Marketing
    Public Class SendCampaignResponse
        Public Property Success As Boolean

        Public Property Message As String

        Public Property CampaignSendId As Integer

        Public Property TotalRecipient As Integer
    End Class
End Namespace