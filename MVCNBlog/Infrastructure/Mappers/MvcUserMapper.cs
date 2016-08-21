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
            var roles = bllUser.Roles.Select(bllRole => bllRole.ToMvcRole()).ToList();
            return new UserViewModel()
            {
                Id = bllUser.Id,
                Name = bllUser.Name,
                Rank = bllUser.Rank,
                CreationDate = bllUser.CreationDate,
                Role = roles.Find((role => role.RoleId != 3)),
                PayedRole = (IPayedRole) roles.Find((role => role.RoleId == 3))
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
                Name = userViewModel.Name,
                CreationDate = userViewModel.CreationDate,
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
                Name = userViewModel.Name,
                Password = userViewModel.Password,
                CreationDate = userViewModel.CreationDate,
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
                Name = bllUser.Name,
                CreationDate = bllUser.CreationDate,
                Rank = bllUser.Rank,
                SubscriptionCost = bllUser.SubscriptionCost,
                Role = roles.Find((role => role.RoleId != 3)),
                PayedRole = (IPayedRole)roles.Find((role => role.RoleId == 3))
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
                CreationDate = userViewModel.CreationDate,
                Roles = roles.Select(mvcRole => mvcRole.ToBllRole()).ToList()
            };
        }

        #endregion

    }
}