using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ModelBinderTesting.App_Start;

namespace ModelBinderTesting
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}