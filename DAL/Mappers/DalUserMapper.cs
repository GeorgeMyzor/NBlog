using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using ORM.Entities;

namespace DAL.Mappers
{
    public static class DalUserMapper
    {
        public static DalUser ToDalUser(this User ormUser)
        {
            return new DalUser
            {
                Id = ormUser.Id,
                Name = ormUser.Name,
                CreationDate = ormUser.CreationDate,
                Roles = ormUser.Roles.Select(ormRole => ormRole.ToDalRole())
            };
        }

        public static User ToOrmUser(this DalUser dalUser)
        {
            return new User
            {
                Id = dalUser.Id,
                Name = dalUser.Name,
                CreationDate = dalUser.CreationDate,
                Roles = dalUser.Roles.Select(dalRole => dalRole.ToOrmRole()).ToList()
            };
        }
    }
}
