Namespace NVCMS.Modules.Marketing

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