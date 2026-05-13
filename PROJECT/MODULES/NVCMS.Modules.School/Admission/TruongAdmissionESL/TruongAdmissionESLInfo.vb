Namespace NVCMS.Modules.School
    Public Class TruongAdmissionESLInfo
        Private _id As Integer
        Private _TruongId As Integer
        Private _AdmissionID As Integer
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
        Public Property AdmissionID() As Integer
            Get
                Return _AdmissionID
            End Get
            Set(ByVal Value As Integer)
                _AdmissionID = Value
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