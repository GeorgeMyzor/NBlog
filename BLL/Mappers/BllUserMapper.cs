using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllUserMapper
    {
        public static BllUser ToBllUser(this DalUser dalUser)
        {
            return dalUser == null ? null : new BllUser()
            {
                Id = dalUser.Id,
                Name = dalUser.Name,
                CreationDate = dalUser.CreationDate,
                Password = dalUser.Password,
                Roles = dalUser.Roles.Select(dalRole => dalRole.ToBllRole())
            };
        }

        public static DalUser ToDalUser(this BllUser bllUser)
        {
            return new DalUser()
            {
                Id = bllUser.Id,
                Name = bllUser.Name,
                CreationDate = bllUser.CreationDate,
                Password = bllUser.Password,
                Roles = bllUser.Roles.Select(bllRole => bllRole.ToDalRole())
            };
        }
    }
}
