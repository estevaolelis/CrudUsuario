using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFinanceira.Models 
{
    [Table("clientes")]
    public class Cliente
    {
        [Key] // Indica que é a Chave Primária
        [Column("id")]
        public int Id { get; set; }
        
        [Column("nome")]
        public string Nome { get; set; }
        
        [Column("documento")]
        public string Documento { get; set; }
        
        [Column("email")]
        public string Email { get; set; }
        
        [Column("status")]
        public bool Status { get; set; }
        
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        public virtual List<Conta> Contas { get; set; }
    }
}