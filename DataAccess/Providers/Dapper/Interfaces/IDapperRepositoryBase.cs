using System.Collections.Generic;

namespace DataAccess.Providers.Dapper.Interfaces
{
    public interface IDapperRepositoryBase<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        int Add(TEntity entity);
        int AddRange(IEnumerable<TEntity> collection);
        void Update(TEntity entity);
        void Remove(int id);
    }
}
