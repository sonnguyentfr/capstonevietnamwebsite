Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.School

    Public Class AdmissionBFController
        Public Function Admis_BF_GetByTruongID(ByVal TruongId As Integer) As AdmissionBFInfo
            Return CType(CBO.FillObject(Of AdmissionBFInfo)(DataProvider.Instance.Admis_BF_GetByTruongID(TruongId), True), AdmissionBFInfo)
        End Function
        'Public Function Admis_BF_Insert(ByVal AdmFall As DateTime, ByVal AdmWinter As DateTime, ByVal AdmSpring As DateTime, ByVal AdmSummer As DateTime, ByVal AdmRoll As Boolean, ByVal Gradesfrom As Integer, ByVal Gradesto As Integer, ByVal Top5School As String, ByVal Top5Percen As Integer, ByVal ASScore As String, ByVal APCourse As Boolean, ByVal APCourseList As Integer, ByVal IBCourse As Boolean, ByVal IBCourseList As Integer, ByVal HonorsCourse As Boolean, ByVal HonorsCourseList As Integer, ByVal Linkofweb As String, ByVal Top5Extract As String, ByVal Top5ExtractVN As String, ByVal StandardiziedTest As Boolean, ByVal TESTToefl As Boolean, ByVal TESTToeflMin As Integer, ByVal TESTIELTS As Boolean, ByVal TESTIELTSMin As Integer, ByVal TESTSSAT As Boolean, ByVal TESTSSATMin As Integer, ByVal TESTSLEP As Boolean, ByVal TESTSLEPMin As Integer, ByVal TESTSALTE As Boolean, ByVal TESTSALTEMin As Integer, ByVal TESTOther As String, ByVal EnglishPlacementtest As Boolean, ByVal ESL As Boolean, ByVal NOSTotal As Integer, ByVal NOSInternation As Integer, ByVal NOSVietnames As Integer, ByVal NOSRatio As String, ByVal COSTuti As Integer, ByVal COSBook As Integer, ByVal COSHealth As Integer, ByVal COSRoom As Integer, ByVal COSOther As String, ByVal SummerProgram As Boolean, ByVal SummerProgramAges As String, ByVal SummerProgramDuration As String, ByVal SummerProgramDeadline As String, ByVal SummerProgramCOST As String, ByVal SummerProgramOther As String, ByVal ScholarshipInternation As Boolean, ByVal ScholarshipInternationRang As String, ByVal ScholarshipInternationRangVN As String, ByVal OtherFinancial As Boolean, ByVal OtherFinancialRang As String, ByVal OtherFinancialRangVN As String, ByVal HousingBF As Boolean, ByVal HousingHome As Boolean, ByVal HousingPlacement As Boolean, ByVal Studentrequiredoncampus As Boolean, ByVal Createddate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer) As Integer
        '    Return CType(DataProvider.Instance.Admis_BF_Insert(AdmFall, AdmWinter, AdmSpring, AdmSummer, AdmRoll, Gradesfrom, Gradesto, Top5School, Top5Percen, ASScore, APCourse, APCourseList, IBCourse, IBCourseList, HonorsCourse, HonorsCourseList, Linkofweb, Top5Extract, Top5ExtractVN, StandardiziedTest, TESTToefl, TESTToeflMin, TESTIELTS, TESTIELTSMin, TESTSSAT, TESTSSATMin, TESTSLEP, TESTSLEPMin, TESTSALTE, TESTSALTEMin, TESTOther, EnglishPlacementtest, ESL, NOSTotal, NOSInternation, NOSVietnames, NOSRatio, COSTuti, COSBook, COSHealth, COSRoom, COSOther, SummerProgram, SummerProgramAges, SummerProgramDuration, SummerProgramDeadline, SummerProgramCOST, SummerProgramOther, ScholarshipInternation, ScholarshipInternationRang, ScholarshipInternationRangVN, OtherFinancial, OtherFinancialRang, OtherFinancialRangVN, HousingBF, HousingHome, HousingPlacement, Studentrequiredoncampus, Createddate, UserId, PortalId), Integer)
        'End Function

        ''------------------------------------------'
        'Public Sub Admis_BF_Update(ByVal id As Integer, ByVal AdmFall As DateTime, ByVal AdmWinter As DateTime, ByVal AdmSpring As DateTime, ByVal AdmSummer As DateTime, ByVal AdmRoll As Boolean, ByVal Gradesfrom As Integer, ByVal Gradesto As Integer, ByVal Top5School As String, ByVal Top5Percen As Integer, ByVal ASScore As String, ByVal APCourse As Boolean, ByVal APCourseList As Integer, ByVal IBCourse As Boolean, ByVal IBCourseList As Integer, ByVal HonorsCourse As Boolean, ByVal HonorsCourseList As Integer, ByVal Linkofweb As String, ByVal Top5Extract As String, ByVal Top5ExtractVN As String, ByVal StandardiziedTest As Boolean, ByVal TESTToefl As Boolean, ByVal TESTToeflMin As Decimal, ByVal TESTIELTS As Boolean, ByVal TESTIELTSMin As Integer, ByVal TESTSSAT As Boolean, ByVal TESTSSATMin As Integer, ByVal TESTSLEP As Boolean, ByVal TESTSLEPMin As Integer, ByVal TESTSALTE As Boolean, ByVal TESTSALTEMin As Integer, ByVal TESTOther As String, ByVal EnglishPlacementtest As Boolean, ByVal ESL As Boolean, ByVal NOSTotal As Integer, ByVal NOSInternation As Integer, ByVal NOSVietnames As Integer, ByVal NOSRatio As String, ByVal COSTuti As Integer, ByVal COSBook As Integer, ByVal COSHealth As Integer, ByVal COSRoom As Integer, ByVal COSOther As String, ByVal SummerProgram As Boolean, ByVal SummerProgramAges As String, ByVal SummerProgramDuration As String, ByVal SummerProgramDeadline As String, ByVal SummerProgramCOST As String, ByVal SummerProgramOther As String, ByVal ScholarshipInternation As Boolean, ByVal ScholarshipInternationRang As String, ByVal ScholarshipInternationRangVN As String, ByVal OtherFinancial As Boolean, ByVal OtherFinancialRang As String, ByVal OtherFinancialRangVN As String, ByVal HousingBF As Boolean, ByVal HousingHome As Boolean, ByVal HousingPlacement As Boolean, ByVal Studentrequiredoncampus As Boolean, ByVal Createddate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
        '    DataProvider.Instance.Admis_BF_Update(id, AdmFall, AdmWinter, AdmSpring, AdmSummer, AdmRoll, Gradesfrom, Gradesto, Top5School, Top5Percen, ASScore, APCourse, APCourseList, IBCourse, IBCourseList, HonorsCourse, HonorsCourseList, Linkofweb, Top5Extract, Top5ExtractVN, StandardiziedTest, TESTToefl, TESTToeflMin, TESTIELTS, TESTIELTSMin, TESTSSAT, TESTSSATMin, TESTSLEP, TESTSLEPMin, TESTSALTE, TESTSALTEMin, TESTOther, EnglishPlacementtest, ESL, NOSTotal, NOSInternation, NOSVietnames, NOSRatio, COSTuti, COSBook, COSHealth, COSRoom, COSOther, SummerProgram, SummerProgramAges, SummerProgramDuration, SummerProgramDeadline, SummerProgramCOST, SummerProgramOther, ScholarshipInternation, ScholarshipInternationRang, ScholarshipInternationRangVN, OtherFinancial, OtherFinancialRang, OtherFinancialRangVN, HousingBF, HousingHome, HousingPlacement, Studentrequiredoncampus, Createddate, UserId, PortalId)
        'End Sub
        ''------------------------------------------'
        'Public Sub Admis_BF_UpdateCurrency(ByVal id As Integer, Currency As Integer, ByVal UserId As Integer, ByVal PortalId As Integer)
        '    DataProvider.Instance.Admis_BF_UpdateCurrency(id, Currency, UserId, PortalId)
        'End Sub
        ''------------------------------------------'
        'Public Sub Admis_BF_Delete(ByVal id As Integer)
        '    DataProvider.Instance.Admis_BF_Delete(id)
        'End Sub

        ''------------------------------------------'
        'Public Function Admis_BF_GetByID(ByVal id As Integer) As AdmissionBFInfo
        '    Return CType(CBO.FillObject(DataProvider.Instance.Admis_BF_GetByID(id), GetType(AdmissionBFInfo)), AdmissionBFInfo)
        'End Function

        ''------------------------------------------'
        'Public Function Admis_BF_GetAll() As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance.Admis_BF_GetAll(), GetType(AdmissionBFInfo))
        'End Function

        '------------------------------------------'
    End Class

End Namespace