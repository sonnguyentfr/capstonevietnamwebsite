Imports System.IO
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Framework
Namespace DesktopModules.TinTuc.Control
    ''' <summary>
    ''' Trang chủ quản trị (skin _admin/Index.ascx): Phân tích Marketing từ Google Analytics 4.
    ''' Control chỉ in khung HTML; số liệu do API /DesktopModules/NVCMS/API/MarketingAnalytics/ (NVCMS.API) trả về,
    ''' khóa GA4 ở máy chủ (Cấu hình website › Google Analytics). Tên lớp giữ như cũ để Index.ascx không phải sửa.
    ''' </summary>
    Partial Class ThongkeNhuanButUser
        Inherits PortalModuleBase

        Private Const AssetFolder As String = "~/DesktopModules/NVCMS.Marketing/Analytics/"

        Protected ReadOnly Property ApiRoot() As String
            Get
                Return ResolveUrl("~/DesktopModules/NVCMS/API/MarketingAnalytics/")
            End Get
        End Property

        ''' <summary>URL tài nguyên kèm ?v= thời điểm sửa tệp (sửa JS / CSS là trình duyệt tải lại).</summary>
        Protected Function AssetUrl(ByVal fileName As String) As String
            Dim url As String = ResolveUrl(AssetFolder & fileName)
            Try
                Dim path As String = Server.MapPath(AssetFolder & fileName)
                If File.Exists(path) Then url &= "?v=" & File.GetLastWriteTimeUtc(path).Ticks.ToString("x")
            Catch ex As Exception
                ' không đọc được tệp -> không gắn phiên bản
            End Try
            Return System.Web.HttpUtility.HtmlAttributeEncode(url)
        End Function

        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            MyBase.OnInit(e)
            Try
                ' sinh input __RequestVerificationToken cho API (ValidateAntiForgeryToken)
                ServicesFramework.Instance.RequestAjaxAntiForgerySupport()
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
    End Class
End Namespace
