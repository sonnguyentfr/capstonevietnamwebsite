Imports DotNetNuke.Web.Api

Namespace NVCMS.Modules.HeThong.Api

    ''' <summary>Đăng ký route Web API: /DesktopModules/NVCMSHeThong/API/{controller}/{action}</summary>
    Public Class ServiceRouteMapper
        Implements IServiceRouteMapper

        Public Sub RegisterRoutes(mapRouteManager As IMapRoute) Implements IServiceRouteMapper.RegisterRoutes
            mapRouteManager.MapHttpRoute(
                "NVCMSHeThong",
                "default",
                "{controller}/{action}",
                New String() {"NVCMS.Modules.HeThong.Api"})
        End Sub

    End Class

End Namespace
