using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels.Roles
{
    public interface IPayedRole : IRole
    {
        int Cost { get; set; }
    }
}