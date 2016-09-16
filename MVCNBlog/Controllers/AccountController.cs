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
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var currentAccount = accountService.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
                throw new HttpException(404, "Account wasn't found.");

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
        public ActionResult Login(LoginUserViewModel loginUser, string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginUser.Email, loginUser.Password))
                {
                    FormsAuthentication.SetAuthCookie(loginUser.Email, loginUser.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("All", "Article");
                }

                ModelState.AddModelError("", "Incorrect login or password.");
            }

            return View(loginUser);
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
        public ActionResult Register(RegisterUserViewModel registerUser)
        {
            if(IsDuplicateUser(registerUser))
                return View(registerUser);
            
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                var membershipUser = ((CustomMembershipProvider) Membership.Provider)
                    .CreateUser(registerUser.Name, registerUser.Password, registerUser.Email, null, null, false, null, out status);
                
                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(registerUser.Email, false);
                    return RedirectToAction("All", "Article");
                }

                ModelState.AddModelError("", "Error registration. " + status);
            }

            return View(registerUser);
        }
        
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }


        [NonAction]
        private bool IsDuplicateUser(RegisterUserViewModel registerUser)
        {
            var anyEmailUser = accountService.GetAccountEntity(registerUser.Email);
            var anyNameUser = accountService.GetAccountEntityByName(registerUser.Name);

            if (anyEmailUser != null)
            {
                ModelState.AddModelError(nameof(registerUser.Email), "User with same email already registered.");

                return true;
            }

            if (anyNameUser != null)
            {
                ModelState.AddModelError(nameof(registerUser.Name), "User with same name already registered.");

                return true;
            }

            return false;
        }

        #endregion

        #region Edit account

        [HttpGet]
        public ActionResult Edit()
        {
            var currentAccount = accountService.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
                throw new HttpException(404, $"{nameof(currentAccount)} wasnt found.");

            return View(currentAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountViewModel editingAccount)
        {
            var user = accountService.GetAccountEntityByName(editingAccount.Name);
            if (user != null && user.Email != User.Identity.Name)
                ModelState.AddModelError(nameof(AccountViewModel.Name), "A user with the same name already exists");

            var currentAccount = accountService.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (ModelState.IsValidField("Name") && editingAccount.Role.RoleId == currentAccount.Role.RoleId)
            {
                accountService.UpdateAccount(editingAccount.ToBllUser());

                return RedirectToAction("Index");
            }

            return View(currentAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVipStatus(AccountViewModel editingUser)
        {
            accountService.UpdateAccountPaidRole(editingUser.ToBllUser());

            return RedirectToAction("Edit");
        }

        [HttpGet]
        public ActionResult UpdatePicture()
        {
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePicture(HttpPostedFileBase uploadImage)
        {
            if (!User.IsInRole("VipUser"))
            {
                string buyVipMessage = "Please buy VIP.";
                if (Request.IsAjaxRequest())
                {
                    return Json(new {ErrorMessage = buyVipMessage }, JsonRequestBehavior.AllowGet);
                }

                TempData["PicError"] = buyVipMessage;

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

                var currentAccount = accountService.GetAccountEntity(User.Identity.Name);
                currentAccount.UserPic = imageData;

                accountService.UpdateAccountPicture(currentAccount);

                if (Request.IsAjaxRequest())
                {
                    var imageBytesStr = Convert.ToBase64String(imageData);
                    return Json(new {ProfilePicture = imageBytesStr}, JsonRequestBehavior.AllowGet);
                }

                return RedirectToAction("Edit");
            }

            string imageErrorString = "Either image not found or size too big.";
            if (Request.IsAjaxRequest())
            {
                return Json(new { ErrorMessage = imageErrorString }, JsonRequestBehavior.AllowGet);
            }

            TempData["PicError"] = imageErrorString;

            return RedirectToAction("Edit");
        }

        #endregion

        #region Delete account

        [HttpGet]
        public ActionResult Delete()
        {
            var currentAccount = accountService.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
                throw new HttpException(404, "Account wasn't found.");

            return View(currentAccount);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete()
        {
            var currentAccount = accountService.GetAccountEntity(User.Identity.Name);
            if (currentAccount == null)
                throw new HttpException(404, "Account wasn't found.");

            accountService.DeleteAccount(currentAccount);

            return RedirectToAction("LogOff");
        }

        #endregion
    }
}