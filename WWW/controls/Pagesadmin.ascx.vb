Namespace DesktopModules.TinTuc.Control

    Public MustInherit Class Pages
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase


#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region
        #Region "Property"
        Public Property Current() As Integer
            Get
                If Not ViewState.Item("trang") Is Nothing Then
                    Dim x As Integer = 0
                    Try
                        x = Integer.Parse(ViewState.Item("trang"))
                    Catch ex As Exception
                        x = 0
                    End Try
                    Return x
                Else
                    ViewState.Add("trang", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("trang") = Value.ToString
            End Set
        End Property
        Public Property PageIndex() As Integer
            Get
                If Not ViewState.Item("PageIndex") Is Nothing Then
                    Dim x As Integer = 1
                    Try
                        x = Integer.Parse(CType(ViewState.Item("PageIndex"), String))
                    Catch ex As Exception
                        x = 1
                    End Try
                    Return x
                Else
                    ViewState.Add("PageIndex", "1")
                    Return 1
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("PageIndex") = Value.ToString
            End Set
        End Property
        Public Property TotalPage() As Integer
            Get
                If Not ViewState.Item("TotalPage") Is Nothing Then
                    Dim x As Integer = 0
                    Try
                        x = Integer.Parse(ViewState.Item("TotalPage"))
                    Catch ex As Exception
                        x = 0
                    End Try
                    Return x
                Else
                    ViewState.Add("TotalPage", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("TotalPage") = Value.ToString
            End Set
        End Property
        Public Property TagId() As Integer
            Get
                If Not ViewState.Item("TagId") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("TagId")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("TagId", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("TagId") = Value.ToString
            End Set
        End Property
        Public Property KeySearch() As String
            Get
                If Not ViewState.Item("KeySearch") Is Nothing Then
                    Return ViewState.Item("KeySearch")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("KeySearch", value)
            End Set
        End Property
        Public Property Datefrom() As String
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return ViewState.Item("Datefrom")
                Else
                    Return BL.minDateV
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("Datefrom", value)
            End Set
        End Property
        Public Property DateTo() As String
            Get
                If Not ViewState.Item("todate") Is Nothing Then
                    Return ViewState.Item("todate")
                Else
                    Return BL.maxDateV
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("todate", value)
            End Set
        End Property
        Public Property CategoryId() As Integer
            Get
                If Not ViewState.Item("CategoryId") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("CategoryId")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("CategoryId", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CategoryId") = Value.ToString
            End Set
        End Property
#End Region

#Region "Event"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Page.IsPostBack = False Then
                    Dim pSecurity As New PortalSecurity
                    PageIndex = 1
                    If IsNumeric(Request.QueryString("trang")) Then
                        PageIndex = Integer.Parse(Request.QueryString("trang"))
                    End If
                    If IsNumeric(Request.QueryString("tag")) Then
                        TagId = Integer.Parse(Request.QueryString("tag"))
                    End If
                    If Not String.IsNullOrEmpty(Request.Item("q")) Then
                        KeySearch = pSecurity.InputFilter(Request.Item("q"), PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup)
                    End If
                    If IsNumeric(Request.Item("cid")) Then
                        CategoryId = CType(pSecurity.InputFilter(Request.Item("cid"), PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup), Integer)
                    End If
                    If IsDate(Request.Item("f")) Then
                        Datefrom = pSecurity.InputFilter(Request.Item("f"), PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup)
                    End If
                    If IsDate(Request.Item("t")) Then
                        DateTo = pSecurity.InputFilter(Request.Item("t"), PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup)
                    End If
                    Current = PageIndex - (PageIndex Mod 5) + 1
                    bindPages()
                End If

            Catch ex As Exception
            End Try
        End Sub
        Protected ReadOnly Property ParamURL(ByVal page As Integer) As String
            Get
                Dim url As String = DirectCast(HttpContext.Current.Items()("UrlRewrite:OriginalUrl"), String)
                Dim separateURL As String() = url.Split(CType("?", Char))
                If separateURL.Count() > 1 Then
                    Dim queryString As NameValueCollection = System.Web.HttpUtility.ParseQueryString(separateURL(1))
                    'queryString.Remove("page")
                    queryString("trang") = page.ToString()
                    url = separateURL(0) + "?" + queryString.ToString()
                Else
                    url = separateURL(0) + "?trang=" & page.ToString()
                End If
                
                Return url
                'Return CType((NavigateURL(PortalSettings.ActiveTab.TabID) & "?page=" & page.ToString() & IIf(TagId > 0, "&tag=" & TagId.ToString(), "")) & IIf(String.IsNullOrEmpty(KeySearch), "", "&q=" & KeySearch) & IIf(CategoryId = 0, "", "&cid=" & CategoryId) & IIf(String.IsNullOrEmpty(Datefrom) OrElse Datefrom = BL.minDateV, "", "&f=" & Datefrom) & IIf(String.IsNullOrEmpty(DateTo) OrElse DateTo = BL.maxDateV, "", "&t=" & DateTo), String)
            End Get
        End Property
        Public Sub bindPages()
            'Dim OnClientClick As String = "javascript: document.getElementById('ajax-loader').style.display='block'"
            btnPg1.Text = (Current).ToString
            btnPg1.NavigateUrl = ParamURL(Current)
            btnPg2.Text = (Current + 1).ToString
            btnPg2.NavigateUrl = ParamURL(Current + 1)
            btnPg3.Text = (Current + 2).ToString
            btnPg3.NavigateUrl = ParamURL(Current + 2)
            btnPg4.Text = (Current + 3).ToString
            btnPg4.NavigateUrl = ParamURL(Current + 3)
            btnPg5.Text = (Current + 4).ToString
            btnPg5.NavigateUrl = ParamURL(Current + 4)
            btnPg1.Visible = True
            btnPg2.Visible = True
            btnPg3.Visible = True
            btnPg4.Visible = True
            btnPg5.Visible = True


            'btnPg1.Attributes.Remove("onclick")
            'btnPg2.Attributes.Remove("onclick")
            'btnPg3.Attributes.Remove("onclick")
            'btnPg4.Attributes.Remove("onclick")
            'btnPg5.Attributes.Remove("onclick")


            btnPrevious.Visible = True
            btnPrevious.NavigateUrl = ParamURL(Current - 5)
            btnNext.Visible = True
            btnNext.NavigateUrl = ParamURL(Current + 5)
            btnLast.NavigateUrl = ParamURL(TotalPage)
            btnLast.NavigateUrl = ParamURL(TotalPage)
            btnfirst.NavigateUrl = ParamURL(1)
            If PageIndex < 5 Then
                btnfirst.Visible = False
            Else
                btnfirst.Visible = True
            End If
            'btnPrevious.Attributes.Remove("onclick")
            'btnNext.Attributes.Remove("onclick")

            If Current = 1 Then
                btnPrevious.Visible = False
                'Else
                'btnPrevious.Attributes.Add("onclick", OnClientClick)
            End If
            If Current + 4 > TotalPage Then
                btnNext.Visible = False
                'Else
                'btnNext.Attributes.Add("onclick", OnClientClick)
            End If

            If ((Current + 0) > TotalPage) Then
                btnPg1.Visible = False
                'Else
                'btnPg1.Attributes.Add("onclick", OnClientClick)
            End If

            If ((Current + 1) > TotalPage) Then
                btnPg2.Visible = False
                'Else
                'btnPg2.Attributes.Add("onclick", OnClientClick)
            End If

            If ((Current + 2) > TotalPage) Then
                btnPg3.Visible = False
                'Else
                'btnPg3.Attributes.Add("onclick", OnClientClick)
            End If
            If ((Current + 3) > TotalPage) Then
                btnPg4.Visible = False
                'Else
                'btnPg4.Attributes.Add("onclick", OnClientClick)
            End If
            If ((Current + 4) > TotalPage) Then
                btnPg5.Visible = False
                'Else
                'btnPg5.Attributes.Add("onclick", OnClientClick)
            End If

            If (Current) = PageIndex Then
                'btnPg1.Attributes.Remove("onclick")
                btnPg1.CssClass = "page-link active"
                btnPg1.Text = btnPg1.Text
            Else
                btnPg1.CssClass = "page-link"
            End If
            If (Current + 1) = PageIndex Then
                'btnPg2.Attributes.Remove("onclick")
                btnPg2.CssClass = "page-link active"
                btnPg2.Text = btnPg2.Text
            Else
                btnPg2.CssClass = "page-link"
            End If
            If (Current + 2) = PageIndex Then
                'btnPg3.Attributes.Remove("onclick")
                btnPg3.CssClass = "page-link active"
                btnPg3.Text = btnPg3.Text
            Else
                btnPg3.CssClass = "page-link"
            End If
            If (Current + 3) = PageIndex Then
                'btnPg4.Attributes.Remove("onclick")
                btnPg4.CssClass = "page-link active"
                btnPg4.Text = btnPg4.Text
            Else
                btnPg4.CssClass = "page-link"
            End If
            If (Current + 4) = PageIndex Then
                'btnPg5.Attributes.Remove("onclick")
                btnPg5.CssClass = "page-link active"
                btnPg5.Text = btnPg5.Text
            Else
                btnPg5.CssClass = "page-link"
            End If
        End Sub
#End Region

    End Class
End Namespace