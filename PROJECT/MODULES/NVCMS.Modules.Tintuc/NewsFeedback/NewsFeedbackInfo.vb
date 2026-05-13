'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/27/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class NV_NewsFeedbackInfo
        Private _NewsFeedbackId As Integer
        Private _NewsId As Integer
        Private _FullName As String
        Private _Email As String
        Private _CreateDate As DateTime
        Private _Title As String
        Private _PhoneNumber As String
        Private _Content As String
        Private _Address As String
        Private _IPTrack As String
        Private _Status As Integer


        '------------------------------------------'
        Public Property NewsFeedbackId() As Integer
            Get
                Return _NewsFeedbackId
            End Get
            Set(ByVal Value As Integer)
                _NewsFeedbackId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NewsId() As Integer
            Get
                Return _NewsId
            End Get
            Set(ByVal Value As Integer)
                _NewsId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FullName() As String
            Get
                Return _FullName
            End Get
            Set(ByVal Value As String)
                _FullName = Value
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
        Public Property CreateDate() As DateTime
            Get
                Return _CreateDate
            End Get
            Set(ByVal Value As DateTime)
                _CreateDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property PhoneNumber() As String
            Get
                Return _PhoneNumber
            End Get
            Set(ByVal Value As String)
                _PhoneNumber = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Content() As String
            Get
                Return _Content
            End Get
            Set(ByVal Value As String)
                _Content = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Address() As String
            Get
                Return _Address
            End Get
            Set(ByVal Value As String)
                _Address = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IPTrack() As String
            Get
                Return _IPTrack
            End Get
            Set(ByVal Value As String)
                _IPTrack = Value
            End Set
        End Property

        Public Property Status() As Integer
            Get
                Return _Status
            End Get
            Set(ByVal Value As Integer)
                _Status = Value
            End Set
        End Property
    End Class
End Namespace