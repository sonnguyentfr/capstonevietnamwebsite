Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Policy
Imports System.Threading
Imports DotNetNuke.Entities.Controllers
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Mail
Imports HtmlAgilityPack
Imports Microsoft.Web.Administration
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.Users
Imports NVCMS.Modules.Video
Imports SMS
Imports Vbuzz.Modules.Media
Imports NVCMS.Modules.School
Public Class Ultis
#Region "Chơi khô mãu, mr Dòi cho Reset Pool luôn"
    Public Shared ctlvideo As New Videos_Controller
    Public Shared _Marketing_Truong_Version_Controller As New MarketingSchoolController
    Public Shared Function RecycleApplicationPool(ByVal siteName As String) As Boolean
        If siteName Is Nothing Then siteName = System.Web.Hosting.HostingEnvironment.SiteName

        Using iisManager As ServerManager = New ServerManager()
            Dim sites As SiteCollection = iisManager.Sites
            For Each site As Microsoft.Web.Administration.Site In sites
                If site.Name = siteName Then
                    iisManager.ApplicationPools(site.Applications("/").ApplicationPoolName).Recycle()
                    Return True
                End If
            Next
        End Using

        Return False
    End Function
#End Region
#Region "Bam het URL"
    'Thanh xoan
    Private Const KoDauChars As String = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU"
    Private Const uniChars As String = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ"
    Public Shared Function UnicodeToASCII(ByVal s As String) As String
        Dim retVal As String = String.Empty
        s = s.Trim()
        Dim pos As Integer

        For i As Integer = 0 To s.Length - 1
            pos = uniChars.IndexOf(s(i).ToString())

            If pos >= 0 Then
                retVal += KoDauChars(pos)
            Else
                retVal += s(i)
            End If
        Next

        Return retVal
    End Function

    Public Shared Function UnicodeToKoDauAndGach(ByVal s As String) As String
        Dim strChar As String = "-abcdefghijklmnopqrstxyzuvxw0123456789 "
        s = s.Replace("  ", " ")
        s = s.Replace("+", "-")
        s = UnicodeToASCII(s.ToLower().Trim())
        Dim sReturn As String = ""

        For i As Integer = 0 To s.Length - 1

            If strChar.IndexOf(s(i)) > -1 Then

                If s(i) <> " "c Then
                    sReturn += s(i)
                ElseIf i > 0 AndAlso s(i - 1) <> "-"c Then
                    sReturn += "-"
                End If
            End If
        Next

        sReturn = sReturn.Replace("--", "-")
        Return sReturn
    End Function
    '=====
#End Region
#Region "Permission Button"
    Public Shared ctlnews As New NV_NewsController

    Public Shared nhomphongvien As String = "Bien Tap"
    Public Shared nhompheduyet As String = "Phe duyet"
    Public Shared nhomxuatban As String = "Xuat ban"


    Public Shared nutluu As String = "zNutLuu"
    Public Shared nutguibientap As String = "zNutGuiBienTap"
    Public Shared nutguixuatban As String = "zNutGuiXuatBan"
    Public Shared nutxuatbanluon As String = "zNutXuatBanLuon"
    Public Shared nuttatca As String = "zNutTatca"
#End Region
#Region "Phan quyen cac nut"


    ''' <summary>
    ''' Phan quyen cac nút
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <returns></returns>
    Public Shared Function ButtonNutLuu(uid As Integer) As Boolean
        Dim an As String = False
        Dim objUserinfo As UserInfo = UserController.GetUserById(PortalSettings.Current.PortalId, uid)
        If Not objUserinfo Is Nothing Then
            With objUserinfo
                If .IsInRole(nutluu) Or .IsInRole(nuttatca) Then
                    an = True
                Else
                    an = False
                End If
            End With
        Else
            an = False
        End If

        Return an
    End Function
    Public Shared Function ButtonGuiTienTap(ByVal uid As Integer) As Boolean
        Dim an As Boolean = False
        Dim objUserinfo As UserInfo = UserController.GetUserById(PortalSettings.Current.PortalId, uid)
        If Not objUserinfo Is Nothing Then
            With objUserinfo
                If objUserinfo.IsInRole(nutguibientap) Or objUserinfo.IsInRole(nuttatca) Then
                    an = True
                Else
                    an = False
                End If
            End With
        Else
            Return an
        End If

        Return an
    End Function
    Public Shared Function ButtonGuiXuatBan(ByVal uid As Integer) As Boolean
        Dim an As Boolean = False
        Dim objUserinfo As UserInfo = UserController.GetUserById(PortalSettings.Current.PortalId, uid)
        If Not objUserinfo Is Nothing Then
            With objUserinfo
                If objUserinfo.IsInRole(nutguixuatban) Or objUserinfo.IsInRole(nuttatca) Then
                    an = True
                Else
                    an = False
                End If
            End With
        Else
            Return an
        End If

        Return an
    End Function
    Public Shared Function ButtonXuatBanLuon(ByVal uid As Integer) As Boolean
        Dim an As Boolean = False
        Dim objUserinfo As UserInfo = UserController.GetUserById(PortalSettings.Current.PortalId, uid)
        If Not objUserinfo Is Nothing Then
            With objUserinfo
                If objUserinfo.IsInRole(nutxuatbanluon) Or objUserinfo.IsInRole(nuttatca) Then
                    an = True
                Else
                    an = False
                End If
            End With
        Else
            Return an
        End If
        Return an
    End Function
    Public Shared Function UButtonEdit(ByVal Newid As Integer) As Boolean
        Dim an As Boolean = False
        Dim objNews As NV_NewsInfo = ctlnews.GetByID(Newid)
        If Not objNews Is Nothing Then
            With objNews
                If (.Status = NewsStatus.DangBienSoan) Or (.Status = NewsStatus.BiTraLai) Then
                    an = True
                End If
            End With
        Else
            Return an
        End If
        Return an
    End Function
    Public Shared Function UButtonBiTraLai(ByVal Newid As Integer) As Boolean
        Dim an As Boolean = False
        Dim objNews As NV_NewsInfo = ctlnews.GetByID(Newid)
        If Not objNews Is Nothing Then
            With objNews
                If (.Status = NewsStatus.BiTraLai) Then
                    an = True
                End If
            End With
        Else
            Return an
        End If
        Return an
    End Function
    Public Shared Function UButtonTrieuHoi(ByVal Newid As Integer) As Boolean
        Dim an As Boolean = False
        Dim objNews As NV_NewsInfo = ctlnews.GetByID(Newid)
        If Not objNews Is Nothing Then
            With objNews
                If (.Status = NewsStatus.ChoPheDuyet) Or (.Status = NewsStatus.ChoXuatBan) Then
                    If .IsEdited = False Then
                        an = True
                    End If
                End If
            End With
        Else
            Return an
        End If
        Return an
    End Function
#End Region
#Region "VIDEO phan quyen nut"
    Public Shared Function VideoUButtonEdit(ByVal videoid As Integer) As Boolean
        Dim an As Boolean = False
        Dim objVideo As Videos_Info = ctlvideo.GetByID(videoid, PortalSettings.Current.PortalId)
        If Not objVideo Is Nothing Then
            With objVideo
                If (.Status = NewsStatus.DangBienSoan) Or (.Status = NewsStatus.BiTraLai) Then
                    an = True
                End If
            End With
        Else
            Return an
        End If
        Return an
    End Function
    Public Shared Function VideoFormatEdittedBy(portalid As Integer, ByVal video As Integer) As String
        Dim objInfo As Videos_Info = ctlvideo.GetByID(video, portalid)
        If Not objInfo Is Nothing Then
            Return CType(("Tin bài đang được chỉnh sửa bởi: <strong>" + BL.GetNameByUserId(portalid, objInfo.EditedUser) + "</strong>, khoảng " + IIf(Now.Subtract(objInfo.EditedTime).Days = 0, "", Now.Subtract(objInfo.EditedTime).Days.ToString() + " ngày, ") + IIf(Now.Subtract(objInfo.EditedTime).Hours = 0, "", Now.Subtract(objInfo.EditedTime).Hours.ToString() + " giờ, ") + IIf(Now.Subtract(objInfo.EditedTime).Minutes = 0, "< 1", Now.Subtract(objInfo.EditedTime).Minutes.ToString()) + " phút trước"), String)
        Else
            Return ""
        End If
    End Function
    Public Shared Function VideoFormatTrieuHoi(ByVal status As Integer) As Boolean
        If (status = NewsStatus.ChoXuatBan) Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region
