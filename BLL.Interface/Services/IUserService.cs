using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface IUserService
    {
        BllUser GetUserEntity(int id);
        BllUser GetUserEntity(string name);
        IEnumerable<BllUser> GetAllUserEntities();
        void CreateUser(BllUser user);
        void DeleteUser(BllUser user);
        void UpdateUser(BllUser user);
    }
}
