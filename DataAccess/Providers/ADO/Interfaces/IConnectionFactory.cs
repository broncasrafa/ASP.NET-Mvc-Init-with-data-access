using System.Data;

namespace DataAccess.Providers.ADO.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbProvider GetProvider();
    }
}
