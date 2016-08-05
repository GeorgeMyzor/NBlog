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
        public static DalUser ToDalUser(this BllUser userEntity)
        {
            return new DalUser()
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                CreationDate = userEntity.CreationDate
            };
        }

        public static BllUser ToBllUser(this DalUser dalUser)
        {
            return new BllUser()
            {
                Id = dalUser.Id,
                Name = dalUser.Name,
                CreationDate = dalUser.CreationDate
            };
        }
    }
}
