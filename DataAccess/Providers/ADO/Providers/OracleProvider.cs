using System;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using DataAccess.Providers.ADO.Interfaces;

namespace DataAccess.Providers.ADO.Providers
{
    public class OracleProvider : IDbProvider
    {
        public string ProviderName()
        {
            return "Oracle";
        }
        public IDbConnection CriarConexao(string connectionString)
        {
            return new Oracle.DataAccess.Client.OracleConnection(connectionString);
        }
        public void AbrirConexao(IDbConnection connection)
        {
            connection.Open();

            try
            {
                ExecutarComando(connection.CreateCommand(), "ALTER SESSION SET NLS_COMP=LINGUISTIC", CommandType.Text, null, null);
                ExecutarComando(connection.CreateCommand(), "ALTER SESSION SET NLS_SORT=BINARY_AI", CommandType.Text, null, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while trying to set Oracle database language default", ex);
            }
        }
        public long Cadastrar(IDbCommand command, string cmdText, CommandType type, params IDataParameter[] parameters)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = (Oracle.DataAccess.Client.OracleCommand)command;
            cmd.CommandType = type;
            cmd.CommandText = cmdText;

            if (parameters != null)
                parameters.ToList().ForEach(param => cmd.Parameters.Add(param));

            cmd.Parameters[0].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            return Convert.ToInt64(cmd.Parameters[0].Value);
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
            OracleParameter parameter = null;
            var paramName = parameterName.Substring(0, 2).Equals("V_") ? parameterName : $"V_{parameterName}";

            if (value == null)
                parameter = new OracleParameter(paramName, DBNull.Value);
            else
            {
                parameter = new OracleParameter(paramName, type);
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
            {
                OracleParameter cursor = new OracleParameter("V_CURSOR", OracleType.Cursor);
                cursor.Direction = ParameterDirection.Output;
                command.Parameters.Add(cursor);
                parameters.ToList().ForEach(param => command.Parameters.Add(param));
            }

            Oracle.DataAccess.Client.OracleDataAdapter adapter = new Oracle.DataAccess.Client.OracleDataAdapter((Oracle.DataAccess.Client.OracleCommand)command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Verifica o OracleDbType correspondente nome do tipo da variável fornecida.
        /// </summary>
        /// <param name="Valor">Nome do tipo da váriavel. ex:("System.String")</param>
        /// <returns>OracleDbType correspondente ao valor de entrada.</returns>
        private Oracle.DataAccess.Client.OracleDbType GetOracleDbType(Type Valor)
        {
            try
            {
                if (Valor == typeof(Int32))
                    return Oracle.DataAccess.Client.OracleDbType.Int32;
                else if (Valor == typeof(Int64))
                    return Oracle.DataAccess.Client.OracleDbType.Int64;
                else if (Valor == typeof(Int16))
                    return Oracle.DataAccess.Client.OracleDbType.Int16;
                else if (Valor == typeof(byte))
                    return Oracle.DataAccess.Client.OracleDbType.Byte;
                else if (Valor == typeof(decimal))
                    return Oracle.DataAccess.Client.OracleDbType.Decimal;
                if (Valor == typeof(bool))
                    return Oracle.DataAccess.Client.OracleDbType.Int32;
                else if (Valor == typeof(DateTime))
                    return Oracle.DataAccess.Client.OracleDbType.Date;
                else if (Valor == typeof(string))
                    return Oracle.DataAccess.Client.OracleDbType.Varchar2;
                else if (Valor == typeof(byte[]))
                    return Oracle.DataAccess.Client.OracleDbType.Blob;
                else
                    throw new Exception("Tipo '" + Valor + "' não existente");
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
