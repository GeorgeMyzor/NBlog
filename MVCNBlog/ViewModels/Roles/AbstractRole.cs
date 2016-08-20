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

    public class Role : IRole
    {
        public virtual int RoleId { get; set; }
        public virtual string RoleName { get; set; }
    }

    public class PayedRole : Role, IPayedRole
    {
        public int Cost { get; set; }
    }
}