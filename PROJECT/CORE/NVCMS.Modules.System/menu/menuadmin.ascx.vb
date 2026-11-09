Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Framework
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.Exceptions

Namespace NVCMS.Modules.HeThong

    ''' <summary>
    ''' Phần chung của các control trong DesktopModules/NVCMS.HeThong: đường dẫn tài nguyên, API, quyền,
    ''' token chống CSRF cho các lệnh POST.
    ''' </summary>
    Public MustInherit Class HeThongControlBase
        Inherits PortalModuleBase

        ''' <summary>Đổi số này mỗi khi sửa JS/CSS để trình duyệt tải lại.</summary>
        Public Const AssetVersion As String = "1.0.0"

        Protected ReadOnly Property AssetRoot As String
            Get
                Return ResolveUrl("~/DesktopModules/NVCMS.HeThong/")
            End Get
        End Property

        Protected ReadOnly Property ApiRoot As String
            Get
                Return ResolveUrl("~/DesktopModules/NVCMSHeThong/API/")
            End Get
        End Property

        ''' <summary>Có quyền Edit trên module và là Administrators/SuperUser (API kiểm tra lại phía server).</summary>
        Protected ReadOnly Property CanEdit As Boolean
            Get
                Return ModulePermissionController.CanEditModuleContent(ModuleConfiguration) AndAlso IsMenuAdmin
            End Get
        End Property

        Private ReadOnly Property IsMenuAdmin As Boolean
            Get
                Return UserInfo IsNot Nothing AndAlso (UserInfo.IsSuperUser OrElse UserInfo.IsInRole(PortalSettings.AdministratorRoleName))
            End Get
        End Property

        Protected Shared Function Attr(value As Object) As String
            Return HttpUtility.HtmlAttributeEncode(Convert.ToString(value))
        End Function

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            Try
                ' sinh input __RequestVerificationToken cho các lệnh POST
                ServicesFramework.Instance.RequestAjaxAntiForgerySupport()
            Catch ex As Exception
                Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

    End Class

    ''' <summary>
    ''' Khung giao diện quản trị menu (DesktopModules/NVCMS.HeThong/menu/menuadmin.ascx).
    ''' Dữ liệu lấy qua Web API Menu (Scripts/menu-admin.js); control chỉ truyền ngữ cảnh module.
    ''' </summary>
    Public Class menuadmin
        Inherits HeThongControlBase
    End Class

End Namespace
