Namespace NVCMS.Modules.Marketing
    Public Class Mail_Send_LogInfo
        Private _Id As Int16
        Private _CampaignSendId As Integer
        Private _ListMailId As Integer
        Private _Email As String
        Private _SesMessageId As String
        Private _Status As String
        Private _ErrorMessage As String
        Private _SentTime As DateTime
        Private _DeliveredTime As DateTime
        Private _OpenedTime As DateTime
        Private _ClickedTime As DateTime
        Private _CreatedDate As DateTime
        Private _ResendCount As Integer
        Private _SenderEmailId As Integer


        '------------------------------------------'
        Public Property Id() As Int16
            Get
                Return _Id
            End Get
            Set(ByVal Value As Int16)
                _Id = Value
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
        Public Property ListMailId() As Integer
            Get
                Return _ListMailId
            End Get
            Set(ByVal Value As Integer)
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
        Public Property SesMessageId() As String
            Get
                Return _SesMessageId
            End Get
            Set(ByVal Value As String)
                _SesMessageId = Value
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
        Public Property ErrorMessage() As String
            Get
                Return _ErrorMessage
            End Get
            Set(ByVal Value As String)
                _ErrorMessage = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SentTime() As DateTime
            Get
                Return _SentTime
            End Get
            Set(ByVal Value As DateTime)
                _SentTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property DeliveredTime() As DateTime
            Get
                Return _DeliveredTime
            End Get
            Set(ByVal Value As DateTime)
                _DeliveredTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OpenedTime() As DateTime
            Get
                Return _OpenedTime
            End Get
            Set(ByVal Value As DateTime)
                _OpenedTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ClickedTime() As DateTime
            Get
                Return _ClickedTime
            End Get
            Set(ByVal Value As DateTime)
                _ClickedTime = Value
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
        Public Property ResendCount() As Integer
            Get
                Return _ResendCount
            End Get
            Set(ByVal Value As Integer)
                _ResendCount = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SenderEmailId() As Integer
            Get
                Return _SenderEmailId
            End Get
            Set(ByVal Value As Integer)
                _SenderEmailId = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace