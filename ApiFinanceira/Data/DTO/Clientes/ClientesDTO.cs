namespace ApiFinanceira.Data.DTO.Clientes
{
    public class ClientesDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime? DataCriacao { get; set; }
    }
}