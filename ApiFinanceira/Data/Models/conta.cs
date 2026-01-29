using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFinanceira.Models
{
    [Table("contas")]
    public class Conta
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        
        [ForeignKey("UsuarioId")]
        public virtual Cliente Cliente { get; set; }

        [Column("tipo_conta")]
        public string TipoConta { get; set; }

        [Column("saldo")]
        public decimal Saldo { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}