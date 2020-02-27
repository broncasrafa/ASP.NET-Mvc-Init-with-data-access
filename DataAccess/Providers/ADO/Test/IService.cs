using DataAccess.Providers.ADO.Interfaces;

namespace DataAccess.Providers.ADO.Test
{
    public class IService
    {
        /// <summary>
        /// Obter a conexão com a base de dados
        /// </summary>
        /// <returns>retorna conexão com a base de dados</returns>
        public static IConnectionFactory GetConnection()
        {
            return new DbConnectionFactory("DadosConexao");
        }
    }
}
