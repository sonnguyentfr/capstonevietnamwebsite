'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Marketing
    Public Class Marketing_Mail_CampaingInfo
        Private _id As Integer
        Private _Title As String
        Private _Description As String
        Private _CreatedDate As DateTime
        Private _UserId As Integer
        Private _PortalId As Integer


        '------------------------------------------'
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal Value As Integer)
                _id = Value
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
        Public Property CreatedDate() As DateTime
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property UserId() As Integer
            Get
                Return _UserId
            End Get
            Set(ByVal Value As Integer)
                _UserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property PortalId() As Integer
            Get
                Return _PortalId
            End Get
            Set(ByVal Value As Integer)
                _PortalId = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
    Public Class Marketing_Mail_Campaing_ViewInfo
        Inherits Marketing_Mail_CampaingInfo
        Private _soluongemail As Integer
        Public Property soluongemail() As Integer
            Get
                Return _soluongemail
            End Get
            Set(ByVal Value As Integer)
                _soluongemail = Value
            End Set
        End Property
    End Class
    Public Class Marketing_Mail_Send_Log_GetUnopenedForResend_Result

        Public Property Id As Integer

        Public Property CampaignSendId As Integer

        Public Property ListMailId As Integer

        Public Property Email As String

        Public Property Status As String

        Public Property SentTime As Nullable(Of DateTime)

        Public Property OpenedTime As Nullable(Of DateTime)

        Public Property ResendCount As Integer

        Public Property SenderEmailId As Nullable(Of Integer)

        Public Property SenderEmail As String

        Public Property SenderName As String

    End Class
End Namespace