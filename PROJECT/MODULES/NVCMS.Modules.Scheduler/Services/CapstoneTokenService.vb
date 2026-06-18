Imports System.Configuration
Imports System.Net
Imports Newtonsoft.Json

Public Class TokenService
    Private Shared token As String = Nothing
    Private Shared expiration As DateTime = DateTime.MinValue

    Private Shared cap_api_url As String = ConfigurationManager.AppSettings("cap_api_url")
    Private Shared tokenUrl As String = ConfigurationManager.AppSettings("cap_api_url_login")
    Private Shared username As String = ConfigurationManager.AppSettings("cap_api_user")
    Private Shared password As String = ConfigurationManager.AppSettings("cap_api_password")


    Public Shared Function GetToken() As String
        If String.IsNullOrEmpty(token) OrElse DateTime.Now >= expiration Then
            RefreshToken()
        End If
        Return token
    End Function


    Private Shared Sub RefreshToken()
        Try
            ' 🔥 Bypass SSL certificate error
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            ServicePointManager.ServerCertificateValidationCallback =
            Function(sender, certificate, chain, sslPolicyErrors) True

            Dim payload = New With {
            .username = username,
            .password = password
        }

            Dim json As String = JsonConvert.SerializeObject(payload)

            Using client As New WebClient()
                client.Headers(HttpRequestHeader.ContentType) = "application/json"
                client.Headers(HttpRequestHeader.Accept) = "*/*"

                Dim urlfull As String = cap_api_url & tokenUrl
                Dim response As String = client.UploadString(urlfull, "POST", json)

                Dim obj = JsonConvert.DeserializeObject(Of LoginResponse)(response)

                If obj IsNot Nothing AndAlso obj.success AndAlso obj.data IsNot Nothing Then
                    token = obj.data.token
                    expiration = DateTime.Parse(obj.data.expiration)
                Else
                    Throw New Exception("Không lấy được token từ API")
                End If
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub


    Private Class LoginResponse
        Public Property success As Boolean
        Public Property message As String
        Public Property data As TokenData
    End Class

    Private Class TokenData
        Public Property token As String
        Public Property expiration As String
    End Class
End Class
