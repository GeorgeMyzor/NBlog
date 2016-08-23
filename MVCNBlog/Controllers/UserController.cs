﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using LoggingModule;
using MVCNBlog.Infrastructure;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.Infrastructure.ModelBinders;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;
using MVCNBlog.ViewModels.User;

namespace MVCNBlog.Controllers
{
    [Authorize(Roles = "Administrator,Moderator")]
    public class UserController : Controller
    {
        private readonly IUserService service;
        private readonly ILogger logger;
        private readonly int pageSize;

        public UserController(IUserService service, ILogger logger)
        {
            this.service = service;
            this.logger = logger;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? id, string name)
        {
            var user = service.GetUserEntity(id ?? 0).ToMvcUser();

            if (user == null)
                throw new HttpException(404, "Not found");

            string urlWithName = user.Name.RemoveSpecialCharacters();
            urlWithName = Url.Encode(urlWithName);

            if (!urlWithName.Equals(name))
            {
                return RedirectToAction("Index", new { id, name = urlWithName });
            }

            return View(user);
        }

        public ActionResult All(int page = 1)
        {
            logger.Warn("test {0} test2 {1}", 1, 2);
            logger.Fatal("Test fatal");
            var users = new ListViewModel<UserViewModel>()
            {
                ViewModels = service.GetPagedUsers(page,pageSize).Select(user => user.ToMvcUser()),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = service.GetUsersCount()
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
        public ActionResult Create(RegisterUserViewModel userViewModel)
        {
            var user = service.GetUserEntity(userViewModel.Name);
            if (user != null)
                ModelState.AddModelError(nameof(RegisterUserViewModel.Name), "A user with the same name already exists");

            if (ModelState.IsValid)
            {
                userViewModel.Password = Crypto.HashPassword(userViewModel.Password);
                service.CreateUser(userViewModel.ToBllUser());
                return RedirectToAction("All");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var editingUser = service.GetUserEntity(id ?? 0)?.ToMvcUser();

            if (editingUser == null)
                throw new HttpException(404, "Not found");

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
            var deletingUser = service.GetUserEntity(id ?? 0)?.ToMvcUser();

            if (deletingUser == null)
                throw new HttpException(404, "Not found");

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

        [HttpPost]
        [AllowAnonymous]
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
                return Json("A user with the same name already exists.",
                    JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
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