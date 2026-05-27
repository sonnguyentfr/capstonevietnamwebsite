Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Common.Utilities
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.IO

Public Class nvcmsBL
    Public Shared regEmailCacheKey As String = "regEmail"
    Public Shared regSMSCacheKey As String = "regSMS"
    Public Shared regCuttingNumber As Integer = 300
    Public Shared NewsTinMoiNhat As String = "NewsTinMoiNhat"
    Public Shared NewsHomeHotSite As String = "NewsHomeHotSite"
    Public Shared NewsTinNong As String = "NewsTinNong"
    Public Shared NewsXuHuongDoc As String = "NewsXuHuongDoc"
    Public Shared NewsHomeCat As String = "NewsHomeCat"
    Public Shared NewsCatList As String = "NewsCatList"
    Public Shared NewsDetailCache As String = "NewsDetailCache"
    Public Shared settingAnhLuuTruVirtual As String = "settingAnhLuuTruVirtual"
    Public Shared settingAnhLuuTruPhysical As String = "settingAnhLuuTruPhysical"
    Public Shared settingFlashVirtual As String = "settingFlashVirtual"
    Public Shared settingFlashPhysical As String = "settingFlashPhysical"
    Public Shared settingDocumentVirtual As String = "settingDocumentVirtual"
    Public Shared settingDocumentPhysical As String = "settingDocumentPhysical"
    Public Shared settingMediaPathVirtual As String = "settingMediaPathVirtual"
    Public Shared settingMediaPathPhysical As String = "settingMediaPathPhysical"
    Public Shared settingMediaLuuTruVirtual As String = "settingMediaLuuTruVirtual"
    Public Shared settingMediaLuuTruPhysical As String = "settingMediaLuuTruPhysical"
    Public Shared settingBackupPathVirtual As String = "settingBackupPathVirtual"
    Public Shared settingBackupPathPhysical As String = "settingBackupPathPhysical"
    Public Shared settingSanphamPhysical As String = "settingSanPhamPhysical"
    Public Shared settingSanphamVirtual As String = "settingSanPhamVirtual"
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

    Public Shared settingRss_Source = "settingRssSource"
    Public Shared settingRss_Column = "settingRssColumn"
    Public Shared settingRss_Template = "settingRssTemplate"
    Public Shared settingRss_PageSize = "settingRssPageSize"

    Public Shared settingView_Cate = "settingViewCate"
    Public Shared settingView_Type = "settingViewType"
    Public Shared settingView_Total = "settingViewTop"
    Public Shared settingView_Template = "settingViewTemplate"
    Public Shared settingView_ImgSize = "settingViewImgSize"
    Public Shared settingView_SizeDes = "settingSizeDes"
    Public Shared settingView_SizeTitle = "settingSizeTitle"
    Public Shared settingViewTop_Categories = "settingTopCategories"

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
    Public Shared settingList_ShowOtherNews = "settingList_ShowOtherNews"
    Public Shared settingDetails_More = "settingDetails_More"
    Public Shared settingDetails_Other = "settingDetails_Other"
    Public Shared settingDetails_Template = "settingDetails_Template"
    Public Shared settingDetails_Allow = "settingDetails_Allow"
    Public Shared settingDetails_Comment = "settingDetails_Comment"

    Public Shared minDateV As DateTime = "01/01/2000"
    Public Shared maxDateV As DateTime = "01/01/2100"
    Public Shared MaxURLWord As Integer = 60

    Public Shared settingPage = "settingPage"
    Public Shared settingPageEn = "settingPageEN"
    Public Shared settingPagesitename = "settingPagesitename"
    Public Shared settingPagesiteweb = "settingPagesiteweb"
    Public Shared settingPagesitediachi = "settingPagesitediachi"
    Public Shared settingPagesitediachi1 = "settingPagesitediachi1"
    Public Shared settingPagesitediachi2 = "settingPagesitediachi2"
    Public Shared settingPagesiteemail = "settingPagesiteemail"
    Public Shared settingPagesiteemail1 = "settingPagesiteemail1"
    Public Shared settingPagesiteemail2 = "settingPagesiteemail2"
    Public Shared settingPagesitedienthoai = "settingPagesitedienthoai"
    Public Shared settingPagesitedienthoai1 = "settingPagesitedienthoai1"
    Public Shared settingPagesitedienthoai2 = "settingPagesitedienthoai2"
    Public Shared settingPagesitechinhnhanh1 = "settingPagesitechinhnhanh1"
    Public Shared settingPagesitechinhnhanh2 = "settingPagesitechinhnhanh2"
    Public Shared settingPagesiteNhanMail = "settingPagesiteNhanMail"
    Public Shared settingPagesiteNhanMailList = "settingPagesiteNhanMailList"

    Public Shared settingPagesitetomtat = "settingPagesitetomtat"
    Public Shared settingPagesitetag = "settingPagesitetag"
    Public Shared settingPagesitefacebookpage = "settingPagesitefacebookpage"
    Public Shared settingPagesiteyoutube = "settingPagesiteyoutube"
    Public Shared settingPagesiteLinkedin = "settingPagesiteLinkedin"
    Public Shared settingPagesiteInstagram = "settingPagesiteInstagram"
    Public Shared settingPagesiteZalo = "settingPagesiteZalo"
    Public Shared settingPagesiteTwitter = "settingPagesiteTwitter"
    Public Shared settingPagesitewhatsapp = "settingPagesitewhatsapp"
    Public Shared settingPagesiteSkype = "settingPagesiteSkype"
    Public Shared settingPageGooogleCapcha = "settingPageGooogleCapcha"
    Public Shared settingPageGooogleCapchaSecret = "settingPageGooogleCapchaSecret"
    Public Shared settingPagesiteHeaderCode = "settingPagesiteHeaderCode"
    Public Shared settingPagesiteFooterCode = "settingPagesiteFooterCode"


    Public Shared settingPageSiteCDN = "settingPageSiteCDN"
    Public Shared settingPageSiteFilesServer = "settingPageSiteFilesServer"

    Public Shared settingPageMailSMTP = "settingPageMailSMTP"
    Public Shared settingPageMailTenHienThi = "settingPageMailTenHienThi"
    Public Shared settingPageMailEmail = "settingPageMailEmail"
    Public Shared settingPageMailMatkhau = "settingPageMailMatkhau"



    Public Shared settingPagesiteLogo = "settingPagesiteLogo"
    Public Shared settingPagesiteLogofooter = "settingPagesiteLogofooter"
    Public Shared settingPageTinTuc = "settingPageTinTuc"
    Public Shared settingPageTinAnh = "settingPageTinAnh"
    Public Shared settingPageVideo = "settingPageVideo"
    Public Shared settingPageEvents = "settingPageEvents"
    Public Shared settingFolderAttachId = "settingFolderAttachId"
    Public Shared settingPageThongKe = "settingPageThongKe"
    'Cache string
    Public Shared cacheLibTrinhDoAll = "cacheTrinhDoAll_"
    Public Shared cacheLibQuocGia = "cacheQuocGia_"
    Public Shared cacheLibMajor = "cacheLibMajor_"
    Public Shared cacheLibCodeHinhThuc = "cacheCodeHinhThuc_"
    Public Shared cacheLibFollowUpPhuongThuc = "cacheFollowUpPhuongThuc_"
    Public Shared cacheLibFollow_TrangThaiNhom = "cacheLibFollow_TrangThaiNhom_"
    Public Shared cacheLibFollowUpTrangThai = "cacheLibFollowUpTrangThai_"

    Public Shared cacheStudent_OS_List = "cacheStudent_OS_List_"
    Public Shared cacheStudent_OS_List_Count = "cacheStudent_OS_List_Count_"
    Public Shared cacheLibLoaiTruongAll = "cacheLibLoaiTruongAll_"
    Public Shared cacheLibLoaiTruongAllShow = "cacheLibLoaiTruongAllShow_"
    Public Shared cacheShowBaiMoiDanhMuc As String = "cacheShowBaiMoiDanhMuc_"
    Public Shared cacheShowGetAllByType As String = "cacheShowGetAllByType_"
    Public Shared cacheShowIndexNews As String = "cacheShowIndexNews_"
    Public Shared cacheMarketingSchool As String = "cacheMarketingSchool_"
    Public Shared cacheMarketingSchoolDetail As String = "cacheMarketingSchoolDetail_"
    Public Shared cacheMarketingSchool_VerSion As String = "cacheMarketingSchool_VerSion_"
    Public Shared cacheShortUrl As String = "cacheShortUrl_"
    Public Shared settingPagePCNewsDetail As String = "1"
    'CDN
    Public Shared filesDomain As String = "https://capstonevietnam-fileserver.nvcms.net"
    Public Shared cdnDomain As String = "/static"
    Public Shared Function GetLanguage() As String
        Return System.Threading.Thread.CurrentThread.CurrentCulture.ToString()
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

