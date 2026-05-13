Namespace NVCMS.Modules.LibCRM
    Public Class LibSchoolTypeInfo
        Private _id As Integer
        Private _Loaitruong As String
        Private _Descreption As String
        Private _IsActive As Boolean
        Private _Ordernumber As Integer
        Private _PortalId As Integer
        Private _CreatedDate As DateTime


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
        Public Property Loaitruong() As String
            Get
                Return _Loaitruong
            End Get
            Set(ByVal Value As String)
                _Loaitruong = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Descreption() As String
            Get
                Return _Descreption
            End Get
            Set(ByVal Value As String)
                _Descreption = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property IsActive() As Boolean
            Get
                Return _IsActive
            End Get
            Set(ByVal Value As Boolean)
                _IsActive = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Ordernumber() As Integer
            Get
                Return _Ordernumber
            End Get
            Set(ByVal Value As Integer)
                _Ordernumber = Value
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