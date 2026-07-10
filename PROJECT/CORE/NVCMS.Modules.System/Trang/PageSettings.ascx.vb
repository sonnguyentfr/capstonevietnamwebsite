Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.HeThong

Public Class PageSettings
    Inherits Entities.Modules.PortalModuleBase
    Dim iPortalId = PortalId
    Public Property PhotoAbPath() As String
        Get
            If Not ViewState.Item("PhotoAbPath") Is Nothing Then
                Return CType(ViewState.Item("PhotoAbPath"), String)
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            ViewState.Add("PhotoAbPath", value)
        End Set
    End Property
    Public Property PhotoVirPath() As String
        Get
            If Not ViewState.Item("PhotoVirPath") Is Nothing Then
                Return CType(ViewState.Item("PhotoVirPath"), String)
            Else
                Return nvcmsBL.GetImagePath(True, PortalId, True)
            End If
        End Get
        Set(ByVal value As String)
            ViewState.Add("PhotoVirPath", value)
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                BindPage()
                'If (Not PortalController.GetPortalSetting(nvcmsBL.settingPage, iPortalId, Null.NullString) Is Nothing) Then
                '    If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPage, iPortalId, Null.NullString)) Then
                '        BindddlPageTinTuc(ddlPortalId.SelectedValue)
                '        ddlPageTinTuc.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPagePC, iPortalId, Null.NullString))
                '        BindddlPageTinAnh(ddlPortalId.SelectedValue)
                '        BindddlPageVideo(ddlPortalId.SelectedValue)

                '        '
                '        'BindddlPageTinAnh(ddlPageTinTuc.SelectedValue)
                '        'If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPagePCNewsDetail, iPortalId, Null.NullString)) Then
                '        '    ddlPageTinAnh.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPagePCNewsDetail, iPortalId, Null.NullString))
                '        'End If

                '    End If
                'End If

                BindPageEN()
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageEn, iPortalId, Null.NullString)) Then
                    ddlPortalIdEn.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageEn, iPortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageTinTuc, iPortalId, Null.NullString)) Then
                    BindddlPageTinTuc(ddlPortalId.SelectedValue)
                    ddlPageTinTuc.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageTinTuc, iPortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageTinAnh, iPortalId, Null.NullString)) Then
                    BindddlPageTinAnh(ddlPortalId.SelectedValue)
                    ddlPageTinAnh.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageTinAnh, iPortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageVideo, iPortalId, Null.NullString)) Then
                    BindddlPageVideo(ddlPortalId.SelectedValue)
                    ddlPageVideo.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageVideo, iPortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageEvents, iPortalId, Null.NullString)) Then
                    BindddlPageEvents(ddlPortalId.SelectedValue)
                    ddlPageEvents.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageEvents, iPortalId, Null.NullString))
                End If
                BindddlFolder()
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingFolderAttachId, iPortalId, Null.NullString)) Then
                    ddlfolder.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingFolderAttachId, iPortalId, Null.NullString))
                End If

                sitename.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitename, iPortalId, Null.NullString)
                siteweb.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteweb, iPortalId, Null.NullString)
                sitediachi.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitediachi, iPortalId, Null.NullString)
                siteemail.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteemail, iPortalId, Null.NullString)
                sitedienthoai.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai, iPortalId, Null.NullString)

                tenchinhnhanh1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitechinhnhanh1, iPortalId, Null.NullString)
                sitediachi1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitediachi1, iPortalId, Null.NullString)
                siteemail1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteemail1, iPortalId, Null.NullString)
                sitedthoigianlamviec1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagethoigianlamviec1, iPortalId, Null.NullString)
                sitedienthoai1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai1, iPortalId, Null.NullString)

                tenchinhnhanh2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitechinhnhanh2, iPortalId, Null.NullString)
                sitediachi2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitediachi2, iPortalId, Null.NullString)
                sitedthoigianlamviec2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagethoigianlamviec2, iPortalId, Null.NullString)
                siteemail2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteemail2, iPortalId, Null.NullString)
                sitedienthoai2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai2, iPortalId, Null.NullString)

                sitetomtat.Text = PortalController.GetPortalSetting(nvcmsBL.settingPagesitetomtat, iPortalId, Null.NullString)
                sitetag.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitetag, iPortalId, Null.NullString)
                sitefacebookpage.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitefacebookpage, iPortalId, Null.NullString)

                siteyoutube.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteyoutube, iPortalId, Null.NullString)

                siteLinkedin.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLinkedin, iPortalId, Null.NullString)
                siteInstagram.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteInstagram, iPortalId, Null.NullString)
                siteZalo.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteZalo, iPortalId, Null.NullString)
                siteTwitter.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteTwitter, iPortalId, Null.NullString)
                sitewhatsapp.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitewhatsapp, iPortalId, Null.NullString)
                siteSkype.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteSkype, iPortalId, Null.NullString)
                'code
                siteHeaderCode.Text = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteHeaderCode, iPortalId, Null.NullString)
                siteFooterCode.Text = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteFooterCode, iPortalId, Null.NullString)
                'Mail
                chkNhanEmail.Checked = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteNhanMail, PortalId, False)
                If chkNhanEmail.Checked = True Then
                    EmailLienhe.Visible = True
                    sitemaillist.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteNhanMailList, iPortalId, Null.NullString)
                End If
                'CAPCHA
                txtgoooglekey.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageGooogleCapcha, iPortalId, Null.NullString)
                txtgoooglekeysecret.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageGooogleCapchaSecret, iPortalId, Null.NullString)
                'CDN SERVER
                sitecdn.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageSiteCDN, iPortalId, Null.NullString)
                sitefileserver.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageSiteFilesServer, iPortalId, Null.NullString)
                '------------Logo
                hpflinkimage.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogo, iPortalId, Null.NullString)
                If Not PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogo, iPortalId, Null.NullString) Is Nothing Then
                    Me.dvPreviewlogo.Visible = True
                    Me.dvPreviewlogo.InnerHtml = "<img src=""" & PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogo, iPortalId, Null.NullString) & """  height='100px' />"
                End If
                '------------Logo Footer
                hpflinkimagefooter.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogofooter, iPortalId, Null.NullString)
                If Not PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogofooter, iPortalId, Null.NullString) Is Nothing Then
                    Me.dvPreviewlogofooter.Visible = True
                    Me.dvPreviewlogofooter.InnerHtml = "<img src=""" & PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogofooter, iPortalId, Null.NullString) & """  height='100px' />"
                End If
                'Email Server
                emailsmtp.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageMailSMTP, iPortalId, Null.NullString)
                emailtenhienthi.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageMailTenHienThi, iPortalId, Null.NullString)
                emailEmail.Value = Server.HtmlDecode(PortalController.GetPortalSetting(nvcmsBL.settingPageMailEmail, iPortalId, Null.NullString))
                emailmatkhau.Value = Server.HtmlDecode(PortalController.GetPortalSetting(nvcmsBL.settingPageMailMatkhau, iPortalId, Null.NullString))
                '------------------

                If Not Request.UrlReferrer Is Nothing Then
                    If Request.UrlReferrer.AbsoluteUri = Request.Url.AbsoluteUri Then
                        ViewState("UrlReferrer") = ""
                    Else
                        ViewState("UrlReferrer") = Convert.ToString(Request.UrlReferrer)
                    End If
                Else
                    ViewState("UrlReferrer") = ""
                End If
                PhotoAbPath = nvcmsBL.GetImagePath(False, PortalId, True)
            End If
            'End If
        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
        End Try
    End Sub

    Protected Sub lbtUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtUpdate2.Click
        Try
            'CheckValidFolder()
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitename, sitename.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteweb, siteweb.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitediachi, sitediachi.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteemail, siteemail.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitedienthoai, sitedienthoai.Value, True)

            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitechinhnhanh1, tenchinhnhanh1.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitediachi1, sitediachi1.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagethoigianlamviec1, sitedthoigianlamviec1.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteemail1, siteemail1.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitedienthoai1, sitedienthoai1.Value, True)

            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitechinhnhanh2, tenchinhnhanh2.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitediachi2, sitediachi2.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagethoigianlamviec2, sitedthoigianlamviec2.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteemail2, siteemail2.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitedienthoai2, sitedienthoai2.Value, True)
            'Mail
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteNhanMail, chkNhanEmail.Checked.ToString(), True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteNhanMailList, sitemaillist.Value, True)

            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitetomtat, sitetomtat.Text, True)

            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitetag, sitetag.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitefacebookpage, sitefacebookpage.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteyoutube, siteyoutube.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteLinkedin, siteLinkedin.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteInstagram, siteInstagram.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteZalo, siteZalo.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteTwitter, siteTwitter.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesitewhatsapp, sitewhatsapp.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteSkype, siteSkype.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteHeaderCode, siteHeaderCode.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteFooterCode, siteFooterCode.Text, True)


            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPage, ddlPortalId.SelectedValue, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageEn, ddlPortalIdEn.SelectedValue, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageTinTuc, ddlPageTinTuc.SelectedValue, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageTinAnh, ddlPageTinAnh.SelectedValue, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageVideo, ddlPageVideo.SelectedValue, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageEvents, ddlPageEvents.SelectedValue, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingFolderAttachId, ddlfolder.SelectedValue, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageGooogleCapcha, txtgoooglekey.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageGooogleCapchaSecret, txtgoooglekeysecret.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageSiteCDN, sitecdn.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageSiteFilesServer, sitefileserver.Value, True)
            'Mail server
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageMailSMTP, emailsmtp.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageMailTenHienThi, emailtenhienthi.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageMailEmail, emailEmail.Value, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPageMailMatkhau, emailmatkhau.Value, True)
            'Logo
            Dim strFileName As String = ""
            Dim strFileNamePath As String = ""
            If Me.filelogo.PostedFile.FileName <> "" Then
                strFileName = System.IO.Path.GetFileName(Me.filelogo.PostedFile.FileName)
                Me.filelogo.PostedFile.SaveAs(PhotoAbPath & "/" & strFileName)
                strFileNamePath = GetMediaPath(PhotoVirPath, Me.filelogo.PostedFile.FileName)
            Else
                strFileNamePath = hpflinkimage.Value
            End If
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteLogo, strFileNamePath, True)
            'Logo footer
            Dim strFileNamefooter As String = ""
            Dim strFileNamePathfooter As String = ""
            If Me.filelogofooter.PostedFile.FileName <> "" Then
                strFileNamefooter = System.IO.Path.GetFileName(Me.filelogofooter.PostedFile.FileName)
                Me.filelogofooter.PostedFile.SaveAs(PhotoAbPath & "/" & strFileNamefooter)
                strFileNamePathfooter = GetMediaPath(PhotoVirPath, Me.filelogofooter.PostedFile.FileName)
            Else
                strFileNamePathfooter = hpflinkimagefooter.Value
            End If
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingPagesiteLogofooter, strFileNamePathfooter, True)
            '----------------------

            ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật thành công!');</script>")
        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
            ClientAPI.RegisterStartUpScript(Me.Page, "showError", "<script>notifyError('Cập nhật cấu hình thất bại');</script>")
        End Try
    End Sub
    Protected Sub BindPage()
        Dim arrpage As New ArrayList
        arrpage = PortalController.Instance.GetPortals()

        ddlPortalId.DataSource = arrpage
        Me.ddlPortalId.DataTextField = "PortalName"
        Me.ddlPortalId.DataValueField = "PortalId"
        Me.ddlPortalId.DataBind()
        ddlPortalId.SelectedValue = PortalSettings.Current.PortalId
        BindddlPageTinAnh(PortalSettings.Current.PortalId)
        BindddlPageVideo(PortalSettings.Current.PortalId)
        BindddlPageEvents(PortalSettings.Current.PortalId)
        BindddlPageTinTuc(PortalSettings.Current.PortalId)
    End Sub

    Protected Sub BindPageEN()
        Dim arrpage As New ArrayList
        arrpage = PortalController.Instance.GetPortals()

        ddlPortalIdEn.DataSource = arrpage
        Me.ddlPortalIdEn.DataTextField = "PortalName"
        Me.ddlPortalIdEn.DataValueField = "PortalId"
        Me.ddlPortalIdEn.DataBind()
        Me.ddlPortalIdEn.Items.Insert(0, New ListItem("--Chọn Trang Tiếng Anh --", "-1"))
    End Sub
    Private Sub BindddlPageTinAnh(ByVal itemid As Integer)
        Dim arr = TabController.GetPortalTabs(itemid, -1, True, False)
        ddlPageTinAnh.DataSource = arr
        ddlPageTinAnh.DataTextField = "IndentedTabName"
        ddlPageTinAnh.DataValueField = "tabid"
        ddlPageTinAnh.DataBind()
        Me.ddlPageTinAnh.Items.Insert(0, New ListItem("--Chọn Trang--", "-1"))
    End Sub

    Private Sub BindddlPageVideo(ByVal itemid As Integer)
        Dim arr = TabController.GetPortalTabs(itemid, -1, True, False)
        ddlPageVideo.DataSource = arr
        ddlPageVideo.DataTextField = "IndentedTabName"
        ddlPageVideo.DataValueField = "tabid"
        ddlPageVideo.DataBind()
        Me.ddlPageVideo.Items.Insert(0, New ListItem("--Chọn Trang--", "-1"))
    End Sub
    Private Sub BindddlPageEvents(ByVal itemid As Integer)
        Dim arr = TabController.GetPortalTabs(itemid, -1, True, False)
        ddlPageEvents.DataSource = arr
        ddlPageEvents.DataTextField = "IndentedTabName"
        ddlPageEvents.DataValueField = "tabid"
        ddlPageEvents.DataBind()
        Me.ddlPageEvents.Items.Insert(0, New ListItem("--Chọn Trang--", "-1"))
    End Sub
    Private Sub BindddlFolder()
        Dim arr = FolderManager.Instance.GetFolders(PortalSettings.Current.PortalId)
        ddlfolder.DataSource = arr
        ddlfolder.DataTextField = "DisplayName"
        ddlfolder.DataValueField = "FolderID"
        ddlfolder.DataBind()
        Me.ddlfolder.Items.Insert(0, New ListItem("--Chọn Folder --", "-1"))
    End Sub
    Private Sub BindddlPageTinTuc(ByVal itemid As Integer)
        Dim arr = TabController.GetPortalTabs(itemid, -1, True, False)
        ddlPageTinTuc.DataSource = arr
        ddlPageTinTuc.DataTextField = "IndentedTabName"
        ddlPageTinTuc.DataValueField = "tabid"
        ddlPageTinTuc.DataBind()
        Me.ddlPageTinTuc.Items.Insert(0, New ListItem("--Chọn Trang--", "-1"))
    End Sub

    Protected Sub lbtCancelTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtCancelTop2.Click
        Try
            Response.Redirect(NavigateURL(), True)
        Catch exc As Exception    'Module failed to load
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

    Private Sub chkNhanEmail_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkNhanEmail.CheckedChanged
        If chkNhanEmail.Checked = True Then
            EmailLienhe.Visible = True
        Else
            EmailLienhe.Visible = False
            sitemaillist.Value = ""
        End If
    End Sub
    Private Function GetUploadPath(ByVal spath As String) As String
        Try
            Return spath.Substring(0, spath.LastIndexOf("/", System.StringComparison.Ordinal))
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Function GetMediaPath(ByVal foldername As String, ByVal radupload As String) As String
        If radupload.Length > 0 Then
            Return foldername & "/" & radupload
        Else
            Return ""
        End If
    End Function

End Class