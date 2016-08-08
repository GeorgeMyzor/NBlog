using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService service;

        public AccountController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var editingUser = service.GetUserEntity(id.Value).ToMvcUser();

            return View(editingUser);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult ConfirmEdit(UserViewModel editingUser)
        {
            service.UpdateUser(editingUser.ToBllUser());

            return RedirectToAction("Index");
        }
    }
}