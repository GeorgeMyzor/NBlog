using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using ORM.Entities;

namespace DAL.Mappers
{
    public static class DalRoleMapper
    {
        public static DalRole ToDalRole(this Role ormRole)
        {
            return new DalRole
            {
                Id = ormRole.Id,
                Name = ormRole.Name,
            };
        }

        public static Role ToOrmRole(this DalRole dalRole)
        {
            return new Role
            {
                Id = dalRole.Id
            };
        }
    }
}
