'******************************************
'Author         :SonNT 
'Created Date   :12/26/2017
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.School
    Public Class AdmissionBFInfo
        Private _id As Integer
        Private _currency As Integer
        Private _AdmFall As DateTime
        Private _AdmWinter As DateTime
        Private _AdmSpring As DateTime
        Private _AdmSummer As DateTime
        Private _AdmRoll As Boolean
        Private _Gradesfrom As Integer
        Private _Gradesto As Integer
        Private _Top5School As String
        Private _Top5Percen As Integer
        Private _ASScore As String
        Private _APCourse As Boolean
        Private _APCourseList As Integer
        Private _IBCourse As Boolean
        Private _IBCourseList As Integer
        Private _HonorsCourse As Boolean
        Private _HonorsCourseList As Integer
        Private _Linkofweb As String
        Private _Top5Extract As String
        Private _Top5ExtractVN As String
        Private _StandardiziedTest As Boolean
        Private _TESTToefl As Boolean
        Private _TESTToeflMin As Decimal
        Private _TESTIELTS As Boolean
        Private _TESTIELTSMin As Integer
        Private _TESTSSAT As Boolean
        Private _TESTSSATMin As Integer
        Private _TESTSLEP As Boolean
        Private _TESTSLEPMin As Integer
        Private _TESTSALTE As Boolean
        Private _TESTSALTEMin As Integer
        Private _TESTOther As String
        Private _EnglishPlacementtest As Boolean
        Private _ESL As Boolean
        Private _NOSTotal As Integer
        Private _NOSInternation As Integer
        Private _NOSVietnames As Integer
        Private _NOSRatio As String
        Private _COSTuti As Integer
        Private _COSBook As Integer
        Private _COSHealth As Integer
        Private _COSRoom As Integer
        Private _COSOther As String
        Private _SummerProgram As Boolean
        Private _SummerProgramAges As String
        Private _SummerProgramDuration As String
        Private _SummerProgramDeadline As String
        Private _SummerProgramCOST As String
        Private _SummerProgramOther As String
        Private _ScholarshipInternation As Boolean
        Private _ScholarshipInternationRang As String
        Private _ScholarshipInternationRangVN As String
        Private _OtherFinancial As Boolean
        Private _OtherFinancialRang As String
        Private _OtherFinancialRangVN As String
        Private _HousingBF As Boolean
        Private _HousingHome As Boolean
        Private _HousingPlacement As Boolean
        Private _Studentrequiredoncampus As Boolean
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
        Public Property currency() As Integer
            Get
                Return _currency
            End Get
            Set(ByVal Value As Integer)
                _currency = Value
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
        Public Property Gradesfrom() As Integer
            Get
                Return _Gradesfrom
            End Get
            Set(ByVal Value As Integer)
                _Gradesfrom = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Gradesto() As Integer
            Get
                Return _Gradesto
            End Get
            Set(ByVal Value As Integer)
                _Gradesto = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Top5School() As String
            Get
                Return _Top5School
            End Get
            Set(ByVal Value As String)
                _Top5School = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Top5Percen() As Integer
            Get
                Return _Top5Percen
            End Get
            Set(ByVal Value As Integer)
                _Top5Percen = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ASScore() As String
            Get
                Return _ASScore
            End Get
            Set(ByVal Value As String)
                _ASScore = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property APCourse() As Boolean
            Get
                Return _APCourse
            End Get
            Set(ByVal Value As Boolean)
                _APCourse = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property APCourseList() As Integer
            Get
                Return _APCourseList
            End Get
            Set(ByVal Value As Integer)
                _APCourseList = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IBCourse() As Boolean
            Get
                Return _IBCourse
            End Get
            Set(ByVal Value As Boolean)
                _IBCourse = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IBCourseList() As Integer
            Get
                Return _IBCourseList
            End Get
            Set(ByVal Value As Integer)
                _IBCourseList = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HonorsCourse() As Boolean
            Get
                Return _HonorsCourse
            End Get
            Set(ByVal Value As Boolean)
                _HonorsCourse = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HonorsCourseList() As Integer
            Get
                Return _HonorsCourseList
            End Get
            Set(ByVal Value As Integer)
                _HonorsCourseList = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Linkofweb() As String
            Get
                Return _Linkofweb
            End Get
            Set(ByVal Value As String)
                _Linkofweb = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Top5Extract() As String
            Get
                Return _Top5Extract
            End Get
            Set(ByVal Value As String)
                _Top5Extract = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Top5ExtractVN() As String
            Get
                Return _Top5ExtractVN
            End Get
            Set(ByVal Value As String)
                _Top5ExtractVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property StandardiziedTest() As Boolean
            Get
                Return _StandardiziedTest
            End Get
            Set(ByVal Value As Boolean)
                _StandardiziedTest = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTToefl() As Boolean
            Get
                Return _TESTToefl
            End Get
            Set(ByVal Value As Boolean)
                _TESTToefl = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTToeflMin() As Decimal
            Get
                Return _TESTToeflMin
            End Get
            Set(ByVal Value As Decimal)
                _TESTToeflMin = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTIELTS() As Boolean
            Get
                Return _TESTIELTS
            End Get
            Set(ByVal Value As Boolean)
                _TESTIELTS = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTIELTSMin() As Integer
            Get
                Return _TESTIELTSMin
            End Get
            Set(ByVal Value As Integer)
                _TESTIELTSMin = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTSSAT() As Boolean
            Get
                Return _TESTSSAT
            End Get
            Set(ByVal Value As Boolean)
                _TESTSSAT = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTSSATMin() As Integer
            Get
                Return _TESTSSATMin
            End Get
            Set(ByVal Value As Integer)
                _TESTSSATMin = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTSLEP() As Boolean
            Get
                Return _TESTSLEP
            End Get
            Set(ByVal Value As Boolean)
                _TESTSLEP = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTSLEPMin() As Integer
            Get
                Return _TESTSLEPMin
            End Get
            Set(ByVal Value As Integer)
                _TESTSLEPMin = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTSALTE() As Boolean
            Get
                Return _TESTSALTE
            End Get
            Set(ByVal Value As Boolean)
                _TESTSALTE = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTSALTEMin() As Integer
            Get
                Return _TESTSALTEMin
            End Get
            Set(ByVal Value As Integer)
                _TESTSALTEMin = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TESTOther() As String
            Get
                Return _TESTOther
            End Get
            Set(ByVal Value As String)
                _TESTOther = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property EnglishPlacementtest() As Boolean
            Get
                Return _EnglishPlacementtest
            End Get
            Set(ByVal Value As Boolean)
                _EnglishPlacementtest = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ESL() As Boolean
            Get
                Return _ESL
            End Get
            Set(ByVal Value As Boolean)
                _ESL = Value
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
        Public Property COSOther() As String
            Get
                Return _COSOther
            End Get
            Set(ByVal Value As String)
                _COSOther = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerProgram() As Boolean
            Get
                Return _SummerProgram
            End Get
            Set(ByVal Value As Boolean)
                _SummerProgram = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerProgramAges() As String
            Get
                Return _SummerProgramAges
            End Get
            Set(ByVal Value As String)
                _SummerProgramAges = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerProgramDuration() As String
            Get
                Return _SummerProgramDuration
            End Get
            Set(ByVal Value As String)
                _SummerProgramDuration = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerProgramDeadline() As String
            Get
                Return _SummerProgramDeadline
            End Get
            Set(ByVal Value As String)
                _SummerProgramDeadline = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerProgramCOST() As String
            Get
                Return _SummerProgramCOST
            End Get
            Set(ByVal Value As String)
                _SummerProgramCOST = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerProgramOther() As String
            Get
                Return _SummerProgramOther
            End Get
            Set(ByVal Value As String)
                _SummerProgramOther = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipInternation() As Boolean
            Get
                Return _ScholarshipInternation
            End Get
            Set(ByVal Value As Boolean)
                _ScholarshipInternation = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipInternationRang() As String
            Get
                Return _ScholarshipInternationRang
            End Get
            Set(ByVal Value As String)
                _ScholarshipInternationRang = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipInternationRangVN() As String
            Get
                Return _ScholarshipInternationRangVN
            End Get
            Set(ByVal Value As String)
                _ScholarshipInternationRangVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherFinancial() As Boolean
            Get
                Return _OtherFinancial
            End Get
            Set(ByVal Value As Boolean)
                _OtherFinancial = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherFinancialRang() As String
            Get
                Return _OtherFinancialRang
            End Get
            Set(ByVal Value As String)
                _OtherFinancialRang = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherFinancialRangVN() As String
            Get
                Return _OtherFinancialRangVN
            End Get
            Set(ByVal Value As String)
                _OtherFinancialRangVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingBF() As Boolean
            Get
                Return _HousingBF
            End Get
            Set(ByVal Value As Boolean)
                _HousingBF = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingHome() As Boolean
            Get
                Return _HousingHome
            End Get
            Set(ByVal Value As Boolean)
                _HousingHome = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingPlacement() As Boolean
            Get
                Return _HousingPlacement
            End Get
            Set(ByVal Value As Boolean)
                _HousingPlacement = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Studentrequiredoncampus() As Boolean
            Get
                Return _Studentrequiredoncampus
            End Get
            Set(ByVal Value As Boolean)
                _Studentrequiredoncampus = Value
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