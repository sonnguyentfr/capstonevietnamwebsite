Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports System.IO

Namespace BUH.Modules.FAQs
    Partial Class MainCustomeDisplaySpecial
        Inherits Entities.Modules.PortalModuleBase
        Private setting_vitri As String
        Private setting_template As String
        Private ReadOnly TOKEN_LIST_TOP As String = "LIST"
        Private ReadOnly TOKEN_STT As String = "[STT]"
        Private ReadOnly TOKEN_ID As String = "[ID]"
        Private ReadOnly TOKEN_QUESTION As String = "[QUESTION]"
        Private ReadOnly TOKEN_ANSWER As String = "[ANSWER]"
        Private ReadOnly TOKEN_DATE As String = "[DATE]"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                LoadData()
                'End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Function ToHTML(sTemplate As String, obj As Question_Info) As String
            Dim Question As String = obj.CauHoi
            Dim Answer As String = Server.HtmlDecode(obj.Traloi)
            Dim sDate As String = obj.CreatedDate
            Dim iId As Integer = obj.id
            sTemplate = sTemplate.Replace(TOKEN_QUESTION, Question).Replace(TOKEN_ANSWER, Answer).Replace(TOKEN_DATE, sDate).Replace(TOKEN_ID, iId)
            Return sTemplate
        End Function

        Private Sub LoadData()
            Dim isSettingNull As Boolean = LoadSetting()
            If Not isSettingNull Then
                Dim sTemplate As String = ""
                Try
                    Dim sTemplateFile As String = Convert.ToString(Server.MapPath("/Portals/0/FAQs/")) & setting_template
                    If File.Exists(sTemplateFile) Then
                        sTemplate = File.ReadAllText(sTemplateFile)
                    End If

                    'Box tach file Template                      
                    Dim sTemplate_top As String = If(sTemplate.Contains(TOKEN_LIST_TOP), TrimToken(sTemplate, TOKEN_LIST_TOP), "")
                    Dim sListTop As String = ""

                    Dim ctl As New Question_Controller()
                    'Hien thi tin theo cai dat type
                    Dim listMore = ctl._Find_Index("", "", 3, PortalSettings.PortalId, 1, 100)
                    For i As Integer = 0 To (listMore.Count - 1)
                        If i >= listMore.Count Then
                            Exit For
                        End If
                        Dim objInfo As Question_Info = DirectCast(listMore(i), Question_Info)
                        sListTop += ToHTML(sTemplate_top, objInfo)
                    Next
                    'Replace token
                    If sTemplate_top <> "" Then
                        sTemplate = sTemplate.Replace(sTemplate_top, sListTop).Replace((Convert.ToString("[") & TOKEN_LIST_TOP) + "]", "").Replace((Convert.ToString("[/") & TOKEN_LIST_TOP) + "]", "")
                    End If
                    ltContent.Text = sTemplate
                Catch ex As Exception
                    ltContent.Text = "Load module error . " + ex.Message
                End Try
            End If
        End Sub
        Private Function TrimToken(sInput As String, sToken As String) As String
            Try
                Dim sStart As String = (Convert.ToString("[") & sToken) + "]"
                Dim sEnd As String = (Convert.ToString("[/") & sToken) + "]"
                If Not sInput.Contains(sStart) OrElse Not sInput.Contains(sEnd) Then
                    Return ""
                End If

                Dim startIndex As Integer = sInput.IndexOf(sStart, StringComparison.CurrentCultureIgnoreCase) + sStart.Length
                Dim endIndex As Integer = sInput.IndexOf(sEnd, startIndex, StringComparison.CurrentCultureIgnoreCase)
                Dim length As Integer = endIndex - startIndex

                Return sInput.Substring(startIndex, length)
            Catch
                Return ""
            End Try
        End Function
        Private Function LoadSetting() As Boolean
            Dim isNull As Boolean = False
            Try
                'Template
                If Not Null.IsNull(Settings("FAQs_StyleSettings")) Then
                    setting_template = Settings("FAQs_StyleSettings")
                Else
                    isNull = True
                End If
                If isNull Then
                    Return isNull
                End If
            Catch
            End Try
            Return isNull
        End Function
    End Class
End Namespace
