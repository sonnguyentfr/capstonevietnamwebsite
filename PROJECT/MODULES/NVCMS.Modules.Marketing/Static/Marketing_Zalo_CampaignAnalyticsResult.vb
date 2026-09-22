Namespace NVCMS.Modules.Marketing

    ''' <summary>
    ''' Ket qua phan tich chien dich Zalo ZNS - anh xa 10 result set cua
    ''' sp_Marketing_Zalo_Campaign_Analytics.
    ''' </summary>
    Public Class Marketing_Zalo_CampaignAnalyticsResult

        Public Sub New()
            Summary = New Marketing_Zalo_CampaignSummary()
            StatusDistribution = New List(Of Marketing_Zalo_StatusStat)()
            Templates = New List(Of Marketing_Zalo_TemplateStat)()
            EventCats = New List(Of Marketing_Zalo_EventCatStat)()
            Events = New List(Of Marketing_Zalo_EventStat)()
            Phones = New List(Of Marketing_Zalo_PhoneStat)()
            Timeline = New List(Of Marketing_Zalo_DailyStat)()
            Hours = New List(Of Marketing_Zalo_HourStat)()
            Errors = New List(Of Marketing_Zalo_ErrorStat)()
            Details = New List(Of Marketing_Zalo_SendDetail)()
        End Sub

        ''' <summary>RS1 - KPI tong quan</summary>
        Public Property Summary() As Marketing_Zalo_CampaignSummary

        ''' <summary>RS2 - Phan bo trang thai hang doi</summary>
        Public Property StatusDistribution() As List(Of Marketing_Zalo_StatusStat)

        ''' <summary>RS3 - Thong ke theo template ZNS</summary>
        Public Property Templates() As List(Of Marketing_Zalo_TemplateStat)

        ''' <summary>RS4 - Thong ke theo nhom su kien</summary>
        Public Property EventCats() As List(Of Marketing_Zalo_EventCatStat)

        ''' <summary>RS5 - Thong ke theo su kien / dia diem</summary>
        Public Property Events() As List(Of Marketing_Zalo_EventStat)

        ''' <summary>RS6 - So luot gui theo so dien thoai</summary>
        Public Property Phones() As List(Of Marketing_Zalo_PhoneStat)

        ''' <summary>RS7 - Dien bien theo ngay</summary>
        Public Property Timeline() As List(Of Marketing_Zalo_DailyStat)

        ''' <summary>RS8 - Phan bo theo khung gio</summary>
        Public Property Hours() As List(Of Marketing_Zalo_HourStat)

        ''' <summary>RS9 - Phan tich nguyen nhan loi</summary>
        Public Property Errors() As List(Of Marketing_Zalo_ErrorStat)

        ''' <summary>RS10 - Chi tiet tung luot gui</summary>
        Public Property Details() As List(Of Marketing_Zalo_SendDetail)

    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_CampaignSummary
        ' Thong tin chien dich
        Public Property CampaignId() As Integer
        Public Property Title() As String
        Public Property Description() As String
        Public Property CampaignStatus() As Integer
        Public Property CampaignCreatedDate() As Nullable(Of DateTime)

        ' Quy mo
        Public Property TotalPhoneInList() As Integer
        Public Property TotalSend() As Integer
        Public Property TotalPhoneTargeted() As Integer

        ' Trang thai hang doi
        Public Property TotalQueued() As Integer
        Public Property TotalProcessing() As Integer
        Public Property TotalSent() As Integer
        Public Property TotalFailed() As Integer
        Public Property TotalRetry() As Integer
        Public Property TotalRetryCount() As Integer

        ' Ket qua giao ban tin phia Zalo
        Public Property TotalDelivered() As Integer
        Public Property TotalRejected() As Integer
        Public Property TotalPendingDelivery() As Integer

        ' Ty le (%)
        Public Property SuccessRate() As Decimal
        Public Property FailRate() As Decimal
        Public Property PendingRate() As Decimal
        Public Property DeliveryRate() As Decimal
        Public Property CoverageRate() As Decimal
        Public Property AvgSendPerPhone() As Decimal

        ' Do phu
        Public Property TotalTemplateUsed() As Integer
        Public Property TotalEventCat() As Integer
        Public Property TotalEvent() As Integer

        ' Hieu nang duong ong gui (giay)
        Public Property AvgWaitSeconds() As Decimal
        Public Property AvgProcessSeconds() As Decimal
        Public Property AvgTotalSeconds() As Decimal
        Public Property MaxProcessSeconds() As Integer

        ' Moc thoi gian
        Public Property FirstQueuedTime() As Nullable(Of DateTime)
        Public Property FirstSentTime() As Nullable(Of DateTime)
        Public Property LastCompletedTime() As Nullable(Of DateTime)

        ' Chi phi uoc tinh (VND)
        Public Property EstimatedCostSent() As Decimal
        Public Property EstimatedCostAll() As Decimal
        Public Property CostPerDelivered() As Decimal

        ' Quota Zalo OA ghi nhan gan nhat
        Public Property RemainingQuota() As Nullable(Of Integer)
        Public Property DailyQuota() As Nullable(Of Integer)
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_StatusStat
        Public Property Status() As String
        Public Property StatusLabel() As String
        Public Property Quantity() As Integer
        Public Property Percentage() As Decimal
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_TemplateStat
        Public Property TemplateId() As Long
        Public Property TemplateName() As String
        Public Property TemplateQuality() As String
        Public Property TemplateStatus() As String
        Public Property Price() As Decimal
        Public Property TotalSend() As Integer
        Public Property DistinctPhone() As Integer
        Public Property Sent() As Integer
        Public Property Failed() As Integer
        Public Property Retry() As Integer
        Public Property Queued() As Integer
        Public Property Processing() As Integer
        Public Property Delivered() As Integer
        Public Property SuccessRate() As Decimal
        Public Property EstimatedCost() As Decimal
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_EventCatStat
        Public Property EventCatId() As Integer
        Public Property EventCatName() As String
        Public Property DateShow() As String
        Public Property TotalSend() As Integer
        Public Property DistinctPhone() As Integer
        Public Property Sent() As Integer
        Public Property Failed() As Integer
        Public Property Pending() As Integer
        Public Property Delivered() As Integer
        Public Property SuccessRate() As Decimal
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_EventStat
        Public Property EventId() As Integer
        Public Property EventName() As String
        Public Property EventCatId() As Integer
        Public Property EventCatName() As String
        Public Property Location() As String
        Public Property FromDate() As Nullable(Of DateTime)
        Public Property TotalSend() As Integer
        Public Property DistinctPhone() As Integer
        Public Property Sent() As Integer
        Public Property Failed() As Integer
        Public Property Pending() As Integer
        Public Property Delivered() As Integer
        Public Property SuccessRate() As Decimal
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_PhoneStat
        Public Property Phone() As String
        Public Property FullName() As String
        Public Property TotalSend() As Integer
        Public Property TemplateCount() As Integer
        Public Property Sent() As Integer
        Public Property Failed() As Integer
        Public Property Retry() As Integer
        Public Property Pending() As Integer
        Public Property TotalRetryCount() As Integer
        Public Property Delivered() As Integer
        Public Property FirstSendTime() As Nullable(Of DateTime)
        Public Property LastSendTime() As Nullable(Of DateTime)
        Public Property LastStatus() As String
        Public Property LastErrorCode() As Nullable(Of Integer)
        Public Property LastErrorMessage() As String
        Public Property SuccessRate() As Decimal
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_DailyStat
        Public Property SendDate() As Nullable(Of DateTime)
        Public Property TotalSend() As Integer
        Public Property Sent() As Integer
        Public Property Failed() As Integer
        Public Property Pending() As Integer
        Public Property Delivered() As Integer
        Public Property SuccessRate() As Decimal
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_HourStat
        Public Property SendHour() As Integer
        Public Property TotalSend() As Integer
        Public Property Sent() As Integer
        Public Property Failed() As Integer
        Public Property SuccessRate() As Decimal
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_ErrorStat
        Public Property ErrorCode() As Integer
        Public Property ErrorMessage() As String
        Public Property Quantity() As Integer
        Public Property DistinctPhone() As Integer
        Public Property Percentage() As Decimal
        Public Property LastOccurredTime() As Nullable(Of DateTime)
    End Class

    '------------------------------------------'

    Public Class Marketing_Zalo_SendDetail
        Public Property QueueId() As Long
        Public Property LogId() As Nullable(Of Long)
        Public Property Phone() As String
        Public Property FullName() As String
        Public Property TemplateId() As Long
        Public Property TemplateName() As String
        Public Property QueueStatus() As String
        Public Property LogStatus() As String
        Public Property DeliveryStatus() As Nullable(Of Integer)
        Public Property DeliveryMessage() As String
        Public Property RetryCount() As Integer
        Public Property ErrorCode() As Nullable(Of Integer)
        Public Property ErrorMessage() As String
        Public Property MsgId() As String
        Public Property TrackingId() As String
        Public Property SendingMode() As String
        Public Property EventCatId() As Integer
        Public Property EventCatName() As String
        Public Property EventId() As Integer
        Public Property EventName() As String
        Public Property ScheduledAt() As Nullable(Of DateTime)
        Public Property StartedAt() As Nullable(Of DateTime)
        Public Property CompletedAt() As Nullable(Of DateTime)
        Public Property SentTime() As Nullable(Of DateTime)
        Public Property WaitSeconds() As Nullable(Of Integer)
        Public Property ProcessSeconds() As Nullable(Of Integer)
        Public Property TotalSeconds() As Nullable(Of Integer)
        Public Property Price() As Decimal
    End Class

End Namespace
