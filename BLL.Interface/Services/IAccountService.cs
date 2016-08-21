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
        BllUser GetAccountEntity(string name);
        IEnumerable<BllUser> GetAllUserEntities();
        void DeleteAccount(BllUser user);
        void UpdateAccount(BllUser user);
        void UpdateAccountPaidRole(BllUser user);
    }
}
