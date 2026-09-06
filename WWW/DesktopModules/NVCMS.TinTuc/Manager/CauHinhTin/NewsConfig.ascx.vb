Imports DotNetNuke.Modules.UrlManagement
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports NVCMS.Modules.Hethong
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Configurations
    Partial Class NewsConfigPC
        Inherits Entities.Modules.PortalModuleBase

        Private ReadOnly ctlNews As New NV_NewsController

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    BindSettings()
                    Dim sSettingRights = PortalController.GetPortalSetting("settingsTAGS", 0, Null.NullString)
                    If Not String.IsNullOrEmpty(sSettingRights) Then
                        If Not String.IsNullOrEmpty(sSettingRights) Then
                            Me.settingsTAGS.Value = sSettingRights
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindSettings()
            Dim ctlNewsCategories As New NewsSettingsController
            Dim arrsettingsNews As ArrayList
            arrsettingsNews = ctlNewsCategories.GetAllByType(CInt(drlSettings.SelectedValue), 20, 0)
            rptSettings.DataSource = arrsettingsNews
            rptSettings.DataBind()

        End Sub
        Protected Sub drlSettings_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles drlSettings.SelectedIndexChanged
            hdf_Value.Value = String.Empty
            If drlSettings.SelectedValue = "settingsTAGS" Then
                Me.select.Visible = False
                Me.settingsTAGS.Value = PortalController.GetPortalSetting(drlSettings.SelectedValue, PortalId, Null.NullString)
            Else
                BindSettings()
                Me.select.Visible = True
            End If
            BindSettings()
        End Sub
        Protected Sub lbtUpdateOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtUpdateOrder.Click
            'DotNetNuke.UI.Utilities.ClientAPI.RegisterStartUpScript(Me.Page, "showError", "<script>alert('" & hdf_Value.Value & "');</script>")
            Dim ctlNewsCategories As New NewsSettingsController
            ctlNewsCategories.Delete(drlSettings.SelectedValue, PortalId)
            Dim sSettings = hdf_Value.Value
            Dim strArr As String() = sSettings.Split(CType(",", Char))
            For i As Integer = 0 To strArr.Length - 1
                If IsNumeric(strArr(i)) Then
                    ctlNewsCategories.Insert(strArr(i), i, drlSettings.SelectedValue, 0)
                End If
            Next
            BindSettings()
            ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật cấu hình tin bài thành công!');</script>")
            WebsiteClearCache.ClearCapstoneViewCache()
        End Sub
        Protected Sub lbtUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
            Try
                PortalController.UpdatePortalSetting(PortalId, "settingsTAGS", settingsTAGS.Value, True)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifySuccess('Cập nhật thành công!','Cập nhật thẻ Tag thành công!');", True)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                DotNetNuke.UI.Utilities.ClientAPI.RegisterStartUpScript(Me.Page, "showError", "<script>notifyError('Cập nhật cấu hình thất bại');</script>")
            End Try
        End Sub



    End Class
End Namespace

