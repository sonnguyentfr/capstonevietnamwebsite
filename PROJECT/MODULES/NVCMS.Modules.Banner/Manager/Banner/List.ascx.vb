Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Exceptions
Imports NVCMS.Modules.HeThong
Namespace NVCMS.Modules.Banner
    Partial Class ListBanner
        Inherits Entities.Modules.PortalModuleBase
#Region "Controls"
        Public Property key() As String
            Get
                If Not ViewState.Item("key") Is Nothing Then
                    Return ViewState.Item("key")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("KeySearch", value)
            End Set
        End Property
        Public Property vitri() As Integer
            Get
                If Not ViewState.Item("vitri") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("vitri")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("vitri", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("vitri") = Value.ToString
            End Set
        End Property
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
#End Region
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    PortalCurrent = PortalId
                    BindVitri()
                    If IsNumeric(Request.Item("vitri")) Then
                        vitri = Request.Item("vitri")
                        ddlCategory.SelectedValue = vitri
                    End If
                    BindGridData()
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
        Private Sub lbtFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtFind.Click, ddlCategory.SelectedIndexChanged
            Response.Redirect(NavigateURL(TabId) & "?vitri=" & ddlCategory.SelectedValue)
        End Sub
        Private Sub lbtAddBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAddBottom.Click, lbtAddTop.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub BindGridData()
            Dim ctlVideos As New BannerAdvController
            If (vitri > 0) Then
                Me.drgViewData.DataSource = ctlVideos.GetAllVitri(PortalCurrent, vitri)
                Me.drgViewData.DataBind()
            Else
                Me.drgViewData.DataSource = ctlVideos.GetAll(PortalCurrent)
                Me.drgViewData.DataBind()
            End If

        End Sub
        Private Sub BindVitri()
            Dim ctlVideos As New BannerAdv_VitriController
            Me.ddlCategory.DataSource = ctlVideos._Vitri_GetAll(PortalCurrent)
            Me.ddlCategory.DataTextField = "Title"
            Me.ddlCategory.DataValueField = "id"
            Me.ddlCategory.DataBind()
            Me.ddlCategory.Items.Insert(0, New ListItem("--Chọn vị trí--", "0"))
        End Sub
    End Class
End Namespace