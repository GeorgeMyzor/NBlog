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
                Email = ormUser.Email,
                Name = ormUser.Name,
                CreationDate = ormUser.CreationDate,
                Password = ormUser.Password,
                UserPic = ormUser.UserPic,
                Roles = ormUser.Roles.Select(ormRole => ormRole.ToDalRole())
            };
        }

        public static User ToOrmUser(this DalUser dalUser)
        {
            return new User
            {
                Id = dalUser.Id,
                Email = dalUser.Email,
                Name = dalUser.Name,
                CreationDate = dalUser.CreationDate,
                Password = dalUser.Password,
                UserPic = dalUser.UserPic,
                Roles = dalUser.Roles.Select(dalRole => dalRole.ToOrmRole()).ToList()
            };
        }
    }
}
