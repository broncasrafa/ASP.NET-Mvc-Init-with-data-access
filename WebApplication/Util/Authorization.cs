using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Entities;

namespace WebApplication.Util
{
    public class Authorization : ActionFilterAttribute
    {
        private Enums.Perfis[] _PerfisAcesso { get; set; }


        public Authorization(Enums.Perfis[] perfisAcesso = null) : base()
        {
            _PerfisAcesso = perfisAcesso;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            UserProperty userProperty = new UserProperty();

            if (userProperty.IsUserLogged())
            {
                if (_PerfisAcesso != null)
                {
                    Usuario usuarioLogado = userProperty.GetUserLogged();

                    if (usuarioLogado == null)
                    {
                        actionContext.Result = new RedirectResult("~/Login/Index");
                    }

                    if (usuarioLogado.IdPerfil != (int)Enums.Perfis.Administrador &&
                        (from c in _PerfisAcesso.ToList() where Convert.ToInt32(c) == usuarioLogado.IdPerfil select c).ToList().Count == 0)
                    {
                        actionContext.Result = new RedirectResult("~/Home/Index");
                    }
                }
            }
            else
            {
                actionContext.Result = new RedirectResult("~/Login/Index");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}