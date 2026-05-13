Namespace NVCMS.Modules.School
    Public Class Admission4YearInfo
        Private _id As Integer
        Private _currency As Integer
        Private _TestRequirementsTOEFLESL As Boolean
        Private _TestRequirementsTOEFLUnder As Boolean
        Private _TestRequirementsTOEFLGrad As Boolean
        Private _TestRequirementsSATUnder As Boolean
        Private _TestRequirementsGMATGrad As Boolean
        Private _FallESL As DateTime
        Private _FallUnder As DateTime
        Private _FallGrad As DateTime
        Private _FallAss As DateTime
        Private _WinterESL As DateTime
        Private _WinterUnder As DateTime
        Private _WinterGrad As DateTime
        Private _WinterAss As DateTime
        Private _SpringESL As DateTime
        Private _SpringUnder As DateTime
        Private _SpringGrad As DateTime
        Private _SpringAss As DateTime
        Private _SummerESL As DateTime
        Private _SummerUnder As DateTime
        Private _SummerGrad As DateTime
        Private _SummerAss As DateTime
        Private _RollingESL As Boolean
        Private _RollingUnder As Boolean
        Private _RollingGrad As Boolean
        Private _RollingAss As Boolean
        Private _ToeflCBTESL As String
        Private _ToeflCBTUnder As String
        Private _ToeflCBTGrad As String
        Private _ToeflCBTAss As String
        Private _ToefliBTESL As String
        Private _ToefliBTUnder As String
        Private _ToefliBTGrad As String
        Private _ToeflPBTESL As String
        Private _ToeflPBTUnder As String
        Private _ToeflPBTGrad As String
        Private _IELTSESL As String
        Private _IELTSUnder As String
        Private _IELTSGrad As String
        Private _IELTSAss As String
        Private _iTEPESL As String
        Private _iTEPUnder As String
        Private _iTEPGrad As String
        Private _iTEPAss As String
        Private _SAT1ESL As String
        Private _SAT1Under As String
        Private _SAT1Grad As String
        Private _SAT2ESL As String
        Private _SAT2Under As String
        Private _SAT2Grad As String
        Private _GREESL As String
        Private _GREUnder As String
        Private _GREGrad As String
        Private _GMATESL As String
        Private _GMATUnder As String
        Private _GMATGrad As String
        Private _OtherESL As String
        Private _OtherUnder As String
        Private _OtherGrad As String
        Private _OtherAss As String
        Private _NOSTotalESL As Integer
        Private _NOSTotalUnder As Integer
        Private _NOSTotalGrad As Integer
        Private _NOSTotalAss As Integer
        Private _NOSInternationalESL As Integer
        Private _NOSInternationalUnder As Integer
        Private _NOSInternationalGrad As Integer
        Private _NOSInternationalAss As Integer
        Private _NOSVNESL As Integer
        Private _NOSVNUnder As Integer
        Private _NOSVNGrad As Integer
        Private _NOSVNAss As Integer
        Private _NOSStudentFacultyRatioESL As String
        Private _NOSStudentFacultyRatioUnder As String
        Private _NOSStudentFacultyRatioGrad As String
        Private _NOSStudentFacultyRatioAss As String
        Private _COSTuitionfeeESL As Integer
        Private _COSTuitionfeeUnder As Integer
        Private _COSTuitionfeeGrad As Integer
        Private _COSTuitionfeeAss As Integer
        Private _COSBooksuppliesESL As Integer
        Private _COSBooksuppliesUnder As Integer
        Private _COSBooksuppliesGrad As Integer
        Private _COSBooksuppliesAss As Integer
        Private _COSHealthESL As Integer
        Private _COSHealthUnder As Integer
        Private _COSHealthGrad As Integer
        Private _COSHealthAss As Integer
        Private _COSRoomESL As Integer
        Private _COSRoomUnder As Integer
        Private _COSRoomGrad As Integer
        Private _COSRoomAss As Integer
        Private _COSTransportESL As Integer
        Private _COSTransportUnder As Integer
        Private _COSTransportGrad As Integer
        Private _COSTransportAss As Integer
        Private _COSOtherESL As Integer
        Private _COSOtherUnder As Integer
        Private _COSOtherGrad As Integer
        Private _COSOtherAss As Integer
        Private _IntensiveEnglish As Boolean
        Private _HighSchoolCompletion As Boolean
        Private _FSESL As String
        Private _FSUnder As String
        Private _FSGrad As String
        Private _FSAcademic As String
        Private _MostMajor As String
        Private _Top5TransferSchools As String
        Private _OnCampus As Boolean
        Private _GraduationRate As String
        Private _EmploymentRateAfterGraduation As String
        Private _ScholarshipESL As Boolean
        Private _ScholarshipESLRange As String
        Private _ScholarshipESLRangeVN As String
        Private _ScholarshipUnder As Boolean
        Private _ScholarshipUnderRange As String
        Private _ScholarshipUnderRangeVN As String
        Private _ScholarshipUnderTranfer As Boolean
        Private _ScholarshipUnderTranferRange As String
        Private _ScholarshipUnderTranferRangeVN As String
        Private _ScholarshipGrad As Boolean
        Private _ScholarshipGradRange As String
        Private _ScholarshipGradRangeVN As String
        Private _ScholarshipNote As String
        Private _ScholarshipNoteVN As String
        Private _ScholarshipAss As Boolean
        Private _ScholarshipAssRange As String
        Private _ScholarshipAssRangeVN As String
        Private _OtherfinancialESL As Boolean
        Private _OtherfinancialESLRange As String
        Private _OtherfinancialESLRangeVN As String
        Private _OtherfinancialUnder As Boolean
        Private _OtherfinancialUnderRange As String
        Private _OtherfinancialUnderRangeVN As String
        Private _OtherfinancialUnderTranfer As Boolean
        Private _OtherfinancialUnderTranferRange As String
        Private _OtherfinancialUnderTranferRangeVN As String
        Private _OtherfinancialGrad As Boolean
        Private _OtherfinancialGradRange As String
        Private _OtherfinancialGradRangeVN As String
        Private _OtherfinancialNote As String
        Private _OtherfinancialNoteVN As String
        Private _OtherfinancialAss As Boolean
        Private _OtherfinancialAssRange As String
        Private _OtherfinancialAssRangeVN As String
        Private _HousingOptionOncampusESL As Boolean
        Private _HousingOptionOncampusUnder As Boolean
        Private _HousingOptionOncampusGrad As Boolean
        Private _HousingOptionOncampusAss As Boolean
        Private _HousingOptionHostFamilyESL As Boolean
        Private _HousingOptionHostFamilyUnder As Boolean
        Private _HousingOptionHostFamilyGrad As Boolean
        Private _HousingOptionHostFamilyAss As Boolean
        Private _HousingOptionApertmentESL As Boolean
        Private _HousingOptionApertmentUnder As Boolean
        Private _HousingOptionApertmentGrad As Boolean
        Private _HousingOptionApertmentAss As Boolean
        Private _HousingOptionHousingESL As Boolean
        Private _HousingOptionHousingUnder As Boolean
        Private _HousingOptionHousingGrad As Boolean
        Private _HousingOptionHousingAss As Boolean
        Private _HousingOptionOtherESL As String
        Private _HousingOptionOtherESLVN As String
        Private _HousingOptionOtherUnder As String
        Private _HousingOptionOtherUnderVN As String
        Private _HousingOptionOtherGrad As String
        Private _HousingOptionOtherGradVN As String
        Private _HousingOptionOtherAss As String
        Private _HousingOptionOtherAssVN As String
        Private _HousingOptionRequirecampus As Boolean
        Private _CreatedDate As DateTime
        Private _UserId As Integer
        Private _Portalid As Integer


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
        Public Property TestRequirementsTOEFLESL() As Boolean
            Get
                Return _TestRequirementsTOEFLESL
            End Get
            Set(ByVal Value As Boolean)
                _TestRequirementsTOEFLESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TestRequirementsTOEFLUnder() As Boolean
            Get
                Return _TestRequirementsTOEFLUnder
            End Get
            Set(ByVal Value As Boolean)
                _TestRequirementsTOEFLUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TestRequirementsTOEFLGrad() As Boolean
            Get
                Return _TestRequirementsTOEFLGrad
            End Get
            Set(ByVal Value As Boolean)
                _TestRequirementsTOEFLGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TestRequirementsSATUnder() As Boolean
            Get
                Return _TestRequirementsSATUnder
            End Get
            Set(ByVal Value As Boolean)
                _TestRequirementsSATUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TestRequirementsGMATGrad() As Boolean
            Get
                Return _TestRequirementsGMATGrad
            End Get
            Set(ByVal Value As Boolean)
                _TestRequirementsGMATGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FallESL() As DateTime
            Get
                Return _FallESL
            End Get
            Set(ByVal Value As DateTime)
                _FallESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FallUnder() As DateTime
            Get
                Return _FallUnder
            End Get
            Set(ByVal Value As DateTime)
                _FallUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FallGrad() As DateTime
            Get
                Return _FallGrad
            End Get
            Set(ByVal Value As DateTime)
                _FallGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FallAss() As DateTime
            Get
                Return _FallAss
            End Get
            Set(ByVal Value As DateTime)
                _FallAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property WinterESL() As DateTime
            Get
                Return _WinterESL
            End Get
            Set(ByVal Value As DateTime)
                _WinterESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property WinterUnder() As DateTime
            Get
                Return _WinterUnder
            End Get
            Set(ByVal Value As DateTime)
                _WinterUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property WinterGrad() As DateTime
            Get
                Return _WinterGrad
            End Get
            Set(ByVal Value As DateTime)
                _WinterGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property WinterAss() As DateTime
            Get
                Return _WinterAss
            End Get
            Set(ByVal Value As DateTime)
                _WinterAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SpringESL() As DateTime
            Get
                Return _SpringESL
            End Get
            Set(ByVal Value As DateTime)
                _SpringESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SpringUnder() As DateTime
            Get
                Return _SpringUnder
            End Get
            Set(ByVal Value As DateTime)
                _SpringUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SpringGrad() As DateTime
            Get
                Return _SpringGrad
            End Get
            Set(ByVal Value As DateTime)
                _SpringGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SpringAss() As DateTime
            Get
                Return _SpringAss
            End Get
            Set(ByVal Value As DateTime)
                _SpringAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerESL() As DateTime
            Get
                Return _SummerESL
            End Get
            Set(ByVal Value As DateTime)
                _SummerESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerUnder() As DateTime
            Get
                Return _SummerUnder
            End Get
            Set(ByVal Value As DateTime)
                _SummerUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerGrad() As DateTime
            Get
                Return _SummerGrad
            End Get
            Set(ByVal Value As DateTime)
                _SummerGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SummerAss() As DateTime
            Get
                Return _SummerAss
            End Get
            Set(ByVal Value As DateTime)
                _SummerAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property RollingESL() As Boolean
            Get
                Return _RollingESL
            End Get
            Set(ByVal Value As Boolean)
                _RollingESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property RollingUnder() As Boolean
            Get
                Return _RollingUnder
            End Get
            Set(ByVal Value As Boolean)
                _RollingUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property RollingGrad() As Boolean
            Get
                Return _RollingGrad
            End Get
            Set(ByVal Value As Boolean)
                _RollingGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property RollingAss() As Boolean
            Get
                Return _RollingAss
            End Get
            Set(ByVal Value As Boolean)
                _RollingAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToeflCBTESL() As String
            Get
                Return _ToeflCBTESL
            End Get
            Set(ByVal Value As String)
                _ToeflCBTESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToeflCBTUnder() As String
            Get
                Return _ToeflCBTUnder
            End Get
            Set(ByVal Value As String)
                _ToeflCBTUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToeflCBTGrad() As String
            Get
                Return _ToeflCBTGrad
            End Get
            Set(ByVal Value As String)
                _ToeflCBTGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToeflCBTAss() As String
            Get
                Return _ToeflCBTAss
            End Get
            Set(ByVal Value As String)
                _ToeflCBTAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToefliBTESL() As String
            Get
                Return _ToefliBTESL
            End Get
            Set(ByVal Value As String)
                _ToefliBTESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToefliBTUnder() As String
            Get
                Return _ToefliBTUnder
            End Get
            Set(ByVal Value As String)
                _ToefliBTUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToefliBTGrad() As String
            Get
                Return _ToefliBTGrad
            End Get
            Set(ByVal Value As String)
                _ToefliBTGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToeflPBTESL() As String
            Get
                Return _ToeflPBTESL
            End Get
            Set(ByVal Value As String)
                _ToeflPBTESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToeflPBTUnder() As String
            Get
                Return _ToeflPBTUnder
            End Get
            Set(ByVal Value As String)
                _ToeflPBTUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ToeflPBTGrad() As String
            Get
                Return _ToeflPBTGrad
            End Get
            Set(ByVal Value As String)
                _ToeflPBTGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IELTSESL() As String
            Get
                Return _IELTSESL
            End Get
            Set(ByVal Value As String)
                _IELTSESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IELTSUnder() As String
            Get
                Return _IELTSUnder
            End Get
            Set(ByVal Value As String)
                _IELTSUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IELTSGrad() As String
            Get
                Return _IELTSGrad
            End Get
            Set(ByVal Value As String)
                _IELTSGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IELTSAss() As String
            Get
                Return _IELTSAss
            End Get
            Set(ByVal Value As String)
                _IELTSAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property iTEPESL() As String
            Get
                Return _iTEPESL
            End Get
            Set(ByVal Value As String)
                _iTEPESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property iTEPUnder() As String
            Get
                Return _iTEPUnder
            End Get
            Set(ByVal Value As String)
                _iTEPUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property iTEPGrad() As String
            Get
                Return _iTEPGrad
            End Get
            Set(ByVal Value As String)
                _iTEPGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property iTEPAss() As String
            Get
                Return _iTEPAss
            End Get
            Set(ByVal Value As String)
                _iTEPAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SAT1ESL() As String
            Get
                Return _SAT1ESL
            End Get
            Set(ByVal Value As String)
                _SAT1ESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SAT1Under() As String
            Get
                Return _SAT1Under
            End Get
            Set(ByVal Value As String)
                _SAT1Under = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SAT1Grad() As String
            Get
                Return _SAT1Grad
            End Get
            Set(ByVal Value As String)
                _SAT1Grad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SAT2ESL() As String
            Get
                Return _SAT2ESL
            End Get
            Set(ByVal Value As String)
                _SAT2ESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SAT2Under() As String
            Get
                Return _SAT2Under
            End Get
            Set(ByVal Value As String)
                _SAT2Under = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SAT2Grad() As String
            Get
                Return _SAT2Grad
            End Get
            Set(ByVal Value As String)
                _SAT2Grad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property GREESL() As String
            Get
                Return _GREESL
            End Get
            Set(ByVal Value As String)
                _GREESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property GREUnder() As String
            Get
                Return _GREUnder
            End Get
            Set(ByVal Value As String)
                _GREUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property GREGrad() As String
            Get
                Return _GREGrad
            End Get
            Set(ByVal Value As String)
                _GREGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property GMATESL() As String
            Get
                Return _GMATESL
            End Get
            Set(ByVal Value As String)
                _GMATESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property GMATUnder() As String
            Get
                Return _GMATUnder
            End Get
            Set(ByVal Value As String)
                _GMATUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property GMATGrad() As String
            Get
                Return _GMATGrad
            End Get
            Set(ByVal Value As String)
                _GMATGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherESL() As String
            Get
                Return _OtherESL
            End Get
            Set(ByVal Value As String)
                _OtherESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherUnder() As String
            Get
                Return _OtherUnder
            End Get
            Set(ByVal Value As String)
                _OtherUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherGrad() As String
            Get
                Return _OtherGrad
            End Get
            Set(ByVal Value As String)
                _OtherGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherAss() As String
            Get
                Return _OtherAss
            End Get
            Set(ByVal Value As String)
                _OtherAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSTotalESL() As Integer
            Get
                Return _NOSTotalESL
            End Get
            Set(ByVal Value As Integer)
                _NOSTotalESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSTotalUnder() As Integer
            Get
                Return _NOSTotalUnder
            End Get
            Set(ByVal Value As Integer)
                _NOSTotalUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSTotalGrad() As Integer
            Get
                Return _NOSTotalGrad
            End Get
            Set(ByVal Value As Integer)
                _NOSTotalGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSTotalAss() As Integer
            Get
                Return _NOSTotalAss
            End Get
            Set(ByVal Value As Integer)
                _NOSTotalAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSInternationalESL() As Integer
            Get
                Return _NOSInternationalESL
            End Get
            Set(ByVal Value As Integer)
                _NOSInternationalESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSInternationalUnder() As Integer
            Get
                Return _NOSInternationalUnder
            End Get
            Set(ByVal Value As Integer)
                _NOSInternationalUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSInternationalGrad() As Integer
            Get
                Return _NOSInternationalGrad
            End Get
            Set(ByVal Value As Integer)
                _NOSInternationalGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSInternationalAss() As Integer
            Get
                Return _NOSInternationalAss
            End Get
            Set(ByVal Value As Integer)
                _NOSInternationalAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSVNESL() As Integer
            Get
                Return _NOSVNESL
            End Get
            Set(ByVal Value As Integer)
                _NOSVNESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSVNUnder() As Integer
            Get
                Return _NOSVNUnder
            End Get
            Set(ByVal Value As Integer)
                _NOSVNUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSVNGrad() As Integer
            Get
                Return _NOSVNGrad
            End Get
            Set(ByVal Value As Integer)
                _NOSVNGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSVNAss() As Integer
            Get
                Return _NOSVNAss
            End Get
            Set(ByVal Value As Integer)
                _NOSVNAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSStudentFacultyRatioESL() As String
            Get
                Return _NOSStudentFacultyRatioESL
            End Get
            Set(ByVal Value As String)
                _NOSStudentFacultyRatioESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSStudentFacultyRatioUnder() As String
            Get
                Return _NOSStudentFacultyRatioUnder
            End Get
            Set(ByVal Value As String)
                _NOSStudentFacultyRatioUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSStudentFacultyRatioGrad() As String
            Get
                Return _NOSStudentFacultyRatioGrad
            End Get
            Set(ByVal Value As String)
                _NOSStudentFacultyRatioGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NOSStudentFacultyRatioAss() As String
            Get
                Return _NOSStudentFacultyRatioAss
            End Get
            Set(ByVal Value As String)
                _NOSStudentFacultyRatioAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTuitionfeeESL() As Integer
            Get
                Return _COSTuitionfeeESL
            End Get
            Set(ByVal Value As Integer)
                _COSTuitionfeeESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTuitionfeeUnder() As Integer
            Get
                Return _COSTuitionfeeUnder
            End Get
            Set(ByVal Value As Integer)
                _COSTuitionfeeUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTuitionfeeGrad() As Integer
            Get
                Return _COSTuitionfeeGrad
            End Get
            Set(ByVal Value As Integer)
                _COSTuitionfeeGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTuitionfeeAss() As Integer
            Get
                Return _COSTuitionfeeAss
            End Get
            Set(ByVal Value As Integer)
                _COSTuitionfeeAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSBooksuppliesESL() As Integer
            Get
                Return _COSBooksuppliesESL
            End Get
            Set(ByVal Value As Integer)
                _COSBooksuppliesESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSBooksuppliesUnder() As Integer
            Get
                Return _COSBooksuppliesUnder
            End Get
            Set(ByVal Value As Integer)
                _COSBooksuppliesUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSBooksuppliesGrad() As Integer
            Get
                Return _COSBooksuppliesGrad
            End Get
            Set(ByVal Value As Integer)
                _COSBooksuppliesGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSBooksuppliesAss() As Integer
            Get
                Return _COSBooksuppliesAss
            End Get
            Set(ByVal Value As Integer)
                _COSBooksuppliesAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSHealthESL() As Integer
            Get
                Return _COSHealthESL
            End Get
            Set(ByVal Value As Integer)
                _COSHealthESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSHealthUnder() As Integer
            Get
                Return _COSHealthUnder
            End Get
            Set(ByVal Value As Integer)
                _COSHealthUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSHealthGrad() As Integer
            Get
                Return _COSHealthGrad
            End Get
            Set(ByVal Value As Integer)
                _COSHealthGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSHealthAss() As Integer
            Get
                Return _COSHealthAss
            End Get
            Set(ByVal Value As Integer)
                _COSHealthAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSRoomESL() As Integer
            Get
                Return _COSRoomESL
            End Get
            Set(ByVal Value As Integer)
                _COSRoomESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSRoomUnder() As Integer
            Get
                Return _COSRoomUnder
            End Get
            Set(ByVal Value As Integer)
                _COSRoomUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSRoomGrad() As Integer
            Get
                Return _COSRoomGrad
            End Get
            Set(ByVal Value As Integer)
                _COSRoomGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSRoomAss() As Integer
            Get
                Return _COSRoomAss
            End Get
            Set(ByVal Value As Integer)
                _COSRoomAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTransportESL() As Integer
            Get
                Return _COSTransportESL
            End Get
            Set(ByVal Value As Integer)
                _COSTransportESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTransportUnder() As Integer
            Get
                Return _COSTransportUnder
            End Get
            Set(ByVal Value As Integer)
                _COSTransportUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTransportGrad() As Integer
            Get
                Return _COSTransportGrad
            End Get
            Set(ByVal Value As Integer)
                _COSTransportGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSTransportAss() As Integer
            Get
                Return _COSTransportAss
            End Get
            Set(ByVal Value As Integer)
                _COSTransportAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSOtherESL() As Integer
            Get
                Return _COSOtherESL
            End Get
            Set(ByVal Value As Integer)
                _COSOtherESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSOtherUnder() As Integer
            Get
                Return _COSOtherUnder
            End Get
            Set(ByVal Value As Integer)
                _COSOtherUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSOtherGrad() As Integer
            Get
                Return _COSOtherGrad
            End Get
            Set(ByVal Value As Integer)
                _COSOtherGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property COSOtherAss() As Integer
            Get
                Return _COSOtherAss
            End Get
            Set(ByVal Value As Integer)
                _COSOtherAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IntensiveEnglish() As Boolean
            Get
                Return _IntensiveEnglish
            End Get
            Set(ByVal Value As Boolean)
                _IntensiveEnglish = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HighSchoolCompletion() As Boolean
            Get
                Return _HighSchoolCompletion
            End Get
            Set(ByVal Value As Boolean)
                _HighSchoolCompletion = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FSESL() As String
            Get
                Return _FSESL
            End Get
            Set(ByVal Value As String)
                _FSESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FSUnder() As String
            Get
                Return _FSUnder
            End Get
            Set(ByVal Value As String)
                _FSUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FSGrad() As String
            Get
                Return _FSGrad
            End Get
            Set(ByVal Value As String)
                _FSGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FSAcademic() As String
            Get
                Return _FSAcademic
            End Get
            Set(ByVal Value As String)
                _FSAcademic = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property MostMajor() As String
            Get
                Return _MostMajor
            End Get
            Set(ByVal Value As String)
                _MostMajor = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Top5TransferSchools() As String
            Get
                Return _Top5TransferSchools
            End Get
            Set(ByVal Value As String)
                _Top5TransferSchools = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OnCampus() As Boolean
            Get
                Return _OnCampus
            End Get
            Set(ByVal Value As Boolean)
                _OnCampus = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property GraduationRate() As String
            Get
                Return _GraduationRate
            End Get
            Set(ByVal Value As String)
                _GraduationRate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property EmploymentRateAfterGraduation() As String
            Get
                Return _EmploymentRateAfterGraduation
            End Get
            Set(ByVal Value As String)
                _EmploymentRateAfterGraduation = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipESL() As Boolean
            Get
                Return _ScholarshipESL
            End Get
            Set(ByVal Value As Boolean)
                _ScholarshipESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipESLRange() As String
            Get
                Return _ScholarshipESLRange
            End Get
            Set(ByVal Value As String)
                _ScholarshipESLRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipESLRangeVN() As String
            Get
                Return _ScholarshipESLRangeVN
            End Get
            Set(ByVal Value As String)
                _ScholarshipESLRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipUnder() As Boolean
            Get
                Return _ScholarshipUnder
            End Get
            Set(ByVal Value As Boolean)
                _ScholarshipUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipUnderRange() As String
            Get
                Return _ScholarshipUnderRange
            End Get
            Set(ByVal Value As String)
                _ScholarshipUnderRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipUnderRangeVN() As String
            Get
                Return _ScholarshipUnderRangeVN
            End Get
            Set(ByVal Value As String)
                _ScholarshipUnderRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipUnderTranfer() As Boolean
            Get
                Return _ScholarshipUnderTranfer
            End Get
            Set(ByVal Value As Boolean)
                _ScholarshipUnderTranfer = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipUnderTranferRange() As String
            Get
                Return _ScholarshipUnderTranferRange
            End Get
            Set(ByVal Value As String)
                _ScholarshipUnderTranferRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipUnderTranferRangeVN() As String
            Get
                Return _ScholarshipUnderTranferRangeVN
            End Get
            Set(ByVal Value As String)
                _ScholarshipUnderTranferRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipGrad() As Boolean
            Get
                Return _ScholarshipGrad
            End Get
            Set(ByVal Value As Boolean)
                _ScholarshipGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipGradRange() As String
            Get
                Return _ScholarshipGradRange
            End Get
            Set(ByVal Value As String)
                _ScholarshipGradRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipGradRangeVN() As String
            Get
                Return _ScholarshipGradRangeVN
            End Get
            Set(ByVal Value As String)
                _ScholarshipGradRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipNote() As String
            Get
                Return _ScholarshipNote
            End Get
            Set(ByVal Value As String)
                _ScholarshipNote = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipNoteVN() As String
            Get
                Return _ScholarshipNoteVN
            End Get
            Set(ByVal Value As String)
                _ScholarshipNoteVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipAss() As Boolean
            Get
                Return _ScholarshipAss
            End Get
            Set(ByVal Value As Boolean)
                _ScholarshipAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipAssRange() As String
            Get
                Return _ScholarshipAssRange
            End Get
            Set(ByVal Value As String)
                _ScholarshipAssRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ScholarshipAssRangeVN() As String
            Get
                Return _ScholarshipAssRangeVN
            End Get
            Set(ByVal Value As String)
                _ScholarshipAssRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialESL() As Boolean
            Get
                Return _OtherfinancialESL
            End Get
            Set(ByVal Value As Boolean)
                _OtherfinancialESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialESLRange() As String
            Get
                Return _OtherfinancialESLRange
            End Get
            Set(ByVal Value As String)
                _OtherfinancialESLRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialESLRangeVN() As String
            Get
                Return _OtherfinancialESLRangeVN
            End Get
            Set(ByVal Value As String)
                _OtherfinancialESLRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialUnder() As Boolean
            Get
                Return _OtherfinancialUnder
            End Get
            Set(ByVal Value As Boolean)
                _OtherfinancialUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialUnderRange() As String
            Get
                Return _OtherfinancialUnderRange
            End Get
            Set(ByVal Value As String)
                _OtherfinancialUnderRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialUnderRangeVN() As String
            Get
                Return _OtherfinancialUnderRangeVN
            End Get
            Set(ByVal Value As String)
                _OtherfinancialUnderRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialUnderTranfer() As Boolean
            Get
                Return _OtherfinancialUnderTranfer
            End Get
            Set(ByVal Value As Boolean)
                _OtherfinancialUnderTranfer = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialUnderTranferRange() As String
            Get
                Return _OtherfinancialUnderTranferRange
            End Get
            Set(ByVal Value As String)
                _OtherfinancialUnderTranferRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialUnderTranferRangeVN() As String
            Get
                Return _OtherfinancialUnderTranferRangeVN
            End Get
            Set(ByVal Value As String)
                _OtherfinancialUnderTranferRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialGrad() As Boolean
            Get
                Return _OtherfinancialGrad
            End Get
            Set(ByVal Value As Boolean)
                _OtherfinancialGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialGradRange() As String
            Get
                Return _OtherfinancialGradRange
            End Get
            Set(ByVal Value As String)
                _OtherfinancialGradRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialGradRangeVN() As String
            Get
                Return _OtherfinancialGradRangeVN
            End Get
            Set(ByVal Value As String)
                _OtherfinancialGradRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialNote() As String
            Get
                Return _OtherfinancialNote
            End Get
            Set(ByVal Value As String)
                _OtherfinancialNote = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialNoteVN() As String
            Get
                Return _OtherfinancialNoteVN
            End Get
            Set(ByVal Value As String)
                _OtherfinancialNoteVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialAss() As Boolean
            Get
                Return _OtherfinancialAss
            End Get
            Set(ByVal Value As Boolean)
                _OtherfinancialAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialAssRange() As String
            Get
                Return _OtherfinancialAssRange
            End Get
            Set(ByVal Value As String)
                _OtherfinancialAssRange = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OtherfinancialAssRangeVN() As String
            Get
                Return _OtherfinancialAssRangeVN
            End Get
            Set(ByVal Value As String)
                _OtherfinancialAssRangeVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOncampusESL() As Boolean
            Get
                Return _HousingOptionOncampusESL
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionOncampusESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOncampusUnder() As Boolean
            Get
                Return _HousingOptionOncampusUnder
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionOncampusUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOncampusGrad() As Boolean
            Get
                Return _HousingOptionOncampusGrad
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionOncampusGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOncampusAss() As Boolean
            Get
                Return _HousingOptionOncampusAss
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionOncampusAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHostFamilyESL() As Boolean
            Get
                Return _HousingOptionHostFamilyESL
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHostFamilyESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHostFamilyUnder() As Boolean
            Get
                Return _HousingOptionHostFamilyUnder
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHostFamilyUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHostFamilyGrad() As Boolean
            Get
                Return _HousingOptionHostFamilyGrad
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHostFamilyGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHostFamilyAss() As Boolean
            Get
                Return _HousingOptionHostFamilyAss
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHostFamilyAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionApertmentESL() As Boolean
            Get
                Return _HousingOptionApertmentESL
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionApertmentESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionApertmentUnder() As Boolean
            Get
                Return _HousingOptionApertmentUnder
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionApertmentUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionApertmentGrad() As Boolean
            Get
                Return _HousingOptionApertmentGrad
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionApertmentGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionApertmentAss() As Boolean
            Get
                Return _HousingOptionApertmentAss
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionApertmentAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHousingESL() As Boolean
            Get
                Return _HousingOptionHousingESL
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHousingESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHousingUnder() As Boolean
            Get
                Return _HousingOptionHousingUnder
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHousingUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHousingGrad() As Boolean
            Get
                Return _HousingOptionHousingGrad
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHousingGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionHousingAss() As Boolean
            Get
                Return _HousingOptionHousingAss
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionHousingAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherESL() As String
            Get
                Return _HousingOptionOtherESL
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherESL = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherESLVN() As String
            Get
                Return _HousingOptionOtherESLVN
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherESLVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherUnder() As String
            Get
                Return _HousingOptionOtherUnder
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherUnder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherUnderVN() As String
            Get
                Return _HousingOptionOtherUnderVN
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherUnderVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherGrad() As String
            Get
                Return _HousingOptionOtherGrad
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherGrad = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherGradVN() As String
            Get
                Return _HousingOptionOtherGradVN
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherGradVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherAss() As String
            Get
                Return _HousingOptionOtherAss
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherAss = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionOtherAssVN() As String
            Get
                Return _HousingOptionOtherAssVN
            End Get
            Set(ByVal Value As String)
                _HousingOptionOtherAssVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HousingOptionRequirecampus() As Boolean
            Get
                Return _HousingOptionRequirecampus
            End Get
            Set(ByVal Value As Boolean)
                _HousingOptionRequirecampus = Value
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
        Public Property Portalid() As Integer
            Get
                Return _Portalid
            End Get
            Set(ByVal Value As Integer)
                _Portalid = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace