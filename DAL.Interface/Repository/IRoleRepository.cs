using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;

namespace DAL.Interface.Repository
{
    public interface IRoleRepository
    {
        DalRole GetById(int key);
        DalRole GetByPredicate(Expression<Func<DalRole, bool>> f);
        int GetRoleCost(int roleId);
    }
}
