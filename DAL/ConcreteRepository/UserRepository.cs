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
    public class UserRepository : IRepository<DalUser>
    {
        private readonly DbContext context;

        public UserRepository(DbContext uow)
        {
            this.context = uow;
        }

        public IEnumerable<DalUser> GetAll()
        {
            return context.Set<User>().ToList().Select(ormUser => ormUser.ToDalUser());
        }

        public DalUser GetById(int key)
        {
            var ormUser = context.Set<User>().FirstOrDefault(user => user.Id == key);

            return ormUser.ToDalUser();
        }

        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> f)
        {
            //Expression<Func<DalUser, bool>> -> Expression<Func<User, bool>> (!)
            throw new NotImplementedException();
        }

        public void Create(DalUser dalUser)
        {
            var ormUser = dalUser.ToOrmUser();
            List<Role> newRoles = new List<Role>();

            foreach (var role in ormUser.Roles)
            {
                var dbRole = context.Set<Role>().Find(role.Id);
                newRoles.Add(dbRole);
            }
            ormUser.Roles = newRoles;
            context.Set<User>().Add(ormUser);
        }

        public void Delete(DalUser dalUser)
        {
            var ormUser = dalUser.ToOrmUser();

            ormUser = context.Set<User>().Single(u => u.Id == ormUser.Id);
            context.Set<User>().Remove(ormUser);
        }

        public void Update(DalUser dalUser)
        {
            var editingUser = dalUser.ToOrmUser();
            var ormUser = context.Set<User>().Single(u => u.Id == dalUser.Id);

            ormUser.Name = editingUser.Name;
            var vipRole = ormUser.Roles.Find((role => role.Id == 3));
            if (vipRole != null)
                editingUser.Roles.Add(vipRole);
            ormUser.Roles.Clear();
            
            foreach (var role in editingUser.Roles)
            {
                var dbRole = context.Set<Role>().Find(role.Id);
                ormUser.Roles.Add(dbRole);
            }
        }
    }
}
