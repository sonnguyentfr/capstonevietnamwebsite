Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.HeThong
Namespace NVCMS.Modules.Banner
    Partial Class maindisplay
        Inherits PortalModuleBase
#Region "Propertice"
        Dim ctlAdvBanner As New BannerAdvController
        Public Property ItemID() As Integer
            Get
                If Not ViewState("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("ItemID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("ItemID") = Value.ToString
            End Set
        End Property

        Private setting_vitri As String
        Private setting_template As String
        Private setting_showtieude As Boolean
        Private setting_showmota As Boolean
        Private ReadOnly TOKEN_LIST_TOP As String = "LIST"
        Private ReadOnly TOKEN_NAME As String = "[NAME]"
        Private ReadOnly TOKEN_NAMEALT As String = "[NAMEALT]"
        Private ReadOnly TOKEN_URL As String = "[URL]"
        Private ReadOnly TOKEN_IMAGE As String = "[IMAGE]"
        Private ReadOnly TOKEN_DESCRIPTION As String = "[DESCRIPTION]"
#End Region
#Region "pageLoad"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                ltContent.Text = LoadData()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
#Region "Bind dataa"
        Private Function LoadData() As String
            Dim isSettingNull As Boolean = LoadSetting()
            Dim sTemplate As String = ""
            If Not isSettingNull Then
                Try
                    Dim cachestring As String = "TemplateBanner" & setting_template & ModuleId & PortalId & TabId
                    Dim cache As Hashtable = Common.Utilities.DataCache.GetCache(Of Hashtable)(cachestring)
                    If (cache Is Nothing) Then
                        cache = New Hashtable
                    End If
                    If Not cache.ContainsKey(cachestring) Then
                        Dim sTemplateFile As String = Server.MapPath("/Portals/0/TemplateBanner/") + setting_template
                        If File.Exists(sTemplateFile) Then
                            sTemplate = File.ReadAllText(sTemplateFile)
                        End If

                        'Box tach file Template                      
                        Dim sTemplate_top As String = If(sTemplate.Contains(TOKEN_LIST_TOP), TrimToken(sTemplate, TOKEN_LIST_TOP), "")
                        Dim sListTop As String = ""

                        Dim bannerController As New BannerAdvController()
                        'Hien thi tin theo cai dat type
                        Dim VitriId As Integer = Convert.ToInt32(setting_vitri)
                        Dim listMore = bannerController.GetAllShow(PortalId, VitriId)
                        If listMore.Count > 0 Then
                            For i As Integer = 0 To (listMore.Count - 1)
                                If i >= listMore.Count Then
                                    Exit For
                                End If
                                Dim banner As BannerAdvInfo = DirectCast(listMore(i), BannerAdvInfo)
                                sListTop += ToHTML(sTemplate_top, banner, (i + 1))
                            Next
                            'Replace token
                            If sTemplate_top <> "" Then
                                sTemplate = sTemplate.Replace(sTemplate_top, sListTop).Replace((Convert.ToString("[") & TOKEN_LIST_TOP) + "]", "").Replace((Convert.ToString("[/") & TOKEN_LIST_TOP) + "]", "")
                            End If

                        Else
                            sTemplate = ""
                        End If
                        cache.Item(cachestring) = sTemplate.ToString()
                        If (DotNetNuke.Common.Globals.PerformanceSettings.HeavyCaching <> PerformanceSettings.NoCaching) Then
                            DotNetNuke.UI.Utilities.DataCache.SetCache(cachestring, cache)
                        End If
                    End If
                    Return cache.Item(cachestring)
                Catch ex As Exception
                    ltContent.Text = "Load module error . " + ex.Message
                End Try
            End If
            'Return sTemplate
        End Function
#End Region
#Region "function"
        Private Function ToHTML(sTemplate As String, banner As BannerAdvInfo, position As Integer) As String
            Dim url As String = "#"
            Dim title As String = ""
            Dim image As String = ""
            Dim Description As String = ""
            If setting_showtieude = True Then
                title = banner.Title
            End If
            If setting_showmota = True Then
                Description = Server.HtmlDecode(banner.Contact)
            End If
            If CutstringPhotoExtension(banner.IMGLink) = "gif" Then
                image = banner.IMGLink.Replace("/DATA", "/data")
            Else
                If (banner.Width = 0) Or (banner.Height = 0) Then
                    image = banner.IMGLink.Replace("/DATA", nvcmsBL.filesDomain)
                Else
                    image = nvcmsBL.FormatThumbImage(banner.IMGLink, banner.Width, banner.Height, "crop", "middle", "")
                End If

            End If
            If banner.KieuBanner = 1 Then
                If (banner.Link = "#") Or (banner.Link = "") Then
                Else
                    url = "/bannerclick/" & BuildEntryLink(banner.id, banner.Title.ToLower())
                End If

            End If
            sTemplate = sTemplate.Replace(TOKEN_DESCRIPTION, Description).Replace(TOKEN_IMAGE, image).Replace(TOKEN_NAME, title).Replace(TOKEN_NAMEALT, HttpUtility.HtmlEncode(title)).Replace(TOKEN_URL, url)
            Return sTemplate
        End Function
        Public Function CutstringPhotoExtension(str As String) As String
            Dim extesnsion As String = ""
            extesnsion = Path.GetExtension(str)
            If extesnsion.Length > 1 Then
                Return extesnsion.Remove(0, 1)
            Else
                Return extesnsion
            End If
            Return extesnsion
        End Function
        Private Function TrimToken(sInput As String, sToken As String) As String
            Try
                Dim sStart As String = (Convert.ToString("[") & sToken) + "]"
                Dim sEnd As String = (Convert.ToString("[/") & sToken) + "]"
                If Not sInput.Contains(sStart) OrElse Not sInput.Contains(sEnd) Then
                    Return ""
                End If

                Dim startIndex As Integer = sInput.IndexOf(sStart, StringComparison.CurrentCultureIgnoreCase) + sStart.Length
                Dim endIndex As Integer = sInput.IndexOf(sEnd, startIndex, StringComparison.CurrentCultureIgnoreCase)
                Dim length As Integer = endIndex - startIndex

                Return sInput.Substring(startIndex, length)
            Catch
                Return ""
            End Try
        End Function
        Private Function BuildEntryLink(EntryId As Integer, ByVal EntryTitle As String) As String
            Dim ignoreCase As RegexOptions = RegexOptions.IgnoreCase
            Dim strTitle As String = (Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(TextHelper.CleanSign(HttpUtility.HtmlDecode(EntryTitle)).Replace("'", String.Empty).Replace("""", String.Empty).Replace("&amp;", String.Empty).Replace("&", String.Empty), ChrW(258) & "|" & ChrW(256) & "|" & ChrW(192) & "|" & ChrW(193) & "|" & ChrW(194) & "|" & ChrW(195) & "|" & ChrW(196) & "|" & ChrW(197), "A"), ChrW(259) & "|" & ChrW(257) & "|" & ChrW(224) & "|" & ChrW(225) & "|" & ChrW(226) & "|" & ChrW(227) & "|" & ChrW(228) & "|" & ChrW(229) & "|" & ChrW(261), "a"), ChrW(198), "AE"), ChrW(230), "ae"), ChrW(223), "ss"), ChrW(199) & "|" & ChrW(262) & "|" & ChrW(264) & "|" & ChrW(266) & "|" & ChrW(268), "C"), ChrW(263) & "|" & ChrW(265) & "|" & ChrW(267) & "|" & ChrW(269) & "|" & ChrW(231), "c"), ChrW(270) & "|" & ChrW(272), "D"), ChrW(271) & "|" & ChrW(273), "d"), ChrW(274) & "|" & ChrW(276) & "|" & ChrW(278) & "|" & ChrW(280) & "|" & ChrW(282) & "|" & ChrW(201) & "|" & ChrW(280) & "|" & ChrW(200) & "|" & ChrW(201) & "|" & ChrW(202) & "|" & ChrW(203), "E"), ChrW(275) & "|" & ChrW(277) & "|" & ChrW(279) & "|" & ChrW(281) & "|" & ChrW(283) & "|" & ChrW(234) & "|" & ChrW(235) & "|" & ChrW(232) & "|" & ChrW(233), "e"), ChrW(284) & "|" & ChrW(286) & "|" & ChrW(288) & "|" & ChrW(290) & "|" & ChrW(290), "G"), ChrW(285) & "|" & ChrW(287) & "|" & ChrW(289) & "|" & ChrW(291) & "|" & ChrW(291), "g"), ChrW(292) & "|" & ChrW(294), "H"), ChrW(293) & "|" & ChrW(295), "h"), ChrW(204) & "|" & ChrW(205) & "|" & ChrW(206) & "|" & ChrW(207) & "|" & ChrW(296) & "|" & ChrW(298) & "|" & ChrW(300) & "|" & ChrW(302) & "|" & ChrW(304) & "|" & ChrW(304), "I"), ChrW(236) & "|" & ChrW(237) & "|" & ChrW(238) & "|" & ChrW(239) & "|" & ChrW(297) & "|" & ChrW(299) & "|" & ChrW(301) & "|" & ChrW(303), "i"), ChrW(306), "IJ"), ChrW(308), "J"), ChrW(309), "j"), ChrW(310), "K"), ChrW(311), "k"), ChrW(209) & "|" & ChrW(209), "N"), ChrW(241), "n"), ChrW(210) & "|" & ChrW(211) & "|" & ChrW(212) & "|" & ChrW(213) & "|" & ChrW(214) & "|" & ChrW(216) & "|" & ChrW(336), "O"), ChrW(242) & "|" & ChrW(243) & "|" & ChrW(244) & "|" & ChrW(245) & "|" & ChrW(246) & "|" & ChrW(248) & "|" & ChrW(337), "o"), ChrW(338), "OE"), ChrW(339), "oe"), ChrW(340) & "|" & ChrW(344) & "|" & ChrW(342) & "|" & ChrW(340), "R"), ChrW(345) & "|" & ChrW(343) & "|" & ChrW(341), "r"), ChrW(352) & "|" & ChrW(350) & "|" & ChrW(348) & "|" & ChrW(346), "S"), ChrW(353) & "|" & ChrW(351) & "|" & ChrW(349) & "|" & ChrW(347), "s"), ChrW(356) & "|" & ChrW(354), "T"), ChrW(357) & "|" & ChrW(355), "t"), ChrW(370) & "|" & ChrW(368) & "|" & ChrW(366) & "|" & ChrW(364) & "|" & ChrW(362) & "|" & ChrW(360) & "|" & ChrW(217) & "|" & ChrW(218) & "|" & ChrW(219) & "|" & ChrW(220), "U"), ChrW(371) & "|" & ChrW(369) & "|" & ChrW(367) & "|" & ChrW(365) & "|" & ChrW(363) & "|" & ChrW(361) & "|" & ChrW(250) & "|" & ChrW(251) & "|" & ChrW(252) & "|" & ChrW(249), "u"), ChrW(372), "W"), ChrW(373), "w"), ChrW(376) & "|" & ChrW(374) & "|" & ChrW(221), "Y"), ChrW(375) & "|" & ChrW(255) & "|" & ChrW(253), "y"), ChrW(381) & "|" & ChrW(379) & "|" & ChrW(377), "Z"), ChrW(382) & "|" & ChrW(380) & "|" & ChrW(378), "z"), "[^a-z0-9_-" & ChrW(258) & ChrW(259) & ChrW(256) & ChrW(257) & ChrW(192) & ChrW(193) & ChrW(194) & ChrW(195) & ChrW(196) & ChrW(197) & ChrW(224) & ChrW(225) & ChrW(226) & ChrW(227) & ChrW(228) & ChrW(229) & ChrW(261) & ChrW(230) & ChrW(198) & ChrW(223) & ChrW(199) & ChrW(262) & ChrW(263) & ChrW(264) & ChrW(265) & ChrW(266) & ChrW(267) & ChrW(268) & ChrW(269) & ChrW(231) & ChrW(270) & ChrW(271) & ChrW(272) & ChrW(273) & ChrW(274) & ChrW(275) & ChrW(276) & ChrW(277) & ChrW(278) & ChrW(279) & ChrW(280) & ChrW(281) & ChrW(282) & ChrW(283) & ChrW(201) & ChrW(234) & ChrW(235) & ChrW(280) & ChrW(200) & ChrW(201) & ChrW(202) & ChrW(203) & ChrW(232) & ChrW(233) & ChrW(284) & ChrW(285) & ChrW(286) & ChrW(287) & ChrW(288) & ChrW(289) & ChrW(290) & ChrW(291) & ChrW(290) & ChrW(291) & ChrW(292) & ChrW(293) & ChrW(294) & ChrW(295) & ChrW(204) & ChrW(205) & ChrW(206) & ChrW(207) & ChrW(296) & ChrW(297) & ChrW(298) & ChrW(299) & ChrW(300) & ChrW(301) & ChrW(302) & ChrW(303) & ChrW(304) & ChrW(204) & ChrW(237) & ChrW(238) & ChrW(239) & ChrW(236) & ChrW(306) & ChrW(308) & ChrW(309) & ChrW(310) & ChrW(311) & ChrW(209) & ChrW(209) & ChrW(210) & ChrW(211) & ChrW(212) & ChrW(213) & ChrW(214) & ChrW(336) & ChrW(216) & ChrW(242) & ChrW(243) & ChrW(244) & ChrW(245) & ChrW(337) & ChrW(246) & ChrW(248) & ChrW(241) & ChrW(338) & ChrW(339) & ChrW(340) & ChrW(345) & ChrW(344) & ChrW(343) & ChrW(342) & ChrW(341) & ChrW(340) & ChrW(353) & ChrW(352) & ChrW(351) & ChrW(350) & ChrW(349) & ChrW(348) & ChrW(347) & ChrW(346) & ChrW(357) & ChrW(356) & ChrW(355) & ChrW(354) & ChrW(371) & ChrW(370) & ChrW(369) & ChrW(368) & ChrW(367) & ChrW(366) & ChrW(365) & ChrW(364) & ChrW(363) & ChrW(362) & ChrW(361) & ChrW(360) & ChrW(217) & ChrW(218) & ChrW(219) & ChrW(220) & ChrW(217) & ChrW(250) & ChrW(251) & ChrW(252) & ChrW(249) & ChrW(373) & ChrW(372) & ChrW(376) & ChrW(375) & ChrW(374) & ChrW(221) & ChrW(255) & ChrW(253) & ChrW(382) & ChrW(381) & ChrW(380) & ChrW(379) & ChrW(378) & ChrW(377) & "]+", "-", ignoreCase)).Replace("---", "-")
                    If strTitle.EndsWith("-") Then
                strTitle = strTitle.Remove(strTitle.Length - 1)
            End If

            Return strTitle + "-" + EntryId.ToString()
        End Function
#End Region
#Region "LoadiSettings"
        Private Function LoadSetting() As Boolean
            Dim isNull As Boolean = False
            Try
                'Vi tri
                If Not Null.IsNull(Settings("NVCMSBannerVitriSetting")) Then
                    setting_vitri = Settings("NVCMSBannerVitriSetting")
                Else
                    isNull = True
                End If
                If isNull Then
                    Return isNull
                End If
                'Template
                If Not Null.IsNull(Settings("NVCMSBannerTemplateSetting")) Then
                    setting_template = Settings("NVCMSBannerTemplateSetting")
                Else
                    isNull = True
                End If
                If isNull Then
                    Return isNull
                End If
                'show tieu de
                If Not Null.IsNull(Settings("NVCMSBannerShowTitleSetting")) Then
                    setting_showtieude = Settings("NVCMSBannerShowTitleSetting")
                Else
                    isNull = True
                End If
                If isNull Then
                    Return isNull
                End If
                'show tieu de
                If Not Null.IsNull(Settings("NVCMSBannerShowMotaSetting")) Then
                    setting_showmota = Settings("NVCMSBannerShowMotaSetting")
                Else
                    isNull = True
                End If
                If isNull Then
                    Return isNull
                End If
            Catch
            End Try
            Return isNull
        End Function
#End Region
    End Class
End Namespace