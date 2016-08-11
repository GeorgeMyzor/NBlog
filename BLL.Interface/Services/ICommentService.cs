﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface ICommentService
    {
        BllComment GetCommentEntity(int id);
        void CreateComment(BllComment comment);
        void DeleteComment(BllComment comment);
        void UpdateComment(BllComment comment);
    }
}
