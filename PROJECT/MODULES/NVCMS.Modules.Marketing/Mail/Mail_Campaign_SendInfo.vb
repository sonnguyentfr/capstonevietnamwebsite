Namespace NVCMS.Modules.Marketing
    Public Class Campaign_SendInfo
        Private _Id As Integer
        Private _CampaignId As Integer
        Private _TemplateId As Integer
        Private _Subject As String
        Private _Body As String
        Private _Status As Integer
        Private _TotalRecipient As Integer
        Private _TotalSent As Integer
        Private _TotalDelivered As Integer
        Private _TotalOpened As Integer
        Private _TotalClicked As Integer
        Private _TotalBounced As Integer
        Private _TotalComplaint As Integer
        Private _TotalUnsubscribed As Integer
        Private _ScheduleTime As DateTime
        Private _StartedTime As DateTime
        Private _CompletedTime As DateTime
        Private _CreatedDate As DateTime


        '------------------------------------------'
        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal Value As Integer)
                _Id = Value
            End Set
        End Property

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
        Public Property TemplateId() As Integer
            Get
                Return _TemplateId
            End Get
            Set(ByVal Value As Integer)
                _TemplateId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Subject() As String
            Get
                Return _Subject
            End Get
            Set(ByVal Value As String)
                _Subject = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Body() As String
            Get
                Return _Body
            End Get
            Set(ByVal Value As String)
                _Body = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Status() As Integer
            Get
                Return _Status
            End Get
            Set(ByVal Value As Integer)
                _Status = Value
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
        Public Property ScheduleTime() As DateTime
            Get
                Return _ScheduleTime
            End Get
            Set(ByVal Value As DateTime)
                _ScheduleTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property StartedTime() As DateTime
            Get
                Return _StartedTime
            End Get
            Set(ByVal Value As DateTime)
                _StartedTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CompletedTime() As DateTime
            Get
                Return _CompletedTime
            End Get
            Set(ByVal Value As DateTime)
                _CompletedTime = Value
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
    End Class
End Namespace
