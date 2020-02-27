using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataAccess.Providers.ADO.Interfaces;
using Domain.Entities;

namespace DataAccess.Providers.ADO.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private DbContext _context;
        private IDbProvider _dbProvider;

        public UsuarioRepository(DbContext context) : base(context)
        {
            _context = context;
            _dbProvider = _context.DbProvider;
        }

        protected sealed override string TableName
        {
            get
            {
                return "tbUsuario";
            }
        }
        protected override Usuario BuildEntity(DataRow dr)
        {
            if (dr == null) return null;

            return new Usuario
            {
                Id = Convert.ToInt32(dr.ItemArray[0]),
                IdPerfil = dr.Field<int>("IdPerfil"),
                Login = dr.Field<string>("Login"),
                Senha = dr.Field<string>("Senha"),
                Nome = dr.Field<string>("Nome"),
                Email = dr.Field<string>("Email"),
                Telefone = dr.Field<string>("Telefone"),
                AvatarUrl = dr.Field<string>("AvatarUrl"),
                IdUsuarioCadastro = dr.Field<int>("IdUsuarioCadastro"),
                IdUsuarioAtualizacao = dr.Field<int?>("IdUsuarioAtualizacao"),
                DataCadastro = dr.Field<DateTime>("DataCadastro"),
                DataAtualizacao = dr.Field<DateTime?>("DataAtualizacao"),
                Ativo = dr.Field<bool>("Ativo")
            };
        }
        protected override Usuario BuildEntity(DataTable dataTable)
        {
            return dataTable.AsEnumerable()
                            .Select(c => new Usuario
                            {
                                Id = Convert.ToInt32(c.ItemArray[0]),
                                IdPerfil = c.Field<int>("IdPerfil"),
                                Login = c.Field<string>("Login"),
                                Senha = c.Field<string>("Senha"),
                                Nome = c.Field<string>("Nome"),
                                Email = c.Field<string>("Email"),
                                Telefone = c.Field<string>("Telefone"),
                                AvatarUrl = c.Field<string>("AvatarUrl"),
                                IdUsuarioCadastro = c.Field<int>("IdUsuarioCadastro"),
                                IdUsuarioAtualizacao = c.Field<int?>("IdUsuarioAtualizacao"),
                                DataCadastro = c.Field<DateTime>("DataCadastro"),
                                DataAtualizacao = c.Field<DateTime?>("DataAtualizacao"),
                                Ativo = c.Field<bool>("Ativo")
                            })
                            .FirstOrDefault();
        }

        public override int Insert(Usuario entity)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine($@"INSERT INTO {TableName} 
                                    (Login
                                   , Senha
                                   , IdPerfil
                                   , Nome
                                   , Email
                                   , Telefone
                                   , AvatarUrl
                                   , IdUsuarioCadastro
                                   , DataCadastro
                                   , Ativo
                                   , IdUsuarioAtualizacao
                                   , DataAtualizacao) 
                        VALUES(@Login
                             , @Senha
                             , @IdPerfil
                             , @Nome
                             , @Email
                             , @Telefone
                             , @AvatarUrl
                             , @IdUsuarioCadastro
                             , @DataCadastro
                             , @Ativo
                             , @IdUsuarioAtualizacao
                             , @DataAtualizacao)");

            if (_dbProvider.ProviderName().Equals("SqlServer"))
                query.Append("SELECT SCOPE_IDENTITY()");
            else if (_dbProvider.ProviderName().Equals("MySQL"))
                query.Append("; SELECT LAST_INSERT_ID();");

            var parameters = new List<IDataParameter>
            {
                _dbProvider.CriarParametro("@Login", SqlDbType.VarChar, entity.Login),
                _dbProvider.CriarParametro("@Senha", SqlDbType.VarChar, entity.Senha),
                _dbProvider.CriarParametro("@IdPerfil", SqlDbType.Int, entity.IdPerfil),
                _dbProvider.CriarParametro("@Nome", SqlDbType.VarChar, entity.Nome),
                _dbProvider.CriarParametro("@Email", SqlDbType.VarChar, entity.Email),
                _dbProvider.CriarParametro("@Telefone", SqlDbType.VarChar, entity.Telefone),
                _dbProvider.CriarParametro("@AvatarUrl", SqlDbType.VarChar, entity.AvatarUrl),
                _dbProvider.CriarParametro("@IdUsuarioCadastro", SqlDbType.Int, entity.IdUsuarioCadastro),
                _dbProvider.CriarParametro("@DataCadastro", SqlDbType.DateTime, DateTime.Now),
                _dbProvider.CriarParametro("@Ativo", SqlDbType.TinyInt, entity.Ativo),
                _dbProvider.CriarParametro("@IdUsuarioAtualizacao", SqlDbType.Int, Convert.DBNull),
                _dbProvider.CriarParametro("@DataAtualizacao", SqlDbType.DateTime, Convert.DBNull)
            };

            var result = _dbProvider.Cadastrar(_context.CreateCommand(), query.ToString(), CommandType.Text, parameters.ToArray());
            return Convert.ToInt32(result);
        }
        public override void Update(Usuario entity)
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendLine($@"UPDATE {TableName} 
                                       SET Nome = @Nome
                                         , Email = @Email
                                         , Telefone = @Telefone
                                         , AvatarUrl = @AvatarUrl
                                         , IdPerfil = @IdPerfil
                                         , Senha = @Senha
                                         , Login = @Login
                                         , IdUsuarioAtualizacao = @IdUsuarioAtualizacao
                                         , DataAtualizacao = @DataAtualizacao
                                     WHERE Id = @IdUsuario");

                var parameters = new List<IDataParameter>
                {
                    _dbProvider.CriarParametro("@IdUsuario", SqlDbType.Int, entity.Id),
                    _dbProvider.CriarParametro("@Nome", SqlDbType.VarChar, entity.Nome),
                    _dbProvider.CriarParametro("@Email", SqlDbType.VarChar, entity.Email),
                    _dbProvider.CriarParametro("@Telefone", SqlDbType.VarChar, entity.Telefone),
                    _dbProvider.CriarParametro("@AvatarUrl", SqlDbType.VarChar, entity.AvatarUrl),
                    _dbProvider.CriarParametro("@Login", SqlDbType.VarChar, entity.Login),
                    _dbProvider.CriarParametro("@Senha", SqlDbType.VarChar, entity.Senha),
                    _dbProvider.CriarParametro("@IdPerfil", SqlDbType.Int, entity.IdPerfil),
                    _dbProvider.CriarParametro("@IdUsuarioAtualizacao", SqlDbType.Int, entity.IdUsuarioAtualizacao),
                    _dbProvider.CriarParametro("@DataAtualizacao", SqlDbType.DateTime, DateTime.Now),
                };

                _dbProvider.ExecutarComando(_context.CreateCommand(), query.ToString(), CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Usuario Register(Usuario usuario)
        {
            try
            {
                var idNovo = Insert(usuario);
                var usuarioNovo = GetById(idNovo);
                return usuarioNovo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
