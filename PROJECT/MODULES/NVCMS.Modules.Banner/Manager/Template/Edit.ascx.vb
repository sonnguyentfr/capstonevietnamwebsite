Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.HeThong
Namespace NVCMS.Modules.Banner
    Partial Class EditTemplate
        Inherits Entities.Modules.PortalModuleBase
#Region "Propertice"
        Dim controller As New TemplateController
        Private Shared ReadOnly VietNamChar As String() = New String() {"aeouidy", "áàạảãâấầậẩẫăắằặẳẵ", "éèẹẻẽêếềệểễ", "óòọỏõôốồộổỗơớờợởỡ", "úùụủũưứừựửữ", "íìịỉĩ", "đ", "ýỳỵỷỹ"}
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
        Public Property folderPath() As String
            Get
                If Not ViewState.Item("folderPath") Is Nothing Then
                    Return CType(ViewState.Item("folderPath"), String)
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("folderPath", value)
            End Set
        End Property
        Public Property PhotoVirPath() As String
            Get
                If Not ViewState.Item("PhotoVirPath") Is Nothing Then
                    Return CType(ViewState.Item("PhotoVirPath"), String)
                Else
                    Return nvcmsBL.GetImagePath(True, PortalId, True)
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("PhotoVirPath", value)
            End Set
        End Property
#End Region
#Region "Function"

        ''' <summary>
        ''' Chả biết để làm gì không.
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        Public Shared Function TrimVietnamesChar(ByVal str As String) As String
            Dim tg As String = str.ToLower().Trim()

            For i As Integer = 1 To VietNamChar.Length - 1

                For j As Integer = 0 To VietNamChar(i).Length - 1
                    tg = tg.Replace(VietNamChar(i)(j), VietNamChar(0)(i - 1))
                Next
            Next

            Return tg
        End Function
        ''' <summary>
        ''' Mr Dòi
        ''' Băm chuỗi để link cho đẹp
        ''' </summary>
        ''' <param name="strInput"></param>
        ''' <returns></returns>
        Public Shared Function ToUrlFriendly(ByVal strInput As String) As String
            Dim strTitle As String = strInput.Trim()
            strTitle = strTitle.Trim("-"c)
            Dim chars As Char() = "–$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray()
            strTitle = strTitle.Replace("c#", "C-Sharp")
            strTitle = strTitle.Replace("vb.net", "VB-Net")
            strTitle = strTitle.Replace("asp.net", "Asp-Net")
            strTitle = TrimVietnamesChar(strTitle)
            strTitle.Replace(".", "-")

            For i As Integer = 0 To chars.Length - 1
                Dim strChar As String = chars.GetValue(i).ToString()

                If strTitle.Contains(strChar) Then
                    strTitle = strTitle.Replace(strChar, "-")
                End If
            Next

            strTitle = strTitle.Replace(" ", "-")
            strTitle = strTitle.Replace("--", "-")
            strTitle = strTitle.Replace("---", "-")
            strTitle = strTitle.Replace("----", "-")
            strTitle = strTitle.Replace("-----", "-")
            strTitle = strTitle.Replace("-----", "-")
            strTitle = strTitle.Replace("---", "-")
            strTitle = strTitle.Replace("--", "-")
            strTitle = strTitle.Trim()
            strTitle = strTitle.Trim("-"c)
            Return strTitle
        End Function
#End Region
#Region "pageLoad"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    folderPath = Server.MapPath("/Portals/0/TemplateBanner/") 'PortalSettings.HomeDirectoryMapPath + "TemplateBanner"
                    If Request.Item("itemid") <> "" Then
                        ItemID = CInt(Request.Item("itemid"))
                        BindData(ItemID)
                    Else
                        lbtDel.Visible = False
                    End If
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region
#Region "Bind dataa"
        Private Sub BindData(id As Integer)
            Dim template As TemplateInfo = controller._GetByID(id, PortalId)
            If Not template Is Nothing Then
                With template
                    Title.Value = template.TemplateName
                    txtFilePath.Value = PortalSettings.HomeDirectory & "TemplateBanner/" & template.FilePath
                    If File.Exists(folderPath & "/" & template.FilePath) Then
                        txtcode.Value = File.ReadAllText(folderPath & "/" & template.FilePath)
                        hdf_textcode.Value = File.ReadAllText(folderPath & "/" & template.FilePath)
                    Else
                        txtcode.Value = "File template không tồn tại. Vui lòng cập nhật lại !"
                    End If
                End With
            End If

        End Sub

#End Region
#Region "action"
        Private Function addnews() As Boolean
            Try
                If Not Directory.Exists(folderPath) Then
                    Directory.CreateDirectory(folderPath)
                End If

                Dim id As Integer = If(Request.QueryString("itemid") IsNot Nothing, Convert.ToInt32(Request.QueryString("itemid")), -1)
                Dim template As TemplateInfo = (If(id = -1, New TemplateInfo(), controller._GetByID(id, PortalId)))
                template.TemplateName = Title.Value
                Dim fileName As String = ToUrlFriendly(template.TemplateName & ".html")
                template.PortalId = PortalId

                If id = -1 Then
                    template.FilePath = fileName
                    controller._Insert(template.TemplateName, template.FilePath, template.PortalId)
                    File.WriteAllText(folderPath & "/" & template.FilePath, hdf_textcode.Value)
                Else
                    File.WriteAllText(folderPath & "/" & template.FilePath, hdf_textcode.Value)
                    controller._Update(id, template.TemplateName, template.FilePath, template.PortalId)
                End If
                Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                Return False
            End Try
        End Function
        Private Sub lbtCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtCancel.Click
            Try
                controller._Delete(ItemID, PortalId)
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub lbtDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtDel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub lbtUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
            If addnews() Then
                Response.Redirect(NavigateURL(), True)
            End If
        End Sub
        Private Sub clearcontroldata()
            Me.Title.Value = ""
        End Sub
#End Region

    End Class
End Namespace