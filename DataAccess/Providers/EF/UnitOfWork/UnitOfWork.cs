using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using DataAccess.Providers.EF.Repositories;

namespace DataAccess.Providers.EF.UnitOfWork
{
    /* https://dotnettutorials.net/lesson/unit-of-work-csharp-mvc/ */
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext, new()
    {
        private readonly TContext _context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
        private DbContextTransaction _objTransaction;
        private Dictionary<string, object> _repositories;

        public UnitOfWork()
        {
            _context = new TContext();
        }

        public TContext Context
        {
            get { return _context; }
        }

        public GenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            if(_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<TEntity>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<TEntity>)_repositories[type];
        }
        
        public void CreateTransaction()
        {
            _objTransaction = _context.Database.BeginTransaction();
        }
        public void Commit()
        {
            _objTransaction.Commit();
        }
        public void Rollback()
        {
            _objTransaction.Rollback();
            _objTransaction.Dispose();
        }
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                DbEntityValidationException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        private void DbEntityValidationException(DbEntityValidationException dbEx)
        {
            foreach(var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach(var validationError in validationErrors.ValidationErrors)
                {
                    _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                }
            }            
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _disposed = true;
                }
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
