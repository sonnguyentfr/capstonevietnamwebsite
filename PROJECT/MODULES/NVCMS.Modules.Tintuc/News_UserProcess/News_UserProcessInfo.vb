'******************************************
'Author         :DuongNQ
'Created Date   :3/25/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class News_UserProcessInfo
        Private _ID As Integer
        Private _UserID As Integer
        Private _NewsID As Integer
        Private _Status As Integer
        Private _CreatedDate As DateTime
        Private _CreatedUser As Integer

        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal Value As Integer)
                _ID = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal Value As Integer)
                _UserID = Value
            End Set
        End Property

        Public Property NewsID() As Integer
            Get
                Return _NewsID
            End Get
            Set(ByVal Value As Integer)
                _NewsID = Value
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

        Public Property CreatedDate() As DateTime
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedDate = Value
            End Set
        End Property

        Public Property CreatedUser() As Integer
            Get
                Return _CreatedUser
            End Get
            Set(ByVal Value As Integer)
                _CreatedUser = Value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal ID As Integer, ByVal UserID As Integer, ByVal NewsID As Integer, ByVal Status As Integer, ByVal CreatedDate As DateTime, ByVal CreatedUser As Integer)
            Me.ID = ID
            Me.UserID = UserID
            Me.NewsID = NewsID
            Me.Status = Status
            Me.CreatedDate = CreatedDate
            Me.CreatedUser = CreatedUser
        End Sub
    End Class
End Namespace