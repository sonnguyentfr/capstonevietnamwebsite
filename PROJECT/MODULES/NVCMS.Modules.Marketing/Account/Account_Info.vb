'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Marketing
    Public Class Marketing_Mail_AccountInfo
        Private _id As Integer
        Private _Name As String
        Private _Mail As String
        Private _Password As String
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
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Mail() As String
            Get
                Return _Mail
            End Get
            Set(ByVal Value As String)
                _Mail = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Password() As String
            Get
                Return _Password
            End Get
            Set(ByVal Value As String)
                _Password = Value
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