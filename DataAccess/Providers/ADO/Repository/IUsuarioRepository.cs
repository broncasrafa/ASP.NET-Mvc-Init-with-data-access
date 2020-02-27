using Domain.Entities;

namespace DataAccess.Providers.ADO.Repository
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Usuario Register(Usuario usuario);
    }
}
