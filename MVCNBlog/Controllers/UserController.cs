using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.Infrastructure.ModelBinders;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService service;
        private readonly int pageSize;

        public UserController(IUserService service)
        {
            this.service = service;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }

        public ActionResult Index(int page = 1)
        {
            //TODO service get count
            var users = new ListViewModel<UserViewModel>()
            {
               ViewModels = service.GetAllUserEntities().Select(user => user.ToMvcUser())
                        .OrderBy((article => article.Id))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = service.GetAllUserEntities().Count()
                }
            };

            return View(users);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(CreatingUserViewModel userViewModel)
        {
            var user = service.GetUserEntity(userViewModel.Name);
            if (user != null)
                ModelState.AddModelError("Name", "A user with the same name already exists");

            if (ModelState.IsValid)
            {
                service.CreateUser(userViewModel.ToBllUser());
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var editingUser = service.GetUserEntity(id.Value)?.ToMvcUser();

            return View(editingUser);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(UserViewModel editingUser)
        {
            service.UpdateUser(editingUser.ToBllUser());

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var deletingUser = service.GetUserEntity(id.Value)?.ToMvcUser();

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

        #region Remote validation

        public JsonResult ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Json("Name should be 3 to 10 length, only letters.", JsonRequestBehavior.AllowGet);

            name = name.ToLower();

            var isValidName = Regex.IsMatch(name, @"^(?=.{3,8}$)(([a-z])\2?(?!\2))+$");
            if (!isValidName)
                return Json("Name should be 3 to 10 length, only letters.",
                    JsonRequestBehavior.AllowGet);

            var user = service.GetUserEntity(name);
            if(user != null)
                return Json("A user with the same name already exists",
                    JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidatePassword(string password)
        {
            if (password != null)
            {
                var isValidPassword = Regex.IsMatch(password, @"^(?=.{5,15}$)(?=.*[0-9])([a-zA-Z0-9])+$");
                if (!isValidPassword)
                    return Json("Password should be 5 to 15 length, at least one digit.",
                      JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}