using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;

namespace DAL.Interface.Repository
{
    public interface IUserRepository : IRepository<DalUser>
    {
        int GetUserActivity(int id);
        void UpdatePaidRole(DalUser user);
        void UpdateUserPicture(DalUser user);
        IEnumerable<DalUser> GetPagedUsers(int pageNum, int pageSize);
    }
}
