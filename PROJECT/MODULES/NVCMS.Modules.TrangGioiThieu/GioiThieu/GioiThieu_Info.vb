'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.TrangGioiThieu
    Public Class GioiThieu_Info
        Private _id As Integer
        Private _TrangDanhMuc As String
        Private _Tieudephu As String
        Private _ImagePath As String
        Private _tomtat As String
        Private _Noidung As String
        Private _Link As String
        Private _ParentId As Integer
        Private _Ordernumber As Integer
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
        Public Property TrangDanhMuc() As String
            Get
                Return _TrangDanhMuc
            End Get
            Set(ByVal Value As String)
                _TrangDanhMuc = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Tieudephu() As String
            Get
                Return _Tieudephu
            End Get
            Set(ByVal Value As String)
                _Tieudephu = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ImagePath() As String
            Get
                Return _ImagePath
            End Get
            Set(ByVal Value As String)
                _ImagePath = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property tomtat() As String
            Get
                Return _tomtat
            End Get
            Set(ByVal Value As String)
                _tomtat = Value
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
        Public Property Link() As String
            Get
                Return _Link
            End Get
            Set(ByVal Value As String)
                _Link = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ParentId() As Integer
            Get
                Return _ParentId
            End Get
            Set(ByVal Value As Integer)
                _ParentId = Value
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
    End Class
End Namespace