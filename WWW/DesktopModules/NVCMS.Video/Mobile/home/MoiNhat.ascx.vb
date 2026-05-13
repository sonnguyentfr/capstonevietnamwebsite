Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.Video
Namespace NVCMS.Modules.Video
    Partial Class Moinhat
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Public tabdetail As Integer = 0
        Public count As Integer = 1
        Public Sub New()
        End Sub
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    Dim ctlNews As New Videos_Controller
                    Dim arrHot As ArrayList = ctlNews.NVVideos_HOTSiteTop3(count, 0)
                    If DataCache.GetCache(BL.NewsHomeCat & "videomoinhat") Is Nothing Then
                        DataCache.SetCache(BL.NewsHomeCat & "videomoinhat", arrHot, Nothing, DateTime.Now.AddSeconds(10), TimeSpan.Zero)
                        rptMoinhatVideo.DataSource = arrHot
                        rptMoinhatVideo.DataBind()
                    Else
                        rptMoinhatVideo.DataSource = DataCache.GetCache(BL.NewsHomeCat & "videomoinhat")
                        rptMoinhatVideo.DataBind()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

    End Class
End Namespace