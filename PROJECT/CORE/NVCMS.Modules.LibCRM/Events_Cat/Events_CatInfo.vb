'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.LibCRM
    Public Class Lib_Events_CatInfo
        Private _id As Integer
        Private _CatName As String
        Private _CatNameEN As String
        Private _Marketing As Integer
        Private _chonnhieu As Boolean
        Private _CODE As String
        Private _Source As String
        Private _Email As String
        Private _DateShow As String
        Private _FromDate As DateTime
        Private _EndDate As DateTime
        Private _Avatar As String
        Private _Desception As String
        Private _DesceptionEN As String
        Private _Contentx As String
        Private _ContentxEN As String
        Private _ContentMail As String
        Private _CreatedDate As DateTime
        Private _UserId As Integer
        Private _PortalId As Integer
        Private _Isactive As Boolean
        Private _Ordernumber As Integer
        Private _FairSchool As String
        Private _FairDiengia As String
        Private _FairTestimonial As String
        Private _FairDonviTaiTro As String
        Private _FairOrg As String
        Private _TabId As Integer
        Private _sendmail As Boolean
        Private _sendCode As Boolean
        Private _titleMail As String
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
        Public Property titleMail() As String
            Get
                Return _titleMail
            End Get
            Set(ByVal Value As String)
                _titleMail = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property CatName() As String
            Get
                Return _CatName
            End Get
            Set(ByVal Value As String)
                _CatName = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property CatNameEN() As String
            Get
                Return _CatNameEN
            End Get
            Set(ByVal Value As String)
                _CatNameEN = Value
            End Set
        End Property
        Public Property Marketing() As Integer
            Get
                Return _Marketing
            End Get
            Set(ByVal Value As Integer)
                _Marketing = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property chonnhieu() As Integer
            Get
                Return _chonnhieu
            End Get
            Set(ByVal Value As Integer)
                _chonnhieu = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Source() As String
            Get
                Return _Source
            End Get
            Set(ByVal Value As String)
                _Source = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal Value As String)
                _Email = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Code() As String
            Get
                Return _CODE
            End Get
            Set(ByVal Value As String)
                _CODE = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property DateShow() As String
            Get
                Return _DateShow
            End Get
            Set(ByVal Value As String)
                _DateShow = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property FromDate() As DateTime
            Get
                Return _FromDate
            End Get
            Set(ByVal Value As DateTime)
                _FromDate = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property EndDate() As DateTime
            Get
                Return _EndDate
            End Get
            Set(ByVal Value As DateTime)
                _EndDate = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Avatar() As String
            Get
                Return _Avatar
            End Get
            Set(ByVal Value As String)
                _Avatar = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Desception() As String
            Get
                Return _Desception
            End Get
            Set(ByVal Value As String)
                _Desception = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property DesceptionEN() As String
            Get
                Return _DesceptionEN
            End Get
            Set(ByVal Value As String)
                _DesceptionEN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Contentx() As String
            Get
                Return _Contentx
            End Get
            Set(ByVal Value As String)
                _Contentx = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ContentxEN() As String
            Get
                Return _ContentxEN
            End Get
            Set(ByVal Value As String)
                _ContentxEN = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ContentMail() As String
            Get
                Return _ContentMail
            End Get
            Set(ByVal Value As String)
                _ContentMail = Value
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
        Public Property Isactive() As Boolean
            Get
                Return _Isactive
            End Get
            Set(ByVal Value As Boolean)
                _Isactive = Value
            End Set
        End Property

        '------------------------------------------'
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
        Public Property FairSchool() As String
            Get
                Return _FairSchool
            End Get
            Set(ByVal Value As String)
                _FairSchool = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property FairDiengia() As String
            Get
                Return _FairDiengia
            End Get
            Set(ByVal Value As String)
                _FairDiengia = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property FairTestimonial() As String
            Get
                Return _FairTestimonial
            End Get
            Set(ByVal Value As String)
                _FairTestimonial = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property FairDonviTaiTro() As String
            Get
                Return _FairDonviTaiTro
            End Get
            Set(ByVal Value As String)
                _FairDonviTaiTro = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property FairOrg() As String
            Get
                Return _FairOrg
            End Get
            Set(ByVal Value As String)
                _FairOrg = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Tabid() As Integer
            Get
                Return _TabId
            End Get
            Set(ByVal Value As Integer)
                _TabId = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property sendmail() As Boolean
            Get
                Return _sendmail
            End Get
            Set(ByVal Value As Boolean)
                _sendmail = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property sendCode() As Boolean
            Get
                Return _sendCode
            End Get
            Set(ByVal Value As Boolean)
                _sendCode = Value
            End Set
        End Property
    End Class
End Namespace