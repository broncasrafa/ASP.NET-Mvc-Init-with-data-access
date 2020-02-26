using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using DataAccess.Providers.EF.Context;
using DataAccess.Providers.EF.Interfaces;
using DataAccess.Providers.EF.UnitOfWork;
using System.Linq.Expressions;

namespace DataAccess.Providers.EF.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable where TEntity : class
    {
        private IDbSet<TEntity> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        public ApplicationDbContext Context { get; set; }

        public GenericRepository(IUnitOfWork<ApplicationDbContext> unitOfWork) : this(unitOfWork.Context)
        {
        }
        public GenericRepository(ApplicationDbContext context)
        {
            _isDisposed = false;
            Context = context;
        }

        protected virtual IDbSet<TEntity> Entities
        {
            get
            {
                return _entities ?? (_entities = Context.Set<TEntity>());
            }
        }
        public virtual IQueryable<TEntity> Table
        {
            get { return Entities; }
        }        

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Entities.ToList();
        }
        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }
        public virtual TEntity GetById(object id)
        {
            return Entities.Find(id);
        }
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }
        public virtual void Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (Context == null || _isDisposed)
                    Context = new ApplicationDbContext();

                Context.Entry(entity).State = EntityState.Modified;
            }
            catch (DbEntityValidationException dbEx)
            {
                DbEntityValidationException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public virtual void Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (Context == null || _isDisposed)
                    Context = new ApplicationDbContext();

                Entities.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                DbEntityValidationException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public virtual void Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);

                if (Context == null || _isDisposed)
                    Context = new ApplicationDbContext();
            }
            catch (DbEntityValidationException dbEx)
            {
                DbEntityValidationException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void InsertMany(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Set<TEntity>().AddRange(entities);
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                DbEntityValidationException(dbEx);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();

            _isDisposed = true;
        }
        private void DbEntityValidationException(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                }
            }
        }
    }
}
