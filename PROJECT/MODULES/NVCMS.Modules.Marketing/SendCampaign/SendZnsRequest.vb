Namespace NVCMS.Modules.Marketing

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