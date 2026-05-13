'******************************************
'Author         :SonNguyen
'Created Date   :3/21/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Student
    Public Class StudentFollow_LogInfo
        Private _id As Integer
        Private _StudentId As Integer
        Private _Noidung As String
        Private _CreatedDate As DateTime
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
        Public Property StudentId() As Integer
            Get
                Return _StudentId
            End Get
            Set(ByVal Value As Integer)
                _StudentId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Noidung() As String
            Get
                Return _Noidung
            End Get
            Set(ByVal Value As String)
                _Noidung = Value
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