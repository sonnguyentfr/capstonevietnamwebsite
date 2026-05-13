Namespace NVCMS.Modules.Marketing
    Partial Class SendMailMain
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                'If Not IsPostBack Then

                Dim DynamicPage As String
                Select Case Request.Item("view")
                    Case "add", "edit"
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Edit.ascx")
                    Case "mail"
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Mail.ascx")
                    Case Else
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Viewer.ascx")
                End Select
                Dim objModule As Entities.Modules.PortalModuleBase = CType(Me.LoadControl(DynamicPage), DotNetNuke.Entities.Modules.PortalModuleBase)
                If Not objModule Is Nothing Then
                    objModule.ModuleConfiguration = Me.ModuleConfiguration
                    phDynamicPlaceHolder.Controls.Add(objModule)
                End If
                'End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

    End Class
End Namespace
