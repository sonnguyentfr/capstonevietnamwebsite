Imports System
Imports System.Activities.Expressions
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
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

        Private ReadOnly TOKEN_LISTITEM As String = "LIST_ITEM"

        Private ReadOnly TOKEN_LISTMEDIA As String = "LISTMEDIA"
        Private ReadOnly TOKEN_LISTMEDIA2 As String = "LISTMEDIA2"
        Private ReadOnly TOKEN_LISTMEDIA3 As String = "LISTMEDIA3"
        Private ReadOnly TOKEN_MEDIANAME As String = "[MEDIANAME]"
        Private ReadOnly TOKEN_MEDIALINK As String = "[MEDIALINK]"
        Private ReadOnly TOKEN_MEDIAID As String = "[MEDIAID]"
        Private ReadOnly TOKEN_MEDIADESCREPTION As String = "[MEDIADESCREPTION]"

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
                    Dim objLadingPage As LadingPage_Info = _LadingPage_Controller._GetByID(setting_trangLadingPage, PortalId)
                    ltContent.Text = setting_trangLadingPage.ToString()
                    If Not objLadingPage Is Nothing Then
                        With objLadingPage
                            If setting_showtitle = True Then
                                scontent = "<h2>" & .TrangDanhMuc & "</h2>"
                            End If
                            If setting_showsubpage = True Then
                                Dim arrsubpage As New ArrayList
                                arrsubpage = _LadingPage_Controller._GetAllByParentId(.id, PortalId)
                                If Not arrsubpage Is Nothing AndAlso arrsubpage.Count > 0 Then
                                    Dim sTemplate_list As String = If(sTemplate.Contains(TOKEN_LISTITEM), TrimToken(sTemplate, TOKEN_LISTITEM), "")
                                    Dim sListItem As String = ""

                                    For i As Integer = 0 To (arrsubpage.Count - 1)
                                        If i >= arrsubpage.Count Then
                                            Exit For
                                        End If
                                        Dim objmedia As LadingPage_Info = DirectCast(arrsubpage(i), LadingPage_Info)
                                        sListItem += ToHTMLLadingPage(sTemplate_list, objmedia, (i + 1))
                                    Next
                                    If sListItem <> "" Then
                                        sTemplate = sTemplate.Replace(sTemplate_list, sListItem).Replace((Convert.ToString("[") & TOKEN_LISTITEM) + "]", "").Replace((Convert.ToString("[/") & TOKEN_LISTITEM) + "]", "")
                                    End If
                                End If
                            End If
                            scontent += Server.HtmlDecode(.Noidung)
                            'Lay danh sách ảnh
                            Dim arrMedia As New ArrayList
                            arrMedia = _Media_Controller._GetAll(.id)
                            If Not arrMedia Is Nothing AndAlso arrMedia.Count > 0 Then
                                Dim sTemplate_top As String = If(sTemplate.Contains(TOKEN_LISTMEDIA), TrimToken(sTemplate, TOKEN_LISTMEDIA), "")
                                Dim sTemplate_top2 As String = If(sTemplate.Contains(TOKEN_LISTMEDIA2), TrimToken(sTemplate, TOKEN_LISTMEDIA2), "")
                                Dim sTemplate_top3 As String = If(sTemplate.Contains(TOKEN_LISTMEDIA3), TrimToken(sTemplate, TOKEN_LISTMEDIA3), "")
                                Dim sListMedia As String = ""
                                Dim sListMedia2 As String = ""
                                Dim sListMedia3 As String = ""
                                For i As Integer = 0 To (arrMedia.Count - 1)
                                    If i >= arrMedia.Count Then
                                        Exit For
                                    End If
                                    Dim objmedia As Media_Info = DirectCast(arrMedia(i), Media_Info)
                                    sListMedia += ToHTML(sTemplate_top, objmedia, (i + 1))
                                    sListMedia3 += ToHTML(sTemplate_top3, objmedia, (i + 1))
                                Next
                                If sTemplate_top <> "" Then
                                    sTemplate = sTemplate.Replace(sTemplate_top, sListMedia).Replace((Convert.ToString("[") & TOKEN_LISTMEDIA) + "]", "").Replace((Convert.ToString("[/") & TOKEN_LISTMEDIA) + "]", "")
                                End If
                                If sTemplate_top2 <> "" Then
                                    sTemplate = sTemplate.Replace(sTemplate_top2, sListMedia).Replace((Convert.ToString("[") & TOKEN_LISTMEDIA2) + "]", "").Replace((Convert.ToString("[/") & TOKEN_LISTMEDIA2) + "]", "")
                                End If
                                If sTemplate_top3 <> "" Then
                                    sTemplate = sTemplate.Replace(sTemplate_top3, sListMedia3).Replace((Convert.ToString("[") & TOKEN_LISTMEDIA3) + "]", "").Replace((Convert.ToString("[/") & TOKEN_LISTMEDIA3) + "]", "")
                                End If
                            End If
                            Dim url = Ultis.FormatLinkLadingPage(TabId, .id, .TrangDanhMuc)
                            sTemplate = sTemplate.Replace(TOKEN_NAME, .TrangDanhMuc).Replace(TOKEN_NAMEPHU, .Tieudephu).Replace(TOKEN_URL, url).Replace(TOKEN_NAMETITLE, ReplaceChuoi.titlenews(.TrangDanhMuc)).Replace(TOKEN_IMAGE, .ImagePath.Replace("/DATA", BL.filesDomain)).Replace(TOKEN_DESCREPTION, .tomtat).Replace(TOKEN_CONTENT, Server.HtmlDecode(scontent))
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
        Private Function ToHTML(sTemplate As String, objmedia As Media_Info, position As Integer) As String
            Dim Image As String = "#"
            Dim title As String = ""
            Dim descreption As String = ""
            title = objmedia.Title
            Dim sid As Integer = objmedia.id
            descreption = objmedia.Descreption
            Image = objmedia.MediaLnk.Replace("/DATA", "/DATA")
            sTemplate = sTemplate.Replace(TOKEN_MEDIANAME, title).Replace(TOKEN_MEDIALINK, Image).Replace(TOKEN_MEDIAID, sid.ToString()).Replace(TOKEN_MEDIADESCREPTION, descreption)
            Return sTemplate
        End Function
        Private Function ToHTMLLadingPage(sTemplate As String, objmedia As LadingPage_Info, position As Integer) As String
            Dim Image As String = "#"
            Dim title As String = ""
            Dim Stitle As String = ""
            Dim scontent As String = ""
            Dim sid As Integer = 0
            Dim salt As String = ""
            Dim surl As String = Ultis.FormatLinkLadingPage(TabId, objmedia.id, objmedia.TrangDanhMuc)
            Dim descreption As String = ""
            If Not objmedia Is Nothing Then
                With objmedia
                    title = objmedia.TrangDanhMuc
                    Stitle = objmedia.Tieudephu
                    salt = ReplaceChuoi.titlenews(.TrangDanhMuc)
                    sid = objmedia.id
                    surl = Ultis.FormatLinkLadingPage(TabId, objmedia.id, objmedia.TrangDanhMuc)
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
                If Not Null.IsNull(ModuleConfiguration.ModuleSettings("TrangLadingPage_Template")) Then
                    setting_details_template = ModuleConfiguration.ModuleSettings("TrangLadingPage_Template").ToString()
                Else
                    isNull = True
                End If
                If isNull Then Return isNull
                '-----
                If Not Null.IsNull(ModuleConfiguration.ModuleSettings("TrangLadingPage_Id")) Then
                    setting_trangLadingPage = Convert.ToInt32(ModuleConfiguration.ModuleSettings("TrangLadingPage_Id"))
                Else
                    isNull = True
                End If
                If isNull Then Return isNull
                '-----
                If Not Null.IsNull(ModuleConfiguration.ModuleSettings("TrangLadingPage_ShowSubPage")) Then
                    setting_showsubpage = Convert.ToBoolean(ModuleConfiguration.ModuleSettings("TrangLadingPage_ShowSubPage"))
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
    End Class
End Namespace
