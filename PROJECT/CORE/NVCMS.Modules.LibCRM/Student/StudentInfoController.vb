'******************************************
'Author         :Mr Dòi
'Created Date   :3/21/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Data
Namespace NVCMS.Modules.LibCRM

    Public Class Lib_StudentInfoController
        Private Sub ClearCacheAll()
        End Sub
        Public Function _Info_Insert(vp As Integer, type As Integer, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal FollowPhuongThuc As Integer, ByVal FollowKetQua As Integer, ByVal FollowNoiDung As String, ByVal FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean) As Integer
            Return DataProvider.Instance._Info_Insert(vp, type, Hotendem, Ten, Sex, Ngaysinh, kieungaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5, PermissionUser, FollowPhuongThuc, FollowKetQua, FollowNoiDung, FollowUpStatus, FollowUpDateUpdate, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate, CreatedDate, UserId, PortalId, Xoa)
            ClearCacheAll()
        End Function
        '------------------------------------------'
        Public Sub _Info_InsertCode(id As Integer, code As String)
            DataProvider.Instance._Info_InsertCode(id, code)
        End Sub
        '------------------------------------------'
        Public Sub _Info_InsertHinhThuc(id As Integer, HinhThuc As Integer)
            DataProvider.Instance._Info_InsertHinhThuc(id, HinhThuc)
        End Sub
        '------------------------------------------'
        Public Sub _Info_InsertExcel(Code As String, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean)
            DataProvider.Instance._Info_InsertExcel(Code, Hotendem, Ten, Sex, Ngaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5, PermissionUser, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate, CreatedDate, UserId, PortalId, Xoa)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_Update(ByVal id As Integer, ByVal Hotendem As String, ByVal Ten As String, ByVal Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean, ByVal PermissionUser As String, ByVal FollowPhuongThuc As Integer, ByVal FollowKetQua As Integer, ByVal FollowNoiDung As String, ByVal FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime, ByVal BoFullName As String, ByVal BoSodienthoai As String, ByVal BoEmail As String, ByVal BoNghenghiep As String, ByVal BoEditUserId As Integer, ByVal BoEditDate As DateTime, ByVal BoApproveUserId As Integer, ByVal BoApproveDate As DateTime, ByVal MeFullName As String, ByVal MeSodienthoai As String, ByVal MeEmail As String, ByVal MeNghenghiep As String, ByVal MeEditUserId As Integer, ByVal MeEditDate As DateTime, ByVal MeApproveUserId As Integer, ByVal MeApproveDate As DateTime, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Xoa As Boolean)
            DataProvider.Instance._Info_Update(id, Hotendem, Ten, Sex, Ngaysinh, kieungaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5, PermissionUser, FollowPhuongThuc, FollowKetQua, FollowNoiDung, FollowUpStatus, FollowUpDateUpdate, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate, BoFullName, BoSodienthoai, BoEmail, BoNghenghiep, BoEditUserId, BoEditDate, BoApproveUserId, BoApproveDate, MeFullName, MeSodienthoai, MeEmail, MeNghenghiep, MeEditUserId, MeEditDate, MeApproveUserId, MeApproveDate, CreatedDate, UserId, PortalId, Xoa)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateInfo(ByVal id As Integer, ByVal Hotendem As String, ByVal Ten As String, Sex As Boolean, ByVal Ngaysinh As DateTime, kieungaysinh As Integer, ByVal Sodienthoai As String, ByVal Email As String, ByVal Diachi As String, ByVal Tinh As Integer, ByVal Huyen As Integer, EB5 As Boolean)
            DataProvider.Instance._Info_UpdateInfo(id, Hotendem, Ten, Sex, Ngaysinh, kieungaysinh, Sodienthoai, Email, Diachi, Tinh, Huyen, EB5)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateVanPhong(ByVal id As Integer, vp As Integer)
            DataProvider.Instance._Info_UpdateVanPhong(id, vp)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateKyhopdong(ByVal id As Integer)
            DataProvider.Instance._Info_UpdateKyhopdong(id)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateTuVan(ByVal id As Integer, ByVal TuVanHocVanmongmuon As String, ByVal TuVanNamdi As String, ByVal TuVanKyhoc As String, ByVal TuVanNganhhoc As String, ByVal TuVanTruongdukien As String, ByVal TuVanQuocgia As String, ByVal TuVanDiadiem As Integer, ByVal TuVanKhanangchitra As Integer, ByVal TuVanKhac As String, ByVal TuVanEditUserId As Integer, ByVal TuVanEditDate As DateTime, ByVal TuVanApproveUserId As Integer, ByVal TuVanApproveDate As DateTime)
            DataProvider.Instance._Info_UpdateTuVan(id, TuVanHocVanmongmuon, TuVanNamdi, TuVanKyhoc, TuVanNganhhoc, TuVanTruongdukien, TuVanQuocgia, TuVanDiadiem, TuVanKhanangchitra, TuVanKhac, TuVanEditUserId, TuVanEditDate, TuVanApproveUserId, TuVanApproveDate)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateHocVan(ByVal id As Integer, ByVal HocVanDanghoc As String, ByVal HocVanTruongdanghoc As String, ByVal HocVanDiemtrungbinh As String, ByVal HocVanDiemsobaithichuanhoa As String, ByVal HocVanLuuy As String, ByVal HocVanEditUserId As Integer, ByVal HocVanEditDate As DateTime, ByVal HocVanApproveUserId As Integer, ByVal HocVanApproveDate As DateTime)
            DataProvider.Instance._Info_UpdateHocVan(id, HocVanDanghoc, HocVanTruongdanghoc, HocVanDiemtrungbinh, HocVanDiemsobaithichuanhoa, HocVanLuuy, HocVanEditUserId, HocVanEditDate, HocVanApproveUserId, HocVanApproveDate)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateFollowUp(ByVal id As Integer, FollowPhuongThuc As Integer, FollowKetQua As Integer, FollowNoidung As String, FollowUpStatus As Integer, ByVal FollowUpDateUpdate As DateTime, ByVal PortalId As Integer)
            DataProvider.Instance._Info_UpdateFollowUp(id, FollowPhuongThuc, FollowKetQua, FollowNoidung, FollowUpStatus, FollowUpDateUpdate, PortalId)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateNhom(ByVal id As Integer, Nhom As Integer)
            DataProvider.Instance._Info_UpdateNhom(id, Nhom)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdatePermissionUser(ByVal id As Integer, AdviserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance._Info_UpdatePermissionUser(id, AdviserId, PortalId)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateSupporterUser(ByVal id As Integer, PermissionUser As String, ByVal PortalId As Integer)
            DataProvider.Instance._Info_UpdateSupporterUser(id, PermissionUser, PortalId)
            ClearCacheAll()
        End Sub

        '------------------------------------------'
        Public Sub _Info_UpdateSpy(ByVal id As Integer, isSpy As Boolean)
            DataProvider.Instance._Info_UpdateSpy(id, isSpy)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateDongyguithongtin(ByVal id As Integer, dongyguithongtin As Boolean)
            DataProvider.Instance._Info_UpdateDongyguithongtin(id, dongyguithongtin)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Sub _Info_UpdateXoa(ByVal id As Integer, Xoa As Boolean)
            DataProvider.Instance._Info_UpdateXoa(id, Xoa)
            ClearCacheAll()
        End Sub
        '------------------------------------------'
        Public Function _Info_GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_GetAll(), GetType(Lib_StudentInfoInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_GetAllSDT() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_GetAllSdt(), GetType(Lib_StudentInfoInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_GetByID(ByVal id As Integer) As Lib_StudentInfoInfo
            Return CType(CBO.FillObject(Of Lib_StudentInfoInfo)(DataProvider.Instance._Info_GetByID(id), True), Lib_StudentInfoInfo)
        End Function
        '------------------------------------------'
        Public Function _Info_GetByCode(ByVal Code As String) As Lib_StudentInfoInfo
            Return CType(CBO.FillObject(Of Lib_StudentInfoInfo)(DataProvider.Instance._Info_GetByCode(Code), True), Lib_StudentInfoInfo)
        End Function
        '------------------------------------------'
        Public Function _Info_GetBySearch(ByVal key As String) As Lib_StudentInfoInfo
            Return CType(CBO.FillObject(Of Lib_StudentInfoInfo)(DataProvider.Instance._Info_GetBySearch(key), True), Lib_StudentInfoInfo)
        End Function
        '------------------------------------------'
        Public Function _Info_GetByEmail(ByVal Email As String) As Lib_StudentInfoInfo
            Return CType(CBO.FillObject(Of Lib_StudentInfoInfo)(DataProvider.Instance._Info_GetByEmail(Email), True), Lib_StudentInfoInfo)
        End Function
        '------------------------------------------'
        Public Function _Info_GetByEmailAll(ByVal Email As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_GetByEmail(Email), GetType(Lib_StudentInfoInfo))
        End Function
        Public Function _Info_GetTrungEmail() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_GetTrungEmail(), GetType(Lib_StudentInfoInfo))
        End Function
        Public Function _Info_GetBySodienthoai(ByVal Sodienthoai As String) As Lib_StudentInfoInfo
            Return CType(CBO.FillObject(Of Lib_StudentInfoInfo)(DataProvider.Instance._Info_GetBySodienthoai(Sodienthoai), True), Lib_StudentInfoInfo)
        End Function
        Public Function _Info_GetBySodienthoaiAll(ByVal Sodienthoai As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_GetBySodienthoai(Sodienthoai), GetType(Lib_StudentInfoInfo))
        End Function
        Public Function _Info_GetTrungSodienthoai() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_GetTrungSodienthoai(), GetType(Lib_StudentInfoInfo))
        End Function
        Public Function _Info_GetByIDEmailExit(ByVal email As String) As Integer
            Return DataProvider.Instance._Info_GetByIDEmailExit(email)
        End Function
        Public Function _Info_GetByIDSDTExit(ByVal sodienthoai As String) As Integer
            Return DataProvider.Instance._Info_GetByIDSDTExit(sodienthoai)
        End Function
        '------------------------------------------'
        Public Function _Info_Find_Count(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer
            Return DataProvider.Instance._Info_Find_Count(subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid)
        End Function
        '------------------------------------------'
        Public Function _Info_Find_Index(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            'Dim stringcache = "OS_StudentList" & fullname & Vp & Email & Sodienthoai & Bachoc & Quociga & Status & sex & Khanangchitra & location & Namsinh & Namsinhto & KyHopDong & Sukien & EventCatId & EventId & Checkin & Portalid & PageIndex & PageSize
            'If DataCache.GetCache(stringcache) Is Nothing Then
            '    Dim arrtop = CBO.FillCollection(DataProvider.Instance._Info_Find_Index(subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
            '    DataCache.SetCache(stringcache, arrtop)
            'End If
            'Return DataCache.GetCache(stringcache)
            Return CBO.FillCollection(DataProvider.Instance._Info_Find_Index(subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_Campaign_Find_Count(ByVal subtractIds As String, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer
            Return DataProvider.Instance._Info_Campaign_Find_Count(subtractIds, Vp, fullname, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, Sukien, EventCatId, EventId, Checkin, Portalid)
        End Function
        '------------------------------------------'
        Public Function _Info_Campaign_Find_Index(ByVal subtractIds As String, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_Campaign_Find_Index(subtractIds, Vp, fullname, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, Sukien, EventCatId, EventId, Checkin, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
        End Function

        Public Function _Info_MarketingFind_Count(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, isspy As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer) As Integer
            Return DataProvider.Instance._Info_MarketingFind_Count(subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, isspy, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid)
        End Function
        '------------------------------------------'
        Public Function _Info_MarketingFind_Index(ByVal subtractIds As String, fromdate As DateTime, enddate As DateTime, Vp As Integer, ByVal fullname As String, Email As String, Sodienthoai As String, Bachoc As String, Quociga As String, Status As Integer, sex As Integer, isspy As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, KyHopDong As Boolean, Sukien As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_MarketingFind_Index(subtractIds, fromdate, enddate, Vp, fullname, Email, Sodienthoai, Bachoc, Quociga, Status, sex, isspy, Khanangchitra, location, Namsinh, Namsinhto, KyHopDong, Sukien, EventCatId, EventId, Checkin, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_FindPermissionUser_Count(ByVal AdviserId As Integer, vp As Integer, tinh As Integer, hinhthuc As Integer, sukien As Integer, trangthai As Integer, Email As String, Sodienthoai As String, ByVal Portalid As Integer) As Integer
            Return DataProvider.Instance._Info_FindPermissionUser_Count(AdviserId, vp, tinh, hinhthuc, sukien, trangthai, Email, Sodienthoai, Portalid)
        End Function
        '------------------------------------------'
        Public Function _Info_FindPermissionUser_Index(ByVal AdviserId As Integer, vp As Integer, tinh As Integer, hinhthuc As Integer, sukien As Integer, trangthai As Integer, Email As String, Sodienthoai As String, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_FindPermissionUser_Index(AdviserId, vp, tinh, hinhthuc, sukien, trangthai, Email, Sodienthoai, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_UserFind_Count(ByVal fullname As String, Code As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, quocgia As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, hinhthuc As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer) As Integer
            'Dim stringcache = "StudentList_Record" & fullname & Code & vp & Email & Sodienthoai & Bachoc & quocgia & Status & sex & Khanangchitra & location & Namsinh & Namsinhto & hinhthuc & EventCatId & EventId & Checkin & UserId & Portalid
            'If DataCache.GetCache(stringcache) Is Nothing Then
            '    Dim totalrecord = DataProvider.Instance._Info_UserFind_Count(fullname, Code, vp, Email, Sodienthoai, Bachoc, quocgia, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, hinhthuc, EventCatId, EventId, Checkin, UserId, Portalid)
            '    DataCache.SetCache(stringcache, totalrecord)
            'End If
            'Return DataCache.GetCache(stringcache)
            Return DataProvider.Instance._Info_UserFind_Count(fullname, Code, vp, Email, Sodienthoai, Bachoc, quocgia, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, hinhthuc, EventCatId, EventId, Checkin, UserId, Portalid)
        End Function
        '------------------------------------------'
        Public Function _Info_UserFind_Index(control As Integer, ByVal fullname As String, Code As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, quocgia As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinh As Integer, Namsinhto As Integer, hinhthuc As Integer, EventCatId As Integer, EventId As Integer, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Dim stringcache = "CacheName_StudentList_User" & fullname & Code & vp & Email & Sodienthoai & Bachoc & quocgia & Status & sex & Khanangchitra & location & Namsinh & Namsinhto & hinhthuc & EventCatId & EventId & Checkin & UserId & Portalid & PageIndex & PageSize
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance._Info_UserFind_Index(control, fullname, Code, vp, Email, Sodienthoai, Bachoc, quocgia, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, hinhthuc, EventCatId, EventId, Checkin, UserId, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance._Info_UserFind_Index(control, fullname, Code, vp, Email, Sodienthoai, Bachoc, quocgia, Status, sex, Khanangchitra, location, Namsinh, Namsinhto, hinhthuc, EventCatId, EventId, Checkin, UserId, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_UserFollowFind_Count(ByVal fullname As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinhfrom As Integer, Namsinhto As Integer, hinhthuc As Integer, phuongthuc As Integer, EventCatId As Integer, datefrom As DateTime, dateto As DateTime, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer) As Integer
            Return DataProvider.Instance._Info_UserFollowFind_Count(fullname, vp, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinhfrom, Namsinhto, hinhthuc, phuongthuc, EventCatId, datefrom, dateto, Checkin, UserId, Portalid)
        End Function
        '------------------------------------------'
        Public Function _Info_UserFollowFind_Index(control As Integer, ByVal fullname As String, vp As Integer, Email As String, Sodienthoai As String, Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, location As String, Namsinhfrom As Integer, Namsinhto As Integer, hinhthuc As Integer, phuongthuc As Integer, EventCatId As Integer, datefrom As DateTime, dateto As DateTime, Checkin As Integer, UserId As Integer, ByVal Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Info_UserFollowFind_Index(control, fullname, vp, Email, Sodienthoai, Bachoc, Status, sex, Khanangchitra, location, Namsinhfrom, Namsinhto, hinhthuc, phuongthuc, EventCatId, datefrom, dateto, Checkin, UserId, Portalid, PageIndex, PageSize), GetType(Lib_StudentInfoInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_Static_Count(Bachoc As String, Status As Integer, sex As Integer, Khanangchitra As Integer, ByVal Portalid As Integer) As Integer
            Return DataProvider.Instance._Info_Static_Count(Bachoc, Status, sex, Khanangchitra, Portalid)
        End Function
        '------------------------------------------'
        'Public Function _Info_Checin_School(EventCatId As Integer, EventId As Integer, StudentId As Integer) As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance._Info_Checin_School(EventCatId, EventId, StudentId), GetType(SchoolCheckinStudentInfo))
        'End Function
        '------------------------------------------'
        '  Thong ke
        '------------------------------------------'
        Public Function _Info_StaticUser_TelesaleCount(datetime As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, PortalId As Integer) As Integer
            Return DataProvider.Instance._Info_StaticUser_TelesaleCount(datetime, phuongthuctiepcan, Status, UserId, PortalId)
        End Function
        '------------------------------------------'
        Public Function _Info_StaticUser_TelesaleCountTyle(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, PortalId As Integer) As Integer
            Return DataProvider.Instance._Info_StaticUser_TelesaleCountTyle(datefrom, dateto, phuongthuctiepcan, Status, UserId, PortalId)
        End Function
        '------------------------------------------'
        'Public Function _Info_StaticUser_TelesaleCountTyle_Index(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, UserId As Integer, PortalId As Integer) As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance._Info_StaticUser_TelesaleCountTyle_Index(datefrom, dateto, phuongthuctiepcan, UserId, PortalId), GetType(StudentFollowInfo))
        'End Function
        ''------------------------------------------'
        'Public Function _Info_StaticUser_UserTelesaleKhachHang(StudentId As Integer, datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, status As Integer, UserId As Integer, PortalId As Integer) As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance._Info_StaticUser_UserTelesaleKhachHang(StudentId, datefrom, dateto, phuongthuctiepcan, status, UserId, PortalId), GetType(StudentFollowInfo))
        'End Function





        '------------------------------------------'
        'Public Function _Info_StaticUser_UserTelesaleGroupKhachHang(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer, PageIndex As Integer, PageSize As Integer) As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance._Info_StaticUser_UserTelesaleGroupKhachHang(datefrom, dateto, phuongthuctiepcan, Status, UserId, nguon, PortalId, PageIndex, PageSize), GetType(StudentFollowInfo))
        'End Function
        ''------------------------------------------'
        'Public Function _Info_StaticUser_UserTelesaleGroupKhachHang_CountCuocGoi(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer) As Integer
        '    Return DataProvider.Instance._Info_StaticUser_UserTelesaleGroupKhachHang_CountCuocGoi(datefrom, dateto, phuongthuctiepcan, Status, UserId, nguon, PortalId)
        'End Function
        ''------------------------------------------'
        'Public Function _Info_StaticUser_UserTelesaleGroupKhachHang_CountKhachHang(datefrom As DateTime, dateto As DateTime, phuongthuctiepcan As Integer, Status As Integer, UserId As Integer, nguon As Integer, PortalId As Integer) As Integer
        '    Return DataProvider.Instance._Info_StaticUser_UserTelesaleGroupKhachHang_CountKhachHang(datefrom, dateto, phuongthuctiepcan, Status, UserId, nguon, PortalId)
        'End Function
        '------------------------------------------'






        'Public Function _Info_StaticUser_TrangThaiCountTyle(datefrom As DateTime, dateto As DateTime, Status As Integer, UserId As Integer, PortalId As Integer) As Integer
        '    Return DataProvider.Instance._Info_StaticUser_TrangThaiCountTyle(datefrom, dateto, Status, UserId, PortalId)
        'End Function
        ''------------------------------------------'
        'Public Function _Info_StaticUser_TrangThaiCountTyle_Index(datefrom As DateTime, dateto As DateTime, Status As Integer, UserId As Integer, PortalId As Integer) As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance._Info_StaticUser_TrangThaiCountTyle_Index(datefrom, dateto, Status, UserId, PortalId), GetType(StudentFollowInfo))
        'End Function
        ''------------------------------------------'
        'Public Function _Info_StaticUser_Permission(datefrom As DateTime, dateto As DateTime, UserId As Integer, PortalId As Integer) As Integer
        '    Return DataProvider.Instance._Info_StaticUser_Permission(datefrom, dateto, UserId, PortalId)
        'End Function
        ''------------------------------------------'
        'Public Function _Info_StaticUser_Permission_New(datefrom As DateTime, dateto As DateTime, UserId As Integer, PortalId As Integer) As Integer
        '    Return DataProvider.Instance._Info_StaticUser_Permission_New(datefrom, dateto, UserId, PortalId)
        'End Function
    End Class

End Namespace