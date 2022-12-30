#region Using

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace SmartAdminMvc
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("Report/{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("CrystalImageHandler.aspx /{ *pathInfo}");
            routes.LowercaseUrls = true;
            //routes.MapRoute("Default", "{controller}/{action}/{id}", new
            //{
            //    controller = "Home",
            //    action = "Index",
            //    controller = "Users",
            //    action = "Userlogin",
            //    id = UrlParameter.Optional
            //});


            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                //controller = "Home",
                //action = "Index",
                controller = "Users",
                action = "Userlogin",
                id = UrlParameter.Optional
            }).RouteHandler = new DashRouteHandler();
        }
    }
}