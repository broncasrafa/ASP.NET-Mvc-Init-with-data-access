namespace Domain.Entities
{
    public class Usuario : Base
    {
        public int IdPerfil { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string AvatarUrl { get; set; }
    }
}
