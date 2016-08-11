using System;
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
        private readonly IRepository<DalComment> commentRepository;

        public CommentService(IUnitOfWork uow, IRepository<DalComment> repository)
        {
            this.uow = uow;
            this.commentRepository = repository;
        }

        public BllComment GetCommentEntity(int id)
        {
            return commentRepository.GetById(id).ToBllComment();
        }

        public void CreateComment(BllComment comment)
        {
            comment.PublicationDate = DateTime.Today;

            commentRepository.Create(comment.ToDalComment());
            uow.Commit();
        }

        public void DeleteComment(BllComment comment)
        {
            commentRepository.Delete(new DalComment()
            {
                Id = comment.Id
            });
            uow.Commit();
        }

        public void UpdateComment(BllComment comment)
        {
            commentRepository.Update(comment.ToDalComment());
            uow.Commit();
        }
    }
}
