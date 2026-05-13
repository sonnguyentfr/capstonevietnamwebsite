Imports System
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.PhongBan
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.User
Imports NVCMS.Web.Components

Namespace DesktopModules.TinTuc.Manager.adminnews

    Public MustInherit Class forhome
        Inherits Entities.Modules.PortalModuleBase
        Dim ctlnhuabut As New NhuanButController

#Region "Properties"
        Public Property TotalPage() As Integer
            Get
                If Not ViewState.Item("TotalPage") Is Nothing Then
                    Try
                        Return CInt(ViewState.Item("TotalPage"))
                    Catch ex As Exception
                        Return Null.NullInteger
                    End Try
                Else
                    ViewState.Add("TotalPage", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("TotalPage") = Value.ToString
            End Set
        End Property
        Property CurrentPage() As Integer 'Trang hiện tại
            Get
                If Not ViewState.Item("CurrentPage") Is Nothing Then
                    Return CInt(ViewState.Item("CurrentPage"))
                Else
                    ViewState.Add("CurrentPage", "1")
                    Return 1
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("CurrentPage") = value.ToString
            End Set
        End Property
        Property PageSize() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("PageSize") Is Nothing Then
                    Return CInt(ViewState.Item("PageSize"))
                Else
                    ViewState.Add("PageSize", "10000")
                    Return 10000
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("PageSize") = value.ToString
            End Set
        End Property
        Property TotalRecord() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("TotalRecord") Is Nothing Then
                    Return CInt(ViewState.Item("TotalRecord"))
                Else
                    ViewState.Add("TotalRecord", "0")
                    Return 0
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("TotalRecord") = value.ToString
            End Set
        End Property
        Public Property KeySearch() As String
            Get
                If Not ViewState.Item("KeySearch") Is Nothing Then
                    Return ViewState.Item("KeySearch")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("KeySearch", value)
            End Set
        End Property
        Public Property Datefrom() As Date
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return CType(ViewState.Item("Datefrom"), Date)
                Else
                    Return DateTime.Now.Day & "/" & DateTime.Now.Month & "/" & DateTime.Now.Year
                End If
            End Get
            Set(ByVal value As Date)
                ViewState.Add("Datefrom", value)
            End Set
        End Property
        Public Property DateTo() As Date
            Get
                If Not ViewState.Item("todate") Is Nothing Then
                    Return CType(ViewState.Item("todate"), Date)
                Else
                    Return DateTime.Now.Day & "/" & DateTime.Now.Month & "/" & DateTime.Now.Year
                End If
            End Get
            Set(ByVal value As Date)
                ViewState.Add("todate", value)
            End Set
        End Property


        Public Property uid() As Integer
            Get
                If Not ViewState.Item("uid") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("uid")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("uid", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("uid") = Value.ToString
            End Set
        End Property


#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(tblSearch, Me.lbtFind, Asc(vbCr))
                If Not IsPostBack Then

                    KeySearch = Request.Item("key")
                    If IsDate(Request.Item("from")) Then
                        Datefrom = Request.Item("from")
                        If Not Datefrom = BL.minDateV Then
                            Me.txtStartdate.Value = Datefrom
                        End If
                    End If
                    If IsDate(Request.Item("to")) Then
                        DateTo = Request.Item("to")
                        If Not DateTo = BL.maxDateV Then
                            Me.txtEndDate.Value = DateTo
                        End If
                    End If
                    thoigiantu.Text = Datefrom
                    thoigianden.Text = DateTo
                    BindDllUserPost()
                    If IsNumeric(Request.Item("uid")) Then
                        uid = Request.Item("uid")
                        ddlUserPost.SelectedValue = uid
                    End If
                    BinddrgDataViewer()
                Else
                    Dim sTemp As String = Request("__EVENTARGUMENT")
                    If Not String.IsNullOrEmpty(sTemp) AndAlso sTemp.StartsWith("Page_") Then
                        CurrentPage = Integer.Parse(sTemp.Replace("Page_", ""))

                        'Fill dữ liệu vào grid
                        BinddrgDataViewer()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BinddrgDataViewer()
            Try
                Dim ctl As New NV_NewsController
                TotalRecord = ctl.FindByPhongBanStatus_Count(Datefrom, DateTo, "", 0, PortalId, NewsStatus.DaXuatBan, uid, "")
                Me.lbTotalNewsCount.Text = CType(TotalRecord, String)
                'xuat ra text html
                Dim strtable As String = ""
                'lay user ra
                If uid > 0 Then
                    Dim obj As UserInfo = UserController.GetUserById(PortalId, uid)
                    If Not obj Is Nothing Then
                        With obj
                            Dim isobaiviet As New Integer
                            isobaiviet = ctl.FindByPhongBanStatus_Count(CType(Datefrom, Date), CType(DateTo, Date), "", 0, PortalId, NewsStatus.DaXuatBan, obj.UserID, "")
                            If (isobaiviet > 0) Then
                                strtable += "<div class='nk-tb-item'><div class='nk-tb-col'><div class='user-card'><div class='user-info'><span class='tb-lead'>" & obj.DisplayName & "</span></div></div></div><div class='nk-tb-col tb-col-mb'><span class='tb-amount'>" & isobaiviet & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 0) & "</span></div><div class='nk-tb-col tb-col-lg'><span>" & Counttype(obj.UserID, 7) & "</span></div><div class='nk-tb-col tb-col-lg'><span>" & Counttype(obj.UserID, 2) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 3) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 4) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 5) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 6) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 9) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 12) & "</span></div><div class='nk-tb-col tb-col-md'><span class='auto currency'>" & GetNhuanBut(obj.UserID) & "</span></div></div>"
                            Else
                                strtable = "<h2>Làm gì có bài mà tìm ò_ó </h2>"
                            End If
                        End With
                    End If


                Else
                    Dim arr As ArrayList = UserController.GetUsers(PortalId)
                    If (arr.Count > 0) Then
                        For i As Integer = 0 To arr.Count - 1
                            Dim obj As UserInfo = CType(arr(i), UserInfo)
                            If obj.Membership.Approved = True Then
                                'Lay tin bai cua no ra
                                Dim isobaiviet As New Integer
                                isobaiviet = ctl.FindByPhongBanStatus_Count(CType(Datefrom, Date), CType(DateTo, Date), "", 0, PortalId, NewsStatus.DaXuatBan, obj.UserID, "")
                                If (isobaiviet > 0) Then
                                    strtable += "<div class='nk-tb-item'><div class='nk-tb-col'><div class='user-card'><div class='user-info'><span class='tb-lead'>" & obj.DisplayName & "</span></div></div></div><div class='nk-tb-col tb-col-mb'><span class='tb-amount'>" & isobaiviet & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 0) & "</span></div><div class='nk-tb-col tb-col-lg'><span>" & Counttype(obj.UserID, 7) & "</span></div><div class='nk-tb-col tb-col-lg'><span>" & Counttype(obj.UserID, 2) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 3) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 4) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 5) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 6) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 9) & "</span></div><div class='nk-tb-col tb-col-md'><span>" & Counttype(obj.UserID, 12) & "</span></div><div class='nk-tb-col tb-col-md'><span class='auto currency'>" & GetNhuanBut(obj.UserID) & "</span></div></div>"
                                End If
                            End If
                        Next
                    End If
                End If

                Me.ltrnhuanbut.Text = strtable
                '---
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Function Counttype(Userids As Integer, types As Integer) As Integer
            Dim ctl As New NV_NewsController
            Dim icount As Integer = 0
            'icount = ctl.AdminFind_CountNewsKind(Datefrom, DateTo, "", 0, PortalId, Userids, "vi-VN", types)
            icount = ctl.FindByPhongBanStatus_CountWithType(Datefrom, DateTo, "", types, PortalId, NewsStatus.DaXuatBan, Userids, "")
            Return icount
        End Function
        Public Function GetNhuanBut(ByVal uid As Integer) As Integer
            Dim snhuanbut As Integer = 0
            Dim _NhuanButController As New NhuanButController
            snhuanbut = _NhuanButController.NhuanBut_User_GetTongTien(Datefrom, DateTo, uid)
            Return snhuanbut
        End Function
        Private Sub BindDllUserPost()
            'Me.ddlUserPost.DataSource = UserController.GetUsers(PortalId)
            'Me.ddlUserPost.DataTextField = "Username"
            'Me.ddlUserPost.DataValueField = "UserID"
            'Me.ddlUserPost.DataBind()
            'Me.ddlUserPost.Items.Insert(0, New ListItem("-- Chọn tác giả --", "0"))
            Dim arrU As New ArrayList
            arrU = UserController.GetUsers(PortalId)
            Dim arrUNews As New ArrayList
            If arrU.Count > 0 Then
                For i As Integer = 0 To arrU.Count - 1
                    Dim obju As UserInfo = CType(arrU(i), UserInfo)
                    With obju
                        If .IsDeleted = False Then
                            If .Membership.Approved = True Then
                                arrUNews.Add(obju)
                            End If

                        End If
                    End With

                Next
            End If
            Me.ddlUserPost.DataSource = arrUNews
            Me.ddlUserPost.DataTextField = "DisplayName"
            Me.ddlUserPost.DataValueField = "UserID"
            Me.ddlUserPost.DataBind()
            Me.ddlUserPost.Items.Insert(0, New ListItem("--Tất cả--", "0"))
        End Sub
        'Private Sub BindDllUserPost()
        '    Me.ddlUserPost.DataSource = UserController.GetUsers(PortalId)
        '    Me.ddlUserPost.DataTextField = "DisplayName"
        '    Me.ddlUserPost.DataValueField = "UserID"
        '    Me.ddlUserPost.DataBind()
        '    Me.ddlUserPost.Items.Insert(0, New ListItem("--Tất cả người dùng--", "0"))
        'End Sub
        Private Sub lbtFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtFind.Click
            Try
                HttpCacheHelper.RemoveCache("NhuanButView" & CurrentPage & 0 & uid & DateTo & Datefrom)
                Response.Redirect(NavigateURL(TabId) & "?uid=" & ddlUserPost.SelectedValue & "&from=" & Me.txtStartdate.Value & "&to=" & Me.txtEndDate.Value)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Function Highlight(ByVal InputTxt As String,
                    ByVal StartTag As String,
                    ByVal EndTag As String) As String

            Dim ResultStr As String = InputTxt
            Dim strArr As String() = KeySearch.Trim.Split(" ")
            For Each word As String In strArr
                ResultStr = Regex.Replace(ResultStr, "\b(" & Regex.Escape(word) & ")\b", StartTag & "$1" & EndTag, RegexOptions.IgnoreCase)
            Next

            Return ResultStr
        End Function

#End Region


    End Class
End Namespace