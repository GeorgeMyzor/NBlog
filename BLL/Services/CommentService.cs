﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using BLL.Mappers;
using DAL.Interface.DTO;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork uow;
        private readonly ICommentRepository commentRepository;

        public CommentService(IUnitOfWork uow, ICommentRepository repository)
        {
            this.uow = uow;
            this.commentRepository = repository;
        }

        public int GetCommentsCount(string userName)
        {
            return commentRepository.GetCount(userName);
        }

        public BllComment GetCommentEntity(int id)
        {
            return commentRepository.GetById(id).ToBllComment();
        }

        public IEnumerable<BllComment> GetArticleComments(int articleId)
        {
            return commentRepository.GetArticleComments(articleId).Select(comment => comment.ToBllComment());
        }
        
        public void CreateComment(BllComment comment)
        {
            comment.PublicationDate = DateTime.Now;

            commentRepository.Create(comment.ToDalComment());
            uow.Commit();
        }

        public void DeleteComment(BllComment comment)
        {
            commentRepository.Delete(comment.ToDalComment());
            uow.Commit();
        }

        public void UpdateComment(BllComment comment)
        {
            commentRepository.Update(comment.ToDalComment());
            uow.Commit();
        }
    }
}
