Namespace NVCMS.Modules.Marketing

    ''' <summary>
    ''' Dashboard thong ke chien dich Zalo ZNS.
    ''' Toan bo so lieu duoc lay tu sp_Marketing_Zalo_Campaign_Analytics
    ''' thong qua API /DesktopModules/NVCMS/API/ZaloReport/GetDashboard.
    ''' </summary>
    Public MustInherit Class ZaloCampaignStatic
        Inherits Entities.Modules.PortalModuleBase

        Private _campaignCtl As New Marketing_Zalo_Campaign_Controller()

        ''' <summary>Id chien dich Zalo lay tu querystring.</summary>
        Public ReadOnly Property CampaignId() As Integer
            Get
                Dim id As Integer = 0
                Integer.TryParse(Request.QueryString("campaignId"), id)
                Return id
            End Get
        End Property

        ''' <summary>Duong dan quay lai danh sach chien dich.</summary>
        Public ReadOnly Property BackUrl() As String
            Get
                Return NavigateURL()
            End Get
        End Property

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            If CampaignId <= 0 Then
                Response.Redirect(NavigateURL())
                Return
            End If

            If Not IsPostBack Then
                ' Kiem tra chien dich ton tai truoc khi render dashboard
                Dim camp As Marketing_Zalo_CampaignInfo = _campaignCtl._GetByID(CampaignId)
                If camp Is Nothing Then
                    Response.Redirect(NavigateURL())
                    Return
                End If
            End If
        End Sub

    End Class

End Namespace
