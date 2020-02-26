using Application.IAppServices;
using Domain.Entities;
using Domain.Interfaces.IServices;

namespace Application.AppServices
{
    public class AppServiceUsuario : IAppServiceUsuario
    {
        private readonly IServiceUsuario _serviceUsuario;

        public AppServiceUsuario(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        public Usuario Login(string username, string password)
        {
            return _serviceUsuario.Login(username, password);
        }
    }
}
