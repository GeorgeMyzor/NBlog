using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    [CustomAuthorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserService service;
        private readonly int pageSize;

        public UserController(IUserService service)
        {
            this.service = service;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }

        #region View

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? id, string name)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var user = service.GetUserEntity(id.Value).ToMvcUser();

            if (user == null)
                throw new HttpException(404, $"{nameof(user)} wasnt found.");

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
            var totalItems = service.GetUsersCount() - 1;
            if (page > (totalItems + pageSize - 1) / pageSize)
                throw new HttpException(404, "");

            var users = new ListViewModel<UserViewModel>()
            {
                ViewModels = service.GetPagedUsers(page,pageSize).Select(user => user.ToMvcUser())
                .SkipWhile(user => user.Email == User.Identity.Name),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                }
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView(users);
            }

            return View(users);
        }

        #endregion

        #region Create

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterUserViewModel newUser)
        {
            var duplicateEmailUser = service.GetUserEntityByEmail(newUser.Email);
            if (duplicateEmailUser != null)
                ModelState.AddModelError(nameof(RegisterUserViewModel.Email), "A user with the same email already exists");

            var duplicateNameUser = service.GetUserEntity(newUser.Name);
            if (duplicateNameUser != null)
                ModelState.AddModelError(nameof(RegisterUserViewModel.Name), "A user with the same name already exists");

            if (ModelState.IsValid)
            {
                newUser.UserPic = Settings.GetDefaultProfilePicture();

                service.CreateUser(newUser.ToBllUser());
                return RedirectToAction("All");
            }

            return View();
        }

        #endregion

        #region Edit

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var editingUser = service.GetUserEntity(id.Value)?.ToMvcUser();

            if (editingUser == null)
                throw new HttpException(404, $"{nameof(editingUser)} wasn't found.");

            return View(editingUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel editingUser)
        {
            if (ModelState.IsValidField(nameof(editingUser.Name)) && ModelState.IsValidField(nameof(editingUser.Role)))
            {
                service.UpdateUser(editingUser.ToBllUser());

                return RedirectToAction("Index");
            }

            var outUser = service.GetUserEntity(editingUser.Id)?.ToMvcUser();

            return View("Index", outUser);
        }

        [HttpGet]
        public ActionResult UpdatePicture()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePicture(int? id, HttpPostedFileBase uploadImage)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            if (Request.Files.Count > 0 && uploadImage == null)
            {
                uploadImage = Request.Files[0];
            }
            
            if (uploadImage != null && uploadImage.ContentLength / 1024 < 200)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                var currentUser = service.GetUserEntity(id.Value);
                currentUser.UserPic = imageData;

                service.UpdateUserPicture(currentUser);

                if (Request.IsAjaxRequest())
                {
                    var imageBytesStr = Convert.ToBase64String(imageData);
                    return Json(new { ProfilePicture = imageBytesStr }, JsonRequestBehavior.AllowGet);
                }

                return View("Edit", currentUser.ToMvcUser());
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { ErrorMessage = "Inavalid image." }, JsonRequestBehavior.AllowGet);
            }

            TempData["PicError"] = "Either image not found or image size too big.";

            return RedirectToAction("Edit", id);
        }

        #endregion

        #region Delete

        public ActionResult Delete(int? id)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var deletingUser = service.GetUserEntity(id.Value)?.ToMvcUser();

            if (deletingUser == null)
                throw new HttpException(404, $"{nameof(deletingUser)} wasn't found.");

            return View(deletingUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(BllUser deletingUser)
        {
            deletingUser = service.GetUserEntity(deletingUser.Id);

            if(deletingUser == null)
                throw new HttpException(404, "User not found.");

            service.DeleteUser(deletingUser);

            return RedirectToAction("All");
        }

        #endregion

        #region Remote validation

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ValidateName(string name)
        {
            if (!Request.IsAjaxRequest())
                throw new HttpException(404, "Page not found.");

            var user = service.GetUserEntity(name);
            if(user != null && user.Email != User.Identity.Name)
                return Json("A user with the same name already exists.",
                    JsonRequestBehavior.AllowGet);

            return ValidateLoginName(name);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ValidateLoginName(string name)
        {
            if (!Request.IsAjaxRequest())
                throw new HttpException(404, "Page not found.");

            if (string.IsNullOrEmpty(name))
                return Json("Name should be 3 to 15 length, only letters.", JsonRequestBehavior.AllowGet);

            name = name.ToLower();

            var isValidName = Regex.IsMatch(name, @"^(?=.{3,15}$)(([a-z])\2?(?!\2))+$");
            if (!isValidName)
                return Json("Name should be 3 to 15 length, only letters.",
                    JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ValidateEmail(string email)
        {
            if (!Request.IsAjaxRequest())
                throw new HttpException(404, "Page not found.");

            var user = service.GetUserEntityByEmail(email);
            if (user != null && user.Email != User.Identity.Name)
                return Json("User with the same email already exists.",
                    JsonRequestBehavior.AllowGet);

            return ValidateLoginEmail(email);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ValidateLoginEmail(string email)
        {
            if (!Request.IsAjaxRequest())
                throw new HttpException(404, "Page not found.");

            var errorString = "Email should be 5 to 30 length.";

            if (string.IsNullOrEmpty(email))
                return Json("", JsonRequestBehavior.AllowGet);

            email = email.ToLower();

            var isValidName = Regex.IsMatch(email,
                @"^(?=.{5,30}$)[a-z0-9!#$%&'*+=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (!isValidName)
                return Json(errorString,
                    JsonRequestBehavior.AllowGet);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ValidatePassword(string password)
        {
            if (!Request.IsAjaxRequest())
                throw new HttpException(404, "Page not found.");

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