using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Providers.Dapper
{
    public class DapperHelper
    {
        private SqlConnection _Connection { get; set; }
        private SqlConnection SqlConnection()
        {
            _Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DapperConnection"].ConnectionString);
            return _Connection;
        }

        public IDbConnection CreateConnection()
        {
            _Connection = SqlConnection();
            _Connection.Open();
            return _Connection;
        }
    }
}
