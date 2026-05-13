Imports System
Imports DotNetNuke
Imports NVCMS.Modules.Video
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.NV_Videos.Manager.Videos
    Public MustInherit Class Listen
        Inherits Entities.Modules.PortalModuleBase
        Private ctlVideos As New Videos_Controller

        Public Property ItemID() As Integer
            Get
                If Not ViewState.Item("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState.Item("ItemID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("ItemID") = Value.ToString
            End Set
        End Property

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then
                    ItemID = CType(Request.Item("videosclip"), Integer)
                    Dim objVideo As Videos_Info = ctlVideos.GetByID(ItemID, PortalId)
                    If Not objVideo Is Nothing Then
                        With objVideo
                            Me.lblTenBaiHat.Text = objVideo.Title
                            ltrtomtat.Text = .Tomtat
                            ltrnoidung.Text = Server.HtmlDecode(.Noidung)
                            Me.ltrdate.Text = BL.FormatDate(.PublicDate)
                            If (.IsNotes = True) Then
                            Else
                                If .IsYoutube = True Then
                                    ltrkieuvideo.Text = "<strong>Youtube</strong>"
                                    Me.ltrplayVideo.Text = "<iframe width='100%' height='400px' src='https://www.youtube.com/embed/" & .LinkVideos & "?autoplay=1&autohide=1&showinfo=0&wmode=opaque&rel=0&loop=1&enablejsapi=1&origin=" & HttpContext.Current.Request.Url.Authority & "&widgetid=1' frameborder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe>"
                                End If
                                If .IsNotes = False And .IsYoutube = False Then
                                    ltrkieuvideo.Text = "<strong>File</strong>"
                                    Me.ltrplayVideo.Text = "<video style='width:100%' src='" & .LinkVideos & "' controls='controls'></video>"
                                End If
                            End If
                        End With
                    End If
                    'Lưu vết để quay về (Giữ nguyên bộ search khi back lại)
                    If Not Request.UrlReferrer Is Nothing Then
                        ViewState("UrlReferrer") = Convert.ToString(Request.UrlReferrer)
                    Else
                        ViewState("UrlReferrer") = ""
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub lbtCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtCancel.Click
            Response.Redirect(Convert.ToString(ViewState("UrlReferrer")), True)
        End Sub

#End Region

    End Class
End Namespace