using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo Username é obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Campo Senha é obrigatório")]
        public string Password { get; set; }
    }
}