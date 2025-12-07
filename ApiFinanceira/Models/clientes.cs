using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("clientes")]
public class clientes : BaseModel
{
    [PrimaryKey("id")]
    public int id { get; set; }
    [Column("nome")]
    public string nome { get; set; }
    [Column("documento")]
    public string documento { get; set; }
    [Column("email")]
    public string email { get; set; }
    [Column("status")]
    public bool status { get; set; }
    [Column("data_criacao")]
    public DateTime? data_criacao { get; set; }
}
