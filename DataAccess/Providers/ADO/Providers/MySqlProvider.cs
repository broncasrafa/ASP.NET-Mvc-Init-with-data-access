using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using DataAccess.Providers.ADO.Interfaces;

namespace DataAccess.Providers.ADO.Providers
{
    public class MySqlProvider : IDbProvider
    {
        public string ProviderName()
        {
            return "MySQL";
        }
        public IDbConnection CriarConexao(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
        public void AbrirConexao(IDbConnection connection)
        {
            connection.Open();
        }
        public long Cadastrar(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters)
        {
            try
            {
                command.CommandType = type;
                command.CommandText = cmdText;

                if (parameters != null)
                    parameters.ToList().ForEach(param => command.Parameters.Add(param));

                return Convert.ToInt64(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CriarParametros(IDbCommand command, params IDataParameter[] parameters)
        {
            if (parameters != null)
            {
                parameters.ToList().ForEach(param => command.Parameters.Add(param));
            }
        }
        public int ExecutarComando(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters)
        {
            command.CommandType = type;
            command.CommandText = cmdText;

            if (parameters != null)
                parameters.ToList().ForEach(param => command.Parameters.Add(param));

            return command.ExecuteNonQuery();
        }
        public DataSet Pesquisar(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters)
        {
            command.CommandType = type;
            command.CommandText = cmdText;

            if (parameters != null)
                parameters.ToList().ForEach(param => command.Parameters.Add(param));

            MySqlDataAdapter adapter = new MySqlDataAdapter((MySqlCommand)command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }
        public IDataParameter CriarParametro(string parameterName, SqlDbType type, object value)
        {
            MySqlParameter parameter = null;
            var paramName = parameterName.Substring(0, 1).Equals("@") ? parameterName : $"@{parameterName}";

            if (value == null)
                parameter = new MySqlParameter(paramName, DBNull.Value);
            else
            {
                parameter = new MySqlParameter(paramName, value);
                //parameter.MySqlDbType = (MySqlDbType)type;
                //parameter.Value = value;
            }
            return parameter;
        }

        /// <summary>
        /// Verifica o MySqlDbType correspondente nome do tipo da variável fornecida.
        /// </summary>
        /// <param name="Valor">Nome do tipo da váriavel. ex:("System.String")</param>
        /// <returns>MySqlDbType correspondente ao valor de entrada.</returns>
        private static MySqlDbType GetMySqlDbType(Type Valor)
        {
            throw new NotImplementedException();
        }
    }
}
