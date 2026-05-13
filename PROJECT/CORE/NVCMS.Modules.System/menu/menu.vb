Namespace NVCMS.Modules.HeThong


    Public Class MenuInfo

#Region "Private Members"
        Private _id As Integer
        Private _parentId As Integer
        Private _name As String = String.Empty
        Private _isUsed As Integer
        Private _iconUrl As String = String.Empty
        Private _title As String = String.Empty
        Private _contents As String = String.Empty
        Private _tabId As Integer
        Private _url As String = String.Empty
        Private _levelNo As Integer
        Private _orderNo As Integer
        Private _portalId As Integer
        Private _languageId As String = String.Empty
        Private _params As String = String.Empty
        'Trường mới thêm vào mới mục đích Sort
        Private _sortColumn As String = String.Empty
#End Region

#Region "Constructors"
        ' initialization
        Public Sub New()
        End Sub

        Public Sub New(ByVal id As Integer, ByVal parentId As Integer, ByVal name As String, ByVal isUsed As Integer, ByVal iconUrl As String, ByVal title As String, ByVal contents As String, ByVal tabId As Integer, ByVal url As String, ByVal levelNo As Integer, ByVal orderNo As Integer, ByVal portalId As Integer, ByVal languageId As String, ByVal params As String)
            Me.Id = id
            Me.ParentId = parentId
            Me.Name = name
            Me.IsUsed = isUsed
            Me.IconUrl = iconUrl
            Me.Title = title
            Me.Contents = contents
            Me.TabId = tabId
            Me.Url = url
            Me.LevelNo = levelNo
            Me.OrderNo = orderNo
            Me.PortalId = portalId
            Me.LanguageId = languageId
            Me.Params = params
        End Sub
#End Region

#Region "Public Properties"
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal Value As Integer)
                _id = Value
            End Set
        End Property

        Public Property ParentId() As Integer
            Get
                Return _parentId
            End Get
            Set(ByVal Value As Integer)
                _parentId = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property

        Public Property IsUsed() As Integer
            Get
                Return _isUsed
            End Get
            Set(ByVal Value As Integer)
                _isUsed = Value
            End Set
        End Property

        Public Property IconUrl() As String
            Get
                Return _iconUrl
            End Get
            Set(ByVal Value As String)
                _iconUrl = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property

        Public Property Contents() As String
            Get
                Return _contents
            End Get
            Set(ByVal Value As String)
                _contents = Value
            End Set
        End Property

        Public Property TabId() As Integer
            Get
                Return _tabId
            End Get
            Set(ByVal Value As Integer)
                _tabId = Value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return _url
            End Get
            Set(ByVal Value As String)
                _url = Value
            End Set
        End Property

        Public Property LevelNo() As Integer
            Get
                Return _levelNo
            End Get
            Set(ByVal Value As Integer)
                _levelNo = Value
            End Set
        End Property

        Public Property OrderNo() As Integer
            Get
                Return _orderNo
            End Get
            Set(ByVal Value As Integer)
                _orderNo = Value
            End Set
        End Property

        Public Property PortalId() As Integer
            Get
                Return _portalId
            End Get
            Set(ByVal Value As Integer)
                _portalId = Value
            End Set
        End Property

        Public Property LanguageId() As String
            Get
                Return _languageId
            End Get
            Set(ByVal Value As String)
                _languageId = Value
            End Set
        End Property

        Public Property Params() As String
            Get
                Return _params
            End Get
            Set(ByVal Value As String)
                _params = Value
            End Set
        End Property

        Public Property SortColumn() As String
            Get
                Return _sortColumn
            End Get
            Set(ByVal Value As String)
                _sortColumn = Value
            End Set
        End Property

#End Region

    End Class
End Namespace