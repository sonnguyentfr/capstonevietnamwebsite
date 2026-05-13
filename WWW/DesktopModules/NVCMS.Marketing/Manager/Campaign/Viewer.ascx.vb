Imports DotNetNuke.UI.Utilities

Namespace NVCMS.Modules.Marketing

    Public MustInherit Class Campaign
        Inherits Entities.Modules.PortalModuleBase

#Region "Controls"
        Dim _Marketing_Mail_Campaing As New Marketing_Mail_Campaing
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
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    BindGridData()
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region

        Private Sub BindGridData()
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = _Marketing_Mail_Campaing._GetAll()
            Me.ltrcount.Text = arrNewsCategories.Count
            Me.rptlistacc.DataSource = arrNewsCategories
            Me.rptlistacc.DataBind()
        End Sub
        Public Function GetTotalMail(id As Integer) As Integer
            Dim ctl As New Marketing_Mail_ListMail
            Dim arr As New ArrayList
            arr = ctl._GetAll(id)
            Return arr.Count
        End Function
#Region "edit insert"
        Protected Sub GetInfo(sender As Object, e As EventArgs)
            ItemID = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            Dim objMarketing_Mail_CampaingInfo As Marketing_Mail_CampaingInfo
            objMarketing_Mail_CampaingInfo = _Marketing_Mail_Campaing._GetByID(ItemID)
            If Not objMarketing_Mail_CampaingInfo Is Nothing Then
                With objMarketing_Mail_CampaingInfo
                    lbtDelete.Visible = True
                    Me.txtTitle.Text = .Title
                    txtMota.Text = .Description
                End With

            End If
            ClientAPI.RegisterStartUpScript(Me.Page, "Modalhoatdong", "<script>Modalhoatdong();</script>")
        End Sub

        Private Sub lbtUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
            Try
                If ItemID > 0 Then
                    'Edit
                    _Marketing_Mail_Campaing._Update(ItemID, Me.txtTitle.Text, txtMota.Text, DateTime.Now, UserId, PortalId)
                Else
                    _Marketing_Mail_Campaing._Insert(Me.txtTitle.Text, txtMota.Text, DateTime.Now, UserId, PortalId)
                End If
                Me.txtTitle.Text = ""
                txtMota.Text = ""
                ItemID = 0
                BindGridData()
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "ModalFollowUpClose();", True)
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật thành công!');</script>")
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Protected Sub lbtDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtDelete.Click
            If ItemID > 0 Then
                _Marketing_Mail_Campaing._Delete(ItemID)
                ItemID = 0
                BindGridData()
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "ModalFollowUpClose();", True)
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Xóa dữ liệu thành công!');</script>")
            End If
        End Sub
        Private Sub lbtAdd_Click(sender As Object, e As EventArgs) Handles lbtAdd.Click, lbtAddTop.Click
            Me.txtTitle.Text = ""
            txtMota.Text = ""
            Me.lbtDelete.Visible = False
            ItemID = 0
            ClientAPI.RegisterStartUpScript(Me.Page, "Modalhoatdong", "<script>Modalhoatdong();</script>")
        End Sub

#End Region
    End Class

End Namespace
