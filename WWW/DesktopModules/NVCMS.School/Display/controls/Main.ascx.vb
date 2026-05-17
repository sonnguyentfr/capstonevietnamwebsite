Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.School
    Partial Class MainCustomeDisplay
        Inherits Entities.Modules.PortalModuleBase

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                'If Not IsPostBack Then
                Dim DynamicPage As String
                If Settings("SchoolControlDisplayImageSetting") = "NoImage" Then
                    If Settings("SchoolControlDisplayStyleSetting") <> "" Then
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/CapHome/" & Settings("SchoolControlDisplayStyleSetting"))
                    Else
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/CapHome/CapV2_HomeTruongNoiBat.ascx")
                    End If
                Else
                    If Settings("SchoolControlDisplayStyleSetting") <> "" Then
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/CapHome/" & Settings("SchoolControlDisplayStyleSetting"))
                    Else
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/CapHome/CapV2_HomeTruongNoiBat.ascx")
                    End If
                End If

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
