using System;
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
    public class CommentRepository : IRepository<DalComment>
    {
        private readonly DbContext context;

        public CommentRepository(DbContext uow)
        {
            this.context = uow;
        }
        
        public int GetCount(string userName = null)
        {
            return userName == null
                ? context.Set<Comment>().Count()
                : context.Set<Comment>().Count(comment => comment.Author.Name == userName);
        }

        public IEnumerable<DalComment> GetAll()
        {
            return context.Set<Comment>().ToList().Select(ormComment => ormComment.ToDalComment());
        }

        public DalComment GetById(int id)
        {
            var ormComment = context.Set<Comment>().FirstOrDefault(comment => comment.Id == id);

            return ormComment.ToDalComment();
        }

        public DalComment GetByPredicate(Expression<Func<DalComment, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalComment dalComment)
        {
            var ormComment = dalComment.ToOrmComment();

            ormComment.Author = context.Set<User>().SingleOrDefault((user => user.Id == dalComment.AuthorId));
            ormComment.Article = context.Set<Article>().SingleOrDefault((article => article.Id == dalComment.ArticleId));

            context.Set<Comment>().Add(ormComment);
        }

        public void Delete(DalComment dalComment)
        {
            var ormComment = context.Set<Comment>().Single(u => u.Id == dalComment.Id);
            context.Set<Comment>().Remove(ormComment);
        }

        public void Update(DalComment dalComment)
        {
            var ormComment = context.Set<Comment>().Single(u => u.Id == dalComment.Id);

            ormComment.Content = dalComment.Content;
        }
    }
}
