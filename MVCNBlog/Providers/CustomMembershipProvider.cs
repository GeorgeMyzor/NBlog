﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using BLL.Interface.Entities;
using BLL.Interface.Services;

namespace MVCNBlog.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public IUserService UserService
            => (IUserService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof (IUserService));
        public IRoleService RoleService
            => (IRoleService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof (IRoleService));

        public MembershipUser CreateUser(string email, string password)
        {
            MembershipUser membershipUser = GetUser(email, false);

            if (membershipUser != null)
            {
                return null;
            }

            var user = new BllUser
            {
                Email = email,
                Password = Crypto.HashPassword(password),
                CreationDate = DateTime.Now
            };

            var role = RoleService.GetRoleEntity("User");
            if (role != null)
            {
                user.Roles = new List<BllRole>()
                {
                    role
                };
            }

            //TODO static method?
            string path = HttpContext.Current.Server.MapPath("~/Content/no-profile-img.gif");
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Image image = Image.FromStream(stream);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                user.UserPic = ms.ToArray();
            }

            UserService.CreateUser(user);
            membershipUser = GetUser(email, false);
            return membershipUser;
        }

        public override bool ValidateUser(string email, string password)
        {
            var user = UserService.GetUserEntityByEmail(email);

            if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
            {
                return true;
            }
            return false;
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            var user = UserService.GetUserEntityByEmail(email);

            if (user == null) return null;

            var memberUser = new MembershipUser("CustomMembershipProvider", user.Name,
                null, user.Email, null, null,
                false, false, user.CreationDate,
                DateTime.MinValue, DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue);

            return memberUser;
        }
        
        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipUser membershipUser = GetUser(email, false);

            if (membershipUser != null)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var user = new BllUser
            {
                Email = email,
                Name = username,
                Password = Crypto.HashPassword(password),
                CreationDate = DateTime.Now
            };

            var role = RoleService.GetRoleEntity("User");
            if (role != null)
            {
                user.Roles = new List<BllRole>()
                {
                    role
                };
            }

            string path = HttpContext.Current.Server.MapPath("~/Content/no-profile-img.gif");
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Image image = Image.FromStream(stream);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                user.UserPic = ms.ToArray();
            }

            UserService.CreateUser(user);
            membershipUser = GetUser(email, false);
            status = MembershipCreateStatus.Success;

            return membershipUser;
        }

        #region Stabs

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}