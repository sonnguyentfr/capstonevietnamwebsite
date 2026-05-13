Imports System
Imports System.Web.UI
Imports DotNetNuke
Imports NVCMS.Modules.TinTuc
Namespace DesktopModules.TinTuc.Display.News

    Public MustInherit Class newsviewer
        Inherits Entities.Modules.PortalModuleBase
        Dim _newscontroller As New NV_NewsController
        Public Property fbclid() As String
            Get
                If Not ViewState.Item("fbclid") Is Nothing Then
                    Return ViewState.Item("fbclid")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("fbclid", value)
            End Set
        End Property
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                fbclid = Request.Item("fbclid")
                Dim sUrl1 As String = Request.RawUrl
                Dim sUrl As String = sUrl1.Replace("?fbclid=" & fbclid, "")
                'Dim sUrl As String = Request.RawUrl

                Dim sId As Integer = Ultis.GetRequestId(sUrl)
                If sId = -1 Then 'Index
                    Dim o_control As UserControl
                    o_control = CType(Page.LoadControl("~/DesktopModules/NVCMS.TinTuc/Display/Detail/Index.ascx"), UserControl)
                    Me.plhNews.Controls.Add(o_control)
                ElseIf sId > 0 Then 'Detail
                    Dim o_control As UserControl
                    Dim objnews As NV_NewsInfo
                    objnews = _newscontroller.GetByID(sId)
                    If Not objnews Is Nothing Then
                        With objnews
                            If .IsPhoto = True Then
                                o_control = CType(Page.LoadControl("~/DesktopModules/NVCMS.TinTuc/Display/Detail/Detail.ascx"), UserControl)
                            Else
                                o_control = CType(Page.LoadControl("~/DesktopModules/NVCMS.TinTuc/Display/Detail/Detail.ascx"), UserControl)
                            End If
                        End With
                    Else
                        o_control = CType(Page.LoadControl("~/DesktopModules/NVCMS.TinTuc/Display/Detail/Index.ascx"), UserControl)
                    End If
                    Me.plhNews.Controls.Add(o_control)
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace