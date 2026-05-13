Imports System
Imports DotNetNuke
Imports Vbuzz.Modules.TinTuc

Namespace DesktopModules.TinTuc.Manager.newsapprove

    Public MustInherit Class Approve_inc_process
        Inherits Entities.Modules.PortalModuleBase

#Region "Property"
        Public Property ItemID() As Int64
            Get
                If Not ViewState.Item("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(ViewState.Item("ItemID"))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Int64)
                ViewState.Item("ItemID") = Value.ToString
            End Set
        End Property
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then
                    If Request.Item("itemid") <> "" Then
                        ItemID = CInt(Request.Item("itemid"))
                        Dim ctl As New NV_NewsController
                        Dim obj As NV_NewsInfo = ctl.GetByID(ItemID)
                        lbNews.Text = obj.Title
                        lbUserCreated.Text = GetUserName(obj.UserId)

                        BinddrgDataViewer()
                    End If
                    'Lưu vết để quay về
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
        Private Sub BinddrgDataViewer()
            Try
                Dim ctl As New NewsProcessController
                Dim ds As ArrayList
                ds = ctl.GetByNewsId(ItemID)

                Me.drgDataViewer.DataSource = ds
                Me.drgDataViewer.DataBind()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Public Function GetUserName(ByVal userid As Integer) As String
            Return "(" + BL.GetNameByUserId(PortalId, userid) + ")"
        End Function
        Public Function FormatVisible(ByVal id As Object) As String
            If IsNumeric(id) AndAlso id > 0 Then
                Return "True"
            Else
                Return "False"
            End If
        End Function
        Protected Sub lbtCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtCancel.Click
            Response.Redirect(Convert.ToString(ViewState("UrlReferrer")), True)
        End Sub
#End Region

    End Class
End Namespace