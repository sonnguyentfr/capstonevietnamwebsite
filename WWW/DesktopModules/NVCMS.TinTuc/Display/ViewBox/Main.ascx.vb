Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Entities
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports NVCMS.Modules.TinTuc
Imports System.IO
Imports System.Text.RegularExpressions
Imports DotNetNuke.Common
Imports System.Globalization
Imports System.Collections
Imports DotNetNuke.Entities.Tabs
Imports System.Web.Mvc
Imports NVCMSMVC.Web.Components
Imports NVCMS.Web.Components
Imports System.Diagnostics

Namespace DesktopModules.TinTuc.View
    Public MustInherit Class Main
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase
        Private setting_cate As String
        Private setting_type As String
        Private setting_top As Integer
        Private setting_more As Integer
        Private setting_TopWidth As Integer
        Private setting_TopHeight As Integer
        Private setting_MoreWidth As Integer
        Private setting_MoreHeight As Integer
        Private setting_sizeDes As Integer
        Private setting_sizeTitle As Integer
        Private setting_template As String
        Private ReadOnly TOKEN_LIST_TOP As String = "LIST_TOP"
        Private ReadOnly TOKEN_LIST_CAT As String = "LIST_CAT"
        Private ReadOnly TOKEN_LIST_TOP2 As String = "LIST_TOP2"
        Private ReadOnly TOKEN_LIST_MORE As String = "LIST_MORE"
        Private ReadOnly TOKEN_LIST_ICON As String = "LIST_ICON"
        Private ReadOnly TOKEN_CATURL As String = "[CATURL]"
        Private ReadOnly TOKEN_CATNAME As String = "[CATNAME]"
        Private ReadOnly TOKEN_NEWID As String = "[NEWID]"
        Private ReadOnly TOKEN_USER As String = "[USER]"
        Private ReadOnly TOKEN_NGAY As String = "[NGAY]"
        Private ReadOnly TOKEN_THU As String = "[THU]"
        Private ReadOnly TOKEN_YEAR As String = "[YEAR]"
        Private ReadOnly TOKEN_NAME As String = "[NAME]"
        Private ReadOnly TOKEN_VIEWCOUNT As String = "[VIEWCOUNT]"
        Private ReadOnly TOKEN_TAG As String = "[TAG]"
        Private ReadOnly TOKEN_SOURCEDOMAIN As String = "[SOURCEDOMAIN]"
        Private ReadOnly TOKEN_SOURCE As String = "[SOURCE]"
        Private ReadOnly TOKEN_SOURCEPLAY As String = "[SOURCEPLAY]"
        Private ReadOnly TOKEN_NAMETITLE As String = "[NAMEALT]"
        Private ReadOnly TOKEN_NAMECAT As String = "[NAMECAT]"
        Private ReadOnly TOKEN_URL As String = "[URL]"
        Private ReadOnly TOKEN_URLCAT As String = "[URLCAT]"
        Private ReadOnly TOKEN_IMAGE As String = "[IMAGE]"
        Private ReadOnly TOKEN_IMAGEHEIGHT As String = "[IMAGEHEIGHT]"
        Private ReadOnly TOKEN_IMAGEWIDTH As String = "[IMAGEWIDTH]"
        Private ReadOnly TOKEN_IMAGEFULL As String = "[IMAGEFULL]"
        Private ReadOnly TOKEN_DATE As String = "[DATE]"
		Private ReadOnly TOKEN_DATETIME As String = "[DATETIME]"
        Private ReadOnly TOKEN_DATECOUNT As String = "[DATECOUNT]"
        Private ReadOnly TOKEN_ATTACH_FILE As String = "[ATTACH_FILE]"
        Private ReadOnly TOKEN_DESCRIPTION As String = "[DESCRIPTION]"
        Private ReadOnly TOKEN_DESCRIPTIONHTML As String = "[DESCRIPTIONHTML]"
        Private ReadOnly TOKEN_POSITION As String = "[POSITION]"
        Private ReadOnly TOKEN_TOP As String = "TOP_"
        Private ReadOnly TOKEN_EXPIRED_DATE As String = "[EXPIRED_DATE]"
        Private ReadOnly TOKEN_CONTENT As String = "[CONTENT]"
        Private newsController As NV_NewsController = New NV_NewsController()
        Private _NewsSettingsController As NewsSettingsController = New NewsSettingsController()
        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            Dim dt As DateTime = DateTime.Now.AddDays(10)
            Response.Cache.SetCacheability(HttpCacheability.[Public])
            Response.Cache.SetExpires(dt)
            Response.Cache.SetMaxAge(New TimeSpan(dt.Ticks - DateTime.Now.Ticks))
            Response.ClearHeaders()
            MyBase.OnLoad(e)
        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                'Dim sw As New Stopwatch
                'sw.Start()
                ltContent.Text = LoadData()
                'sw.Stop()
                'ltrllia.Text = sw.ElapsedMilliseconds.ToString() & "-ms"
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub
		<CompressContent>
        <OutputCache(Duration:=60, VaryByParam:="*", Location:=OutputCacheLocation.Client)>
        Private Function ToHTML(ByVal sTemplate As String, ByVal news As NV_NewsInfo, ByVal position As Integer, ByVal imgWidth As Integer, ByVal imgHeight As Integer) As String
            Dim tabID As Integer = BL.GetMappingTabIDByCategoryID(news.CategoryId)
            If tabID = -1 OrElse CStr(tabID) Is Nothing Then
                tabID = BL.tabDanhMuc
            End If
            Dim url As String = "#"
            Dim urlcat As String = "#"
            Dim ctlNewsByPortal As NV_NewsController = New NV_NewsController()
            Dim objNewsPortal As NV_NewsInfo
            objNewsPortal = ctlNewsByPortal.GetByID(news.NewId)
            url = Ultis.FormatLink(tabID, news.NewId, news.Title)
            urlcat = Globals.NavigateURL(tabID)
            Dim title As String = HttpUtility.HtmlEncode(news.Title)

            If setting_sizeTitle <> 0 AndAlso title <> "" Then
                title = Ultis.SubString(title, setting_sizeTitle, "...")
            End If

            Dim tag As String = ""

            If news.Tags <> "" Then
                Dim sTags As String() = news.Tags.Split(","c)

                For i As Integer = 0 To sTags.Length - 1
                    Dim tagreplace As String = sTags(i)
                    tagreplace = tagreplace.Replace(" ", "+")

                    If sTags(i) <> "" Then
                        tag += "<li><a href='/tags.html?tag=" & tagreplace & "'><span class='trending-span'>#</span>" & sTags(i) & "</a><li>"
                    End If
                Next
            End If

            Dim sourcedomain As String = ""

            If news.SourceText <> "" Then
                If Ultis.IsValidURL(news.SourceText) Then
                    Dim myUri As Uri = New Uri(news.SourceText)
                    sourcedomain = myUri.Host
                End If
            End If
            Dim titleatl As String = HttpUtility.HtmlEncode(news.Title)
            Dim titlecat As String = news.CategoryName
            Dim thu As String = news.PublishedDate.ToString("dd")
            Dim nam As String = news.PublishedDate.ToString("yyyy")
            Dim ngay As String = news.PublishedDate.ToString("MM", New CultureInfo("vi-VN"))
            Dim dates As String = news.PublishedDate.ToShortDateString()
			Dim datestime As String = news.PublishedDate.ToString("HH:mm")
            Dim expiredDate As String = "01/01/2100"
            Dim attachFiles As String = If(news.AttachedFiles <> "", news.AttachedFiles, "#")
            Dim datecount As String = Ultis.ToRelativeDate(news.PublishedDate)
            Dim image As String = ""

            If imgWidth <> 0 AndAlso imgHeight <> 0 Then
                image = Ultis.FormatThumbImage(news.ImagePath, imgWidth, imgHeight, "crop", "middle", "")
            End If

            If imgWidth = 0 And imgHeight = 0 Then
                image = news.ImagePath
            End If

            If imgWidth <> 0 AndAlso imgHeight = 0 Then
                image = Ultis.FormatThumbImage(news.ImagePath, imgWidth, 0, "crop", "middle", "")
            End If

            If imgWidth = 0 AndAlso imgHeight <> 0 Then
                image = Ultis.FormatThumbImage(news.ImagePath, 0, imgHeight, "crop", "middle", "")
            End If

            Dim sourceplay As String = ""

            If news.IsVideo Then

                If news.SourceText.Contains("facebook") Then
                    sourceplay = "<div class='fb-video' data-href='" & news.SourceText & "' data-width='" + imgWidth & "' data-height='" + imgHeight & "' data-show-text='false'></div>"
                End If
            Else
                sourceplay = news.SourceText
            End If

            Dim source As String = news.SourceText
            Dim username As String = ""
            username = BL.GetUserName(PortalId, news.UserId)
            Dim description As String = BL.RemoveHTMLTags(news.Summary).Replace("<", "").Replace(">", "")
            Dim descriptionhtml As String = news.Summary
            If setting_sizeDes <> 0 AndAlso description <> "" Then
                description = Ultis.SubString(description, setting_sizeDes, "...")
            End If
            description = HttpUtility.HtmlEncode(description)
            sTemplate = sTemplate.Replace(TOKEN_DESCRIPTIONHTML, descriptionhtml).Replace(TOKEN_DESCRIPTION, description).Replace(TOKEN_IMAGE, image).Replace(TOKEN_IMAGEHEIGHT, Convert.ToString(imgHeight)).Replace(TOKEN_IMAGEWIDTH, Convert.ToString(imgWidth)).Replace(TOKEN_IMAGEFULL, news.ImagePath).Replace(TOKEN_DATE, dates).Replace(TOKEN_DATETIME, datestime).Replace(TOKEN_DATECOUNT, datecount).Replace(TOKEN_NGAY, ngay).Replace(TOKEN_THU, thu).Replace(TOKEN_YEAR, nam).Replace(TOKEN_USER, username).Replace(TOKEN_SOURCE, source).Replace(TOKEN_SOURCEPLAY, sourceplay).Replace(TOKEN_SOURCEDOMAIN, sourcedomain).Replace(TOKEN_NAME, title).Replace(TOKEN_NEWID, news.NewId.ToString).Replace(TOKEN_VIEWCOUNT, news.ViewCount.ToString()).Replace(TOKEN_TAG, tag).Replace(TOKEN_NAMETITLE, titleatl).Replace(TOKEN_NAMECAT, titlecat).Replace(TOKEN_URL, url).Replace(TOKEN_URLCAT, urlcat).Replace(TOKEN_POSITION, position.ToString()).Replace(TOKEN_EXPIRED_DATE, expiredDate).Replace(TOKEN_ATTACH_FILE, attachFiles)
            If sTemplate.Contains(TOKEN_CONTENT) Then sTemplate = sTemplate.Replace(TOKEN_CONTENT, Server.HtmlDecode(news.Content))
            Return sTemplate
        End Function

        Private Function ToHTMLCat(ByVal sTemplate As String, ByVal news As NV_NewsCategoriesInfo, ByVal position As Integer) As String
            Dim tabID As Integer = BL.GetMappingTabIDByCategoryID(news.CategoryID)
            Dim title As String = ReplaceChuoi.titlenews(news.CategoryName)
            sTemplate = sTemplate.Replace(TOKEN_NAME, title).Replace(TOKEN_URL, Globals.NavigateURL(tabID))
            Return sTemplate
        End Function

        <CompressContent>
        <OutputCache(Duration:=60, VaryByParam:="*", Location:=OutputCacheLocation.Client)>
        Private Function LoadData() As String

            Dim isSettingNull As Boolean = LoadSetting()
            Dim sTemplate As String = ""
            If Not isSettingNull Then

                Try

                    Dim cachestring As String = "Template" & setting_template & ModuleId
                    Dim cache As Hashtable = DataCache.GetCache(Of Hashtable)(cachestring)
                    If (cache Is Nothing) Then
                        cache = New Hashtable
                    End If
                    If Not cache.ContainsKey(cachestring) Then
                        Dim sTemplateFile As String = Server.MapPath("/Portals/0/NewsTemplates/") & setting_template
                        If File.Exists(sTemplateFile) Then
                            sTemplate = File.ReadAllText(sTemplateFile)

                            Dim sTemplate_top As String = If(sTemplate.Contains(TOKEN_LIST_TOP), TrimToken(sTemplate, TOKEN_LIST_TOP), "")
                            Dim sTemplate_top2 As String = If(sTemplate.Contains(TOKEN_LIST_TOP2), TrimToken(sTemplate, TOKEN_LIST_TOP2), "")
                            Dim sTemplate_more As String = If(sTemplate.Contains(TOKEN_LIST_MORE), TrimToken(sTemplate, TOKEN_LIST_MORE), "")
                            Dim sTemplate_cat As String = If(sTemplate.Contains(TOKEN_LIST_CAT), TrimToken(sTemplate, TOKEN_LIST_CAT), "")
                            Dim sTemplate_icon As String = ""
                            Dim sTemplate_top_item As String() = New String(setting_top - 1) {}

                            For i As Integer = 0 To sTemplate_top_item.Length - 1
                                If sTemplate.Contains("[" & TOKEN_TOP & (i + 1) & "]") Then
                                    sTemplate_top_item(i) = TrimToken(sTemplate, TOKEN_TOP & (i + 1).ToString())
                                Else
                                    sTemplate_top_item(i) = ""
                                End If
                            Next

                            Dim sListTop As String = ""
                            Dim sListTop2 As String = ""
                            Dim sListMore As String = ""
                            Dim sListIcon As String = ""
                            Dim sListCat As String = ""
                            Dim newsController As NV_NewsController = New NV_NewsController()
                            Dim newsCat As NV_NewsCategoriesController = New NV_NewsCategoriesController()

                            If setting_type = "Cate" Then

                                Dim cateID As Integer = 0
                                cateID = Convert.ToInt16(setting_cate)
                                Dim SubtractIds As String = ""
                                Dim catController As NV_NewsCategoriesController = New NV_NewsCategoriesController()
                                Dim objCat As NV_NewsCategoriesInfo
                                objCat = catController.GetByID(cateID)
                                Dim CatName As String = ""
                                CatName = objCat.CategoryName
                                Dim CatURL As String = "#"
                                Dim tabid = BL.GetMappingTabIDByCategoryID(cateID)
                                CatURL = Globals.NavigateURL(tabid)
                                sTemplate = sTemplate.Replace(TOKEN_CATNAME, CatName)
                                sTemplate = sTemplate.Replace(TOKEN_CATURL, CatURL)
                                Dim listcat = newsCat.GetByParentId(cateID, 0)

                                If listcat.Count > 0 Then

                                    For i As Integer = 0 To listcat.Count - 1
                                        If i >= listcat.Count Then Exit For
                                        Dim cat As NV_NewsCategoriesInfo = CType(listcat(i), NV_NewsCategoriesInfo)
                                        sListCat += ToHTMLCat(sTemplate_cat, cat, (i + 1))
                                    Next
                                End If
                                Dim cacheName As String = BL.NewsCatList & "Cate" & cateID & setting_more & setting_top & ModuleId
                                ' Dim fromCache As ArrayList = HttpCacheHelper.GetFromCache(cacheName)
                                ' If fromCache Is Nothing Then
                                    ' Dim arrtop As ArrayList = newsController.ShowBaiMoiDanhMuc("", cateID, 0, (setting_more + setting_top), False)
                                    ' If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                        ' fromCache = arrtop
                                        ' HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News"}, cacheName, fromCache, TimeSpan.FromDays(30))
                                    ' End If
                                ' End If

                                ' Dim listMore = fromCache
                                '--
								If DataCache.GetCache(cacheName) Is Nothing Then
									Dim arrtop As ArrayList = newsController.ShowBaiMoiDanhMuc("", cateID, 0, (setting_more + setting_top), False)
									DataCache.SetCache(cacheName, arrtop)
								End If
								Dim listMore = DataCache.GetCache(cacheName)
                                If setting_top > 0 Then

                                    For i As Integer = 0 To setting_top - 1
                                        If i >= listMore.Count Then Exit For
                                        Dim news As NV_NewsInfo = CType(listMore(i), NV_NewsInfo)
                                        SubtractIds += news.NewId & ","

                                        If sTemplate_top_item(i) <> "" Then
                                            sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                        Else
                                            sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                            sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                        End If
                                    Next

                                    For i As Integer = listMore.Count To setting_top - 1
                                        If sTemplate.Contains(TOKEN_TOP) Then
                                            sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                        Else
                                            sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                            Exit For
                                        End If
                                    Next
                                End If

                                If setting_more > 0 AndAlso sTemplate_more <> "" Then

                                    For i As Integer = setting_top To listMore.Count - 1
                                        If i >= listMore.Count Then Exit For
                                        Dim news As NV_NewsInfo = CType(listMore(i), NV_NewsInfo)
                                        sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                    Next
                                End If
                            Else
                                sTemplate_icon = TrimToken(sTemplate, TOKEN_LIST_ICON)

                                If setting_type = "" Then
                                    sTemplate = ""
                                ElseIf setting_type = "Slider" Then
                                    Dim SubtractIds As String = ""
                                    Dim cacheNameSlider As String = BL.NewsCatList & "Slider" & 1 & setting_more & setting_top & ModuleId
                                    ' Dim fromCacheSlider As ArrayList = HttpCacheHelper.GetFromCache(cacheNameSlider)
                                    ' If fromCacheSlider Is Nothing Then
                                        ' Dim arrtop As ArrayList = _NewsSettingsController.GetAllByType(1, (setting_more + setting_top), 0)
                                        ' If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                            ' fromCacheSlider = arrtop
                                            ' HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News_Settings"}, cacheNameSlider, fromCacheSlider, TimeSpan.FromDays(30))
                                        ' End If
                                    ' End If
                                    ' Dim listMoreTinNong = fromCacheSlider
									If DataCache.GetCache(cacheNameSlider) Is Nothing Then
										Dim arrtop As ArrayList = _NewsSettingsController.GetAllByType(1, (setting_more + setting_top), 0)
										DataCache.SetCache(cacheNameSlider, arrtop)
									End If
									Dim listMoreTinNong = DataCache.GetCache(cacheNameSlider)
                                    If setting_top > 0 Then
                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim newsseting As NewsSettingsInfo = CType(listMoreTinNong(i), NewsSettingsInfo)
                                            Dim news As NV_NewsInfo = newsController.GetByID(Convert.ToInt32(newsseting.NewId))
                                            If Not news Is Nothing And news.Status = NewsStatus.DaXuatBan And news.PublishedDate < DateTime.Now And news.isActive = True Then
                                                Session("SubtractIdsSlider") += news.NewId & ","
                                                If sTemplate_top_item(i) <> "" Then
                                                    sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                                Else
                                                    sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                    sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                End If
                                            End If
                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1
                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then
                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim newsseting As NewsSettingsInfo = CType(listMoreTinNong(i), NewsSettingsInfo)
                                            Dim news As NV_NewsInfo = newsController.GetByID(Convert.ToInt32(newsseting.NewId))
                                            If Not news Is Nothing And news.Status = NewsStatus.DaXuatBan Then
                                                Session("SubtractIdsSlider") += news.NewId & ","
                                                sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                            End If

                                        Next
                                    End If
                                ElseIf setting_type = "TinNong" Then
                                    Dim SubtractIds As String = ""
                                    Dim cacheNameTinNong As String = BL.NewsCatList & "TinNong" & 2 & setting_more & setting_top & ModuleId
                                    Dim fromCacheTinNong As ArrayList = HttpCacheHelper.GetFromCache(cacheNameTinNong)
                                    If fromCacheTinNong Is Nothing Then
                                        Dim arrtop As ArrayList = _NewsSettingsController.GetAllByType(2, (setting_more + setting_top), 0)
                                        If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                            fromCacheTinNong = arrtop
                                            HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News_Settings"}, cacheNameTinNong, fromCacheTinNong, TimeSpan.FromDays(30))
                                        End If
                                    End If
                                    Dim listMoreTinNong = fromCacheTinNong
                                    If setting_top > 0 Then

                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim newsseting As NewsSettingsInfo = CType(listMoreTinNong(i), NewsSettingsInfo)
                                            SubtractIds += newsseting.NewId & ","

                                            If sTemplate_top_item(i) <> "" Then
                                                Dim news As NV_NewsInfo = newsController.GetByID(Convert.ToInt32(newsseting.NewId))
                                                If Not news Is Nothing Then
                                                    sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                                End If

                                            Else
                                                Dim news As NV_NewsInfo = newsController.GetByID(Convert.ToInt32(newsseting.NewId))
                                                If Not news Is Nothing Then
                                                    sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                    sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                End If

                                            End If
                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1

                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then

                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                        Next
                                    End If
                                ElseIf setting_type = "XuHuongDoc" Then
                                    Dim SubtractIds As String = ""
                                    'Dim listMoreTinNong = _NewsSettingsController.GetAllByType(3, (setting_more + setting_top), 0)
                                    '--
                                    Dim cacheNameXuHuongDoc As String = BL.NewsCatList & "XuHuongDoc" & 1 & setting_more & setting_top & ModuleId
                                    Dim fromCacheXuHuongDoc As ArrayList = HttpCacheHelper.GetFromCache(cacheNameXuHuongDoc)
                                    If fromCacheXuHuongDoc Is Nothing Then
                                        Dim arrtop As ArrayList = _NewsSettingsController.GetAllByType(3, (setting_more + setting_top), 0)
                                        If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                            fromCacheXuHuongDoc = arrtop
                                            HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News_Settings"}, cacheNameXuHuongDoc, fromCacheXuHuongDoc, TimeSpan.FromDays(30))
                                        End If
                                    End If
                                    Dim listMoreTinNong = fromCacheXuHuongDoc
                                    If setting_top > 0 Then

                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim newsseting As NewsSettingsInfo = CType(listMoreTinNong(i), NewsSettingsInfo)
                                            Dim news As NV_NewsInfo = newsController.GetByID(Convert.ToInt32(newsseting.NewId))
                                            If Not news Is Nothing Then
                                                SubtractIds += news.NewId & ","
                                                If sTemplate_top_item(i) <> "" Then
                                                    sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                                Else
                                                    sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                    sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                End If
                                            End If
                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1

                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then

                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim newsseting As NewsSettingsInfo = CType(listMoreTinNong(i), NewsSettingsInfo)
                                            Dim news As NV_NewsInfo = newsController.GetByID(Convert.ToInt32(newsseting.NewId))
                                            If Not news Is Nothing Then
                                                sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                            End If
                                        Next
                                    End If
                                ElseIf setting_type = "TinMoiNhat" Then
                                    Dim SubtractIds As String = ""
                                    '--
                                    Dim cacheNameTinMoiNhat As String = BL.NewsCatList & "TinMoiNhat" & 0 & setting_more & setting_top & ModuleId
                                    Dim fromCacheTinMoiNhat As ArrayList = HttpCacheHelper.GetFromCache(cacheNameTinMoiNhat)
                                    If fromCacheTinMoiNhat Is Nothing Then
                                        Dim Session_SubtractIdsSlider = ""
                                        If Not Session("SubtractIdsSlider") Is Nothing Then
                                            Session_SubtractIdsSlider = Session("SubtractIdsSlider")
                                        End If
                                        Dim arrtop As ArrayList = newsController.ShowBaiMoiNhat(Session_SubtractIdsSlider.ToString, 0, (setting_more + setting_top))
                                        If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                            fromCacheTinMoiNhat = arrtop
                                            HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News"}, cacheNameTinMoiNhat, fromCacheTinMoiNhat, TimeSpan.FromDays(30))
                                        End If
                                    End If
                                    Dim listMoreTinNong = fromCacheTinMoiNhat
                                    If setting_top > 0 Then
                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            If Not news Is Nothing And news.Status = NewsStatus.DaXuatBan And news.PublishedDate < DateTime.Now And news.isActive = True Then
                                                Session("SubtractIdsMoiNhat") += news.NewId & ","
                                                If sTemplate_top_item(i) <> "" Then
                                                    sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                                Else
                                                    sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                    sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                End If
                                            End If

                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1
                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then
                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            Session("SubtractIdsMoiNhat") += news.NewId & ","
                                            sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                        Next
                                    End If
                                ElseIf setting_type = "TinDocNhieu" Then
                                    Dim SubtractIds As String = ""
                                    '--
                                    Dim cacheNameTinDocNhieu As String = BL.NewsCatList & "TinDocNhieu" & 0 & setting_more & setting_top & ModuleId
                                    Dim fromCacheTinDocNhieu As ArrayList = HttpCacheHelper.GetFromCache(cacheNameTinDocNhieu)
                                    If fromCacheTinDocNhieu Is Nothing Then
                                        Dim arrtop As ArrayList = newsController.ShowTopViewSite(0, 30, (setting_more + setting_top))
                                        If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                            fromCacheTinDocNhieu = arrtop
                                            HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News"}, cacheNameTinDocNhieu, fromCacheTinDocNhieu, TimeSpan.FromDays(30))
                                        End If
                                    End If
                                    Dim listMoreTinNong = fromCacheTinDocNhieu

                                    If setting_top > 0 Then

                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            SubtractIds += news.NewId & ","

                                            If sTemplate_top_item(i) <> "" Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                            End If
                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1

                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then

                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                        Next
                                    End If
                                ElseIf setting_type = "Tin24h" Then
                                    Dim SubtractIds As String = ""
                                    '--
                                    Dim thang As Integer = DateTime.Now.ToString("MM")
                                    Dim nam As Integer = DateTime.Now.ToString("yyyy")
                                    Dim ngay As Integer = DateTime.Now.ToString("dd")
                                    Dim cacheNameTin24hqua As String = BL.NewsCatList & "Tin24hqua" & 0 & setting_more & setting_top & ModuleId
                                    Dim fromCacheTin24hqua As ArrayList = HttpCacheHelper.GetFromCache(cacheNameTin24hqua)
                                    If fromCacheTin24hqua Is Nothing Then
                                        Dim arrtop As ArrayList = newsController.ShowYearMonth(0, nam, thang, ngay)
                                        If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                            fromCacheTin24hqua = arrtop
                                            HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News"}, cacheNameTin24hqua, fromCacheTin24hqua, TimeSpan.FromDays(30))
                                        End If
                                    End If
                                    Dim listMoreTinNong = fromCacheTin24hqua
                                    If setting_top > 0 Then
                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            SubtractIds += news.NewId & ","

                                            If sTemplate_top_item(i) <> "" Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                            End If
                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1
                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then
                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                        Next
                                    End If

                                ElseIf setting_type = "TinAnh" Then
                                    Dim SubtractIds As String = ""
                                    Dim listMoreTinNong = newsController.ShowBaiMoiNhat("", PortalId, (setting_more + setting_top))

                                    If setting_top > 0 Then

                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            SubtractIds += news.NewId & ","

                                            If sTemplate_top_item(i) <> "" Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                            End If
                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1

                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then

                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                        Next
                                    End If
                                ElseIf setting_type = "TinVideo" Then
                                    Dim SubtractIds As String = ""
                                    Dim listMoreTinNong = newsController.ShowBaiMoiNhat("", PortalId, (setting_more + setting_top))

                                    If setting_top > 0 Then

                                        For i As Integer = 0 To setting_top - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            SubtractIds += news.NewId & ","

                                            If sTemplate_top_item(i) <> "" Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                                sListTop2 += ToHTML(sTemplate_top2, news, (i + 1), setting_TopWidth, setting_TopHeight)
                                            End If
                                        Next

                                        For i As Integer = listMoreTinNong.Count To setting_top - 1

                                            If sTemplate.Contains(TOKEN_TOP) Then
                                                sTemplate = sTemplate.Replace(sTemplate_top_item(i), "").Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                            Else
                                                sTemplate = sTemplate.Replace(sListTop, "").Replace("[" & TOKEN_LIST_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_LIST_TOP & (i + 1) & "]", "")
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    If setting_more > 0 AndAlso sTemplate_more <> "" Then

                                        For i As Integer = setting_top To listMoreTinNong.Count - 1
                                            If i >= listMoreTinNong.Count Then Exit For
                                            Dim news As NV_NewsInfo = CType(listMoreTinNong(i), NV_NewsInfo)
                                            sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                        Next
                                    End If
                                End If
                            End If

                            If sTemplate_top <> "" Then sTemplate = sTemplate.Replace(sTemplate_top, sListTop).Replace("[" & TOKEN_LIST_TOP & "]", "").Replace("[/" & TOKEN_LIST_TOP & "]", "")
                            If sTemplate_top2 <> "" Then sTemplate = sTemplate.Replace(sTemplate_top2, sListTop2).Replace("[" & TOKEN_LIST_TOP2 & "]", "").Replace("[/" & TOKEN_LIST_TOP2 & "]", "")
                            If sTemplate_more <> "" Then sTemplate = sTemplate.Replace(sTemplate_more, sListMore).Replace("[" & TOKEN_LIST_MORE & "]", "").Replace("[/" & TOKEN_LIST_MORE & "]", "")
                            If sTemplate_icon <> "" Then sTemplate = sTemplate.Replace(sTemplate_icon, sListIcon).Replace("[" & TOKEN_LIST_ICON & "]", "").Replace("[/" & TOKEN_LIST_ICON & "]", "")
                            If sListCat <> "" Then sTemplate = sTemplate.Replace(sTemplate_cat, sListCat).Replace("[" & TOKEN_LIST_CAT & "]", "").Replace("[/" & TOKEN_LIST_CAT & "]", "")
                            cache.Item(cachestring) = sTemplate.ToString()
                        End If
                        If (DotNetNuke.Common.Globals.PerformanceSettings.HeavyCaching <> PerformanceSettings.NoCaching) Then
                            DataCache.SetCache(cachestring, cache)
                        End If
                    End If
                    Return cache.Item(cachestring)
                Catch ex As Exception
                    'ltContent.Text = "Không có dữ liệu! Hoặc lỗi. "
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If

        End Function

        Private Function TrimToken(ByVal sInput As String, ByVal sToken As String) As String
            Try
                Dim sStart As String = "[" & sToken & "]"
                Dim sEnd As String = "[/" & sToken & "]"
                If Not sInput.Contains(sStart) OrElse Not sInput.Contains(sEnd) Then Return ""
                Dim startIndex As Integer = sInput.IndexOf(sStart, StringComparison.CurrentCultureIgnoreCase) + sStart.Length
                Dim endIndex As Integer = sInput.IndexOf(sEnd, startIndex, StringComparison.CurrentCultureIgnoreCase)
                Dim length As Integer = endIndex - startIndex
                Return sInput.Substring(startIndex, length)
            Catch
                Return ""
            End Try
        End Function

        Private Function LoadSetting() As Boolean
            Dim isNull As Boolean = False

            Try

                If Not Null.IsNull(Settings(BL.settingView_Type.ToString())) Then
                    setting_type = Settings(BL.settingView_Type.ToString()).ToString()
                Else
                    isNull = True
                End If

                If isNull Then Return isNull

                If setting_type = "Cate" Then

                    If Not Null.IsNull(Settings(BL.settingView_Cate.ToString())) Then
                        setting_cate = Settings(BL.settingView_Cate.ToString()).ToString()
                    Else
                        isNull = True
                    End If

                    If isNull Then Return isNull
                End If

                If Not Null.IsNull(Settings(BL.settingView_Total.ToString())) Then
                    Dim sTotal As String() = Settings(BL.settingView_Total.ToString()).ToString().Split(";"c)
                    setting_top = Convert.ToInt32(sTotal(0))
                    setting_more = Convert.ToInt32(sTotal(1))
                Else
                    isNull = True
                End If

                If isNull Then Return isNull

                If Not Null.IsNull(Settings(BL.settingView_ImgSize.ToString())) Then
                    Dim sImgSize As String() = Settings(BL.settingView_ImgSize.ToString()).ToString().Split(";"c)
                    Dim sTopSize As String() = sImgSize(0).Split(","c)
                    Dim sMoreSize As String() = sImgSize(1).Split(","c)
                    setting_TopWidth = Convert.ToInt32(sTopSize(0))
                    setting_TopHeight = Convert.ToInt32(sTopSize(1))
                    setting_MoreWidth = Convert.ToInt32(sMoreSize(0))
                    setting_MoreHeight = Convert.ToInt32(sMoreSize(1))
                Else
                    isNull = True
                End If

                If isNull Then Return isNull

                If Not Null.IsNull(Settings(BL.settingView_Template.ToString())) Then
                    setting_template = Settings(BL.settingView_Template.ToString()).ToString()
                Else
                    isNull = True
                End If

                If isNull Then Return isNull

                If Not Null.IsNull(Settings(BL.settingView_SizeDes.ToString())) Then
                    setting_sizeDes = Convert.ToInt32(Settings(BL.settingView_SizeDes.ToString()))
                Else
                    setting_sizeDes = 0
                End If

                If Not Null.IsNull(Settings(BL.settingView_SizeTitle.ToString())) Then
                    setting_sizeTitle = Convert.ToInt32(Settings(BL.settingView_SizeTitle.ToString()))
                Else
                    setting_sizeTitle = 0
                End If

            Catch
            End Try

            Return isNull
        End Function
    End Class
End Namespace
