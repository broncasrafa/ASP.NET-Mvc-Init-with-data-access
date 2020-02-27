using System.Collections.Generic;
using DataAccess.Providers.ADO.Interfaces;
using DataAccess.Providers.ADO.Repository;
using Domain.Entities;

namespace DataAccess.Providers.ADO.Test
{
    public class UsuarioService : IService, IUsuarioService
    {
        private readonly DbContext _context;
        private IConnectionFactory _connectionFactory;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService()
        {
            _connectionFactory = GetConnection();
            _context = new DbContext(_connectionFactory);
            _usuarioRepository = new UsuarioRepository(_context);
        }

        public Usuario RegistrarUsuario(Usuario usuario)
        {
            return _usuarioRepository.Register(usuario);
        }
        public Usuario GetUsuarioById(int idUsuario)
        {
            return _usuarioRepository.GetById(idUsuario);
        }
        public IEnumerable<Usuario> GetUsuarios()
        {
            return _usuarioRepository.GetAll();
        }
        public void AtualizarUsuario(Usuario usuario)
        {
            _usuarioRepository.Update(usuario);
        }
        public int MudarStatusUsuario(int idUsuario, int idUsuarioAtualizacao, bool ativo)
        {
            return _usuarioRepository.ChangeStatus(idUsuario, idUsuarioAtualizacao, ativo);
        }
        public void DeletarUsuario(int idUsuario)
        {
            _usuarioRepository.Delete(idUsuario);
        }
    }
}
