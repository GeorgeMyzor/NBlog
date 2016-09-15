using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interface.Services;
using LoggingModule;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.Providers;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.User;

namespace MVCNBlog.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService service;

        public AccountController(IAccountService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var currentAccount = service.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
            {
                var httpException = new HttpException(404, "Account wasn't found.");
                throw httpException;
            }

            return View(currentAccount);
        }

        #region Auth

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserViewModel viewModel, string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(viewModel.Email, viewModel.Password))
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, viewModel.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("All", "Article");
                }

                ModelState.AddModelError("", "Incorrect login or password.");
            }

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserViewModel viewModel)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            var anyUser = service.GetAccountEntity(viewModel.Email);

            if (anyUser != null)
            {
                ModelState.AddModelError(nameof(viewModel.Email), "User with same email already registered.");
                if (anyUser.Name == viewModel.Name)
                    ModelState.AddModelError(nameof(viewModel.Name), "User with same name already registered.");

                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                var membershipUser = ((CustomMembershipProvider) Membership.Provider)
                    .CreateUser(viewModel.Name, viewModel.Password, viewModel.Email, null, null, false, null, out status);

                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                    return RedirectToAction("All", "Article");
                }

                ModelState.AddModelError("", "Error registration.");
            }
            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Editing account

        [HttpGet]
        public ActionResult Edit()
        {
            var currentAccount = service.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
            {
                var outputString = $"{nameof(currentAccount)} wasnt found.";
                var httpException = new HttpException(404, outputString);

                throw httpException;
            }

            return View(currentAccount);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(AccountViewModel editingUser)
        {
            var user = service.GetAccountEntityByName(editingUser.Name);
            if (user != null && user.Email != User.Identity.Name)
                ModelState.AddModelError(nameof(AccountViewModel.Name), "A user with the same name already exists");

            if (ModelState.IsValid)
            {
                service.UpdateAccount(editingUser.ToBllUser());

                return RedirectToAction("Index");
            }
            var outputUser = service.GetAccountEntity(User.Identity.Name).ToMvcAccount();

            return View(outputUser);
        }

        [HttpPost]
        public ActionResult EditVipStatus(AccountViewModel editingUser)
        {
            service.UpdateAccountPaidRole(editingUser.ToBllUser());

            return RedirectToAction("Edit");
        }

        [HttpGet]
        public ActionResult UpdatePicture()
        {
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ActionName("UpdatePicture")]
        public ActionResult UpdatePictureConfirm(HttpPostedFileBase uploadImage)
        {
            if (!User.IsInRole("VipUser"))
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new {ErrorMessage = "Please buy VIP."}, JsonRequestBehavior.AllowGet);
                }

                TempData["PicError"] = "Please buy VIP.";

                return RedirectToAction("Edit");
            }

            if (Request.Files.Count > 0 && uploadImage == null)
            {
                uploadImage = Request.Files[0];
            }

            if (uploadImage != null && uploadImage.ContentLength/1024 < 200)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                var currentAccount = service.GetAccountEntity(User.Identity.Name);
                currentAccount.UserPic = imageData;

                service.UpdateAccountPicture(currentAccount);

                if (Request.IsAjaxRequest())
                {
                    var imageBytesStr = Convert.ToBase64String(imageData);
                    return Json(new {ProfilePicture = imageBytesStr}, JsonRequestBehavior.AllowGet);
                }

                return RedirectToAction("Edit");
            }
            
            if (Request.IsAjaxRequest())
            {
                return Json(new { ErrorMessage = "Inavalid image."}, JsonRequestBehavior.AllowGet);
            }

            TempData["PicError"] = "Inavalid image.";

            return RedirectToAction("Edit");
        }

        #endregion

        #region Delete account

        [HttpGet]
        public ActionResult Delete()
        {
            var currentAccount = service.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
            {
                string outputString = $"Account wasn't found.";
                var httpException = new HttpException(404, outputString);
                throw httpException;
            }

            return View(currentAccount);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete()
        {
            var currentAccount = service.GetAccountEntity(User.Identity.Name);
            if (currentAccount == null)
            {
                string outputString = $"Account wasn't found.";
                var httpException = new HttpException(404, outputString);
                throw httpException;
            }

            service.DeleteAccount(currentAccount);

            return RedirectToAction("LogOff");
        }

        #endregion
    }
}