Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports DotNetNuke.Services.Scheduling
Imports Newtonsoft.Json
Imports NVCMS.Modules.EventsWebsite

Namespace NVCMS.Modules.Scheduler

    Public Class ImportCrmDataScheduledJob
        Inherits SchedulerClient

        Dim _EventsWebsite_CatController As New EventsWebsite_CatController

        Private Shared cap_api_url As String = ConfigurationManager.AppSettings("cap_api_url")
        Private Shared cap_api_url_readgoooglesheet As String = ConfigurationManager.AppSettings("cap_api_url_readgoooglesheet")

        ' HttpClient dùng 1 lần duy nhất
        Private Shared ReadOnly http As New HttpClient() With {
            .Timeout = TimeSpan.FromSeconds(30)
        }

        Public Sub New(item As ScheduleHistoryItem)
            MyBase.New()
            Me.ScheduleHistoryItem = item
        End Sub


        ' ==========================
        ' ENTRY POINT (sync)
        ' ==========================
        Public Overrides Sub DoWork()
            ' Chạy async
            DoWorkAsync().GetAwaiter().GetResult()
        End Sub
        ' ==========================
        ' HÀM CHÍNH – ASYNC
        ' ==========================
        Private Async Function DoWorkAsync() As Task
            Try
                ScheduleHistoryItem.AddLogNote("Job bắt đầu...")

                Dim events = GetOnlineEvents()
                Dim token As String = TokenService.GetToken()

                For Each ev In events
                    If String.IsNullOrEmpty(ev.link_data_google_sheet) Then
                        ScheduleHistoryItem.AddLogNote($"Event {ev.Id} KHÔNG có Google Sheet ID → bỏ qua.")
                        Continue For
                    End If

                    Await CallImportApi(
                        ev.link_data_google_sheet,
                        ev.link_data_google_sheet_range,
                        token,
                        ev.Id
                    )
                Next

                ScheduleHistoryItem.Succeeded = True
                ScheduleHistoryItem.AddLogNote("Job hoàn thành thành công!")

            Catch ex As Exception
                ScheduleHistoryItem.Succeeded = False
                ScheduleHistoryItem.AddLogNote("Lỗi job tổng: " & ex.ToString())
                Me.Errored(ex)

            Finally
                ScheduleHistoryItem.AddLogNote("Job chạy xong.")
            End Try
        End Function

        ' ==========================
        ' LẤY DANH SÁCH EVENT ONLINE
        ' ==========================
        Private Function GetOnlineEvents() As ArrayList
            Return _EventsWebsite_CatController.Events_Cat_GetAllShowOnline(50)
        End Function

        Private Async Function CallImportApi(spreadsheetId As String, range As String, token As String, eventId As Integer) As Task
            Try
                Dim url = cap_api_url & cap_api_url_readgoooglesheet

                ' Payload gửi lên API
                Dim payload = New With {
                    .spreadsheetId = spreadsheetId,
                    .eventCat_id = eventId,
                    .range = range
                }

                Dim json As String = JsonConvert.SerializeObject(payload)
                Dim content = New StringContent(json, Encoding.UTF8, "application/json")

                ' KHÔNG SET Header static trong HttpClient → phải set trong request này
                Dim req = New HttpRequestMessage(HttpMethod.Post, url)
                req.Content = content
                req.Headers.Authorization =
                    New System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token)

                ScheduleHistoryItem.AddLogNote($"Event {eventId} → Gọi API Import...")

                ' Gọi API
                Dim resp = Await http.SendAsync(req)
                Dim body = Await resp.Content.ReadAsStringAsync()

                ScheduleHistoryItem.AddLogNote($"Event {eventId} - API Status: {resp.StatusCode}")
                ScheduleHistoryItem.AddLogNote($"Event {eventId} - Kết quả: {body}")

            Catch ex As TaskCanceledException
                ScheduleHistoryItem.AddLogNote($"Event {eventId} - TIMEOUT (30s): {ex.Message}")

            Catch ex As Exception
                ScheduleHistoryItem.AddLogNote($"Event {eventId} - Lỗi API: {ex.Message}")
            End Try
        End Function


    End Class
End Namespace
