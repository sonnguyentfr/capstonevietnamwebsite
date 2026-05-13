Imports System.IO
Imports System.Security.Cryptography
Imports DotNetNuke.UI.Utilities
Namespace NVCMS.Modules.Marketing
    Public MustInherit Class Account
        Inherits Entities.Modules.PortalModuleBase

#Region "Controls"
        Dim _Marketing_Mail_AccountController As New Marketing_Mail_AccountController
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
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    BindGridData()
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region

        Private Sub BindGridData()
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = _Marketing_Mail_AccountController._GetAll()
            'Me.ltrcount.Text = arrNewsCategories.Count
            'Me.rptlistacc.DataSource = arrNewsCategories
            'Me.rptlistacc.DataBind()
        End Sub
#Region "edit insert"
        Protected Sub GetInfo(sender As Object, e As EventArgs)
            'ItemID = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            'Dim objMarketing_Mail_AccountInfo As Marketing_Mail_AccountInfo
            'objMarketing_Mail_AccountInfo = _Marketing_Mail_AccountController._GetByID(ItemID)
            'If Not objMarketing_Mail_AccountInfo Is Nothing Then
            '    With objMarketing_Mail_AccountInfo
            '        lbtDelete.Visible = True
            '        Me.txtMail.Text = .Mail
            '        txtName.Text = .Name
            '        txtPass.Text = .Password
            '    End With

            'End If
            ClientAPI.RegisterStartUpScript(Me.Page, "Modalhoatdong", "<script>Modalhoatdong();</script>")
        End Sub

        'Private Sub lbtUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
        '    Try
        '        'If ItemID > 0 Then
        '        '    'Edit
        '        '    _Marketing_Mail_AccountController._Update(ItemID, Me.txtName.Text, txtMail.Text, Encrypt(Me.txtPass.Text.Trim()), UserId, PortalId)
        '        'Else
        '        '    _Marketing_Mail_AccountController._Insert(Me.txtName.Text, txtMail.Text, Encrypt(Me.txtPass.Text.Trim()), UserId, PortalId)
        '        'End If
        '        'Me.txtMail.Text = ""
        '        'Me.txtName.Text = ""
        '        'Me.txtPass.Text = ""
        '        ItemID = 0
        '        BindGridData()
        '        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "ModalFollowUpClose();", True)
        '        ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật thành công!');</script>")
        '    Catch exc As Exception    'Module failed to load
        '        ProcessModuleLoadException(Me, exc)
        '    End Try
        'End Sub
        'Protected Sub lbtDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtDelete.Click
        '    If ItemID > 0 Then
        '        _Marketing_Mail_AccountController._Delete(ItemID)
        '        ItemID = 0
        '        BindGridData()
        '        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "ModalFollowUpClose();", True)
        '        ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Xóa dữ liệu thành công!');</script>")
        '    End If
        'End Sub
        'Private Sub lbtAdd_Click(sender As Object, e As EventArgs) Handles lbtAdd.Click, lbtAddTop.Click
        '    Me.txtMail.Text = ""
        '    Me.txtName.Text = ""
        '    Me.txtPass.Text = ""
        '    Me.lbtDelete.Visible = False
        '    ItemID = 0
        '    ClientAPI.RegisterStartUpScript(Me.Page, "Modalhoatdong", "<script>Modalhoatdong();</script>")
        'End Sub
        Public Function Encrypt(clearText As String) As String
            Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
            Using encryptor As Aes = Aes.Create()
                Dim pdb As New Rfc2898DeriveBytes("SonNguyenCapStone8C", New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
                 &H65, &H64, &H76, &H65, &H64, &H65,
                 &H76})
                encryptor.Key = pdb.GetBytes(32)
                encryptor.IV = pdb.GetBytes(16)
                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                        cs.Write(clearBytes, 0, clearBytes.Length)
                        cs.Close()
                    End Using
                    clearText = Convert.ToBase64String(ms.ToArray())
                End Using
            End Using
            Return clearText
        End Function
#End Region
    End Class

End Namespace
