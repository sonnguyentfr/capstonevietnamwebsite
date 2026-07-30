Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports NVCMS.Modules.EventsWebsite
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.FormLandingPage

    Public MustInherit Class SettingCustomeDisplay
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase
#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    hdf_moduleid.Value = ModuleId.ToString()
                    BindSuKienSapDienRa()
                    If CType(ModuleSettings("FormOptionDisPlay_Background"), String) <> "" Then
                        Me.ImgBackground.ImageUrl = ModuleSettings("FormOptionDisPlay_Background")
                    End If
                    If CType(ModuleSettings("FormOptionDisPlay_Title"), String) <> "" Then
                        Me.txttiel.Text = ModuleSettings("FormOptionDisPlay_Title")
                    End If
                    If CType(ModuleSettings("FormOptionDisPlay_Noidunggioithieu"), String) <> "" Then
                        Me.txtNoiDung.Text = ModuleSettings("FormOptionDisPlay_Noidunggioithieu")
                    End If


                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_EventCat")) Then
                        Me.ddlSuken.SelectedValue = ModuleSettings("FormOptionDisPlay_EventCat").ToString()
                        Dim iddlSuken = ModuleSettings("FormOptionDisPlay_EventCat").ToString()
                        If CType(iddlSuken, Integer) > 0 Then
                            BindDiaDiem(iddlSuken)
                            ddlSukendiadiem.Enabled = True
                            If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_EventDiaDiem")) Then
                                Me.ddlSukendiadiem.SelectedValue = ModuleSettings("FormOptionDisPlay_EventDiaDiem").ToString()
                            End If
                        End If
                    End If


                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_Hovaten")) Then
                        Dim schkhovaten = ModuleSettings("FormOptionDisPlay_Hovaten").ToString()
                        Me.chkhovaten.Checked = Convert.ToBoolean(schkhovaten)
                    End If
                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_Sodienthoai")) Then
                        Dim schksodienthoai = ModuleSettings("FormOptionDisPlay_Sodienthoai").ToString()
                        Me.chkDienthoai.Checked = Convert.ToBoolean(schksodienthoai)
                    End If
                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_Email")) Then
                        Dim schkemail = ModuleSettings("FormOptionDisPlay_Email").ToString()
                        Me.chkEMail.Checked = Convert.ToBoolean(schkemail)
                    End If
                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_Ngaysinh")) Then
                        Dim schkngaysinh = ModuleSettings("FormOptionDisPlay_Ngaysinh").ToString()
                        Me.chkNgaySinh.Checked = Convert.ToBoolean(schkngaysinh)
                    End If
                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_DiachiTinh")) Then
                        Dim schkdiachitinh = ModuleSettings("FormOptionDisPlay_DiachiTinh").ToString()
                        Me.chkTinh.Checked = Convert.ToBoolean(schkdiachitinh)
                    End If
                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_Vaitro")) Then
                        Dim svaitro = ModuleSettings("FormOptionDisPlay_Vaitro").ToString()
                        Me.chkVaitro.Checked = Convert.ToBoolean(svaitro)
                    End If
                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_Yeucautuvan")) Then
                        Dim schkYecauTuvan = ModuleSettings("FormOptionDisPlay_Yeucautuvan").ToString()
                        Me.chkYecauTuvan.Checked = Convert.ToBoolean(schkYecauTuvan)
                    End If

                    If Not Null.IsNull(ModuleSettings("FormOptionDisPlay_EB5")) Then
                        Dim schkEbfive = ModuleSettings("FormOptionDisPlay_EB5").ToString()
                        Me.chkEbfive.Checked = Convert.ToBoolean(schkEbfive)
                    End If
                    If CType(ModuleSettings("FormOptionDisPlay_GuiMail"), String) <> "" Then
                        Dim viewtype = ModuleSettings("FormOptionDisPlay_GuiMail")
                        If viewtype = "KhongGui" Then
                            ShowViewTypeCate(False)
                        End If
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub BindSuKienSapDienRa()
            Dim ctlNewsCategory As New EventsWebsite_CatController
            'Online
            Dim arrtoponline As ArrayList = ctlNewsCategory.Events_Cat_GetAllShowOnline(50)

            Me.ddlSuken.DataSource = arrtoponline
            Me.ddlSuken.DataTextField = "CatName"
            Me.ddlSuken.DataValueField = "id"
            Me.ddlSuken.DataBind()
            ddlSuken.Items.Insert(0, New ListItem("- Chọn sự kiện -", "0"))
        End Sub
        Private Sub BindDiaDiem(idtinh As Integer)
            Dim arrNewsCategories As New ArrayList
            Dim ctlNewsCategory As New EventsWebsiteController
            arrNewsCategories = ctlNewsCategory.Events_GetAllShowByCat(idtinh, 50)
            Me.ddlSukendiadiem.DataSource = arrNewsCategories
            Me.ddlSukendiadiem.DataTextField = "Title"
            Me.ddlSukendiadiem.DataValueField = "id"
            Me.ddlSukendiadiem.DataBind()
            Me.ddlSukendiadiem.Items.Insert(0, New ListItem("----", 0))
        End Sub
        Protected Sub ddlSuken_SelectIndexChange(sender As Object, e As EventArgs)
            ddlSukendiadiem.Enabled = False
            ddlSukendiadiem.Items.Clear()
            Dim stateId As Integer = Integer.Parse(ddlSuken.SelectedItem.Value)
            If stateId > 0 Then
                BindDiaDiem(ddlSuken.SelectedItem.Value)
                ddlSukendiadiem.Enabled = True
            End If

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
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Title", Me.txttiel.Text)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_EventCat", Me.ddlSuken.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_EventDiaDiem", Me.ddlSukendiadiem.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Hovaten", Me.chkhovaten.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Sodienthoai", Me.chkDienthoai.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Email", Me.chkEMail.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Ngaysinh", Me.chkNgaySinh.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_DiachiTinh", Me.chkTinh.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Vaitro", Me.chkVaitro.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Yeucautuvan", Me.chkYecauTuvan.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_EB5", Me.chkEbfive.Checked)
                objModules.UpdateModuleSetting(ModuleId, "FormOptionDisPlay_Noidunggioithieu", Me.txtNoiDung.Text)
                objModules.UpdateModuleSetting(ModuleId, BL.settingForm_MailOK, nhanmail)
                objModules.UpdateModuleSetting(ModuleId, BL.settingForm_MailOK, nhanmail)
                objModules.UpdateModuleSetting(ModuleId, BL.settingForm_MailNhan, Me.txtemailnhan.Text)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub ShowViewTypeCate(ByVal viewCate As Boolean)
            rd_KhongGui.Checked = viewCate

            rd_Gui.Checked = Not viewCate
            tr_nhanmail.Visible = Not viewCate
        End Sub
        Protected Sub rdGetType_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
            If rd_Gui.Checked Then
                tr_nhanmail.Visible = True
            Else
                tr_nhanmail.Visible = False
            End If
        End Sub
#End Region


    End Class

End Namespace
