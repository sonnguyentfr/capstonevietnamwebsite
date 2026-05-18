'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Banner
    Public Class BannerAdv_VitriInfo
        Private _id As Integer
        Private _Title As String
        Private _width As Integer
        Private _height As Integer
        Private _Images As String
        Private _CreatedByUserId As Integer
        Private _CreatedOnDate As DateTime
        Private _LastModifiedByUserId As Integer
        Private _LastModifiedOnDate As DateTime
        Private _ModuleId As Integer
        Private _portalid As Integer


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
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property width() As Integer
            Get
                Return _width
            End Get
            Set(ByVal Value As Integer)
                _width = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property height() As Integer
            Get
                Return _height
            End Get
            Set(ByVal Value As Integer)
                _height = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Images() As String
            Get
                Return _Images
            End Get
            Set(ByVal Value As String)
                _Images = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CreatedByUserId() As Integer
            Get
                Return _CreatedByUserId
            End Get
            Set(ByVal Value As Integer)
                _CreatedByUserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CreatedOnDate() As DateTime
            Get
                Return _CreatedOnDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedOnDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LastModifiedByUserId() As Integer
            Get
                Return _LastModifiedByUserId
            End Get
            Set(ByVal Value As Integer)
                _LastModifiedByUserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LastModifiedOnDate() As DateTime
            Get
                Return _LastModifiedOnDate
            End Get
            Set(ByVal Value As DateTime)
                _LastModifiedOnDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ModuleId() As Integer
            Get
                Return _ModuleId
            End Get
            Set(ByVal Value As Integer)
                _ModuleId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property portalid() As Integer
            Get
                Return _portalid
            End Get
            Set(ByVal Value As Integer)
                _portalid = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace