Imports System
Imports System.Data.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Aspose.Cells
Imports Dnn.PersonaBar.Library.AppEvents
Imports Dnn.PersonaBar.Library.Prompt.Common
Imports DotNetNuke
Imports DotNetNuke.UI.Skins.Controls
Imports DotNetNuke.UI.Utilities
Imports Lucene.Net.Analysis.De
Imports Lucene.Net.Index
Imports Microsoft.AnalysisServices.AdomdClient
Imports NVCMS.Modules.EventsWebsite
Imports NVCMS.Modules.LibCRM

Namespace NVCMS.Modules.FormLandingPage
    Partial Class MainCustomeDisplay
        Inherits Entities.Modules.PortalModuleBase
        Dim _EventsWebsiteController As New EventsWebsiteController
        Dim isCaptchaValid As Boolean
        Dim _LibLocationController As New LibLocationController
        Dim _Lib_StudentInfoController As New Lib_StudentInfoController
        Dim _Lib_EventsController As New Lib_EventsController
        Dim _Lib_Events_CatController As New Lib_Events_CatController

        Dim _Lib_EventsStudentController As New Lib_EventsStudentController
        'Private ctrlGoogleReCaptcha As New GoogleReCaptcha.GoogleReCaptcha()
#Region "Propertice"
        Public shovaten As Boolean = False
        Public ssodienthoai As Boolean = False
        Public semail As Boolean = False
        Public sngaysinh As Boolean = False
        Public sdiachitinh As Boolean = False
        Public svaitro As Boolean = False
        Public schkYecauTuvan As Boolean = False
        Public seb5 As Boolean = False
        Public Property ItemID() As Integer
            Get
                If Not ViewState("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("ItemID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("ItemID") = Value.ToString
            End Set
        End Property
        Public Property isukien() As Integer
            Get
                If Not ViewState("isukien") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("isukien"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("isukien", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("isukien") = Value.ToString
            End Set
        End Property
        Public Property idiadiem() As Integer
            Get
                If Not ViewState("idiadiem") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("idiadiem"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("idiadiem", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("idiadiem") = Value.ToString
            End Set
        End Property
        Public Property sTitle() As String
            Get
                If Not ViewState.Item("sTitle") Is Nothing Then
                    Return ViewState.Item("sTitle")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("sTitle", value)
            End Set
        End Property
#End Region
#Region "page load"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try


                If UserId = 1 Or UserInfo.IsInRole("Administrators") Or UserInfo.IsInRole("Manager") Or (UserId = PortalSettings.AdministratorId) Then 'TrungNS: Add CHECK if User is Portal Administrator
                    hplEditMoudle.Visible = True
                    hplEditMoudle.NavigateUrl = NavigateURL(TabId) & "/ctl/Module/ModuleId/" & ModuleId & "?ReturnURL=/" & NavigateURL(TabId)
                End If
                Dim sbackground As String = "https://capstone.edu.vn/static/capstonev3/images/index-03/scarch-bg.jpg"
                If Not Null.IsNull(Settings("FormOptionDisPlay_Background")) Then
                    sbackground = Settings("FormOptionDisPlay_Background")
                    ltrbackground.Text = "<div class='ladi-section-background' style='background-image:url(" & sbackground & ");'></div>"
                End If
                If Not Null.IsNull(Settings("FormOptionDisPlay_Title")) Then
                    sTitle = Settings("FormOptionDisPlay_Title")
                    ltrtitle.Text = "<div class='content'><h3>" & sTitle & "</h3></div>"
                End If
                If Not Null.IsNull(Settings("FormOptionDisPlay_Noidunggioithieu")) Then
                    Dim sgioithieu = Settings("FormOptionDisPlay_Noidunggioithieu")
                    ltrnoidung.Text = Server.HtmlDecode(sgioithieu)
                End If
                'Dim shovaten As Boolean = False
                If Not Null.IsNull(Settings("FormOptionDisPlay_Hovaten")) Then
                    shovaten = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_Hovaten")), Boolean)
                    Me.hovaten.Visible = shovaten
                End If
                'Dim ssodienthoai As Boolean = False
                If Not Null.IsNull(Settings("FormOptionDisPlay_Sodienthoai")) Then
                    ssodienthoai = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_Sodienthoai")), Boolean)
                    Me.sodienthoai.Visible = ssodienthoai
                End If
                'Dim semail As Boolean = False
                If Not Null.IsNull(Settings("FormOptionDisPlay_Email")) Then
                    semail = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_Email")), Boolean)
                    Me.email.Visible = semail
                End If
                'Dim sngaysinh As Boolean = False
                If Not Null.IsNull(Settings("FormOptionDisPlay_Ngaysinh")) Then
                    sngaysinh = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_Ngaysinh")), Boolean)
                    Me.ngaysinh.Visible = sngaysinh
                End If
                'Dim sdiachitinh As Boolean = False
                If Not Null.IsNull(Settings("FormOptionDisPlay_DiachiTinh")) Then
                    sdiachitinh = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_DiachiTinh")), Boolean)
                    Me.diachitinh.Visible = sdiachitinh
                    BindTinh()
                End If
                If Not Null.IsNull(Settings("FormOptionDisPlay_Vaitro")) Then
                    svaitro = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_Vaitro")), Boolean)
                    Me.type.Visible = svaitro
                End If
                If Not Null.IsNull(Settings("FormOptionDisPlay_Yeucautuvan")) Then
                    schkYecauTuvan = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_Yeucautuvan")), Boolean)
                    Me.yeucautuvan.Visible = schkYecauTuvan
                End If

                'Dim isukien As Integer = 0
                'Dim idiadiem As Integer = 0
                If Not Null.IsNull(Settings("FormOptionDisPlay_EventCat")) Then
                    isukien = CType(Convert.ToInt64(Settings("FormOptionDisPlay_EventCat")), Integer)
                    If Not Null.IsNull(Settings("FormOptionDisPlay_EventDiaDiem")) Then
                        idiadiem = CType(Convert.ToInt64(Settings("FormOptionDisPlay_EventDiaDiem")), Integer)
                    End If
                    If isukien > 0 Then
                        Dim arrsukien As New ArrayList
                        arrsukien = _EventsWebsiteController.Events_GetAllShowByCat(isukien, 50)
                        If arrsukien.Count > 1 And idiadiem = 0 Then
                            diadiem.Visible = True
                            Me.ddldiadiem.DataSource = arrsukien
                            Me.ddldiadiem.DataTextField = "Title"
                            Me.ddldiadiem.DataValueField = "id"
                            Me.ddldiadiem.DataBind()
                            Me.ddldiadiem.Items.Insert(0, New ListItem("--Chọn địa điểm--", 0))
                        End If
                    End If
                End If
                'Dim seb5 As Boolean = False
                If Not Null.IsNull(Settings("FormOptionDisPlay_EB5")) Then
                    seb5 = CType(Convert.ToBoolean(Settings("FormOptionDisPlay_EB5")), Boolean)
                    Me.ebfive.Visible = seb5
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindTinh()

            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = _LibLocationController.Location_SelectByParentId(82, 0)
            Me.ddldiachitinh.DataSource = arrNewsCategories
            Me.ddldiachitinh.DataTextField = "Name"
            Me.ddldiachitinh.DataValueField = "id"
            Me.ddldiachitinh.DataBind()
            Me.ddldiachitinh.Items.Insert(0, New ListItem("--Nơi sinh sống--", "0"))
        End Sub

#End Region
#Region "submit"
        Public Sub Clearform()
            If shovaten = True Then
                Me.txtFullName.Text = ""
            End If
            If semail = True Then
                txtEmail.Text = ""
            End If
            If ssodienthoai = True Then
                Me.txtPhone.Text = ""
            End If
            If sngaysinh = True Then
                Me.txtngaysinh.Text = ""
            End If
            If sdiachitinh = True Then
                ddldiachitinh.SelectedValue = 0
            End If
        End Sub
        Private Function ConvertStringNonAttact(value As String) As String
            Dim objSecurity As New PortalSecurity
            value.Replace("document.cookie", "")
            value.Replace("window.location", "")
            Return System.Text.RegularExpressions.Regex.Replace(objSecurity.InputFilter(value, PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup Or PortalSecurity.FilterFlag.NoSQL Or PortalSecurity.FilterFlag.NoAngleBrackets), "<[^>]*>", "").Trim()
        End Function
        Function CheckValid() As Boolean
            Dim result As Boolean = False
            Dim ivaitro As Boolean = False
            Dim ihovaten As Boolean = False
            Dim isodienthoai As Boolean = False
            Dim iemail As Boolean = False
            Dim ingaysinh As Boolean = False
            Dim itinh As Boolean = False
            Dim icapcha As Boolean = False
            Dim _EmailValidator As New EmailValidator
            If svaitro = True Then
                If ddlType.SelectedValue = 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Bạn chưa chọn vai trò');</script>")
                    DotNetNuke.Common.SetFormFocus(Control.FindControl("ddlType"))
                    Me.ddlType.Focus()
                Else
                    ivaitro = True
                End If

            End If
            If shovaten = True Then
                If Me.txtFullName.Text = "" Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Bạn chưa nhập họ tên');</script>")
                    DotNetNuke.Common.SetFormFocus(Control.FindControl("txtFullName"))
                    Me.txtFullName.Focus()
                Else
                    ihovaten = True
                End If
            End If
            If ssodienthoai = True Then
                If Me.txtPhone.Text = "" Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Bạn chưa nhập Số điện thoại');</script>")
                    DotNetNuke.Common.SetFormFocus(Control.FindControl("txtPhone"))
                    Me.txtPhone.Focus()
                Else
                    If (Me.txtPhone.Text.Length = 10 And IsNumeric(Me.txtPhone.Text)) Then
                        isodienthoai = True
                    Else
                        ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Số điện thoại không đúng!');</script>")
                        DotNetNuke.Common.SetFormFocus(Control.FindControl("txtPhone"))
                        Me.txtPhone.Focus()
                    End If
                End If
            End If
            If semail = True Then
                If Me.txtEmail.Text = "" Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Bạn chưa nhập email');</script>")
                    DotNetNuke.Common.SetFormFocus(Control.FindControl("txtEmail"))
                    Me.txtEmail.Focus()
                Else
                    If (isEmail(Me.txtEmail.Text) = False) Then
                        ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Email không đúng định dạng!');</script>")
                        DotNetNuke.Common.SetFormFocus(Control.FindControl("txtEmail"))
                        Me.txtEmail.Focus()
                    Else
                        iemail = True
                    End If

                End If
            Else
                Return True
            End If

            If sngaysinh = True Then
                If Me.txtngaysinh.Text = "" Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Bạn chưa nhập Ngày sinh');</script>")
                    DotNetNuke.Common.SetFormFocus(Control.FindControl("txtngaysinh"))
                    Me.txtngaysinh.Focus()
                Else
                    If (Me.txtngaysinh.Text.Length = 10) Then
                        ingaysinh = True
                    Else
                        ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Ngày sinh không đúng');</script>")
                        DotNetNuke.Common.SetFormFocus(Control.FindControl("txtngaysinh"))
                        Me.txtngaysinh.Focus()
                    End If
                End If
            End If
            'If sdiachitinh = True Then
            '    If Me.ddldiachitinh.SelectedValue = "0" Then
            '        ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Bạn chưa chọn nơi sống');</script>")
            '        DotNetNuke.Common.SetFormFocus(Control.FindControl("ddldiachitinh"))
            '        Me.ddldiachitinh.Focus()
            '    Else
            '        itinh = True

            '    End If
            'End If
            If ctlCaptcha.IsValid() Then
                icapcha = True
            Else
                ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Mã bảo mật không đúng!');</script>")
            End If
            If ivaitro = True And ihovaten = True And isodienthoai = True And iemail = True And ingaysinh = True And icapcha = True Then
                result = True
            End If
            Return result
        End Function
        Private Function isEmail(inputEmail As String) As Boolean
            Dim re As New Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase)
            Return re.IsMatch(inputEmail)
        End Function
        Private Sub lbtSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
            'If (CheckValid() = True) Then
            'Đầu tiên là lấy thông tind đia điểm trước
            Dim sCode As String = ""
            Dim vp As Integer = 0
            Dim diamdiem As String = ""
            Dim thoijantu As String = ""
            Dim thoijanden As String = ""
            Dim diadiemi As Integer = 0
            If idiadiem = 0 Then
                diadiemi = ddldiadiem.SelectedValue
            Else
                diadiemi = idiadiem
            End If
            Dim objEvent As Lib_EventsInfo
            objEvent = _Lib_EventsController.Events_GetByID(diadiemi, 50)
            If Not objEvent Is Nothing Then
                With objEvent
                    vp = objEvent.Vanphong
                    diamdiem = objEvent.diadiem
                    thoijantu = objEvent.fromdatetime.ToString("HH:mm - dd/MM/yyy")
                    thoijanden = objEvent.enddatetime.ToString("HH:mm - dd/MM/yyy")
                End With
            End If


            Dim objEventCat As Lib_Events_CatInfo = _Lib_Events_CatController.Events_Cat_GetByID(isukien, 50)
            sCode += objEventCat.Code
            sCode += DateTime.Now.ToString("yMM")
            '1. Kiem tra xem Người dùng đã có trên CRM chưa
            '1.1 kiểm tra email
            Dim semail As String = Me.txtEmail.Text
            Dim objInfoEmail As Lib_StudentInfoInfo
            objInfoEmail = _Lib_StudentInfoController._Info_GetByEmail(semail)
            If Not objInfoEmail Is Nothing Then
                With objInfoEmail
                    Dim strStudentCheckinStudent1 As New List(Of String)
                    Dim arrStudentEvent As New ArrayList
                    arrStudentEvent = _Lib_EventsStudentController.Events_Student_GetAllByEvent(diadiemi)
                    If Not arrStudentEvent Is Nothing AndAlso arrStudentEvent.Count > 0 Then
                        For i As Integer = 0 To arrStudentEvent.Count - 1
                            Dim objs As Lib_EventsStudentInfo = CType(arrStudentEvent(i), Lib_EventsStudentInfo)
                            strStudentCheckinStudent1.Add(objs.StudentId)
                        Next
                    End If
                    If strStudentCheckinStudent1.Contains(.id) Then
                        _Lib_EventsStudentController.Events_Student_UpdateStudentNguon(diadiemi, objInfoEmail.id, "LandingPage")
                        _Lib_EventsStudentController.Events_Student_UpdateStudentNguonTutao(diadiemi, objInfoEmail.id, Request.Item("s") & ",")
                    Else
                        _Lib_EventsStudentController.Events_Student_Insert(diadiemi, isukien, objInfoEmail.id, objInfoEmail.CODE, SuKienZ.StatusCu, "LandingPage", DateTime.Now, 50, Request.Item("s") & ",")
                    End If
                End With
            Else
                Dim ssodienthoai As String = Me.txtPhone.Text
                Dim objInfoSodienthoai As Lib_StudentInfoInfo
                objInfoSodienthoai = _Lib_StudentInfoController._Info_GetBySodienthoai(ssodienthoai)
                If Not objInfoSodienthoai Is Nothing Then
                    With objInfoSodienthoai
                        Dim strStudentCheckinStudent1 As New List(Of String)
                        Dim arrStudentEvent As New ArrayList
                        arrStudentEvent = _Lib_EventsStudentController.Events_Student_GetAllByEvent(diadiemi)
                        If Not arrStudentEvent Is Nothing AndAlso arrStudentEvent.Count > 0 Then
                            For i As Integer = 0 To arrStudentEvent.Count - 1
                                Dim objs As Lib_EventsStudentInfo = CType(arrStudentEvent(i), Lib_EventsStudentInfo)
                                strStudentCheckinStudent1.Add(objs.StudentId)
                            Next
                        End If
                        If strStudentCheckinStudent1.Contains(.id) Then
                            _Lib_EventsStudentController.Events_Student_UpdateStudentNguon(diadiemi, objInfoSodienthoai.id, "LandingPage")
                            _Lib_EventsStudentController.Events_Student_UpdateStudentNguonTutao(diadiemi, objInfoSodienthoai.id, Request.Item("s") & ",")
                        Else
                            _Lib_EventsStudentController.Events_Student_Insert(diadiemi, isukien, objInfoSodienthoai.id, objInfoSodienthoai.CODE, SuKienZ.StatusCu, "LandingPage", DateTime.Now, 50, Request.Item("s") & ",")
                        End If
                    End With
                Else
                    Dim firstname = ConvertStringNonAttact(Me.txtFullName.Text)
                    Dim lastname = ConvertStringNonAttact(Me.txtFullName.Text)
                    Dim sdt = ConvertStringNonAttact(Me.txtPhone.Text)
                    Dim emailaddress = ConvertStringNonAttact(txtEmail.Text)

                    'Tuvan
                    Dim tuvannamdi As String = ""
                    'ConvertStringNonAttact(Me.txtTuVanNamDi.Value)
                    Dim tuvannganhhoc As String = "" '= ConvertStringNonAttact(Me.txtTuVanNganhHoc.Value)
                    Dim tuvantruong As String = "" '= ConvertStringNonAttact(Me.txtTuvanTruong.Value)
                    Dim tKhacdanghoc As String = ""
                    Dim stKhacTruong As String = ""
                    Dim stKhacTruongnew = ConvertStringNonAttact(stKhacTruong)
                    Dim tKhacDiemTrungbinh As String = "" '= ConvertStringNonAttact(Me.tKhacDiemTrungbinh.Value)
                    Dim tKhacdiemsobaithi As String = "" ' = ConvertStringNonAttact(Me.tKhacdiemsobaithi.Value)
                    Dim tKhacNote As String = "" '= ConvertStringNonAttact(Me.tKhacNote.Text)
                    Dim diachi As String = ""
                    Dim itinh As Integer = 0
                    If sdiachitinh = True Then
                        itinh = ddldiachitinh.SelectedValue
                    End If
                    Dim sngaysinh As Date = Date.Now
                    Try
                        If Not String.IsNullOrEmpty(txtngaysinh.Text) Then
                            sngaysinh = Date.ParseExact(txtngaysinh.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        End If
                    Catch ex As Exception
                    End Try
                    Dim stuvankhacz As String = ConvertStringNonAttact(Me.txtyeucautuvan.Text)
                    'Dim stuvankhacz As String = "<mark>Khách đăng ký Online tại: " & objEventCat.CatName & "</mark>."
                    'If stuvankhac <> "" Then
                    '    stuvankhacz += "Muốn tư vấn:" & stuvankhac
                    'End If
                    If svaitro = True Then
                        If Me.ddlType.SelectedValue = 1 Then
                            ItemID = _Lib_StudentInfoController._Info_Insert(vp, ddlType.SelectedValue, firstname, lastname, 0, sngaysinh, 1, sdt, emailaddress, diachi, itinh, 0, Me.chkebfive.Checked, "", 15, 0, stuvankhacz, 1, DateTime.Now, "", tuvannamdi, 0, tuvannganhhoc, tuvantruong, 0, 0, 0, stuvankhacz, UserId, DateTime.Now, UserId, DateTime.Now, tKhacdanghoc, stKhacTruongnew, tKhacDiemTrungbinh, tKhacdiemsobaithi, tKhacNote, UserId, DateTime.Now, UserId, DateTime.Now, DateTime.Now, UserId, 50, False)
                        End If
                        If Me.ddlType.SelectedValue = 2 Then
                            ItemID = _Lib_StudentInfoController._Info_Insert(vp, ddlType.SelectedValue, firstname, lastname, 0, sngaysinh, 1, sdt, emailaddress, diachi, itinh, 0, Me.chkebfive.Checked, "", 15, 0, stuvankhacz, 1, DateTime.Now, "", tuvannamdi, 0, tuvannganhhoc, tuvantruong, 0, 0, 0, stuvankhacz, UserId, DateTime.Now, UserId, DateTime.Now, tKhacdanghoc, stKhacTruongnew, tKhacDiemTrungbinh, tKhacdiemsobaithi, tKhacNote, UserId, DateTime.Now, UserId, DateTime.Now, DateTime.Now, UserId, 50, False)
                        End If
                    Else
                        ItemID = _Lib_StudentInfoController._Info_Insert(vp, ddlType.SelectedValue, firstname, lastname, 0, sngaysinh, 1, sdt, emailaddress, diachi, itinh, 0, Me.chkebfive.Checked, "", 15, 0, stuvankhacz, 1, DateTime.Now, "", tuvannamdi, 0, tuvannganhhoc, tuvantruong, 0, 0, 0, stuvankhacz, UserId, DateTime.Now, UserId, DateTime.Now, tKhacdanghoc, stKhacTruongnew, tKhacDiemTrungbinh, tKhacdiemsobaithi, tKhacNote, UserId, DateTime.Now, UserId, DateTime.Now, DateTime.Now, UserId, 50, False)
                    End If
                    _Lib_StudentInfoController._Info_InsertCode(ItemID, sCode & ItemID)
                    _Lib_EventsStudentController.Events_Student_Insert(diadiemi, isukien, ItemID, sCode & ItemID, SuKienZ.StatusOnline, "Landingpage", DateTime.Now, 50, Request.Item("s") & ",")
                    Clearform()
                    foregsuc.Visible = True
                    formreg.Visible = False
                End If
            End If



            'End If
        End Sub

#End Region
    End Class
End Namespace
