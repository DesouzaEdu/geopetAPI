using projetoFinal.db.Models.PessoaCuidadora;
using System.ComponentModel.DataAnnotations.Schema;
using projetoFinal.Controllers.inputs;

namespace projetoFinal.db.Models.Pets;
public class PetModel {
    public int Id {get; set;}
   
    [Column("STATUS")]
    public bool Status {get; set;} 
    [Column("PESSOA_CUIDADORA")] 
    public PessoaCuidadoraModel PessoaCuidadora { get; set; } 
    
    [Column("NOME")]
    public string Nome { get; set; }
    [Column("PESO")]
    public decimal Peso { get; set; }
    [Column("HASH_LOCALIZACAO")]
    public string HashLocalizacao { get; set; }
    [Column("IDADE")]
    public int Idade { get; set; }
    [Column("RACA")]
    public string Raca { get; set; }
    [Column("PORTE")]
    public string Porte { get; set; }
}