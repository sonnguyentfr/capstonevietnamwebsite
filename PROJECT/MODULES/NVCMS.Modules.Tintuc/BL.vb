Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports aejw.Network
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Users
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.Users
Public Class BL
    Public Shared regEmailCacheKey As String = "regEmail"
    Public Shared regSMSCacheKey As String = "regSMS"
    Public Shared regCuttingNumber As Integer = 300
    Public Shared NewsCatList As String = "NewsCatList"
    Public Shared NewsTinMoiNhat As String = "NewsTinMoiNhat"
    Public Shared NewsHomeHotSite As String = "NewsHomeHotSite"
    Public Shared NewsTinNong As String = "NewsTinNong"
    Public Shared NewsXuHuongDoc As String = "NewsXuHuongDoc"
    Public Shared NewsHomeCat As String = "NewsHomeCat"
    Public Shared NewsDetailCache As String = "NewsDetailCache"
    Public Shared settingAnhLuuTruVirtual As String = "settingAnhLuuTruVirtual"
    Public Shared settingAnhLuuTruPhysical As String = "settingAnhLuuTruPhysical"
    Public Shared settingFlashVirtual As String = "settingFlashVirtual"
    Public Shared settingFlashPhysical As String = "settingFlashPhysical"
    Public Shared settingMediaPathVirtual As String = "settingMediaPathVirtual"
    Public Shared settingMediaPathPhysical As String = "settingMediaPathPhysical"
    Public Shared settingMediaLuuTruVirtual As String = "settingMediaLuuTruVirtual"
    Public Shared settingMediaLuuTruPhysical As String = "settingMediaLuuTruPhysical"
    Public Shared settingBackupPathVirtual As String = "settingBackupPathVirtual"
    Public Shared settingBackupPathPhysical As String = "settingBackupPathPhysical"
    Public Shared settingBaiHatVirtual As String = "settingBaiHatVirtual"
    Public Shared settingBaiHatPhysical As String = "settingBaiHatPhysical"
    Public Shared settingVideoVirtual As String = "settingVideoVirtual"
    Public Shared settingVideoPhysical As String = "settingVideoPhysical"
    Public Shared settingSXCTVirtual As String = "settingSXCTVirtual"
    Public Shared settingSXCTPhysical As String = "settingSXCTPhysical"
    Public Shared settingFTPVirtual As String = "settingFTPVirtual"
    Public Shared settingFTPPhysical As String = "settingFTPPhysical"
    Public Shared settingDalet As String = "settingDalet"
    Public Shared settingDalet2XML As String = "settingDalet2XML"
    Public Shared settingNetia As String = "settingNetia"
    Public Shared settingNetia2XML As String = "settingNetia2XML"
    Public Shared settingMultiMediaCopyPath1 As String = "settingMultiMediaCopyPath1"
    Public Shared settingMultiMediaCopyPath2 As String = "settingMultiMediaCopyPath2"
    Public Shared settingMultiMediaCopyPath3 As String = "settingMultiMediaCopyPath3"
    Public Shared settingAlertRequestDuration As String = "settingAlertRequestDuration"
    Public Shared settingAutoSaveRequestDuration As String = "settingAutoSaveRequestDuration"
    Public Shared settingDataRequestDuration As String = "settingDataRequestDuration"
    Public Shared cacheNewsNoteByNewId As String = "cacheNewsNoteByNewId"

    Public Shared pageDanhSachTin As String = "/quan-tri/tin-tuc/dang-soan-thao"
    Public Shared pageDanhSachTinId As Integer = 2038

    Public Shared pageDanhSachTinChoPheDuyet As String = "/quan-tri/tin-tuc/cho-phe-duyet"
    Public Shared pageDanhSachTinChoPheDuyetId As Integer = 2040

    Public Shared pageDanhSachTinChoXuatBan As String = "/quan-tri/tin-tuc/cho-xuat-ban"
    Public Shared pageDanhSachTinChoXuatBanId As Integer = 2041

    Public Shared pageDanhSachTinBiTraLai As String = "/quan-tri/tin-tuc/cho-xuat-ban"
    Public Shared pageDanhSachTinBiTraLaiId As Integer = 3060

    Public Shared pageDanhSachTinXuatBan As String = "/quan-tri/tin-tuc/da-xuat-ban"
    Public Shared pageDanhSachTinXuatBanId As Integer = 2042

    Public Shared pagePheDuyetXB As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/duyet-xuat-ban"
    Public Shared pageSuaPheDuyetXB As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-duyet-xuat-ban"
    Public Shared pageSuaPheDuyetXBAnh As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-duyet-xuat-ban-anh"
    Public Shared pagePheDuyetXBId As Integer = 2053

    Public Shared pageDaXuatBan As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/bai-da-xuat-ban"
    Public Shared pageDaXuatBanSua As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-da-xuat-ban"
    Public Shared pageDaXuatBanId As Integer = 3093

    Public Shared pageHuyXuatBan As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/bai-huy-xuat-ban"
    'Public Shared pageSuaPheDuyetXB As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-duyet-xuat-ban"
    Public Shared pageHuyXuatBanId As Integer = 4093

    Public Shared pageTheoDoiTinBai As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/theo-doi-tin-bai"
    Public Shared pageTheoDoiTinBaiId As Integer = 4094
    Public Shared pageSuaTheoDoiTinBai As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-theo-doi-tin-bai"


    Public Shared pageThemMoiTin As String = "/quantri/quantritintuc/themmoitin.html"
    Public Shared pageQuanTriTinBai As String = "/quantri/quantritintuc/quantritinbai.html"
    Public Shared pageNhoChinhDuyet As String = "/quantri/quantritintuc/nhochinhduyet.html"

    Public Shared pagePheDuyet As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/duyet-tin-bai"
    Public Shared pageSuaPheDuyet As String = "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-duyet-bien-tap"
    Public Shared pagePheDuyetId As Integer = 2052
    'Video
    Public Shared pageDanhSachVideo As String = "/quan-tri/video/dang-soan-thao"
    Public Shared pageDanhSachDuyetVideo As String = "/quan-tri/video-cap-cao/cho-xuat-ban"
    Public Shared pageDanhSachVideoDaXuatBan As String = "/quan-tri/video-cap-cao/da-xuat-ban"
    '
    Public Shared settingListHot_PageSize = "settingListHot_PageSize"
    Public Shared settingList_ImgSizeHOT = "settingList_ImgSizeHOT"
    Public Shared settingList_PageSize = "settingList_PageSize"
    Public Shared settingList_ImgSize = "settingList_ImgSize"
    Public Shared settingList_Template = "settingList_Template"
    Public Shared settingList_Time = "settingList_Time"
    Public Shared settingList_Top = "settingList_Top"
    Public Shared settingList_ViewType = "settingList_ViewType"
    Public Shared settingList_Order = "settingList_Order"
    Public Shared settingList_SizeDes = "settingList_SizeDes"
    Public Shared settingList_Expired = "settingList_Expired"
    Public Shared settingList_ShowPage = "settingList_ShowPage"
    Public Shared settingDetails_More = "settingDetails_More"
    Public Shared settingDetails_MorePage = "settingDetails_MorePage"
    Public Shared settingDetails_Other = "settingDetails_Other"
    Public Shared settingDetails_Template = "settingDetails_Template"
    Public Shared settingDetails_Allow = "settingDetails_Allow"
    Public Shared settingDetails_Comment = "settingDetails_Comment"

    Public Shared settingView_Cate = "settingViewCate"
    Public Shared settingView_CateHOT = "settingViewCateHOT"
    Public Shared settingView_Type = "settingViewType"
    Public Shared settingView_Total = "settingViewTop"
    Public Shared settingView_Template = "settingViewTemplate"
    Public Shared settingView_ImgSize = "settingViewImgSize"
    Public Shared settingView_SizeDes = "settingSizeDes"
    Public Shared settingViewTop_Categories = "settingTopCategories"
    Public Shared settingView_SizeTitle = "settingSizeTitle"
    'Form
    Public Shared settingForm_MailOK = "settingForm_MailOK"
    Public Shared settingForm_MailNhan = "settingForm_MailNhan"
    Public Shared settingForm_MailNhanTieude = "settingForm_MailNhanTieude"
    Public Shared settingForm_MailStyle = "settingForm_MailStyle"
    'Bieu may
    Public Shared settingBieuMau_Type = "settingBieuMau_Type"
    Public Shared settingBieuMau_DanhMuc = "settingBieuMau_DanhMuc"
    '--
    Public Shared msgProcessHuyGuiPheDuyet As String = "Triệu hồi"
    Public Shared msgProcessGuiPheDuyet As String = "Gửi phê duyệt"
    Public Shared msgProcessPheDuyet As String = "Phê duyệt"
    Public Shared msgProcessUyNhiemXB As String = "Ủy nhiệm xuất bản"
    Public Shared msgProcessXuatBan As String = "Xuất bản"
    Public Shared msgProcessHenGioXuatBan As String = "Hẹn giờ xuất bản"
    Public Shared msgProcessHuyXB As String = "Hủy xuất bản"
    Public Shared msgProcessTraLai As String = "Trả lại "
    Public Shared msgProcessXoa As String = "Xóa tin bài"
    Public Shared msgProcessXoaCSDL As String = "Xóa khỏi CSDL"
    Public Shared msgProcessEditByPublisher As String = "Lãnh đạo ban Chỉnh sửa"
    Public Shared msgProcessEditByCreator As String = "Người đăng chỉnh sửa"
    Public Shared msgProcessEditByApprover As String = "Lãnh đạo phòng chỉnh sửa"
    Public Shared msgProcessCreated As String = "Khởi tạo"
    Public Shared msgProcessRegisterd As String = "Đăng ký tin bài"
    Public Shared msgProcessAdminEdit As String = "Quản trị chỉnh sửa"
    Public Shared msgProcessAdminRestore As String = "Khôi phục tin bài"
    Public Shared msgProcessSending4Approval As String = "Trình duyệt tin tới: "
    Public Shared msgProcessSending4Publish As String = "Trình xuất bản tới: "
    Public Shared msgProcessDatBaiTTTin As String = "Đặt bài"
    Public Shared msgProcessGiaoXL As String = "Giao xử lý tới phòng ban: "
    Public Shared msgProcessGiaoXLPhongVien As String = "Giao xử lý cho phóng viên: "
    Public Shared msgProcessUnlockNews As String = "Mở khóa tin bài"

    Public Shared tabNhoXL As Integer = 149
    Public Shared tabChoXB As Integer = 102
    Public Shared tabChoPheDuyet As Integer = 76
    Public Shared tabThemMoi As Integer = 111
    Public Shared tabAlbum As Integer = 116
    Public Shared tabTags As Integer = 139
    Public Shared tabDetailMuzik As Integer = 1214
    Public Shared tabDetailVideo As Integer = 165
    Public Shared tabDanhMuc As Integer = 1242


    Public Shared minDateV As DateTime = "01/01/2000"
    Public Shared maxDateV As DateTime = "01/01/2100"
    Public Shared filesDomain As String = "https://capstonevietnam-fileserver.nvcms.net"
    Public Shared qsTimKiem As String() = New String() {"sch", "key", "catid", "type", "from", "to", "pageNo", "kind", "pbid", "uid", "status", "isactive", "source"}

    Public Shared Function GetMSGByStatus(targetStatus) As String
        Dim strResult As String = String.Empty

        Select Case targetStatus
            Case NewsStatus.ChoPheDuyet
                strResult = msgProcessSending4Approval
            Case NewsStatus.ChoXuatBan
                strResult = msgProcessSending4Publish
        End Select

        Return strResult
    End Function
    Public Shared Function GetNguoiNhanEmail(ByVal Portalid As Integer) As ArrayList
        Dim arrReturn As New ArrayList
        arrReturn = DataCache.GetCache(regEmailCacheKey)
        If arrReturn Is Nothing Then
            Dim arrTemp As ArrayList = UserController.GetUsers(Portalid)
            For Each obj As UserInfo In arrTemp
                If Boolean.Parse(obj.Profile.GetPropertyValue("RegEmail")) Then
                    arrReturn.Add(obj)
                End If
            Next

            DataCache.SetCache(regEmailCacheKey, arrReturn)
        End If

        Return arrReturn
    End Function
    Public Shared Function GetNguoiNhanSMS(ByVal Portalid As Integer) As ArrayList
        Dim arrReturn As New ArrayList
        arrReturn = DataCache.GetCache(regSMSCacheKey)
        If arrReturn Is Nothing Then
            Dim arrTemp As ArrayList = UserController.GetUsers(Portalid)
            For Each obj As UserInfo In arrTemp
                If Boolean.Parse(obj.Profile.GetPropertyValue("RegSMS")) Then
                    arrReturn.Add(obj)
                End If
            Next

            DataCache.SetCache(regSMSCacheKey, arrReturn)
        End If

        Return arrReturn
    End Function
    Public Shared Function GetMappingTabIDByCategoryID(ByVal categoryid As Integer) As Integer
        Try
            If categoryid > 0 Then
                Dim strCacheKey As String
                strCacheKey = "CategoryTabID:" & categoryid
                Dim strTabID As String
                strTabID = DataCache.GetCache(strCacheKey)
                If strTabID = "" Then
                    Dim ctlCategory As New NV_NewsCategoriesController
                    Dim objCategory As NV_NewsCategoriesInfo = ctlCategory.GetByID(categoryid)
                    If Not objCategory Is Nothing Then
                        strTabID = objCategory.TabID
                    End If
                    DataCache.SetCache(strCacheKey, strTabID)
                End If
                Return strTabID
            Else
                Return "-1"
            End If
        Catch ex As Exception
            Return "-1"
        End Try
    End Function
    Public Shared Function GetMappingTabIDDetailByCategoryID(ByVal categoryid As Integer) As Integer
        Try
            If categoryid > 0 Then
                Dim strCacheKey As String
                strCacheKey = "CategoryTabIDdetail:" & categoryid
                Dim strTabID As String
                strTabID = DataCache.GetCache(strCacheKey)
                If strTabID = "" Then
                    Dim ctlCategory As New NV_NewsCategoriesController
                    Dim objCategory As NV_NewsCategoriesInfo = ctlCategory.GetByID(categoryid)
                    If Not objCategory Is Nothing Then
                        strTabID = objCategory.TabIdDetail
                    End If
                    DataCache.SetCache(strCacheKey, strTabID)
                End If
                Return strTabID
            Else
                Return "-1"
            End If
        Catch ex As Exception
            Return "-1"
        End Try
    End Function
    Public Shared Function GetMappingCategoryIDByTabID(ByVal tabid As Integer) As Integer
        Try
            If tabid > 0 Then
                Dim strCacheKey As String
                strCacheKey = "TabCategoryID:" & tabid
                Dim strCategoryID As String
                strCategoryID = DataCache.GetCache(strCacheKey)
                If strCategoryID = "" Then
                    Dim ctlCategory As New NV_NewsCategoriesController
                    Dim objCategory As NV_NewsCategoriesInfo = ctlCategory.GetByTabID(tabid)
                    If Not objCategory Is Nothing Then
                        strCategoryID = objCategory.CategoryID
                    End If
                    DataCache.SetCache(strCacheKey, strCategoryID)
                End If
                Return strCategoryID
            Else
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Shared Function GetNameByUserId(ByVal portalid As Integer, ByVal UserId As Integer) As String
        Try
            If UserId > 0 Then
                Dim strCacheKey As String
                strCacheKey = "UserInfo:" & UserId
                Dim strUserName As String
                strUserName = DataCache.GetCache(strCacheKey)
                If strUserName = "" Then
                    Dim objUser As UserInfo = UserController.GetUserById(portalid, UserId)
                    If Not objUser Is Nothing Then
                        With objUser
                            strUserName = .DisplayName
                            DataCache.SetCache(strCacheKey, strUserName)
                        End With
                    End If
                End If
                Return strUserName
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Shared Function GetTenLanhDaoXB(ByVal portalid As Integer, ByVal UserId As Integer) As String
        Try
            If UserId > 0 Then
                Dim strCacheKey As String
                strCacheKey = "LDXuatBan:" & UserId
                Dim strUserName As String
                strUserName = DataCache.GetCache(strCacheKey)
                If strUserName = "" Then
                    'Dim objUser As UserInfo = UserController.GetUserById(portalid, UserId)
                    Dim objUser As UserInfo = UserController.GetUserById(portalid, UserId)
                    If Not objUser Is Nothing Then
                        With objUser
                            strUserName = objUser.DisplayName
                            DataCache.SetCache(strCacheKey, strUserName)
                        End With
                    End If

                End If
                Return strUserName
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Shared Function IsLanhDaoBan(ByVal portalid As Integer, ByVal UserId As Integer) As Boolean
        Dim iPhongBan As Integer = GetPhongBanIdByUserId(portalid, UserId)
        If iPhongBan = 1 Then 'Ban Giam Doc
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetButDanh(ByVal portalid As Integer, ByVal UserId As Integer) As String
        Try
            If UserId > 0 Then
                Dim strCacheKey As String
                strCacheKey = "ButDanh:" & UserId
                Dim strButDanh As String = DataCache.GetCache(strCacheKey)
                If String.IsNullOrEmpty(strButDanh) Then
                    strButDanh = ""
                End If
                If strButDanh = "" Then
                    Dim objUserInfo As UserInfo = UserController.GetUserById(portalid, UserId)
                    If Not objUserInfo Is Nothing Then

                        strButDanh = objUserInfo.Profile.GetPropertyValue("interpret")
                        DataCache.SetCache(strCacheKey, strButDanh)
                    End If
                End If
                Return strButDanh
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Shared Function GetUserName(ByVal portalid As Integer, ByVal userid As Integer) As String
        Dim strCacheKey As String
        strCacheKey = "ByUser:" & userid
        Dim strResult As String = String.Empty
        strResult = DataCache.GetCache(strCacheKey)
        If strResult = "" Then
            Dim obj As UserInfo = UserController.GetUserById(portalid, userid)
            If Not obj Is Nothing Then
                strResult = obj.Username
                DataCache.SetCache(strCacheKey, strResult)
            End If
        End If

        Return strResult
    End Function
    Public Shared Function GetDanhSachNguoiNhan(ByVal PortalId As Integer, ByVal sNguoiNhan As String) As String
        Dim iNguoiNhan As String() = sNguoiNhan.Split(";")
        Dim sReturn As String = ""
        For Each s As String In iNguoiNhan
            If IsNumeric(s) Then
                Dim objUser As UserInfo = UserController.GetUserById(PortalId, s)
                If Not objUser Is Nothing Then
                    If sReturn <> "" Then
                        sReturn = sReturn & ", " & objUser.DisplayName
                    Else
                        sReturn = objUser.DisplayName
                    End If
                End If
            End If
        Next
        Return sReturn
    End Function
    Public Shared Function GetLanhDaoPhongBan(ByVal portalid As Integer, ByVal phongbanid As Integer) As String
        Dim strResult As String = String.Empty

        Dim arrNguoiDungPhongBan As ArrayList = New PhongBanNguoiDungController().GetByPhongBan(phongbanid, portalid)
        For Each objUser As PhongBanNguoiDungInfo In arrNguoiDungPhongBan
            If objUser.LaLanhDao = True Then
                strResult = BL.AppendPatternToString(strResult, objUser.UserId.ToString, ";")
            End If
        Next

        Return strResult
    End Function
    Public Shared Function GetCreatedInfo(ByVal portalid As Integer, ByVal tacgia As Integer, ByVal Createdate As DateTime) As String
        Dim sResult As String = String.Empty
        sResult = BL.AppendPatternToString(sResult, BL.GetNameByUserId(portalid, tacgia), ",")

        Return sResult + " (" + Createdate.ToString("HH:mm dd/MM/yyyy") + ")"
    End Function
    Public Shared Function GetSend2ApprovalInfo(ByVal portalid As Integer, ByVal tacgia As Integer, ByVal sendingRequestDate As DateTime) As String
        Dim sResult As String = String.Empty
        sResult = BL.AppendPatternToString(sResult, BL.GetNameByUserId(portalid, tacgia), ",")

        Return sResult + " (" + sendingRequestDate.ToString("HH:mm dd/MM/yyyy") + ")"
    End Function
    Public Shared Function GetApprovalInfo(ByVal portalid As Integer, ByVal approver As Integer, ByVal approvedDate As DateTime) As String
        Return BL.GetNameByUserId(portalid, approver) + " (" + approvedDate.ToString("HH:mm dd/MM/yyyy") + ")"
    End Function
    Public Shared Function GetLDXLInfo(ByVal portalid As Integer, ByVal publisher As Integer, ByVal publishedDate As DateTime) As String
        Return BL.GetTenLanhDaoXB(portalid, publisher) + " (" + publishedDate.ToString("HH:mm dd/MM/yyyy") + ")"
    End Function
    Public Shared Function GetPublishedInfo(ByVal portalid As Integer, ByVal publisher As Integer, ByVal publishedDate As DateTime) As String
        Return BL.GetNameByUserId(portalid, publisher) + " / " + publishedDate.ToString("HH:mm dd/MM/yyyy")
    End Function
    Public Shared Function GetPhongBanTenViettat(ByVal phongbanid As Integer) As String
        Dim strCacheKey As String
        strCacheKey = "PhongBanViettat:" & phongbanid
        Dim strResult As String = DataCache.GetCache(strCacheKey)
        If String.IsNullOrEmpty(strResult) Then
            strResult = String.Empty
        End If
        If strResult = "" Then
            Dim ctlPhongBan As New PhongBanController
            Dim objPhongBan As PhongBanInfo = ctlPhongBan.GetById(phongbanid)
            If Not objPhongBan Is Nothing Then
                If Not String.IsNullOrEmpty(objPhongBan.TenVietTat) Then
                    strResult = objPhongBan.TenVietTat
                    DataCache.SetCache(strCacheKey, strResult)
                End If
            End If
        End If

        Return strResult
    End Function
    Public Shared Function GetTenPhongBan(ByVal phongbanid As Integer) As String
        Dim strCacheKey As String
        strCacheKey = "TenPhongBan:" & phongbanid
        Dim strResult As String = DataCache.GetCache(strCacheKey)
        If String.IsNullOrEmpty(strResult) Then
            strResult = String.Empty
        End If
        If strResult = "" Then
            Dim ctlPhongBan As New PhongBanController
            Dim objPhongBan As PhongBanInfo = ctlPhongBan.GetById(phongbanid)
            If Not objPhongBan Is Nothing Then
                If Not String.IsNullOrEmpty(objPhongBan.TenPhongBan) Then
                    strResult = objPhongBan.TenPhongBan
                    DataCache.SetCache(strCacheKey, strResult)
                End If
            End If
        End If

        Return strResult
    End Function
    Public Shared Function GetTenParentPhongBan(ByVal phongbanid As Integer) As String
        Dim strCacheKey As String
        strCacheKey = "TenParentPhongBan:" & phongbanid
        Dim strResult As String = DataCache.GetCache(strCacheKey)
        If String.IsNullOrEmpty(strResult) Then
            strResult = String.Empty
        End If
        If strResult = "" Then
            Dim ctlPhongBan As New PhongBanController
            Dim objPhongBan As PhongBanInfo = ctlPhongBan.GetById(phongbanid)
            If Not objPhongBan Is Nothing Then
                Dim objParent As PhongBanInfo = ctlPhongBan.GetById(objPhongBan.ParentId)
                If Not objParent Is Nothing Then
                    If Not String.IsNullOrEmpty(objParent.TenPhongBan) Then
                        strResult = objParent.TenPhongBan
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If
            End If
        End If

        Return strResult
    End Function
    Public Shared Function GetPhongBanIdByUserId(ByVal portalid As Integer, ByVal UserId As Integer) As Integer
        If UserId > 0 Then
            Dim strCacheKey As String
            strCacheKey = "PhongBanInfo:" & UserId
            Dim strPhongBan As String = "-1"
            strPhongBan = DataCache.GetCache(strCacheKey)
            If strPhongBan = "-1" OrElse strPhongBan = "" Then
                Dim ctl As New PhongBanNguoiDungController
                Dim arr As ArrayList = ctl.GetByNguoiDung(UserId, portalid)
                If Not arr Is Nothing AndAlso arr.Count > 0 Then
                    Dim obj As PhongBanNguoiDungInfo = DirectCast(arr(0), PhongBanNguoiDungInfo)
                    strPhongBan = obj.PhongBan
                    DataCache.SetCache(strCacheKey, strPhongBan)
                End If
            End If
            Return strPhongBan
        Else
            Return "-1"
        End If
    End Function
    Public Shared Function GetArrIDPhongBanById(ByVal portalid As Integer, ByVal phongbanId As Integer) As String
        Dim strCacheKey As String
        strCacheKey = "PhongBanArrIDs:" & phongbanId
        Dim strArrPhongBan As String = DataCache.GetCache(strCacheKey)
        If String.IsNullOrEmpty(strArrPhongBan) Then
            strArrPhongBan = String.Empty
        End If
        If strArrPhongBan = "" Then
            Dim ctl As New PhongBanController
            Dim arrAll As ArrayList = ctl.GetAll(portalid)
            GetRecusivePhongBanIDs(portalid, arrAll, phongbanId, strArrPhongBan)
            DataCache.SetCache(strCacheKey, strArrPhongBan)
        End If

        Return strArrPhongBan
    End Function
    Public Shared Sub GetRecusivePhongBanIDs(portalId As Integer, ByVal arrPhongBan As ArrayList, ByVal ParentPhongBanId As Integer, ByRef arrReturn As String)
        arrReturn = BL.AppendPatternToString(arrReturn, ParentPhongBanId, ",")
        For Each objPhongBan As PhongBanInfo In arrPhongBan
            If objPhongBan.ParentId = ParentPhongBanId Then
                arrReturn = BL.AppendPatternToString(arrReturn, objPhongBan.Id, ",")
                ' kiem tra xem co doi tuong con hay khong
                Dim ctl As New PhongBanController
                If ctl.GetByParentId(portalId, objPhongBan.Id).Count > 0 Then
                    GetRecusivePhongBanIDs(portalId, arrPhongBan, objPhongBan.Id, arrReturn)
                End If
            End If
        Next
    End Sub
    Public Shared Function GetPhongBanAnhDaiDien(ByVal phongbanid As Integer) As String
        Dim strCacheKey As String
        strCacheKey = "PhongBanDaiDien:" & phongbanid
        Dim strResult As String = DataCache.GetCache(strCacheKey)
        If String.IsNullOrEmpty(strResult) Then
            strResult = ""
        End If
        If strResult = "" Then
            Dim ctlPhongBan As New PhongBanController
            Dim objPhongBan As PhongBanInfo = ctlPhongBan.GetById(phongbanid)
            If Not objPhongBan Is Nothing Then
                strResult = objPhongBan.LanguageId 'Anh dai dien
                DataCache.SetCache(strCacheKey, strResult)
            End If
        End If

        Return strResult
    End Function
    Public Shared Function StripHTML(ByVal source As String) As String
        Try
            Dim result As String

            ' Remove HTML Development formatting
            ' Replace line breaks with space
            ' because browsers inserts space
            result = source.Replace(vbCr, " ")
            ' Replace line breaks with space
            ' because browsers inserts space
            result = result.Replace(vbLf, " ")
            ' Remove step-formatting
            result = result.Replace(vbTab, String.Empty)
            ' Remove repeating spaces because browsers ignore them
            result = System.Text.RegularExpressions.Regex.Replace(result, "( )+", " ")

            ' Remove the header (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*head([^>])*>", "<head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*head( )*>)", "</head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<head>).*(</head>)", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' remove all scripts (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*script([^>])*>", "<script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*script( )*>)", "</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'result = System.Text.RegularExpressions.Regex.Replace(result,
            '         @"(<script>)([^(<script>\.</script>)])*(</script>)",
            '         string.Empty,
            '         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<script>).*(</script>)", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' remove all styles (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*style([^>])*>", "<style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*style( )*>)", "</style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<style>).*(</style>)", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' insert tabs in spaces of <td> tags
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*td([^>])*>", vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' insert line breaks in places of <BR> and <LI> tags
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*br( )*>", vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*li( )*>", vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' insert line paragraphs (double line breaks) in place
            ' if <P>, <DIV> and <TR> tags
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*div([^>])*>", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*tr([^>])*>", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*p([^>])*>", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' Remove remaining tags like <a>, links, images,
            ' comments etc - anything that's enclosed inside < >
            result = System.Text.RegularExpressions.Regex.Replace(result, "<[^>]*>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' replace special characters:
            result = System.Text.RegularExpressions.Regex.Replace(result, " ", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            result = System.Text.RegularExpressions.Regex.Replace(result, "•", " * ", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "‹", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "›", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "™", "(tm)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "⁄", "/", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "<", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, ">", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "©", "(c)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "®", "(r)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Remove all others. More can be added, see
            ' http://hotwired.lycos.com/webmonkey/reference/special_characters/
            result = System.Text.RegularExpressions.Regex.Replace(result, "&(.{2,6});", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' for testing
            'System.Text.RegularExpressions.Regex.Replace(result,
            '       this.txtRegex.Text,string.Empty,
            '       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            ' make line breaking consistent
            result = result.Replace(vbLf, vbCr)

            ' Remove extra line breaks and tabs:
            ' replace over 2 breaks with 2 and over 4 tabs with 4.
            ' Prepare first to remove any whitespaces in between
            ' the escaped characters and remove redundant tabs in between line breaks
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")( )+(" & vbCr & ")", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbTab & ")( )+(" & vbTab & ")", vbTab & vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbTab & ")( )+(" & vbCr & ")", vbTab & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")( )+(" & vbTab & ")", vbCr & vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Remove redundant tabs
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")(" & vbTab & ")+(" & vbCr & ")", vbCr & vbCr, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Remove multiple tabs following a line break with just one tab
            result = System.Text.RegularExpressions.Regex.Replace(result, "(" & vbCr & ")(" & vbTab & ")+", vbCr & vbTab, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            ' Initial replacement target string for line breaks
            Dim breaks As String = vbCr & vbCr & vbCr
            ' Initial replacement target string for tabs
            Dim tabs As String = vbTab & vbTab & vbTab & vbTab & vbTab
            For index As Integer = 0 To result.Length - 1
                result = result.Replace(breaks, vbCr & vbCr)
                result = result.Replace(tabs, vbTab & vbTab & vbTab & vbTab)
                breaks = breaks + vbCr
                tabs = tabs + vbTab
            Next

            ' That's it.
            Return result
        Catch
            Return source
        End Try
    End Function
    Public Shared Function FormatText(ByVal sStr As String, Optional ByVal number As Integer = 100) As String
        sStr = sStr.Replace("_"c, " "c)
        sStr = sStr.Replace("%20", " "c)
        If number >= sStr.Length Then
            Return sStr
        End If
        Dim last As Integer = sStr.LastIndexOf(" "c, number)
        If Not Null.IsNull(last) Then
            Return sStr.Substring(0, last).Replace("_"c, " "c) + "..."
            'Return sStr.Substring(0, last).Replace("_"c, " "c) ' Setting của riêng BLĐ
        Else
            Dim sResul As String = String.Empty
            If sStr.Length > number Then
                sResul = sStr.Substring(0, number - 1)
            Else
                sResul = sStr
            End If
            Return sResul
        End If
    End Function
    Public Shared Function FormatDate(ByVal d As DateTime) As String
        Return d.ToString("HH:mm dd/MM/yyyy")
    End Function
    Public Shared Function ConvertTiengVietCoDauThanhKhongDauV1(ByVal sTiengVietCoDau As String) As String
        '---------------------------------a^
        sTiengVietCoDau = sTiengVietCoDau.Replace("ấ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ầ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ẩ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ẫ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ậ", "a")
        '---------------------------------A^
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ấ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ầ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ẩ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ẫ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ậ", "A")
        '---------------------------------a(
        sTiengVietCoDau = sTiengVietCoDau.Replace("ắ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ằ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ẳ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ẵ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ặ", "a")
        '---------------------------------A(
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ắ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ằ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ẳ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ẵ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ặ", "A")
        '---------------------------------a
        sTiengVietCoDau = sTiengVietCoDau.Replace("á", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("à", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ả", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ã", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ạ", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("â", "a")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ă", "a")
        '---------------------------------A
        sTiengVietCoDau = sTiengVietCoDau.Replace("Á", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("À", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ả", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ã", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ạ", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Â", "A")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ă", "A")
        '---------------------------------e^
        sTiengVietCoDau = sTiengVietCoDau.Replace("ế", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ề", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ể", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ễ", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ệ", "e")
        '---------------------------------E^
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ế", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ề", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ể", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ễ", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ệ", "E")
        '---------------------------------e
        sTiengVietCoDau = sTiengVietCoDau.Replace("é", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("è", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ẻ", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ẽ", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ẹ", "e")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ê", "e")
        '---------------------------------E
        sTiengVietCoDau = sTiengVietCoDau.Replace("É", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("È", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ẻ", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ẽ", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ẹ", "E")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ê", "E")
        '---------------------------------i
        sTiengVietCoDau = sTiengVietCoDau.Replace("í", "i")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ì", "i")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỉ", "i")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ĩ", "i")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ị", "i")
        '---------------------------------I
        sTiengVietCoDau = sTiengVietCoDau.Replace("Í", "I")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ì", "I")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ỉ", "I")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ĩ", "I")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ị", "I")
        '---------------------------------o^
        sTiengVietCoDau = sTiengVietCoDau.Replace("ố", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ồ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ổ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỗ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ộ", "o")
        '---------------------------------O^
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ố", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ồ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ổ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ô", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ộ", "O")
        '---------------------------------o*
        sTiengVietCoDau = sTiengVietCoDau.Replace("ớ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ờ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ở", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỡ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ợ", "o")
        '---------------------------------O*
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ớ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ờ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ở", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ỡ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ợ", "O")
        '---------------------------------u*
        sTiengVietCoDau = sTiengVietCoDau.Replace("ứ", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ừ", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ử", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ữ", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ự", "u")
        '---------------------------------U*
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ứ", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ừ", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ử", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ữ", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ự", "U")
        '---------------------------------y
        sTiengVietCoDau = sTiengVietCoDau.Replace("ý", "y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỳ", "y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỷ", "y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỹ", "y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỵ", "y")
        '---------------------------------Y
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ý", "Y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ỳ", "Y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ỷ", "Y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ỹ", "Y")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ỵ", "Y")
        '---------------------------------DD
        sTiengVietCoDau = sTiengVietCoDau.Replace("Đ", "D")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Đ", "D")
        sTiengVietCoDau = sTiengVietCoDau.Replace("đ", "d")
        '---------------------------------o
        sTiengVietCoDau = sTiengVietCoDau.Replace("ó", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ò", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ỏ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("õ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ọ", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ô", "o")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ơ", "o")
        '---------------------------------O
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ó", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ò", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ỏ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Õ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ọ", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ô", "O")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ơ", "O")
        '---------------------------------u
        sTiengVietCoDau = sTiengVietCoDau.Replace("ú", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ù", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ủ", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ũ", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ụ", "u")
        sTiengVietCoDau = sTiengVietCoDau.Replace("ư", "u")
        '---------------------------------U
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ú", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ù", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ủ", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ũ", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ụ", "U")
        sTiengVietCoDau = sTiengVietCoDau.Replace("Ư", "U")
        '--------------------------------- 
        sTiengVietCoDau = sTiengVietCoDau.Trim

        'Thay thế dấu trắng bằng - để truyền trên url
        sTiengVietCoDau = sTiengVietCoDau.Replace("  ", " ")
        sTiengVietCoDau = sTiengVietCoDau.Replace(" ", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("""", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("(", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace(")", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace(":", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace(",", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("?", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace(".", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("--", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("/", "-")
        sTiengVietCoDau = Regex.Replace(sTiengVietCoDau, "[""|:|?|,|.|--]", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("--", "-")

        Return sTiengVietCoDau
    End Function
    ''' <summary>
    ''' Thay thế tiếng xâu tiếng việt có dấu thành không dấu phục vụ cho cỗ máy tìm kiếm V2
    ''' </summary>
    ''' <param name="sTiengVietCoDau"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertTiengVietCoDauThanhKhongDauV2(ByVal sTiengVietCoDau As String) As String
        Dim r As Regex = New Regex("[ĂÂẠÃẢÀÁăâạãảàáẶẴẲẰẮặẵẳằắẬẪẨẦẤậẫẩầấ]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "a")

        r = New Regex("[ÊẸẼẺÈÉêẹỄẽẾỂếễềỆệèểỀéẻ]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "e")

        r = New Regex("[ỊỈĨÌÍịĩỉìí]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "i")

        r = New Regex("[ỰỮỬỪỨựữửừứ]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "u")

        r = New Regex("[ỴỸỶỲÝỹỵỷỳý]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "y")

        r = New Regex("[đĐĐ]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "d")

        r = New Regex("[ỢỠỞỜỚợỡởờớỘÔỔỒỐộỗổồốƠỎÕÒÓỌÔôọóòỏõơ]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "o")

        r = New Regex("[ỰỮỬỪỨựữửừứƯỤŨỦÙÚưụũủùú]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "u")
        '--------------------------------- 
        sTiengVietCoDau = sTiengVietCoDau.Trim

        r = New Regex("[():/,?.]")
        sTiengVietCoDau = r.Replace(sTiengVietCoDau, "-")

        sTiengVietCoDau = sTiengVietCoDau.Replace("  ", " ")
        'sTiengVietCoDau = sTiengVietCoDau.Replace("""", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("--", "-")
        sTiengVietCoDau = sTiengVietCoDau.Replace("<", "")
        sTiengVietCoDau = sTiengVietCoDau.Replace(">", "")
        sTiengVietCoDau = sTiengVietCoDau.Replace("=", "")

        'TrungNS:
        Dim sb As New StringBuilder()
        For i As Integer = 0 To sTiengVietCoDau.Length - 1
            If Char.GetUnicodeCategory(sTiengVietCoDau(i)) <> UnicodeCategory.NonSpacingMark AndAlso Not Char.IsPunctuation(sTiengVietCoDau(i)) AndAlso Not Char.IsSymbol(sTiengVietCoDau(i)) Then
                sb.Append(sTiengVietCoDau(i))
            End If
        Next

        sTiengVietCoDau = sb.ToString

        Return sTiengVietCoDau
    End Function
    ''' <summary>
    ''' Thay thế tiếng xâu tiếng việt có dấu thành không dấu phục vụ cho cỗ máy tìm kiếm V3
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertTiengVietCoDauThanhKhongDau(ByVal str As String) As String
        Dim sb As New StringBuilder()
        str = str.Replace("&", "and")
        str = Regex.Replace(str, "Đ|đ|đ|Đ", "d", RegexOptions.IgnoreCase)
        str = str.Normalize(NormalizationForm.FormKD)
        For i As Integer = 0 To str.Length - 1
            If Char.IsWhiteSpace(str(i)) Then
                sb.Append("-"c)
            ElseIf Char.GetUnicodeCategory(str(i)) <> UnicodeCategory.NonSpacingMark AndAlso Not Char.IsPunctuation(str(i)) AndAlso Not Char.IsSymbol(str(i)) Then
                sb.Append(str(i))
            End If
        Next
        Dim sReturn As String = sb.ToString.Replace("--", "-")
        sReturn = sReturn.Trim("-".ToCharArray)

        Return sReturn.ToLower
    End Function
    Public Shared Function FormatPlayer(ByVal source As Integer, ByVal type As Integer, ByVal folder As String, ByVal fileID As String, ByVal fileName As String, ByVal filePath As String) As String
        If type = FilesType.IMAGE Then
            Return String.Format("<a title='Chèn vào bài' style='display: block;' class='dragInsert' onclick='javascript:insertImages({0});'><img src='/images/icons/image_add.png' border='0px' style='border: 0; text-decoration:none;'></a><div class='divDrag' style='display: block;' id='{1}' title='{2}'><b>Ảnh: {3}</b><a title='IMAGES' class='{4}' href='{5}' target='_blank'><img src='/images/icons/map_magnify.png' border='0px' style='border: 0; text-decoration:none;'></a><a title='Hiển thị mã' style='border: 0; text-decoration:none;' class='ShowCodeBlock' onclick='javascript:showImagesCodeBlock({6});'><img src='/images/icons/page_white_code.png' border='0px' style='border: 0; text-decoration:none;'></a></div>", fileID, fileID, filePath, fileName, folder, fileName, fileID)
        Else
            Dim sourcetype As String = String.Empty
            Select Case source
                Case SourcesType.FTP
                    sourcetype = "FTP"
                Case SourcesType.UPLOAD
                    sourcetype = "UPLOAD"
            End Select
            Return String.Format("<a title='Chèn vào bài' style='display: block;' class='dragInsert' onclick='insertMedia(""<b>File(" + sourcetype + "): </b>"",{0});'><img src='/images/icons/film_go.png' border='0px' style='border: 0; text-decoration:none;'></a><div class='divDrag' style='display: block;' id='{1}' onclick='javascript:fnSelect({2});'><b>{3}</b><a title='Play' class='{4}' href='{5}'><img src='/images/icons/control_play_blue.png' border='0px' style='border: 0; text-decoration:none;'></a><a title='Download' class='{6}' href='{7}' target='_blank'><img src='/images/icons/control_repeat_blue.png' border='0px' style='border: 0; text-decoration:none;'></a><a title='ShowCodeBlock' style='border: 0; text-decoration:none;' class='ShowCodeBlock' onclick='showCodeBlock(""<b>File(" + sourcetype + "): </b>"",{8});'><img src='/images/icons/page_white_code.png' border='0px' style='border: 0; text-decoration:none;'></a></div>", fileID, fileID, fileID, fileName, folder, fileName, folder, fileName, fileID)
        End If
    End Function
    Public Shared Function FormatLoaiTinBai(ByVal NewsKind As Integer) As String
        Select Case NewsKind
            Case 1 'Tin chay: TODO
                Return "/Images/vov/ts.png"
            Case 2 'Tin sống
                Return "/Images/vov/ts.png"
            Case 3 'Bài
                Return "/Images/vov/bai.png"
            Case 4 'Phản ánh: TODO
                Return "/Images/vov/pv.png"
            Case 5 'Phỏng vấn
                Return "/Images/vov/pv.png"
            Case 6 'Phóng sự: TODO
                Return "/Images/vov/pv.png"
            Case 7 'Tổng hợp
                Return "/Images/vov/th.png"
            Case 8 'Bình luận
                Return "/Images/vov/bl.png"
            Case 9
                Return "/Images/vov/ttn.png"
            Case 10
                Return "/Images/vov/ttg.png"
            Case 11
                Return "/Images/vov/ttvh.png"
            Case Else
                Return ""
        End Select
    End Function
    Public Shared Function FormatNhuanButLoaitin(ByVal id As Integer) As String
        Select Case id
            Case 1 'Tin chay: TODO
                Return "Bài"
            Case 2 'Tin sống
                Return "Ảnh"
            Case 3 'Bài
                Return "Videos"
            Case 4 'Bài
                Return "Tin"
            Case Else
                Return ""
        End Select
    End Function
    Public Shared Function FormatLoaiTinBaiText(ByVal NewsKind As Integer) As String
        Select Case NewsKind
            Case 1
                Return "TC"
            Case 2
                Return "TS"
            Case 3
                Return "Bài"
            Case 4
                Return "PA"
            Case 5
                Return "PV"
            Case 6
                Return "PS"
            Case 7
                Return "TH"
            Case 8
                Return "BL"
            Case 9
                Return "TTN"
            Case 10
                Return "TTG"
            Case 11
                Return "TTVH"
            Case Else
                Return "Tin bài"
        End Select
    End Function
    Public Shared Function FormatLoaiTinBaiHTML(ByVal NewsKind As Integer) As String
        Select Case NewsKind
            Case 1
                Return "<font style='color:DarkBlue;font-weight:bold;'>TC</font>"
            Case 2
                Return "<font style='color:#0000ff;font-weight:bold;'>TS</font>"
            Case 3
                Return "<font style='color:#000;font-weight:bold;'>Bài</font>"
            Case 4
                Return "<font style='color:#920e1d;font-weight:bold;'>PA</font>"
            Case 5
                Return "<font style='color:#920e1d;font-weight:bold;'>PV</font>"
            Case 6
                Return "<font style='color:#250025;font-weight:bold;'>PS</font>"
            Case 7
                Return "<font style='color:#333300;font-weight:bold;'>TH</font>"
            Case 8
                Return "<font style='color:#000066;font-weight:bold;'>BL</font>"
            Case 9
                Return "<font style='color:#920e1d;font-weight:bold;'>TTN</font>"
            Case 10
                Return "<font style='color:#7A1F00;font-weight:bold;'>TTG</font>"
            Case 11
                Return "<font style='color:#003300;font-weight:bold;'>TTVH</font>"
            Case Else
                Return "<font style='color:#000;font-weight:bold;'>Tin bài</font>"
        End Select
    End Function
    Public Shared Function FormatTheLoai(ByVal theloai As Integer) As String
        Select Case theloai
            Case TheLoaiTin.Text
                Return "/Images/icons/vov/TEXT_ONLY.png"
            Case TheLoaiTin.Image
                Return "/Images/icons/icon_image.png"
            Case TheLoaiTin.Audio
                Return "/Images/icons/icon_audio.png"
            Case TheLoaiTin.Video
                Return "/Images/icons/icon_video.png"
            Case TheLoaiTin.TextImage
                Return "/Images/icons/vov/TEXT_I.png"
            Case TheLoaiTin.TextAudio
                Return "/Images/icons/vov/TEXT_A.png"
            Case TheLoaiTin.TextVideo
                Return "/Images/icons/vov/TEXT_V.png"
            Case TheLoaiTin.TextImageAudio
                Return "/Images/icons/vov/TEXT_AI.png"
            Case TheLoaiTin.TextAudioVideo
                Return "/Images/icons/vov/TEXT_AV.png"
            Case TheLoaiTin.TextImageVideo
                Return "/Images/icons/vov/TEXT_IV.png"
            Case Else
                Return "/Images/icons/vov/TEXT_AIV.png"
        End Select
    End Function
    Public Shared Function AppendPatternToString(ByVal sSourceString As String, ByVal sPatten As String, ByVal spliterString As String, Optional ByVal AppendToheader As Boolean = True) As String
        If sSourceString Is Null.NullString Then
            sSourceString = String.Empty
        End If
        Dim arrChar As Char() = {spliterString}
        Dim sTemp As String = spliterString + sSourceString.Trim(arrChar) + spliterString
        If Not sTemp.Contains(spliterString + sPatten + spliterString) Then
            If AppendToheader = True Then
                sSourceString = sPatten + spliterString + sSourceString.Trim(arrChar)
            Else
                sSourceString = sSourceString.Trim(arrChar) + spliterString + sPatten
            End If
        End If
        Return sSourceString.Trim(arrChar)
    End Function
    Public Shared Function RemovePatternFromString(ByVal sSourceString As String, ByVal sPatten As String, ByVal spliterString As String) As String
        Dim arrChar As Char() = {spliterString}
        Dim sTemp As String = spliterString + sSourceString.Trim(arrChar) + spliterString
        If sTemp.Contains(spliterString + sPatten + spliterString) Then
            sSourceString = sTemp.Replace(spliterString + sPatten & spliterString, spliterString)
        End If
        Return sSourceString.Trim(arrChar)
    End Function
    Public Shared Function ContainPatternInString(ByVal sSourceString As String, ByVal sPatten As String, ByVal spliterString As String) As Boolean
        If String.IsNullOrEmpty(sSourceString) Then
            Return False
        End If
        Dim arrChar As Char() = {spliterString}
        Dim sTemp As String = spliterString + sSourceString.Trim(arrChar) + spliterString
        If sTemp.Contains(spliterString + sPatten + spliterString) Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function IsMount_drive(localdrive As String, sharelocation As String, Optional user As String = "blank", Optional pass As String = "blank") As Boolean

        Dim oNetDrive As New NetworkDrive()

        Try
            oNetDrive.LocalDrive = localdrive
            oNetDrive.ShareName = sharelocation
            If user = "blank" Then
                oNetDrive.MapDrive()
            Else
                oNetDrive.MapDrive(user, pass)
            End If
        Catch err As Exception
            Return False
        End Try

        Return True
    End Function
    Public Shared TheLoaiTinBai As New Dictionary(Of String, Integer)() From {
        {"T", 1},
        {"I", 2},
        {"A", 3},
        {"V", 4},
        {"TI", 5},
        {"TA", 6},
        {"TV", 7},
        {"TIA", 8},
        {"TIV", 9},
        {"TAV", 10},
        {"TIAV", 11}
        }
    Public Shared Function CanViewControl(ByVal portalid As Integer, ByVal itabid As Integer) As Boolean
        Try
            If Not DotNetNuke.Security.Permissions.TabPermissionController.CanViewPage(New DotNetNuke.Entities.Tabs.TabController().GetTab(itabid, portalid, True)) Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Shared Function IsXBGroup(ByVal userin As UserInfo) As Boolean
        If userin.IsInRole("Xuat ban") OrElse userin.IsInRole("Administrators") Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function IsPheDuyetGroup(ByVal userin As UserInfo) As Boolean
        If userin.IsInRole("Phe duyet") OrElse userin.IsInRole("Administrators") Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function IsAdminGroup(ByVal userin As UserInfo) As Boolean
        If userin.IsInRole("Administrators") OrElse userin.IsInRole("Manager") OrElse userin.IsSuperUser Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetFilesList(ByVal arrMedia As String) As String
        Dim list As String = String.Empty
        Dim pats As String() = arrMedia.Split(";")
        For Each s As String In pats
            list += s.Split("|")(1) + "; "
        Next s

        Return list
    End Function
    Public Shared Function GetLoaiTinBai(arrFilesList As String, arrImgList As String) As Integer
        Const lText As String = "T"
        Dim lImages As String = String.Empty
        Dim lAudio As String = String.Empty
        Dim lVideo As String = String.Empty
        Dim strArr As String() = arrFilesList.Split(CType(";", Char))
        For i As Integer = 0 To strArr.Length - 1
            If strArr(i) <> "" Then
                Dim filename As String = strArr(i).Split("|")(1).ToString
                Dim ext As String = Path.GetExtension(filename).ToLower
                Select Case ext
                    Case ".mp3", ".wma", ".wav"
                        lAudio = "A"
                    Case ".mp4", ".flv", ".avi", ".mpeg", ".swf"
                        lVideo = "V"
                End Select
            End If
        Next
        If arrImgList.Length > 0 Then
            lImages = "I"
        End If

        Dim strCombination As String = lText + lImages + lAudio + lVideo

        Return BL.TheLoaiTinBai(strCombination)
    End Function
    Public Shared Function FormatNewsAction(action As Integer) As String
        Select Case action
            Case 1
                Return "Xem tin"
            Case 2
                Return "Lấy tin"
            Case 3
                Return "Chọn tin"
            Case Else
                Return "Chưa xác định"
        End Select
    End Function
    Public Shared Function GetStorageFolder(objNews As NV_NewsInfo, portalid As Integer) As String
        If objNews.IsArchived Then
            GetStorageFolder = PortalController.GetPortalSetting(BL.settingMediaLuuTruVirtual, portalid, Null.NullString) + "/" + objNews.StorageFolder
        Else
            GetStorageFolder = PortalController.GetPortalSetting(BL.settingMediaPathVirtual, portalid, Null.NullString) + "/" + objNews.StorageFolder
        End If
    End Function
    Public Shared Function GetLanguage() As String
        Return System.Threading.Thread.CurrentThread.CurrentCulture.ToString()
    End Function
    Public Shared Function RemoveHTMLTags(ByVal html As String) As String
        ' Remove HTML tags.
        Return Regex.Replace(html, "<.*?>", "")
    End Function
#Region "upload anh tu base64"
    Public Shared Function UploadImageFromBase64(ByVal imagebase64a As String, tenfile As String, serverPath As String, virtualpath As String, useridz As Integer, portalid As Integer, newid As Integer) As String
        Dim sresult As String = ""
        Try
            Dim mD5Provider = New MD5CryptoServiceProvider()
            Dim hashDateTime = BitConverter.ToString(mD5Provider.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff t")))).Replace("-", String.Empty)
            Dim imagebase64 = extractbase(imagebase64a)
            Dim fileInputName = tenfile & "." & extractMimeType(imagebase64a)

            If Not Directory.Exists(serverPath) Then
                If serverPath IsNot Nothing Then Directory.CreateDirectory(serverPath)
            End If
            Dim imageBytes = Convert.FromBase64String(imagebase64)
            Dim memoryStream = New MemoryStream(imageBytes, 0, imageBytes.Length)
            memoryStream.Write(imageBytes, 0, imageBytes.Length)
            Dim imgStream = System.Drawing.Image.FromStream(memoryStream, True)
            If serverPath IsNot Nothing Then
                Dim fileInputUrl = Path.Combine(serverPath & "\", Path.GetFileName(fileInputName))
                imgStream.Save(fileInputUrl)
            End If
            sresult = virtualpath & "/" & fileInputName
            'Insert vào Database
            Dim ctlMediaNews As New NewsByMediaController
            Dim ctlMedia As New MediaItemController
            Dim idmedia As Integer = 0
            idmedia = ctlMedia._Insert(fileInputName, fileInputName, serverPath, sresult, imageBytes.Length, extractMimeType(imagebase64a), DateTime.Now, useridz, PortalSettings.Current.PortalId)
            'chen vao bang product media
            ctlMediaNews._Insert(newid, idmedia, DateTime.Now, useridz, portalid)
            Return sresult
        Catch ex As Exception
        End Try

        Return sresult
    End Function

    Private Shared Function extractMimeType(ByVal final As String) As String
        Dim sTotal As String() = final.ToString().Split(","c)
        'Cat doan 1
        Dim stotal0 As String = sTotal(0)
        '--cat tiep
        Dim stotal01 As String() = stotal0.ToString().Split(";"c)
        Dim stotal0a As String = stotal01(0)

        Dim stotal01aa As String() = stotal0a.ToString().Split("/"c)
        Dim stotal0aaaaa As String = stotal01aa(1)
        '---
        Dim stotal1 As String = sTotal(1)
        Return stotal0aaaaa
    End Function
    Private Shared Function extractbase(ByVal final As String) As String
        Dim sTotal As String() = final.ToString().Split(","c)
        'Cat doan 1
        Dim stotal0 As String = sTotal(1)

        Return stotal0
    End Function
    Public Shared Function UrlFriendly(ByVal stringConvert As String, ByVal Optional specialReplace As String = "-") As String
        stringConvert = Decode(stringConvert)
        stringConvert = stringConvert.ToLower().Trim()
        stringConvert = Regex.Replace(stringConvert, "[àáảãạâầấẩẫậăằắẳẵặÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶ]", "a")
        stringConvert = Regex.Replace(stringConvert, "[òóỏõọôồốổỗộơờớởỡợÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢ]", "o")
        stringConvert = Regex.Replace(stringConvert, "[èéẻẽẹêềếểễệÈÉẺẼẸÊỀẾỂỄỆ]", "e")
        stringConvert = Regex.Replace(stringConvert, "[íìỉĩịÌÍỈĨỊ]", "i")
        stringConvert = Regex.Replace(stringConvert, "[úùủũụưứừửữựÙÚỦŨỤƯỪỨỬỮỰ]", "u")
        stringConvert = Regex.Replace(stringConvert, "[ýỳỷỹỵỲÝỶỸỴ]", "y")
        stringConvert = Regex.Replace(stringConvert, "[đĐ]", "d")
        stringConvert = Regex.Replace(stringConvert, "--", "-")
        stringConvert = Regex.Replace(stringConvert, "\W+", specialReplace)
        Return stringConvert.Trim()
    End Function
    Public Shared Function Decode(ByVal stringConvert As String) As String
        Return HttpUtility.HtmlDecode(stringConvert)
    End Function
#End Region
End Class