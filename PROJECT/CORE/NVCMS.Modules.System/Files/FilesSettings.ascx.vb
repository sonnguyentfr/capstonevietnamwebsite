Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports aejw.Network
Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.HeThong
Public Class FilesSettings
    Inherits Entities.Modules.PortalModuleBase
    Dim iPortalId = PortalId
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then

                txtAnhLuuTruVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingAnhLuuTruVirtual, iPortalId, Null.NullString)
                txtAnhLuuTruPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingAnhLuuTruPhysical, iPortalId, Null.NullString)
                txtFlashVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingFlashVirtual, iPortalId, Null.NullString)
                txtFlashPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingFlashPhysical, iPortalId, Null.NullString)
                txtMediaPathVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingMediaPathVirtual, iPortalId, Null.NullString)
                txtMediaPathPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingMediaPathPhysical, iPortalId, Null.NullString)
                txtMediaLuuTruVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingMediaLuuTruVirtual, iPortalId, Null.NullString)
                txtMediaLuuTruPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingMediaLuuTruPhysical, iPortalId, Null.NullString)

                txtDocumentLuuTruVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingDocumentVirtual, iPortalId, Null.NullString)
                txtDocumentLuuTruPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingDocumentPhysical, iPortalId, Null.NullString)


                txtSanPhamTruVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingSanphamVirtual, iPortalId, Null.NullString)
                txtSanPhaTruPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingSanphamPhysical, iPortalId, Null.NullString)
                txtBackupPathVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingBackupPathVirtual, iPortalId, Null.NullString)
                txtBackupPathPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingBackupPathPhysical, iPortalId, Null.NullString)
                txtBaiHatVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingBaiHatVirtual, iPortalId, Null.NullString)
                txtBaiHatPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingBaiHatPhysical, iPortalId, Null.NullString)
                txtVideoVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingVideoVirtual, iPortalId, Null.NullString)
                txtVideoPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingVideoPhysical, iPortalId, Null.NullString)
                txtSXCTVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingSXCTVirtual, iPortalId, Null.NullString)
                txtSXCTPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingSXCTPhysical, iPortalId, Null.NullString)
                txtFTPVirtual.Text = PortalController.GetPortalSetting(nvcmsBL.settingFTPVirtual, iPortalId, Null.NullString)
                txtFTPPhysical.Text = PortalController.GetPortalSetting(nvcmsBL.settingFTPPhysical, iPortalId, Null.NullString)
                txtDalet.Text = PortalController.GetPortalSetting(nvcmsBL.settingDalet, iPortalId, Null.NullString)
                chkDalet2XML.Checked = PortalController.GetPortalSetting(nvcmsBL.settingDalet2XML, iPortalId, False)
                txtNetia.Text = PortalController.GetPortalSetting(nvcmsBL.settingNetia, iPortalId, Null.NullString)
                chkNetia2XML.Checked = PortalController.GetPortalSetting(nvcmsBL.settingNetia2XML, iPortalId, False)
                txtMultiMediaCopyPath1.Text = PortalController.GetPortalSetting(nvcmsBL.settingMultiMediaCopyPath1, iPortalId, Null.NullString)
                txtMultiMediaCopyPath2.Text = PortalController.GetPortalSetting(nvcmsBL.settingMultiMediaCopyPath2, iPortalId, Null.NullString)
                txtMultiMediaCopyPath3.Text = PortalController.GetPortalSetting(nvcmsBL.settingMultiMediaCopyPath3, iPortalId, Null.NullString)
                txtAlertRequestDuration.Text = PortalController.GetPortalSetting(nvcmsBL.settingAlertRequestDuration, iPortalId, Null.NullString)
                txtAutoSaveRequestDuration.Text = PortalController.GetPortalSetting(nvcmsBL.settingAutoSaveRequestDuration, iPortalId, Null.NullString)
                txtDataRequestDuration.Text = PortalController.GetPortalSetting(nvcmsBL.settingDataRequestDuration, iPortalId, Null.NullString)

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

    Protected Sub lbtUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
        Try
            'CheckValidFolder()
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingAnhLuuTruVirtual, txtAnhLuuTruVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingAnhLuuTruPhysical, txtAnhLuuTruPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingFlashVirtual, txtFlashVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingFlashPhysical, txtFlashPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingMediaPathVirtual, txtMediaPathVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingMediaPathPhysical, txtMediaPathPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingMediaLuuTruVirtual, txtMediaLuuTruVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingMediaLuuTruPhysical, txtMediaLuuTruPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingDocumentVirtual, txtDocumentLuuTruVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingDocumentPhysical, txtDocumentLuuTruPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingSanphamVirtual, txtSanPhamTruVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingSanphamPhysical, txtSanPhaTruPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingBackupPathVirtual, txtBackupPathVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingBackupPathPhysical, txtBackupPathPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingBaiHatVirtual, txtBaiHatVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingBaiHatPhysical, txtBaiHatPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingVideoVirtual, txtVideoVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingVideoPhysical, txtVideoPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingSXCTVirtual, txtSXCTVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingSXCTPhysical, txtSXCTPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingFTPVirtual, txtFTPVirtual.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingFTPPhysical, txtFTPPhysical.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingDalet, txtDalet.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingDalet2XML, Convert.ToString(chkDalet2XML.Checked), True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingNetia, txtNetia.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingNetia2XML, Convert.ToString(chkNetia2XML.Checked), True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingMultiMediaCopyPath1, txtMultiMediaCopyPath1.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingMultiMediaCopyPath2, txtMultiMediaCopyPath2.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingMultiMediaCopyPath3, txtMultiMediaCopyPath3.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingAlertRequestDuration, txtAlertRequestDuration.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingAutoSaveRequestDuration, txtAutoSaveRequestDuration.Text, True)
            PortalController.UpdatePortalSetting(iPortalId, nvcmsBL.settingDataRequestDuration, txtDataRequestDuration.Text, True)

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifySuccess('Cập nhật Thành công!','Hệ thống Files lưu trữ đã được cập nhật thành công!');", True)
        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
            'ClientAPI.RegisterStartUpScript(Me.Page, "showError", "<script>notifyError('Cập nhật cấu hình thất bại');</script>")
        End Try
    End Sub

    Private Sub CheckValidFolder()
        Try
            If Not Directory.Exists(txtAnhLuuTruPhysical.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtAnhLuuTruPhysical.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtMediaPathPhysical.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtMediaPathPhysical.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtMediaLuuTruPhysical.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtMediaLuuTruPhysical.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtBackupPathPhysical.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtBackupPathPhysical.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtDalet.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtDalet.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtNetia.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtNetia.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtMultiMediaCopyPath1.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtMultiMediaCopyPath1.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtMultiMediaCopyPath2.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtMultiMediaCopyPath2.Text + " không tồn tại!');", True)
            End If
            If Not Directory.Exists(txtMultiMediaCopyPath3.Text) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Thư mục " + txtMultiMediaCopyPath3.Text + " không tồn tại!');", True)
            End If
        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi','Lỗi gì thì chưa rõ lắm!');", True)

        End Try
    End Sub
    Protected Sub lbtCancelTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtCancelTop.Click
        Try
            Response.Redirect(NavigateURL(), True)
        Catch exc As Exception    'Module failed to load
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub
    Protected Sub txtAnhLuuTruVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtAnhLuuTruVirtual.TextChanged
        txtAnhLuuTruPhysical.Text = Server.MapPath(txtAnhLuuTruVirtual.Text.Trim())
    End Sub
    Protected Sub txtBackupPathVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtBackupPathVirtual.TextChanged
        txtBackupPathPhysical.Text = Server.MapPath(txtBackupPathVirtual.Text.Trim())
    End Sub
    Protected Sub txtMediaLuuTruVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtMediaLuuTruVirtual.TextChanged
        txtMediaLuuTruPhysical.Text = Server.MapPath(txtMediaLuuTruVirtual.Text.Trim())
    End Sub

    Protected Sub txtDocumentLuuTruVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtDocumentLuuTruVirtual.TextChanged
        txtDocumentLuuTruPhysical.Text = Server.MapPath(txtDocumentLuuTruVirtual.Text.Trim())
    End Sub

    Protected Sub txtSanPhamTruVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtSanPhamTruVirtual.TextChanged
        txtSanPhaTruPhysical.Text = Server.MapPath(txtSanPhamTruVirtual.Text.Trim())
    End Sub

    Protected Sub txtCleanAudioVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtBaiHatVirtual.TextChanged
        txtBaiHatPhysical.Text = Server.MapPath(txtBaiHatVirtual.Text.Trim())
    End Sub
    Protected Sub txtAttachedFilesVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtVideoVirtual.TextChanged
        txtVideoPhysical.Text = Server.MapPath(txtVideoVirtual.Text.Trim())
    End Sub
    Protected Sub txtFlashVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtFlashVirtual.TextChanged
        txtFlashPhysical.Text = Server.MapPath(txtFlashVirtual.Text.Trim())
    End Sub
    Protected Sub txtsxctVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtSXCTVirtual.TextChanged
        txtSXCTPhysical.Text = Server.MapPath(txtSXCTVirtual.Text.Trim())
    End Sub
    Protected Sub txtftpVirtual_TextChanged(sender As Object, e As System.EventArgs) Handles txtFTPVirtual.TextChanged
        txtFTPPhysical.Text = Server.MapPath(txtFTPVirtual.Text.Trim())
    End Sub

    Private Sub MapDriver(driver As String, sharename As String, username As String, password As String)
        Dim oNetDrive As New NetworkDrive()
        Try
            'set propertys
            oNetDrive.Force = True
            oNetDrive.Persistent = True
            oNetDrive.LocalDrive = driver
            oNetDrive.ShareName = sharename
            oNetDrive.SaveCredentials = True
            'match call to options provided
            If String.IsNullOrEmpty(username) Then
                oNetDrive.MapDrive()
            Else
                oNetDrive.MapDrive(username, password)
            End If
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifySuccess('Cập nhật Thành công!','Map ổ: " + driver + " thành công!');", True)
        Catch err As Exception
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Cập nhật Lỗi!','Map ổ: " + driver + " thất bại!');", True)
        End Try
        oNetDrive = Nothing
    End Sub


End Class