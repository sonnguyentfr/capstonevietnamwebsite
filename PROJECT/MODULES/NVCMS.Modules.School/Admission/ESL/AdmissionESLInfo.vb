'******************************************
'Author         :SonNT 
'Created Date   :12/26/2017
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.School
    Public Class AdmissionESLInfo
        Private _id As Integer
        Private _AdmFall As DateTime
        Private _AdmWinter As DateTime
        Private _AdmSpring As DateTime
        Private _AdmSummer As DateTime
        Private _AdmRoll As Boolean
        Private _TypeOfCourse As String
        Private _LCName As String
        Private _LCLenght As String
        Private _LCCost As String
        Private _Conditional As String
        Private _RateOfStudent As Integer
        Private _NOSTotal As Integer
        Private _NOSInternation As Integer
        Private _NOSVietnames As Integer
        Private _NOSRatio As String
        Private _WorkOpp As Boolean
        Private _COSTuti As Integer
        Private _COSBook As Integer
        Private _COSHealth As Integer
        Private _COSRoom As Integer
        Private _COSTrans As Integer
        Private _COSOther As String
        Private _Scholarship As Boolean
        Private _ScholarshipRange As String
        Private _Financial As Boolean
        Private _FinancialRange As String
        Private _HousingOncampus As Boolean
        Private _HousingHomeStay As Boolean
        Private _HousingApartment As Boolean
        Private _HousingService As Boolean
        Private _HousingOther As String
        Private _Createddate As DateTime
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
        Public Property AdmFall() As DateTime
            Get
                Return _AdmFall
            End Get
            Set(ByVal Value As DateTime)
                _AdmFall = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property AdmWinter() As DateTime
            Get
                Return _AdmWinter
            End Get
            Set(ByVal Value As DateTime)
                _AdmWinter = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property AdmSpring() As DateTime
            Get
                Return _AdmSpring
            End Get
            Set(ByVal Value As DateTime)
                _AdmSpring = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property AdmSummer() As DateTime
            Get
                Return _AdmSummer
            End Get
            Set(ByVal Value As DateTime)
                _AdmSummer = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property AdmRoll() As Boolean
            Get
                Return _AdmRoll
            End Get
            Set(ByVal Value As Boolean)
                _AdmRoll = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TypeOfCourse() As String
            Get
                Return _TypeOfCourse
            End Get
            Set(ByVal Value As String)
                _TypeOfCourse = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property LCName() As String
            Get
                Return _LCName
            End Get
            Set(ByVal Value As String)
                _LCName = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property LCLenght() As String
            Get
                Return _LCLenght
            End Get
            Set(ByVal Value As String)
                _LCLenght = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LCCost() As String
            Get
                Return _LCCost
            End Get
            Set(ByVal Value As String)
                _LCCost = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Conditional() As String
            Get
                Return _Conditional
            End Get
            Set(ByVal Value As String)
                _Conditional = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property RateOfStudent() As Integer
            Get
                Return _RateOfStudent
            End Get
            Set(ByVal Value As Integer)
                _RateOfStudent = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSTotal() As Integer
            Get
                Return _NOSTotal
            End Get
            Set(ByVal Value As Integer)
                _NOSTotal = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSInternation() As Integer
            Get
                Return _NOSInternation
            End Get
            Set(ByVal Value As Integer)
                _NOSInternation = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSVietnames() As Integer
            Get
                Return _NOSVietnames
            End Get
            Set(ByVal Value As Integer)
                _NOSVietnames = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSRatio() As String
            Get
                Return _NOSRatio
            End Get
            Set(ByVal Value As String)
                _NOSRatio = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property WorkOpp() As Boolean
            Get
                Return _WorkOpp
            End Get
            Set(ByVal Value As Boolean)
                _WorkOpp = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTuti() As Integer
            Get
                Return _COSTuti
            End Get
            Set(ByVal Value As Integer)
                _COSTuti = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSBook() As Integer
            Get
                Return _COSBook
            End Get
            Set(ByVal Value As Integer)
                _COSBook = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSHealth() As Integer
            Get
                Return _COSHealth
            End Get
            Set(ByVal Value As Integer)
                _COSHealth = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSRoom() As Integer
            Get
                Return _COSRoom
            End Get
            Set(ByVal Value As Integer)
                _COSRoom = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTrans() As Integer
            Get
                Return _COSTrans
            End Get
            Set(ByVal Value As Integer)
                _COSTrans = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Scholarship() As Boolean
            Get
                Return _Scholarship
            End Get
            Set(ByVal Value As Boolean)
                _Scholarship = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ScholarshipRange() As String
            Get
                Return _ScholarshipRange
            End Get
            Set(ByVal Value As String)
                _ScholarshipRange = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property COSOther() As String
            Get
                Return _COSOther
            End Get
            Set(ByVal Value As String)
                _COSOther = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Financial() As Boolean
            Get
                Return _Financial
            End Get
            Set(ByVal Value As Boolean)
                _Financial = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property FinancialRange() As String
            Get
                Return _FinancialRange
            End Get
            Set(ByVal Value As String)
                _FinancialRange = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property HousingOncampus() As Boolean
            Get
                Return _HousingOncampus
            End Get
            Set(ByVal Value As Boolean)
                _HousingOncampus = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingHomeStay() As Boolean
            Get
                Return _HousingHomeStay
            End Get
            Set(ByVal Value As Boolean)
                _HousingHomeStay = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingApartment() As Boolean
            Get
                Return _HousingApartment
            End Get
            Set(ByVal Value As Boolean)
                _HousingApartment = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingService() As Boolean
            Get
                Return _HousingService
            End Get
            Set(ByVal Value As Boolean)
                _HousingService = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOther() As String
            Get
                Return _HousingOther
            End Get
            Set(ByVal Value As String)
                _HousingOther = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Createddate() As DateTime
            Get
                Return _Createddate
            End Get
            Set(ByVal Value As DateTime)
                _Createddate = Value
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