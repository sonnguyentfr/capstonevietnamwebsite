'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006

Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.LibCRM

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.LibCRM", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "Cap_Location"

        Public MustOverride Function Location_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Function Location_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function Location_SelectByParentId(Parentid As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Sub Location_Insert(ByVal Name As String, ByVal ShortName As String, currency As String, currencyName As String, currencycode As String, PostCode As String, ByVal ParentId As Integer, ByVal Status As Boolean, Ordernumber As Integer, ByVal mapLatitude As String, ByVal mapLongitude As String, ByVal Info As String, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)

        Public MustOverride Sub Location_Delete(ByVal id As Integer, PortalId As Integer)

        Public MustOverride Sub Location_Update(ByVal id As Integer, ByVal Name As String, ByVal ShortName As String, currency As String, currencyName As String, currencycode As String, PostCode As String, ByVal ParentId As Integer, ByVal Status As Boolean, Ordernumber As Integer, ByVal mapLatitude As String, ByVal mapLongitude As String, ByVal Info As String, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)

        Public MustOverride Sub Location_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer, ByVal PortalId As Integer)
#End Region
#Region "Student_Info"

        Public MustOverride Function _Info_Insert(vp As Integer, type As Integer, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal FollowPhuongThuc As Integer, ByVal FollowKetQua As Integer, ByVal FollowNoiDung As String, ByVal FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean) As Integer

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
#Region "Events_Cat"

        Public MustOverride Function Events_Cat_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetByTabID(ByVal tabid As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAllShow(PortalId As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAllShowPastCount(ByVal CatName As String, Portalid As Integer) As Integer
        Public MustOverride Function Events_Cat_GetAllShowPast(ByVal CatName As String, Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAllShowOnline(PortalId As Integer) As IDataReader

        Public MustOverride Sub Events_Cat_Insert(ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, code As String, Source As String, Email As String, ByVal DateShow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, Tabid As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)

        Public MustOverride Sub Events_Cat_Delete(ByVal id As Integer, PortalId As Integer)

        Public MustOverride Sub Events_Cat_Update(ByVal id As Integer, ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, Source As String, Email As String, ByVal DateShow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, TabId As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)

        Public MustOverride Sub Events_Cat_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)

        Public MustOverride Sub Events_Cat_UpdateFairSchool(ByVal id As Integer, Portalid As Integer, FairSchool As String)

        Public MustOverride Sub Events_Cat_UpdateFairOrg(ByVal id As Integer, Portalid As Integer, FairOrg As String)

        Public MustOverride Sub Events_Cat_UpdateFairDiengia(ByVal id As Integer, Portalid As Integer, FairDiengia As String)

        Public MustOverride Sub Events_Cat_UpdateFairTestimonial(ByVal id As Integer, Portalid As Integer, FairTestimonial As String)

        Public MustOverride Sub Events_Cat_UpdateFairDonviTaiTro(ByVal id As Integer, Portalid As Integer, FairDonviTaiTro As String)


#End Region
#Region "Events"
        Public MustOverride Function Events_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Function Events_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function Events_GetAllByCat(CatId As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Function Events_GetAllShowByCat(CatId As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Sub Events_Insert(ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)

        Public MustOverride Sub Events_Update(ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)

        Public MustOverride Sub Events_Delete(ByVal id As Integer, PortalId As Integer)
        Public MustOverride Function Events_Find_Count(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer) As Integer
        Public MustOverride Function Events_Find_Index(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Events_FindShow_Count(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer) As Integer
        Public MustOverride Function Events_FindShow_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Events_FindShowPast_Count(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer) As Integer
        Public MustOverride Function Events_FindShowPast_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Events_GetAllShow(PortalId As Integer, Count As Integer) As IDataReader

        Public MustOverride Function Events_GetAllOnline(PortalId As Integer, Count As Integer) As IDataReader

        Public MustOverride Function Events_GetAllShowEnd(PortalId As Integer, Count As Integer) As IDataReader
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

        Public MustOverride Function Events_Student_FindIndexByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Sub Events_Student_DeleteStudentEventId(EventId As Integer, StudentId As Integer)

        Public MustOverride Function Events_Student_SelectByEventstudentid(EventId As Integer, StudentId As Integer)
#End Region
#Region "Cap_Loaitruong"

        Public MustOverride Function Cap_Loaitruong_GetByID(ByVal id As Integer, Portalid As Integer) As IDataReader

        Public MustOverride Function Cap_Loaitruong_GetAll(Portalid As Integer) As IDataReader

        Public MustOverride Function Cap_Loaitruong_GetAllShow(Portalid As Integer) As IDataReader

        Public MustOverride Sub Cap_Loaitruong_Insert(ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)

        Public MustOverride Sub Cap_Loaitruong_Delete(ByVal id As Integer, Portalid As Integer)

        Public MustOverride Sub Cap_Loaitruong_Update(ByVal id As Integer, ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)

        Public MustOverride Sub Cap_Loaitruong_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)

#End Region
#Region "Cap_Truong_Major"

        Public MustOverride Function Major_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function Major_GetAll() As IDataReader

#End Region
#Region "DM_LoaiTruong"
        Public MustOverride Sub LoaiTruong_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Loaitruong As String, ByVal Descreption As String, IsActive As Boolean, Ordernumber As Integer, ByVal PortalId As Integer, ByVal CreatedDate As DateTime)
        Public MustOverride Function LoaiTruong_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function LoaiTruong_GetAllShow(PortalId As Integer) As IDataReader
        Public MustOverride Function LoaiTruong_GetById(Id As Integer) As IDataReader
        Public MustOverride Sub LoaiTruong_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
#End Region
#Region "DM_TrinhDo"
        Public MustOverride Sub TrinhDo_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String)
        Public MustOverride Function TrinhDo_GetAllByChoose(ids As String) As IDataReader
        Public MustOverride Function TrinhDo_GetAll() As IDataReader
        Public MustOverride Function TrinhDo_GetById(Id As Integer) As IDataReader
        Public MustOverride Function TrinhDo_FindCount(Title As String) As Integer
        Public MustOverride Function TrinhDo_FindIndex(Title As String, PageIndex As Integer, PageSize As Integer) As IDataReader

#End Region
#Region "DM_Code_HinhThuc"
        Public MustOverride Sub Code_HinhThuc_CRUD(ByVal Action As String, ByVal id As Integer, ByVal Title As String, ByVal Code As String, ParentId As Integer, ByVal CreatedDate As DateTime, ByVal UserId As Integer)
        Public MustOverride Function Code_HinhThuc_GetAll() As IDataReader
        Public MustOverride Function Code_HinhThuc_GetById(Id As Integer) As IDataReader
        Public MustOverride Function Code_HinhThuc_FindCount(Title As String) As Integer
        Public MustOverride Function Code_HinhThuc_FindIndex(Title As String, PageIndex As Integer, PageSize As Integer) As IDataReader

#End Region
#Region "DM_FollowUpPhuongThuc"
        Public MustOverride Sub FollowUpPhuongThuc_CRUD(ByVal Action As String, ByVal id As Integer, ByVal PhuongThuc As String, ByVal ParentId As Integer, isShow As Boolean, IsActive As Boolean, ByVal UserId As Integer)
        Public MustOverride Function FollowUpPhuongThuc_GetAll() As IDataReader
        Public MustOverride Function FollowUpPhuongThuc_GetById(Id As Integer) As IDataReader
        Public MustOverride Function FollowUpPhuongThuc_GetByParentId(ParentId As Integer) As IDataReader
        Public MustOverride Function FollowUpPhuongThuc_FindCount(Title As String) As Integer
        Public MustOverride Function FollowUpPhuongThuc_FindIndex(Title As String, PageIndex As Integer, PageSize As Integer) As IDataReader

#End Region
#Region "DM_FollowUpTrangThaiNhom"
        Public MustOverride Sub Follow_TrangThaiNhom_CRUD(ByVal Action As String, ByVal id As Integer, ByVal TenNhom As String, ByVal Descreption As String, ByVal Ordernumber As Integer, ByVal Createddate As DateTime, ByVal Userid As Integer)
        Public MustOverride Function Follow_TrangThaiNhom_GetAll() As IDataReader
        Public MustOverride Function Follow_TrangThaiNhom_GetById(Id As Integer) As IDataReader
#End Region
#Region "DM_FollowUpTrangThaiNhom"
        Public MustOverride Sub Follow_TrangThai_CRUD(ByVal Action As String, ByVal Id As Integer, ByVal Title As String, ByVal ParentId As Integer, isShow As Boolean, isActive As Boolean, ByVal Kyhopdong As Boolean, ByVal UserId As Integer, ByVal CreatedDate As DateTime, Student_NhomId As Integer)
        Public MustOverride Function Follow_TrangThaI_GetAll(ByVal Kyhopdong As Boolean) As IDataReader
        Public MustOverride Function Follow_TrangThai_GetById(Id As Integer) As IDataReader
#End Region

#End Region


    End Class

End Namespace