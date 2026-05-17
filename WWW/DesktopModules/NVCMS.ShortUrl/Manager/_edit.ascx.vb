Imports System
Imports System.Drawing.Printing
Imports System.Web
Imports System.Web.UI
Imports DotNetNuke
Imports DotNetNuke.UI.Skins.Controls
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.Video
Namespace NVCMS.Modules.ShortURL

    Public MustInherit Class inc_edit
        Inherits Entities.Modules.PortalModuleBase
        Dim _ShortUrlController As New ShortUrlController
        Dim _ShortUrlShareController As New ShortUrlShareController
#Region "Controls"
        Public Property ItemId() As Integer
            Get
                If Not ViewState.Item("ItemId") Is Nothing Then
                    Try
                        Return CInt(ViewState.Item("ItemId"))
                    Catch ex As Exception
                        Return Null.NullInteger
                    End Try
                Else
                    ViewState.Add("ItemId", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("ItemId") = Value.ToString
            End Set
        End Property

#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then

                    If IsNumeric(Request.Item("itemid")) Then
                        ItemId = Request.Item("itemid")
                    End If
                    BinddrgDataViewer(ItemId)
                Else

                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
#Region "BindData"

        Private Sub BinddrgDataViewer(ByVal ItemId As Integer)
            Dim objhistory As ShortUrl_Info = _ShortUrlController._GetByID(ItemId)
            If Not objhistory Is Nothing Then
                With objhistory
                    Me.txtshorturl.Text = .short_url
                    Me.txtrealurl.Text = .real_url
                End With
            End If
        End Sub
#End Region
#Region "Edit"
        Private Sub lbtXBSave_Click(sender As Object, e As EventArgs) Handles lbtXBSave.Click
            Dim objShortUrl As New ShortUrl_Info
            With objShortUrl
                .id = ItemId
                .short_url = txtshorturl.Text
                .create_date = DateTime.Now
                .created_by = HttpContext.Current.Request.UserHostAddress
                .real_url = txtrealurl.Text
                .created_user = UserId
            End With
            Dim stxtshorturl As String = txtshorturl.Text
            Dim objhistory As ShortUrl_Info = _ShortUrlController._Redirect(stxtshorturl)
            If ItemId > 0 Then
                Dim objhistory_curent As ShortUrl_Info = _ShortUrlController._GetByID(ItemId)
                If Not objhistory_curent Is Nothing Then
                    With objhistory_curent
                        If .short_url <> stxtshorturl Then
                            If Not objhistory Is Nothing Then
                                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Lỗi! Shortlink đã tồn tại');</script>")
                                ClientAPI.RegisterStartUpScript(Me.Page, "OpenDialogSuaNgayXuatBan", "<script>OpenDialogSuaNgayXuatBan();</script>")
                            Else
                                _ShortUrlController._CRUD("UPDATE", objShortUrl)
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "CloseDialogSuaNgayXuatBan();", True)
                                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật thành công!');</script>")
                            End If

                        Else
                            _ShortUrlController._CRUD("UPDATE", objShortUrl)
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "CloseDialogSuaNgayXuatBan();", True)
                            ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật thành công!');</script>")
                        End If
                    End With

                End If
            Else
                If Not objhistory Is Nothing Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Lỗi! Shortlink đã tồn tại');</script>")
                    ClientAPI.RegisterStartUpScript(Me.Page, "OpenDialogSuaNgayXuatBan", "<script>OpenDialogSuaNgayXuatBan();</script>")
                Else
                    _ShortUrlController._CRUD("INSERT", objShortUrl)
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "CloseDialogSuaNgayXuatBan();", True)
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật thành công!');</script>")
                End If

            End If
            DotNetNuke.Common.Utilities.DataCache.ClearCache(nvcmsBL.cacheShortUrl)

            Response.Redirect(NavigateURL(), True)

            'remove cache
        End Sub

        Private Sub lbtXoa_Click(sender As Object, e As EventArgs) Handles lbtXoa.Click
            Dim objhistory As ShortUrl_Info = _ShortUrlController._GetByID(ItemId)
            If Not objhistory Is Nothing Then
                With objhistory
                    _ShortUrlController._CRUD("DELETE", objhistory)
                    _ShortUrlShareController._DeleteByShortUrlId(.short_url)
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Xóa Link thành công!');</script>")
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "CloseDialogSuaNgayXuatBan();", True)
                    Response.Redirect(NavigateURL(), True)
                End With
            End If
        End Sub
        Private Sub lbtXBCancel_Click(sender As Object, e As EventArgs) Handles lbtXBCancel.Click
            Response.Redirect(NavigateURL(), True)
        End Sub

#End Region
    End Class


End Namespace
