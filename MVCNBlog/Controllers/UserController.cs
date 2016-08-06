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
        //[ModelBinder(typeof(UserModelBinder))]
        public ActionResult Create( UserViewModel userViewModel)
        {
            userViewModel.Roles.Add(new AdministratorRole());
            userViewModel.Roles.Add(new VipUserRole());
            service.CreateUser(userViewModel.ToBllUser());
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int userId = -1)
        {
            var deletingUser = service.GetUserEntity(userId);

            if(deletingUser == null)
                return HttpNotFound();

            return View(deletingUser);
        }

        public ActionResult ConfirmDelete(BllUser deletingUser)
        {
            service.DeleteUser(deletingUser);
            return RedirectToAction("Index");
        }
    }
}