Namespace NVCMS.API.ReadGoogleSheet.Models

    Public Class SmtpSettings

        Public Property Host As String = String.Empty
        Public Property Port As Integer = 587
        Public Property EnableSsl As Boolean = True
        Public Property Username As String = String.Empty
        Public Property Password As String = String.Empty
        Public Property DefaultFromEmail As String = String.Empty
        Public Property DefaultFromName As String = String.Empty

    End Class

End Namespace
