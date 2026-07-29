Imports System.Data
Imports System.Linq
Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.Marketing

Namespace NVCMS.Modules.Marketing

    Public Class MarketingReportController

        ''' <summary>
        ''' Returns analytics for a campaign send (the Static page's "sendid"),
        ''' without relying on a separate report stored procedure.
        ''' </summary>
        Public Function GetDashboard(CampaignSendId As Integer) As DashboardResult
            If CampaignSendId <= 0 Then Throw New ArgumentOutOfRangeException("CampaignSendId")

            Dim sendController As New Mail_Campaign_SendController()
            Dim statistics As CampaignStatistics = sendController.GetStatistics(CampaignSendId)
            Dim logs As List(Of Mail_Send_LogInfo) = sendController.GetSendLogs(CampaignSendId, Nothing, Nothing, 0, 100000, "CreatedDate", "DESC").Logs

            Return New DashboardResult With {
                .Summary = CreateSummary(statistics, logs),
                .Status = sendController.GetStatusDistribution(CampaignSendId).Select(Function(x) New DashboardStatus With {.Status = x.Status, .Total = x.Count}).ToList(),
                .SentTimeline = CreateTimeline(logs, Function(x) x.SentTime),
                .OpenTimeline = CreateTimeline(logs, Function(x) x.OpenedTime),
                .Delay = CreateDelays(logs),
                .Details = logs.Select(Function(x) New DashboardDetail With {
                    .Email = x.Email, .Status = x.Status,
                    .SentTime = ToNullableDate(x.SentTime), .DeliveredTime = ToNullableDate(x.DeliveredTime),
                    .OpenedTime = ToNullableDate(x.OpenedTime), .ClickedTime = ToNullableDate(x.ClickedTime),
                    .OpenSeconds = GetOpenSeconds(x), .ErrorMessage = x.ErrorMessage, .SesMessageId = x.SesMessageId
                }).ToList()
            }
        End Function

        Private Function CreateSummary(statistics As CampaignStatistics, logs As IEnumerable(Of Mail_Send_LogInfo)) As DashboardSummary
            Dim sent = logs.Count(Function(x) x.SentTime <> DateTime.MinValue)
            Dim delivered = logs.Count(Function(x) x.DeliveredTime <> DateTime.MinValue)
            Dim openedLogs = logs.Where(Function(x) x.OpenedTime <> DateTime.MinValue).ToList()
            Dim opened = openedLogs.Count
            Dim clicked = logs.Count(Function(x) x.ClickedTime <> DateTime.MinValue)
            Dim openSeconds = openedLogs.Where(Function(x) x.SentTime <> DateTime.MinValue AndAlso x.OpenedTime >= x.SentTime).
                Select(Function(x) CInt((x.OpenedTime - x.SentTime).TotalSeconds)).ToList()
            Dim totalRecipients = If(statistics.TotalRecipients > 0, statistics.TotalRecipients, logs.Count)

            Return New DashboardSummary With {
                .TotalRecipient = totalRecipients,
                .Queued = Math.Max(0, totalRecipients - sent),
                .Sent = sent, .Delivered = delivered,
                .Opened = opened, .Clicked = clicked,
                .Bounce = statistics.CountBounced, .Complaint = statistics.CountComplaint,
                .Unsubscribe = statistics.CountUnsubscribed,
                .AvgOpenSeconds = If(openSeconds.Any(), CInt(Math.Round(openSeconds.Average())), 0),
                .FastestOpen = If(openSeconds.Any(), openSeconds.Min(), 0),
                .SlowestOpen = If(openSeconds.Any(), openSeconds.Max(), 0),
                .OpenRate = If(sent = 0, 0D, Math.Round(opened * 100D / sent, 2)),
                .ClickRate = If(sent = 0, 0D, Math.Round(clicked * 100D / sent, 2)),
                .BounceRate = If(sent = 0, 0D, Math.Round(statistics.CountBounced * 100D / sent, 2)),
                .ComplaintRate = If(sent = 0, 0D, Math.Round(statistics.CountComplaint * 100D / sent, 2)),
                .CTR = If(opened = 0, 0D, Math.Round(clicked * 100D / opened, 2))
            }
        End Function

        Private Function CreateTimeline(logs As IEnumerable(Of Mail_Send_LogInfo), selector As Func(Of Mail_Send_LogInfo, DateTime)) As List(Of DashboardTimeline)
            Return logs.Where(Function(x) selector(x) <> DateTime.MinValue).
                GroupBy(Function(x) New With {.Ngay = selector(x).Date, .Gio = selector(x).Hour}).
                OrderBy(Function(x) x.Key.Ngay).ThenBy(Function(x) x.Key.Gio).
                Select(Function(x) New DashboardTimeline With {.Ngay = x.Key.Ngay, .Gio = x.Key.Gio, .Total = x.Count()}).ToList()
        End Function

        Private Function CreateDelays(logs As IEnumerable(Of Mail_Send_LogInfo)) As List(Of DashboardDelay)
            Return logs.Where(Function(x) x.SentTime <> DateTime.MinValue AndAlso x.OpenedTime <> DateTime.MinValue AndAlso x.OpenedTime >= x.SentTime).
                GroupBy(Function(x) GetDelayRange((x.OpenedTime - x.SentTime).TotalSeconds)).
                Select(Function(x) New DashboardDelay With {.DelayRange = x.Key, .Total = x.Count()}).
                OrderBy(Function(x) x.DelayRange).ToList()
        End Function

        Private Function GetDelayRange(seconds As Double) As String
            If seconds < 60 Then Return "Under 1 minute"
            If seconds < 300 Then Return "1-5 minutes"
            If seconds < 1800 Then Return "5-30 minutes"
            If seconds < 3600 Then Return "30-60 minutes"
            Return "Over 1 hour"
        End Function

        Private Function ToNullableDate(value As DateTime) As Nullable(Of DateTime)
            Return If(value = DateTime.MinValue, CType(Nothing, Nullable(Of DateTime)), value)
        End Function

        Private Function GetOpenSeconds(log As Mail_Send_LogInfo) As Integer
            If log.SentTime = DateTime.MinValue OrElse log.OpenedTime = DateTime.MinValue OrElse log.OpenedTime < log.SentTime Then Return 0
            Return CInt((log.OpenedTime - log.SentTime).TotalSeconds)
        End Function
        Private Sub ReadSummary(dr As IDataReader,
                                result As DashboardResult)

            If dr.Read() Then

                With result.Summary

                    .TotalRecipient = Null.SetNullInteger(dr("TotalRecipient"))
                    .Queued = Null.SetNullInteger(dr("Queued"))
                    .Sending = Null.SetNullInteger(dr("Sending"))
                    .Sent = Null.SetNullInteger(dr("Sent"))
                    .Delivered = Null.SetNullInteger(dr("Delivered"))
                    .Opened = Null.SetNullInteger(dr("Opened"))
                    .Clicked = Null.SetNullInteger(dr("Clicked"))
                    .Bounce = Null.SetNullInteger(dr("Bounce"))
                    .Complaint = Null.SetNullInteger(dr("Complaint"))
                    .Unsubscribe = Null.SetNullInteger(dr("Unsubscribe"))

                    .AvgOpenSeconds = Null.SetNullInteger(dr("AvgOpenSeconds"))
                    .FastestOpen = Null.SetNullInteger(dr("FastestOpen"))
                    .SlowestOpen = Null.SetNullInteger(dr("SlowestOpen"))

                    If .TotalRecipient > 0 Then

                        .OpenRate = Math.Round(.Opened * 100D / .TotalRecipient, 2)

                        .ClickRate = Math.Round(.Clicked * 100D / .TotalRecipient, 2)

                        .BounceRate = Math.Round(.Bounce * 100D / .TotalRecipient, 2)

                        .ComplaintRate = Math.Round(.Complaint * 100D / .TotalRecipient, 2)

                    End If

                    If .Opened > 0 Then

                        .CTR = Math.Round(.Clicked * 100D / .Opened, 2)

                    End If

                End With

            End If

        End Sub
        Private Sub ReadStatus(dr As IDataReader,
                               result As DashboardResult)

            While dr.Read()

                Dim item As New DashboardStatus()

                item.Status = dr("Status").ToString()

                item.Total = Null.SetNullInteger(dr("Total"))

                result.Status.Add(item)

            End While

        End Sub
        Private Sub ReadSentTimeline(dr As IDataReader,
                                     result As DashboardResult)

            While dr.Read()

                Dim item As New DashboardTimeline()

                item.Ngay = CType(dr("Ngay"), DateTime)

                item.Gio = Convert.ToInt32(dr("Gio"))

                item.Total = Convert.ToInt32(dr("Total"))

                result.SentTimeline.Add(item)

            End While

        End Sub
        Private Sub ReadOpenTimeline(dr As IDataReader,
                                     result As DashboardResult)

            While dr.Read()

                Dim item As New DashboardTimeline()

                item.Ngay = CType(dr("Ngay"), DateTime)

                item.Gio = Convert.ToInt32(dr("Gio"))

                item.Total = Convert.ToInt32(dr("Total"))

                result.OpenTimeline.Add(item)

            End While

        End Sub
        Private Sub ReadDelay(dr As IDataReader,
                              result As DashboardResult)

            While dr.Read()

                Dim item As New DashboardDelay()

                item.DelayRange = dr("DelayRange").ToString()

                item.Total = Convert.ToInt32(dr("Total"))

                result.Delay.Add(item)

            End While

        End Sub
        Private Sub ReadDetail(dr As IDataReader,
                               result As DashboardResult)

            While dr.Read()

                Dim item As New DashboardDetail()

                item.Email = dr("Email").ToString()

                item.Status = dr("Status").ToString()

                If Not IsDBNull(dr("SentTime")) Then
                    item.SentTime = CType(dr("SentTime"), DateTime)
                End If

                If Not IsDBNull(dr("DeliveredTime")) Then
                    item.DeliveredTime = CType(dr("DeliveredTime"), DateTime)
                End If

                If Not IsDBNull(dr("OpenedTime")) Then
                    item.OpenedTime = CType(dr("OpenedTime"), DateTime)
                End If

                If Not IsDBNull(dr("ClickedTime")) Then
                    item.ClickedTime = CType(dr("ClickedTime"), DateTime)
                End If

                If Not IsDBNull(dr("OpenSeconds")) Then
                    item.OpenSeconds = Convert.ToInt32(dr("OpenSeconds"))
                End If

                item.ErrorMessage = If(IsDBNull(dr("ErrorMessage")), "", dr("ErrorMessage").ToString())
                item.SesMessageId = If(IsDBNull(dr("SesMessageId")), "", dr("SesMessageId").ToString())

                result.Details.Add(item)

            End While

        End Sub

    End Class

End Namespace
