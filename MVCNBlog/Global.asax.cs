using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MVCNBlog.Controllers;
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
            var exception = Server.GetLastError();
            Response.Clear();
            var httpException = exception as HttpException;
            if (httpException != null )
            {
                if (httpException.GetHttpCode() == 404)
                {
                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Error");
                    routeData.Values.Add("action", "Error");
                    routeData.Values.Add("exception", exception);
                    routeData.Values.Add("statusCode", 404);
                    Server.ClearError();
                    IController errorController = new ErrorController();
                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                    Response.End();
                }

                if (httpException.GetHttpCode() == 403)
                {
                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Error");
                    routeData.Values.Add("action", "Error");
                    routeData.Values.Add("exception", exception);
                    routeData.Values.Add("statusCode", 403);
                    Server.ClearError();
                    IController errorController = new ErrorController();
                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
            else
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Error");
                routeData.Values.Add("exception", exception);
                routeData.Values.Add("statusCode", 500);

                Response.TrySkipIisCustomErrors = true;
                IController controller = new ErrorController();
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Response.End();
            }
        }
    }
}
