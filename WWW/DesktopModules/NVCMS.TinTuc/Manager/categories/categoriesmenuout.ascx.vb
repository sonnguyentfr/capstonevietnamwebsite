Imports System
Imports DotNetNuke
Imports NVCMS.Modules.Hethong

Namespace DesktopModules.TinTuc.Manager.categories
    Public MustInherit Class categoriesmenuout
        Inherits Entities.Modules.PortalModuleBase

#Region "Controls"
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                Try
                    Dim ds As DataSet
                    ds = NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsCategories_selectall(PortalId)
                    Me.drgMenu.DataSource = ds
                    Me.drgMenu.DataBind()
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region

        Public Function FormatURL(ByVal sitem As String, ByVal strID As String) As String
            Return NavigateURL(54) & "&" & sitem & "=" & strID
        End Function
    End Class

End Namespace
