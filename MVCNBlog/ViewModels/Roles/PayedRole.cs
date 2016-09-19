using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels.Roles
{
    public sealed class PayedRole : Role, IPayedRole
    {
        public int Cost { get; set; }
    }
}