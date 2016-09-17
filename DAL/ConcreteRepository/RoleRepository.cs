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
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null.");
            this.context = context;
        }

        public DalRole GetById(int id)
        {
            ValidateParams(id);

            var ormRole = context.Set<Role>().FirstOrDefault(role => role.Id == id);

            return ormRole?.ToDalRole();
        }

        /// <summary>
        /// Return role by predicate <param name="expression"></param>
        /// </summary>
        /// <param name="expression">Rule according to which will find out a role.</param>
        /// <returns>Role.</returns>
        public DalRole GetByPredicate(Expression<Func<DalRole, bool>> expression)
        {
            var newExpr = Modifier.Convert<DalRole, Role>(expression);

            var role = context.Set<Role>().FirstOrDefault(newExpr);
            return role.ToDalRole();
        }

        /// <summary>
        /// Gets role cost by role <param name="id">. If role is payed role cost will be greater then 0.</param>
        /// </summary>
        /// <param name="id">Id of role.</param>
        /// <returns>Role cost. 0 if not a payed role.</returns>
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
                throw new ArgumentOutOfRangeException(nameof(id), $"{nameof(id)} must be positive.");
            }    
        }
        
        #endregion
    }
}
