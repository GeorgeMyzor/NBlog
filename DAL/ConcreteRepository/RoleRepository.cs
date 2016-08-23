using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using DAL.Mappers;
using ORM.Entities;

namespace DAL.ConcreteRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbContext context;

        public RoleRepository(DbContext context)
        {
            this.context = context;
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null.");
        }

        public DalRole GetById(int id)
        {
            ValidateParams(id);

            var ormRole = context.Set<Role>().FirstOrDefault(role => role.Id == id);

            return ormRole?.ToDalRole();
        }

        public DalRole GetByPredicate(Expression<Func<DalRole, bool>> expression)
        {
            var newExpr = Modifier.Convert<DalRole, Role>(expression);

            var role = context.Set<Role>().FirstOrDefault(newExpr);
            return role.ToDalRole();
        }

        public int GetRoleCost(int id)
        {
            ValidateParams(id);

            var roleCost = context.Set<RoleCosts>().FirstOrDefault(dbRoleCost => dbRoleCost.RoleId == id);

            return roleCost?.Cost ?? 0;
        }

        #region Private methods

        private void ValidateParams(int id)
        {
            if (id < 0)
            {
                //TODO logg
                throw new ArgumentOutOfRangeException(nameof(id), $"{nameof(id)} must be positive.");
            }    
        }


        #endregion
    }
}
