using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;

namespace MVCNBlog.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            return View(service.GetAllUserEntities().Select(user => user.ToMvcUser()));
        }

    }
}