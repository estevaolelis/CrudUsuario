namespace ApiFinanceira.Data.DTO.Exportacao;

public class ResultadoExportacao
{
    public byte[] Conteudo { get; set; }
    public string NomeArquivo { get; set; }
    public string ContentType { get; set; }
}