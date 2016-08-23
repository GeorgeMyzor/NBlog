using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound(int? statusCode, Exception exception)
        {
            int code = statusCode ?? 404;
            Response.StatusCode = code;
            var error = new ErrorViewModel
            {
                StatusCode = statusCode.ToString(),
                StatusDescription = HttpWorkerRequest.GetStatusDescription(code),
                Message = exception?.Message ?? "Not found.",
                DateTime = DateTime.Now
            };

            return View(error);
        }

        public ActionResult NoPermissions()
        {
            return View();
        }

        public ActionResult Error(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            var error = new ErrorViewModel
            {
                StatusCode = statusCode.ToString(),
                StatusDescription = HttpWorkerRequest.GetStatusDescription(statusCode),
                Message = exception.Message,
                DateTime = DateTime.Now
            };

            return View(error);
        }
    }
}