#Region "General BL"
    Public Shared Function GetRequestIdXML(surl As String) As String
        Dim iStart As Integer = surl.LastIndexOf("/", System.StringComparison.Ordinal)
        Dim iEnd As Integer = surl.LastIndexOf(".xml", System.StringComparison.Ordinal)
        If iEnd > 0 Then
            Dim iLength As Integer = iEnd - iStart - 1

            Return CType(surl.Substring(iStart + 1, iLength), String)
        Else
            Return 0
        End If
    End Function
    Public Shared Function GetRequestId(surl As String) As Integer
        Dim iStart As Integer = surl.LastIndexOf("-", System.StringComparison.Ordinal)
        Dim iEnd As Integer = surl.LastIndexOf(".html", System.StringComparison.Ordinal)
        If iEnd > 0 Then
            Dim iLength As Integer = iEnd - iStart - 1
            If IsNumeric(surl.Substring(iStart + 1, iLength)) Then
                Return CType(surl.Substring(iStart + 1, iLength), Integer)
            Else
                Return -1
            End If
        Else
            Return -1
        End If
    End Function
    Public Shared Function GenerateThumbs(ByVal absolutePath As String, width As Integer) As String
        Dim photoFile As New System.IO.FileInfo(absolutePath)
        If photoFile.Exists Then
            Dim objImage As New ImageUltilities

            objImage.Width = width + 20
            objImage.Height = -1
            objImage.vAlign = VerticalAlign.Bottom
            objImage.ImagePath = absolutePath
            Dim imageThumb As System.Drawing.Image = objImage.FillImage()

            If Not Directory.Exists(photoFile.Directory.FullName & "/thumb" + width.ToString() + "/") Then
                Directory.CreateDirectory(photoFile.Directory.FullName & "/thumb" + width.ToString() + "/")
            End If
            imageThumb.Save(photoFile.Directory.FullName & "/thumb" & width.ToString() & "/" & photoFile.Name)
        End If
    End Function
    Public Shared Function GenerateThumbs(ByVal absolutePath As String) As String
        Dim photoFile As New System.IO.FileInfo(absolutePath)
        If photoFile.Exists Then
            'comment: Tobe removed
            'GenerateThumbs(absolutePath, 476) '1 An tuong
            GenerateThumbs(absolutePath, 450) '1
            'GenerateThumbs(absolutePath, 443) '1 Index: 1st - Hot Cat 
            'GenerateThumbs(absolutePath, 441) '1 Hot trang chu
            GenerateThumbs(absolutePath, 360) '1
            'GenerateThumbs(absolutePath, 318) '1 Hau truong
            'GenerateThumbs(absolutePath, 315) '1 Gioi mot
            GenerateThumbs(absolutePath, 283)
            'GenerateThumbs(absolutePath, 254) '1 Am nhac
            'GenerateThumbs(absolutePath, 244) '1 Dien anh
            'GenerateThumbs(absolutePath, 232) '1 Hot Top Trang chu
            'INDEX
            GenerateThumbs(absolutePath, 220) '1 Dien anh, Index 
            'GenerateThumbs(absolutePath, 208)
            'GenerateThumbs(absolutePath, 206) '1
            'GenerateThumbs(absolutePath, 192) ' 1 Index: list
            GenerateThumbs(absolutePath, 190) '1
            'GenerateThumbs(absolutePath, 165)
            GenerateThumbs(absolutePath, 160) '1
            'GenerateThumbs(absolutePath, 141)
            'SMALLEST
            GenerateThumbs(absolutePath, 127)
            'GenerateThumbs(absolutePath, 86) '1
        End If
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
        Return CType((imgPath.Replace("/DATA", BL.filesDomain) & "?dpi=150&quality=100&width=" & width.ToString() & IIf(height > 0, "&height=" & height.ToString(), "") & IIf(String.IsNullOrEmpty(mode), "&mode=crop", "&mode=" & mode) & IIf(String.IsNullOrEmpty(anchor), "&anchor=middlecenter", "&anchor=" & anchor) & IIf(String.IsNullOrEmpty(format), "", "&format=" & format) & IIf(String.IsNullOrEmpty(scale), "", "&scale=" & scale)), String)
    End Function
    Public Shared Function FormatFullImage(imgPath As String) As String
        Return CType((imgPath.Replace("/DATA", BL.filesDomain)), String)
    End Function
    Public Shared Function GetImageThumb(ByVal imgpath As String) As String
        If Not String.IsNullOrEmpty(imgpath) Then
            imgpath = imgpath.Replace("\", "/")
            Return imgpath.Substring(0, imgpath.LastIndexOf("/", System.StringComparison.Ordinal)) + "/Thumb/" + Path.GetFileName(imgpath)
        Else
            Return ""
        End If
    End Function
    Public Shared Function FormatLinkVideo(ByVal tabid As Integer, ByVal Id As Integer, ByVal title As String) As String
        If Not Null.IsNull(tabid) Then
            Return NavigateURL(tabid, "", BuildEntryLink(Id, title.ToLower())) & ".html"
        Else
            Return "#"
        End If

    End Function
    Public Shared Function FormatLink(ByVal tabid As Integer, ByVal Id As Integer, ByVal title As String) As String
        Dim tabnewsdetail As String = PortalController.GetPortalSetting(nvcmsBL.settingPagePCNewsDetail, 0, Null.NullString)
        If (Not tabnewsdetail Is Nothing) And (tabnewsdetail <> "") Then
            If IsNumeric(tabnewsdetail) Then
                tabid = tabnewsdetail
            Else
                Dim ctlnews As New NV_NewsController
                Dim objnews As NV_NewsInfo = ctlnews.GetByID(Id)
                If Not objnews Is Nothing Then
                    tabid = objnews.CategoryId
                End If
            End If
        End If
        If Not Null.IsNull(tabid) Then
            Return NavigateURL(tabid, "", BuildEntryLink(Id, title.ToLower())) & ".html"
        Else
            Return "#"
        End If

    End Function
    Public Shared Function FormatLinkadminXemtruoc(stringurl As String, ByVal Id As Integer, ByVal title As String) As String
        If Not Null.IsNull(Id) Then
            Return stringurl & "/" & BuildEntryLink(Id, title.ToLower()) & ".html"
        Else
            Return "#"
        End If

    End Function
    Public Shared Function BuildEntryLink(EntryId As Integer, ByVal EntryTitle2 As String) As String
        Dim ignoreCase As RegexOptions = RegexOptions.IgnoreCase
        Dim EntryTitle As String = UnicodeToKoDauAndGach(EntryTitle2)
        Dim strTitle As String = (Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(TextHelper.CleanSign(HttpUtility.HtmlDecode(EntryTitle)).Replace("'", String.Empty).Replace("""", String.Empty).Replace("&amp;", String.Empty).Replace("&", String.Empty), ChrW(258) & "|" & ChrW(256) & "|" & ChrW(192) & "|" & ChrW(193) & "|" & ChrW(194) & "|" & ChrW(195) & "|" & ChrW(196) & "|" & ChrW(197), "A"), ChrW(259) & "|" & ChrW(257) & "|" & ChrW(224) & "|" & ChrW(225) & "|" & ChrW(226) & "|" & ChrW(227) & "|" & ChrW(228) & "|" & ChrW(229) & "|" & ChrW(261), "a"), ChrW(198), "AE"), ChrW(230), "ae"), ChrW(223), "ss"), ChrW(199) & "|" & ChrW(262) & "|" & ChrW(264) & "|" & ChrW(266) & "|" & ChrW(268), "C"), ChrW(263) & "|" & ChrW(265) & "|" & ChrW(267) & "|" & ChrW(269) & "|" & ChrW(231), "c"), ChrW(270) & "|" & ChrW(272), "D"), ChrW(271) & "|" & ChrW(273), "d"), ChrW(274) & "|" & ChrW(276) & "|" & ChrW(278) & "|" & ChrW(280) & "|" & ChrW(282) & "|" & ChrW(201) & "|" & ChrW(280) & "|" & ChrW(200) & "|" & ChrW(201) & "|" & ChrW(202) & "|" & ChrW(203), "E"), ChrW(275) & "|" & ChrW(277) & "|" & ChrW(279) & "|" & ChrW(281) & "|" & ChrW(283) & "|" & ChrW(234) & "|" & ChrW(235) & "|" & ChrW(232) & "|" & ChrW(233), "e"), ChrW(284) & "|" & ChrW(286) & "|" & ChrW(288) & "|" & ChrW(290) & "|" & ChrW(290), "G"), ChrW(285) & "|" & ChrW(287) & "|" & ChrW(289) & "|" & ChrW(291) & "|" & ChrW(291), "g"), ChrW(292) & "|" & ChrW(294), "H"), ChrW(293) & "|" & ChrW(295), "h"), ChrW(204) & "|" & ChrW(205) & "|" & ChrW(206) & "|" & ChrW(207) & "|" & ChrW(296) & "|" & ChrW(298) & "|" & ChrW(300) & "|" & ChrW(302) & "|" & ChrW(304) & "|" & ChrW(304), "I"), ChrW(236) & "|" & ChrW(237) & "|" & ChrW(238) & "|" & ChrW(239) & "|" & ChrW(297) & "|" & ChrW(299) & "|" & ChrW(301) & "|" & ChrW(303), "i"), ChrW(306), "IJ"), ChrW(308), "J"), ChrW(309), "j"), ChrW(310), "K"), ChrW(311), "k"), ChrW(209) & "|" & ChrW(209), "N"), ChrW(241), "n"), ChrW(210) & "|" & ChrW(211) & "|" & ChrW(212) & "|" & ChrW(213) & "|" & ChrW(214) & "|" & ChrW(216) & "|" & ChrW(336), "O"), ChrW(242) & "|" & ChrW(243) & "|" & ChrW(244) & "|" & ChrW(245) & "|" & ChrW(246) & "|" & ChrW(248) & "|" & ChrW(337), "o"), ChrW(338), "OE"), ChrW(339), "oe"), ChrW(340) & "|" & ChrW(344) & "|" & ChrW(342) & "|" & ChrW(340), "R"), ChrW(345) & "|" & ChrW(343) & "|" & ChrW(341), "r"), ChrW(352) & "|" & ChrW(350) & "|" & ChrW(348) & "|" & ChrW(346), "S"), ChrW(353) & "|" & ChrW(351) & "|" & ChrW(349) & "|" & ChrW(347), "s"), ChrW(356) & "|" & ChrW(354), "T"), ChrW(357) & "|" & ChrW(355), "t"), ChrW(370) & "|" & ChrW(368) & "|" & ChrW(366) & "|" & ChrW(364) & "|" & ChrW(362) & "|" & ChrW(360) & "|" & ChrW(217) & "|" & ChrW(218) & "|" & ChrW(219) & "|" & ChrW(220), "U"), ChrW(371) & "|" & ChrW(369) & "|" & ChrW(367) & "|" & ChrW(365) & "|" & ChrW(363) & "|" & ChrW(361) & "|" & ChrW(250) & "|" & ChrW(251) & "|" & ChrW(252) & "|" & ChrW(249), "u"), ChrW(372), "W"), ChrW(373), "w"), ChrW(376) & "|" & ChrW(374) & "|" & ChrW(221), "Y"), ChrW(375) & "|" & ChrW(255) & "|" & ChrW(253), "y"), ChrW(381) & "|" & ChrW(379) & "|" & ChrW(377), "Z"), ChrW(382) & "|" & ChrW(380) & "|" & ChrW(378), "z"), "[^a-z0-9_-" & ChrW(258) & ChrW(259) & ChrW(256) & ChrW(257) & ChrW(192) & ChrW(193) & ChrW(194) & ChrW(195) & ChrW(196) & ChrW(197) & ChrW(224) & ChrW(225) & ChrW(226) & ChrW(227) & ChrW(228) & ChrW(229) & ChrW(261) & ChrW(230) & ChrW(198) & ChrW(223) & ChrW(199) & ChrW(262) & ChrW(263) & ChrW(264) & ChrW(265) & ChrW(266) & ChrW(267) & ChrW(268) & ChrW(269) & ChrW(231) & ChrW(270) & ChrW(271) & ChrW(272) & ChrW(273) & ChrW(274) & ChrW(275) & ChrW(276) & ChrW(277) & ChrW(278) & ChrW(279) & ChrW(280) & ChrW(281) & ChrW(282) & ChrW(283) & ChrW(201) & ChrW(234) & ChrW(235) & ChrW(280) & ChrW(200) & ChrW(201) & ChrW(202) & ChrW(203) & ChrW(232) & ChrW(233) & ChrW(284) & ChrW(285) & ChrW(286) & ChrW(287) & ChrW(288) & ChrW(289) & ChrW(290) & ChrW(291) & ChrW(290) & ChrW(291) & ChrW(292) & ChrW(293) & ChrW(294) & ChrW(295) & ChrW(204) & ChrW(205) & ChrW(206) & ChrW(207) & ChrW(296) & ChrW(297) & ChrW(298) & ChrW(299) & ChrW(300) & ChrW(301) & ChrW(302) & ChrW(303) & ChrW(304) & ChrW(204) & ChrW(237) & ChrW(238) & ChrW(239) & ChrW(236) & ChrW(306) & ChrW(308) & ChrW(309) & ChrW(310) & ChrW(311) & ChrW(209) & ChrW(209) & ChrW(210) & ChrW(211) & ChrW(212) & ChrW(213) & ChrW(214) & ChrW(336) & ChrW(216) & ChrW(242) & ChrW(243) & ChrW(244) & ChrW(245) & ChrW(337) & ChrW(246) & ChrW(248) & ChrW(241) & ChrW(338) & ChrW(339) & ChrW(340) & ChrW(345) & ChrW(344) & ChrW(343) & ChrW(342) & ChrW(341) & ChrW(340) & ChrW(353) & ChrW(352) & ChrW(351) & ChrW(350) & ChrW(349) & ChrW(348) & ChrW(347) & ChrW(346) & ChrW(357) & ChrW(356) & ChrW(355) & ChrW(354) & ChrW(371) & ChrW(370) & ChrW(369) & ChrW(368) & ChrW(367) & ChrW(366) & ChrW(365) & ChrW(364) & ChrW(363) & ChrW(362) & ChrW(361) & ChrW(360) & ChrW(217) & ChrW(218) & ChrW(219) & ChrW(220) & ChrW(217) & ChrW(250) & ChrW(251) & ChrW(252) & ChrW(249) & ChrW(373) & ChrW(372) & ChrW(376) & ChrW(375) & ChrW(374) & ChrW(221) & ChrW(255) & ChrW(253) & ChrW(382) & ChrW(381) & ChrW(380) & ChrW(379) & ChrW(378) & ChrW(377) & "]+", "-", ignoreCase)).Replace("---", "-")
        strTitle = strTitle.Replace(Chr(160), "")
        If strTitle.EndsWith("-") Then
            strTitle = strTitle.Remove(strTitle.Length - 1)
        End If
        Return strTitle + "-" + EntryId.ToString()
    End Function
    Public Shared Function FormatLinkRSS(ByVal stringurl As String, ByVal Id As Integer, ByVal title As String) As String
        If Not Null.IsNull(Id) Then
            Return stringurl & "/" & BuildEntryLink(Id, title.ToLower()) & ".xml"
        Else
            Return "#"
        End If
    End Function
    Public Shared Sub NewsHardDelete(server As HttpServerUtility, newsid As Integer)
        Dim ctlNews As New NV_NewsController
        'Delete related media, files
        Dim ctlNewsMedia As New NewsByMediaController
        Dim ctl As New MediaItemController
        Dim arrNewsMedia As ArrayList = ctlNewsMedia._GetAllByNewId(newsid)
        For Each obj As NewsByMediaInfo In arrNewsMedia
            Dim objMedia As MediaItemInfo = ctl._GetByID(obj.mediaid)
            Try
                Dim mediaPath As String = server.MapPath(objMedia.MediaUrl)
                File.Delete(mediaPath)
            Catch ex As Exception
            End Try

            ctl._Delete(obj.mediaid)
        Next
        ctlNewsMedia._DeleteByNewId(newsid)
        'Delete News & Processes, versions, feedbacks, newsbycategory, newsbyuser
        ctlNews.Delete(newsid)
    End Sub
    Public Shared Function GetPhongBan(portalid As Integer) As ArrayList
        Dim ctlPhongBan As New PhongBanController
        Dim arrAll As ArrayList = ctlPhongBan.GetByParentId(portalid, 0)
        Dim arrTempParent As New ArrayList
        Dim arrResult As New ArrayList
        For Each obj As PhongBanInfo In arrAll
            If obj.ParentId = 0 AndAlso obj.IsActive = True Then
                arrTempParent.Add(obj)
            End If
        Next
        For Each objPB As PhongBanInfo In arrTempParent
            GetPhongBanList(ctlPhongBan.GetAll(portalid), objPB.Id, arrResult, "")
        Next

        Return arrResult
    End Function
    Public Shared Sub GetPhongBanList(ByVal arrPhongBan As ArrayList, ByVal ParentPhongBanId As Integer, ByRef arrReturn As ArrayList, ByVal Prefix As String)
        For Each objPhongBan As PhongBanInfo In arrPhongBan
            If objPhongBan.ParentId = ParentPhongBanId Then
                objPhongBan.TenPhongBan = Prefix & objPhongBan.TenPhongBan
                arrReturn.Add(objPhongBan)
            End If
        Next
    End Sub
    Public Shared Function GenerateQueryStringParameters(request As HttpRequest, queryStringKeys As String()) As String
        Dim queryString As New StringBuilder(64)
        For Each key As String In queryStringKeys
            If Not String.IsNullOrEmpty(request.QueryString(key)) Then
                If queryString.Length > 0 Then
                    queryString.Append("&")
                End If

                queryString.Append(key).Append("=").Append(request.QueryString(key))
            End If
        Next

        Return queryString.ToString()
    End Function
    Public Shared Function GetUserCategoriesByRole(portalid As Integer, userid As Integer, rolename As String) As ArrayList
        Dim objRoleController As New RoleController
        Dim objCtl As New NV_NewsCategoriesController
        Dim iRoleId As Integer = objRoleController.GetRoleByName(portalid, rolename).RoleID
        Dim arr As ArrayList = objCtl.GetAllUsersByRole(iRoleId)
        If userid = 0 Then
            Return arr
        End If
        Dim arrResult As New ArrayList
        For Each obj As UserInfo In arr
            If obj.UserID <> userid Then
                arrResult.Add(obj)
            End If
        Next

        Return arrResult
    End Function
    Public Shared Function CountLinkShare(Newid As Integer) As Integer
        Dim i As Integer = 0
        Dim ctl As New NewsByShareController
        i = Integer.Parse(ctl._GetCountbyNewId(Newid))
        Return i
    End Function
    Public Shared Sub UpdateNewsByTags(newsid As Integer, tags As String)
        Dim ctltags As New NewsByTagsController
        ctltags.NewsByTags_DeleteByNewId(newsid)
        If tags <> "" Then
            Dim tag2 As String() = tags.Split(CType(",", Char))
            For i2 As Integer = 0 To tag2.Length - 1
                Dim tagmoi As String = tag2(i2).Trim()
                'Dim tagmoi2 As String = ReplaceChuoi.tags(tagmoi)
                ctltags.NewsByTags_Insert(newsid, tagmoi, tagmoi, PortalSettings.Current.PortalId)
            Next
        End If
    End Sub
    Public Shared Function NewsDaXuatBanCanEdit(newsid As Integer, ByVal UserId As Integer) As Boolean
        Dim returnvalue As Boolean = True
        Dim objUserinfo As UserInfo = UserController.GetUserById(PortalSettings.Current.PortalId, UserId)
        Dim _NV_NewsController As New NV_NewsController
        Dim objnews = _NV_NewsController.GetByID(newsid)
        If Not objnews Is Nothing Then
            With objnews
                If .CanViewLock = False And objUserinfo.IsInRole("LanhDaoToaSoan") And objUserinfo.IsSuperUser Then
                    returnvalue = False
                End If
            End With
        End If
        Return returnvalue
    End Function
    Public Shared Function NewsDaXuatBanCanEditNgayXuatBan(ByVal UserId As Integer) As Boolean
        Dim returnvalue As Boolean = False
        Dim objUserinfo As UserInfo = UserController.GetUserById(PortalSettings.Current.PortalId, UserId)

        If objUserinfo.IsInRole("LanhDaoToaSoan") And objUserinfo.IsSuperUser Then
            returnvalue = True
        End If
        Return returnvalue
    End Function
#End Region
#Region "Security"
    ''' <summary>
    ''' Xoa toan bo cach server
    ''' </summary>
    Public Shared Sub XoaToanBoCacheServer()
        Try
            DataCache.ClearHostCache(True)
            DataCache.ClearCache()
            DataCache.ClearTabsCache(1)
            DataCache.ClearTabsCache(2)
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function GetSafeRawUrl(ByVal url As String) As String
        If String.IsNullOrEmpty(url) Then
            Return String.Empty
        End If
        Dim tProcessedRaw As String = url
        tProcessedRaw = tProcessedRaw.Replace("""", String.Empty)
        tProcessedRaw = tProcessedRaw.Replace("<", "%3C")
        tProcessedRaw = tProcessedRaw.Replace(">", "%3E")
        tProcessedRaw = tProcessedRaw.Replace("&", "%26")

        Return tProcessedRaw.Replace("'", String.Empty)
    End Function
    Public Shared Function isValidNumber(o As Object) As Boolean
        Dim i As Integer = 0
        If TypeOf o Is Integer Then
            i = CInt(o)
        ElseIf TypeOf o Is String Then
            Integer.TryParse(TryCast(o, String), i)
        End If

        If 0 < i AndAlso i <= 1000000 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function TryParseInt(o As Object, ByRef i As Integer) As Boolean
        i = 0
        If TypeOf o Is Integer Then
            i = CInt(o)
            Return True
        ElseIf TypeOf o Is String Then
            Return Integer.TryParse(TryCast(o, String), i)
        End If

        Return False
    End Function
    Public Shared Function IsValidEmail(email As String) As Boolean
        Return Regex.IsMatch(email, "^([0-9a-z]+[-._+&])*[0-9a-z]+@([-0-9a-z]+[.])+[a-z]{2,6}$", RegexOptions.IgnoreCase)
    End Function
    Public Shared Function IsValidURL(url As String) As Boolean
        Return Regex.IsMatch(url, "^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$")
    End Function
    Public Shared Function IsValidInt(val As String) As Boolean
        Return Regex.IsMatch(val, "^[1-9]\d*\.?[0]*$")
    End Function
#End Region
#Region "Media Handlers"
    Public Shared Sub MoveMedia(server As HttpServerUtility, portalid As Integer, newsid As Integer, userid As Integer, username As String)
        Try
            Dim storagePath As String = GetMediaStoragePath(False, portalid, True)
            Dim backupPath As String = GetBackUpPath(False, portalid, True)

            Dim ctlNewsByCategory As New NewsByMediaController
            Dim ctlMedia As New AIMediaItemController
            Dim arrNewsByCategory As ArrayList = ctlNewsByCategory._GetAllByNewId(newsid)
            For Each objNbC As NewsByMediaInfo In arrNewsByCategory
                Dim objMedia As AIMediaItemInfo = ctlMedia.GetById(objNbC.mediaid)

                Dim sourcePath As String = server.MapPath(objMedia.ItemGUID + "/" + objMedia.Folder) + "\" + objMedia.Title
                'TODO: Dieu chinh HERE
                Dim newFileName As String = objMedia.Title
                '1. Nếu tồn tại file, đặt tên mới: Tenfile-username_HH_mm_fff
                'If File.Exists(desAssumedPath) Then
                '    newFileName = Path.GetFileNameWithoutExtension(curFileName) + "-" + username + "_" + DateTime.Now.ToString("HH_mm_ss_fff") + Path.GetExtension(curFileName)
                'End If
                Dim desPath As String = storagePath + "\" + newFileName
                Dim buPath As String = backupPath + "\" + newFileName
                Try
                    File.Copy(sourcePath, desPath)
                    'For Backup
                    File.Copy(sourcePath, buPath)
                Catch ex As Exception
                End Try
                '2. Update Media path
                objMedia.ItemGUID = PortalController.GetPortalSetting(BL.settingMediaLuuTruVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
                objMedia.MediaUrl = objMedia.ItemGUID + "/" + GetStorageFolder() + "/" + newFileName
                objMedia.Status = Vbuzz.Modules.Media.MediaStatus.Published
                objMedia.Folder = GetStorageFolder()
                objMedia.PublishedUser = userid
                objMedia.PulishedDate = DateTime.Now

                ctlMedia.Update(objMedia)
                'TODO: ashx hanler after this: 
                'FileSystemUtils.DeleteFile(Server.MapPath(objMedia.MediaUrl))
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub Export2Dalet_Netia(server As HttpServerUtility, ByVal portalid As Integer, ByVal arrMedia As String, ByVal ver As String)
        Try
            If Not String.IsNullOrEmpty(arrMedia) Then
                Dim destinationDALET As String = GetDALETPath(portalid)
                Dim destinationNETIA As String = GetNETIAPath(portalid)
                Dim destinationMultiMedia1 As String = CreateStorage(GetMultiMediaPath1(portalid))

                If Not String.IsNullOrEmpty(destinationDALET) Then
                    Dim strArr As String() = arrMedia.Split(CType(";", Char))
                    For i As Integer = 0 To strArr.Length - 1
                        If strArr(i) <> "" Then
                            Dim folder As String = strArr(i).Split("|")(0)
                            'Dim desPath As String = desDalet + "\" + ver + Path.GetFileName(strArr(i).ToString)
                            Dim desPath As String = destinationDALET + "\" + ver + BL.ConvertTiengVietCoDauThanhKhongDauV1(Path.GetFileName(strArr(i).Split("|")(1).ToString))
                            Dim desbuPath As String = destinationMultiMedia1 + "\" + ver + BL.ConvertTiengVietCoDauThanhKhongDauV1(Path.GetFileName(strArr(i).Split("|")(1).ToString))
                            Dim sourcePath As String = server.MapPath(folder) + "\" + strArr(i).Split("|")(1).ToString
                            Try
                                If File.Exists(sourcePath) Then
                                    File.Copy(sourcePath, desPath)
                                    'For Backup
                                    File.Copy(sourcePath, desbuPath)
                                End If
                            Catch ex As Exception
                            End Try
                        End If
                    Next
                End If
                If Not String.IsNullOrEmpty(destinationNETIA) Then
                    Dim strArr As String() = arrMedia.Split(CType(";", Char))
                    For i As Integer = 0 To strArr.Length - 1
                        If strArr(i) <> "" Then
                            Dim folder As String = strArr(i).Split("|")(0)
                            Dim desPath As String = destinationNETIA + "\" + ver + BL.ConvertTiengVietCoDauThanhKhongDauV1(Path.GetFileName(strArr(i).Split("|")(1).ToString))
                            Dim sourcePath As String = server.MapPath(folder) + "\" + strArr(i).Split("|")(1).ToString
                            If File.Exists(sourcePath) Then
                                File.Copy(sourcePath, desPath)
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub InsertMediaFiles(server As HttpServerUtility, portalid As Integer, userid As Integer, newsid As Integer, catid As Integer, ByVal arrMedia As String)
        Try
            If Not String.IsNullOrEmpty(arrMedia) Then
                '1. Delete media related to this News
                Dim ctlNewsMedia As New NewsByMediaController
                Dim ctl As New Vbuzz.Modules.Media.AIMediaItemController
                'Xóa trong Media, NewsByMedia: ALREADY CHECKED IN STOREPROCEDURE                
                Try
                    Dim arrNewsMedia As ArrayList = ctlNewsMedia._GetAllByNewId(newsid)
                    For Each obj As NewsByMediaInfo In arrNewsMedia
                        ctl.Delete(obj.mediaid)
                    Next
                    ctlNewsMedia._DeleteByNewId(newsid)
                Catch ex As Exception
                End Try
                '2. Insert new files
                Dim strArr As String() = arrMedia.Split(CType(";", Char))
                For i As Integer = 0 To strArr.Length - 1
                    If strArr(i) <> "" Then
                        Dim folder As String = GetUploadPath(True, portalid, True) '/VOV-DATA/MEDIA/2013/09
                        Dim fileName As String = strArr(i)
                        'i. thêm mới media item
                        Dim item As Vbuzz.Modules.Media.AIMediaItemInfo = CollectMediaData(portalid, userid, catid, Nothing, folder, fileName, server.MapPath(folder) + "\" + fileName)
                        item.ItemGUID = folder.Substring(0, folder.Length - 8) 'Folder Type: Upload, FTP, DALET
                        item.Folder = folder.Substring(folder.Length - 7)
                        Dim iMediaId As Integer = ctl.Add(item)
                        'ii. Insert Media -> Category
                        Dim mediaCateInf As New Vbuzz.Modules.Media.AIMediaItemInCateInfo
                        mediaCateInf.MediaItemId = newsid
                        mediaCateInf.CateId = catid
                        Dim mediaCateCtl As New Vbuzz.Modules.Media.AIMediaItemInCateController
                        mediaCateCtl.Add(mediaCateInf)

                        Dim objNewsByMediaInfo As New NewsByMediaInfo
                        With objNewsByMediaInfo
                            .newid = newsid
                            .mediaid = iMediaId
                        End With
                        ctlNewsMedia._Insert(newsid, iMediaId, DateTime.Now, userid, portalid)
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function CollectMediaData(portalid As Integer, userid As Integer, catid As Integer, ByVal item As AIMediaItemInfo, folder As String, filename As String, ByVal filePath As String) As AIMediaItemInfo
        Try
            If item Is Nothing Then
                ' trường hợp thêm mới
                item = New Vbuzz.Modules.Media.AIMediaItemInfo
                item.CreatedItem = DateTime.Now
                item.CreatedUser = userid
            Else
                ' trường hợp cập nhật

            End If
            item.Title = Path.GetFileName(filePath)
            item.Description = Path.GetFileName(filePath)
            item.LastUpdatedUser = userid
            item.LastModifiedDate = Date.Now

            ' thông tin về media file
            Dim mediaFile As New System.IO.FileInfo(filePath)
            If mediaFile.Exists Then
                item.MediaUrl = folder + "/" + filename
                item.Extension = mediaFile.Extension.ToLower()
                item.Size = CType(mediaFile.Length, Integer)
            End If
            item.Status = MediaStatus.Created
            item.PortalId = portalid
            item.LanguageId = "vi-VN"
            Select Case mediaFile.Extension.ToLower
                Case ".wma", ".wmv", ".wav", ".mp3"
                    item.TypeId = 1 'Audio
                Case ".flv", ".mp4", ".mpeg"
                    item.TypeId = 2 'Video
                Case Else
                    item.TypeId = 3 'Images
            End Select
            item.TECHLVL_ID = catid

            Return item
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetUploadPath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(BL.settingMediaPathPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingMediaPathVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(BL.settingMediaPathVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingMediaPathPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(BL.settingMediaPathPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    Public Shared Function GetMediaStoragePath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(BL.settingMediaLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingMediaLuuTruVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(BL.settingMediaLuuTruVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingMediaLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(BL.settingMediaLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    Public Shared Function GetBackUpPath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(BL.settingBackupPathPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingBackupPathVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(BL.settingBackupPathVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingBackupPathPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(BL.settingBackupPathPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    Public Shared Function GetImagePath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(BL.settingAnhLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingAnhLuuTruVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(BL.settingAnhLuuTruVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingAnhLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(BL.settingAnhLuuTruPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    Public Shared Function GetFlashPath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(BL.settingFlashPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingFlashVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(BL.settingFlashVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingFlashPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(BL.settingFlashPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    Public Shared Function GetBaiHatPath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(BL.settingBaiHatPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingBaiHatVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(BL.settingBaiHatVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingBaiHatPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(BL.settingBaiHatPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function
    Public Shared Function GetVideoPath(ByVal isVirtual As Boolean, ByVal portalid As Integer, ByVal includedTimeStamp As Boolean) As String
        CreateStorage(PortalController.GetPortalSetting(BL.settingVideoPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString))
        Dim destination As String
        Dim timeStamp As String = GetStorageFolder()
        If isVirtual Then
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingVideoVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "/" + timeStamp
            Else
                destination = PortalController.GetPortalSetting(BL.settingVideoVirtual, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        Else
            If includedTimeStamp Then
                destination = PortalController.GetPortalSetting(BL.settingVideoPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString) + "\" + timeStamp.Replace("/", "\")
            Else
                destination = PortalController.GetPortalSetting(BL.settingVideoPhysical, portalid, DotNetNuke.Common.Utilities.Null.NullString)
            End If
        End If

        Return destination
    End Function

    Public Shared Function GetDALETPath(portalid As Integer) As String
        Dim destination As String = String.Empty
        Dim destinationFolder As String = PortalController.GetPortalSetting(BL.settingDalet, portalid, DotNetNuke.Common.Utilities.Null.NullString)
        If PathHelper.IsPhysicalPath(destinationFolder) Then
            destination = destinationFolder
        End If

        Return destination
    End Function
    Public Shared Function GetNETIAPath(portalid As Integer) As String
        Dim destination As String = String.Empty
        Dim destinationFolderNETIA As String = PortalController.GetPortalSetting(BL.settingNetia, portalid, DotNetNuke.Common.Utilities.Null.NullString)
        If PathHelper.IsPhysicalPath(destinationFolderNETIA) Then
            destination = destinationFolderNETIA
        End If

        Return destination
    End Function
    Public Shared Function GetMultiMediaPath1(portalid As Integer) As String
        Dim destination As String = String.Empty
        Dim destinationFolder As String = PortalController.GetPortalSetting(BL.settingMultiMediaCopyPath1, portalid, DotNetNuke.Common.Utilities.Null.NullString)
        If PathHelper.IsPhysicalPath(destinationFolder) Then
            destination = destinationFolder
        End If

        Return destination
    End Function
    Public Shared Function GetMultiMediaPath2(portalid As Integer) As String
        Dim destination As String = String.Empty
        Dim destinationFolder As String = PortalController.GetPortalSetting(BL.settingMultiMediaCopyPath2, portalid, DotNetNuke.Common.Utilities.Null.NullString)
        If PathHelper.IsPhysicalPath(destinationFolder) Then
            destination = destinationFolder
        End If

        Return destination
    End Function
    Public Shared Function GetMultiMediaPath3(portalid As Integer) As String
        Dim destination As String = String.Empty
        Dim destinationFolder As String = PortalController.GetPortalSetting(BL.settingMultiMediaCopyPath3, portalid, DotNetNuke.Common.Utilities.Null.NullString)
        If PathHelper.IsPhysicalPath(destinationFolder) Then
            destination = destinationFolder
        End If

        Return destination
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
    Public Shared Function CreateAvatarDir(ByVal destination As String) As String
        If Not Directory.Exists(destination + "\Avatar") Then
            Directory.CreateDirectory(destination + "\Avatar")
        End If

        Return destination + "\Avatar"
    End Function
    Public Shared Function GetStorageFolder() As String
        Return Now.ToString("yyyy") + "/" + Now.ToString("MM") + "/" + Now.ToString("dd")
    End Function
    'Public Shared Function GetVideoDuration(ByVal file As String, Newid As Integer) As String
    '    Dim strCacheKey As String
    '    strCacheKey = "VideoDurationDetail:" & Newid
    '    Dim strResult As String = "PT"
    '    'strResult = DataCache.GetCache(strCacheKey)
    '    If DataCache.GetCache(strCacheKey) = "" Then
    '        Dim wmp As WindowsMediaPlayer = New WindowsMediaPlayerClass()
    '        Dim mediainfo As IWMPMedia = wmp.newMedia(file)
    '        Dim videotime As TimeSpan = TimeSpan.FromSeconds(mediainfo.duration)
    '        Dim videogio = videotime.ToString("hh")
    '        If videogio > 0 Then
    '            strResult += videogio & "H"
    '        End If
    '        Dim videophut = videotime.ToString("mm")
    '        If videophut > 0 Then
    '            strResult += videophut & "M"
    '        End If
    '        Dim videogiay = videotime.ToString("ss")
    '        If videogiay > 0 Then
    '            strResult += videogiay & "S"
    '        End If
    '        DataCache.SetCache(strCacheKey, strResult)
    '    Else
    '        strResult = DataCache.GetCache(strCacheKey)
    '    End If
    '    Return strResult
    'End Function
    'Public Shared Function GetVideoDurationSecond(ByVal file As String) As String
    '    Dim strResult As String = ""
    '    Dim wmp As WindowsMediaPlayer = New WindowsMediaPlayerClass()
    '    Dim mediainfo As IWMPMedia = wmp.newMedia(file)
    '    strResult = mediainfo.duration.ToString()
    '    Return strResult

    'End Function
    Public Shared Function convertVideoDuration(duration As Integer) As String
        Dim strCacheKey As String
        strCacheKey = "convertVideoDuration:" & duration
        Dim strResult As String = "PT"
        'strResult = DataCache.GetCache(strCacheKey)
        If DataCache.GetCache(strCacheKey) = "" Then
            Dim videotime As TimeSpan = TimeSpan.FromSeconds(duration)
            Dim videogio = videotime.ToString("hh")
            If videogio > 0 Then
                strResult += videogio & "H"
            End If
            Dim videophut = videotime.ToString("mm")
            If videophut > 0 Then
                strResult += videophut & "M"
            End If
            Dim videogiay = videotime.ToString("ss")
            If videogiay > 0 Then
                strResult += videogiay & "S"
            End If
            DataCache.SetCache(strCacheKey, strResult)
        Else
            strResult = DataCache.GetCache(strCacheKey)
        End If
        Return strResult
    End Function
#End Region
#Region "Function"
    Public Shared Function SubString(ByVal sInput As String, ByVal inCount As Integer, ByVal sSpace As String) As String
        If String.IsNullOrEmpty(sInput) Then
            Return String.Empty
        End If
        Dim sReturn As String = ""
        Dim sInputChars As String() = sInput.Split(" ")
        If (sInputChars.Length <= inCount) Then
            sReturn = sInput
        Else
            For i As Integer = 0 To (inCount - 1)
                sReturn = sReturn + sInputChars(i) + " "
            Next
        End If
        If Not String.IsNullOrEmpty(sSpace) Then
            sReturn += sSpace
        End If
        Return sReturn
    End Function
    Public Shared Function ToRelativeDate(ByVal dateTime__1 As DateTime) As String
        Dim timeSpan__2 = DateTime.Now - dateTime__1

        If timeSpan__2 <= TimeSpan.FromSeconds(60) Then
            Return String.Format("{0} giây trước", timeSpan__2.Seconds)
        End If

        If timeSpan__2 <= TimeSpan.FromMinutes(60) Then
            Return If(timeSpan__2.Minutes > 1, [String].Format("{0} phút trước", timeSpan__2.Minutes), "khoảng 1 phút trước")
        End If

        If timeSpan__2 <= TimeSpan.FromHours(24) Then
            Return If(timeSpan__2.Hours > 1, [String].Format("{0} giờ trước", timeSpan__2.Hours), "khoảng 1 giờ trước")
        End If

        If timeSpan__2 <= TimeSpan.FromDays(30) Then
            Return If(timeSpan__2.Days > 1, [String].Format("{0} ngày trước", CInt(timeSpan__2.Days)).ToLower, "hôm qua")
        End If

        If timeSpan__2 <= TimeSpan.FromDays(365) Then
            Return If(timeSpan__2.Days > 30, [String].Format("{0} tháng trước", CInt(timeSpan__2.Days \ 30)).Trim, "khoảng 1 tháng trước")
        End If

        Return If(timeSpan__2.Days > 365, [String].Format("{0} năm trước", timeSpan__2.Days \ 365), "Khoảng một năm trước")
    End Function
    Public Shared Function GetYouTubeVideoIdFromUrl(ByVal url As String) As String
        Dim uri As Uri = Nothing

        If Not Uri.TryCreate(url, UriKind.Absolute, uri) Then

            Try
                uri = New UriBuilder("http", url).Uri
            Catch
                Return ""
            End Try
        End If

        Dim host As String = uri.Host
        Dim youTubeHosts As String() = {"www.youtube.com", "youtube.com", "youtu.be", "www.youtu.be"}
        If Not youTubeHosts.Contains(host) Then Return ""
        Dim query = HttpUtility.ParseQueryString(uri.Query)

        If query.AllKeys.Contains("v") Then
            Return Regex.Match(query("v"), "^[a-zA-Z0-9_-]{11}$").Value
        ElseIf query.AllKeys.Contains("u") Then
            Return Regex.Match(query("u"), "/watch\?v=([a-zA-Z0-9_-]{11})").Groups(1).Value
        Else
            Dim last = uri.Segments.Last().Replace("/", "")

            If Regex.IsMatch(last, "^v=[a-zA-Z0-9_-]{11}$") Then
                Return last.Replace("v=", "")
            Else
                Return last
            End If
            Dim segments As String() = uri.Segments
            If segments.Length > 2 AndAlso segments(segments.Length - 2) <> "v/" AndAlso segments(segments.Length - 2) <> "watch/" Then
                Return ""
            End If
            Return Regex.Match(last, "^[a-zA-Z0-9_-]{11}$").Value
        End If
    End Function
    Public Shared Function GetLinkFromIframe(ByVal url As String) As String
        Dim sresult As String = ""
        Dim htmlDoc As HtmlDocument = New HtmlDocument
        htmlDoc.LoadHtml(url)
        If (htmlDoc.DocumentNode.SelectNodes("//iframe") IsNot Nothing) Then
            For Each img As HtmlNode In htmlDoc.DocumentNode.SelectNodes("//iframe")
                Dim att As HtmlAttribute = img.Attributes("src")
                sresult = att.Value
            Next
        End If
        Return sresult
    End Function
#End Region
#Region "Functions Get Files from DALET, FTP,.."
    Public Shared Function GetFilesByFolder(currentPage As Integer, ByVal source As Integer, ByVal type As Integer, ByVal key As String, pagesize As Integer) As DataSet
        Dim strFolderName As String = String.Empty
        If source = SourcesType.FTP Then
            strFolderName = PortalController.GetPortalSetting(BL.settingFTPVirtual, 0, DotNetNuke.Common.Utilities.Null.NullString)
        ElseIf source = SourcesType.UPLOAD Then
            strFolderName = PortalController.GetPortalSetting(BL.settingMediaPathVirtual, 0, DotNetNuke.Common.Utilities.Null.NullString)
        End If
        Dim dsResult As New DataSet
        Dim tblFiles As DataTable = GetFileTable()

        If Not String.IsNullOrEmpty(strFolderName) Then
            Dim ctl As New AIMediaItemController
            Dim totalcount As Integer = ctl.FindFiles_Count(strFolderName, type, key, BL.minDateV, BL.maxDateV)
            Dim arrFiles As ArrayList = ctl.FindFiles_Index(strFolderName, type, key, BL.minDateV, BL.maxDateV, 1, currentPage, pagesize)
            For Each objFile As AIMediaItemInfo In arrFiles
                AddFileToTable(source, type, tblFiles, objFile, totalcount)
            Next
        End If

        dsResult.Tables.Add(tblFiles)

        Return dsResult
    End Function
    Public Shared Sub AddFileToTable(ByVal source As Integer, ByVal type As Integer, ByVal tblFiles As DataTable, ByVal objFile As AIMediaItemInfo, ByVal totalcount As Integer)
        Dim dRow As DataRow
        dRow = tblFiles.NewRow
        'dRow("FileType") = "File"
        dRow("FileId") = objFile.Id
        dRow("FileName") = BL.FormatPlayer(source, type, objFile.ItemGUID + "/" + objFile.Folder, objFile.Id.ToString(), objFile.Title, objFile.MediaUrl)
        dRow("FilePath") = objFile.MediaUrl
        dRow("FileSize") = String.Format("{0:F} Mb", objFile.Size / 1024 / 1024)
        dRow("DateModified") = objFile.CreatedItem.ToString("HH:mm dd/MM/yy")
        dRow("TotalPage") = (totalcount \ 20 + IIf((totalcount Mod 20 = 0), 0, 1)).ToString

        tblFiles.Rows.Add(dRow)
    End Sub
    Public Shared Function GetAttributeString(ByVal attributes As System.IO.FileAttributes) As String
        Dim strResult As String = ""
        If (attributes And FileAttributes.Archive) = FileAttributes.Archive Then
            strResult += "A"
        End If
        If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
            strResult += "R"
        End If
        If (attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
            strResult += "H"
        End If
        If (attributes And FileAttributes.System) = FileAttributes.System Then
            strResult += "S"
        End If
        Return strResult
    End Function
    Public Shared Function GetFileTable() As DataTable

        Dim tblFiles As New DataTable("tblFiles")

        Dim myColumns As New DataColumn
        myColumns.DataType = System.Type.GetType("System.String")
        myColumns.ColumnName = "FileType"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.Int32")
        myColumns.ColumnName = "FileId"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.String")
        myColumns.ColumnName = "FileName"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.String")
        myColumns.ColumnName = "FilePath"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.String")
        myColumns.ColumnName = "FileSize"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.Int32")
        myColumns.ColumnName = "IntFileSize"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.Int32")
        myColumns.ColumnName = "StorageLocation"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.String")
        myColumns.ColumnName = "DateModified"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.String")
        myColumns.ColumnName = "Extension"
        tblFiles.Columns.Add(myColumns)

        myColumns = New DataColumn
        myColumns.DataType = System.Type.GetType("System.Int32")
        myColumns.ColumnName = "TotalPage"
        tblFiles.Columns.Add(myColumns)

        Return tblFiles
    End Function
#End Region
#Region "Lock - Unlock - Visible News"
    Public Shared Sub UnlockNews(ByVal newsid As Integer, userid As Integer)
        Try
            Dim ctlNews As New NV_NewsController
            ctlNews.UpdateLock(newsid, False, userid)
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub LockNews(ByVal newsid As Integer, userid As Integer)
        Try
            Dim ctlNews As New NV_NewsController
            ctlNews.UpdateLock(newsid, True, userid)
        Catch ex As Exception
        End Try
    End Sub
    ''' <summary>
    ''' Hàm kiểm tra 1 tin có bị LOCK hay không
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function CheckNewsLock(portalid As Integer, ByVal newsid As Integer) As Boolean
        Dim ctlNews As New NV_NewsController
        Dim arr As ArrayList = ctlNews.GetLock(portalid, newsid)
        If arr.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function FormatLock(portalid As Integer, ByVal newsid As Integer) As Boolean
        If Ultis.CheckNewsLock(portalid, newsid) = True Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function FormatLockByUser(portalid As Integer, ByVal newsid As Integer, userid As Integer) As Boolean
        Dim ctlNews As New NV_NewsController
        Dim objNews As NV_NewsInfo = ctlNews.GetByID(newsid)
        If objNews.IsEdited Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function FormatLockByUserDangoanThao(portalid As Integer, ByVal newsid As Integer, userid As Integer) As Boolean
        Dim ctlNews As New NV_NewsController
        Dim objNews As NV_NewsInfo = ctlNews.GetByID(newsid)
        If objNews.IsEdited AndAlso (objNews.Status = NewsStatus.DangBienSoan Or NewsStatus.BiTraLai) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function FormatVisibleByStatus(portalid As Integer, ByVal canaction As Boolean, ByVal newsid As Integer) As Boolean
        If Ultis.CheckNewsLock(portalid, newsid) = True Then
            Return False
        End If
        If canaction Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function FormatTrieuHoi(portalid As Integer, ByVal newsid As Integer, ByVal status As Integer, userid As Integer, approvalUser As Integer) As Boolean
        'NẾU BÀI ĐANG LOCK => OUT
        If Ultis.CheckNewsLock(portalid, newsid) Then
            Return False
        End If

        If (status = NewsStatus.ChoPheDuyet OrElse (status = NewsStatus.ChoXuatBan AndAlso (userid = approvalUser OrElse (Not approvalUser > 0)))) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function GetNameStatusName(ByVal id As Integer) As String
        Dim ctlNewsCategories As New NV_NewsStatusController
        Dim objNewsCategories As NV_NewsStatusInfo
        objNewsCategories = ctlNewsCategories.NV_NewsStatus_GetByID(id)
        If Not objNewsCategories Is Nothing Then
            With objNewsCategories
                If (.NewsStatusId = 0) Then
                    Return "<span class='tb-status text-warning'>Đang biên soạn</span>"
                End If
                If (.NewsStatusId = 1) Then
                    Return "<span class='tb-status text-info'>Chờ phê duyệt</span>"
                End If
                If (.NewsStatusId = 2) Then
                    Return "<span class='tb-status text-primary'>Đã xuất bản</span>"
                End If
                If (.NewsStatusId = 3) Then
                    Return "<span class='tb-status text-gray'>Bị trả lại</span>"
                End If
                If (.NewsStatusId = 4) Then
                    Return "<span class='tb-status text-danger'>Huỷ xuất bản</span>"
                End If
                If (.NewsStatusId = 5) Then
                    Return "<span class='tb-status text-success'>Chờ xuất bản</span>"
                End If

            End With
            Return objNewsCategories.StatusName
        Else
            Return "--"
        End If
    End Function
#End Region
#Region "Format: Color, Text, Icon,.."
    Public Shared Function FormatEdittedBy(portalid As Integer, ByVal newsid As Integer) As String
        Dim ctl As New NV_NewsController
        Dim objInfo As NV_NewsInfo = ctl.GetByID(newsid)
        If Not objInfo Is Nothing Then
            Return CType(("Tin bài đang được chỉnh sửa bởi: <strong>" + BL.GetNameByUserId(portalid, objInfo.EditedUser) + "</strong>, khoảng " + IIf(Now.Subtract(objInfo.EditedTime).Days = 0, "", Now.Subtract(objInfo.EditedTime).Days.ToString() + " ngày, ") + IIf(Now.Subtract(objInfo.EditedTime).Hours = 0, "", Now.Subtract(objInfo.EditedTime).Hours.ToString() + " giờ, ") + IIf(Now.Subtract(objInfo.EditedTime).Minutes = 0, "< 1", Now.Subtract(objInfo.EditedTime).Minutes.ToString()) + " phút trước"), String)
        Else
            Return ""
        End If
    End Function
    Public Shared Function FormatReturndBy(portalid As Integer, ByVal newsid As Integer) As String
        Dim ctl As New NV_NewsController
        Dim objInfo As NV_NewsInfo = ctl.GetByID(newsid)
        If Not objInfo Is Nothing Then
            Return CType(("Tin bài được trả lại bởi: <strong>" + BL.GetNameByUserId(portalid, objInfo.ReturnedUser) + "</strong>, khoảng " + IIf(Now.Subtract(objInfo.ReturnedDate).Days = 0, "", Now.Subtract(objInfo.EditedTime).Days.ToString() + " ngày, ") + IIf(Now.Subtract(objInfo.EditedTime).Hours = 0, "", Now.Subtract(objInfo.EditedTime).Hours.ToString() + " giờ, ") + IIf(Now.Subtract(objInfo.EditedTime).Minutes = 0, "< 1", Now.Subtract(objInfo.EditedTime).Minutes.ToString()) + " phút trước. <br /><strong style='color:red'>Nội dung: " + objInfo.Note + "</strong>"), String)
        Else
            Return ""
        End If
    End Function
    Public Shared Function NewsNotes(portalid As Integer, ByVal newsid As Integer) As String
        Dim ctl As New NewsNoteController
        Dim objInfo As NewsNoteInfo = ctl.News_Note_GetByNewIdTop1(newsid)
        If Not objInfo Is Nothing Then
            With objInfo
                Return CType(("<em class='icon ni ni-quote-left'></em>Lời nhắn: " + .Noidung), String)
            End With
        Else
            Return ""
        End If
    End Function
    Public Shared Function NewsNotesShow(portalid As Integer, ByVal newsid As Integer) As Boolean
        Dim sresult As Boolean = False
        Dim ctl As New NewsNoteController
        Dim objInfo As NewsNoteInfo = ctl.News_Note_GetByNewIdTop1(newsid)
        If Not objInfo Is Nothing Then
            With objInfo
                sresult = True
            End With
        End If
        Return sresult
    End Function
    Public Shared Function FormatTinThuocPhongBan(portalId As Integer, ByVal phongbanid As Integer, userid As Integer) As System.Drawing.Color
        If phongbanid = BL.GetPhongBanIdByUserId(portalId, userid) Then
            Return Color.Black
        Else
            Return Color.Chocolate
        End If
    End Function
    Public Shared Function FormatHenGioXB(ByVal status As Integer) As String
        If status = NewsStatus.DaXuatBan Then
            Return "False"
        Else
            Return "True"
        End If
    End Function
    Public Shared Function FormatColorGetNews(ByVal newsid As Integer, userid As Integer) As System.Drawing.Color
        Dim ctl As New ViewNewsController
        Dim obj As ViewNewsInfo = ctl.GetByNewsIdAndUserId(newsid, userid)
        If Not obj Is Nothing Then
            Return Color.DarkGray
        Else
            Return System.Drawing.Color.DarkBlue
        End If
    End Function
    Public Shared Function FormatColor(portalid As Integer, userid As Integer, ByVal UsersView As String, ByVal UsersGet As String) As System.Drawing.Color
        Dim tenViettat = BL.GetPhongBanTenViettat(BL.GetPhongBanIdByUserId(portalid, userid))
        If BL.ContainPatternInString(UsersGet, tenViettat, "-") Then
            Return System.Drawing.Color.FromName("#265a8a")
        ElseIf BL.ContainPatternInString(UsersView, userid.ToString, ",") Then
            Return Color.DarkGray
        Else
            Return System.Drawing.Color.Black
        End If
    End Function
    'Public Shared Function FormatTooltip(portalid As Integer, ByVal title As String, ByVal publishedDate As DateTime, ByVal userId As Integer, ByVal content As String) As String
    '    Return String.Format("{0}|<font style='color:maroon;'>Ngày xuất bản: {1}</font>|<font style='color:maroon;padding-bottom:5px;'>Tác giả: {2}</font>|{3}", title, publishedDate.ToString("HH:mm dd/MM/yyyy"), BL.GetNameByUserId(PortalId, userId), BL.FormatText(BL.StripHTML(Server.HtmlDecode(content)), 300))
    'End Function
    Public Shared Function FormatTooltip(portalid As Integer, ByVal userId As Integer, ByVal categoryname As String) As String
        Return String.Format("Tác giả: {0} | Chuyên mục chính: {1}", BL.GetNameByUserId(portalid, userId), categoryname)
    End Function
    Public Shared Function FormatTooltipClueTip(portalid As Integer, ByVal title As String, ByVal publishedDate As DateTime, ByVal userId As Integer, ByVal categoryname As String) As String
        Return String.Format("{0}|<font style='color:maroon;'>Ngày xuất bản: {1}</font>|<font style='color:maroon;padding-bottom:5px;'>Tác giả: {2}</font>|<font style='color:maroon;padding-bottom:5px;'>Chuyên mục chính: {3}</font>", title, BL.FormatDate(publishedDate), BL.GetNameByUserId(portalid, userId), categoryname)
    End Function
    Public Shared Function FormatViewColor(portalid As Integer, userid As Integer, ByVal UsersView As String, ByVal UsersGet As String) As System.Drawing.Color
        Dim tenViettat = BL.GetPhongBanTenViettat(BL.GetPhongBanIdByUserId(portalid, userid))
        If BL.ContainPatternInString(UsersGet, tenViettat, "-") Then
            Return System.Drawing.Color.FromName("#993300")
        ElseIf BL.ContainPatternInString(UsersView, userid.ToString, ",") Then
            Return System.Drawing.Color.FromName("#585858")
        Else
            Return System.Drawing.Color.Black
        End If
    End Function
    Public Shared Function FormatViewColorByName(portalid As Integer, userid As Integer, ByVal UsersView As String, ByVal UsersGet As String) As String
        Dim tenViettat = BL.GetPhongBanTenViettat(BL.GetPhongBanIdByUserId(portalid, userid))
        If BL.ContainPatternInString(UsersGet, tenViettat, "-") Then
            Return "#993300"
        ElseIf BL.ContainPatternInString(UsersView, userid.ToString, ",") Then
            Return "#585858"
        Else
            Return "#000000"
        End If
    End Function
#End Region
#Region "WorkFlow"
    Public Shared Function LoadWorkFlow(ByVal portalid As Integer, ByVal userin As UserInfo) As ArrayList
        Dim arrResult As New ArrayList
        If userin.IsInRole("Phe duyet") OrElse userin.IsInRole("Xuat ban") OrElse userin.IsInRole("Manager") OrElse userin.IsInRole("Administrators") Then
            arrResult = New News_UserWFController().GetByPhongBanId(LoaiWF.DanhChoLanhDaoPhong, BL.GetPhongBanIdByUserId(portalid, userin.UserID))
        Else
            arrResult = New News_UserWFController().GetByPhongBanId(LoaiWF.DanhChoPhongVien, BL.GetPhongBanIdByUserId(portalid, userin.UserID))
        End If

        Return arrResult
    End Function

    Public Shared Sub Save2Version(ByVal obj As NV_NewsInfo, userid As Integer)
        Try
            Dim ctlVersion As New NewsVersionController
            Dim objVersion As New NewsVersionInfo
            Dim ctlProcess As New NewsProcessController

            With objVersion
                .NewId = obj.NewId
                .CategoryId = obj.CategoryId
                .Title = obj.Title
                .ImagePath = obj.ImagePath
                .Summary = obj.Summary
                .Content = obj.Content
                .isActive = obj.isActive
                .Hotcat = obj.Hotcat
                .Hotsite = obj.Hotsite
                .PortalId = obj.PortalId
                .UserId = obj.UserId
                .Status = obj.Status
                .Note = obj.Note
                .TypeUrl = obj.TypeUrl
                .CreateDate = DateTime.Now
                .ApprovalRequestDate = obj.ApprovalRequestDate
                .ApprovalDate = obj.ApprovalDate
                .ApprovalUser = obj.ApprovalUser
                .ReturnedDate = obj.ReturnedDate
                .ReturnedUser = obj.ReturnedUser
                .CancelPublishDate = obj.CancelPublishDate
                .CancelPublishUser = obj.CancelPublishUser
                .PublishedDate = obj.PublishedDate
                .PublishedUser = obj.CancelPublishUser
                .SourceInfo = obj.SourceInfo
                .Unit = obj.Unit
                .Type = obj.Type
                .NewsKind = obj.NewsKind
                .Tags = obj.Tags
                .IsImage = obj.IsImage
                .IsEdited = obj.IsEdited
                .StorageFolder = obj.StorageFolder
                .AttachedFiles = obj.AttachedFiles
                .EditedUser = obj.EditedUser
                .EditedTime = obj.EditedTime
                .Credit = obj.Credit
                .VoteCount = obj.VoteCount
                .ViewCount = obj.ViewCount
                .Links = obj.Links
                .IsArchived = obj.IsArchived

                .CreatedUser = userid 'Version created by WHOM
            End With
            Dim versionid As Integer = ctlVersion.Insert(objVersion)
            'Update Process wiz this version
            Dim objProcess As NewsProcessInfo = ctlProcess.GetCurrentProcess(obj.NewId)
            objProcess.VersionId = versionid
            ctlProcess.Update(objProcess)
        Catch ex As Exception
            ProcessPageLoadException(ex)
        End Try
    End Sub
#End Region
#Region "SMS - EMAIL"
    Public Shared Sub SendEmails(userin As UserInfo, tieude As String, arrUser As System.Collections.Generic.List(Of Entities.Users.UserInfo))
        Dim strResult As String = ""
        Dim msgResult As DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType
        Dim isValid As Boolean = True
        Dim intMailsSent As Integer = -1
        Try
            If arrUser.Count = 0 Then
                strResult = String.Format(DotNetNuke.Services.Localization.Localization.GetString("NoMessagesSent"), intMailsSent)
                msgResult = DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning
                isValid = False
            Else
                ' create object
                Dim sSubject As String = "Thông báo tin bài mới"
                Dim sContent As String = String.Empty
                sContent += "Chào bạn, <br /> <br />"
                sContent += "Vừa có tin bài mới được cập nhật <br />"
                sContent += "Tiêu đề    : " + tieude + " <br />"
                sContent += "Người gửi  : " + userin.DisplayName + " <br />"
                sContent += "Thời gian  : " + DateTime.Now.ToString("HH:mm dd/MM/yyyy") + " <br />"
                sContent += "Xin trân trọng thông báo. <br /><br />"
                sContent += "<i>Hệ thống gửi mail tự động.</i>"

                Dim objSendBulkEMail As New DotNetNuke.Services.Mail.SendTokenizedBulkEmail(New System.Collections.Generic.List(Of String), arrUser, False, sSubject, sContent)
                'Email config
                Dim sSMTPServer As String = HostController.Instance.GetString("SMTPServer")
                Dim sSMTPAuthentication As String = HostController.Instance.GetString("SMTPAuthentication")
                Dim sSMTPEnableSSL As String = HostController.Instance.GetString("SMTPEnableSSL")
                Dim bSMTPEnableSSL As Boolean = False
                If sSMTPEnableSSL = "Y" Then
                    bSMTPEnableSSL = True
                End If

                Dim sSMTPUsername As String = HostController.Instance.GetString("SMTPUsername")
                Dim sSMTPPassword As String = HostController.Instance.GetString("SMTPPassword")

                objSendBulkEMail.SetSMTPServer(sSMTPServer, sSMTPAuthentication, sSMTPUsername, sSMTPPassword, bSMTPEnableSSL)
                objSendBulkEMail.BodyFormat = DotNetNuke.Services.Mail.MailFormat.Html

                objSendBulkEMail.Priority = DotNetNuke.Services.Mail.MailPriority.Normal

                Dim myUser As Entities.Users.UserInfo = objSendBulkEMail.SendingUser
                If myUser Is Nothing Then myUser = userin
                'myUser.Email = UserInfo.Email
                objSendBulkEMail.SendingUser = myUser
                objSendBulkEMail.ReplyTo = myUser
                objSendBulkEMail.AddressMethod = SendTokenizedBulkEmail.AddressMethods.Send_TO ' 
                objSendBulkEMail.RemoveDuplicates = True

                ' send mail
                Dim objThread As New Thread(AddressOf objSendBulkEMail.Send)
                objThread.Start()
            End If
        Catch exc As Exception    'Module failed to load
        End Try
    End Sub

    'Private Sub SendMail(strFrom As String, strTo As String, strCC As String, strBCC As String, strSubject As String, strBody As String)
    '    Dim strSMTP As String = "smtp.gmail.com:587"
    '    DotNetNuke.Services.Mail.Mail.SendMail(strFrom, strTo, strCC, strBCC, DotNetNuke.Services.Mail.MailPriority.High, strSubject,
    '        DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, strBody, "", strSMTP, "1",
    '        "trienlam@capstonevietnam.com", "tlcap2016@123", True)
    'End Sub
    Public Shared Sub SendMail(strFrom As String, strTo As String, strCC As String, strSubject As String, sContent As String)
        'Email config
        Dim sSMTPServer As String = HostController.Instance.GetString("SMTPServer")
        Dim sSMTPAuthentication As String = HostController.Instance.GetString("SMTPAuthentication")
        Dim sSMTPEnableSSL As String = HostController.Instance.GetString("SMTPEnableSSL")
        Dim bSMTPEnableSSL As Boolean = False
        If sSMTPEnableSSL = "Y" Then
            bSMTPEnableSSL = True
        End If
        sContent = "<!DOCTYPE html><html><head><meta content='fair.capstonevietnam.com' http-equiv='Copyright'><meta http-equiv='Content-Type' content='text/html;charset=utf-8'><meta name='viewport' content='width=device-width,initial-scale=1.0'><title>Thư xác nhận tham gia triển lãm</title><meta content='Demo' http-equiv='Version'><style type='text/css'>body{margin:0;padding:0;background-color:#fff;color:#777;font-family:Arial,Helvetica,sans-serif;font-size:12px;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;width:100%!important}a,a:link,a:visited{color:#2c8fd6;text-decoration:underline}a:active,a:hover{text-decoration:none;color:#125f96!important}h1,h1 a,h2,h2 a,h3,h3 a{color:#2c8fd6!important}h2{padding:0 0 10px;margin:0 0 10px}h2.name{padding:0 0 7px;margin:0 0 7px}h3{padding:0 0 5px;margin:0 0 5px}p{margin:0 0 14px;padding:0}img{border:0;-ms-interpolation-mode:bicubic;max-width:100%}a img{border:none}table td{border-collapse:collapse}td.quote{font-family:Georgia,'Times New Roman',Times,serif;font-size:18px;line-height:20pt;color:#2c8fd6}span.noLink a,span.phone a{color:2c8fd6;text-decoration:none}.ExternalClass,.ReadMsgBody{width:100%}@media (max-width:767px){td[class=container],td[class=shareContainer],td[class=topContainer]{padding-left:20px!important;padding-right:20px!important}table[class=row]{width:100%!important;max-width:600px!important}img[class=banner],img[class=wideImage]{width:100%!important;height:auto!important;max-width:100%}}@media (max-width:560px){td[class=socialIconsContainer],td[class=twoFromThree]{display:block;width:100%!important}td[class=authorInfo],td[class=inner2]{padding-right:30px!important}td[class=socialIconsContainer]{border-top:0!important}td[class=socialIcons2],td[class=socialIcons]{padding-top:0!important;text-align:left!important;padding-left:30px!important;padding-bottom:20px!important}}@media (max-width:480px){td[class=inner],td[class=inner_image]{padding-left:30px!important;padding-right:30px!important}body,html{margin-right:auto;margin-left:auto}td[class=oneFromTwo]{display:block;width:100%!important}td[class=inner_image]{padding-bottom:25px!important}img[class=wideImage]{width:auto!important;margin:0 auto}td[class=viewOnline]{display:none!important}td[class=date]{font-size:14px!important;padding:10px 30px!important;background-color:#f4f4f4;text-align:left!important}td[class=title]{font-size:24px!important;line-height:32px!important}table[class=quoteContainer]{width:100%!important;float:none}td[class=quote]{padding-right:0!important}td[class=spacer]{padding-top:18px!important}}@media (max-width:380px){td[class=authorInfo],td[class=icon],td[class=socialIcons2]{text-align:center!important}td[class=shareContainer]{padding:0 10px!important}td[class=topContainer]{padding:10px 10px 0!important;background-color:#e9e9e9!important}td[class=container]{padding:0 10px 10px!important}table[class=row]{min-width:240px!important}img[class=wideImage]{width:100%!important;max-width:255px}td[class=spacer2]{display:none!important}td[class=spacer3]{padding-top:23px!important}table[class=iconContainer],table[class=iconContainer_right]{width:100%!important;float:none!important}table[class=authorPicture]{float:none!important;margin:0 auto!important;width:80px!important}td[class=icon]{padding:5px 0 25px!important}td[class=icon] img{display:inline!important}img[class=buttonRight]{float:none!important}img[class=bigButton]{width:100%!important;max-width:224px;height:auto!important}h2[class=website]{font-size:22px!important}}#loader{display:block;position:absolute;left:50%;top:0;margin:20px 0 20px -110px}</style><!-- Internet Explorer fix --><!--[if IE]><style type='text/css'>@media (max-width:560px){td[class=twoFromThree],td[class=socialIconsContainer]{float:left;padding:0px;}}@media only screen and (max-width:480px){    td[class=oneFromTwo]{float:left;padding:0px;}}@media (max-width:380px){span[class=phone]{display:block !important;}}</style><![endif]--><!-- / Internet Explorer fix --> " _
        & " </head> " _
        & " <body> " _
        & "     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;'> " _
        & "         <tbody> " _
        & "             <tr> " _
        & "                 <td class='container' style='padding-left:5px; padding-right:5px; padding-bottom:20px; background-color:#e9e9e9;'> " _
        & "                     <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'> " _
        & "                         <tbody> " _
        & "                             <tr> " _
        & "                                 <td Class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'> " _
        & "                                     Xin chào bạn: <b>__TenNguoiNhan__</b>" _
        & "                                 </td> " _
        & "                             </tr> " _
        & "                             <tr> " _
        & "                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'> " _
        & "                                      Vừa có tin bài __Tieude__ vừa được gửi __Luong__" _
        & "                                 </td> " _
        & "                             </tr> " _
        & "                             <tr> " _
        & "                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:20px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:20px; line-height:26px; text-align: center; color:#b11116; font-weight:600;'> " _
        & "                                     Tác giả: __TacGia__ " _
        & "                                 </td> " _
        & "                             </tr> " _
        & "                             <tr> " _
        & "                                 <td class='title' colspan='2' style='padding-top:15px; padding-right:30px; padding-bottom:20px; padding-left:30px;border-top:1px #dddddd dotted;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:22px; line-height:26px; text-align: center; font-weight: bold; color:#1d1d1d; font-weight:300;'> " _
        & "                                     <p>Thời gian: __ThoiGian__</p> " _
        & "                                     <p>Chuyên mục:  __ChuyenMuc__</p> " _
        & "  " _
        & "                                 </td> " _
        & "                             </tr> " _
        & "                         </tbody> " _
        & "                     </table> " _
        & " </body> " _
        & " </html> "
        Dim sSMTPUsername As String = HostController.Instance.GetString("SMTPUsername")
        Dim sSMTPPassword As String = HostController.Instance.GetString("SMTPPassword")
        '===============================================
        Mail.SendMail(strFrom, strTo, strCC, "", DotNetNuke.Services.Mail.MailPriority.High, strSubject,
                DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, sContent, "", sSMTPServer, "1",
                sSMTPUsername, sSMTPPassword, True)
        System.Threading.Thread.Sleep(1000)
    End Sub
    Public Shared Function GetMailGroup(role As String) As String
        Dim listmailbientap As String = ""
        Dim allUsers As ArrayList = UserController.GetUsers(PortalSettings.Current.PortalId)
        For i As Integer = 0 To allUsers.Count - 1
            Dim dnnUser As UserInfo = CType(allUsers(i), UserInfo)
            If dnnUser.IsInRole(role) Then
                listmailbientap += dnnUser.Email & ","
            End If
        Next
        If listmailbientap.Length > 2 Then
            listmailbientap = listmailbientap.Trim().Substring(0, listmailbientap.Length - 1)
        End If
        Return listmailbientap.ToString()
    End Function
    Public Shared Sub SendMailThongBaoBai(objnews As NV_NewsInfo, PortalId As Integer)
        Dim stitle As String = "Bài viết __Tieude__ gửi __TrangThai__"
        Dim scontent As String = ""
        scontent = "<!DOCTYPE html><html><head><meta http-equiv='Content-Type' content='text/html;charset=utf-8'><meta name='viewport' content='width=device-width,initial-scale=1.0'><title>Tin bài chờ Duyệt / Chờ Xuất bản</title><meta content='Demo' http-equiv='Version'>" _
                    & " <style type='text/css'>body{margin:0;padding:0;background-color:#fff;color:#777;font-family:Arial,Helvetica,sans-serif;font-size:12px;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;width:100%!important}a,a:link,a:visited{color:#2c8fd6;text-decoration:underline}a:active,a:hover{text-decoration:none;color:#125f96!important}h1,h1 a,h2,h2 a,h3,h3 a{color:#2c8fd6!important}h2{padding:0 0 10px;margin:0 0 10px}h2.name{padding:0 0 7px;margin:0 0 7px}h3{padding:0 0 5px;margin:0 0 5px}p{margin:0 0 14px;padding:0}img{border:0;-ms-interpolation-mode:bicubic;max-width:100%}a img{border:none}table td{border-collapse:collapse}td.quote{font-family:Georgia,'Times New Roman',Times,serif;font-size:18px;line-height:20pt;color:#2c8fd6}span.noLink a,span.phone a{color:2c8fd6;text-decoration:none}.ExternalClass,.ReadMsgBody{width:100%}@media (max-width:767px){td[class=container],td[class=shareContainer],td[class=topContainer]{padding-left:20px!important;padding-right:20px!important}table[class=row]{width:100%!important;max-width:600px!important}img[class=banner],img[class=wideImage]{width:100%!important;height:auto!important;max-width:100%}}@media (max-width:560px){td[class=socialIconsContainer],td[class=twoFromThree]{display:block;width:100%!important}td[class=authorInfo],td[class=inner2]{padding-right:30px!important}td[class=socialIconsContainer]{border-top:0!important}td[class=socialIcons2],td[class=socialIcons]{padding-top:0!important;text-align:left!important;padding-left:30px!important;padding-bottom:20px!important}}@media (max-width:480px){td[class=inner],td[class=inner_image]{padding-left:30px!important;padding-right:30px!important}body,html{margin-right:auto;margin-left:auto}td[class=oneFromTwo]{display:block;width:100%!important}td[class=inner_image]{padding-bottom:25px!important}img[class=wideImage]{width:auto!important;margin:0 auto}td[class=viewOnline]{display:none!important}td[class=date]{font-size:14px!important;padding:10px 30px!important;background-color:#f4f4f4;text-align:left!important}td[class=title]{font-size:24px!important;line-height:32px!important}table[class=quoteContainer]{width:100%!important;float:none}td[class=quote]{padding-right:0!important}td[class=spacer]{padding-top:18px!important}}@media (max-width:380px){td[class=authorInfo],td[class=icon],td[class=socialIcons2]{text-align:center!important}td[class=shareContainer]{padding:0 10px!important}td[class=topContainer]{padding:10px 10px 0!important;background-color:#e9e9e9!important}td[class=container]{padding:0 10px 10px!important}table[class=row]{min-width:240px!important}img[class=wideImage]{width:100%!important;max-width:255px}td[class=spacer2]{display:none!important}td[class=spacer3]{padding-top:23px!important}table[class=iconContainer],table[class=iconContainer_right]{width:100%!important;float:none!important}table[class=authorPicture]{float:none!important;margin:0 auto!important;width:80px!important}td[class=icon]{padding:5px 0 25px!important}td[class=icon] img{display:inline!important}img[class=buttonRight]{float:none!important}img[class=bigButton]{width:100%!important;max-width:224px;height:auto!important}h2[class=website]{font-size:22px!important}}#loader{display:block;position:absolute;left:50%;top:0;margin:20px 0 20px -110px}</style><!-- Internet Explorer fix --><!--[if IE]><style type='text/css'>@media (max-width:560px){td[class=twoFromThree],td[class=socialIconsContainer]{float:left;padding:0px;}}@media only screen and (max-width:480px){    td[class=oneFromTwo]{float:left;padding:0px;}}@media (max-width:380px){span[class=phone]{display:block !important;}}</style><![endif]--><!-- / Internet Explorer fix -->" _
                    & " </head>" _
                    & " <body>" _
                    & " <table width='90%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; margin:0 auto;'>" _
                    & " <tbody>" _
                        & " <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                               & "<b>__Tieude__</b> --> <span style='color:red'> __TrangThai__</span>" _
                           & "  </td>" _
                        & " </tr>" _
                       & "  <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                               & "  Tác giả:  __TacGia___" _
                           & "  </td>" _
                       & "  </tr>" _
                       & "  <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                            & "     Chuyên mục:  __ChuyenMuc___" _
                           & "  </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                & " Ngày đăng:  __NgayDang__" _
                            & " </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td width='200px' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;'><img src='__AnhDaiDien__' align='left' width='200px' style='margin-right:15px;' /></td><td class='title' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                & "<h4>__Tieude__</h4><p>__Tomtat__</p>" _
                            & " </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                            & " <h3>__Tieude__</h3>   " _
                            & " <hr /><p>Ngày đăng:  __NgayDang__ | Tác giả: __TacGia___ | Chuyên mục: __ChuyenMuc___</p>  <hr /> " _
                            & " <b>__Tomtat__</b>   " _
                            & " </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                & "____SUABAI____" _
                            & " </td>" _
                        & " </tr>" _
                    & " </tbody>" _
                & " </table>" _
            & " </body>" _
            & " </html>"
        scontent = scontent.Replace("__Tieude__", objnews.Title)
        scontent = scontent.Replace("__TacGia___", BL.GetButDanh(PortalId, objnews.UserId))
        scontent = scontent.Replace("__ChuyenMuc___", objnews.CategoryName)
        scontent = scontent.Replace("__NgayDang__", BL.FormatDate(objnews.CreateDate))
        scontent = scontent.Replace("__Tomtat__", objnews.Summary)
        Dim noidung As String = ""
        noidung = objnews.Content.Replace("/DATA/", "https://thuongtruong-fileserver.nvcms.net/")
        'scontent = scontent.Replace("__NoiDung__", HttpUtility.HtmlDecode(noidung))
        scontent = scontent.Replace("__TrangThai__", objnews.StatusName)
        Dim anhdaidien = ""
        anhdaidien = objnews.ImagePath.Replace("/DATA/", "https://thuongtruong-fileserver.nvcms.net/")
        scontent = scontent.Replace("__AnhDaiDien__", anhdaidien)
        'Sua tin bai
        Dim linksua As String = "#"
        If objnews.Status = NewsStatus.ChoPheDuyet Then
            linksua = "<a href='https://cms.thuongtruong.com.vn/quan-tri/tin-tuc-cap-cao/phe-duyet-tin?view=edit&itemid=" & objnews.NewId & "' style='font-weight:bold;color:red;'>Duyệt tin bài</a>"
        End If
        If objnews.Status = NewsStatus.ChoXuatBan Then
            linksua = "<a href='https://cms.thuongtruong.com.vn/quan-tri/tin-tuc-cap-cao/xuat-ban-tin-bai?view=edit&itemid=" & objnews.NewId & "' style='font-weight:bold;color:red;'>Xuất bản tin bài</a>"
        End If

        scontent = scontent.Replace("____SUABAI____", linksua)

        stitle = stitle.Replace("__Tieude__", objnews.Title)
        stitle = stitle.Replace("__TrangThai__", objnews.StatusName)


        Dim sName As String = "Tạp chí Thương Trường <admin@thuongtruong.com.vn>"
        Dim strSMTP As String = "smtp.gmail.com:587"
        DotNetNuke.Services.Mail.Mail.SendMail(sName, "bbtthuongtruong@gmail.com", "", "", DotNetNuke.Services.Mail.MailPriority.High, stitle, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, scontent, "", strSMTP, "1", "nguyen@nvportal.net", "ponslqcyxcodsksr", True)
        System.Threading.Thread.Sleep(1000)
    End Sub
    Public Shared Sub SendMailThongBaoBaiSuaBaiXB(objnews As NV_NewsInfo, PortalId As Integer, UserId As Integer)
        Dim stitle As String = "Bài viết __Tieude__ được sửa lúc " & DateTime.Now.ToString("HH:MM dd/MM/yyyy") & "bởi: " & BL.GetButDanh(PortalId, UserId)
        Dim scontent As String = ""
        scontent = "<!DOCTYPE html><html><head><meta http-equiv='Content-Type' content='text/html;charset=utf-8'><meta name='viewport' content='width=device-width,initial-scale=1.0'><title>Tin bài chờ Duyệt / Chờ Xuất bản</title><meta content='Demo' http-equiv='Version'>" _
                    & " <style type='text/css'>body{margin:0;padding:0;background-color:#fff;color:#777;font-family:Arial,Helvetica,sans-serif;font-size:12px;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;width:100%!important}a,a:link,a:visited{color:#2c8fd6;text-decoration:underline}a:active,a:hover{text-decoration:none;color:#125f96!important}h1,h1 a,h2,h2 a,h3,h3 a{color:#2c8fd6!important}h2{padding:0 0 10px;margin:0 0 10px}h2.name{padding:0 0 7px;margin:0 0 7px}h3{padding:0 0 5px;margin:0 0 5px}p{margin:0 0 14px;padding:0}img{border:0;-ms-interpolation-mode:bicubic;max-width:100%}a img{border:none}table td{border-collapse:collapse}td.quote{font-family:Georgia,'Times New Roman',Times,serif;font-size:18px;line-height:20pt;color:#2c8fd6}span.noLink a,span.phone a{color:2c8fd6;text-decoration:none}.ExternalClass,.ReadMsgBody{width:100%}@media (max-width:767px){td[class=container],td[class=shareContainer],td[class=topContainer]{padding-left:20px!important;padding-right:20px!important}table[class=row]{width:100%!important;max-width:600px!important}img[class=banner],img[class=wideImage]{width:100%!important;height:auto!important;max-width:100%}}@media (max-width:560px){td[class=socialIconsContainer],td[class=twoFromThree]{display:block;width:100%!important}td[class=authorInfo],td[class=inner2]{padding-right:30px!important}td[class=socialIconsContainer]{border-top:0!important}td[class=socialIcons2],td[class=socialIcons]{padding-top:0!important;text-align:left!important;padding-left:30px!important;padding-bottom:20px!important}}@media (max-width:480px){td[class=inner],td[class=inner_image]{padding-left:30px!important;padding-right:30px!important}body,html{margin-right:auto;margin-left:auto}td[class=oneFromTwo]{display:block;width:100%!important}td[class=inner_image]{padding-bottom:25px!important}img[class=wideImage]{width:auto!important;margin:0 auto}td[class=viewOnline]{display:none!important}td[class=date]{font-size:14px!important;padding:10px 30px!important;background-color:#f4f4f4;text-align:left!important}td[class=title]{font-size:24px!important;line-height:32px!important}table[class=quoteContainer]{width:100%!important;float:none}td[class=quote]{padding-right:0!important}td[class=spacer]{padding-top:18px!important}}@media (max-width:380px){td[class=authorInfo],td[class=icon],td[class=socialIcons2]{text-align:center!important}td[class=shareContainer]{padding:0 10px!important}td[class=topContainer]{padding:10px 10px 0!important;background-color:#e9e9e9!important}td[class=container]{padding:0 10px 10px!important}table[class=row]{min-width:240px!important}img[class=wideImage]{width:100%!important;max-width:255px}td[class=spacer2]{display:none!important}td[class=spacer3]{padding-top:23px!important}table[class=iconContainer],table[class=iconContainer_right]{width:100%!important;float:none!important}table[class=authorPicture]{float:none!important;margin:0 auto!important;width:80px!important}td[class=icon]{padding:5px 0 25px!important}td[class=icon] img{display:inline!important}img[class=buttonRight]{float:none!important}img[class=bigButton]{width:100%!important;max-width:224px;height:auto!important}h2[class=website]{font-size:22px!important}}#loader{display:block;position:absolute;left:50%;top:0;margin:20px 0 20px -110px}</style><!-- Internet Explorer fix --><!--[if IE]><style type='text/css'>@media (max-width:560px){td[class=twoFromThree],td[class=socialIconsContainer]{float:left;padding:0px;}}@media only screen and (max-width:480px){    td[class=oneFromTwo]{float:left;padding:0px;}}@media (max-width:380px){span[class=phone]{display:block !important;}}</style><![endif]--><!-- / Internet Explorer fix -->" _
                    & " </head>" _
                    & " <body>" _
                    & " <table width='90%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; margin:0 auto;'>" _
                    & " <tbody>" _
                        & " <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                               & "<b>__Tieude__</b>" _
                           & "  </td>" _
                        & " </tr>" _
                       & "  <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                               & "  Tác giả:  __TacGia___" _
                           & "  </td>" _
                       & "  </tr>" _
                       & "  <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                            & "     Chuyên mục:  __ChuyenMuc___" _
                           & "  </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                & " Ngày đăng:  __NgayDang__" _
                            & " </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td width='200px' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;'><img src='__AnhDaiDien__' align='left' width='200px' style='margin-right:15px;' /></td><td class='title' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                & "<h4>__Tieude__</h4><p>__Tomtat__</p>" _
                            & " </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                            & " <h3>__Tieude__</h3>   " _
                            & " <hr /><p>Ngày đăng:  __NgayDang__ | Tác giả: __TacGia___ | Chuyên mục: __ChuyenMuc___</p>  <hr /> " _
                            & " <b>__Tomtat__</b>   " _
                             & " __NoiDung__  " _
                            & " </td>" _
                        & " </tr>" _
                        & " <tr>" _
                            & " <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                & "____SUABAI____" _
                            & " </td>" _
                        & " </tr>" _
                    & " </tbody>" _
                & " </table>" _
            & " </body>" _
            & " </html>"
        scontent = scontent.Replace("__Tieude__", objnews.Title)
        scontent = scontent.Replace("__TacGia___", BL.GetButDanh(PortalId, objnews.UserId))
        scontent = scontent.Replace("__ChuyenMuc___", objnews.CategoryName)
        scontent = scontent.Replace("__NgayDang__", BL.FormatDate(objnews.CreateDate))
        scontent = scontent.Replace("__Tomtat__", objnews.Summary)
        Dim noidung As String = ""
        noidung = objnews.Content.Replace("/DATA/", "https://thuongtruong-fileserver.nvcms.net/")
        scontent = scontent.Replace("__NoiDung__", HttpUtility.HtmlDecode(noidung))
        scontent = scontent.Replace("__TrangThai__", objnews.StatusName)
        Dim anhdaidien = ""
        anhdaidien = objnews.ImagePath.Replace("/DATA/", "https://thuongtruong-fileserver.nvcms.net/")
        scontent = scontent.Replace("__AnhDaiDien__", anhdaidien)
        'Sua tin bai
        Dim linksua As String = "#"
        If objnews.Status = NewsStatus.ChoPheDuyet Then
            linksua = "<a href='https://cms.thuongtruong.com.vn/quan-tri/tin-tuc-cap-cao/phe-duyet-tin?view=edit&itemid=" & objnews.NewId & "' style='font-weight:bold;color:red;'>Duyệt tin bài</a>"
        End If
        If objnews.Status = NewsStatus.ChoXuatBan Then
            linksua = "<a href='https://cms.thuongtruong.com.vn/quan-tri/tin-tuc-cap-cao/xuat-ban-tin-bai?view=edit&itemid=" & objnews.NewId & "' style='font-weight:bold;color:red;'>Xuất bản tin bài</a>"
        End If

        scontent = scontent.Replace("____SUABAI____", linksua)

        stitle = stitle.Replace("__Tieude__", objnews.Title)
        stitle = stitle.Replace("__TrangThai__", objnews.StatusName)


        Dim sName As String = "Tạp chí Thương Trường <admin@thuongtruong.com.vn>"
        Dim strSMTP As String = "smtp.gmail.com:587"
        DotNetNuke.Services.Mail.Mail.SendMail(sName, "bbtthuongtruong@gmail.com", "", "", DotNetNuke.Services.Mail.MailPriority.High, stitle, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, scontent, "", strSMTP, "1", "nguyen@nvportal.net", "ponslqcyxcodsksr", True)
        System.Threading.Thread.Sleep(1000)
    End Sub
    Public Shared Sub SendMailThongBaoUser(objnews As NV_NewsInfo, ou As UserInfo)
        Dim stitle As String = "Bài viết: __Tieude__ --> __TrangThai__"
        Dim scontent As String = ""
        scontent = "<!DOCTYPE html><html><head><meta http-equiv='Content-Type' content='text/html;charset=utf-8'><meta name='viewport' content='width=device-width,initial-scale=1.0'><title>Tin bài chờ Duyệt / Chờ Xuất bản</title><meta content='Demo' http-equiv='Version'>" _
                    & " <style type='text/css'>body{margin:0;padding:0;background-color:#fff;color:#777;font-family:Arial,Helvetica,sans-serif;font-size:12px;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;width:100%!important}a,a:link,a:visited{color:#2c8fd6;text-decoration:underline}a:active,a:hover{text-decoration:none;color:#125f96!important}h1,h1 a,h2,h2 a,h3,h3 a{color:#2c8fd6!important}h2{padding:0 0 10px;margin:0 0 10px}h2.name{padding:0 0 7px;margin:0 0 7px}h3{padding:0 0 5px;margin:0 0 5px}p{margin:0 0 14px;padding:0}img{border:0;-ms-interpolation-mode:bicubic;max-width:100%}a img{border:none}table td{border-collapse:collapse}td.quote{font-family:Georgia,'Times New Roman',Times,serif;font-size:18px;line-height:20pt;color:#2c8fd6}span.noLink a,span.phone a{color:2c8fd6;text-decoration:none}.ExternalClass,.ReadMsgBody{width:100%}@media (max-width:767px){td[class=container],td[class=shareContainer],td[class=topContainer]{padding-left:20px!important;padding-right:20px!important}table[class=row]{width:100%!important;max-width:600px!important}img[class=banner],img[class=wideImage]{width:100%!important;height:auto!important;max-width:100%}}@media (max-width:560px){td[class=socialIconsContainer],td[class=twoFromThree]{display:block;width:100%!important}td[class=authorInfo],td[class=inner2]{padding-right:30px!important}td[class=socialIconsContainer]{border-top:0!important}td[class=socialIcons2],td[class=socialIcons]{padding-top:0!important;text-align:left!important;padding-left:30px!important;padding-bottom:20px!important}}@media (max-width:480px){td[class=inner],td[class=inner_image]{padding-left:30px!important;padding-right:30px!important}body,html{margin-right:auto;margin-left:auto}td[class=oneFromTwo]{display:block;width:100%!important}td[class=inner_image]{padding-bottom:25px!important}img[class=wideImage]{width:auto!important;margin:0 auto}td[class=viewOnline]{display:none!important}td[class=date]{font-size:14px!important;padding:10px 30px!important;background-color:#f4f4f4;text-align:left!important}td[class=title]{font-size:24px!important;line-height:32px!important}table[class=quoteContainer]{width:100%!important;float:none}td[class=quote]{padding-right:0!important}td[class=spacer]{padding-top:18px!important}}@media (max-width:380px){td[class=authorInfo],td[class=icon],td[class=socialIcons2]{text-align:center!important}td[class=shareContainer]{padding:0 10px!important}td[class=topContainer]{padding:10px 10px 0!important;background-color:#e9e9e9!important}td[class=container]{padding:0 10px 10px!important}table[class=row]{min-width:240px!important}img[class=wideImage]{width:100%!important;max-width:255px}td[class=spacer2]{display:none!important}td[class=spacer3]{padding-top:23px!important}table[class=iconContainer],table[class=iconContainer_right]{width:100%!important;float:none!important}table[class=authorPicture]{float:none!important;margin:0 auto!important;width:80px!important}td[class=icon]{padding:5px 0 25px!important}td[class=icon] img{display:inline!important}img[class=buttonRight]{float:none!important}img[class=bigButton]{width:100%!important;max-width:224px;height:auto!important}h2[class=website]{font-size:22px!important}}#loader{display:block;position:absolute;left:50%;top:0;margin:20px 0 20px -110px}</style><!-- Internet Explorer fix --><!--[if IE]><style type='text/css'>@media (max-width:560px){td[class=twoFromThree],td[class=socialIconsContainer]{float:left;padding:0px;}}@media only screen and (max-width:480px){    td[class=oneFromTwo]{float:left;padding:0px;}}@media (max-width:380px){span[class=phone]{display:block !important;}}</style><![endif]--><!-- / Internet Explorer fix -->" _
                    & " </head>" _
                    & " <body>" _
                    & " <table width='90%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; margin:0 auto;'>" _
                    & " <tbody>" _
                        & " <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                               & "<b>__Tieude__</b> --> <span style='color:red'> __TrangThai__</span>" _
                           & "  </td>" _
                        & " </tr>" _
                       & "  <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                               & "  Tác giả:  __TacGia___" _
                           & "  </td>" _
                       & "  </tr>" _
                       & "  <tr>" _
                           & "  <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                            & "     Chuyên mục:  __ChuyenMuc___" _
                           & "  </td>" _
                        & " </tr>" _
                        & "<tr>" _
                            & " <td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                & " Ngày đăng:  __NgayDang__" _
                            & " </td>" _
                        & " </tr>" _
                        & "__Kiemtratrangtai___" _
                    & " </tbody>" _
                & " </table>" _
            & " </body>" _
            & " </html>"
        Dim strbitralai As String = "<tr>" _
                                        & "<td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                        & " __strNgayXuLy__:  __NgayXuLy__  ||  __strNguoiXuLy__:  __NguoiXuLy__  " _
                                        & "</td>" _
                                    & "</tr>" _
                                    & "<tr>" _
                                        & "<td class='title' colspan='2' style='border:solid 1px #ececec;padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>" _
                                        & " __strxemthongtin__" _
                                        & "</td>" _
                                    & "</tr>"

        If objnews.Status = NewsStatus.BiTraLai Then
            strbitralai = strbitralai.Replace("__strNgayXuLy__", "Ngày trả lại")
            strbitralai = strbitralai.Replace("__NgayXuLy__", BL.FormatDate(objnews.ReturnedDate))
            strbitralai = strbitralai.Replace("__strNguoiXuLy__", "Người xử lý")
            strbitralai = strbitralai.Replace("__NguoiXuLy__", BL.GetButDanh(PortalSettings.Current.PortalId, objnews.ReturnedUser))
            strbitralai = strbitralai.Replace("__strxemthongtin__", "<a href='https://cms.thuongtruong.com.vn/quan-tri/tin-tuc/them-moi?itemid=" & objnews.NewId & "' style='font-weight:bold;color:red;'>Xem lại bài</a>")
        End If
        If objnews.Status = NewsStatus.ChoXuatBan Then
            strbitralai = strbitralai.Replace("__strNgayXuLy__", "Ngày gửi xuất bản")
            strbitralai = strbitralai.Replace("__NgayXuLy__", BL.FormatDate(objnews.ApprovalRequestDate))
            strbitralai = strbitralai.Replace("__strNguoiXuLy__", "Người xử lý")
            strbitralai = strbitralai.Replace("__NguoiXuLy__", BL.GetButDanh(PortalSettings.Current.PortalId, objnews.ApprovalUser))
            strbitralai = strbitralai.Replace("__strxemthongtin__", "")
        End If
        If objnews.Status = NewsStatus.DaXuatBan Then
            strbitralai = strbitralai.Replace("__strNgayXuLy__", "Ngày xuất bản")
            strbitralai = strbitralai.Replace("__NgayXuLy__", BL.FormatDate(objnews.PublishedDate))
            strbitralai = strbitralai.Replace("__strNguoiXuLy__", "Người xử lý")
            strbitralai = strbitralai.Replace("__NguoiXuLy__", BL.GetButDanh(PortalSettings.Current.PortalId, objnews.PublishedUser))
            strbitralai = strbitralai.Replace("__strxemthongtin__", "Nhuận bút" & objnews.Credit & "<br />" & "Link bài viết" & Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(objnews.CategoryId), objnews.NewId, objnews.Title))
        End If

        scontent = scontent.Replace("__Tieude__", objnews.Title)
        scontent = scontent.Replace("__TrangThai__", objnews.StatusName)
        scontent = scontent.Replace("__TacGia___", BL.GetButDanh(PortalSettings.Current.PortalId, objnews.UserId))
        scontent = scontent.Replace("__ChuyenMuc___", objnews.CategoryName)
        scontent = scontent.Replace("__NgayDang__", BL.FormatDate(objnews.CreateDate))
        scontent = scontent.Replace("__Kiemtratrangtai___", strbitralai)

        stitle = stitle.Replace("__Tieude__", objnews.Title)
        stitle = stitle.Replace("__TrangThai__", objnews.StatusName)



        Dim sName As String = "Tạp chí Thương Trường <admin@thuongtruong.com.vn>"
        Dim strSMTP As String = "smtp.gmail.com:587"
        If Ultis.IsValidEmail(ou.Email) Then
            DotNetNuke.Services.Mail.Mail.SendMail(sName, ou.Email, "", "", DotNetNuke.Services.Mail.MailPriority.High, stitle, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, scontent, "", strSMTP, "1", "nguyen@nvportal.net", "ponslqcyxcodsksr", True)
            System.Threading.Thread.Sleep(1000)
        End If

    End Sub
    Public Shared Sub Send_SMS(userid As Integer, portalid As Integer, moduleid As Integer, ByVal PhoneNumber As String, ByVal Message As String, ByVal SendDate As DateTime)
        Try
            Dim ctlSMS_Inbox As New SMS_InboxController
            Dim objSMS_Inbox As SMS_InboxInfo = Nothing
            Dim SMS_Id As Integer = 0
            objSMS_Inbox = New SMS_InboxInfo
            objSMS_Inbox.ToPhone = PhoneNumber
            objSMS_Inbox.Content = Message
            objSMS_Inbox.CreateDate = Date.Now
            objSMS_Inbox.CreateByUser = userid.ToString()
            objSMS_Inbox.SendDate = SendDate
            objSMS_Inbox.PortalId = portalid
            objSMS_Inbox.ModuleId = moduleid
            SMS_Id = ctlSMS_Inbox.Insert(objSMS_Inbox)
            'Gui tin nhan
            'If Not Send(PhoneNumber, Message, SMS_Id) Then
            '    ' Me.lbThongBao.Text &= String.Format("Không gửi được tin nhắn tới số [{0}]!", PhoneNumber) & vbCrLf
            'End If
        Catch ex As Exception
        End Try
    End Sub
    ''' <summary>
    ''' Gửi tin nhắn đến 01 số điện thoại
    ''' </summary>
    ''' <param name="userid"></param>
    ''' <param name="portalid"></param>
    ''' <param name="moduleid"></param>
    ''' <param name="PhoneNumber"></param>
    ''' <param name="Message"></param>
    ''' <param name="SMS_Id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Shared Function Send(userid As Integer, portalid As Integer, moduleid As Integer, ByVal PhoneNumber As String, ByVal Message As String, ByVal SMS_Id As Integer) As Boolean
    '    'Gui tin nhan
    '    Dim ctlSMS As New SMSHelper
    '    Dim ctlSMS_Inbox As New SMS_InboxController
    '    If ctlSMS.Send(PhoneNumber, Message) Then
    '        'Cap nhat trang thai tin nhan trong bang inbox va chuyen sang bang Outbox
    '        'Chuyen tin nhan sang bang Outbox
    '        Dim ctlSMS_Outbox As New SMS_OutboxController
    '        Dim objSMS_Outbox As New SMS_OutboxInfo(0, ctlSMS.PhoneNumber, PhoneNumber, Date.Now, Message, userid.ToString(), Now, 0, SMS_Id, portalid, moduleid)
    '        ctlSMS_Outbox.Insert(objSMS_Outbox)
    '        'Cap nhat trang thai gui thanh cong tin nhan trong bang Inbox
    '        ctlSMS_Inbox.Update_Status(SMS_Id, 2)
    '        Return True
    '    Else
    '        'Ghi log loi
    '        Dim ctlSMS_Log As New SMS_LogController
    '        Dim objSMS_Log As New SMS_LogInfo(0, ctlSMS.PhoneNumber, PhoneNumber, Date.Now, ctlSMS.Received, userid.ToString(), Now, 0, SMS_Id, portalid, moduleid)
    '        ctlSMS_Log.Insert(objSMS_Log)
    '        'Cap nhat trang thai gui loi tin nhan trong bang Inbox
    '        ctlSMS_Inbox.Update_Status(SMS_Id, 1)
    '        Return False
    '    End If
    'End Function

#End Region
#Region "Xu ly anh upload"
    Public Shared Function GetMediaPath(ByVal foldername As String, ByVal radupload As String) As String
        If radupload.Length > 0 Then
            Return foldername & "/" & radupload
        Else
            Return ""
        End If
    End Function
    Public Shared Function CutstringPhotoName(str As String) As String
        Dim iStart As Integer = str.LastIndexOf("/", System.StringComparison.Ordinal)
        Dim iEnd As Integer = str.LastIndexOf("", System.StringComparison.Ordinal)
        If iEnd > 0 Then
            Dim iLength As Integer = iEnd - iStart
            Return CType(str.Substring(iStart + 1, iLength), String)
        Else
            Return 0
        End If
    End Function
    Public Shared Function CutstringPhotoExtension(str As String) As String
        Dim extesnsion As String = ""
        extesnsion = Path.GetExtension(str)
        If extesnsion.Length > 1 Then
            Return extesnsion.Remove(0, 1)
        Else
            Return extesnsion
        End If
        Return extesnsion
    End Function
    Public Shared Function Enableanh(str As String) As String
        If Not str Is Nothing Then
            If (str.ToLower() = "png") Or (str.ToLower() = "jpg") Or (str.ToLower() = "jpeg") Or (str.ToLower() = "gif") Or (str.ToLower() = "jfif") Then
                Return ""
            Else
                Return "disabled='disabled'"
            End If
        Else
            Return "disabled='disabled'"
        End If

    End Function
    Public Shared Function GetBackround(str As String, fulllink As String) As String
        If Not str Is Nothing Then
            If (str.ToLower() = "png") Or (str.ToLower() = "jpg") Or (str.ToLower() = "jpeg") Or (str.ToLower() = "gif") Or (str.ToLower() = "jfif") Then
                Return fulllink
            End If
            If (str.ToLower() = "pdf") Then
                Return "/static/bgpdf.png"
            End If
            If (str.ToLower() = "rar") Or (str.ToLower() = "zip") Then
                Return "/static/bgrar.png"
            End If
            If (str.ToLower() = "doc") Or (str.ToLower() = "docx") Then
                Return "/static/bgdoc.png"
            End If
            If (str.ToLower() = "mp4") Or (str.ToLower() = "avi") Or (str.ToLower() = "mpeg") Then
                Return "/static/noimagevideo.png"
            End If
            If (str.ToLower() = "xls") Or (str.ToLower() = "xlsx") Then
                Return "/static/bgxls.png"
            Else
                Return "/static/noimage.png"
            End If
        Else
            Return "/static/noimage.png"
        End If
    End Function
    Public Shared Function CutstringPhotoPath(str As String) As String
        Dim iStart As Integer = str.LastIndexOf("/", System.StringComparison.Ordinal)
        Dim iEnd As Integer = str.LastIndexOf("", System.StringComparison.Ordinal)
        If iEnd > 0 Then
            Dim iLength As Integer = iEnd - iStart - 1
            Return CType(str.Substring(0, iStart), String)
        Else
            Return 0
        End If
    End Function
    Public Shared Sub XoaAnhThua(newid As Integer)
        'Dim ctlMedia As New MediaItemController
        ''lay tin tuc ra
        'Dim ctlNews As New NV_NewsController
        'Dim objNews As NV_NewsInfo = ctlNews.GetByID(newid, PortalSettings.Current.PortalId)
        'Dim ctlMediaNews As New NewsByMediaController
        'Dim currentMediaByNews = ctlMediaNews._GetAllByNewId(newid)
        'If Not currentMediaByNews Is Nothing AndAlso currentMediaByNews.Count > 0 Then
        '    For i As Integer = 0 To currentMediaByNews.Count - 1
        '        Dim objnewsMedia As NewsByMediaInfo = CType(currentMediaByNews(i), NewsByMediaInfo)
        '        If objNews.Content.Contains(objnewsMedia.ImageName) Or objNews.ImagePath.Contains(objnewsMedia.ImageName) Then

        '        Else
        '            'Xoa file
        '            Dim FileToDelete As String = objnewsMedia.ImageFullPhysic & "\" & objnewsMedia.ImageName
        '            If System.IO.File.Exists(FileToDelete) = True Then
        '                System.IO.File.Delete(FileToDelete)
        '            End If
        '            'Xoa bang media
        '            ctlMedia._Delete(objnewsMedia.MediaId)
        '            'Xoa news media
        '            ctlMediaNews._DeleteByMediaId(objnewsMedia.MediaId)

        '        End If
        '    Next
        'End If
    End Sub
#End Region
#Region "Media: Musik, video, clip"
    Public Shared Function FormatVisibleByStatus(ByVal status As Integer) As Boolean
        If (status = NewsStatus.DangBienSoan) OrElse (status = NewsStatus.ChoPheDuyet) OrElse (status = NewsStatus.BiTraLai) OrElse (status = NewsStatus.HuyXuatBan) Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region
#Region "kiem tra cau hinh tin"
    Public Shared Function CheckCauHinhTin(newid As Integer, kieutin As Integer, PortalId As Integer) As Boolean
        Dim _NewsSettingsController As New NewsSettingsController
        Dim sresult As Boolean = False
        Dim arrHots As ArrayList = _NewsSettingsController.GetAllByType(kieutin, 100, PortalId)
        If arrHots.Count > 0 Then
            For Each obj As NewsSettingsInfo In arrHots
                If obj.NewId = newid Then
                    sresult = True
                End If
            Next
        End If
        Return sresult
    End Function

#End Region
#Region "NhuanBut"
    Public Shared Function GetTienNhuanBut(NewId As Integer) As Integer
        Dim result As String = 0
        Dim ctlnhuanbut As New NhuanButController
        result = ctlnhuanbut.NhuanBut_GetTongTien(NewId, 0)
        Return result
    End Function
#End Region
#Region "Crawler"
    Public Shared Function CrawlerGetNameNameL(ByVal path As String) As String
        Dim _filename As String = ""
        Dim sep() As Char = {"/", "\", "//"}
        _filename = path.Split(sep).Last()
        Return _filename
    End Function
    Public Shared Function HtmlAgi(ByVal url As String, ByVal key As String) As String
        Dim Webget = New HtmlWeb()
        Dim doc = Webget.Load(url)
        Dim ourNode As HtmlNode = doc.DocumentNode.SelectSingleNode(String.Format("//meta[@property='{0}']", key))

        If ourNode IsNot Nothing Then
            Return ourNode.GetAttributeValue("content", "")
        Else
            Return ""
        End If
    End Function
    Public Shared Function HtmlAgiMetaName(ByVal url As String, ByVal key As String) As String
        Dim Webget = New HtmlWeb()
        Dim doc = Webget.Load(url)
        Dim ourNode As HtmlNode = doc.DocumentNode.SelectSingleNode(String.Format("//meta[@name='{0}']", key))

        If ourNode IsNot Nothing Then
            Return ourNode.GetAttributeValue("content", "")
        Else
            Return ""
        End If
    End Function
    Private Shared Function ConvertStringNonAttact(value As String) As String
        Dim objSecurity As New PortalSecurity
        value.Replace("document.cookie", "")
        value.Replace("window.location", "")
        Return System.Text.RegularExpressions.Regex.Replace(objSecurity.InputFilter(value, PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup Or PortalSecurity.FilterFlag.NoSQL Or PortalSecurity.FilterFlag.NoAngleBrackets), "<[^>]*>", "").Trim()
    End Function
#End Region
#Region "Get Cat User"
    Public Shared Function GetCaterogyIdPheDuyet(userId As Integer, ByVal PortalId As Integer) As String
        Dim sresult As String = ""
        Dim arrResult As New ArrayList
        Dim ctlNewsCategories As New NV_NewsCategoriesController
        Dim arrAllCategory As New ArrayList
        arrAllCategory = ctlNewsCategories.GetAll(PortalId)

        Dim stringcache = "CategoryId_PheDuyet" & userId & PortalId
        If DataCache.GetCache(stringcache) Is Nothing Then

            Dim objRoleCtl As New RoleController
            Dim iRoleId As Integer = objRoleCtl.GetRoleByName(PortalId, "Phe duyet").RoleID
            Dim arrTemp As ArrayList = ctlNewsCategories.GetAllCategoriesByUserIdAndRoleId(userId, iRoleId, "")
            For Each objTemp As NV_NewsCategoriesInfo In arrAllCategory
                For Each objTemp1 As NV_NewsCategoriesInfo In arrTemp
                    If objTemp.CategoryID = objTemp1.CategoryID Then
                        arrResult.Add(objTemp)
                        sresult += objTemp.CategoryID & ","
                    End If
                Next
            Next
            If (sresult.Length > 0) Then
                sresult = sresult.Substring(0, sresult.Length - 1)
            End If
            DataCache.SetCache(stringcache, sresult)
        End If
        Return DataCache.GetCache(stringcache)

    End Function
    Public Shared Function GetCaterogyIdXuatBan(userId As Integer, ByVal PortalId As Integer) As String
        Dim sresult As String = ""
        Dim arrResult As New ArrayList
        Dim ctlNewsCategories As New NV_NewsCategoriesController
        Dim arrAllCategory As New ArrayList
        arrAllCategory = ctlNewsCategories.GetAll(PortalId)

        Dim stringcache = "CategoryId_XuatBan" & userId & PortalId
        If DataCache.GetCache(stringcache) Is Nothing Then

            Dim objRoleCtl As New RoleController
            Dim iRoleId As Integer = objRoleCtl.GetRoleByName(PortalId, "Xuat ban").RoleID
            Dim arrTemp As ArrayList = ctlNewsCategories.GetAllCategoriesByUserIdAndRoleId(userId, iRoleId, "")
            For Each objTemp As NV_NewsCategoriesInfo In arrAllCategory
                For Each objTemp1 As NV_NewsCategoriesInfo In arrTemp
                    If objTemp.CategoryID = objTemp1.CategoryID Then
                        arrResult.Add(objTemp)
                        sresult += objTemp.CategoryID & ","
                    End If
                Next
            Next
            If (sresult.Length > 0) Then
                sresult = sresult.Substring(0, sresult.Length - 1)
            End If
            DataCache.SetCache(stringcache, sresult)
        End If
        Return DataCache.GetCache(stringcache)

    End Function
#End Region
#Region "Marketing School"
    Public Shared Sub Marketing_SaveTruongVersion(ByVal objMarketingSchoolInfo As MarketingSchoolInfo, ItemId As Integer, userid As Integer)
        Try
            Dim objMarketingSchoolInfo1 As New MarketingSchoolInfo
            With objMarketingSchoolInfo1
                .Truongid = ItemId
                .CODE = objMarketingSchoolInfo.CODE
                .NameofSchool = objMarketingSchoolInfo.NameofSchool
                .Tomtat = objMarketingSchoolInfo.Tomtat
                .Logo = objMarketingSchoolInfo.Logo
                .Conver = objMarketingSchoolInfo.Conver
                .VideoLink = objMarketingSchoolInfo.VideoLink
                .Descreption = objMarketingSchoolInfo.Descreption
                .Website = objMarketingSchoolInfo.Website
                .Email = objMarketingSchoolInfo.Email
                .Phone = objMarketingSchoolInfo.Phone
                .Social = objMarketingSchoolInfo.Social
                .Loaitruongtext = objMarketingSchoolInfo.Loaitruongtext
                .Kiemdinh = objMarketingSchoolInfo.Kiemdinh
                .TypeofRankingVN = objMarketingSchoolInfo.TypeofRankingVN
                .Loaitruongtext = objMarketingSchoolInfo.Loaitruongtext
                .SingleSex = objMarketingSchoolInfo.SingleSex
                .Info = objMarketingSchoolInfo.Info
                .CreatedDate = DateTime.Now
                .UserId = userid
            End With
            _Marketing_Truong_Version_Controller.Marketing_Truong_Version_Insert(objMarketingSchoolInfo)
        Catch ex As Exception
            ProcessPageLoadException(ex)
        End Try
    End Sub
    Public Shared Function Encrypt(plainText As String) As String
        Dim key As String = "SonNguyenCapStone8CDoanNayLafUyEmailDawng"
        Dim aes As Aes = Aes.Create()
        Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32))
        aes.Key = keyBytes
        aes.IV = New Byte(15) {} ' Zero IV (use a random one in production for security)

        Dim encryptor As ICryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV)
        Dim plainBytes As Byte() = Encoding.UTF8.GetBytes(plainText)

        Using ms As New IO.MemoryStream()
            Using cs As New CryptoStream(ms, encryptor, CryptoStreamMode.Write)
                cs.Write(plainBytes, 0, plainBytes.Length)
                cs.FlushFinalBlock()
                ' Convert the encrypted bytes to a hexadecimal string in lowercase
                Return BitConverter.ToString(ms.ToArray()).Replace("-", "").ToLower()
            End Using
        End Using
    End Function

    Public Shared Function Decrypt(cipherText As String) As String
        Dim key As String = "SonNguyenCapStone8CDoanNayLafUyEmailDawng"
        Dim aes As Aes = Aes.Create()
        Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32))
        aes.Key = keyBytes
        aes.IV = New Byte(15) {} ' Zero IV (match the Encrypt function's IV)

        Dim cipherBytes As Byte() = Enumerable.Range(0, cipherText.Length \ 2).
        Select(Function(i) Convert.ToByte(cipherText.Substring(i * 2, 2), 16)).ToArray()

        Dim decryptor As ICryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV)

        Using ms As New IO.MemoryStream(cipherBytes)
            Using cs As New CryptoStream(ms, decryptor, CryptoStreamMode.Read)
                Using sr As New IO.StreamReader(cs)
                    Return sr.ReadToEnd()
                End Using
            End Using
        End Using
    End Function
#End Region
End Class