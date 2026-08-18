'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Marketing
    Public Class Marketing_Mail_ListMailInfo
        Private _id As Integer
        Private _Campaingid As Integer
        Private _Email As String
        Private _Status As Boolean
        Private _sendcount As Integer
        Private _isUnsub As Boolean
        Private _Datetime As DateTime
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
        Public Property Campaingid() As Integer
            Get
                Return _Campaingid
            End Get
            Set(ByVal Value As Integer)
                _Campaingid = Value
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
        Public Property sendcount() As Integer
            Get
                Return _sendcount
            End Get
            Set(ByVal Value As Integer)
                _sendcount = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property isUnsub() As Boolean
            Get
                Return _isUnsub
            End Get
            Set(ByVal Value As Boolean)
                _isUnsub = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Status() As Boolean
            Get
                Return _Status
            End Get
            Set(ByVal Value As Boolean)
                _Status = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Datetime() As DateTime
            Get
                Return _Datetime
            End Get
            Set(ByVal Value As DateTime)
                _Datetime = Value
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
End Namespace