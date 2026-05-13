Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.UI.Utilities

Public Class PageSettings
    Inherits Entities.Modules.PortalModuleBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                BindPage()
                'If (Not PortalController.GetPortalSetting(nvcmsBL.settingPage, PortalId, Null.NullString) Is Nothing) Then
                '    If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPage, PortalId, Null.NullString)) Then
                '        BindddlPageTinTuc(ddlPortalId.SelectedValue)
                '        ddlPageTinTuc.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPagePC, PortalId, Null.NullString))
                '        BindddlPageTinAnh(ddlPortalId.SelectedValue)
                '        BindddlPageVideo(ddlPortalId.SelectedValue)

                '        '
                '        'BindddlPageTinAnh(ddlPageTinTuc.SelectedValue)
                '        'If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPagePCNewsDetail, PortalId, Null.NullString)) Then
                '        '    ddlPageTinAnh.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPagePCNewsDetail, PortalId, Null.NullString))
                '        'End If

                '    End If
                'End If
                BindPageEN()
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageEn, PortalId, Null.NullString)) Then
                    ddlPortalIdEn.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageEn, PortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageTinTuc, PortalId, Null.NullString)) Then
                    BindddlPageTinTuc(ddlPortalId.SelectedValue)
                    ddlPageTinTuc.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageTinTuc, PortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageTinAnh, PortalId, Null.NullString)) Then
                    BindddlPageTinAnh(ddlPortalId.SelectedValue)
                    ddlPageTinAnh.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageTinAnh, PortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageVideo, PortalId, Null.NullString)) Then
                    BindddlPageVideo(ddlPortalId.SelectedValue)
                    ddlPageVideo.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageVideo, PortalId, Null.NullString))
                End If
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingPageEvents, PortalId, Null.NullString)) Then
                    BindddlPageEvents(ddlPortalId.SelectedValue)
                    ddlPageEvents.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingPageEvents, PortalId, Null.NullString))
                End If
                BindddlFolder()
                If IsNumeric(PortalController.GetPortalSetting(nvcmsBL.settingFolderAttachId, PortalId, Null.NullString)) Then
                    ddlfolder.SelectedValue = CInt(PortalController.GetPortalSetting(nvcmsBL.settingFolderAttachId, PortalId, Null.NullString))
                End If

                sitename.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitename, PortalId, Null.NullString)
                siteweb.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteweb, PortalId, Null.NullString)
                sitediachi.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitediachi, PortalId, Null.NullString)
                siteemail.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteemail, PortalId, Null.NullString)
                sitedienthoai.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai, PortalId, Null.NullString)

                tenchinhnhanh1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitechinhnhanh1, PortalId, Null.NullString)
                sitediachi1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitediachi1, PortalId, Null.NullString)
                siteemail1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteemail1, PortalId, Null.NullString)
                sitedienthoai1.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai1, PortalId, Null.NullString)

                tenchinhnhanh2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitechinhnhanh2, PortalId, Null.NullString)
                sitediachi2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitediachi2, PortalId, Null.NullString)
                siteemail2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteemail2, PortalId, Null.NullString)
                sitedienthoai2.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai2, PortalId, Null.NullString)

                sitetomtat.Text = PortalController.GetPortalSetting(nvcmsBL.settingPagesitetomtat, PortalId, Null.NullString)
                sitetag.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitetag, PortalId, Null.NullString)
                sitefacebookpage.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitefacebookpage, PortalId, Null.NullString)

                siteyoutube.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteyoutube, PortalId, Null.NullString)

                siteLinkedin.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLinkedin, PortalId, Null.NullString)
                siteInstagram.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteInstagram, PortalId, Null.NullString)
                siteZalo.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteZalo, PortalId, Null.NullString)
                siteTwitter.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteTwitter, PortalId, Null.NullString)
                sitewhatsapp.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesitewhatsapp, PortalId, Null.NullString)
                siteSkype.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteSkype, PortalId, Null.NullString)
                'code
                siteHeaderCode.Text = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteHeaderCode, PortalId, Null.NullString)
                siteFooterCode.Text = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteFooterCode, PortalId, Null.NullString)
                'Mail
                chkNhanEmail.Checked = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteNhanMail, PortalId, False)
                If chkNhanEmail.Checked = True Then
                    EmailLienhe.Visible = True
                    sitemaillist.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteNhanMailList, PortalId, Null.NullString)
                End If
                'CAPCHA
                txtgoooglekey.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageGooogleCapcha, PortalId, Null.NullString)
                txtgoooglekeysecret.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageGooogleCapchaSecret, PortalId, Null.NullString)
                'CDN SERVER
                sitecdn.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageSiteCDN, PortalId, Null.NullString)
                sitefileserver.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageSiteFilesServer, PortalId, Null.NullString)
                '------------Logo
                hpflinkimage.Value = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogo, PortalId, Null.NullString)
                If Not PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogo, PortalId, Null.NullString) Is Nothing Then
                    Me.dvPreviewlogo.Visible = True
                    Me.dvPreviewlogo.InnerHtml = "<img src=""" & PortalController.GetPortalSetting(nvcmsBL.settingPagesiteLogo, PortalId, Null.NullString) & """  height='100px' />"
                End If
                'Email Server
                emailsmtp.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageMailSMTP, PortalId, Null.NullString)
                emailtenhienthi.Value = PortalController.GetPortalSetting(nvcmsBL.settingPageMailTenHienThi, PortalId, Null.NullString)
                emailEmail.Value = Server.HtmlDecode(PortalController.GetPortalSetting(nvcmsBL.settingPageMailEmail, PortalId, Null.NullString))
                emailmatkhau.Value = Server.HtmlDecode(PortalController.GetPortalSetting(nvcmsBL.settingPageMailMatkhau, PortalId, Null.NullString))
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
            End If
            'End If
        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
        End Try
    End Sub

    Protected Sub lbtUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtUpdate2.Click
        Try
            'CheckValidFolder()
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitename, sitename.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteweb, siteweb.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitediachi, sitediachi.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteemail, siteemail.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitedienthoai, sitedienthoai.Value, True)

            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitechinhnhanh1, tenchinhnhanh1.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitediachi1, sitediachi1.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteemail1, siteemail1.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitedienthoai1, sitedienthoai1.Value, True)

            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitechinhnhanh2, tenchinhnhanh2.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitediachi2, sitediachi2.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteemail2, siteemail2.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitedienthoai2, sitedienthoai2.Value, True)
            'Mail
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteNhanMail, chkNhanEmail.Checked.ToString(), True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteNhanMailList, sitemaillist.Value, True)

            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitetomtat, sitetomtat.Text, True)

            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitetag, sitetag.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitefacebookpage, sitefacebookpage.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteyoutube, siteyoutube.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteLinkedin, siteLinkedin.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteInstagram, siteInstagram.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteZalo, siteZalo.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteTwitter, siteTwitter.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesitewhatsapp, sitewhatsapp.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteSkype, siteSkype.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteHeaderCode, siteHeaderCode.Text, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteFooterCode, siteFooterCode.Text, True)


            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPage, ddlPortalId.SelectedValue, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageEn, ddlPortalIdEn.SelectedValue, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageTinTuc, ddlPageTinTuc.SelectedValue, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageTinAnh, ddlPageTinAnh.SelectedValue, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageVideo, ddlPageVideo.SelectedValue, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageEvents, ddlPageEvents.SelectedValue, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingFolderAttachId, ddlfolder.SelectedValue, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageGooogleCapcha, txtgoooglekey.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageGooogleCapchaSecret, txtgoooglekeysecret.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageSiteCDN, sitecdn.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageSiteFilesServer, sitefileserver.Value, True)
            'Mail server
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageMailSMTP, emailsmtp.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageMailTenHienThi, emailtenhienthi.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageMailEmail, emailEmail.Value, True)
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPageMailMatkhau, emailmatkhau.Value, True)
            'Logo
            Dim strFileName As String = ""
            Dim strFileNamePath As String = ""
            If Me.filelogo.PostedFile.FileName <> "" Then
                strFileName = System.IO.Path.GetFileName(Me.filelogo.PostedFile.FileName)
                Me.filelogo.PostedFile.SaveAs(PortalSettings.HomeDirectoryMapPath & "/" & strFileName)
                strFileNamePath = String.Concat(PortalSettings.HomeDirectory, Me.filelogo.PostedFile.FileName)
            Else
                strFileNamePath = hpflinkimage.Value
            End If
            PortalController.UpdatePortalSetting(PortalId, nvcmsBL.settingPagesiteLogo, strFileNamePath, True)
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


End Class