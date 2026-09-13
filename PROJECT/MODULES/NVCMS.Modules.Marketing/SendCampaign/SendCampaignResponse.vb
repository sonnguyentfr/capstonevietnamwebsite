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

    Public Class SendZnsResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property data As SendZnsResponseData
        Public Property errorCode As Integer
    End Class

    Public Class SendZnsResponseData
        Public Property templateId As Long
        Public Property msgId As String
        Public Property sentTime As String
        Public Property sendingMode As String
    End Class
End Namespace