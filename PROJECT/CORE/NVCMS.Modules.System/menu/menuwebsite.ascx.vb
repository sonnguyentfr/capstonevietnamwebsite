Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.SessionState

Public Class menuwebsite
    Implements IHttpHandler, IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "application/json"
        Dim menuItems As List(Of MenuItem) = GetMenuItems()
        Dim json As String = New JavaScriptSerializer().Serialize(menuItems)
        context.Response.Write(json)
    End Sub

    Private Function GetMenuItems() As List(Of MenuItem)
        ' Replace with your database connection and query logic
        Dim menuItems As New List(Of MenuItem)()
        ' Example static data
        menuItems.Add(New MenuItem() With {.id = "1", .parent = "#", .text = "Root Item"})
        menuItems.Add(New MenuItem() With {.id = "2", .parent = "1", .text = "Child Item 1"})
        menuItems.Add(New MenuItem() With {.id = "3", .parent = "1", .text = "Child Item 2"})
        Return menuItems
    End Function

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Class MenuItem
        Public Property id As String
        Public Property parent As String
        Public Property text As String
    End Class
End Class
