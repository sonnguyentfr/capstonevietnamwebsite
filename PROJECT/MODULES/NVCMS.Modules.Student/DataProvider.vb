Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.Student

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' An abstract class for the data access layer
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.Student", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"
#Region "Student_Info"

        Public MustOverride Function Info_OS_Find_Count(ByVal Action As String, ByVal Portalid As Integer) As Integer

        Public MustOverride Function Info_OS_Find_Index(ByVal Action As String, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader


        '====CŨ
        Public MustOverride Function _Info_Insert(ByVal obj As StudentInfoInfo) As Integer

        Public MustOverride Sub _Info_InsertCode(id As Integer, code As String)

        Public MustOverride Sub _Info_InsertHinhThuc(id As Integer, HinhThuc As Integer)

        Public MustOverride Sub _Info_InsertExcel(code As String, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean)

        Public MustOverride Sub _Info_Update(ByVal id As Integer, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal FollowPhuongThuc As Integer, ByVal FollowKetQua As Integer, ByVal FollowNoiDung As String, ByVal FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal BoFullName As String, ByVal BoSodienthoai As String, ByVal BoEmail As String, ByVal BoNghenghiep As String, ByVal BoEditUserId As Integer, ByVal BoEditDate As DateTime, ByVal BoApproveUserId As Integer, ByVal BoApproveDate As DateTime, ByVal MeFullName As String, ByVal MeSodienthoai As String, ByVal MeEmail As String, ByVal MeNghenghiep As String, ByVal MeEditUserId As Integer, ByVal MeEditDate As DateTime, ByVal MeApproveUserId As Integer, ByVal MeApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean)

        Public MustOverride Sub _Info_UpdateInfo(ByVal id As Integer, ByVal Hotendem As String, ByVal Ten As String, Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean)

        Public MustOverride Sub _Info_UpdateTuVan(ByVal id As Integer, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime)

        Public MustOverride Sub _Info_UpdateHocVan(ByVal id As Integer, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime)

        Public MustOverride Sub _Info_UpdateVanPhong(ByVal id As Integer, vp As Integer)

        Public MustOverride Sub _Info_UpdateKyhopdong(ByVal id As Integer)

        Public MustOverride Sub _Info_UpdateFollowUp(ByVal id As Integer, FollowPhuongThuc As Integer, FollowKetQua As Integer, FollowNoidung As String, FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal PortalId As Integer)

        Public MustOverride Sub _Info_UpdateNhom(ByVal id As Integer, Nhom As Integer)

        Public MustOverride Sub _Info_UpdatePermissionUser(ByVal id As Integer, AdviserId As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub _Info_UpdateSupporterUser(ByVal id As Integer, PermissionUser As String, ByVal PortalId As Integer)

        Public MustOverride Sub _Info_UpdateSpy(ByVal id As Integer, isSpy As Boolean)

        Public MustOverride Sub _Info_UpdateDongyguithongtin(ByVal id As Integer, dongyguithongtin As Boolean)
        Public MustOverride Sub _Info_UpdateXoa(ByVal id As Integer, Xoa As Boolean)

        Public MustOverride Function _Info_GetAllSdt() As IDataReader

        Public MustOverride Function _Info_GetAll() As IDataReader

        Public MustOverride Function _Info_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function _Info_GetByCode(ByVal Code As String) As IDataReader

        Public MustOverride Function _Info_GetBySearch(ByVal key As String) As IDataReader

        Public MustOverride Function _Info_GetByEmail(ByVal Email As String) As IDataReader

        Public MustOverride Function _Info_GetTrungEmail() As IDataReader

        Public MustOverride Function _Info_GetBySodienthoai(ByVal Sodienthoai As String) As IDataReader

        Public MustOverride Function _Info_GetTrungSodienthoai() As IDataReader

        Public MustOverride Function _Info_GetByIDEmailExit(ByVal EMail As String) As Integer

        Public MustOverride Function _Info_GetByIDSDTExit(ByVal sodienthoai As String) As Integer

        Public MustOverride Function _Info_Find_Count(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer

        Public MustOverride Function _Info_Find_Index(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function _Info_Campaign_Find_Count(subtractIds As String, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer

        Public MustOverride Function _Info_Campaign_Find_Index(subtractIds As String, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function _Info_MarketingFind_Count(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, isspy As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer

        Public MustOverride Function _Info_MarketingFind_Index(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, isspy As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function _Info_FindPermissionUser_Count(ByVal AdviserId As Integer, vp As Integer, tinh As Integer, hinhthuc As Integer, sukien As Integer, trangthai As Integer, Email As String, Sodienthoai As String, ByVal Portalid As Integer) As Integer

        Public MustOverride Function _Info_FindPermissionUser_Index(ByVal AdviserId As Integer, vp As Integer, tinh As Integer, hinhthuc As Integer, sukien As Integer, trangthai As Integer, Email As String, Sodienthoai As String, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function _Info_UserFind_Count(ByVal fullname As String, Code As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, quocgia As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, hinhthuc As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer) As Integer

        Public MustOverride Function _Info_UserFind_Index(control As Integer, ByVal fullname As String, Code As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, quocgia As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, hinhthuc As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function _Info_UserFollowFind_Count(ByVal fullname As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinhfrom As Integer, Namsinhto As Integer, hinhthuc As Integer, phuongthuc As Integer, EventCatId As Integer, datefrom As DateTime, dateto As DateTime, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer) As Integer

        Public MustOverride Function _Info_UserFollowFind_Index(control As Integer, ByVal fullname As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinhfrom As Integer, Namsinhto As Integer, hinhthuc As Integer, phuongthuc As Integer, EventCatId As Integer, datefrom As DateTime, dateto As DateTime, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function _Info_Static_Count(Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, ByVal Portalid As Integer) As Integer

        Public MustOverride Function _Info_Checin_School(ByVal EventCatId As Integer, EventId As Integer, StudentId As Integer) As IDataReader

        Public MustOverride Function _Info_StaticUser_TelesaleCount(datetime As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, PortalId As Integer) As Integer

        Public MustOverride Function _Info_StaticUser_TelesaleCountTyle(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, PortalId As Integer) As Integer

        Public MustOverride Function _Info_StaticUser_TelesaleCountTyle_Index(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, UserId As Integer, PortalId As Integer) As IDataReader
        Public MustOverride Function _Info_StaticUser_UserTelesaleKhachHang(StudentId As Integer, datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, status As Integer, UserId As Integer, PortalId As Integer) As IDataReader
        Public MustOverride Function _Info_StaticUser_UserTelesaleGroupKhachHang(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer, PageIndex As Integer, PageSize As Integer) As IDataReader
        Public MustOverride Function _Info_StaticUser_UserTelesaleGroupKhachHang_CountCuocGoi(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer) As Integer
        Public MustOverride Function _Info_StaticUser_UserTelesaleGroupKhachHang_CountKhachHang(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer) As Integer
        Public MustOverride Function _Info_StaticUser_TrangThaiCountTyle(datefrom As DateTime, dateto As DateTime, Status As Integer, UserId As Integer, PortalId As Integer) As Integer
        Public MustOverride Function _Info_StaticUser_TrangThaiCountTyle_Index(datefrom As DateTime, dateto As DateTime, Status As Integer, UserId As Integer, PortalId As Integer) As IDataReader
        Public MustOverride Function _Info_StaticUser_Permission(datefrom As DateTime, dateto As DateTime, UserId As Integer, PortalId As Integer) As Integer
        Public MustOverride Function _Info_StaticUser_Permission_New(datefrom As DateTime, dateto As DateTime, UserId As Integer, PortalId As Integer) As Integer
#End Region
#Region "NV_Events_Student"

        Public MustOverride Function Events_Student_GetAllByEvent(EventId As Integer) As IDataReader

        Public MustOverride Function Events_Student_GetAllByStudent(StudentId As Integer) As IDataReader

        Public MustOverride Function Events_Student_GetAllByEventCheckIn(EventId As Integer, Source As String, checkin As Integer) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCheckInbySource(EventId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader

        Public MustOverride Sub Events_Student_Insert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, ByVal Source As Integer, ByVal Nguon As String, ByVal CreatedDate As DateTime, ByVal PortalId As Integer, NguonTutao As String)

        Public MustOverride Sub Events_Student_UpdateCheckIn(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)

        Public MustOverride Sub Events_Student_UpdateThamdu(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Thamdu As Boolean, ByVal ThamduDateUpdate As DateTime, ByVal ThamduUserUpdate As Integer)

        Public MustOverride Sub Events_Student_UpdateCheckInAfterFair(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)

        Public MustOverride Sub Events_Student_UpdateStudentNguon(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal Nguon As String)
        Public MustOverride Sub Events_Student_UpdateStudentNguonTutao(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal NguonTutao As String)

        Public MustOverride Sub Events_Student_UpdateCheckInInsert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, Source As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CreatedDate As DateTime, Portalid As Integer, ByVal CheckInDate As DateTime, ByVal UserId As Integer)

        Public MustOverride Function Events_Student_GetById(id As Integer) As IDataReader
        Public MustOverride Function Events_Student_GetAllByEventCat(EventCatId As Integer) As IDataReader

        Public MustOverride Function Events_Student_GetCountByEventCat(datetime As DateTime, EventCatId As Integer) As Integer

        Public MustOverride Function Events_Student_GetCountByEvent(datetime As DateTime, EventId As Integer) As Integer

        Public MustOverride Function Events_Student_SelectAllByEventCatCheckInbySource(EventCatId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCatbySource(EventId As Integer, Source As Integer) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCatbyNguon(EventCatId As Integer, CheckIn As Integer, Nguon As String) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCatbyNguonTutao(EventCatId As Integer, CheckIn As Integer, NguonTutao As String) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventbyNguon(EventId As Integer, CheckIn As Integer, Nguon As String) As IDataReader

        Public MustOverride Function Events_Student_FindCountByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer) As Integer

        Public MustOverride Function Events_Student_FindCountThamDuByEvent(EventId As Integer, EventCatId As Integer, Thamdu As Integer) As Integer

        Public MustOverride Function Events_Student_FindIndexByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Sub Events_Student_DeleteStudentEventId(EventId As Integer, StudentId As Integer)

        Public MustOverride Function Events_Student_SelectByEventstudentid(EventId As Integer, StudentId As Integer)
#End Region
#Region "Student_Follow_Log"

        Public MustOverride Function _Follow_Log_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function _Follow_Log_GetByStudentID(ByVal Studentid As Integer) As IDataReader

        Public MustOverride Function _Follow_Log_GetAll() As IDataReader

        Public MustOverride Sub _Follow_Log_Insert(ByVal StudentId As Integer, ByVal Noidung As String, ByVal CreatedDate As DateTime, ByVal PortalId As String)

        Public MustOverride Sub _Follow_Log_Delete(ByVal id As Integer)

        Public MustOverride Sub _Follow_Log_Update(ByVal id As Integer, ByVal StudentId As Integer, ByVal Noidung As String, ByVal CreatedDate As DateTime, ByVal PortalId As String)

#End Region
#Region "StudentFromLadipageController"
        Public MustOverride Function StudentFromLadipage_Info_GetAll() As IDataReader

        Public MustOverride Function StudentFromLadipage_Info_GetByEventCatId(ByVal event_id As Integer, ByVal is_update_crm As Boolean) As IDataReader
        Public MustOverride Sub StudentFromLadipage_Info_Update_Crm(ByVal id As Integer)
#End Region

#End Region


    End Class

End Namespace