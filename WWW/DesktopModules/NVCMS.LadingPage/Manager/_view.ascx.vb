Imports System
Imports System.Web
Imports System.Web.UI
Imports DotNetNuke
Imports DotNetNuke.UI.Utilities

Namespace NVCMS.Modules.LadingPage

    Public MustInherit Class inc_list
        Inherits Entities.Modules.PortalModuleBase
        Dim _LadingPage_Controller As New LadingPage_Controller

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then
                    BinddrgDataViewer()
                Else
                    Dim sTemp As String = Request("__EVENTARGUMENT")
                    If Not String.IsNullOrEmpty(sTemp) AndAlso sTemp.StartsWith("Page_") Then
                        BinddrgDataViewer()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
        Private Sub BinddrgDataViewer()
            Try
                Dim arrrecode As ArrayList = _LadingPage_Controller._GetAllByParentId(0, PortalId)
                drgDataViewer.DataSource = arrrecode
                drgDataViewer.DataBind()
                Me.lbTotalNewsFind.Text = arrrecode.Count
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub OndrgDataViewer(sender As Object, e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim customerId As String = TryCast(e.Item.FindControl("hdfid"), HiddenField).Value
                Dim rptOrders As Repeater = TryCast(e.Item.FindControl("rpttranLadingPagesub"), Repeater)
                rptOrders.DataSource = _LadingPage_Controller._GetAllByParentId(customerId, PortalId)
                rptOrders.DataBind()
            End If
        End Sub
        Protected Sub OnrpttranLadingPagesub(sender As Object, e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim customerId As String = TryCast(e.Item.FindControl("hdfid2"), HiddenField).Value
                Dim rptOrders As Repeater = TryCast(e.Item.FindControl("rpttranLadingPagesub2"), Repeater)
                rptOrders.DataSource = _LadingPage_Controller._GetAllByParentId(customerId, PortalId)
                rptOrders.DataBind()
            End If
        End Sub
        Protected Sub OnrpttranLadingPagesub2(sender As Object, e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim customerId As String = TryCast(e.Item.FindControl("hdfid3"), HiddenField).Value
                Dim rptOrders As Repeater = TryCast(e.Item.FindControl("rpttranLadingPagesub3"), Repeater)
                rptOrders.DataSource = _LadingPage_Controller._GetAllByParentId(customerId, PortalId)
                rptOrders.DataBind()
            End If
        End Sub
        Private Sub lbtAddBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAddBottom.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub lbtAddTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAddTop.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        ''' <summary>
        ''' Quick View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub cmdquickview(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim itemidhistory As Integer = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            Dim objTrangLadingPage As LadingPage_Info = _LadingPage_Controller._GetByID(itemidhistory, PortalId)
            If Not objTrangLadingPage Is Nothing Then
                With objTrangLadingPage
                    ltrcautraloi.Text = Server.HtmlDecode(.Noidung)
                End With
            End If
            ClientAPI.RegisterStartUpScript(Me.Page, "OpenDialogHistory", "<script>OpenDialogHistory();</script>")

        End Sub
    End Class

End Namespace
