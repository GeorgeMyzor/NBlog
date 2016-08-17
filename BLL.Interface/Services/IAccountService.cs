using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface IAccountService
    {
        BllUser GetAccountEntity(int id);
        IEnumerable<BllUser> GetAllUserEntities();
        void CreateAccount(BllUser user);
        void DeleteAccount(BllUser user);
        void UpdateAccount(BllUser user);
        void UpdateAccountPaidRole(BllUser user);
    }
}
