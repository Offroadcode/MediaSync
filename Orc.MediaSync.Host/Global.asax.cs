using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Orc.MediaSync.Host
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Version ServerVersion { get; private set; }
        protected void Application_Start()
        {
            ServerVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }
    }
}
