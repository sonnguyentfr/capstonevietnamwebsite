Namespace NVCMS.Modules.Marketing
    Public Class Marketing_Mail_CampaignAnalyticsResult
        Private _Summary As Marketing_Mail_CampaignSummary
        Private _Details As List(Of Marketing_Mail_SendDetailLog)

        Public Sub New()
            _Summary = New Marketing_Mail_CampaignSummary()
            _Details = New List(Of Marketing_Mail_SendDetailLog)()
        End Sub

        Public Property Summary() As Marketing_Mail_CampaignSummary
            Get
                Return _Summary
            End Get
            Set(ByVal Value As Marketing_Mail_CampaignSummary)
                _Summary = Value
            End Set
        End Property

        Public Property Details() As List(Of Marketing_Mail_SendDetailLog)
            Get
                Return _Details
            End Get
            Set(ByVal Value As List(Of Marketing_Mail_SendDetailLog))
                _Details = Value
            End Set
        End Property
    End Class
    Public Class Marketing_Mail_SendDetailLog
        Private _SendLogId As Long
        Private _CampaignSendId As Integer
        Private _ListMailId As Nullable(Of Integer)
        Private _Email As String
        Private _Status As String
        Private _SesMessageId As String
        Private _SentTime As Nullable(Of DateTime)
        Private _DeliveredTime As Nullable(Of DateTime)
        Private _OpenedTime As Nullable(Of DateTime)
        Private _ClickedTime As Nullable(Of DateTime)
        Private _ErrorMessage As String
        Private _CreatedDate As DateTime
        Private _SentToDeliveredSeconds As Nullable(Of Integer)
        Private _DeliveredToOpenSeconds As Nullable(Of Integer)
        Private _SentToOpenSeconds As Nullable(Of Integer)
        Private _DeliveredToOpenMinutes As Nullable(Of Decimal)
        Private _SentToOpenMinutes As Nullable(Of Decimal)

        '------------------------------------------'
        Public Property SendLogId() As Long
            Get
                Return _SendLogId
            End Get
            Set(ByVal Value As Long)
                _SendLogId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CampaignSendId() As Integer
            Get
                Return _CampaignSendId
            End Get
            Set(ByVal Value As Integer)
                _CampaignSendId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ListMailId() As Nullable(Of Integer)
            Get
                Return _ListMailId
            End Get
            Set(ByVal Value As Nullable(Of Integer))
                _ListMailId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal Value As String)
                _Email = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Status() As String
            Get
                Return _Status
            End Get
            Set(ByVal Value As String)
                _Status = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SesMessageId() As String
            Get
                Return _SesMessageId
            End Get
            Set(ByVal Value As String)
                _SesMessageId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SentTime() As Nullable(Of DateTime)
            Get
                Return _SentTime
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _SentTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property DeliveredTime() As Nullable(Of DateTime)
            Get
                Return _DeliveredTime
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _DeliveredTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OpenedTime() As Nullable(Of DateTime)
            Get
                Return _OpenedTime
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _OpenedTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ClickedTime() As Nullable(Of DateTime)
            Get
                Return _ClickedTime
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _ClickedTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ErrorMessage() As String
            Get
                Return _ErrorMessage
            End Get
            Set(ByVal Value As String)
                _ErrorMessage = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CreatedDate() As DateTime
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SentToDeliveredSeconds() As Nullable(Of Integer)
            Get
                Return _SentToDeliveredSeconds
            End Get
            Set(ByVal Value As Nullable(Of Integer))
                _SentToDeliveredSeconds = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property DeliveredToOpenSeconds() As Nullable(Of Integer)
            Get
                Return _DeliveredToOpenSeconds
            End Get
            Set(ByVal Value As Nullable(Of Integer))
                _DeliveredToOpenSeconds = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SentToOpenSeconds() As Nullable(Of Integer)
            Get
                Return _SentToOpenSeconds
            End Get
            Set(ByVal Value As Nullable(Of Integer))
                _SentToOpenSeconds = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property DeliveredToOpenMinutes() As Nullable(Of Decimal)
            Get
                Return _DeliveredToOpenMinutes
            End Get
            Set(ByVal Value As Nullable(Of Decimal))
                _DeliveredToOpenMinutes = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SentToOpenMinutes() As Nullable(Of Decimal)
            Get
                Return _SentToOpenMinutes
            End Get
            Set(ByVal Value As Nullable(Of Decimal))
                _SentToOpenMinutes = Value
            End Set
        End Property
    End Class
    Public Class Marketing_Mail_CampaignSummary
        Private _CampaignId As Integer
        Private _Title As String
        Private _Description As String
        Private _CreatedDate As Nullable(Of DateTime)
        Private _TotalCampaignSend As Integer
        Private _TotalRecipient As Integer
        Private _TotalSent As Integer
        Private _TotalDelivered As Integer
        Private _TotalOpened As Integer
        Private _TotalClicked As Integer
        Private _TotalBounced As Integer
        Private _TotalComplaint As Integer
        Private _TotalUnsubscribed As Integer
        Private _OpenRate As Decimal
        Private _ClickRate As Decimal
        Private _DeliveryRate As Decimal
        Private _BounceRate As Decimal
        Private _FirstStartedTime As Nullable(Of DateTime)
        Private _LastCompletedTime As Nullable(Of DateTime)

        '------------------------------------------'
        Public Property CampaignId() As Integer
            Get
                Return _CampaignId
            End Get
            Set(ByVal Value As Integer)
                _CampaignId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CreatedDate() As Nullable(Of DateTime)
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _CreatedDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalCampaignSend() As Integer
            Get
                Return _TotalCampaignSend
            End Get
            Set(ByVal Value As Integer)
                _TotalCampaignSend = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalRecipient() As Integer
            Get
                Return _TotalRecipient
            End Get
            Set(ByVal Value As Integer)
                _TotalRecipient = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalSent() As Integer
            Get
                Return _TotalSent
            End Get
            Set(ByVal Value As Integer)
                _TotalSent = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalDelivered() As Integer
            Get
                Return _TotalDelivered
            End Get
            Set(ByVal Value As Integer)
                _TotalDelivered = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalOpened() As Integer
            Get
                Return _TotalOpened
            End Get
            Set(ByVal Value As Integer)
                _TotalOpened = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalClicked() As Integer
            Get
                Return _TotalClicked
            End Get
            Set(ByVal Value As Integer)
                _TotalClicked = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalBounced() As Integer
            Get
                Return _TotalBounced
            End Get
            Set(ByVal Value As Integer)
                _TotalBounced = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalComplaint() As Integer
            Get
                Return _TotalComplaint
            End Get
            Set(ByVal Value As Integer)
                _TotalComplaint = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TotalUnsubscribed() As Integer
            Get
                Return _TotalUnsubscribed
            End Get
            Set(ByVal Value As Integer)
                _TotalUnsubscribed = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OpenRate() As Decimal
            Get
                Return _OpenRate
            End Get
            Set(ByVal Value As Decimal)
                _OpenRate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ClickRate() As Decimal
            Get
                Return _ClickRate
            End Get
            Set(ByVal Value As Decimal)
                _ClickRate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property DeliveryRate() As Decimal
            Get
                Return _DeliveryRate
            End Get
            Set(ByVal Value As Decimal)
                _DeliveryRate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property BounceRate() As Decimal
            Get
                Return _BounceRate
            End Get
            Set(ByVal Value As Decimal)
                _BounceRate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FirstStartedTime() As Nullable(Of DateTime)
            Get
                Return _FirstStartedTime
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _FirstStartedTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LastCompletedTime() As Nullable(Of DateTime)
            Get
                Return _LastCompletedTime
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _LastCompletedTime = Value
            End Set
        End Property
    End Class
End Namespace