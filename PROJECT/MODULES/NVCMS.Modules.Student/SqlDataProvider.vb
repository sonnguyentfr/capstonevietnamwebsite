
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.Student

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

#Region "Public Methods"
#Region "Student_Info"

        '------------------------------------------'
        Public Overrides Function Info_OS_Find_Count(ByVal Action As String, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_OS_Find_Count", Action, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function Info_OS_Find_Index(ByVal Action As String, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Student_Info_OS_Find_Index", Action, Portalid, PageIndex, PageSize), IDataReader)
        End Function

        '////////////////////////////////////////////////////////
        '//Cu
        Public Overrides Function _Info_Insert(ByVal objdata As StudentInfoInfo) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "Student_Info_Insert", objdata.VP, objdata.Type, objdata.Hotendem, objdata.Ten, objdata.Sex, objdata.Ngaysinh, objdata.KieuNgaysinh, objdata.Sodienthoai, objdata.Email, objdata.Diachi, objdata.Tinh, objdata.Huyen, objdata.EB5, objdata.PermissionUser, objdata.FollowPhuongThuc, objdata.FollowKetQua, objdata.FollowNoiDung, objdata.FollowUpStatus, objdata.FollowUpDateUpdate, objdata.TuVanHocVanmongmuon, objdata.TuVanNamdi, objdata.TuVanKyhoc, objdata.TuVanNganhhoc, objdata.TuVanTruongdukien, objdata.TuVanQuocgia, objdata.TuVanDiadiem, objdata.TuVanKhanangchitra, objdata.TuVanKhac, objdata.TuVanEditUserId, objdata.TuVanEditDate, objdata.TuVanApproveUserId, objdata.TuVanApproveDate, objdata.HocVanDanghoc, objdata.HocVanTruongdanghoc, objdata.HocVanDiemtrungbinh, objdata.HocVanDiemsobaithichuanhoa, objdata.HocVanLuuy, objdata.HocVanEditUserId, objdata.HocVanEditDate, objdata.HocVanApproveUserId, objdata.HocVanApproveDate, objdata.CreatedDate, objdata.UserId, objdata.PortalId, objdata.Xoa), Integer)
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
        Public Overrides Function Events_Student_FindCountThamDuByEvent(EventId As Integer, EventCatId As Integer, Thamdu As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_FindCountThamDuByEvent", EventId, EventCatId, Thamdu)
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
#Region "Student_Follow_Log"

        Public Overrides Function _Follow_Log_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Follow_Log_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function _Follow_Log_GetByStudentID(ByVal Studentid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Follow_Log_SelectByStudentID", Studentid)
        End Function

        '------------------------------------------'
        Public Overrides Function _Follow_Log_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Student_Follow_Log_SelectAll")
        End Function

        '------------------------------------------'
        Public Overrides Sub _Follow_Log_Insert(ByVal StudentId As Integer, ByVal Noidung As String, ByVal CreatedDate As DateTime, ByVal PortalId As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Follow_Log_Insert", StudentId, Noidung, CreatedDate, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub _Follow_Log_Update(ByVal id As Integer, ByVal StudentId As Integer, ByVal Noidung As String, ByVal CreatedDate As DateTime, ByVal PortalId As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Follow_Log_Update", id, StudentId, Noidung, CreatedDate, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub _Follow_Log_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Student_Follow_Log_Delete", id)
        End Sub

        '------------------------------------------'


#End Region
#Region "StudentFromLadipageController"
        '------------------------------------------'
        Public Overrides Function StudentFromLadipage_Info_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "sp_student_from_ladipage_select_all")
        End Function
        '------------------------------------------'
        Public Overrides Function StudentFromLadipage_Info_GetByEventCatId(ByVal event_id As Integer, ByVal is_update_crm As Boolean) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "sp_student_from_ladipage_select_by_event_id", event_id, is_update_crm)
        End Function
        Public Overrides Sub StudentFromLadipage_Info_Update_Crm(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "sp_student_from_ladipage_update_crm", id)
        End Sub
#End Region
#End Region


    End Class

End Namespace