Imports System
Imports System.Web.UI.WebControls

Namespace NVCMS.Modules.School

    Public MustInherit Class SettingCustomeDisplay
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase


#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try


                If (Page.IsPostBack = False) Then
                    If IsNumeric(ModuleSettings("SchoolControlDisplayPageSetting")) Then
                        Me.txtDisplayNewsPage.Text = ModuleSettings("SchoolControlDisplayPageSetting")
                    End If
                    If CType(ModuleSettings("SchoolControlDisplayStyleSetting"), String) <> "" Then
                        Me.ddlDisplayStyle.Items.FindByValue(ModuleSettings("SchoolControlDisplayStyleSetting")).Selected = True
                    End If
                    If CType(ModuleSettings("SchoolControlDisplayImageSetting"), String) <> "" Then
                        Me.rbtDisplayImage.Items.FindByValue(ModuleSettings("SchoolControlDisplayImageSetting")).Selected = True
                    End If
                    If CType(ModuleSettings("SchoolControlDisplayMarqueeSetting"), String) <> "" Then
                        Me.rbtMarquee.Items.FindByValue(ModuleSettings("SchoolControlDisplayMarqueeSetting")).Selected = True
                    End If
                    If CType(ModuleSettings("NVNewsImageValueSetting"), String) <> "" Then
                        Me.txtImageWidth.Text = Split(ModuleSettings("NVNewsImageValueSetting"))(0)
                        Me.txtImageHeight.Text = Split(ModuleSettings("NVNewsImageValueSetting"))(1)
                    End If

                    If CType(ModuleSettings("SchoolControlDisplayNumberSetting"), String) <> "" Then
                        Me.txtDisplayRow.Text = Split(ModuleSettings("SchoolControlDisplayNumberSetting"))(0)
                        Me.txtDisplayCol.Text = Split(ModuleSettings("SchoolControlDisplayNumberSetting"))(1)
                    End If
                    If IsNumeric(TabModuleSettings("NVNewsDurationSetting")) Then
                        Me.txtDuration.Text = TabModuleSettings("NVNewsDurationSetting")
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
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController

                If IsNumeric(Me.txtDisplayNewsPage.Text) Then
                    objModules.UpdateModuleSetting(ModuleId, "SchoolControlDisplayPageSetting", Me.txtDisplayNewsPage.Text)
                End If
                objModules.UpdateModuleSetting(ModuleId, "SchoolControlDisplayStyleSetting", Me.ddlDisplayStyle.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "SchoolControlDisplayImageSetting", Me.rbtDisplayImage.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "SchoolControlDisplayMarqueeSetting", Me.rbtMarquee.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "NVNewsImageValueSetting", IIf(Trim(Me.txtImageWidth.Text) <> "", Trim(Me.txtImageWidth.Text), "0") & " " & IIf(Trim(Me.txtImageHeight.Text) <> "", Trim(Me.txtImageHeight.Text), "0"))
                objModules.UpdateModuleSetting(ModuleId, "SchoolControlDisplayNumberSetting", IIf(Trim(Me.txtDisplayRow.Text) <> "", Trim(Me.txtDisplayRow.Text), "0") & " " & IIf(Trim(Me.txtDisplayCol.Text) <> "", Trim(Me.txtDisplayCol.Text), "0"))
                If IsNumeric(Me.txtDuration.Text) Then
                    objModules.UpdateTabModuleSetting(TabModuleId, "NVNewsDurationSetting", Me.txtDuration.Text)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

    End Class
End Namespace