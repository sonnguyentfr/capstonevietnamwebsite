Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Control.Home
    Partial Class HotHome
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Public CategoryId As Integer
        Public Sub New()
        End Sub
        Public Sub New(_CategoryId As Integer)
            CategoryId = _CategoryId
        End Sub
        Property SubtractIds() As String
            Get
                If Not Session.Item("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString()) Is Nothing Then
                    Return CType(Session.Item("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString()), String)
                Else
                    Session.Add("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString(), "")
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                Session.Item("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString()) = value.ToString
            End Set
        End Property
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    Dim ctlNews As New NV_NewsController
                    Dim ctl As New NewsSettingsController
                    Dim arrHots As ArrayList = ctl.GetAllByType(3, 10, 0)
                    If Not arrHots Is Nothing AndAlso arrHots.Count > 0 Then
                        Me.rptHot.DataSource = arrHots
                        Me.rptHot.DataBind()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
    End Class
End Namespace

