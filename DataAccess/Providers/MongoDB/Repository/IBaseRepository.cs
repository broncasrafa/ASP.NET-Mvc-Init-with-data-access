using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DataAccess.Providers.MongoDB.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : EntityBase
    {        
        void InsertOne(TEntity entity);
        void InsertMany(IList<TEntity> entities);
        void UpdateOne(Guid id, TEntity entity);
        void UpdateMany(IList<TEntity> entities);
        void DeleteOne(TEntity entity);
        void DeleteMany(IList<TEntity> entities);
        void Delete(ObjectId id);        
        TEntity Save(TEntity entity);
        IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        IList<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        IList<TEntity> GetAll();
        TEntity GetById(ObjectId id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        bool DropCollection();

        #region Async
        Task InsertOneAsync(TEntity entity);
        Task InsertManyAsync(IList<TEntity> entities);
        Task UpdateOneAsync(Guid id, TEntity entity);
        Task UpdateManyAsync(IList<TEntity> entities);
        Task DeleteOneAsync(TEntity entity);
        Task DeleteManyAsync(IList<TEntity> entities);
        Task DeleteAsync(ObjectId id);
        Task<TEntity> SaveAsync(TEntity entity);
        Task<IList<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(ObjectId id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DropCollectionAsync();
        #endregion
    }
}
