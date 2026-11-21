<%@ WebHandler Language="VB" Class="doi.upload.SchoolNames" %>

Imports System
Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports NVCMS.Modules.School

Namespace doi.upload

    ''' <summary>
    ''' Tra ve danh sach truong (id, ten, slug, logo) dang JSON de man soan tin tu nhan dien ten truong trong bai.
    ''' Chi doc, chi cho nguoi dung da dang nhap, cache 10 phut. Khong ghi du lieu.
    ''' Dinh dang: [{"i":12,"n":"Ten truong","s":"ten-truong","l":"duong-dan-logo"}]
    ''' </summary>
    Public Class SchoolNames : Implements IHttpHandler

        Private Const CACHE_KEY As String = "NE_SchoolNames_Json"

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            context.Response.ContentType = "application/json"
            context.Response.ContentEncoding = Encoding.UTF8
            Try
                Dim userId As Integer = DotNetNuke.Entities.Users.UserController.Instance.GetCurrentUserInfo().UserID
                If userId <= 0 Then
                    context.Response.StatusCode = 401
                    context.Response.Write("[]")
                    Return
                End If

                Dim json As String = TryCast(HttpRuntime.Cache(CACHE_KEY), String)
                If json Is Nothing Then
                    json = BuildJson()
                    HttpRuntime.Cache.Insert(CACHE_KEY, json, Nothing, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration)
                End If
                context.Response.Write(json)
            Catch ex As Exception
                context.Response.StatusCode = 500
                context.Response.Write("[]")
            End Try
        End Sub

        Private Shared Function BuildJson() As String
            Dim ctl As New MarketingSchoolController()
            Dim total As Integer = ctl.Marketing_Truong_Find_Count("", "", -1, -1, 0)
            If total < 1 Then total = 1
            Dim size As Integer = Math.Min(Math.Max(total, 200), 5000)
            Dim lst As ArrayList = ctl.Marketing_Truong_Find_Index("", "", -1, -1, -1, 1, size)

            Dim sb As New StringBuilder()
            sb.Append("[")
            Dim first As Boolean = True
            If lst IsNot Nothing Then
                For Each o As Object In lst
                    Dim s As MarketingSchoolInfo = TryCast(o, MarketingSchoolInfo)
                    If s Is Nothing Then Continue For
                    Dim name As String = If(s.NameofSchool, "").Trim()
                    If name.Length < 4 OrElse s.id <= 0 Then Continue For
                    If Not first Then sb.Append(",")
                    first = False
                    sb.Append("{""i"":").Append(s.id.ToString(CultureInfo.InvariantCulture))
                    sb.Append(",""n"":""").Append(JsonEsc(name)).Append("""")
                    sb.Append(",""s"":""").Append(ToSlug(name)).Append("""")
                    sb.Append(",""l"":""").Append(JsonEsc(Ultis.FormatThumbImage(If(s.Logo, ""), 60, 40, "", "", "", ""))).Append("""}")
                Next
            End If
            sb.Append("]")
            Return sb.ToString()
        End Function

        ''' <summary>Cung quy tac voi SlugHelper.ToSlug cua site (bo dau, d thuong, chi giu a-z0-9, noi bang dau gach).</summary>
        Private Shared Function ToSlug(ByVal input As String) As String
            If String.IsNullOrWhiteSpace(input) Then Return ""
            Dim sb As New StringBuilder()
            For Each c As Char In input
                If c = ChrW(273) OrElse c = ChrW(272) Then
                    sb.Append("d"c)
                Else
                    sb.Append(c)
                End If
            Next
            Dim norm As String = sb.ToString().Normalize(NormalizationForm.FormD)
            Dim clean As New StringBuilder()
            For Each c As Char In norm
                If CharUnicodeInfo.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark Then clean.Append(c)
            Next
            Return Regex.Replace(clean.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant(), "[^a-z0-9]+", "-").Trim("-"c)
        End Function

        Private Shared Function JsonEsc(ByVal s As String) As String
            If s Is Nothing Then Return ""
            Dim sb As New StringBuilder()
            For Each c As Char In s
                Select Case c
                    Case """"c : sb.Append("\""")
                    Case "\"c : sb.Append("\\")
                    Case ChrW(10) : sb.Append("\n")
                    Case ChrW(13) : sb.Append("\r")
                    Case ChrW(9) : sb.Append("\t")
                    Case Else
                        If AscW(c) < 32 Then
                            sb.Append("\u").Append(AscW(c).ToString("x4"))
                        Else
                            sb.Append(c)
                        End If
                End Select
            Next
            Return sb.ToString()
        End Function

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
End Namespace
