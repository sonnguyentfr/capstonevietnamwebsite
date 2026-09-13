Namespace NVCMS.Modules.Marketing
    Partial Class ZaloSendMain
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim dynamicPage As String = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Viewer.ascx")
                Dim objModule As Entities.Modules.PortalModuleBase = CType(Me.LoadControl(dynamicPage), DotNetNuke.Entities.Modules.PortalModuleBase)
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