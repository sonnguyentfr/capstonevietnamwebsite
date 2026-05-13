'******************************************
'Author         :DuongNQ
'Created Date   :5/11/2013
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace Vbuzz.Modules.TinTuc
    Public Class News_KeHoachInfo
        Private _Id As Integer
        Private _TieuDe As String
        Private _PhongBanID As Integer
        Private _NoiDung As String
        Private _NgayDuKien As DateTime
        Private _NguoiTao As Integer
        Private _NgayTao As DateTime
        Private _NguoiSua As String


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

        Public Property PhongBanID() As Integer
            Get
                Return _PhongBanID
            End Get
            Set(ByVal Value As Integer)
                _PhongBanID = Value
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

        Public Property NgayDuKien() As DateTime
            Get
                Return _NgayDuKien
            End Get
            Set(ByVal Value As DateTime)
                _NgayDuKien = Value
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

        Public Property NguoiSua() As String
            Get
                Return _NguoiSua
            End Get
            Set(ByVal Value As String)
                _NguoiSua = Value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(Id As Integer, TieuDe As String, PhongBanID As Integer, NoiDung As String, NgayDuKien As DateTime, NguoiTao As Integer, NgayTao As DateTime, NguoiSua As String)
            Me.Id = Id
            Me.TieuDe = TieuDe
            Me.PhongBanID = PhongBanID
            Me.NoiDung = NoiDung
            Me.NgayDuKien = NgayDuKien
            Me.NguoiTao = NguoiTao
            Me.NgayTao = NgayTao
            Me.NguoiSua = NguoiSua
        End Sub

    End Class
End Namespace