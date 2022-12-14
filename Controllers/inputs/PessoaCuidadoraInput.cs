using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetoFinal.Controllers.inputs
{
    public class PessoaCuidadoraInput
    {
        [Column("NOME")]
        public string Nome { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("SENHA")]
        [MinLength (6)]
        [MaxLength(10)]
        public virtual string Senha { get; set; }
        [Column(TypeName = "varchar(8)")]
        [MinLength(8)]
        [MaxLength(8)]
        public string CEP { get; set; }
    }
}
