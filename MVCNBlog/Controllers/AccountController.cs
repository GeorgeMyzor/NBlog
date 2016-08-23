using System;
using System.Collections.Generic;
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
        private readonly ILogger logger;

        public AccountController(IAccountService service, ILogger logger)
        {
            this.logger = logger;
            this.service = service;
        }

        #region Auth

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(viewModel.Name, viewModel.Password))
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Name, viewModel.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("All", "Article");
                }

                logger.Info($"Can not login with name: {viewModel.Name}");
                ModelState.AddModelError("", "Incorrect login or password.");
            }
            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserViewModel viewModel)
        {
            var anyUser = service.GetAccountEntity(viewModel.Name);

            if (anyUser != null)
            {
                ModelState.AddModelError("", "User with this address already registered.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                var membershipUser = ((CustomMembershipProvider)Membership.Provider)
                    .CreateUser(viewModel.Name, viewModel.Password);

                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Name, false);
                    return RedirectToAction("All", "Article");
                }
                logger.Info($"Can not register with name: {viewModel.Name}");
                ModelState.AddModelError("", "Error registration.");
            }
            return View(viewModel);
        }
        
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            var currentAccount = service.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
            {
                var httpException = new HttpException(404, "Not found");
                logger.Warn(httpException, $"{nameof(currentAccount)} wasnt found.");
                throw httpException;
            }

            return View(currentAccount);
        }

        #region Editing account

        [HttpGet]
        public ActionResult Edit()
        {
            var currentAccount = service.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null) if (currentAccount == null)
                {
                    var httpException = new HttpException(404, "Not found");
                    logger.Warn(httpException, $"{nameof(currentAccount)} wasnt found.");
                    throw httpException;
                }

            return View(currentAccount);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(AccountViewModel editingUser)
        {
            service.UpdateAccount(editingUser.ToBllUser());

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult EditVipStatus(AccountViewModel editingUser)
        {
            service.UpdateAccountPaidRole(editingUser.ToBllUser());

            return RedirectToAction("Index");
        }

        #endregion

        [HttpGet]
        public ActionResult Delete()
        {
            var currentAccount = service.GetAccountEntity(User.Identity.Name).ToMvcAccount();
            if (currentAccount == null)
            {
                var httpException = new HttpException(404, "Not found");
                logger.Warn(httpException, $"{nameof(currentAccount)} wasnt found.");
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
                var httpException = new HttpException(404, "Not found");
                logger.Warn(httpException, $"{nameof(currentAccount)} wasnt found.");
                throw httpException;
            }

            service.DeleteAccount(currentAccount);

            return RedirectToAction("LogOff");
        }
    }
}