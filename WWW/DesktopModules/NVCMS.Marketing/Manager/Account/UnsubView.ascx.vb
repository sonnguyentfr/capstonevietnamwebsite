Imports DotNetNuke.UI.Utilities

Namespace NVCMS.Modules.Marketing

    Public MustInherit Class Unsublist
        Inherits Entities.Modules.PortalModuleBase
        Dim _Marketing_Mail_ListMailUnsubController As New Marketing_Mail_ListMailUnsubController
#Region "Controls"

#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then
                    BinddrgDataViewer()
                Else
                    Dim sTemp As String = Request("__EVENTARGUMENT")
                    If Not String.IsNullOrEmpty(sTemp) AndAlso sTemp.StartsWith("Page_") Then
                        'Fill dữ liệu vào grid
                        BinddrgDataViewer()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
#Region "BindData"
        Private Sub BinddrgDataViewer()
            Dim arr As ArrayList = _Marketing_Mail_ListMailUnsubController._GetAll()
            drgDataViewer.DataSource = arr
            drgDataViewer.DataBind()
            Me.lbTotalNewsFind.Text = arr.Count()
        End Sub
        Protected Sub btnDelete(sender As Object, e As EventArgs)
            Dim idmail = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            _Marketing_Mail_ListMailUnsubController._Delete(idmail)
            BinddrgDataViewer()
            ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Xóa dữ liệu thành công');</script>")
        End Sub
#End Region

    End Class


End Namespace
