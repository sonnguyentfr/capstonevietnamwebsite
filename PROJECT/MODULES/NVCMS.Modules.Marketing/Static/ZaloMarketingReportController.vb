Imports System.Data

Namespace NVCMS.Modules.Marketing

    ''' <summary>
    ''' Doc 10 result set cua sp_Marketing_Zalo_Campaign_Analytics va map sang Models.
    ''' </summary>
    Public Class MarketingZaloAnalyticsService

        Private ReadOnly _dataProvider As DataProvider

        Public Sub New()
            _dataProvider = DataProvider.Instance()
        End Sub

        Public Function GetCampaignAnalytics(ByVal campaignId As Integer) As Marketing_Zalo_CampaignAnalyticsResult

            If campaignId <= 0 Then
                Throw New ArgumentException("CampaignId khong hop le.")
            End If

            Dim result As New Marketing_Zalo_CampaignAnalyticsResult()

            Using reader As IDataReader = _dataProvider.Marketing_Zalo_Campaign_Analytics(campaignId)

                ' ==========================================
                ' RESULT SET 1: SUMMARY
                ' ==========================================
                If reader.Read() Then
                    Dim s As New Marketing_Zalo_CampaignSummary()
                    s.CampaignId = GetValue(Of Integer)(reader, "CampaignId")
                    s.Title = GetValue(Of String)(reader, "Title")
                    s.Description = GetValue(Of String)(reader, "Description")
                    s.CampaignStatus = GetValue(Of Integer)(reader, "CampaignStatus")
                    s.CampaignCreatedDate = GetNullable(Of DateTime)(reader, "CampaignCreatedDate")

                    s.TotalPhoneInList = GetValue(Of Integer)(reader, "TotalPhoneInList")
                    s.TotalSend = GetValue(Of Integer)(reader, "TotalSend")
                    s.TotalPhoneTargeted = GetValue(Of Integer)(reader, "TotalPhoneTargeted")

                    s.TotalQueued = GetValue(Of Integer)(reader, "TotalQueued")
                    s.TotalProcessing = GetValue(Of Integer)(reader, "TotalProcessing")
                    s.TotalSent = GetValue(Of Integer)(reader, "TotalSent")
                    s.TotalFailed = GetValue(Of Integer)(reader, "TotalFailed")
                    s.TotalRetry = GetValue(Of Integer)(reader, "TotalRetry")
                    s.TotalRetryCount = GetValue(Of Integer)(reader, "TotalRetryCount")

                    s.TotalDelivered = GetValue(Of Integer)(reader, "TotalDelivered")
                    s.TotalRejected = GetValue(Of Integer)(reader, "TotalRejected")
                    s.TotalPendingDelivery = GetValue(Of Integer)(reader, "TotalPendingDelivery")

                    s.SuccessRate = GetValue(Of Decimal)(reader, "SuccessRate")
                    s.FailRate = GetValue(Of Decimal)(reader, "FailRate")
                    s.PendingRate = GetValue(Of Decimal)(reader, "PendingRate")
                    s.DeliveryRate = GetValue(Of Decimal)(reader, "DeliveryRate")
                    s.CoverageRate = GetValue(Of Decimal)(reader, "CoverageRate")
                    s.AvgSendPerPhone = GetValue(Of Decimal)(reader, "AvgSendPerPhone")

                    s.TotalTemplateUsed = GetValue(Of Integer)(reader, "TotalTemplateUsed")
                    s.TotalEventCat = GetValue(Of Integer)(reader, "TotalEventCat")
                    s.TotalEvent = GetValue(Of Integer)(reader, "TotalEvent")

                    s.AvgWaitSeconds = GetValue(Of Decimal)(reader, "AvgWaitSeconds")
                    s.AvgProcessSeconds = GetValue(Of Decimal)(reader, "AvgProcessSeconds")
                    s.AvgTotalSeconds = GetValue(Of Decimal)(reader, "AvgTotalSeconds")
                    s.MaxProcessSeconds = GetValue(Of Integer)(reader, "MaxProcessSeconds")

                    s.FirstQueuedTime = GetNullable(Of DateTime)(reader, "FirstQueuedTime")
                    s.FirstSentTime = GetNullable(Of DateTime)(reader, "FirstSentTime")
                    s.LastCompletedTime = GetNullable(Of DateTime)(reader, "LastCompletedTime")

                    s.EstimatedCostSent = GetValue(Of Decimal)(reader, "EstimatedCostSent")
                    s.EstimatedCostAll = GetValue(Of Decimal)(reader, "EstimatedCostAll")
                    s.CostPerDelivered = GetValue(Of Decimal)(reader, "CostPerDelivered")

                    s.RemainingQuota = GetNullable(Of Integer)(reader, "RemainingQuota")
                    s.DailyQuota = GetNullable(Of Integer)(reader, "DailyQuota")

                    result.Summary = s
                End If

                ' ==========================================
                ' RESULT SET 2: STATUS DISTRIBUTION
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_StatusStat()
                        o.Status = GetValue(Of String)(reader, "Status")
                        o.StatusLabel = GetValue(Of String)(reader, "StatusLabel")
                        o.Quantity = GetValue(Of Integer)(reader, "Quantity")
                        o.Percentage = GetValue(Of Decimal)(reader, "Percentage")
                        result.StatusDistribution.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 3: BY TEMPLATE
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_TemplateStat()
                        o.TemplateId = GetValue(Of Long)(reader, "TemplateId")
                        o.TemplateName = GetValue(Of String)(reader, "TemplateName")
                        o.TemplateQuality = GetValue(Of String)(reader, "TemplateQuality")
                        o.TemplateStatus = GetValue(Of String)(reader, "TemplateStatus")
                        o.Price = GetValue(Of Decimal)(reader, "Price")
                        o.TotalSend = GetValue(Of Integer)(reader, "TotalSend")
                        o.DistinctPhone = GetValue(Of Integer)(reader, "DistinctPhone")
                        o.Sent = GetValue(Of Integer)(reader, "Sent")
                        o.Failed = GetValue(Of Integer)(reader, "Failed")
                        o.Retry = GetValue(Of Integer)(reader, "Retry")
                        o.Queued = GetValue(Of Integer)(reader, "Queued")
                        o.Processing = GetValue(Of Integer)(reader, "Processing")
                        o.Delivered = GetValue(Of Integer)(reader, "Delivered")
                        o.SuccessRate = GetValue(Of Decimal)(reader, "SuccessRate")
                        o.EstimatedCost = GetValue(Of Decimal)(reader, "EstimatedCost")
                        result.Templates.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 4: BY EVENT CATEGORY
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_EventCatStat()
                        o.EventCatId = GetValue(Of Integer)(reader, "EventCatId")
                        o.EventCatName = GetValue(Of String)(reader, "EventCatName")
                        o.DateShow = GetValue(Of String)(reader, "DateShow")
                        o.TotalSend = GetValue(Of Integer)(reader, "TotalSend")
                        o.DistinctPhone = GetValue(Of Integer)(reader, "DistinctPhone")
                        o.Sent = GetValue(Of Integer)(reader, "Sent")
                        o.Failed = GetValue(Of Integer)(reader, "Failed")
                        o.Pending = GetValue(Of Integer)(reader, "Pending")
                        o.Delivered = GetValue(Of Integer)(reader, "Delivered")
                        o.SuccessRate = GetValue(Of Decimal)(reader, "SuccessRate")
                        result.EventCats.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 5: BY EVENT
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_EventStat()
                        o.EventId = GetValue(Of Integer)(reader, "EventId")
                        o.EventName = GetValue(Of String)(reader, "EventName")
                        o.EventCatId = GetValue(Of Integer)(reader, "EventCatId")
                        o.EventCatName = GetValue(Of String)(reader, "EventCatName")
                        o.Location = GetValue(Of String)(reader, "Location")
                        o.FromDate = GetNullable(Of DateTime)(reader, "FromDate")
                        o.TotalSend = GetValue(Of Integer)(reader, "TotalSend")
                        o.DistinctPhone = GetValue(Of Integer)(reader, "DistinctPhone")
                        o.Sent = GetValue(Of Integer)(reader, "Sent")
                        o.Failed = GetValue(Of Integer)(reader, "Failed")
                        o.Pending = GetValue(Of Integer)(reader, "Pending")
                        o.Delivered = GetValue(Of Integer)(reader, "Delivered")
                        o.SuccessRate = GetValue(Of Decimal)(reader, "SuccessRate")
                        result.Events.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 6: BY PHONE
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_PhoneStat()
                        o.Phone = GetValue(Of String)(reader, "Phone")
                        o.FullName = GetValue(Of String)(reader, "FullName")
                        o.TotalSend = GetValue(Of Integer)(reader, "TotalSend")
                        o.TemplateCount = GetValue(Of Integer)(reader, "TemplateCount")
                        o.Sent = GetValue(Of Integer)(reader, "Sent")
                        o.Failed = GetValue(Of Integer)(reader, "Failed")
                        o.Retry = GetValue(Of Integer)(reader, "Retry")
                        o.Pending = GetValue(Of Integer)(reader, "Pending")
                        o.TotalRetryCount = GetValue(Of Integer)(reader, "TotalRetryCount")
                        o.Delivered = GetValue(Of Integer)(reader, "Delivered")
                        o.FirstSendTime = GetNullable(Of DateTime)(reader, "FirstSendTime")
                        o.LastSendTime = GetNullable(Of DateTime)(reader, "LastSendTime")
                        o.LastStatus = GetValue(Of String)(reader, "LastStatus")
                        o.LastErrorCode = GetNullable(Of Integer)(reader, "LastErrorCode")
                        o.LastErrorMessage = GetValue(Of String)(reader, "LastErrorMessage")
                        o.SuccessRate = GetValue(Of Decimal)(reader, "SuccessRate")
                        result.Phones.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 7: TIMELINE THEO NGAY
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_DailyStat()
                        o.SendDate = GetNullable(Of DateTime)(reader, "SendDate")
                        o.TotalSend = GetValue(Of Integer)(reader, "TotalSend")
                        o.Sent = GetValue(Of Integer)(reader, "Sent")
                        o.Failed = GetValue(Of Integer)(reader, "Failed")
                        o.Pending = GetValue(Of Integer)(reader, "Pending")
                        o.Delivered = GetValue(Of Integer)(reader, "Delivered")
                        o.SuccessRate = GetValue(Of Decimal)(reader, "SuccessRate")
                        result.Timeline.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 8: PHAN BO THEO GIO
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_HourStat()
                        o.SendHour = GetValue(Of Integer)(reader, "SendHour")
                        o.TotalSend = GetValue(Of Integer)(reader, "TotalSend")
                        o.Sent = GetValue(Of Integer)(reader, "Sent")
                        o.Failed = GetValue(Of Integer)(reader, "Failed")
                        o.SuccessRate = GetValue(Of Decimal)(reader, "SuccessRate")
                        result.Hours.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 9: PHAN TICH LOI
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_ErrorStat()
                        o.ErrorCode = GetValue(Of Integer)(reader, "ErrorCode")
                        o.ErrorMessage = GetValue(Of String)(reader, "ErrorMessage")
                        o.Quantity = GetValue(Of Integer)(reader, "Quantity")
                        o.DistinctPhone = GetValue(Of Integer)(reader, "DistinctPhone")
                        o.Percentage = GetValue(Of Decimal)(reader, "Percentage")
                        o.LastOccurredTime = GetNullable(Of DateTime)(reader, "LastOccurredTime")
                        result.Errors.Add(o)
                    End While
                End If

                ' ==========================================
                ' RESULT SET 10: CHI TIET TUNG LUOT GUI
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim o As New Marketing_Zalo_SendDetail()
                        o.QueueId = GetValue(Of Long)(reader, "QueueId")
                        o.LogId = GetNullable(Of Long)(reader, "LogId")
                        o.Phone = GetValue(Of String)(reader, "Phone")
                        o.FullName = GetValue(Of String)(reader, "FullName")
                        o.TemplateId = GetValue(Of Long)(reader, "TemplateId")
                        o.TemplateName = GetValue(Of String)(reader, "TemplateName")
                        o.QueueStatus = GetValue(Of String)(reader, "QueueStatus")
                        o.LogStatus = GetValue(Of String)(reader, "LogStatus")
                        o.DeliveryStatus = GetNullable(Of Integer)(reader, "DeliveryStatus")
                        o.DeliveryMessage = GetValue(Of String)(reader, "DeliveryMessage")
                        o.RetryCount = GetValue(Of Integer)(reader, "RetryCount")
                        o.ErrorCode = GetNullable(Of Integer)(reader, "ErrorCode")
                        o.ErrorMessage = GetValue(Of String)(reader, "ErrorMessage")
                        o.MsgId = GetValue(Of String)(reader, "MsgId")
                        o.TrackingId = GetValue(Of String)(reader, "TrackingId")
                        o.SendingMode = GetValue(Of String)(reader, "SendingMode")
                        o.EventCatId = GetValue(Of Integer)(reader, "EventCatId")
                        o.EventCatName = GetValue(Of String)(reader, "EventCatName")
                        o.EventId = GetValue(Of Integer)(reader, "EventId")
                        o.EventName = GetValue(Of String)(reader, "EventName")
                        o.ScheduledAt = GetNullable(Of DateTime)(reader, "ScheduledAt")
                        o.StartedAt = GetNullable(Of DateTime)(reader, "StartedAt")
                        o.CompletedAt = GetNullable(Of DateTime)(reader, "CompletedAt")
                        o.SentTime = GetNullable(Of DateTime)(reader, "SentTime")
                        o.WaitSeconds = GetNullable(Of Integer)(reader, "WaitSeconds")
                        o.ProcessSeconds = GetNullable(Of Integer)(reader, "ProcessSeconds")
                        o.TotalSeconds = GetNullable(Of Integer)(reader, "TotalSeconds")
                        o.Price = GetValue(Of Decimal)(reader, "Price")
                        result.Details.Add(o)
                    End While
                End If

            End Using

            Return result
        End Function

#Region "Helper Methods ep kieu chong DBNull"

        Private Function GetValue(Of T)(ByVal reader As IDataReader, ByVal name As String) As T
            Dim value As Object = reader(name)
            If value Is DBNull.Value OrElse value Is Nothing Then
                Return CType(Nothing, T)
            End If
            Return CType(Convert.ChangeType(value, GetType(T)), T)
        End Function

        Private Function GetNullable(Of T As Structure)(ByVal reader As IDataReader, ByVal name As String) As Nullable(Of T)
            Dim value As Object = reader(name)
            If value Is DBNull.Value OrElse value Is Nothing Then
                Return Nothing
            End If
            Return CType(Convert.ChangeType(value, GetType(T)), T)
        End Function

#End Region

    End Class

End Namespace
