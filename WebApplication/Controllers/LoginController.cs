using System;
using System.Web.Mvc;
using WebApplication.Models;
using Application.IAppServices;

namespace WebApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAppServiceUsuario _appServiceUsuario;


        public LoginController(IAppServiceUsuario appServiceUsuario)
        {
            _appServiceUsuario = appServiceUsuario;
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {                    
                    return View();
                }

                var result = _appServiceUsuario.Login(viewModel.Username, viewModel.Password);

                if (result != null && result.Id > 0)
                {
                    // verifica o perfil para redirecionar para tela correta de acordo com o perfil do usuario
                    return RedirectToAction("Home");
                }

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Logoff()
        {
            return View("Login");
        }
    }
}