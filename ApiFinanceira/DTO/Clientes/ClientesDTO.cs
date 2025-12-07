namespace ApiFinanceira.DTO.Clientes;

public class ClientesDto
{
    public int id { get; set; }
    public string Nome { get; set; }
    public string documento { get; set; }
    public string email { get; set; }
    public bool status { get; set; }
    public DateTime data_criacao { get; set; }
}