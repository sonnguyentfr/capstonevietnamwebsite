Imports System
Imports DotNetNuke

Namespace DesktopModules.TNReport
    Partial Class Main_NBReportDept
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim DynamicPage As String
                Select Case Request.Item("view")
                    Case "display"
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/viewdetail.ascx")
                    Case "edit"
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/ListNhuanButEdit.ascx")
                    Case "nhom"
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/ListNhuanButNhom.ascx")
                    Case Else
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/ListNhuanBut.ascx")
                End Select
                Dim objModule As Entities.Modules.PortalModuleBase = CType(Me.LoadControl(DynamicPage), DotNetNuke.Entities.Modules.PortalModuleBase)
                If Not objModule Is Nothing Then
                    objModule.ModuleConfiguration = Me.ModuleConfiguration
                    phDynamicPlaceHolder.Controls.Add(objModule)
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

    End Class
End Namespace