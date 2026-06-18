Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports Newtonsoft.Json

Namespace NVCMS.Modules.HeThong

    Public Class CapApiClient

        Private Const TOKEN_CACHE_KEY As String = "CAP_API_TOKEN"

        Private Shared ReadOnly cap_api_url As String = ConfigurationManager.AppSettings("cap_api_url")
        Private Shared ReadOnly tokenUrl As String = ConfigurationManager.AppSettings("cap_api_url_login")
        Private Shared ReadOnly username As String = ConfigurationManager.AppSettings("cap_api_user")
        Private Shared ReadOnly password As String = ConfigurationManager.AppSettings("cap_api_password")

        Private Shared ReadOnly _httpClient As HttpClient = CreateHttpClient()
        Private Shared Function CreateHttpClient() As HttpClient
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim handler As New HttpClientHandler()
#If DEBUG Then
            handler.ServerCertificateCustomValidationCallback =
                Function(sender, cert, chain, sslPolicyErrors) True
#End If
            Dim client As New HttpClient(handler)
            client.Timeout = TimeSpan.FromSeconds(30)
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(
                New MediaTypeWithQualityHeaderValue("application/json"))
            Return client
        End Function

        Public Shared Function GetToken() As String
            Dim token = TryCast(HttpRuntime.Cache(TOKEN_CACHE_KEY), String)
            If String.IsNullOrWhiteSpace(token) Then
                token = RefreshToken()
            End If
            Return token
        End Function
        Private Shared Function RefreshToken() As String
            Dim payload = New With {
                .username = username,
                .password = password
            }
            Dim json = JsonConvert.SerializeObject(payload)
            Dim content As New StringContent(
                json,
                Encoding.UTF8,
                "application/json")
            Dim response =
                _httpClient.PostAsync(
                    cap_api_url & tokenUrl,
                    content).GetAwaiter().GetResult()
            Dim responseBody =
                response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
            If Not response.IsSuccessStatusCode Then
                Throw New Exception(
                    String.Format(
                        "CAP Login Error. Status={0}, Response={1}",
                        CInt(response.StatusCode),
                        responseBody))
            End If
            Dim obj =
                JsonConvert.DeserializeObject(Of LoginResponse)(
                    responseBody)
            If obj Is Nothing _
                OrElse Not obj.Success _
                OrElse obj.Data Is Nothing _
                OrElse String.IsNullOrWhiteSpace(obj.Data.Token) Then
                Throw New Exception(
                    "Không lấy được token. Response: " &
                    responseBody)
            End If
            Dim token = obj.Data.Token
            Dim expireTime As DateTime
            If DateTime.TryParse(obj.Data.Expiration, expireTime) Then
                HttpRuntime.Cache.Insert(
                    TOKEN_CACHE_KEY,
                    token,
                    Nothing,
                    expireTime.AddMinutes(-5),
                    TimeSpan.Zero)
            Else
                HttpRuntime.Cache.Insert(
                    TOKEN_CACHE_KEY,
                    token,
                    Nothing,
                    DateTime.Now.AddHours(23),
                    TimeSpan.Zero)
            End If
            Return token

        End Function

        Public Shared Sub ClearToken()
            HttpRuntime.Cache.Remove(TOKEN_CACHE_KEY)
        End Sub

        Public Shared Function Post(Of T)(
            apiPath As String,
            requestObject As Object) As T
            Return PostInternal(Of T)(
                apiPath,
                requestObject,
                True)

        End Function

        Private Shared Function PostInternal(Of T)(
            apiPath As String,
            requestObject As Object,
            retry As Boolean) As T
            Dim token = GetToken()
            Dim jsonBody = JsonConvert.SerializeObject(requestObject)
            Dim request As New HttpRequestMessage(HttpMethod.Post, cap_api_url & apiPath)
            request.Headers.Authorization = New AuthenticationHeaderValue("Bearer", token)
            request.Content =
                New StringContent(
                    jsonBody,
                    Encoding.UTF8,
                    "application/json")
            Dim response = _httpClient.SendAsync(request).GetAwaiter().GetResult()
            Dim responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
            If response.StatusCode = HttpStatusCode.Unauthorized AndAlso retry Then
                ClearToken()
                Return PostInternal(Of T)(
                    apiPath,
                    requestObject,
                    False)
            End If
            If Not response.IsSuccessStatusCode Then
                Throw New Exception(
                    String.Format(
                        "CAP API Error. Status={0}, Response={1}",
                        CInt(response.StatusCode),
                        responseJson))
            End If
            Return JsonConvert.DeserializeObject(Of T)(responseJson)
        End Function
    End Class
End Namespace