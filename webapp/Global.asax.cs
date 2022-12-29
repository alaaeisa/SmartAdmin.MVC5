#region Using

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

#endregion

namespace SmartAdminMvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //IdentityConfig.RegisterIdentities();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var p = Request.Path.ToLower().Trim();
            if (p.EndsWith("/crystalimagehandler.aspx") && p != "/crystalimagehandler.aspx" && p.EndsWith("/reports/crystalimagehandler.aspx"))
            {
                var fullPath = Request.Url.AbsoluteUri.ToLower();
                var r = fullPath.IndexOf("/reports");
                Response.Redirect(fullPath.Remove(r, 8));
            }
        }
    }
}