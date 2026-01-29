namespace ApiFinanceira.Data.DTO.Base
{
    public class BaseResponseDTO<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = "";
        public T? Dados { get; set; }
        public string? Codigo { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static BaseResponseDTO<T> Ok(T dados, string mensagem = "Operação realizada com sucesso")
        {
            return new BaseResponseDTO<T>
            {
                Sucesso = true,
                Dados = dados,
                Mensagem = mensagem,
                Timestamp = DateTime.UtcNow
            };
        }

        public static BaseResponseDTO<T> Falha(string mensagem, string? codigo = null)
        {
            return new BaseResponseDTO<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Codigo = codigo,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
