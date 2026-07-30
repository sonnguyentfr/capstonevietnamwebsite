Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports System.IO
Namespace NVCMS.Modules.FAQs
    Partial Class uQuestionList
        Inherits Entities.Modules.PortalModuleBase
        Private setting_pageSize As Integer
        Private setting_template As String
        Private ReadOnly TOKEN_LIST_TOP As String = "LIST"
        Private ReadOnly TOKEN_STT As String = "[STT]"
        Private ReadOnly TOKEN_ID As String = "[ID]"
        Private ReadOnly TOKEN_USERNAME As String = "[USERNAME]"
        Private ReadOnly TOKEN_EMAIL As String = "[EMAIL]"
        Private ReadOnly TOKEN_MOBILE As String = "[PHONE]"
        Private ReadOnly TOKEN_ADDRESS As String = "[ADDRESS]"
        Private ReadOnly TOKEN_TITLE As String = "[TITLE]"
        Private ReadOnly TOKEN_QUESTION As String = "[QUESTION]"
        Private ReadOnly TOKEN_USERANSWER As String = "[USERANSWER]"
        Private ReadOnly TOKEN_ANSWER As String = "[ANSWER]"
        Private ReadOnly TOKEN_DATE As String = "[PUBLICHDATE]"
        Private ReadOnly TOKEN_CREATEDDATE As String = "[CREATEDATE]"
#Region "Control"
        Public Property TotalPage() As Integer
            Get
                If Not ViewState.Item("TotalPage") Is Nothing Then
                    Try
                        Return CInt(ViewState.Item("TotalPage"))
                    Catch ex As Exception
                        Return Null.NullInteger
                    End Try
                Else
                    ViewState.Add("TotalPage", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("TotalPage") = Value.ToString
            End Set
        End Property
        Property CurrentPage() As Integer 'Trang hiện tại
            Get
                If Not ViewState.Item("CurrentPage") Is Nothing Then
                    Return CInt(ViewState.Item("CurrentPage"))
                Else
                    ViewState.Add("CurrentPage", "1")
                    Return 1
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("CurrentPage") = value.ToString
            End Set
        End Property
        Property PageSize() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("PageSize") Is Nothing Then
                    Return CInt(ViewState.Item("PageSize"))
                Else
                    ViewState.Add("PageSize", "20")
                    Return 20
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("PageSize") = value.ToString
            End Set
        End Property
        Property TotalRecord() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("TotalRecord") Is Nothing Then
                    Return CInt(ViewState.Item("TotalRecord"))
                Else
                    ViewState.Add("TotalRecord", "0")
                    Return 0
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("TotalRecord") = value.ToString
            End Set
        End Property
#End Region
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not IsPostBack Then
                    If LoadSetting() Then
                        ltContent.Text = "Load cấu hình module lỗi !"
                        Return
                    End If

                    If Request.QueryString("page") IsNot Nothing Then
                        CurrentPage = Convert.ToInt32(Request.QueryString("page"))
                    End If
                    BindDataListQ()
                Else
                    Dim sTemp As String = Request("__EVENTARGUMENT")
                    If [String].IsNullOrEmpty(sTemp) AndAlso sTemp.StartsWith("Page_") Then
                        CurrentPage = Convert.ToInt32(sTemp.Replace("Page_", ""))
                        BindDataListQ()
                    End If
                End If
            Catch ex As Exception
                ltContent.Text = ex.Message
            End Try
        End Sub
        Private Function ToHTML(sTemplate As String, obj As uQuestion_Info) As String
            Dim sUsername As String = obj.UserName
            Dim sEmail As String = obj.Email
            Dim sPhone As String = obj.Mobile
            Dim sAdd As String = obj.Address
            Dim sTitle As String = obj.Title
            Dim Question As String = obj.Question
            Dim sUAnswer As String = obj.UAnswer
            Dim Answer As String = Server.HtmlDecode(obj.Traloi)
            Dim sDate As String = BL.FormatDate(obj.PublichDate)
            Dim sCreateDate As String = BL.FormatDate(obj.CreatedDate)
            Dim iId As Integer = obj.id
            sTemplate = sTemplate.Replace(TOKEN_ID, iId).Replace(TOKEN_USERNAME, sUsername).Replace(TOKEN_EMAIL, sEmail).Replace(TOKEN_MOBILE, sPhone).Replace(TOKEN_ADDRESS, sAdd).
                Replace(TOKEN_CREATEDDATE, sCreateDate).Replace(TOKEN_TITLE, sTitle).Replace(TOKEN_QUESTION, Question).Replace(TOKEN_USERANSWER, sUAnswer).Replace(TOKEN_ANSWER, Answer).Replace(TOKEN_DATE, sDate)
            Return sTemplate
        End Function
        Private Sub BindDataListQ()
            Dim isSettingNull As Boolean = LoadSetting()
            If Not isSettingNull Then
                Dim sTemplate As String = ""
                Try
                    Dim ctl As New uQuestion_Controller()
                    TotalRecord = ctl._Find_Count("", BL.minDateV, BL.maxDateV, "", 3, PortalSettings.PortalId)
                    Dim totalPage As Integer = If(TotalRecord Mod PageSize <> 0, (TotalRecord / PageSize + 1), (TotalRecord / PageSize))
                    If totalPage > 1 Then
                        vbPaging.TotalPage = totalPage
                        vbPaging.bindPages()
                        vbPaging.Visible = True
                    Else
                        vbPaging.Visible = False
                    End If
                    Dim sTemplateFile As String = Convert.ToString(Server.MapPath("/Portals/0/FAQs/")) & setting_template
                    If File.Exists(sTemplateFile) Then
                        sTemplate = File.ReadAllText(sTemplateFile)
                    End If
                    'Box tach file Template                      
                    Dim sTemplate_top As String = If(sTemplate.Contains(TOKEN_LIST_TOP), TrimToken(sTemplate, TOKEN_LIST_TOP), "")
                    Dim sListTop As String = ""


                    'Hien thi tin theo cai dat type
                    Dim listMore = ctl._Find_Index("", BL.minDateV, BL.maxDateV, "", 3, PortalSettings.PortalId, CurrentPage, PageSize)
                    For i As Integer = 0 To (listMore.Count - 1)
                        If i >= listMore.Count Then
                            Exit For
                        End If
                        Dim objInfo As uQuestion_Info = DirectCast(listMore(i), uQuestion_Info)
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
                'SizePageSize
                If Not Null.IsNull(ModuleConfiguration.ModuleSettings("uFAQs_PageSize")) Then
                    setting_pageSize = Convert.ToInt32(ModuleConfiguration.ModuleSettings("uFAQs_PageSize"))
                    PageSize = setting_pageSize
                Else
                    isNull = True
                End If
                If isNull Then
                    Return isNull
                End If
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
