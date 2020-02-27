using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DataAccess.Providers.ADO.Interfaces;

namespace DataAccess.Providers.ADO.Providers
{
    public class SqlServerProvider : IDbProvider
    {
        public string ProviderName()
        {
            return "SqlServer";
        }
        public IDbConnection CriarConexao(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        public void AbrirConexao(IDbConnection connection)
        {
            connection.Open();
        }
        public long Cadastrar(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters)
        {
            command.CommandType = type;
            command.CommandText = cmdText;

            if (parameters != null)
                parameters.ToList().ForEach(param => command.Parameters.Add(param));

            return Convert.ToInt64(command.ExecuteScalar());

        }
        public void CriarParametros(IDbCommand command, params IDataParameter[] parameters)
        {
            if (parameters != null)
            {
                parameters.ToList().ForEach(param => command.Parameters.Add(param));
            }
        }
        public IDataParameter CriarParametro(string parameterName, SqlDbType type, object value)
        {
            SqlParameter parameter = null;
            var paramName = parameterName.Substring(0, 1).Equals("@") ? parameterName : $"@{parameterName}";

            if (value == null)
                parameter = new SqlParameter(paramName, DBNull.Value);
            else
            {
                parameter = new SqlParameter(paramName, type);
                parameter.Value = value;
            }
            return parameter;
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

            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Verifica o SqlDbType correspondente nome do tipo da variável fornecida.
        /// </summary>
        /// <param name="Valor">Nome do tipo da váriavel. ex:("System.String")</param>
        /// <returns>SqlDbType correspondente ao valor de entrada.</returns>
        private static SqlDbType GetSqlDbType(Type Valor)
        {
            try
            {
                if (Valor == typeof(Int32))
                    return SqlDbType.Int;
                else if (Valor == typeof(Int64))
                    return SqlDbType.BigInt;
                else if (Valor == typeof(Int16))
                    return SqlDbType.SmallInt;
                else if (Valor == typeof(byte))
                    return SqlDbType.TinyInt;
                else if (Valor == typeof(decimal))
                    return SqlDbType.Decimal;
                else if (Valor == typeof(DateTime))
                    return SqlDbType.DateTime;
                else if (Valor == typeof(string))
                    return SqlDbType.VarChar;
                else if (Valor == typeof(bool))
                    return SqlDbType.Bit;
                else if (Valor == typeof(byte[]))
                    return SqlDbType.Image;
                else
                    throw new Exception($"Type '{Valor}' not exists");
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
