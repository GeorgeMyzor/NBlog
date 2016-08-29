using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using MVCNBlog.Infrastructure.Mappers;

namespace MVCNBlog.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public IUserService UserService
            => (IUserService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUserService));
        public IRoleService RoleService
            => (IRoleService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IRoleService));

        public override bool IsUserInRole(string name, string roleName)
        {
            throw new NotImplementedException();
            var user = UserService.GetAllUserEntities().FirstOrDefault(u => u.Name == name).ToMvcUser();

            if (user == null) return false;

            BllRole userRole = RoleService.GetRoleEntity(user.Role.RoleId);

            if (userRole != null && userRole.Name == roleName)
            {
                return true;
            }

            return false;
        }

        //for logon user. ^ for any
        public override string[] GetRolesForUser(string email)
        {

            var user = UserService.GetAllUserEntities().FirstOrDefault(u => u.Email == email).ToMvcUser();

            var userRole = user.Role;
            var userPayedRole = user.PayedRole;

            var roles = GetRolesName(userRole?.RoleName, userPayedRole?.RoleName);

            return roles;
        }

        private string[] GetRolesName(params string[] roles)
        {
            return roles.Where(role => role != null).ToArray();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}