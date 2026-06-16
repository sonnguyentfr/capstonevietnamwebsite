Namespace NVCMS.Modules.HeThong

    Public Class LoginResponse

        Public Property Success As Boolean

        Public Property Message As String

        Public Property Data As LoginData

    End Class

    Public Class LoginData

        Public Property Token As String

        Public Property Expiration As DateTime

    End Class

End Namespace