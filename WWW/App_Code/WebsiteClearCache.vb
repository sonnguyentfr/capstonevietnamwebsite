
Imports System
Imports System.Configuration
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers

Public NotInheritable Class WebsiteClearCache

    Private Sub New()
    End Sub

    Private Shared ReadOnly ApiUrl As String =
        ConfigurationManager.AppSettings("cap_web_api_url")

    Private Shared ReadOnly ApiKey As String =
        ConfigurationManager.AppSettings("cap_web_api_token")

    Private Shared ReadOnly _httpClient As HttpClient =
        CreateHttpClient()

    Private Shared Function CreateHttpClient() As HttpClient

        ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12

        Dim handler As New HttpClientHandler()

#If DEBUG Then
        handler.ServerCertificateCustomValidationCallback =
            Function(sender, cert, chain, sslPolicyErrors) True
#End If

        Dim client As New HttpClient(handler)

        client.Timeout = TimeSpan.FromSeconds(15)

        client.DefaultRequestHeaders.Accept.Clear()

        client.DefaultRequestHeaders.Accept.Add(
            New MediaTypeWithQualityHeaderValue("application/json"))

        Return client

    End Function

    Public Shared Function ClearCapstoneViewCache() As String

        Return ClearCache(ApiUrl, ApiKey)

    End Function

    Public Shared Function ClearCache(
        ByVal apiUrl As String,
        ByVal apiKey As String
    ) As String

        Try

            '==================================================
            ' Validate
            '==================================================

            If String.IsNullOrWhiteSpace(apiUrl) Then
                Return "ERROR: cap_web_api_url is empty."
            End If

            If String.IsNullOrWhiteSpace(apiKey) Then
                Return "ERROR: cap_web_api_token is empty."
            End If

            '==================================================
            ' Endpoint
            '
            ' POST /api/Cache/clear-all
            '==================================================

            Dim endpoint As String =
                apiUrl.TrimEnd("/"c)

            ' Nếu config chỉ chứa domain:
            ' https://xxx.com
            '
            ' thì tự nối endpoint.
            If Not endpoint.EndsWith(
                "/api/Cache/clear-all",
                StringComparison.OrdinalIgnoreCase) Then

                endpoint &= "/api/Cache/clear-all"

            End If

            '==================================================
            ' Request
            '==================================================

            Using request As New HttpRequestMessage(
                HttpMethod.Post,
                endpoint)

                '==================================================
                ' IMPORTANT:
                '
                ' API đang đọc:
                '
                ' Request.Headers["X-Cache-Api-Key"]
                '
                ' nên phải gửi custom header.
                '==================================================

                request.Headers.TryAddWithoutValidation(
                    "X-Cache-Api-Key",
                    apiKey)

                '==================================================
                ' Send
                '==================================================

                Using response As HttpResponseMessage =
                    _httpClient.SendAsync(request).
                    GetAwaiter().
                    GetResult()

                    Dim responseBody As String = ""

                    If response.Content IsNot Nothing Then

                        responseBody =
                            response.Content.
                            ReadAsStringAsync().
                            GetAwaiter().
                            GetResult()

                    End If

                    '==================================================
                    ' SUCCESS
                    '==================================================

                    If response.IsSuccessStatusCode Then

                        Return responseBody

                    End If

                    '==================================================
                    ' UNAUTHORIZED
                    '==================================================

                    If response.StatusCode =
                        HttpStatusCode.Unauthorized Then

                        Return String.Format(
                            "ERROR 401: Invalid API key. Response={0}",
                            responseBody)

                    End If

                    '==================================================
                    ' OTHER HTTP ERROR
                    '==================================================

                    Return String.Format(
                        "ERROR {0}: {1}",
                        CInt(response.StatusCode),
                        responseBody)

                End Using

            End Using

        Catch ex As Exception

            DotNetNuke.Services.Exceptions.Exceptions.LogException(ex)

            Return "ERROR: CAP API request timeout."

        Catch ex As HttpRequestException

            DotNetNuke.Services.Exceptions.Exceptions.LogException(ex)

            Return "ERROR: CAP API connection failed. " &
                   ex.Message

        Catch ex As Exception

            DotNetNuke.Services.Exceptions.Exceptions.LogException(ex)

            Return "ERROR: " & ex.Message

        End Try

    End Function

End Class
