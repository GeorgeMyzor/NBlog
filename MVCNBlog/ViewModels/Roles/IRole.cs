using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCNBlog.ViewModels.Roles
{
    public interface IRole
    {
        int RoleId { get; set; }
        string RoleName { get; set; }
    }
}
