using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IRepositoryUsuario
    {
        Usuario Login(string username, string password);
    }
}
