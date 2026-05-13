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
Imports NVCMS.Modules.Video
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

Namespace DesktopModules.Video.View
    Public MustInherit Class Main
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase
        Private setting_top As Integer
        Private setting_TopWidth As Integer
        Private setting_TopHeight As Integer
        Private setting_more As Integer
        Private setting_MoreWidth As Integer
        Private setting_MoreHeight As Integer
        Private setting_template As String
        Private setting_tab As Integer
        Private ReadOnly TOKEN_LIST_TOP As String = "LIST_TOP"
        Private ReadOnly TOKEN_LIST_MORE As String = "LIST_MORE"
        Private ReadOnly TOKEN_NEWID As String = "[NEWID]"
        Private ReadOnly TOKEN_USER As String = "[USER]"
        Private ReadOnly TOKEN_NAME As String = "[NAME]"
        Private ReadOnly TOKEN_NAMETITLE As String = "[NAMEALT]"
        Private ReadOnly TOKEN_URL As String = "[URL]"
        Private ReadOnly TOKEN_IMAGE As String = "[IMAGE]"
        Private ReadOnly TOKEN_DATE As String = "[DATE]"
        Private ReadOnly TOKEN_DESCRIPTION As String = "[DESCRIPTION]"
        Private ReadOnly TOKEN_CONTENT As String = "[CONTENT]"
        Private _Videos_Controller As Videos_Controller = New Videos_Controller()
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

        Private Function ToHTML(ByVal sTemplate As String, ByVal news As Videos_Info, ByVal position As Integer, ByVal imgWidth As Integer, ByVal imgHeight As Integer) As String
            Dim tabID As Integer = BL.GetMappingTabIDByCategoryID(news.CategoryId)
            If tabID = -1 OrElse CStr(tabID) Is Nothing Then
                tabID = BL.tabDanhMuc
            End If
            Dim url As String = "#"
            Dim urlcat As String = "#"
            url = Ultis.FormatLinkVideo(Convert.ToInt32(setting_tab), news.VideoId, news.Title)
            urlcat = Globals.NavigateURL(tabID)
            Dim title As String = HttpUtility.HtmlEncode(news.Title)
            Dim titleatl As String = HttpUtility.HtmlEncode(news.Title)
            Dim titlecat As String = news.CategoryName
            Dim dates As String = ""
            If BL.GetLanguage() = "en-US" Then
                dates = news.PublishedDate.ToString("dd/MM/yyyy")
            Else
                dates = news.PublishedDate.ToString("dd/MM/yyyy")
            End If
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
            Dim description As String = BL.RemoveHTMLTags(news.Summary).Replace("<", "").Replace(">", "")

            description = HttpUtility.HtmlEncode(description)
            sTemplate = sTemplate.Replace(TOKEN_DESCRIPTION, description).Replace(TOKEN_IMAGE, image).Replace(TOKEN_DATE, dates).Replace(TOKEN_NAME, title).
                Replace(TOKEN_NEWID, news.VideoId.ToString).Replace(TOKEN_NAMETITLE, titleatl).Replace(TOKEN_URL, url)
            If sTemplate.Contains(TOKEN_CONTENT) Then sTemplate = sTemplate.Replace(TOKEN_CONTENT, Server.HtmlDecode(news.Content))
            Return sTemplate
        End Function

        <CompressContent>
        <OutputCache(Duration:=60, VaryByParam:="*")>
        Private Function LoadData() As String

            Dim isSettingNull As Boolean = LoadSetting()
            Dim sTemplate As String = ""
            Dim sresult As String = ""
            If Not isSettingNull Then
                Try
                    Dim cachestring As String = "VideoTemplate" & setting_template & ModuleId & "0"
                    Dim cache As Hashtable = DataCache.GetCache(Of Hashtable)(cachestring)
                    If (cache Is Nothing) Then
                        cache = New Hashtable
                    End If
                    If Not cache.ContainsKey(cachestring) Then
                        Dim sTemplateFile As String = Server.MapPath("/Portals/0/VideoTemplate/") & setting_template
                        If File.Exists(sTemplateFile) Then
                            sTemplate = File.ReadAllText(sTemplateFile)
                            Dim sTemplate_top As String = If(sTemplate.Contains(TOKEN_LIST_TOP), TrimToken(sTemplate, TOKEN_LIST_TOP), "")
                            Dim sTemplate_more As String = If(sTemplate.Contains(TOKEN_LIST_MORE), TrimToken(sTemplate, TOKEN_LIST_MORE), "")
                            Dim sTemplate_icon As String = ""

                            Dim sListTop As String = ""
                            Dim sListTop2 As String = ""
                            Dim sListMore As String = ""
                            Dim sListIcon As String = ""
                            Dim sListCat As String = ""
                            Dim sTemplate_top_item As String() = New String(setting_top - 1) {}
                            'doan nay xu ly text
                            Dim SubtractIds As String = ""
                            Dim cacheName As String = "videoboxe" & setting_more & setting_top & ModuleId & "0"
                            Dim fromCache As ArrayList = HttpCacheHelper.GetFromCache(cacheName)
                            If fromCache Is Nothing Then
                                Dim arrtop As ArrayList = _Videos_Controller.Find_Show_Index(0, 1, (setting_more + setting_top))
                                If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                                    fromCache = arrtop
                                    HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"NVCMS_Video"}, cacheName, fromCache, TimeSpan.FromDays(30))
                                End If
                            End If
                            Dim listMore = fromCache
                            '--
                            If setting_top > 0 Then

                                For i As Integer = 0 To setting_top - 1
                                    If i >= listMore.Count Then Exit For
                                    Dim objVideo As Videos_Info = CType(listMore(i), Videos_Info)
                                    SubtractIds += objVideo.VideoId & ","

                                    If sTemplate_top_item(i) <> "" Then
                                        sTemplate = sTemplate.Replace(sTemplate_top_item(i), ToHTML(sTemplate_top_item(i), objVideo, (i + 1), setting_TopWidth, setting_TopHeight))
                                    Else
                                        sListTop += ToHTML(sTemplate_top, objVideo, (i + 1), setting_TopWidth, setting_TopHeight)
                                    End If
                                Next

                            End If

                            If setting_more > 0 AndAlso sTemplate_more <> "" Then

                                For i As Integer = setting_top To listMore.Count - 1
                                    If i >= listMore.Count Then Exit For
                                    Dim news As Videos_Info = CType(listMore(i), Videos_Info)
                                    sListMore += ToHTML(sTemplate_more, news, (i + 1), setting_MoreWidth, setting_MoreHeight)
                                Next
                            End If


                            If sTemplate_top <> "" Then sTemplate = sTemplate.Replace(sTemplate_top, sListTop).Replace("[" & TOKEN_LIST_TOP & "]", "").Replace("[/" & TOKEN_LIST_TOP & "]", "")
                            If sTemplate_more <> "" Then sTemplate = sTemplate.Replace(sTemplate_more, sListMore).Replace("[" & TOKEN_LIST_MORE & "]", "").Replace("[/" & TOKEN_LIST_MORE & "]", "")
                            cache.Item(cachestring) = sTemplate.ToString()
                        End If
                        If (DotNetNuke.Common.Globals.PerformanceSettings.HeavyCaching <> PerformanceSettings.NoCaching) Then
                            DataCache.SetCache(cachestring, cache)
                        End If
                    End If
                    sresult = cache.Item(cachestring)
                Catch ex As Exception
                    'ltContent.Text = "Không có dữ liệu! Hoặc lỗi. "
                    ProcessModuleLoadException(Me, ex)
                End Try

            End If
            Return sresult
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
                If Not Null.IsNull(Settings("VideoSettingPage")) Then
                    setting_tab = Settings("VideoSettingPage").ToString()
                Else
                    isNull = True
                End If

                If isNull Then Return isNull

            Catch
            End Try

            Return isNull
        End Function
    End Class
End Namespace
