using Lands.Backend.Helpers;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Lands.Backend
{
    public class MvcApplication : System.Web.HttpApplication
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

            CheckRolesAndSuperUser();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");
            UsersHelper.CheckSuperUser();
        }
    }
}
