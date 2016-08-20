using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Interface.Entities;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.Infrastructure.Mappers
{
    public static class MvcRoleMapper
    {
        public static IRole ToMvcRole(this BllRole bllRole)
        {
            int roleId = bllRole.Id;
            string roleName = bllRole.Name;
            switch (roleId)
            {
                case 3:
                    return new PayedRole() { RoleId = roleId, RoleName = roleName };
                default:
                    return new Role() { RoleId = roleId, RoleName = roleName };
            }
        }

        public static IRole ToMvcRole(this int roleId)
        {
            switch (roleId)
            {
                case 3:
                    return new PayedRole() { RoleId = roleId };
                default:
                    return new Role() { RoleId = roleId };
            }
        }

        public static BllRole ToBllRole(this IRole mvcRole)
        {
            return new BllRole
            {
                Id = mvcRole.RoleId
            };
        }
    }
}