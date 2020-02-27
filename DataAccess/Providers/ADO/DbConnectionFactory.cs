using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using DataAccess.Providers.ADO.Interfaces;
using DataAccess.Providers.ADO.Providers;

namespace DataAccess.Providers.ADO
{
    public class DbConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _name;

        public DbConnectionFactory(string connectionName)
        {
            if (connectionName == null)
                throw new ArgumentNullException("connectionName");

            var connectionString = ConfigurationManager.ConnectionStrings[connectionName];
            if (connectionString == null)
                throw new ConfigurationErrorsException($"Was not possible to find connection string named '{connectionName}'");

            _name = connectionString.ProviderName;
            _provider = DbProviderFactories.GetFactory(connectionString.ProviderName);
            _connectionString = connectionString.ConnectionString;
        }

        /// <summary>
        /// Creates database connection
        /// </summary>
        /// <returns>returns database connection</returns>
        public IDbConnection CreateConnection()
        {
            var connection = _provider.CreateConnection();
            if (connection == null)
                throw new ConfigurationErrorsException($"Was not possible to create connection string named '{_name}'");

            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Get database provider based on providerName of connectionstring
        /// </summary>
        /// <returns>returns database provider</returns>
        public IDbProvider GetProvider()
        {
            switch (_name)
            {
                case "System.Data.SqlClient":
                    return new SqlServerProvider();
                case "System.Data.OracleClient":
                    return new OracleProvider();
                case "MySql.Data.MySqlClient":
                    return new MySqlProvider();
                default:
                    throw new Exception("Tipo de provedor não suportado.");
            }
        }
    }
}
