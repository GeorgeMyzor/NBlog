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
        public static UserViewModel ToMvcUser(this BllUser userEntity)
        {
            return new UserViewModel()
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                CreationDate = userEntity.CreationDate
            };
        }

        public static BllUser ToBllUser(this UserViewModel userViewModel)
        {
            return new BllUser()
            {
                Id = userViewModel.Id,
                Name = userViewModel.Name,
                CreationDate = userViewModel.CreationDate
            };
        }
    }
}