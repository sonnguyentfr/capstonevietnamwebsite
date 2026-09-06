Imports System.IO
Imports System.Net

Public Class WebsiteClearCache
    Private Const DefaultApiUrl As String = ConfigurationManager.AppSettings("cap_web_api_url")
    Private Const DefaultApiKey As String = ConfigurationManager.AppSettings("cap_web_api_token")

    Public Shared Function ClearCapstoneViewCache() As String
        Return ClearCapstoneViewCache(DefaultApiUrl, DefaultApiKey)
    End Function

    Public Shared Function ClearCapstoneViewCache(ByVal apiUrl As String, ByVal apiKey As String) As String
        Try
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim request As HttpWebRequest = CType(WebRequest.Create(apiUrl), HttpWebRequest)
            request.Method = "POST"
            request.Timeout = 15000
            request.ReadWriteTimeout = 15000
            request.Headers("X-Cache-Api-Key") = apiKey
            request.ContentType = "application/json"
            request.ContentLength = 0

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Return reader.ReadToEnd()
                End Using
            End Using
        Catch ex As WebException
            If ex.Response IsNot Nothing Then
                Using response As HttpWebResponse = CType(ex.Response, HttpWebResponse)
                    Using reader As New StreamReader(response.GetResponseStream())
                        Return "ERROR " & CInt(response.StatusCode).ToString() & ": " & reader.ReadToEnd()
                    End Using
                End Using
            End If
            Return "ERROR: " & ex.Message
        Catch ex As Exception
            Return "ERROR: " & ex.Message
        End Try
    End Function
End Class