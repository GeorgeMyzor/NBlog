using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Interface.Entities;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Mappers
{
    public static class MvcUserMapper
    {
        public static UserViewModel ToMvcUser(this BllUser bllUser)
        {
            return new UserViewModel()
            {
                Id = bllUser.Id,
                Name = bllUser.Name,
                CreationDate = bllUser.CreationDate,
                Roles = bllUser.Roles.Select(bllRole => bllRole.ToMvcRole()).ToList()
            };
        }

        public static BllUser ToBllUser(this UserViewModel userViewModel)
        {
            return new BllUser()
            {
                Id = userViewModel.Id,
                Name = userViewModel.Name,
                Roles = userViewModel.Roles.Select(mvcRole => mvcRole.ToBllRole()).ToList()
            };
        }
    }
}