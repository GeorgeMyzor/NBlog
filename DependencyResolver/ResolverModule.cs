using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Services;
using BLL.Services;
using DAL.ConcreteRepository;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using LoggingModule;
using Ninject;
using Ninject.Web.Common;
using ORM;

namespace DependencyResolver
{
    public static class ResolverModule
    {
        public static void ConfigurateResolverWeb(this IKernel kernel)
        {
            Configure(kernel, true);
        }

        private static void Configure(IKernel kernel, bool isWeb)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<DbContext>().To<EntityModel>().InRequestScope();
            
            //Services
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<IArticleService>().To<ArticleService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<IRoleService>().To<RoleService>();

            //Repos
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IArticleRepository>().To<ArticleRepository>();
            kernel.Bind<IRepository<DalComment>>().To<CommentRepository>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();

            kernel.Bind<ILogger>().To<NLogAdapter>();
        }
    }
}
