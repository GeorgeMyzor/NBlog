using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.Infrastructure.ModelBinders;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;

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
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([ModelBinder(typeof(UserModelBinder))] UserViewModel userViewModel)
        {
            service.CreateUser(userViewModel.ToBllUser());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var editingUser = service.GetUserEntity(id.Value).ToMvcUser();

            return View(editingUser);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel editingUser)
        {
            //TODO update in service and repo
            return View(editingUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var deletingUser = service.GetUserEntity(id.Value);

            if(deletingUser == null)
                return HttpNotFound();

            return View(deletingUser);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(BllUser deletingUser)
        {
            deletingUser = service.GetUserEntity(deletingUser.Id);
            service.DeleteUser(deletingUser);
            return RedirectToAction("Index");
        }
    }
}