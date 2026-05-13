
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke
Imports System.Text

Namespace NVCMS.Modules.LibCRM

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' SQL Server implementation of the abstract DataProvider class
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class SqlDataProvider

        Inherits DataProvider

#Region "Private Members"

        Private Const ProviderType As String = "data"
        Private Const ModuleQualifier As String = "NVPortal_"

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            ' Read the attributes for this provider
            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString("SiteSqlServerV1")

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("SiteSqlServerV1")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region "Private Methods"

        Private Function GetFullyQualifiedName(ByVal name As String) As String
            Return DatabaseOwner & ObjectQualifier & ModuleQualifier & name
        End Function

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#End Region
#Region "Common Function"
        Public Function GetSqlTypeString(ByVal keyword As String) As SqlTypes.SqlString
            Dim _keywords As String = String.Empty
            If keyword <> Null.NullString AndAlso keyword <> String.Empty Then
                _keywords = New SqlTypes.SqlString(FullTextSearchFormat(keyword))
            End If
            Return _keywords
        End Function
        Public Function FullTextSearchFormat(ByVal keywords As String) As String
            If keywords Is Nothing OrElse keywords = String.Empty Then
                Return String.Empty
            End If

            Dim sbKeyWordsFilter As New StringBuilder()
            Dim splitedKeyWords As String() = keywords.Trim().Split(" "c)
            For i As Integer = 0 To splitedKeyWords.Length - 1
                'The first key words
                sbKeyWordsFilter.Append("""")
                sbKeyWordsFilter.Append(splitedKeyWords(i))
                sbKeyWordsFilter.Append("*"" & ")
            Next

            'The last key word
            sbKeyWordsFilter.Append("""")
            sbKeyWordsFilter.Append(splitedKeyWords(splitedKeyWords.Length - 1))
            sbKeyWordsFilter.Append("*""")

            Return sbKeyWordsFilter.ToString()
        End Function
        Public Function WrapWordFullText(word As String) As String
            If String.IsNullOrEmpty(word) Then
                Return String.Empty
            Else
                Return """" & word & "*"""
            End If
        End Function
#End Region
#Region "Public Methods"
#Region "DM_Location"

        Public Overrides Function Location_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Location_SelectByID", id, PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function Location_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Location_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Location_SelectByParentId(Parentid As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Location_SelectByParentId", Parentid, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub Location_Insert(ByVal Name As String, ByVal ShortName As String, currency As String, currencyName As String, currencycode As String, PostCode As String, ByVal ParentId As Integer, ByVal Status As Boolean, Ordernumber As Integer, ByVal mapLatitude As String, ByVal mapLongitude As String, ByVal Info As String, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Location_Insert", Name, ShortName, currency, currencyName, currencycode, PostCode, ParentId, Status, Ordernumber, mapLatitude, mapLongitude, Info, PortalId, CreatedDate)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Location_Update(ByVal id As Integer, ByVal Name As String, ByVal ShortName As String, currency As String, currencyName As String, currencycode As String, PostCode As String, ByVal ParentId As Integer, ByVal Status As Boolean, Ordernumber As Integer, ByVal mapLatitude As String, ByVal mapLongitude As String, ByVal Info As String, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Location_Update", id, Name, ShortName, currency, currencyName, currencycode, PostCode, ParentId, Status, Ordernumber, mapLatitude, mapLongitude, Info, PortalId, CreatedDate)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Location_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Location_Update", id, Ordernumber, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Location_Delete(ByVal id As Integer, PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Location_Delete", id, PortalId)
        End Sub
        '------------------------------------------'


