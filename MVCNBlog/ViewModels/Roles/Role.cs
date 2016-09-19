using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels.Roles
{
    public class Role : IRole
    {
        public virtual int RoleId { get; set; }
        public virtual string RoleName { get; set; }
    }
}