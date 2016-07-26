using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-EN");
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-EN");
        }
    }
}
