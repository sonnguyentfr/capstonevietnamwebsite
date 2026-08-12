Imports NVCMS.Modules.School

Namespace DesktopModules.TinTuc.Control

    Partial Class ChonTruong
        Inherits System.Web.UI.Page

        Private Const PAGE_SIZE As Integer = 60
        Private _schoolCtl As New MarketingSchoolController()

        Private Property CurrentPage() As Integer
            Get
                If ViewState("SchoolPage") IsNot Nothing Then
                    Return CInt(ViewState("SchoolPage"))
                End If
                Return 1
            End Get
            Set(value As Integer)
                ViewState("SchoolPage") = value
            End Set
        End Property

        Private Property SearchKeyword() As String
            Get
                If ViewState("SchoolKeyword") IsNot Nothing Then
                    Return CStr(ViewState("SchoolKeyword"))
                End If
                Return ""
            End Get
            Set(value As String)
                ViewState("SchoolKeyword") = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            If Not IsPostBack Then
                LoadSchools()
            End If
        End Sub

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs)
            SearchKeyword = txtSearch.Text.Trim()
            CurrentPage = 0
            LoadSchools()
        End Sub

        Protected Sub lbtPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
            If CurrentPage > 0 Then
                CurrentPage -= 1
                LoadSchools()
            End If
        End Sub

        Protected Sub lbtNext_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim total As Integer = GetTotal()
            Dim totalPages As Integer = Math.Max(1, CInt(Math.Ceiling(total / PAGE_SIZE)))
            If CurrentPage < totalPages - 1 Then
                CurrentPage += 1
                LoadSchools()
            End If
        End Sub

        Private Function GetTotal() As Integer
            Return _schoolCtl.Marketing_Truong_Find_Count(SearchKeyword, "", -1, -1, 0)
        End Function

        Private Sub LoadSchools()
            Dim keyword As String = SearchKeyword
            txtSearch.Text = keyword

            Dim total As Integer = GetTotal()
            Dim totalPages As Integer = Math.Max(1, CInt(Math.Ceiling(total / PAGE_SIZE)))

            lblTotal.Text = total.ToString()
            lblCurPage.Text = CurrentPage.ToString()
            lblTotalPage.Text = totalPages.ToString()

            lbtPrev.Enabled = (CurrentPage > 0)
            lbtNext.Enabled = (CurrentPage < totalPages - 1)

            Dim lst = _schoolCtl.Marketing_Truong_Find_Index(keyword, "", -1, -1, 0, CurrentPage, PAGE_SIZE)

            If lst Is Nothing OrElse lst.Count = 0 Then
                rptSchools.DataSource = Nothing
                rptSchools.DataBind()
                ltrEmpty.Text = "<p class=""info-row"" style=""padding:14px;text-align:center;color:#999"">Không tìm thấy bản ghi nào.</p>"
            Else
                ltrEmpty.Text = ""
                rptSchools.DataSource = lst
                rptSchools.DataBind()
            End If
        End Sub

    End Class

End Namespace

