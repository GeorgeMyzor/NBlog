using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LoggingModule;
using MVCNBlog.Controllers;

namespace MVCNBlog.Infrastructure
{
    public class AdminPermissionsAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                throw new HttpException(403, "No permissions");
            }
        }
    }
}