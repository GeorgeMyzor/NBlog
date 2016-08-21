using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MVCNBlog.Infrastructure.ModelBinders;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.User;

namespace MVCNBlog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(UserViewModel), new UserModelBinder());
            ModelBinders.Binders.Add(typeof(RegisterUserViewModel), new UserModelBinder());
            ModelBinders.Binders.Add(typeof(LoginUserViewModel), new UserModelBinder());
            ModelBinders.Binders.Add(typeof(AccountViewModel), new AccountModelBinder());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Server.ClearError();
            Response.Redirect("/Error/NotFound");
        }
    }
}
