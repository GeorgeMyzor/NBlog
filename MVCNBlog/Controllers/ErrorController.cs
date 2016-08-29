using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoggingModule;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger logger;
        public ErrorController(ILogger logger)
        {
            this.logger = logger;
        }

        public ActionResult Error(int? statusCode, Exception exception)
        {
            int code;
            if (statusCode == null || string.IsNullOrEmpty(exception.Message))
            {
                code = 404;
                exception = new HttpException(code, "Page not found.");
                logger.Warn(exception.Message);
            }
            else if(statusCode == 500)
            {
                logger.Error(exception);
                code = 500;
                exception = new HttpException(code, "Server error.");
            }
            else
            {
                logger.Warn(exception.Message);
                code = (int)statusCode;
            }

            Response.StatusCode = code;
            var error = new ErrorViewModel
            {
                StatusCode = code.ToString(),
                StatusDescription = HttpWorkerRequest.GetStatusDescription(code),
                Message = FirstCharToUpper(exception?.Message),
                DateTime = DateTime.Now
            };

            return View(error);
        }

        private static string FirstCharToUpper(string errorString)
        {
            if (string.IsNullOrEmpty(errorString))
                return "Page not found.";
            return errorString.First().ToString().ToUpper() + errorString.Substring(1);
        }
    }
}