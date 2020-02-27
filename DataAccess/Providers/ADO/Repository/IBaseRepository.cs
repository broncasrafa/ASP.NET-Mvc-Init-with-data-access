using System.Collections.Generic;
using Domain.Entities;

namespace DataAccess.Providers.ADO.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : Base
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllActive();
        TEntity GetById(int id);
        int ChangeStatus(int id, int idUsuarioAtualizacao, bool ativo);
        int Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
