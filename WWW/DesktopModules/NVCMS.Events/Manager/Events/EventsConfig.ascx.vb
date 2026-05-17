Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.Events

Namespace DesktopModules.TinTuc.Configurations
    Partial Class NewsConfig
        Inherits Entities.Modules.PortalModuleBase

        Private ReadOnly ctlNews As New EventsController

        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    BindSettings()

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
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub BindSettings()
            Dim sSettings = PortalController.GetPortalSetting("settingsEventsHOME", PortalId, Null.NullString)
            Dim arrsettingsNews As New ArrayList
            If Not String.IsNullOrEmpty(sSettings) Then
                Dim strArr As String() = sSettings.Split(CType(",", Char))
                For i As Integer = 0 To strArr.Length - 1
                    If IsNumeric(strArr(i)) Then
                        Dim obj As EventsInfo = ctlNews.Events_GetByID(CType(strArr(i), Integer), PortalId)
                        arrsettingsNews.Add(obj)
                    End If
                Next
            End If
            rptSettings.DataSource = arrsettingsNews
            rptSettings.DataBind()
        End Sub
        Protected Sub lbtUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
            Try
                PortalController.UpdatePortalSetting(PortalId, "settingsEventsHOME", hdf_Value.Value, True)

                BindSettings()

                ClientAPI.RegisterStartUpScript(Me.Page, "showSuccess", "<script>notifySuccess('Cập nhật Sự kiện thành công');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                ClientAPI.RegisterStartUpScript(Me.Page, "showError", "<script>notifyError('Cập nhật cấu hình thất bại');</script>")
            End Try
        End Sub

        Protected Sub lbtCancelTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtCancelTop.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

    End Class
End Namespace

