Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Portals
Imports NVCMS.Modules.TinTuc
Imports DotNetNuke.Common.Utilities
Imports aejw.Network
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography
Imports System.Web
Imports System.Web.UI.WebControls
Imports NVCMS.Modules.LibCRM

Public Class BLSchool
    Public Shared _LoaiTruongController As New LoaiTruongController
#Region "Search Toolbox"
    Public Shared Sub Search_BindLoaiTruongShow(PortalId As Integer, ByVal ddl As DropDownList)
        Dim arrStudentStatus As New ArrayList
        arrStudentStatus = _LoaiTruongController.LoaiTruong_GetAllShow(PortalId)
        ddl.DataSource = arrStudentStatus
        ddl.DataValueField = "id"
        ddl.DataTextField = "Loaitruong"
        ddl.DataBind()
    End Sub
#End Region
End Class