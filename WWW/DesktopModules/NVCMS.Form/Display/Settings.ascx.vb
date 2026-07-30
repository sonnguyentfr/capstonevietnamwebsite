Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.Form

    Public MustInherit Class SettingCustomeDisplay
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase
#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    If CType(ModuleSettings(BL.settingForm_MailOK), String) <> "" Then
                        Dim viewtype = ModuleSettings(BL.settingForm_MailOK)
                        If viewtype = "KhongGui" Then
                            ShowViewTypeCate(False)
                        End If
                    End If
                        If CType(ModuleSettings(BL.settingForm_MailNhan), String) <> "" Then
                        Me.txtemailnhan.Text = ModuleSettings(BL.settingForm_MailNhan)
                    End If
                    If CType(ModuleSettings(BL.settingForm_MailNhanTieude), String) <> "" Then
                        Me.txttieudemail.Text = ModuleSettings(BL.settingForm_MailNhanTieude)
                    End If
                    If CType(ModuleSettings(BL.settingForm_MailStyle), String) <> "" Then
                        Me.ddlDisplayStyle.Items.FindByValue(ModuleSettings(BL.settingForm_MailStyle)).Selected = True
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
                Dim nhanmail As String = ""
                If rd_KhongGui.Checked Then
                    nhanmail = "KhongGui"
                End If
                If rd_Gui.Checked Then
                    nhanmail = "Guimail"
                End If
                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                objModules.UpdateModuleSetting(ModuleId, BL.settingForm_MailOK, nhanmail)
                objModules.UpdateModuleSetting(ModuleId, BL.settingForm_MailNhan, Me.txtemailnhan.Text)
                objModules.UpdateModuleSetting(ModuleId, BL.settingForm_MailNhanTieude, Me.txttieudemail.Text)
                objModules.UpdateModuleSetting(ModuleId, BL.settingForm_MailStyle, Me.ddlDisplayStyle.SelectedValue)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub ShowViewTypeCate(ByVal viewCate As Boolean)
            rd_KhongGui.Checked = viewCate

            rd_Gui.Checked = Not viewCate
            tr_nhanmail.Visible = Not viewCate
            tr_nhanmail2.Visible = Not viewCate
        End Sub
        Protected Sub rdGetType_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
            If rd_Gui.Checked Then
                tr_nhanmail.Visible = True
                tr_nhanmail2.Visible = True
            Else
                tr_nhanmail.Visible = False
                tr_nhanmail2.Visible = False
            End If
        End Sub
#End Region


    End Class

End Namespace
