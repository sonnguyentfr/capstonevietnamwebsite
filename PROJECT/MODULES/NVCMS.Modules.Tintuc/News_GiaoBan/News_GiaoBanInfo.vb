'******************************************
'Author         :DuongNQ
'Created Date   :5/11/2013
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace Vbuzz.Modules.TinTuc
    Public Class News_GiaoBanInfo
        Private _Id As Integer
        Private _TieuDe As String
        Private _NoiDung As String
        Private _NgayGiaoBan As DateTime
        Private _NguoiTao As Integer
        Private _NgayTao As DateTime
        Private _ModuleId As Integer


        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal Value As Integer)
                _Id = Value
            End Set
        End Property

        Public Property TieuDe() As String
            Get
                Return _TieuDe
            End Get
            Set(ByVal Value As String)
                _TieuDe = Value
            End Set
        End Property

        Public Property NoiDung() As String
            Get
                Return _NoiDung
            End Get
            Set(ByVal Value As String)
                _NoiDung = Value
            End Set
        End Property

        Public Property NgayGiaoBan() As DateTime
            Get
                Return _NgayGiaoBan
            End Get
            Set(ByVal Value As DateTime)
                _NgayGiaoBan = Value
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

        Public Property ModuleId() As Integer
            Get
                Return _ModuleId
            End Get
            Set(ByVal Value As Integer)
                _ModuleId = Value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(Id As Integer, TieuDe As String, NoiDung As String, NgayGiaoBan As DateTime, NguoiTao As Integer, NgayTao As DateTime, ModuleId As Integer)
            Me.Id = Id
            Me.TieuDe = TieuDe
            Me.NoiDung = NoiDung
            Me.NgayGiaoBan = NgayGiaoBan
            Me.NguoiTao = NguoiTao
            Me.NgayTao = NgayTao
            Me.ModuleId = ModuleId
        End Sub
    End Class
End Namespace