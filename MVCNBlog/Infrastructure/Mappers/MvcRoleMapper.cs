﻿using System;
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
                case 1:
                    return new AdministratorRole() { RoleId = roleId, RoleName = roleName };
                case 2:
                    return new ModeratorRole() { RoleId = roleId, RoleName = roleName };
                case 3:
                    return new VipUserRole() { RoleId = roleId, RoleName = roleName };
                case 4:
                default:
                    return new UserRole() { RoleId = roleId, RoleName = roleName };
            }
        }

        public static IRole ToMvcRole(this int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return new AdministratorRole() { RoleId = roleId };
                case 2:
                    return new ModeratorRole() { RoleId = roleId };
                case 3:
                    return new VipUserRole() { RoleId = roleId };
                case 4:
                default:
                    return new UserRole() { RoleId = roleId };
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