using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using DataAccess.Providers.EF.Context;
using DataAccess.Providers.EF.Interfaces;
using DataAccess.Providers.EF.UnitOfWork;

namespace DataAccess.Providers.EF.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
        {
        }
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Usuario GetUsuarioByEmail(string email)
        {
            return Context.Usuarios.Where(c => c.Email == email).FirstOrDefault();
        }
    }
}