#Region "FILE"
    Public Shared Function GetStorageFolder() As String
        Return Now.ToString("yyyy") + "/" + Now.ToString("MM") + "/" + Now.ToString("dd")
    End Function
    Public Shared Function CreateStorage(ByVal destination As String) As String
        If Not Directory.Exists(destination + "\" + Now.ToString("yyyy")) Then
            Directory.CreateDirectory(destination + "\" + Now.ToString("yyyy"))
        End If
        If Not Directory.Exists(destination + "\" + Now.ToString("yyyy") + "\" + Now.ToString("MM")) Then
            Directory.CreateDirectory(destination + "\" + Now.ToString("yyyy") + "\" + Now.ToString("MM"))
        End If
        If Not Directory.Exists(destination + "\" + Now.ToString("yyyy") + "\" + Now.ToString("MM") + "\" + Now.ToString("dd")) Then
            Directory.CreateDirectory(destination + "\" + Now.ToString("yyyy") + "\" + Now.ToString("MM") + "\" + Now.ToString("dd"))
        End If
        Return destination + "\" + Now.ToString("yyyy") + "\" + Now.ToString("MM") + "\" + Now.ToString("dd")
    End Function
    Public Shared Function GetImagePath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(nvcmsBL.settingAnhLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(nvcmsBL.settingAnhLuuTruVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(nvcmsBL.settingAnhLuuTruVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(nvcmsBL.settingAnhLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(nvcmsBL.settingAnhLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    Public Shared Function FormatThumbImage(server As HttpServerUtility, imgPath As String, thumbwidth As Integer) As String
        Dim sReturn As String = String.Empty
        If String.IsNullOrEmpty(imgPath) Then
            Return String.Empty
        End If
        sReturn = imgPath.Substring(0, imgPath.LastIndexOf("/", System.StringComparison.Ordinal)) & "/thumb" & thumbwidth.ToString() & "/" & Path.GetFileName(imgPath)
        If File.Exists(server.MapPath(sReturn)) Then
            Return sReturn
        Else
            Return imgPath
        End If
    End Function
    Public Shared Function GetDocumentPath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(settingDocumentPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(settingDocumentVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(settingDocumentVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(settingDocumentPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(settingDocumentPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    ''' <summary>
    ''' Used to generate thumbnails
    ''' Disk caching enabled
    ''' High requests
    ''' </summary>
    ''' <param name="imgPath">virtual image path</param>
    ''' <param name="width">width</param>
    ''' <param name="height">height</param>
    ''' <param name="mode">crop, pad, canvas. Default: crop</param>
    ''' <param name="anchor">topleft, topcenter, topright, middleleft, middlecenter, middleright, bottomleft, bottomcenter, and bottomright. Default: middlecenter</param>
    ''' <param name="format">file format. Default: closest type</param>
    ''' <returns>An thumb image</returns>
    ''' <remarks>Written by TrungNS</remarks>
    Public Shared Function FormatThumbImage(imgPath As String, width As Integer, height As Integer, mode As String, anchor As String, Optional ByVal format As String = "", Optional ByVal scale As String = "") As String
        If Not String.IsNullOrEmpty(imgPath) Then
            Return CType((imgPath.Replace("/DATA", nvcmsBL.filesDomain) & "?width=" & width.ToString() & IIf(height > 0, "&height=" & height.ToString(), "") & IIf(String.IsNullOrEmpty(mode), "&mode=crop", "&mode=" & mode) & IIf(String.IsNullOrEmpty(anchor), "&anchor=middlecenter", "&anchor=" & anchor) & IIf(String.IsNullOrEmpty(format), "", "&format=" & format) & IIf(String.IsNullOrEmpty(scale), "", "&scale=" & scale)), String)
        Else
            Return "/no-image.png"
        End If
    End Function
#End Region
#Region "Xu ly URL"
    Public Shared Function GetRequestId(surl As String) As Integer
        Dim iStart As Integer = surl.LastIndexOf("-", System.StringComparison.Ordinal)
        If IsNumeric(surl.Substring(iStart + 1)) Then
            Return CType(surl.Substring(iStart + 1), Integer)
        Else
            Return 0
        End If

    End Function
#End Region

End Class