#End Region
#Region "Student_Info"

        Public Overrides Function _Info_Insert(VP As Integer, type As Integer, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal FollowPhuongThuc As Integer, ByVal FollowKetQua As Integer, ByVal FollowNoiDung As String, ByVal FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_Insert", VP, type, Hotendem, Ten, Sex, Ngaysinh, kieungaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5, PermissionUser, FollowPhuongThuc, FollowKetQua, FollowNoiDung, FollowUpStatus, FollowUpDateUpdate, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate, CreatedDate, UserId, PortalId, Xoa), Integer)
        End Function
        '------------------------------------------'
        Public Overrides Sub _Info_InsertCode(id As Integer, Code As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_InsertCode", id, Code)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_InsertHinhThuc(id As Integer, HinhThuc As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_InsertHinhThuc", id, HinhThuc)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_InsertExcel(code As String, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_InsertExcel", code, Hotendem, Ten, Sex, Ngaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5, PermissionUser, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate, CreatedDate, UserId, PortalId, Xoa)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_Update(ByVal id As Integer, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal FollowPhuongThuc As Integer, ByVal FollowKetQua As Integer, ByVal FollowNoiDung As String, ByVal FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal BoFullName As String, ByVal BoSodienthoai As String, ByVal BoEmail As String, ByVal BoNghenghiep As String, ByVal BoEditUserId As Integer, ByVal BoEditDate As DateTime, ByVal BoApproveUserId As Integer, ByVal BoApproveDate As DateTime, ByVal MeFullName As String, ByVal MeSodienthoai As String, ByVal MeEmail As String, ByVal MeNghenghiep As String, ByVal MeEditUserId As Integer, ByVal MeEditDate As DateTime, ByVal MeApproveUserId As Integer, ByVal MeApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_Update", id, Hotendem, Ten, Sex, Ngaysinh, kieungaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5, PermissionUser, FollowPhuongThuc, FollowKetQua, FollowNoiDung, FollowUpStatus, FollowUpDateUpdate, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate, BoFullName, BoSodienthoai, BoEmail, BoNghenghiep, BoEditUserId, BoEditDate, BoApproveUserId, BoApproveDate, MeFullName, MeSodienthoai, MeEmail, MeNghenghiep, MeEditUserId, MeEditDate, MeApproveUserId, MeApproveDate, CreatedDate, UserId, PortalId, Xoa)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateInfo(ByVal id As Integer, ByVal Hotendem As String, ByVal Ten As String, Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateInfo", id, Hotendem, Ten, Sex, Ngaysinh, kieungaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateVanPhong(ByVal id As Integer, vp As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateVanPhong", id, vp)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateKyhopdong(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateKyhopdong", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateTuVan(ByVal id As Integer, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateTuVan", id, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateHocVan(ByVal id As Integer, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateHocVan", id, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateFollowUp(ByVal id As Integer, FollowPhuongThuc As Integer, FollowKetQua As Integer, FollowNoidung As String, FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateFollowUp", id, FollowPhuongThuc, FollowKetQua, FollowNoidung, FollowUpStatus, FollowUpDateUpdate, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateNhom(ByVal id As Integer, Nhom As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateNhom", id, Nhom)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdatePermissionUser(ByVal id As Integer, AdviserId As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdatePermissionUser", id, AdviserId, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateSupporterUser(ByVal id As Integer, PermissionUser As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateSupporterUser", id, PermissionUser, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateSpy(ByVal id As Integer, isSpy As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateSpy", id, isSpy)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateDongyguithongtin(ByVal id As Integer, dongyguithongtin As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateDongyguithongtin", id, dongyguithongtin)
        End Sub
        '------------------------------------------'
        Public Overrides Sub _Info_UpdateXoa(ByVal id As Integer, Xoa As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Info_UpdateXoa", id, Xoa)
        End Sub
        '------------------------------------------'
        Public Overrides Function _Info_GetAllSdt() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectSDT")
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectAll")
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectByID", id)
        End Function

        Public Overrides Function _Info_GetByCode(ByVal code As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectByCode", code)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetBySearch(ByVal key As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectBySearch", key)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetByEmail(ByVal Email As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectByEmail", Email)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetTrungEmail() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectTrungEmail")
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetBySodienthoai(ByVal Sodienthoai As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectBySodienthoai", Sodienthoai)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetTrungSodienthoai() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Info_SelectTrungSoDienThoai")
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetByIDEmailExit(ByVal Email As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_SelectByEmailExit", Email)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_GetByIDSDTExit(ByVal sodienthoai As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_SelectBySDTExit", sodienthoai)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_Find_Count(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_Find_Count", subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_Find_Index(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Info_Find_Index", subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_Campaign_Find_Count(subtractIds As String, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_Campaign_Find_Count", subtractIds, Vp, fullname, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, Sukien, EventCatId, EventId, Checkin, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_Campaign_Find_Index(subtractIds As String, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Info_Campaign_Find_Index", subtractIds, Vp, fullname, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, Sukien, EventCatId, EventId, Checkin, Portalid, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        '------------------------------------------'
        Public Overrides Function _Info_MarketingFind_Count(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, isspy As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_MarketingInfo_Find_Count", subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, isspy, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_MarketingFind_Index(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, isspy As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_MarketingInfo_Find_Index", subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, isspy, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_FindPermissionUser_Count(ByVal AdviserId As Integer, vp As Integer, Tinh As Integer, hinhthuc As Integer, sukien As Integer, trangthai As Integer, Email As String, sodienthoai As String, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_FindPermission_Count", AdviserId, vp, Tinh, hinhthuc, sukien, trangthai, Email, sodienthoai, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_FindPermissionUser_Index(ByVal AdviserId As Integer, vp As Integer, Tinh As Integer, hinhthuc As Integer, sukien As Integer, trangthai As Integer, Email As String, Sodienthoai As String, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Info_FindPermission_Index", AdviserId, vp, Tinh, hinhthuc, sukien, trangthai, Email, Sodienthoai, Portalid, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_UserFind_Count(ByVal fullname As String, code As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Quocgia As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, hinhthuc As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_UserFind_Count", fullname, code, vp, Email, Sodienthoai, Bachoc, Quocgia, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, hinhthuc, EventCatId, EventId, Checkin, UserId, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_UserFind_Index(control As Integer, ByVal fullname As String, code As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Quocgia As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, hinhthuc As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Info_UserFind_Index", control, fullname, code, vp, Email, Sodienthoai, Bachoc, Quocgia, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, hinhthuc, EventCatId, EventId, Checkin, UserId, Portalid, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_UserFollowFind_Count(ByVal fullname As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinhfrom As Integer, Namsinhto As Integer, hinhthuc As Integer, phuongthuc As Integer, EventCatId As Integer, datefrom As DateTime, dateto As DateTime, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_UserFollowFind_Count", fullname, vp, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinhfrom, Namsinhto, hinhthuc, phuongthuc, EventCatId, datefrom, dateto, Checkin, UserId, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_UserFollowFind_Index(control As Integer, ByVal fullname As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinhfrom As Integer, Namsinhto As Integer, hinhthuc As Integer, phuongthuc As Integer, EventCatId As Integer, datefrom As DateTime, dateto As DateTime, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Info_UserFollowFind_Index", control, fullname, vp, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinhfrom, Namsinhto, hinhthuc, phuongthuc, EventCatId, datefrom, dateto, Checkin, UserId, Portalid, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_Static_Count(Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_Static_Count", Bachoc, Status, sex, Khanangchitra, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_Checin_School(EventCatId As Integer, EventId As Integer, StudentId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Info_Checin_School", EventCatId, EventId, StudentId), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_TelesaleCount(datetime As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Static_UserTelesaleCount", datetime, phuongthuctiepcan, Status, UserId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_TelesaleCountTyle(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Static_UserTelesaleCountTyle", datefrom, dateto, phuongthuctiepcan, Status, UserId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_TelesaleCountTyle_Index(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, UserId As Integer, PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Static_UserTelesaleCountTyle_Index", datefrom, dateto, phuongthuctiepcan, UserId, PortalId), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_UserTelesaleKhachHang(StudentId As Integer, datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, status As Integer, UserId As Integer, PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Static_UserTelesaleKhachHang", StudentId, datefrom, dateto, phuongthuctiepcan, status, UserId, PortalId), IDataReader)
        End Function







        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_UserTelesaleGroupKhachHang(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer, PageIndex As Integer, PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Static_UserTelesaleGroupKhachHang", datefrom, dateto, phuongthuctiepcan, status, UserId, nguon, PortalId, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_UserTelesaleGroupKhachHang_CountCuocGoi(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Static_UserTelesaleGroupKhachHang_CountCuocGoi", datefrom, dateto, phuongthuctiepcan, status, UserId, nguon, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_UserTelesaleGroupKhachHang_CountKhachHang(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Static_UserTelesaleGroupKhachHang_CountKhachHang", datefrom, dateto, phuongthuctiepcan, status, UserId, nguon, PortalId)
        End Function
        '------------------------------------------'










        Public Overrides Function _Info_StaticUser_TrangThaiCountTyle(datefrom As DateTime, dateto As DateTime, Status As Integer, UserId As Integer, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Static_UserTrangThaiCountTyle", datefrom, dateto, Status, UserId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_TrangThaiCountTyle_Index(datefrom As DateTime, dateto As DateTime, Status As Integer, UserId As Integer, PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Static_UserTrangThaiCountTyle_Index", datefrom, dateto, Status, UserId, PortalId), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_Permission(datefrom As DateTime, dateto As DateTime, UserId As Integer, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Static_UserPermission", datefrom, dateto, UserId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function _Info_StaticUser_Permission_New(datefrom As DateTime, dateto As DateTime, UserId As Integer, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Static_UserPermission_New", datefrom, dateto, UserId, PortalId)
        End Function
        '------------------------------------------'
#End Region
#Region "Events_Cat"
        Public Overrides Function Events_Cat_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectByID", id, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetByTabID(ByVal tabid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_GetByTabID", tabid)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShow(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAllShow", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShowPastCount(ByVal CatName As String, Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Cat_SelectAllPast_Count", CatName, GetSqlTypeString(CatName), Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShowPast(ByVal CatName As String, Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAllPast", CatName, GetSqlTypeString(CatName), Portalid, PageIndex, PageSize)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShowOnline(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAllOnline", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Cat_Insert(ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, code As String, Source As String, Email As String, ByVal DateShow As String, FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, TabId As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_Insert", CatName, CatNameEN, Marketing, chonnhieu, code, Source, Email, DateShow, FromDate, EndDate, Avatar, Desception, DesceptionEN, Contentx, ContentxEN, ContentMail, CreatedDate, UserId, PortalId, Isactive, Ordernumber, TabId, sendmail, sendCode, titleMail)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Events_Cat_Update(ByVal id As Integer, ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, Source As String, Email As String, ByVal DateShow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, TabId As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_Update", id, CatName, CatNameEN, Marketing, chonnhieu, Source, Email, DateShow, FromDate, EndDate, Avatar, Desception, DesceptionEN, Contentx, ContentxEN, ContentMail, CreatedDate, UserId, PortalId, Isactive, Ordernumber, TabId, sendmail, sendCode, titleMail)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateOrdernumber", id, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairSchool(ByVal id As Integer, Portalid As Integer, FairSchool As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairSchool", id, Portalid, FairSchool)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairOrg(ByVal id As Integer, Portalid As Integer, FairOrg As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairOrg", id, Portalid, FairOrg)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairDiengia(ByVal id As Integer, Portalid As Integer, FairDiengia As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairDiengia", id, Portalid, FairDiengia)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairTestimonial(ByVal id As Integer, Portalid As Integer, FairTestimonial As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairTestimonial", id, Portalid, FairTestimonial)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairDonviTaiTro(ByVal id As Integer, Portalid As Integer, FairDonviTaiTro As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairDonviTaiTro", id, Portalid, FairDonviTaiTro)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_Delete(ByVal id As Integer, PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_Delete", id, PortalId)
        End Sub

        '------------------------------------------'


#End Region
#Region "NV_Events"

        Public Overrides Function Events_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectByID", id, PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function Events_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllByCat(CatId As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllByCat", CatId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllShowByCat(CatId As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllShowByCat", CatId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Insert(ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, Org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Insert", Title, TitleEN, CODE, Source, Vanphong, CatId, Avatar, diadiem, diadiemEN, fromdatetime, enddatetime, thanhphan, thanhphanEN, School, Org, Gia, Descreption, DescreptionEN, LienheName, LienheEmail, LienheMobile, LienheAdd, UserId, Portalid, Createddate, Isactive, anhbando, linkbando, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Update(ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, Org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Update", id, Title, TitleEN, CODE, Source, Vanphong, CatId, Avatar, diadiem, diadiemEN, fromdatetime, enddatetime, thanhphan, thanhphanEN, School, Org, Gia, Descreption, DescreptionEN, LienheName, LienheEmail, LienheMobile, LienheAdd, UserId, Portalid, Createddate, Isactive, anhbando, linkbando, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Delete(ByVal id As Integer, PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Delete", id, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Events_Find_Count(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Find_Count", subtractIds, datefrom, dateto, title, CatId, PortalId, Isactive, UserId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Find_Index(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Find_Index", subtractIds, datefrom, dateto, title, CatId, PortalId, Isactive, UserId, PageIndex, PageSize), IDataReader)
        End Function
        Public Overrides Function Events_FindShow_Count(ByVal subtractIds As String, PortalId As Integer, ByVal Isactive As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_FindShow_Count", subtractIds, PortalId, Isactive)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_FindShow_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_FindShow_Index", subtractIds, PortalId, Isactive, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_FindShowPast_Count(ByVal subtractIds As String, PortalId As Integer, ByVal Isactive As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_FindShowPast_Count", subtractIds, PortalId, Isactive)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_FindShowPast_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_FindShowPast_Index", subtractIds, PortalId, Isactive, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllShow(PortalId As Integer, Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllShow", PortalId, Count)
        End Function
        Public Overrides Function Events_GetAllShowEnd(PortalId As Integer, Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllShowEnd", PortalId, Count)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllOnline(PortalId As Integer, Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllOnline", PortalId, Count)
        End Function
#End Region
#Region "NV_Events_Student"

        Public Overrides Function Events_Student_GetAllByEvent(ByVal EventId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEvent", EventId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetAllByStudent(ByVal StudentId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByStudent", StudentId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetAllByEventCheckIn(ByVal EventId As Integer, Source As String, checkin As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCheckIn", EventId, Source, checkin)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCheckInbySource(ByVal EventId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCheckInbySource", EventId, CheckIn, Source)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Student_Insert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, ByVal Source As Integer, ByVal Nguon As String, ByVal CreatedDate As DateTime, ByVal PortalId As Integer, Nguontutao As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_Insert", EventId, EventCatId, StudentId, StudentCode, Source, Nguon, CreatedDate, PortalId, Nguontutao)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateCheckIn(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateCheckIn", EventId, EventCatId, StudentId, Nguoidikem, Checkin, CheckInDate, UserId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateThamdu(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Thamdu As Boolean, ByVal ThamduDateUpdate As DateTime, ByVal ThamduUserUpdate As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateThamdu", EventId, EventCatId, StudentId, Thamdu, ThamduDateUpdate, ThamduUserUpdate)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateCheckInAfterFair(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateCheckInAfterFair", EventId, EventCatId, StudentId, Checkin, CheckInDate, UserId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateStudentNguon(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal Nguon As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateStudentNguon", EventId, StudentId, Nguon)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateStudentNguonTutao(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal NguonTutao As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateStudentNguonTutao", EventId, StudentId, NguonTutao)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateCheckInInsert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, Source As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CreatedDate As DateTime, Portalid As Integer, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateCheckInInsert", EventId, StudentId, EventCatId, StudentCode, Source, Nguoidikem, Checkin, CreatedDate, Portalid, CheckInDate, UserId)
        End Sub
        Public Overrides Function Events_Student_GetById(ByVal NewsFeedbackId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectById", NewsFeedbackId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetAllByEventCat(ByVal EventCatId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCat", EventCatId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetCountByEventCat(Datetime As DateTime, ByVal EventCatId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_CountByEventCat", Datetime, EventCatId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetCountByEvent(Datetime As DateTime, ByVal EventId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_CountByEvent", Datetime, EventId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatCheckInbySource(ByVal EventCatId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatCheckInbySource", EventCatId, CheckIn, Source)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatbySource(ByVal EventCatId As Integer, Source As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatbySource", EventCatId, Source)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatbyNguon(ByVal EventCatId As Integer, CheckIn As Integer, Nguon As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatbyNguon", EventCatId, CheckIn, Nguon)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatbyNguonTutao(ByVal EventCatId As Integer, CheckIn As Integer, NguonTutao As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatbyNguonTutao", EventCatId, CheckIn, NguonTutao)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventbyNguon(ByVal EventId As Integer, CheckIn As Integer, Nguon As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventbyNguon", EventId, CheckIn, Nguon)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_FindCountByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_FindCountByEvent", EventId, EventCatId, Checkin, Source)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_FindIndexByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_FindIndexByEvent", EventId, EventCatId, Checkin, Source, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Student_DeleteStudentEventId(ByVal EventId As Integer, StudentId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_DeleteStudentEventId", EventId, StudentId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectByEventstudentid(ByVal EventId As Integer, StudentId As Integer)
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectByEventstudentid", EventId, StudentId)
        End Function
        '------------------------------------------'

#End Region
#Region "Cap_Loaitruong"

        Public Overrides Function Cap_Loaitruong_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Loaitruong_SelectByID", id, PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function Cap_Loaitruong_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Loaitruong_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Cap_Loaitruong_GetAllShow(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Loaitruong_SelectAllShow", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub Cap_Loaitruong_Insert(ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, ByVal PortalId As Integer, Ordernumber As Integer, ByVal CreatedDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Loaitruong_Insert", Loaitruong, Descreption, IsActive, Ordernumber, PortalId, CreatedDate)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Cap_Loaitruong_Update(ByVal id As Integer, ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Loaitruong_Update", id, Loaitruong, Descreption, IsActive, Ordernumber, PortalId, CreatedDate)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Cap_Loaitruong_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Loaitruong_Update_Ordernumer", id, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Cap_Loaitruong_Delete(ByVal id As Integer, PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Loaitruong_Delete", id, PortalId)
        End Sub

        '------------------------------------------'


#End Region
#Region "Cap_Truong_Major"

        Public Overrides Function Major_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Truong_Major_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function Major_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Truong_Major_SelectAll")
        End Function

        '------------------------------------------'


#End Region
#Region "DM_Truong_LoaiTruong"

        Public Overrides Sub LoaiTruong_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "DM_Truong_LoaiTruong_CRUD", Action, id, Loaitruong, Descreption, IsActive, Ordernumber, PortalId, CreatedDate)
        End Sub
        '------------------------------------------'
        Public Overrides Function LoaiTruong_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Truong_LoaiTruong_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function LoaiTruong_GetAllShow(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Truong_LoaiTruong_SelectAllShow", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function LoaiTruong_GetById(Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Truong_LoaiTruong_SelectByID", Id)
        End Function
        '------------------------------------------'
        Public Overrides Sub LoaiTruong_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "DM_Truong_LoaiTruong_Update_Ordernumer", id, Ordernumber)
        End Sub
        '------------------------------------------'



#End Region
#Region "DM_TrinhDo"

        Public Overrides Sub TrinhDo_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "DM_TrinhDo_CRUD", Action, id, Title, TitleEN)
        End Sub
        '------------------------------------------'
        Public Overrides Function TrinhDo_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_TrinhDo_SelectAll")
        End Function
        '------------------------------------------'
        Public Overrides Function TrinhDo_GetById(Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_TrinhDo_SelectById", Id)
        End Function
        '------------------------------------------'
        Public Overrides Function TrinhDo_GetAllByChoose(ids As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_TrinhDo_SelectAllByChoose", ids)
        End Function
        '------------------------------------------'
        Public Overrides Function TrinhDo_FindCount(ByVal Title As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "DM_TrinhDo_FindCount", Title)
        End Function
        '------------------------------------------'
        Public Overrides Function TrinhDo_FindIndex(ByVal Title As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "DM_TrinhDo_FindIndex", Title, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'



#End Region
#Region "DM_Code_HinhThuc"

        Public Overrides Sub Code_HinhThuc_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Title As String, ByVal Code As String, ParentId As Integer, ByVal CreatedDate As DateTime, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "DM_CODE_HinhThuc_CRUD", Action, id, Title, Code, ParentId, CreatedDate, UserId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Code_HinhThuc_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Code_HinhThuc_SelectAll")
        End Function
        '------------------------------------------'
        Public Overrides Function Code_HinhThuc_GetById(Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_CODE_Hinhthuc_SelectByID", Id)
        End Function
        '------------------------------------------'
        Public Overrides Function Code_HinhThuc_FindCount(ByVal Title As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "DM_Code_HinhThuc_FindCount", Title)
        End Function
        '------------------------------------------'
        Public Overrides Function Code_HinhThuc_FindIndex(ByVal Title As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "DM_Code_HinhThuc_FindIndex", Title, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'



#End Region
#Region "DM_FollowUpPhuongThuc"

        Public Overrides Sub FollowUpPhuongThuc_CRUD(ByVal Action As String, ByVal id As Integer, ByVal PhuongThuc As String, ByVal ParentId As Integer, isShow As Boolean, IsActive As Boolean, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "DM_Follow_PhuongThuc_CRUD", Action, id, PhuongThuc, ParentId, isShow, IsActive, UserId)
        End Sub
        '------------------------------------------'
        Public Overrides Function FollowUpPhuongThuc_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Follow_PhuongThuc_SelectAll")
        End Function
        '------------------------------------------'
        Public Overrides Function FollowUpPhuongThuc_GetById(Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Follow_PhuongThuc_SelectById", Id)
        End Function
        '------------------------------------------'
        Public Overrides Function FollowUpPhuongThuc_GetByParentId(ParentId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Follow_PhuongThuc_SelectByParentID", ParentId)
        End Function
        '------------------------------------------'
        Public Overrides Function FollowUpPhuongThuc_FindCount(ByVal Title As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "DM_Follow_PhuongThuc_FindCount", Title)
        End Function
        '------------------------------------------'
        Public Overrides Function FollowUpPhuongThuc_FindIndex(ByVal Title As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "DM_FollowUpPhuongThuc_FindIndex", Title, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'



#End Region
#Region "DM_Follow_TrangThaiNhom"

        Public Overrides Sub Follow_TrangThaiNhom_CRUD(ByVal Action As String, ByVal id As Integer, ByVal TenNhom As String, ByVal Descreption As String, ByVal Ordernumber As Integer, ByVal Createddate As DateTime, ByVal Userid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "DM_Follow_TrangThaiNhom_CRUD", Action, id, TenNhom, Descreption, Ordernumber, Createddate, Userid)
        End Sub
        '------------------------------------------'
        Public Overrides Function Follow_TrangThaiNhom_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Follow_TrangThaiNhom_SelectAll")
        End Function
        '------------------------------------------'
        Public Overrides Function Follow_TrangThaiNhom_GetById(Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Follow_TrangThaiNhom_SelectById", Id)
        End Function
        '------------------------------------------'
#End Region
#Region "DM_Follow_TrangThai"

        Public Overrides Sub Follow_TrangThai_CRUD(ByVal Action As String, ByVal Id As Integer, ByVal Title As String, ByVal ParentId As Integer, isShow As Boolean, isActive As Boolean, ByVal Kyhopdong As Boolean, ByVal UserId As Integer, ByVal CreatedDate As DateTime, Student_NhomId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "DM_Follow_TrangThai_CRUD", Action, Id, Title, ParentId, isShow, isActive, Kyhopdong, UserId, CreatedDate, Student_NhomId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Follow_TrangThaI_GetAll(ByVal Kyhopdong As Boolean) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Follow_TrangThai_SelectAll", Kyhopdong)
        End Function
        '------------------------------------------'
        Public Overrides Function Follow_TrangThai_GetById(Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "DM_Follow_TrangThai_SelectById", Id)
        End Function
        '------------------------------------------'
#End Region
#End Region


    End Class

End Namespace