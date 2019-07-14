using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Heroic.AutoMapper;

namespace MvcTemplate
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacConfig.Register();

            //https://github.com/MattHoneycutt/HeroicFramework/tree/master/Heroic.AutoMapper
            HeroicAutoMapperConfigurator.LoadMapsFromCallerAndReferencedAssemblies();
        }
    }
}
