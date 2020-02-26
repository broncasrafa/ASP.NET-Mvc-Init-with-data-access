using System;

namespace Domain.Entities
{
    public class Base
    {
        public int Id { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public int? IdUsuarioAtualizacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool Ativo { get; set; }
    }
}
