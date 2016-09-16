﻿using System;
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
    public class CommentRepository : ICommentRepository
    {
        private readonly DbContext context;

        public CommentRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null.");
            this.context = context;
        }
        
        public int GetCount(string userName = null)
        {
            return userName == null
                ? context.Set<Comment>().Count()
                : context.Set<Comment>().Count(comment => comment.Author.Name == userName);
        }

        #region Read

        public IEnumerable<DalComment> GetAll()
        {
            return
                context.Set<Comment>()
                    .OrderBy(comment => comment.PublicationDate)
                    .Select(comment => comment.ToDalComment());
        }

        public IEnumerable<DalComment> GetArticleComments(int articleId)
        {
            var comments = context.Set<Comment>().Where(comment => comment.ArticleId == articleId).
                OrderByDescending(comment => comment.PublicationDate).ToList();

            return comments.Select(ormComment => ormComment.ToDalComment());
        }

        public DalComment GetById(int id)
        {
            ValidateId(id);

            var ormComment = context.Set<Comment>().FirstOrDefault(comment => comment.Id == id);

            return ormComment.ToDalComment();
        }
        
        public DalComment GetByPredicate(Expression<Func<DalComment, bool>> expression)
        {
            var newExpr = Modifier.Convert<DalComment, Comment>(expression);

            var comment = context.Set<Comment>().FirstOrDefault(newExpr);
            return comment?.ToDalComment();
        }

        #endregion

        public void Create(DalComment dalComment)
        {
            ValidateComment(dalComment);

            var ormComment = dalComment.ToOrmComment();

            ormComment.Author = context.Set<User>().SingleOrDefault((user => user.Id == dalComment.AuthorId));
            ormComment.Article = context.Set<Article>().Single((article => article.Id == dalComment.ArticleId));

            context.Set<Comment>().Add(ormComment);
            context.SaveChanges();
        }

        public void Delete(DalComment dalComment)
        {
            ValidateComment(dalComment);

            var ormComment = context.Set<Comment>().Single(u => u.Id == dalComment.Id);
            context.Set<Comment>().Remove(ormComment);
        }

        public void Update(DalComment dalComment)
        {
            ValidateComment(dalComment);

            var ormComment = context.Set<Comment>().Single(u => u.Id == dalComment.Id);

            ormComment.Content = dalComment.Content;
        }
        
        #region Private methods
        
        private static void ValidateComment(DalComment dalComment)
        {
            if (dalComment == null)
            {
                throw new ArgumentNullException(nameof(dalComment), $"{nameof(dalComment)} is null.");
            }
        }

        private static void ValidateId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"{nameof(id)} must be positive.");
            }
        }
        #endregion
    }
}
