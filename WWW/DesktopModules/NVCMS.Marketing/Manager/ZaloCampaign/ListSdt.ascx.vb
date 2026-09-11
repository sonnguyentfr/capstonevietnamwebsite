Namespace NVCMS.Modules.Marketing
    Public MustInherit Class ZaloListSdtViewer
        Inherits Entities.Modules.PortalModuleBase

        Public ReadOnly Property CampaignId() As Integer
            Get
                Dim val As String = Request.QueryString("campaignId")
                Dim id As Integer = 0
                Integer.TryParse(val, id)
                Return id
            End Get
        End Property

        Public ReadOnly Property BackUrl() As String
            Get
                Return NavigateURL()
            End Get
        End Property

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                If CampaignId <= 0 Then
                    Response.Redirect(NavigateURL())
                End If
            End If
        End Sub

    End Class
End Namespace
