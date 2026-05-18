'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Banner
    Public Class BannerAdvInfo
        Private _id As Integer
        Private _Title As String
        Private _KieuBanner As Integer
        Private _IMGLink As String
        Private _Vitri As Integer
        Private _Height As Integer
        Private _Width As Integer
        Private _PortalId As Integer
        Private _UserId As Integer
        Private _Visible As Boolean
        Private _CreatedDate As DateTime
        Private _Ordernumber As Integer
        Private _Link As String
        Private _Startdate As DateTime
        Private _enddate As DateTime
        Private _Contact As String
        Private _Click As Integer
        Private _sView As Integer


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
        Public Property KieuBanner() As Integer
            Get
                Return _KieuBanner
            End Get
            Set(ByVal Value As Integer)
                _KieuBanner = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IMGLink() As String
            Get
                Return _IMGLink
            End Get
            Set(ByVal Value As String)
                _IMGLink = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Vitri() As Integer
            Get
                Return _Vitri
            End Get
            Set(ByVal Value As Integer)
                _Vitri = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Height() As Integer
            Get
                Return _Height
            End Get
            Set(ByVal Value As Integer)
                _Height = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Width() As Integer
            Get
                Return _Width
            End Get
            Set(ByVal Value As Integer)
                _Width = Value
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
        Public Property UserId() As Integer
            Get
                Return _UserId
            End Get
            Set(ByVal Value As Integer)
                _UserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Visible() As Boolean
            Get
                Return _Visible
            End Get
            Set(ByVal Value As Boolean)
                _Visible = Value
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
        Public Property Ordernumber() As Integer
            Get
                Return _Ordernumber
            End Get
            Set(ByVal Value As Integer)
                _Ordernumber = Value
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
        Public Property Startdate() As DateTime
            Get
                Return _Startdate
            End Get
            Set(ByVal Value As DateTime)
                _Startdate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property enddate() As DateTime
            Get
                Return _enddate
            End Get
            Set(ByVal Value As DateTime)
                _enddate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Contact() As String
            Get
                Return _Contact
            End Get
            Set(ByVal Value As String)
                _Contact = Value
            End Set
        End Property



        '------------------------------------------'
        Public Property Click() As Integer
            Get
                Return _Click
            End Get
            Set(ByVal Value As Integer)
                _Click = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property sView() As Integer
            Get
                Return _sView
            End Get
            Set(ByVal Value As Integer)
                _sView = Value
            End Set
        End Property
        '------------------------------------------'
        Public ReadOnly Property TenVitri() As String
            Get
                Dim strCacheKeystatus As String
                strCacheKeystatus = "TenVitri:" & Vitri
                Dim strResultstatus As String = String.Empty
                strResultstatus = DataCache.GetCache(strCacheKeystatus)
                If strResultstatus = "" Then
                    Dim ctl As New BannerAdv_VitriController
                    Dim obj As BannerAdv_VitriInfo = ctl._Vitri_GetByID(Vitri)
                    If Not obj Is Nothing Then
                        strResultstatus = obj.Title
                    Else
                        strResultstatus = ""
                    End If
                    DataCache.SetCache(strCacheKeystatus, strResultstatus)
                End If

                Return strResultstatus
            End Get
        End Property
        '------------------------------------------'
    End Class
End Namespace