'******************************************
'Author         :DuongNQ
'Created Date   :3/25/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class News_UserWFInfo
        Private _ID As Integer
        Private _TenLuong As String
        Private _PhongBan As Integer
        Private _NguoiGui As Integer
        Private _NguoiNhan As String
        Private _TrangThaiDich As Integer
        Private _LoaiWF As Integer
        Private _IsDefault As Boolean
        Private _MoTa As String
        Private _NguoiTao As Integer
        Private _NgayTao As DateTime
        Private _IsActive As Boolean
        Private _OrderNumber As Integer
        Private _PortalId As Integer
        Private _ModuleId As Integer
        Private _LanguageId As String
        Private _IconSmall As String
        Private _IconLarge As String


        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal Value As Integer)
                _ID = Value
            End Set
        End Property

        Public Property TenLuong() As String
            Get
                Return _TenLuong
            End Get
            Set(ByVal Value As String)
                _TenLuong = Value
            End Set
        End Property

        Public Property PhongBan() As Integer
            Get
                Return _PhongBan
            End Get
            Set(ByVal Value As Integer)
                _PhongBan = Value
            End Set
        End Property

        Public Property NguoiGui() As Integer
            Get
                Return _NguoiGui
            End Get
            Set(ByVal Value As Integer)
                _NguoiGui = Value
            End Set
        End Property

        Public Property NguoiNhan() As String
            Get
                Return _NguoiNhan
            End Get
            Set(ByVal Value As String)
                _NguoiNhan = Value
            End Set
        End Property

        Public Property TrangThaiDich() As Integer
            Get
                Return _TrangThaiDich
            End Get
            Set(ByVal Value As Integer)
                _TrangThaiDich = Value
            End Set
        End Property

        Public Property LoaiWF() As Integer
            Get
                Return _LoaiWF
            End Get
            Set(ByVal Value As Integer)
                _LoaiWF = Value
            End Set
        End Property

        Public Property IsDefault() As Boolean
            Get
                Return _IsDefault
            End Get
            Set(ByVal Value As Boolean)
                _IsDefault = Value
            End Set
        End Property

        Public Property MoTa() As String
            Get
                Return _MoTa
            End Get
            Set(ByVal Value As String)
                _MoTa = Value
            End Set
        End Property

        Public Property NguoiTao() As Integer
            Get
                Return _NguoiTao
            End Get
            Set(ByVal Value As Integer)
                _NguoiTao = Value
            End Set
        End Property

        Public Property NgayTao() As DateTime
            Get
                Return _NgayTao
            End Get
            Set(ByVal Value As DateTime)
                _NgayTao = Value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return _IsActive
            End Get
            Set(ByVal Value As Boolean)
                _IsActive = Value
            End Set
        End Property

        Public Property OrderNumber() As Integer
            Get
                Return _OrderNumber
            End Get
            Set(ByVal Value As Integer)
                _OrderNumber = Value
            End Set
        End Property

        Public Property PortalId() As Integer
            Get
                Return _PortalId
            End Get
            Set(ByVal Value As Integer)
                _PortalId = Value
            End Set
        End Property

        Public Property ModuleId() As Integer
            Get
                Return _ModuleId
            End Get
            Set(ByVal Value As Integer)
                _ModuleId = Value
            End Set
        End Property

        Public Property LanguageId() As String
            Get
                Return _LanguageId
            End Get
            Set(ByVal Value As String)
                _LanguageId = Value
            End Set
        End Property

        Public Property IconSmall() As String
            Get
                Return _IconSmall
            End Get
            Set(ByVal Value As String)
                _IconSmall = Value
            End Set
        End Property

        Public Property IconLarge() As String
            Get
                Return _IconLarge
            End Get
            Set(ByVal Value As String)
                _IconLarge = Value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal ID As Integer, ByVal TenLuong As String, ByVal PhongBan As Integer, ByVal NguoiGui As Integer, ByVal NguoiNhan As String, ByVal TrangThaiDich As Integer, ByVal LoaiWF As Integer, ByVal IsDefault As Boolean, ByVal MoTa As String, ByVal NguoiTao As Integer, ByVal NgayTao As DateTime, ByVal IsActive As Boolean, ByVal OrderNumber As Integer, ByVal PortalId As Integer, ByVal ModuleId As Integer, ByVal LanguageId As String, ByVal IconSmall As String, ByVal IconLarge As String)
            Me.ID = ID
            Me.TenLuong = TenLuong
            Me.PhongBan = PhongBan
            Me.NguoiGui = NguoiGui
            Me.NguoiNhan = NguoiNhan
            Me.TrangThaiDich = TrangThaiDich
            Me.LoaiWF = LoaiWF
            Me.IsDefault = IsDefault
            Me.MoTa = MoTa
            Me.NguoiTao = NguoiTao
            Me.NgayTao = NgayTao
            Me.IsActive = IsActive
            Me.OrderNumber = OrderNumber
            Me.PortalId = PortalId
            Me.ModuleId = ModuleId
            Me.LanguageId = LanguageId
            Me.IconSmall = IconSmall
            Me.IconLarge = IconLarge
        End Sub
    End Class
End Namespace