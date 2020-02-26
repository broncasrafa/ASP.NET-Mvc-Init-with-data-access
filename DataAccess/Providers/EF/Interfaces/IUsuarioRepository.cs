using Domain.Entities;

namespace DataAccess.Providers.EF.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Usuario GetUsuarioByEmail(string email);
    }
}
