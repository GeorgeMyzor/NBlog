﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllCommentMapper
    {
        public static DalComment ToDalComment(this BllComment commentEntity)
        {
            return new DalComment()
            {
                Id = commentEntity.Id,
                Content = commentEntity.Content,
                Author = commentEntity.Author.ToDalUser()
            };
        }

        public static BllComment ToBllComment(this DalComment dalComment)
        {
            return new BllComment()
            {
                Id = dalComment.Id,
                Content = dalComment.Content,
                Author = dalComment.Author.ToBllUser()
            };
        }
    }
}