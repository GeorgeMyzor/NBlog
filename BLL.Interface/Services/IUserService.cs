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
        int GetUsersCount();
        BllUser GetUserEntity(int id);
        BllUser GetUserEntity(string name);
        BllUser GetUserByEmail(string email);
        IEnumerable<BllUser> GetAllUserEntities();
        IEnumerable<BllUser> GetPagedUsers(int pageNum, int pageSize);
        void CreateUser(BllUser user);
        void DeleteUser(BllUser user);
        void UpdateUser(BllUser user);
        void UpdateUserPicture(BllUser user);
    }
}
