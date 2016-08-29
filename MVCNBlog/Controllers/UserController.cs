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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? id, string name)
        {
            var user = service.GetUserEntity(id ?? 0).ToMvcUser();

            if (user == null)
            {
                string outputString = $"{nameof(user)} wasnt found.";
                var httpException = new HttpException(404, outputString);

                throw httpException;
            }

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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterUserViewModel userViewModel)
        {
            var user = service.GetUserEntityByEmail(userViewModel.Email);
            if (user != null)
            {
                if(user.Name == userViewModel.Name)
                    ModelState.AddModelError(nameof(RegisterUserViewModel.Name), "A user with the same name already exists");
                ModelState.AddModelError(nameof(RegisterUserViewModel.Email), "A user with the same email already exists");
            }

            if (ModelState.IsValid)
            {
                userViewModel.Password = Crypto.HashPassword(userViewModel.Password);

                string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/no-profile-img.gif");
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                Image image = Image.FromStream(stream);
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    userViewModel.UserPic = ms.ToArray();
                }

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
            {
                string outputString = $"{nameof(editingUser)} wasn't found.";
                var httpException = new HttpException(404, outputString);

                throw httpException;
            }

            return View(editingUser);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(UserViewModel editingUser)
        {
            if (ModelState.IsValidField(nameof(editingUser.Name)) && ModelState.IsValidField(nameof(editingUser.Role)))
            {
                service.UpdateUser(editingUser.ToBllUser());

                return RedirectToAction("Index");
            }

            var outUser = service.GetUserEntity(editingUser.Id)?.ToMvcUser();

            return View(outUser);
        }

        [HttpGet]
        public ActionResult UpdatePicture()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdatePicture(int? id, HttpPostedFileBase uploadImage)
        {
            if (Request.Files.Count > 0 && uploadImage == null)
            {
                uploadImage = Request.Files[0];
            }

            if(id == null)
                throw new HttpException(404, "");

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

            TempData["PicError"] = "Inavalid image.";

            return RedirectToAction("Edit", id);
        }

        public ActionResult Delete(int? id)
        {
            var deletingUser = service.GetUserEntity(id ?? 0)?.ToMvcUser();

            if (deletingUser == null)
            {
                string outputString = $"{nameof(deletingUser)} wasn't found.";
                var httpException = new HttpException(404, outputString);

                throw httpException;
            }

            return View(deletingUser);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(BllUser deletingUser)
        {
            deletingUser = service.GetUserEntity(deletingUser.Id);
            service.DeleteUser(deletingUser);

            return RedirectToAction("All");
        }

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