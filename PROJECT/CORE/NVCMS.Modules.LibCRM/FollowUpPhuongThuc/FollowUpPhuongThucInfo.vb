'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.LibCRM
    Public Class FollowUpPhuongThucInfo
        Private _id As Integer
        Private _PhuongThuc As String
        Private _ParentId As Integer
        Private _isShow As Boolean
        Private _isActive As Boolean
        Private _UserId As Integer
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
        Public Property PhuongThuc() As String
            Get
                Return _PhuongThuc
            End Get
            Set(ByVal Value As String)
                _PhuongThuc = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ParentId() As Integer
            Get
                Return _ParentId
            End Get
            Set(ByVal Value As Integer)
                _ParentId = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property isActive() As Boolean
            Get
                Return _isActive
            End Get
            Set(ByVal Value As Boolean)
                _isActive = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property isShow() As Boolean
            Get
                Return _isShow
            End Get
            Set(ByVal Value As Boolean)
                _isShow = Value
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
    End Class
End Namespace