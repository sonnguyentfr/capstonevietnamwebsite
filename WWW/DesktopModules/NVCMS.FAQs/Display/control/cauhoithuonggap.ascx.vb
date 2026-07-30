Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports System.IO

Namespace NVCMS.Modules.FAQs
    Partial Class MainCustomeDisplaySpecial
        Inherits Entities.Modules.PortalModuleBase
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                LoadData()
                'End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub LoadData()
            Try
                Dim ctl As New Question_Controller()
                'Hien thi tin theo cai dat type
                Dim listMore = ctl._Find_Index("", "", 3, PortalSettings.PortalId, 1, 100)
                rptcauhoithuonggap.DataSource = listMore
                rptcauhoithuonggap.DataBind()
                rptcauhoithuonggap2.DataSource = listMore
                rptcauhoithuonggap2.DataBind()
            Catch ex As Exception
                'ltContent.Text = "Load module error . " + ex.Message
            End Try
        End Sub
        Protected Function GetItemClass(ByVal itemIndex As Integer) As String
            If itemIndex = 0 Then
                Return "active"
            Else
                Return ""
            End If
        End Function
        Protected Function GetItemClass2(ByVal itemIndex As Integer) As String
            If itemIndex = 0 Then
                Return "active show"
            Else
                Return ""
            End If
        End Function
    End Class
End Namespace
