Imports System
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Content.Taxonomy
Imports NVCMS.Modules.TinTuc
Imports DotNetNuke.UI.Utilities
Imports Telerik.Web.UI
Imports System.Collections.Generic
Imports NVCMS.Modules.Hethong
Imports System.IO

Namespace DesktopModules.TinTuc.Manager.news

    Public MustInherit Class newsedit
        Inherits Entities.Modules.PortalModuleBase
        Dim ctlnewbyshare As New NewsByShareController
#Region "propertice"
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
        Public Property PageSize() As Integer
            Get
                If Not ViewState("PageSize") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("PageSize"), String))
                    Catch ex As Exception
                        Return 20
                    End Try
                Else
                    ViewState.Add("PageSize", "20")
                    Return 20
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageSize") = Value.ToString
            End Set
        End Property
#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                Try
                    If Request.Item("itemid") <> "" Then
                        ItemID = CInt(Request.Item("itemid"))
                        Dim ctl As New NV_NewsController
                        Dim obj As NV_NewsInfo
                        obj = ctl.GetByID(ItemID)
                        If Not obj Is Nothing Then
                            With obj
                                If .Status = NewsStatus.DaXuatBan Then
                                    title.Text = .Title
                                    BindData(ItemID)
                                End If

                            End With
                        End If

                    Else

                    End If

                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region
#Region "bindataa"
        Public Sub BindData(id As Integer)
            Dim arr As New ArrayList
            arr = ctlnewbyshare._GetByNewID(id)
            Me.drgDataViewer.DataSource = arr
            Me.drgDataViewer.DataBind()
        End Sub
        Protected Sub lbnThoat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnThoat.Click
            Try

                Response.Redirect(NavigateURL())
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
    End Class
End Namespace