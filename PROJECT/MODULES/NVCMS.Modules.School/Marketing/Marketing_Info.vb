'******************************************
'Author         :SonNguyen
'Created Date   :3/21/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.Lib.FollowUp
Imports NVCMS.Modules.Lib.LoaiTruong
Imports NVCMS.Modules.LibCRM

Namespace NVCMS.Modules.School
    Public Class MarketingSchoolInfo
        Dim _TruongAdmissionController As New TruongAdmissionController
        Dim _Admission4YearController As New Admission4YearController
        Dim _AdmissionBFController As New AdmissionBFController
        Dim _TruongMajorController As New TruongMajorController
        Dim _LoaiTruongController As New LibSchoolTypeController
        Dim _LocationController As New LibLocationController
        Private _id As Integer
        Private _TruongId As Integer
        Private _CODE As String
        Private _NameofSchool As String
        Private _Tomtat As String
        Private _TomtatEN As String
        Private _DescreptionWebsite As String
        Private _DescreptionWebsiteEN As String
        Private _Address As String
        Private _Logo As String
        Private _Logodesign As String
        Private _LogoLink As String
        Private _Conver As String
        Private _ConverLink As String
        Private _VideoLink As String
        Private _Descreption As String
        Private _DescreptionEN As String
        Private _Namthanhlap As String
        Private _Website As String
        Private _Email As String
        Private _Phone As String
        Private _Social As String
        Private _ThanhPholongannhat As String
        Private _ThanhPholongannhatEN As String
        Private _Vitri As String
        Private _Loaitruongtext As String
        Private _LoaitruongtextEN As String
        Private _Kiemdinh As String
        Private _KiemdinhEN As String
        Private _TypeofRanking As String
        Private _TypeofRankingVN As String
        Private _Loai As Integer
        Private _ProgramOfered As String
        Private _MinimumAgeRequirement As Integer
        Private _MinimumGradeRequirement As Integer
        Private _MinimumGradeRequirementOther As String
        Private _SingleSex As Integer
        Private _Indirect As Integer
        Private _OrganizationId As Integer
        Private _Country As Integer
        Private _StateCity As Integer
        Private _Info As String
        Private _InfoEN As String
        Private _PartnershipStatus As String
        Private _ispartner As Boolean
        Private _Status As Boolean
        Private _CreatedDate As DateTime
        Private _isSubAgent As Boolean
        Private _UserId As Integer

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
        Public Property Truongid() As Integer
            Get
                Return _Truongid
            End Get
            Set(ByVal Value As Integer)
                _Truongid = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property CODE() As String
            Get
                Return _CODE
            End Get
            Set(ByVal Value As String)
                _CODE = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property NameofSchool() As String
            Get
                Return _NameofSchool
            End Get
            Set(ByVal Value As String)
                _NameofSchool = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Tomtat() As String
            Get
                Return _Tomtat
            End Get
            Set(ByVal Value As String)
                _Tomtat = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property TomtatEN() As String
            Get
                Return _TomtatEN
            End Get
            Set(ByVal Value As String)
                _TomtatEN = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property DescreptionWebsite() As String
            Get
                Return _DescreptionWebsite
            End Get
            Set(ByVal Value As String)
                _DescreptionWebsite = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property DescreptionWebsiteEN() As String
            Get
                Return _DescreptionWebsiteEN
            End Get
            Set(ByVal Value As String)
                _DescreptionWebsiteEN = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Address() As String
            Get
                Return _Address
            End Get
            Set(ByVal Value As String)
                _Address = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Logo() As String
            Get
                Return _Logo
            End Get
            Set(ByVal Value As String)
                _Logo = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Logodesign() As String
            Get
                Return _Logodesign
            End Get
            Set(ByVal Value As String)
                _Logodesign = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LogoLink() As String
            Get
                Return _LogoLink
            End Get
            Set(ByVal Value As String)
                _LogoLink = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Conver() As String
            Get
                Return _Conver
            End Get
            Set(ByVal Value As String)
                _Conver = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ConverLink() As String
            Get
                Return _ConverLink
            End Get
            Set(ByVal Value As String)
                _ConverLink = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property VideoLink() As String
            Get
                Return _VideoLink
            End Get
            Set(ByVal Value As String)
                _VideoLink = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Descreption() As String
            Get
                Return _Descreption
            End Get
            Set(ByVal Value As String)
                _Descreption = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property DescreptionEN() As String
            Get
                Return _DescreptionEN
            End Get
            Set(ByVal Value As String)
                _DescreptionEN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Namthanhlap() As String
            Get
                Return _Namthanhlap
            End Get
            Set(ByVal Value As String)
                _Namthanhlap = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Website() As String
            Get
                Return _Website
            End Get
            Set(ByVal Value As String)
                _Website = Value
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
        Public Property Phone() As String
            Get
                Return _Phone
            End Get
            Set(ByVal Value As String)
                _Phone = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Social() As String
            Get
                Return _Social
            End Get
            Set(ByVal Value As String)
                _Social = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ThanhPholongannhat() As String
            Get
                Return _ThanhPholongannhat
            End Get
            Set(ByVal Value As String)
                _ThanhPholongannhat = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ThanhPholongannhatEN() As String
            Get
                Return _ThanhPholongannhatEN
            End Get
            Set(ByVal Value As String)
                _ThanhPholongannhatEN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Vitri() As String
            Get
                Return _Vitri
            End Get
            Set(ByVal Value As String)
                _Vitri = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Loaitruongtext() As String
            Get
                Return _Loaitruongtext
            End Get
            Set(ByVal Value As String)
                _Loaitruongtext = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LoaitruongtextEN() As String
            Get
                Return _LoaitruongtextEN
            End Get
            Set(ByVal Value As String)
                _LoaitruongtextEN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Kiemdinh() As String
            Get
                Return _Kiemdinh
            End Get
            Set(ByVal Value As String)
                _Kiemdinh = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property KiemdinhEN() As String
            Get
                Return _KiemdinhEN
            End Get
            Set(ByVal Value As String)
                _KiemdinhEN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TypeofRanking() As String
            Get
                Return _TypeofRanking
            End Get
            Set(ByVal Value As String)
                _TypeofRanking = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TypeofRankingVN() As String
            Get
                Return _TypeofRankingVN
            End Get
            Set(ByVal Value As String)
                _TypeofRankingVN = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Loai() As Integer
            Get
                Return _Loai
            End Get
            Set(ByVal Value As Integer)
                _Loai = Value
            End Set
        End Property
        Public ReadOnly Property LoaiName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "LoaiName:" & Loai
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim objTruongAd As LibSchoolTypeInfo = _LoaiTruongController.Cap_Loaitruong_GetByID(Loai, 50)
                    If Not objTruongAd Is Nothing Then
                        With objTruongAd
                            strResult = .Loaitruong
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public Property ProgramOfered() As String
            Get
                Return _ProgramOfered
            End Get
            Set(ByVal Value As String)
                _ProgramOfered = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property MinimumAgeRequirement() As Integer
            Get
                Return _MinimumAgeRequirement
            End Get
            Set(ByVal Value As Integer)
                _MinimumAgeRequirement = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property MinimumGradeRequirement() As Integer
            Get
                Return _MinimumGradeRequirement
            End Get
            Set(ByVal Value As Integer)
                _MinimumGradeRequirement = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property MinimumGradeRequirementOther() As String
            Get
                Return _MinimumGradeRequirementOther
            End Get
            Set(ByVal Value As String)
                _MinimumGradeRequirementOther = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property SingleSex() As Integer
            Get
                Return _SingleSex
            End Get
            Set(ByVal Value As Integer)
                _SingleSex = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Indirect() As Integer
            Get
                Return _Indirect
            End Get
            Set(ByVal Value As Integer)
                _Indirect = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OrganizationId() As Integer
            Get
                Return _OrganizationId
            End Get
            Set(ByVal Value As Integer)
                _OrganizationId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Country() As Integer
            Get
                Return _Country
            End Get
            Set(ByVal Value As Integer)
                _Country = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property StateCity() As Integer
            Get
                Return _StateCity
            End Get
            Set(ByVal Value As Integer)
                _StateCity = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Info() As String
            Get
                Return _Info
            End Get
            Set(ByVal Value As String)
                _Info = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property InfoEN() As String
            Get
                Return _InfoEN
            End Get
            Set(ByVal Value As String)
                _InfoEN = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property PartnershipStatus() As String
            Get
                Return _PartnershipStatus
            End Get
            Set(ByVal Value As String)
                _PartnershipStatus = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ispartner() As Boolean
            Get
                Return _ispartner
            End Get
            Set(ByVal Value As Boolean)
                _ispartner = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Status() As Boolean
            Get
                Return _Status
            End Get
            Set(ByVal Value As Boolean)
                _Status = Value
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
        Public Property isSubAgent() As Boolean
            Get
                Return _isSubAgent
            End Get
            Set(ByVal Value As Boolean)
                _isSubAgent = Value
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
        ''' <summary>
        ''' Tên Quốc Gia
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CountryName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CountryName:" & Country
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim objTruongAd As LibLocationInfo = _LocationController.Location_GetByID(Country, 0)
                    If Not objTruongAd Is Nothing Then
                        With objTruongAd
                            strResult = .Name
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Quốc Gia Cờ
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CountryFlag() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CountryFlag:" & Country
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim objTruongAd As LibLocationInfo = _LocationController.Location_GetByID(Country, 0)
                    If Not objTruongAd Is Nothing Then
                        With objTruongAd
                            strResult = "" '.Flag
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Tên Bang
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property StateCityName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "StateCity:" & StateCity
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim objTruongAd As LibLocationInfo = _LocationController.Location_GetByID(StateCity, 0)
                    If Not objTruongAd Is Nothing Then
                        With objTruongAd
                            strResult = .Name
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        Public ReadOnly Property LoaiTruongTen() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "LoaiTruongTen:" & Loai
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim objTruongAd As LibSchoolTypeInfo = _LoaiTruongController.Cap_Loaitruong_GetByID(Loai, 50)
                    If Not objTruongAd Is Nothing Then
                        With objTruongAd
                            strResult = .Loaitruong
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG  - Hạn nộp hồ sơ
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CaoDangHanNopHoSo() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CaoDangHanNopHoSo:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim objTruongAd As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not objTruongAd Is Nothing Then
                        With objTruongAd
                            Dim sltrhannophosoCaoDang As String = ""
                            If .RollingAss = True Then
                                sltrhannophosoCaoDang += "<li>Quanh năm</li>"
                            Else
                                If .FallAss = "01/01/2000" Then
                                Else
                                    sltrhannophosoCaoDang += "<li>Kỳ Thu (" & .FallAss.ToString("dd/MM") & ")</li>"
                                End If
                                If .WinterAss = "01/01/2000" Then
                                Else
                                    sltrhannophosoCaoDang += "<li>Kỳ Đông (" & .WinterAss.ToString("dd/MM") & ")</li>"
                                End If
                                If .SpringAss = "01/01/2000" Then
                                Else
                                    sltrhannophosoCaoDang += "<li>Kỳ Xuân (" & .SpringAss.ToString("dd/MM") & ")</li>"
                                End If
                                If .SummerAss = "01/01/2000" Then
                                Else
                                    sltrhannophosoCaoDang += "<li>Kỳ Hè (" & .SummerAss.ToString("dd/MM") & ")</li>"
                                End If
                            End If
                            strResult = sltrhannophosoCaoDang
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        ''' <summary>
        ''' CAO ĐẲNG -Chi phí Học phí
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSTuitionfeeAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSTuitionfeeAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSTuitionfeeAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property

        ''' <summary>
        ''' CAO ĐẲNG - Chi Phí Sách vở
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSBooksuppliesAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSBooksuppliesAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then

                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSBooksuppliesAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG - Chi Phí Bảo Hiểm
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSHealthAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSHealthAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSHealthAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG - Chi Phí Ăn ở
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSRoomAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSRoomAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSRoomAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG - Chi Phí Đi lại
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSTransportAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSTransportAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSTransportAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG - Chi Phí Khác
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSOtherAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSOtherAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSOtherAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG - Tính số ngành học
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AssMajorCount() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "AssMajorCount:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    strResult = _TruongMajorController.TruongMajor_GetCountAllByTruong(id).ToString()
                    DataCache.SetCache(strCacheKey, strResult)
                End If
                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG - Sỉ số lớp học
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NOSStudentFacultyRatioAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NOSStudentFacultyRatioAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .NOSStudentFacultyRatioAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG -  Tổng số học sinh
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NOSTotalAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NOSTotalAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .NOSTotalAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG - Điểm IELTS
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IELTSAss() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "IELTSAss:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .IELTSAss
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học Yêu Cầu điểm chuẩn hóa tối thiểu
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CaoDangYeuCauDiemChuanHoaToiThieu() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CaoDangYeuCauDiemChuanHoaToiThieu:" & id
                Dim strResult As String = ""
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            If .ToeflCBTAss <> "" Then
                                strResult += "<li>TOEFL CBT: " & .ToeflCBTAss & "</li>"
                            End If
                            If .IELTSAss <> "" Then
                                strResult += "<li>IELTS: " & .IELTSAss & "</li>"
                            End If
                            If .iTEPAss <> "" Then
                                strResult += "<li>iTEP: " & .iTEPAss & "</li>"
                            End If
                            If .OtherAss <> "" Then
                                strResult += "<li>Khác: " & .OtherAss & "</li>"
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG Yêu Cầu Tiếng Anh
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CaoDangYeuCauTiengAnh() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CaoDangYeuCauTiengAnh:" & id
                Dim strResult As String = ""
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            If .ToefliBTUnder <> "" Then
                                strResult += "<li>TOEFL iBT: " & .ToefliBTUnder & "</li>"
                            End If
                            If .IELTSUnder <> "" Then
                                strResult += "<li>IELTS: " & .IELTSUnder & "</li>"
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' CAO ĐẲNG Học bổng
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CaoDangHocBong() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CaoDangHocBong:" & id
                Dim strResult As String = ""
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            If .ScholarshipAss Then
                                strResult += "<li>Có | " & .ScholarshipAssRangeVN & "</li>"
                            Else
                                strResult += "<li>Không</li>"
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học - Chi Phí Học phí
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSTuitionfeeUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSTuitionfeeUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSTuitionfeeUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học - Tính số ngành học
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property UnderMajorCount() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "UnderMajorCount:" & id
                Dim strResult As String = String.Empty
                Dim icount As Integer = 0
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    strResult = _TruongMajorController.TruongMajor_GetCountAllByTruong(id).ToString()
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại hoc - Sỉ số lớp 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NOSStudentFacultyRatioUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NOSStudentFacultyRatioUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .NOSStudentFacultyRatioUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại hoc - Tổng số học sinh
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NOSTotalUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NOSTotalUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .NOSTotalUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        '''Đại học - Điểm IELTS
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IELTSUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "IELTSUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .IELTSUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        '''Đại học - Hạn nộp hồ sơ
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DaihocHanNopHoSo() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "DaihocHanNopHoSo:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            Dim sltrhannophosoDaihoc As String = ""
                            If .RollingUnder = True Then
                                sltrhannophosoDaihoc += "<li>Quanh năm</li>"
                            Else
                                If .FallUnder = "01/01/2000" Then
                                Else
                                    sltrhannophosoDaihoc += "<li>Kỳ Thu (" & .FallUnder.ToString("dd/MM") & ")</li>"
                                End If
                                If .WinterUnder = "01/01/2000" Then
                                Else
                                    sltrhannophosoDaihoc += "<li>Kỳ Đông (" & .WinterUnder.ToString("dd/MM") & ")</li>"
                                End If
                                If .SpringUnder = "01/01/2000" Then
                                Else
                                    sltrhannophosoDaihoc += "<li>Kỳ Xuân (" & .SpringUnder.ToString("dd/MM") & ")</li>"
                                End If
                                If .SummerUnder = "01/01/2000" Then
                                Else
                                    sltrhannophosoDaihoc += "<li>Kỳ Hè (" & .SummerUnder.ToString("dd/MM") & ")</li>"
                                End If
                            End If
                            strResult = sltrhannophosoDaihoc
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học Yêu Cầu điểm chuẩn hóa tối thiểu
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DaihocYeuCauDiemChuanHoaToiThieu() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "DaihocYeuCauDiemChuanHoaToiThieu:" & id
                Dim strResult As String = ""
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja

                            If .SAT1Under <> "" Then
                                strResult += "<li>SAT I: " & .SAT1Under & "</li>"
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học - Chi Phí Sách vở
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSBooksuppliesUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSBooksuppliesUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSBooksuppliesUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học - Chi Phí Bảo Hiểm
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSHealthUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSHealthUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSHealthUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học - Chi Phí Ăn ở
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSRoomUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSRoomUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSRoomUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học - Chi Phí Đi lại
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSTransportUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSTransportUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSTransportUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học - Chi Phí Khác
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property COSOtherUnder() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "COSOtherUnder:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .COSOtherUnder
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Đại học Yêu Cầu Tiếng Anh
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DaihocYeuCauTiengAnh() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "DaihocYeuCauTiengAnh:" & id
                Dim strResult As String = ""
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            If .ToefliBTUnder <> "" Then
                                strResult += "<li>TOEFL iBT: " & .ToefliBTUnder & "</li>"
                            End If
                            If .IELTSUnder <> "" Then
                                strResult += "<li>IELTS: " & .IELTSUnder & "</li>"
                            End If
                            If .OtherUnder <> "" Then
                                strResult += "<li>Khác: " & .OtherUnder & "</li>"
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        '''Đại học Học bổng
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DaiHocHocBong() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "DaiHocHocBong:" & id
                Dim strResult As String = ""
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            If .ScholarshipUnder Then
                                strResult += "<li>Có | " & .ScholarshipUnderRangeVN & "</li>"
                            Else
                                strResult += "<li>Không</li>"
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' Chung Học bổng
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TopMostMajor() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TopMostMajor:" & id
                Dim strResult As String = ""
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As Admission4YearInfo = _Admission4YearController.Admis_4Year_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            If Not String.IsNullOrEmpty(.MostMajor) Then
                                Dim MostMajor As String() = .MostMajor.Split(CType(",", Char))
                                For i As Integer = 0 To MostMajor.Length - 1
                                    strResult += "<li>" & MostMajor(i) & "</li>"
                                Next
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC  - Hạn nộp hồ sơ
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TrungHocHanNopHoSo() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TrungHocHanNopHoSo:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then

                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            Dim sltrhannophosoTrungHoc As String = ""
                            If .AdmRoll = True Then
                                sltrhannophosoTrungHoc += "<li>Quanh năm</li>"
                            Else
                                If .AdmFall = "01/01/2000" Then
                                Else
                                    sltrhannophosoTrungHoc += "<li>Kỳ Thu (" & .AdmFall.ToString("dd/MM") & ")</li>"
                                End If
                                If .AdmWinter = "01/01/2000" Then
                                Else
                                    sltrhannophosoTrungHoc += "<li>Kỳ Đông (" & .AdmWinter.ToString("dd/MM") & ")</li>"
                                End If
                                If .AdmSpring = "01/01/2000" Then
                                Else
                                    sltrhannophosoTrungHoc += "<li>Kỳ Xuân (" & .AdmSpring.ToString("dd/MM") & ")</li>"
                                End If
                                If .AdmSummer = "01/01/2000" Then
                                Else
                                    sltrhannophosoTrungHoc += "<li>Kỳ Hè (" & .AdmSummer.ToString("dd/MM") & ")</li>"
                                End If
                            End If
                            strResult = sltrhannophosoTrungHoc
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Yêu Cầu Tiếng Anh
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TrungHocYeuCauTiengAnh() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TrungHocYeuCauTiengAnh:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            Dim sltrhannophosoTrungHoc As String = ""
                            If .StandardiziedTest Then
                                sltrhannophosoTrungHoc += "<li>Kiểm tra chuẩn hóa: Có</li>"
                            End If
                            If .TESTToefl Then
                                sltrhannophosoTrungHoc += "<li>Toefl: " & .TESTToeflMin & "</li>"
                            End If
                            If .TESTIELTS Then
                                sltrhannophosoTrungHoc += "<li>IELTS: " & .TESTIELTSMin & "</li>"
                            End If
                            strResult = sltrhannophosoTrungHoc
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Học Bổng
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TrungHocHocbong() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TrungHocHocbong:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            Dim sltrhannophosoTrungHoc As String = ""
                            If .ScholarshipInternation Then
                                sltrhannophosoTrungHoc += "<li>" & .ScholarshipInternationRangVN & "</li>"
                            Else
                                sltrhannophosoTrungHoc += "<li>Không</li>"
                            End If

                            strResult = sltrhannophosoTrungHoc
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Chi Phí
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TrungHocChiPhi() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TrungHocChiPhi:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult += "<li>Chi phí: " & .COSTuti & "</li>"
                            strResult += "<li>Sách vở: " & .COSBook & "</li>"
                            strResult += "<li>Bảo hiểm: " & .COSHealth & "</li>"
                            strResult += "<li>Nhà ở sinh hoạt: " & .COSRoom & "</li>"
                            strResult += "<li>Khác: " & .COSOther & "</li>"
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Top Trường ĐH
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TruongHocToptruongDH() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TruongHocToptruongDH:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            If Not String.IsNullOrEmpty(.Top5School) Then
                                Dim Top5School As String() = .Top5School.Split(CType(",", Char))
                                For i As Integer = 0 To Top5School.Length - 1
                                    strResult += "<li>" & Top5School(i) & "</li>"
                                Next
                            End If
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Tổng chi phí
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TruongHocTongChiPhi() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TruongHocTongChiPhi:" & id
                Dim strResult As String = "0"
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "0" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            Dim cosOtherValue As String = If(IsNumeric(.COSOther), .COSOther, "0")
                            strResult = .COSTuti + .COSBook + .COSHealth + .COSRoom + cosOtherValue
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Điểm trung binh SAT
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TruongHocDiemTrungBinhSAT() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TruongHocDiemTrungBinhSAT:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .ASScore
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Sỉ số lớp học
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TruongHocSiSoLopHop() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TruongHocSiSoLopHop:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .NOSRatio
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        ''' <summary>
        ''' TRUNG HỌC Tổng số học sinh
        ''' </summary>
        ''' <returns></returns>
        ''' 
        Public ReadOnly Property TruongHocTongSoHocSinh() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TruongHocTongSoHocSinh:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obja As AdmissionBFInfo = _AdmissionBFController.Admis_BF_GetByTruongID(id)
                    If Not obja Is Nothing Then
                        With obja
                            strResult = .NOSTotal
                            DataCache.SetCache(strCacheKey, strResult)
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
    End Class
End Namespace