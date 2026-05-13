'******************************************
'Author         :SonNguyen
'Created Date   :3/21/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.Lib.FollowUp

Namespace NVCMS.Modules.Student
    Public Class StudentFromLadipageInfo
        Private _id As Integer
        Private _hotendem As String
        Private _ten As String
        Private _gioi_tinh As Nullable(Of Boolean)
        Private _ngay_sinh As Nullable(Of Date)
        Private _so_dien_thoai As String
        Private _email As String
        Private _truong_dang_hoc As String
        Private _event_dia_diem As String
        Private _event_id As Nullable(Of Integer)
        Private _event_dia_diem_id As Nullable(Of Integer)
        Private _source As String
        Private _medium As String
        Private _link As String
        Private _ladi_page_id As String
        Private _client_ip As String
        Private _thong_tin_khac As String
        Private _is_update_crm As Nullable(Of Boolean)
        Private _created_date As Nullable(Of DateTime)

        '------------------------------------------'
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal Value As Integer)
                _id = Value
            End Set
        End Property

        Public Property hotendem() As String
            Get
                Return _hotendem
            End Get
            Set(ByVal Value As String)
                _hotendem = Value
            End Set
        End Property

        Public Property ten() As String
            Get
                Return _ten
            End Get
            Set(ByVal Value As String)
                _ten = Value
            End Set
        End Property

        Public Property gioi_tinh() As Nullable(Of Boolean)
            Get
                Return _gioi_tinh
            End Get
            Set(ByVal Value As Nullable(Of Boolean))
                _gioi_tinh = Value
            End Set
        End Property

        Public Property ngay_sinh() As Nullable(Of Date)
            Get
                Return _ngay_sinh
            End Get
            Set(ByVal Value As Nullable(Of Date))
                _ngay_sinh = Value
            End Set
        End Property

        Public Property so_dien_thoai() As String
            Get
                Return _so_dien_thoai
            End Get
            Set(ByVal Value As String)
                _so_dien_thoai = Value
            End Set
        End Property

        Public Property email() As String
            Get
                Return _email
            End Get
            Set(ByVal Value As String)
                _email = Value
            End Set
        End Property

        Public Property truong_dang_hoc() As String
            Get
                Return _truong_dang_hoc
            End Get
            Set(ByVal Value As String)
                _truong_dang_hoc = Value
            End Set
        End Property

        Public Property event_dia_diem() As String
            Get
                Return _event_dia_diem
            End Get
            Set(ByVal Value As String)
                _event_dia_diem = Value
            End Set
        End Property

        Public Property event_id() As Nullable(Of Integer)
            Get
                Return _event_id
            End Get
            Set(ByVal Value As Nullable(Of Integer))
                _event_id = Value
            End Set
        End Property

        Public Property event_dia_diem_id() As Nullable(Of Integer)
            Get
                Return _event_dia_diem_id
            End Get
            Set(ByVal Value As Nullable(Of Integer))
                _event_dia_diem_id = Value
            End Set
        End Property

        Public Property source() As String
            Get
                Return _source
            End Get
            Set(ByVal Value As String)
                _source = Value
            End Set
        End Property

        Public Property medium() As String
            Get
                Return _medium
            End Get
            Set(ByVal Value As String)
                _medium = Value
            End Set
        End Property

        Public Property link() As String
            Get
                Return _link
            End Get
            Set(ByVal Value As String)
                _link = Value
            End Set
        End Property

        Public Property ladi_page_id() As String
            Get
                Return _ladi_page_id
            End Get
            Set(ByVal Value As String)
                _ladi_page_id = Value
            End Set
        End Property

        Public Property client_ip() As String
            Get
                Return _client_ip
            End Get
            Set(ByVal Value As String)
                _client_ip = Value
            End Set
        End Property

        Public Property thong_tin_khac() As String
            Get
                Return _thong_tin_khac
            End Get
            Set(ByVal Value As String)
                _thong_tin_khac = Value
            End Set
        End Property

        Public Property is_update_crm() As Nullable(Of Boolean)
            Get
                Return _is_update_crm
            End Get
            Set(ByVal Value As Nullable(Of Boolean))
                _is_update_crm = Value
            End Set
        End Property

        Public Property created_date() As Nullable(Of DateTime)
            Get
                Return _created_date
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _created_date = Value
            End Set
        End Property

    End Class
End Namespace