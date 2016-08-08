using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels.Roles
{
    public interface IRole
    {
        int RoleId { get; set; }
        string RoleName { get; set; }
    }

    public interface IPayedRole : IRole
    {
        int Cost { get; set; }
    }

    public class UserRole : IRole
    {
        public virtual int RoleId { get; set; }
        public virtual string RoleName { get; set; }
    }

    public class VipUserRole : UserRole, IPayedRole
    {
        public override int RoleId { get; set; } = 3;
        public int Cost { get; set; } = 322;
    }

    public class ModeratorRole : UserRole
    {
        //Moder rules
    }

    public class AdministratorRole : ModeratorRole
    {
    }
}