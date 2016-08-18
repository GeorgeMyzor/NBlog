using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.Providers;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.User;

namespace MVCNBlog.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService service;

        public AccountController(IAccountService service)
        {
            this.service = service;
        }

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
                    //TODO remember me
                    FormsAuthentication.SetAuthCookie(viewModel.Name, true);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password.");
                }
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
            var anyUser = service.GetAllUserEntities().Any(u => u.Name.Contains(viewModel.Name));

            if (anyUser)
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Error registration.");
                }
            }
            return View(viewModel);
        }
        
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var editingAccount = service.GetAccountEntity(id.Value).ToMvcAccount();

            return View(editingAccount);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            var editingAccount = service.GetAccountEntity(id.Value).ToMvcAccount();

            return View(editingAccount);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(AccountViewModel editingUser)
        {
            service.UpdateAccount(editingUser.ToBllUser());

            return RedirectToAction("Index", new { editingUser.Id });
        }
        
        [HttpPost]
        public ActionResult EditVipStatus(AccountViewModel editingUser)
        {
            service.UpdateAccountPaidRole(editingUser.ToBllUser());

            return RedirectToAction("Index", new { editingUser.Id});
        }
    }
}