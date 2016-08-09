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

            var editingAccount = service.GetUserEntity(id.Value).ToMvcAccount();

            return View(editingAccount);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var editingAccount = service.GetUserEntity(id.Value).ToMvcAccount();

            return View(editingAccount);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(AccountViewModel editingUser)
        {
            service.UpdateUser(editingUser.ToBllUser());

            return RedirectToAction("Index", new { editingUser.Id });
        }
        
        [HttpPost]
        public ActionResult EditVipStatus(AccountViewModel editingUser)
        {
            service.UpdateUserPaidRole(editingUser.ToBllUser());

            return RedirectToAction("Index", new { editingUser.Id});
        }
    }
}