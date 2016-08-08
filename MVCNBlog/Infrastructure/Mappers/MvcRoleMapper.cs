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
        //TODO mb config?
        private const string AdminRole = "Administrator";
        private const string ModerRole = "Moderator";
        private const string VipUserRole = "VipUser";
        private const string UserRole = "User";

        public static IRole ToMvcRole(this BllRole bllRole)
        {
            return bllRole.Id.ToMvcRole();
        }

        public static IRole ToMvcRole(this int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return new AdministratorRole() { RoleId = roleId, RoleName = AdminRole };
                case 2:
                    return new ModeratorRole() { RoleId = roleId, RoleName = ModerRole };
                case 3:
                    return new VipUserRole() { RoleId = roleId, RoleName = VipUserRole };
                case 4:
                default:
                    return new UserRole() { RoleId = roleId, RoleName = UserRole };
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