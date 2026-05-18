Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Exceptions
Namespace NVCMS.Modules.Banner
    Partial Class ListTemplate
        Inherits Entities.Modules.PortalModuleBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    BindGridData()
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
        Private Sub lbtAddBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAddBottom.Click, lbtAddTop.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub BindGridData()
            Dim ctlVideos As New TemplateController
            Me.drgViewData.DataSource = ctlVideos._GetAll(PortalId)
            Me.drgViewData.DataBind()
        End Sub
    End Class
End Namespace