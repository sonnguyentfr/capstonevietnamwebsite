Namespace NVCMS.Modules.Marketing
    Public MustInherit Class ZaloCampaignViewer
        Inherits Entities.Modules.PortalModuleBase

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                ' Logic khởi tạo nếu cần
            End If
        End Sub

    End Class
End Namespace
