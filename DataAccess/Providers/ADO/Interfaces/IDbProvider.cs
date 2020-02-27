using System.Data;

namespace DataAccess.Providers.ADO.Interfaces
{
    public interface IDbProvider
    {
        /// <summary>
        /// Obtem a string com o nome do provedor
        /// </summary>
        string ProviderName();

        /// <summary>
        /// Cria uma nova conexão a partir da connection string
        /// </summary>
        IDbConnection CriarConexao(string connectionString);

        /// <summary>
        /// Abre uma nova conexão com o banco de dados podendo definir os parâmetros iniciais necessários
        /// </summary>
        void AbrirConexao(IDbConnection connection);

        /// <summary>
        /// Realiza uma pesquisa no banco de dados
        /// </summary>
        DataSet Pesquisar(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters);

        /// <summary>
        /// Realiza um cadastro no banco de dados e retorna o identificador gerado
        /// </summary>
        long Cadastrar(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters);

        /// <summary>
        /// Executa um comando no banco de dados
        /// </summary>
        int ExecutarComando(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters);

        /// <summary>
        /// Cria um novo parâmetro a partir dos dados fornecidos
        /// </summary>
        void CriarParametros(IDbCommand command, params IDataParameter[] parameters);

        /// <summary>
        /// Cria um novo parâmetro a partir dos dados fornecidos
        /// </summary>
        IDataParameter CriarParametro(string parameterName, SqlDbType type, object value);
    }
}
