using System.Collections.Generic;
using Domain.Entities;

namespace DataAccess.Providers.ADO.Test
{
    public interface IUsuarioService
    {
        IEnumerable<Usuario> GetUsuarios();
        Usuario RegistrarUsuario(Usuario usuario);
        Usuario GetUsuarioById(int idUsuario);
        void AtualizarUsuario(Usuario usuario);
        int MudarStatusUsuario(int idUsuario, int idUsuarioAtualizacao, bool ativo);
        void DeletarUsuario(int idUsuario);
    }
}
