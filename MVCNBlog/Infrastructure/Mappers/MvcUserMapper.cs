﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Interface.Entities;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;
using MVCNBlog.ViewModels.User;

namespace MVCNBlog.Infrastructure.Mappers
{
    public static class MvcUserMapper
    {
        #region View user
        
        public static UserViewModel ToMvcUser(this BllUser bllUser)
        {
            if (bllUser == null)
                return null;
            var roles = bllUser.Roles.Select(bllRole => bllRole.ToMvcRole()).ToList();
            return new UserViewModel()
            {
                Id = bllUser.Id,
                Email = bllUser.Email,
                Name = bllUser.Name,
                Rank = bllUser.Rank,
                CreationDate = bllUser.CreationDate,
                UserPic = bllUser.UserPic,
                Role = roles.Find((role => role.RoleId != Settings.VipRoleId)),
                PayedRole = (IPayedRole) roles.Find((role => role.RoleId == Settings.VipRoleId)),
                Articles = bllUser.Articles?.Select(article => article.ToMvcArticle())
            };
        }

        public static BllUser ToBllUser(this UserViewModel userViewModel)
        {
            var roles = new List<IRole>();
            roles.Add(userViewModel.Role);
            if(userViewModel.PayedRole != null)
                roles.Add(userViewModel.PayedRole);
            return new BllUser()
            {
                Id = userViewModel.Id,
                Email = userViewModel.Email,
                Name = userViewModel.Name,
                CreationDate = userViewModel.CreationDate,
                UserPic = userViewModel.UserPic,
                Roles = roles.Select(mvcRole => mvcRole.ToBllRole()).ToList()
            };
        }

        #endregion

        #region Create user
        
        public static BllUser ToBllUser(this RegisterUserViewModel userViewModel)
        {
            var roles = new List<IRole>();
            roles.Add(userViewModel.Role);
            return new BllUser()
            {
                Email = userViewModel.Email,
                Name = userViewModel.Name,
                Password = userViewModel.Password,
                CreationDate = userViewModel.CreationDate,
                UserPic = userViewModel.UserPic,
                Roles = roles.Select(mvcRole => mvcRole.ToBllRole()).ToList()
            };
        }

        #endregion

        #region View account

        public static AccountViewModel ToMvcAccount(this BllUser bllUser)
        {
            var roles = bllUser.Roles.Select(bllRole => bllRole.ToMvcRole()).ToList();
            return new AccountViewModel()
            {
                Id = bllUser.Id,
                Email = bllUser.Email,
                Name = bllUser.Name,
                CreationDate = bllUser.CreationDate,
                Rank = bllUser.Rank,
                SubscriptionCost = bllUser.SubscriptionCost,
                UserPic = bllUser.UserPic,
                Role = roles.Find((role => role.RoleId != Settings.VipRoleId)),
                PayedRole = (IPayedRole)roles.Find((role => role.RoleId == Settings.VipRoleId))
            };
        }

        public static BllUser ToBllUser(this AccountViewModel userViewModel)
        {
            var roles = new List<IRole>();
            roles.Add(userViewModel.Role);
            if (userViewModel.PayedRole != null)
                roles.Add(userViewModel.PayedRole);
            return new BllUser()
            {
                Id = userViewModel.Id,
                Name = userViewModel.Name,
                UserPic = userViewModel.UserPic,
                Roles = roles.Select(mvcRole => mvcRole.ToBllRole()).ToList()
            };
        }

        #endregion

    }
}