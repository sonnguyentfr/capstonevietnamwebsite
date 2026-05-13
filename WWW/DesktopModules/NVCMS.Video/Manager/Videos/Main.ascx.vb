Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.Video
    Partial Class Main
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim DynamicPage As String
                Select Case Request.Item("view")
                    Case "add", "edit"
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/add.ascx")
                    Case "xemtruoc"
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Listen.ascx")
                    Case Else
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/View.ascx")
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