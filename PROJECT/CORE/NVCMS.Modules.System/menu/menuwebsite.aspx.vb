Imports System.Xml

Public Class menuwebsite1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function GetMenuData() As String
        Dim xmlFilePath As String = HttpContext.Current.Server.MapPath("~/App_Data/menu.xml")
        Dim xmlDoc As New XmlDocument()
        xmlDoc.Load(xmlFilePath)
        Return xmlDoc.OuterXml
    End Function
End Class