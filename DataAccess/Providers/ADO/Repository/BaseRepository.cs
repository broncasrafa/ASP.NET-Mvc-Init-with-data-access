using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataAccess.Providers.ADO.Interfaces;
using Domain.Entities;

namespace DataAccess.Providers.ADO.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Base
    {
        private DbContext _context;
        private IDbProvider _dbProvider;
        protected DbContext Context
        {
            get { return this._context; }
        }

        protected abstract string TableName { get; }
        protected abstract TEntity BuildEntity(DataRow dataRow);
        protected abstract TEntity BuildEntity(DataTable dataTable);


        protected virtual ICollection<TEntity> DataTableToCollection(DataTable dataTable)
        {
            if (dataTable == null) return null;

            return dataTable.AsEnumerable().Select(c => BuildEntity(c)).ToList();
        }
        protected virtual string IdentityCollectionToClauseIn(ICollection<TEntity> collection)
        {
            var listIds = collection.Select(c => c.Id);
            return string.Join(",", listIds);
        }


        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbProvider = _context.DbProvider;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT * FROM {TableName};");

                DataSet ds = _dbProvider.Pesquisar(_context.CreateCommand(), query.ToString(), CommandType.Text, null, null);

                return DataTableToCollection(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual IEnumerable<TEntity> GetAllActive()
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT * FROM {TableName} WHERE Ativo = 1");

                DataSet ds = _dbProvider.Pesquisar(_context.CreateCommand(), query.ToString(), CommandType.Text, null, null);

                return DataTableToCollection(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual TEntity GetById(int id)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT * FROM {TableName} WHERE Id = @Id ;");

                var parameters = new List<IDataParameter>()
                {
                    _dbProvider.CriarParametro("@Id", SqlDbType.Int, id)
                };

                var ds = _dbProvider.Pesquisar(_context.CreateCommand(), query.ToString(), CommandType.Text, parameters.ToArray());
                return BuildEntity(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual int ChangeStatus(int id, int idUsuarioAtualizacao, bool ativo)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine($@"UPDATE {TableName} 
                                       SET Ativo = @Ativo
                                         , DataAtualizacao = @DataAtualizacao
                                         , IdUsuarioAtualizacao = @IdUsuarioAtualizacao
                                     WHERE Id = @Id");

                var parameters = new List<IDataParameter>()
                {
                    _dbProvider.CriarParametro("@Id", SqlDbType.Int, id),
                    _dbProvider.CriarParametro("@DataAtualizacao", SqlDbType.DateTime, DateTime.Now),
                    _dbProvider.CriarParametro("@IdUsuarioAtualizacao", SqlDbType.Int, idUsuarioAtualizacao),
                    _dbProvider.CriarParametro("@Ativo", SqlDbType.Bit, ativo)
                };

                var result = _dbProvider.ExecutarComando(_context.CreateCommand(), query.ToString(), CommandType.Text, parameters.ToArray());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual void Delete(int id)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"DELETE FROM {TableName} WHERE Id = @Id");

                var parameters = new List<IDataParameter>()
                {
                    _dbProvider.CriarParametro("@Id", SqlDbType.Int, id)
                };

                var result = _dbProvider.ExecutarComando(_context.CreateCommand(), query.ToString(), CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public abstract int Insert(TEntity entity);
        public abstract void Update(TEntity entity);

        /// <summary>
        /// Map the database result to Entity type
        /// </summary>
        /// <param name="reader">database result</param>
        /// <returns>returns Entity type</returns>
        //protected TEntity BuildEntity<TEntity>(IDataRecord reader)
        //{
        //    var objRetorno = Activator.CreateInstance<TEntity>();

        //    foreach(var property in typeof(TEntity).GetProperties())
        //    {
        //        if(reader.HasColumn(property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
        //        {
        //            property.SetValue(objRetorno, reader[property.Name]);
        //        }
        //    }

        //    return objRetorno;
        //}
    }
}
