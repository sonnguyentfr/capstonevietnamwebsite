Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Exceptions
Imports NVCMS.Modules.HeThong
Namespace NVCMS.Modules.Banner
    Partial Class List
        Inherits Entities.Modules.PortalModuleBase
        Public Property PortalCurrent() As Integer
            Get
                If Not ViewState("PortalCurrent") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("PortalCurrent"), String))
                    Catch ex As Exception
                        Return PortalId
                    End Try
                Else
                    ViewState.Add("PortalCurrent", PortalId)
                    Return PortalId
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("PortalCurrent") = Value.ToString
            End Set
        End Property
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    PortalCurrent = PortalId
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
            Dim ctlVideos As New BannerAdv_VitriController
            Me.drgViewData.DataSource = ctlVideos._Vitri_GetAll(PortalCurrent)
            Me.drgViewData.DataBind()
        End Sub
    End Class
End Namespace