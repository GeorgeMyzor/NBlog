using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Exceptions;
using DAL.Interface.Repository;

namespace DAL.ConcreteRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; private set; }

        public UnitOfWork(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null." );
            Context = context;
        }

        public void Commit()
        {
            try
            {
                Context?.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new UnitOfWorkException("Failed to update db", ex);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ArgumentException("Failed to add entity", ex);
            }
            catch (SqlException ex)
            {
                throw new UnitOfWorkException("Db not working.", ex);
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}

