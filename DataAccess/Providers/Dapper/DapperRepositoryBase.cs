using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using DataAccess.Providers.Dapper.Interfaces;
using Dapper;


namespace DataAccess.Providers.Dapper
{
    public abstract class DapperRepositoryBase<TEntity> : DapperHelper, IDapperRepositoryBase<TEntity> where TEntity : class
    {
        protected abstract string _TableName { get; }
        private IEnumerable<PropertyInfo> _GetProperties => typeof(TEntity).GetProperties();

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_TableName} SET ");
            var properties = GenerateListOfProperties(_GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property} = @{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove ultima virgula
            updateQuery.Append(" WHERE Id = @Id");

            return updateQuery.ToString();
        }
        private string GenerateInsertQuery()
        {
            var fields = new StringBuilder();
            var parameters = new StringBuilder();
            
            var properties = GenerateListOfProperties(_GetProperties);

            properties.ForEach(prop => {
                fields.Append($"[{prop}],");
            });
            fields.Remove(fields.Length - 1, 1); // remove a ultima virgula
            
            properties.ForEach(prop => {
                parameters.Append($"@{prop},");
            });
            parameters.Remove(parameters.Length - 1, 1); // remove a ultima virgula

            var insertQuery = new StringBuilder($@"INSERT INTO {_TableName} ({fields.ToString()}) 
                                                     VALUES ({parameters.ToString()}) 
                                                   SELECT SCOPE_IDENTITY()");

            return insertQuery.ToString();
        }


        public int Add(TEntity entity)
        {
            using (var db = CreateConnection())
            {
                var queryInsert = GenerateInsertQuery();

                var result = db.Execute(queryInsert, entity);

                return result;
            }
        }

        public int AddRange(IEnumerable<TEntity> collection)
        {
            var result = 0;

            using (var db = CreateConnection())
            {
                foreach(var item in collection)
                {
                    var queryInsert = GenerateInsertQuery();
                    result = db.Execute(queryInsert, item);
                }

                return result;
            }
        }        

        public void Update(TEntity entity)
        {
            using (var db = CreateConnection())
            {
                string queryUpdate = GenerateUpdateQuery();
                db.Execute(queryUpdate, entity);
            }
        }

        public void Remove(int id)
        {
            using (var db = CreateConnection())
            {
                db.Execute($"DELETE FROM {_TableName} WHERE Id = @Id", new { Id = id });
            }
        }

        public TEntity Get(int id)
        {
            using (var db = CreateConnection())
            {
                var result = db.QuerySingleOrDefault<TEntity>($"SELECT * FROM {_TableName} WHERE Id = @Id", new { Id = id });
                if (result == null)
                    throw new KeyNotFoundException($"{ _TableName } with id[{ id}] could not be found.");

                return result;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var db = CreateConnection())
            {
                return db.Query<TEntity>($"SELECT * FROM {_TableName}");
            }
        }

        //public abstract int Insert(TEntity entity);
        //public abstract void Update(TEntity entity);
    }
}