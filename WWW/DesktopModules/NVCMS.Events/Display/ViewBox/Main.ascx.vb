Imports System.IO
Imports System.Web.Mvc
Imports DotNetNuke.Common
Imports NVCMS.Modules.EventsWebsite
Imports NVCMSMVC.Web.Components

Namespace DesktopModules.TinTuc.View
    Public MustInherit Class Main
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase
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
        Private ReadOnly TOKEN_LIST_MORE As String = "LIST_MORE"
        Private ReadOnly TOKEN_LISTITEM As String = "LIST_ITEM"
        Private ReadOnly TOKEN_CATURL As String = "[CATURL]"
        Private ReadOnly TOKEN_CATNAME As String = "[CATNAME]"
        Private ReadOnly TOKEN_NEWID As String = "[NEWID]"
        Private ReadOnly TOKEN_USER As String = "[USER]"
        Private ReadOnly TOKEN_NGAY As String = "[NGAY]"
        Private ReadOnly TOKEN_THU As String = "[THU]"
        Private ReadOnly TOKEN_YEAR As String = "[YEAR]"
        Private ReadOnly TOKEN_NAME As String = "[NAME]"
        Private ReadOnly TOKEN_VIEWCOUNT As String = "[VIEWCOUNT]"
        Private ReadOnly TOKEN_NAMETITLE As String = "[NAMEALT]"
        Private ReadOnly TOKEN_NAMECAT As String = "[NAMECAT]"
        Private ReadOnly TOKEN_URL As String = "[URL]"
        Private ReadOnly TOKEN_URLCAT As String = "[URLCAT]"
        Private ReadOnly TOKEN_IMAGE As String = "[IMAGE]"
        Private ReadOnly TOKEN_IMAGEHEIGHT As String = "[IMAGEHEIGHT]"
        Private ReadOnly TOKEN_IMAGEWIDTH As String = "[IMAGEWIDTH]"
        Private ReadOnly TOKEN_IMAGEFULL As String = "[IMAGEFULL]"
        Private ReadOnly TOKEN_DATE As String = "[DATE]"
        Private ReadOnly TOKEN_DIADIEM As String = "[DIADIEM]"
        Private ReadOnly TOKEN_NGAYDIENRA As String = "[NGAYDIENRA]"
        Private ReadOnly TOKEN_DATECOUNT As String = "[DATECOUNT]"
        Private ReadOnly TOKEN_ATTACH_FILE As String = "[ATTACH_FILE]"
        Private ReadOnly TOKEN_DESCRIPTION As String = "[DESCRIPTION]"
        Private ReadOnly TOKEN_DESCRIPTIONHTML As String = "[DESCRIPTIONHTML]"
        Private ReadOnly TOKEN_POSITION As String = "[POSITION]"
        Private ReadOnly TOKEN_TOP As String = "TOP_"
        Private ReadOnly TOKEN_EXPIRED_DATE As String = "[EXPIRED_DATE]"
        Private ReadOnly TOKEN_CONTENT As String = "[CONTENT]"
        Private eventsCatController As EventsWebsite_CatController = New EventsWebsite_CatController()
        Private eventsController As EventsWebsiteController = New EventsWebsiteController()

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

        Private Function ToHTML(ByVal sTemplate As String, ByVal news As EventsInfo, ByVal position As Integer, ByVal imgWidth As Integer, ByVal imgHeight As Integer) As String
            Dim tabID As Integer = 0
            If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageEvents, PortalId, Null.NullString)) Then
                tabID = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageEvents, PortalId, Null.NullString))
            Else
                tabID = BL.GetMappingTabIDByCategoryID(news.CatId)
            End If

            If tabID = -1 OrElse CStr(tabID) Is Nothing Then
                tabID = BL.tabDanhMuc
            End If
            Dim url As String = "#"
            Dim urlcat As String = "#"
            url = "#" 'Ultis.EventsFormatLink(tabID, news.id, news.Title)
            urlcat = Globals.NavigateURL(tabID)
            Dim title As String = HttpUtility.HtmlEncode(news.Title)

            If setting_sizeTitle <> 0 AndAlso title <> "" Then
                title = Ultis.SubString(title, setting_sizeTitle, "...")
            End If

            Dim tag As String = ""


            Dim titleatl As String = HttpUtility.HtmlEncode(news.Title)
            Dim titlecat As String = news.CatEventName
            Dim thu As String = news.fromdatetime.ToString("dd")
            Dim nam As String = news.fromdatetime.ToString("yyyy")
            Dim ngay As String = news.fromdatetime.ToString("MM", New CultureInfo("vi-VN"))
            Dim dates As String = news.fromdatetime.ToShortDateString()
            Dim expiredDate As String = "01/01/2100"
            Dim datecount As String = Ultis.ToRelativeDate(news.fromdatetime)
            Dim image As String = ""
            Dim diadiem As String = ""
            diadiem = news.diadiem
            If imgWidth <> 0 AndAlso imgHeight <> 0 Then
                image = Ultis.FormatThumbImage(news.Avatar, imgWidth, imgHeight, "crop", "middle", "")
            End If

            If imgWidth = 0 And imgHeight = 0 Then
                image = news.Avatar
            End If

            If imgWidth <> 0 AndAlso imgHeight = 0 Then
                image = Ultis.FormatThumbImage(news.Avatar, imgWidth, 0, "crop", "middle", "")
            End If

            If imgWidth = 0 AndAlso imgHeight <> 0 Then
                image = Ultis.FormatThumbImage(news.Avatar, 0, imgHeight, "crop", "middle", "")
            End If

            Dim sourceplay As String = ""

            Dim username As String = ""
            username = BL.GetUserName(PortalId, news.UserId)
            sTemplate = sTemplate.Replace(TOKEN_IMAGE, image).Replace(TOKEN_IMAGEHEIGHT, Convert.ToString(imgHeight)).Replace(TOKEN_IMAGEWIDTH, Convert.ToString(imgWidth)).Replace(TOKEN_IMAGEFULL, news.Avatar).Replace(TOKEN_NGAYDIENRA, dates).Replace(TOKEN_DIADIEM, diadiem).Replace(TOKEN_DATE, dates).Replace(TOKEN_DATECOUNT, datecount).Replace(TOKEN_NGAY, ngay).Replace(TOKEN_THU, thu).Replace(TOKEN_YEAR, nam).Replace(TOKEN_USER, username).Replace(TOKEN_NAME, title).Replace(TOKEN_NEWID, news.id.ToString).Replace(TOKEN_NAMETITLE, titleatl).Replace(TOKEN_NAMECAT, titlecat).Replace(TOKEN_URL, url).Replace(TOKEN_URLCAT, urlcat).Replace(TOKEN_POSITION, position.ToString()).Replace(TOKEN_EXPIRED_DATE, expiredDate)
            If sTemplate.Contains(TOKEN_CONTENT) Then sTemplate = sTemplate.Replace(TOKEN_CONTENT, Server.HtmlDecode(news.Descreption))
            Return sTemplate
        End Function

        Private Function ToHTMLCat(ByVal sTemplate As String, ByVal news As EventsInfo, ByVal position As Integer) As String
            Dim tabID As Integer = BL.GetMappingTabIDByCategoryID(news.CatId)
            Dim title As String = ReplaceChuoi.titlenews(news.CatEventName)
            'sTemplate = sTemplate.Replace(TOKEN_NAME, title).Replace(TOKEN_URL, Globals.NavigateURL(tabID))
            Return sTemplate
        End Function

        <CompressContent>
        <OutputCache(Duration:=60, VaryByParam:="*")>
        Private Function LoadData() As String

            Dim isSettingNull As Boolean = LoadSetting()
            Dim sTemplate As String = ""
            If Not isSettingNull Then
                Try
                    Dim cachestring As String = "EventsTemplate" & setting_template & ModuleId
                    Dim cache As Hashtable = DataCache.GetCache(Of Hashtable)(cachestring)
                    If (cache Is Nothing) Then
                        cache = New Hashtable
                    End If
                    'If Not cache.ContainsKey(cachestring) Then
                    Dim sTemplateFile As String = Server.MapPath("/Portals/0/EventsTemplates/") & setting_template
                    If File.Exists(sTemplateFile) Then
                        sTemplate = File.ReadAllText(sTemplateFile)

                        Dim sTemplate_top As String = If(sTemplate.Contains(TOKEN_LIST_TOP), TrimToken(sTemplate, TOKEN_LIST_TOP), "")
                        Dim sTemplate_more As String = If(sTemplate.Contains(TOKEN_LIST_MORE), TrimToken(sTemplate, TOKEN_LIST_MORE), "")
                        Dim sTemplate_icon As String = ""
                        Dim sTemplate_top_item As String() = New String(setting_top - 1) {}

                        For i As Integer = 0 To sTemplate_top_item.Length - 1

                            If sTemplate.Contains("[" & TOKEN_TOP & (i + 1) & "]") Then
                                sTemplate_top_item(i) = TrimToken(sTemplate, TOKEN_TOP & (i + 1).ToString())
                                'Response.Write(sTemplate_top_item(i))
                            Else
                                sTemplate_top_item(i) = ""
                            End If
                        Next

                        Dim sListTop As String = ""
                        Dim sListTop2 As String = ""
                        Dim sListMore As String = ""
                        Dim sListIcon As String = ""
                        Dim sListCat As String = ""
                        Dim cateID As Integer = 0
                        Dim SubtractIds As String = ""
                        Dim objCat As Events_CatInfo
                        Dim CatName As String = ""
                        objCat = eventsCatController.Events_Cat_GetByID(cateID, 50)
                        If Not objCat Is Nothing Then
                            With objCat
                                CatName = objCat.CatName
                            End With
                        End If
                        Dim CatURL As String = "#"
                        'Dim tabid = BL.GetMappingTabIDByCategoryID(cateID)
                        'CatURL = Globals.NavigateURL(tabid)
                        sTemplate = sTemplate.Replace(TOKEN_CATNAME, CatName)
                        sTemplate = sTemplate.Replace(TOKEN_CATURL, CatURL)
                        Dim cacheName As String = BL.NewsCatList & "Cate" & cateID & setting_more & setting_top & ModuleId
                        Dim fromCache As ArrayList = HttpCacheHelper.GetFromCache(cacheName)
                        If fromCache Is Nothing Then
                            'Dim arrtop As ArrayList = eventsController.Events_FindShow_Index("", 50, 1, 1, (setting_more + setting_top))
                            Dim arrCat As ArrayList = eventsCatController.Events_Cat_GetAllShowOnline(50)
                            If arrCat IsNot Nothing AndAlso arrCat.Count() > 0 Then
                                fromCache = arrCat
                                'HttpCacheHelper.SaveToCacheDependency("CapstoneVietnam", New String() {"NVCMS_Events"}, cacheName, fromCache, TimeSpan.FromDays(30))
                            End If
                        End If
                        Dim listMore = fromCache
                        '--
                        If setting_top > 0 Then
                            If Not listMore Is Nothing AndAlso listMore.Count > 0 Then
                                For i As Integer = 0 To setting_top - 1
                                    If i >= listMore.Count Then Exit For
                                    Dim news As EventsInfo = CType(listMore(i), EventsInfo)
                                    'SubtractIds += news.id & ","
                                    'Event
                                    Dim arrEvent = eventsController.Events_GetAllShowByCat(news.id, 50)
                                    If arrEvent.Count > 0 Then
                                        Dim sTemplate_Events As String = If(sTemplate.Contains(TOKEN_LISTITEM), TrimToken(sTemplate, TOKEN_LISTITEM), "")
                                        Dim sListEvents As String = ""
                                        For i2 As Integer = 0 To (arrEvent.Count - 1)
                                            If i2 >= arrEvent.Count Then
                                                Exit For
                                            End If
                                            Dim objmedia As EventsInfo = DirectCast(arrEvent(i2), EventsInfo)
                                            sListEvents += ToHTML(sTemplate_Events, objmedia, (i2 + 1), setting_TopWidth, setting_TopHeight)
                                        Next
                                        If sListEvents <> "" Then
                                            sTemplate = sTemplate.Replace(sTemplate_Events, sListEvents).Replace((Convert.ToString("[") & TOKEN_LISTITEM) + "]", "").Replace((Convert.ToString("[/") & TOKEN_LISTITEM) + "]", "")
                                        End If
                                    End If
                                    If sTemplate_top_item(i) <> "" Then
                                        sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), news, (i + 1), setting_TopWidth, setting_TopHeight)).Replace("[" & TOKEN_TOP & (i + 1) & "]", "").Replace("[/" & TOKEN_TOP & (i + 1) & "]", "")
                                    Else
                                        sListTop += ToHTML(sTemplate_top, news, (i + 1), setting_TopWidth, setting_TopHeight)

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
                            Else
                                sTemplate = "Không có dữ liệu!"
                            End If

                        End If
                        If setting_more > 0 AndAlso sTemplate_more <> "" Then
                            For i As Integer = setting_top To listMore.Count - 1
                                If i >= listMore.Count Then Exit For
                                Dim news As EventsInfo = CType(listMore(i), EventsInfo)
                                sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                            Next
                        End If


                        If sTemplate_top <> "" Then sTemplate = sTemplate.Replace(sTemplate_top, sListTop).Replace("[" & TOKEN_LIST_TOP & "]", "").Replace("[/" & TOKEN_LIST_TOP & "]", "")
                        If sTemplate_more <> "" Then sTemplate = sTemplate.Replace(sTemplate_more, sListMore).Replace("[" & TOKEN_LIST_MORE & "]", "").Replace("[/" & TOKEN_LIST_MORE & "]", "")
                        'cache.Item(cachestring) = sTemplate.ToString()
                    End If
                    If (DotNetNuke.Common.Globals.PerformanceSettings.HeavyCaching <> PerformanceSettings.NoCaching) Then
                        DataCache.SetCache(cachestring, cache)
                    End If
                    'End If
                    Return sTemplate.ToString() ' cache.Item(cachestring)
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
