using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace WebApplication.Util
{
    public class UserProperty
    {
        private const string COOKIE_NAME_DEFAULT = "UserInfo";
        private const string COOKIE_LANGUAGE_NAME_DEFAULT = "CultureInfo";

        private void RemoveCookie(string cookieName)
        {
            HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
        }
        private HttpCookie GetRequestCookie(string cookieName)
        {
            try
            {
                return HttpContext.Current.Request.Cookies[cookieName];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Usuario GetUserLogged()
        {
            string cookieName = COOKIE_NAME_DEFAULT;
            Usuario usuario = null;
            HttpCookie cookie = GetRequestCookie(cookieName);

            try
            {
                if (cookie != null)
                {
                    usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(Helpers.Decriptar(cookie["UserId"]));
                    usuario.Nome = Helpers.Decriptar(cookie["UserName"].ToString());
                    usuario.IdPerfil = Convert.ToInt32(Helpers.Decriptar(cookie["PerfilId"]));
                }
                else
                {
                    RemoveCookie(COOKIE_NAME_DEFAULT);
                    HttpContext.Current.Response.Redirect("~/Login");
                }
            }
            catch
            {
                RemoveCookie(COOKIE_NAME_DEFAULT);
            }
            return usuario;
        }
        public bool IsUserLogged()
        {
            string cookieName = COOKIE_NAME_DEFAULT;
            HttpCookie cookie = GetRequestCookie(cookieName);
            return (cookie != null);
        }
        public bool IsUserAllowed(List<Enums.Perfis> perfisAcesso)
        {
            Usuario usuario = GetUserLogged();
            bool retorno = true;

            if (usuario.IdPerfil != Convert.ToInt32(Enums.Perfis.Administrador) &&
                (from c in perfisAcesso where Convert.ToInt32(c) == usuario.IdPerfil select c).ToList().Count() == 0)
            {
                retorno = false;
            }
            return retorno;
        }
        public void UserLogoff()
        {
            string cookieName = COOKIE_NAME_DEFAULT;
            RemoveCookie(cookieName);
        }
        public string GetLanguage()
        {
            string cookieName = COOKIE_LANGUAGE_NAME_DEFAULT;
            string culture = string.Empty;
            HttpCookie cookie = GetRequestCookie(cookieName);

            if (cookie != null)
                culture = cookie.Value;

            return culture;
        }                
    }
}