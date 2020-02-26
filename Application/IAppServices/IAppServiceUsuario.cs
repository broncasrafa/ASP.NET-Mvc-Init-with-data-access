using Domain.Entities;

namespace Application.IAppServices
{
    public interface IAppServiceUsuario
    {
        Usuario Login(string username, string password);
    }
}
