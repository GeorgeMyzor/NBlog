using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVCNBlog.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext ctx)
        {
            if (!ctx.RequestContext.HttpContext.User.Identity.IsAuthenticated)
                base.HandleUnauthorizedRequest(ctx);
            else
            {
                ctx.RequestContext.HttpContext.Response.StatusCode = 403;
                ctx.RequestContext.HttpContext.Response.Status = "Forbidden";
                ctx.RequestContext.HttpContext.Response.StatusDescription = "Forbidden";
                ctx.RequestContext.HttpContext.Response.End();
                ctx.RequestContext.HttpContext.Response.Close();
            }
        }
    }
}