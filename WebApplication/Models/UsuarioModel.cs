using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; } 
        
        [Required(ErrorMessage ="Campo Nome é obrigatório")]        
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Nome é obrigatório")]
        [Display(Name="E-mail")]
        [MaxLength(100, ErrorMessage ="Campo com tamanho máximo de 100 caracteres")]
        public string Email { get; set; }

        public string Telefone { get; set; }
        public string AvatarUrl { get; set; }
        public int IdPerfil { get; set; }

        [Required(ErrorMessage = "Campo Username é obrigatório")]
        [Display(Name ="Username")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Campo Senha é obrigatório")]
        public string Senha { get; set; }
    }
}