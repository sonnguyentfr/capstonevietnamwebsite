Imports System.IO
Imports System.Xml
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.Video
Imports NVCMS.Modules.Hethong
Namespace DesktopModules.TinTuc.Controls
    Partial Class Headersss
        Inherits PortalModuleBase
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
                    ViewState.Add("PageSize", "20")
                    Return 20
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
        Public Property Datefrom() As String
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return ViewState.Item("Datefrom")
                Else
                    Return "01/01/2010"
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("Datefrom", value)
            End Set
        End Property
        Public Property DateTo() As String
            Get
                If Not ViewState.Item("todate") Is Nothing Then
                    Return ViewState.Item("todate")
                Else
                    Return "01/01/2100"
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("todate", value)
            End Set
        End Property
        Public Property CategoryId() As Integer
            Get
                If Not ViewState.Item("CategoryId") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("CategoryId")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("CategoryId", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CategoryId") = Value.ToString
            End Set
        End Property
        Public Property CreatedUser() As Integer
            Get
                If Not ViewState.Item("CreatedUser") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("CreatedUser")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("CreatedUser", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CreatedUser") = Value.ToString
            End Set
        End Property
        Public Property Status() As Integer
            Get
                If Not ViewState.Item("Status") Is Nothing Then
                    Dim x As Integer = -1
                    Try : x = CInt(ViewState.Item("Status")) : Catch ex As Exception : x = -1 : End Try
                    Return x
                Else
                    ViewState.Add("Status", "-1")
                    Return -1
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("Status") = Value.ToString
            End Set
        End Property
#End Region
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not IsPostBack Then
                    'Lay thong tin user
                    BindPage()
                    Me.ltrCurrentWebite.Text = PortalContextHelper.CurrentPortal.PortalName & " (" & PortalId & ")"
                    Dim obju As UserInfo
                    Dim ctluser As New UserController
                    obju = ctluser.GetUser(PortalId, UserId)
                    If Not obju Is Nothing Then
                        With obju
                            imgAvtar.ImageUrl = .Profile.GetPropertyValue("Avatar")
                            ltrname.Text = BL.GetButDanh(PortalId, UserId)
                            ltremail.Text = .Email
                        End With
                    End If
                    'chờ duyệt
                    Dim ctlnews As New NV_NewsController
                    If UserInfo.IsInRole("Phe duyet") Then
                        Me.hplchopheduyet.Visible = True
                        Me.ltrchobientap.Text = ctlnews.SelectApproveNews_Count(Datefrom, DateTo, KeySearch, CategoryId, NewsStatus.ChoPheDuyet, PortalId, CreatedUser, False)
                        Me.rptchopheduyet.DataSource = ctlnews.SelectApproveNews_Index(Datefrom, DateTo, KeySearch, CategoryId, NewsStatus.ChoPheDuyet, PortalId, CreatedUser, 1, 10, False)
                        Me.rptchopheduyet.DataBind()
                    End If
                    'cho xuat ban
                    If UserInfo.IsInRole("Xuat ban") Then
                        Me.hplchoxuatban.Visible = True
                        ltrchoxuatban.Text = ctlnews.SelectApproveNews_Count(Datefrom, DateTo, KeySearch, CategoryId, NewsStatus.ChoPheDuyet, PortalId, CreatedUser, False)
                        Me.rptchoxuatban.DataSource = ctlnews.SelectApproveNews_Index(Datefrom, DateTo, KeySearch, CategoryId, NewsStatus.ChoXuatBan, PortalId, CreatedUser, 1, 10, False)
                        Me.rptchoxuatban.DataBind()
                    End If
                    'Vdeo chờ xuất bản
                    Dim _Videos_Controller As New Videos_Controller
                    If UserInfo.IsInRole("Xuat ban") Then
                        Me.hplvideochoxuatban.Visible = True
                        ltrvideochoxuatban.Text = _Videos_Controller.Find_Count(Datefrom, DateTo, KeySearch, CategoryId, PortalId, NewsStatus.ChoXuatBan, 0)
                        Me.rptvideochoxuatban.DataSource = _Videos_Controller.Find_Index(Datefrom, DateTo, KeySearch, CategoryId, PortalId, NewsStatus.ChoXuatBan, 0, 1, 10, "")
                        Me.rptvideochoxuatban.DataBind()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub BindPage()
            Dim arrpage As New ArrayList
            arrpage = PortalController.Instance.GetPortals()

            Me.ddlPortal.DataSource = arrpage
            Me.ddlPortal.DataTextField = "PortalName"
            Me.ddlPortal.DataValueField = "PortalId"
            Me.ddlPortal.DataBind()
            If Session("CurrentPortal") IsNot Nothing Then
                Dim current = CType(Session("CurrentPortal"), CurrentPortalContextModel)
                ddlPortal.SelectedValue = current.PortalId.ToString()
                ltrCurrentWebite.Text = current.PortalName
            Else
                Me.ddlPortal.SelectedValue = PortalId
            End If
        End Sub
        Protected Sub ddlPortal_SelectedIndexChanged(sender As Object, e As EventArgs)
            Dim selectedPortalId As Integer = Convert.ToInt32(ddlPortal.SelectedValue)
            Dim portal = PortalController.Instance.GetPortal(selectedPortalId)
            Dim current As New CurrentPortalContextModel With {
                .PortalId = portal.PortalID,
                .PortalName = portal.PortalName
            }
            Session("CurrentPortal") = current
            ltrCurrentWebite.Text = current.PortalName & " (" & current.PortalId & ")"
            Dim script As String = "UpdateSuccess('Cập nhật thành công!');setTimeout(function () {    window.location.href = window.location.href;},2000);"
            ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>" & script & "</script>")
        End Sub
    End Class
End Namespace
