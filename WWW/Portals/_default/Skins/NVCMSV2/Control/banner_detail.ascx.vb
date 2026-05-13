Imports System.IO
Imports System.Xml
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Web.Components

Namespace NVCMS.Modules.BannerAdv
    Partial Class BannerDetail
        Inherits Entities.Modules.PortalModuleBase
        Public Property bannerid() As Integer
            Get
                If Not ViewState.Item("bannerid") Is Nothing Then
                    Try
                        Return CInt(ViewState.Item("bannerid"))
                    Catch ex As Exception
                        Return Null.NullInteger
                    End Try
                Else
                    ViewState.Add("bannerid", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("bannerid") = Value.ToString
            End Set
        End Property
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                Try
                    Dim sUrl As String = Request.RawUrl
                    'Dim sUrl As String = Request.RawUrl
                    Dim sId As Integer = nvcmsBL.GetRequestId(sUrl)
                    'Response.Write(sId)
                    Dim ctlAdvbaner As New BannerAdvController
                    If IsNumeric(sId) Then
                        Dim ctlstatic As New BannerAdv_StaticController
                        ctlstatic._Insert(sId, Request.ServerVariables("REMOTE_ADDR"), DateTime.Now, True)
                        'update vao bang banner
                        ctlAdvbaner.UpdateClick(sId)
                        Dim objInfo As BannerAdvInfo = ctlAdvbaner.GetByID(sId)
                        If Not objInfo Is Nothing Then
                            With objInfo
                                Me.lblName.Text = .Title
                                If (.Link <> "") Then
                                    'Response.AddHeader("REFRESH", "10; URL=" & .Link)
                                    ' Wait for 10 seconds before redirecting
                                    'System.Threading.Thread.Sleep(10000)
                                    Response.Redirect(.Link, True)
                                Else
                                    Response.Redirect("/")
                                End If

                            End With
                        Else
                            Response.Redirect("/")
                        End If
                    End If
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub

#End Region

    End Class
End Namespace