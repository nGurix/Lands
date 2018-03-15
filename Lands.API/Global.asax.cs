using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Lands.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static string AdminUser;
        public static string AdminPassWord;
        public static string SMTPName;
        public static string SMTPPort;

        protected void Application_Start()
        {
            AdminUser = ConfigurationManager.AppSettings["AdminUser"];
            AdminPassWord = ConfigurationManager.AppSettings["AdminPassWord"];
            SMTPName = ConfigurationManager.AppSettings["SMTPName"];
            SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
