Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports HtmlAgilityPack
Imports NVCMS.Modules.FairGuide
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.LadingPage
    Partial Class MainCustomeDisplay
        Inherits Entities.Modules.PortalModuleBase

        Private setting_details_template As String
        Private setting_trangLadingPage As Integer

        Dim trangLadingPage As Integer = 0
        Dim setting_showtitle As Boolean = False
        Dim setting_showsubpage As Boolean = False

        Private ReadOnly TOKEN_NAME As String = "[NAME]"
        Private ReadOnly TOKEN_NAMEPHU As String = "[NAMEPHU]"
        Private ReadOnly TOKEN_URL As String = "[URL]"
        Private ReadOnly TOKEN_NAMETITLE As String = "[NAMEALT]"
        Private ReadOnly TOKEN_IMAGE As String = "[IMAGE]"
        Private ReadOnly TOKEN_DESCREPTION As String = "[DESCREPTION]"
        Private ReadOnly TOKEN_CONTENT As String = "[CONTENT]"
        Private ReadOnly TOKEN_ID As String = "[ID]"

        Dim _LadingPage_Controller As New LadingPage_Controller
        Dim _Media_Controller As New Media_Controller
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not IsPostBack Then
                    LoadSetting()
                    Dim sTemplate As String = ""
                    Dim sTemplateFile As String = Server.MapPath("/Portals/0/LadingPageTemplates/") + setting_details_template
                    If File.Exists(sTemplateFile) Then sTemplate = File.ReadAllText(sTemplateFile)
                    Dim scontent As String = ""
                    Dim fbclid = Request.QueryString("fbclid")
                    Dim sUrl = Request.RawUrl.Replace("?fbclid=" + fbclid, "")
                    Dim itemId = Ultis.GetRequestId(sUrl)
                    'Dim objLadingPage As LadingPage_Info = _LadingPage_Controller._GetByID(itemId, PortalId)

                    Dim cacheName As String = "Ladingage_Detail" & itemId
                    Dim objLadingPage As LadingPage_Info = DataCache.GetCache(cacheName)
                    If objLadingPage Is Nothing Then
                        objLadingPage = New LadingPage_Info()
                        With objLadingPage

                            Dim objLadingPageFromDB As LadingPage_Info = _LadingPage_Controller._GetByID(itemId, PortalId)
                            If Not objLadingPageFromDB Is Nothing Then
                                Dim titlelink As String = Ultis.FormatLinkLadingPage(TabId, itemId, objLadingPageFromDB.TrangDanhMuc)
                                Dim requestedUrl As String = DirectCast(HttpContext.Current.Items()("UrlRewrite:OriginalUrl"), String)
                                If requestedUrl <> titlelink Then
                                    Response.StatusCode = 301
                                    Response.RedirectLocation = titlelink
                                    Response.Flush()
                                End If
                                objLadingPage = objLadingPageFromDB
                                DataCache.SetCache(cacheName, objLadingPage)
                                'HttpCacheHelper.SaveToCacheDependency("CapstoneVietNamV2", New String() {"News"}, cacheName, objLadingPageFromDB, TimeSpan.FromDays(30))
                            End If
                        End With

                    Else
                        Dim titlelink As String = Ultis.FormatLinkLadingPage(TabId, itemId, objLadingPage.TrangDanhMuc)
                        Dim requestedUrl As String = DirectCast(HttpContext.Current.Items()("UrlRewrite:OriginalUrl"), String)
                        If requestedUrl <> titlelink Then
                            Response.StatusCode = 301
                            Response.RedirectLocation = titlelink
                            Response.Flush()
                        End If
                    End If

                    ltContent.Text = setting_trangLadingPage.ToString()
                    If Not objLadingPage Is Nothing Then
                        With objLadingPage
                            Dim titlelink As String = Ultis.FormatLinkLadingPage(TabId, itemId, objLadingPage.TrangDanhMuc)
                            Dim requestedUrl As String = DirectCast(HttpContext.Current.Items()("UrlRewrite:OriginalUrl"), String)
                            If requestedUrl <> titlelink Then
                                Response.Redirect(titlelink, True)
                            End If
                            Dim cp As DotNetNuke.Framework.CDefault = CType(Page, DotNetNuke.Framework.CDefault)
                            cp.Title = objLadingPage.TrangDanhMuc
                            cp.Description = HttpUtility.HtmlEncode(objLadingPage.tomtat)

                            Dim parts = SplitHtmlSections(.Noidung)
                            'Lây phần head để thêm vào header trang
                            Dim strlinkcssjs As String = ""
                            strlinkcssjs = parts.HeadImports
                            Dim htmlHeaderCtrl2 As New LiteralControl()
                            htmlHeaderCtrl2.Text = strlinkcssjs.ToString()
                            Page.Header.Controls.Add(htmlHeaderCtrl2)


                            'Console.WriteLine("BODY:")
                            'Console.WriteLine(parts.BodyContent)

                            'Console.WriteLine("SCRIPTS:")
                            'Console.WriteLine(parts.FooterScripts)
                            scontent += parts.BodyContent & parts.FooterScripts

                            sTemplate = sTemplate.Replace(TOKEN_NAME, .TrangDanhMuc).Replace(TOKEN_NAMEPHU, .Tieudephu).Replace(TOKEN_URL, .Link).Replace(TOKEN_NAMETITLE, ReplaceChuoi.titlenews(.TrangDanhMuc)).Replace(TOKEN_IMAGE, .ImagePath.Replace("/DATA", BL.filesDomain)).Replace(TOKEN_DESCREPTION, .tomtat).Replace(TOKEN_CONTENT, Server.HtmlDecode(scontent))
                            ltContent.Text = If(sTemplate <> "", sTemplate, setting_details_template & "Module này chưa được áp dụng Template. Vui lòng chọn Template !")
                        End With
                    Else
                        ltContent.Text += "Nội dung đang cập nhật"
                    End If

                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Function ToHTMLLadingPage(sTemplate As String, objmedia As LadingPage_Info, position As Integer) As String
            Dim Image As String = "#"
            Dim title As String = ""
            Dim Stitle As String = ""
            Dim scontent As String = ""
            Dim sid As Integer = 0
            Dim salt As String = ""
            Dim surl As String = "#"
            Dim descreption As String = ""
            If Not objmedia Is Nothing Then
                With objmedia
                    title = objmedia.TrangDanhMuc
                    Stitle = objmedia.Tieudephu
                    salt = ReplaceChuoi.titlenews(.TrangDanhMuc)
                    sid = objmedia.id
                    surl = .Link
                    descreption = .tomtat
                    scontent = Server.HtmlDecode(objmedia.Noidung)
                    Image = objmedia.ImagePath.Replace("/DATA", "/DATA")
                End With
            End If

            sTemplate = sTemplate.Replace(TOKEN_NAME, title).Replace(TOKEN_IMAGE, Image).Replace(TOKEN_ID, sid.ToString()).Replace(TOKEN_CONTENT, scontent).Replace(TOKEN_NAMEPHU, Stitle).Replace(TOKEN_NAMETITLE, salt).Replace(TOKEN_DESCREPTION, descreption).Replace(TOKEN_URL, surl)
            Return sTemplate
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
        Private Function LoadSetting() As Boolean
            Dim isNull As Boolean = False
            Try
                If Not Null.IsNull(ModuleConfiguration.ModuleSettings("TrangLadingPage_Template_Detail")) Then
                    setting_details_template = ModuleConfiguration.ModuleSettings("TrangLadingPage_Template_Detail").ToString()
                Else
                    isNull = True
                End If
                If isNull Then Return isNull
                '-----
            Catch ex As Exception
                ltContent.Text = ex.Message
            End Try

            Return isNull
        End Function
        ''' <summary>
        ''' Cắt đoạn code html thành 3 phần: HeadImports, BodyContent, FooterScripts
        ''' </summary>
        ''' <param name="htmlContent"></param>
        ''' <returns></returns>
        Public Shared Function SplitHtmlSections(htmlContent As String) As HtmlSplitResult
            Dim result As New HtmlSplitResult()

            Dim doc As New HtmlDocument()
            doc.LoadHtml(htmlContent)

            ' --- HEAD ---
            Dim headNode = doc.DocumentNode.SelectSingleNode("//head")
            result.HeadImports = If(headNode IsNot Nothing, headNode.InnerHtml, "")

            ' --- BODY ---
            Dim bodyNode = doc.DocumentNode.SelectSingleNode("//body")
            Dim scriptHtml As String = ""

            If bodyNode IsNot Nothing Then
                Dim scripts = bodyNode.SelectNodes(".//script")
                If scripts IsNot Nothing Then
                    For Each s In scripts
                        scriptHtml &= s.OuterHtml & Environment.NewLine
                        s.Remove()
                    Next
                End If

                result.BodyContent = bodyNode.InnerHtml
                result.FooterScripts = scriptHtml
            Else
                ' Nếu không có body
                result.BodyContent = htmlContent
                result.FooterScripts = ""
            End If

            Return result
        End Function
    End Class
    Public Class HtmlSplitResult
        Public Property HeadImports As String
        Public Property BodyContent As String
        Public Property FooterScripts As String
    End Class
End Namespace
