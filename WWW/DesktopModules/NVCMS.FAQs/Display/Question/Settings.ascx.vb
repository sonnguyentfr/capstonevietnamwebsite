'------------------------------
' Hien thi 5 tin moi nhat
'------------------------------
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Entities.Modules

Namespace BUH.Modules.FAQs

    Public MustInherit Class SettingCustomeDisplaySpecial
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase
        Dim templateController As New TemplateController()
        Dim moduleController As New ModuleController()

#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    BindTemplate()
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings("FAQs_StyleSettings")) Then
                        Me.dropTemplate.SelectedValue = ModuleConfiguration.ModuleSettings("FAQs_StyleSettings").ToString()
                    End If
                   
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[cnurse]	10/22/2004	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        ''' 
        Private Sub BindTemplate()
            Dim arrTemplate As New ArrayList
            arrTemplate = templateController._GetAll(0)
            dropTemplate.DataSource = arrTemplate
            dropTemplate.DataTextField = "TemplateName"
            dropTemplate.DataValueField = "FilePath"
            dropTemplate.DataBind()
        End Sub
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                objModules.UpdateModuleSetting(ModuleId, "FAQs_StyleSettings", Me.dropTemplate.SelectedValue)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region


    End Class

End Namespace
