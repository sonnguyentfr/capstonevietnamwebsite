Imports System.Configuration
Imports System.IO
Imports System.Net
Imports System.Web
Imports Newtonsoft.Json

Namespace NVCMS.Modules.HeThong

    Public Class CapApiClient

        Private Const TOKEN_CACHE_KEY As String = "CAP_API_TOKEN"

        Public Shared Function GetToken() As String

            Dim token As String =
                TryCast(HttpRuntime.Cache(TOKEN_CACHE_KEY), String)

            If Not String.IsNullOrEmpty(token) Then
                Return token
            End If

            Dim apiUrl As String =
                ConfigurationManager.AppSettings("cap_api_url")

            Dim loginUrl As String =
                ConfigurationManager.AppSettings("cap_api_url_login")

            Dim username As String =
                ConfigurationManager.AppSettings("cap_api_user")

            Dim password As String =
                ConfigurationManager.AppSettings("cap_api_password")

            Dim requestBody As String =
                JsonConvert.SerializeObject(New With {
                    .username = username,
                    .password = password
                })

            Dim request As HttpWebRequest =
                CType(WebRequest.Create(apiUrl & loginUrl), HttpWebRequest)

            request.Method = "POST"
            request.ContentType = "application/json"

            Using writer As New StreamWriter(request.GetRequestStream())
                writer.Write(requestBody)
            End Using

            Dim responseText As String

            Using response As HttpWebResponse =
                CType(request.GetResponse(), HttpWebResponse)

                Using reader As New StreamReader(response.GetResponseStream())
                    responseText = reader.ReadToEnd()
                End Using

            End Using

            Dim loginResponse =
                JsonConvert.DeserializeObject(Of LoginResponse)(responseText)

            If loginResponse Is Nothing _
               OrElse loginResponse.Data Is Nothing _
               OrElse String.IsNullOrEmpty(loginResponse.Data.Token) Then

                Throw New Exception("Không lấy được JWT Token.")

            End If

            token = loginResponse.Data.Token

            HttpRuntime.Cache.Insert(
                TOKEN_CACHE_KEY,
                token,
                Nothing,
                DateTime.Now.AddHours(23),
                TimeSpan.Zero)

            Return token

        End Function

        Public Shared Sub ClearToken()

            HttpRuntime.Cache.Remove(TOKEN_CACHE_KEY)

        End Sub
        Public Shared Function Post(Of T)(apiPath As String,
        requestObject As Object) As T

            Return PostInternal(Of T)(apiPath, requestObject, True)

        End Function
        Private Shared Function PostInternal(Of T)(
        apiPath As String,
        requestObject As Object,
        retry As Boolean) As T

            Dim apiUrl As String =
                ConfigurationManager.AppSettings("cap_api_url")

            Dim token As String = GetToken()

            Dim request As HttpWebRequest =
                CType(WebRequest.Create(apiUrl & apiPath), HttpWebRequest)

            request.Method = "POST"
            request.ContentType = "application/json"

            request.Headers.Add("Authorization", "Bearer " & token)

            Dim jsonBody As String =
                JsonConvert.SerializeObject(requestObject)

            Using writer As New StreamWriter(request.GetRequestStream())
                writer.Write(jsonBody)
            End Using

            Try

                Using response As HttpWebResponse =
                    CType(request.GetResponse(), HttpWebResponse)

                    Using reader As New StreamReader(response.GetResponseStream())

                        Dim responseJson = reader.ReadToEnd()

                        Return JsonConvert.DeserializeObject(Of T)(responseJson)

                    End Using

                End Using

            Catch ex As WebException

                If ex.Response IsNot Nothing Then

                    Dim httpResponse =
                        CType(ex.Response, HttpWebResponse)

                    If httpResponse.StatusCode = HttpStatusCode.Unauthorized _
                        AndAlso retry Then

                        ClearToken()

                        Return PostInternal(Of T)(
                            apiPath,
                            requestObject,
                            False)

                    End If

                    Using reader As New StreamReader(httpResponse.GetResponseStream())

                        Throw New Exception(reader.ReadToEnd())

                    End Using

                End If

                Throw

            End Try

        End Function

    End Class

End Namespace