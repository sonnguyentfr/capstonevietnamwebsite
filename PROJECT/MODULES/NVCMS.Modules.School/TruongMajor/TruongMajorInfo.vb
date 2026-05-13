Namespace NVCMS.Modules.School
    Public Class TruongMajorInfo
        Private _id As Integer
        Private _TruongId As Integer
        Private _Major As Integer
        Private _Associate As Boolean
        Private _Bachelor As Boolean
        Private _Master As Boolean
        Private _Doctor As Boolean
        Private _ProfessionalCertificate As Boolean
        Private _Other As String
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
        Public Property TruongId() As Integer
            Get
                Return _TruongId
            End Get
            Set(ByVal Value As Integer)
                _TruongId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Major() As Integer
            Get
                Return _Major
            End Get
            Set(ByVal Value As Integer)
                _Major = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Associate() As Boolean
            Get
                Return _Associate
            End Get
            Set(ByVal Value As Boolean)
                _Associate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Bachelor() As Boolean
            Get
                Return _Bachelor
            End Get
            Set(ByVal Value As Boolean)
                _Bachelor = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Master() As Boolean
            Get
                Return _Master
            End Get
            Set(ByVal Value As Boolean)
                _Master = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Doctor() As Boolean
            Get
                Return _Doctor
            End Get
            Set(ByVal Value As Boolean)
                _Doctor = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ProfessionalCertificate() As Boolean
            Get
                Return _ProfessionalCertificate
            End Get
            Set(ByVal Value As Boolean)
                _ProfessionalCertificate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Other() As String
            Get
                Return _Other
            End Get
            Set(ByVal Value As String)
                _Other = Value
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
End Namespace