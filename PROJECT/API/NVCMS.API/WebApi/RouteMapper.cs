using DotNetNuke.Web.Api;

namespace NVCMS.API.WebApi
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute routeManager)
        {
            routeManager.MapHttpRoute(
                "NVCMS",
                "default",
                "{controller}/{action}",
                new[] { "NVCMS.API.Controller" }
            );
        }
    }
}