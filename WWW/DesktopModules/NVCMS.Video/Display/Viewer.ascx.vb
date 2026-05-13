Imports System
Imports System.Web.UI
Imports DotNetNuke
Imports DotNetNuke.Entities.Modules

Namespace NVCMS.Modules.Video

    Public MustInherit Class Viewer
        Inherits PortalModuleBase

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim sUrl As String = Request.RawUrl
                Dim sId2 As String = Ultis.GetRequestId2(sUrl)
                Dim sId As String = Ultis.GetRequestName(sId2)

                If sId = "d" Then 'Index
                    Dim o_control As New PortalModuleBase
                    o_control = CType(Page.LoadControl("~/DesktopModules/NVCMS.Video/Display/Detail.ascx"), PortalModuleBase)
                    o_control.ModuleConfiguration = ModuleConfiguration
                    Me.plhNews.Controls.Add(o_control)
                Else
                    Dim o_control As New PortalModuleBase
                    o_control = CType(Page.LoadControl("~/DesktopModules/NVCMS.Video/Display/Index.ascx"), PortalModuleBase)
                    o_control.ModuleConfiguration = ModuleConfiguration
                    Me.plhNews.Controls.Add(o_control)
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace