'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Banner
    Public Class BannerAdv_StaticInfo
        Private _id As Integer
        Private _BannerId As Integer
        Private _IP As String
        Private _Createdate As DateTime
        Private _isclick As Boolean
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
        Public Property BannerId() As Integer
            Get
                Return _BannerId
            End Get
            Set(ByVal Value As Integer)
                _BannerId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IP() As String
            Get
                Return _IP
            End Get
            Set(ByVal Value As String)
                _IP = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Createdate() As DateTime
            Get
                Return _Createdate
            End Get
            Set(ByVal Value As DateTime)
                _Createdate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property isclick() As Boolean
            Get
                Return _isclick
            End Get
            Set(ByVal Value As Boolean)
                _isclick = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace