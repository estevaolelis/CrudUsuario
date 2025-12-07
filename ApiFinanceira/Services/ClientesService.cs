using ApiFinanceira.DTO.Clientes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Supabase.Postgrest;

namespace ApiFinanceira.Services
{
    public class ClientesService
    {
        private readonly Supabase.Client _supabase;
        public ClientesService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<ClientesDto> PostClientesAsync(string nome, string documento, string email)
        {
            var novoCliente = new clientes
            {
                nome = nome,
                documento = documento,
                email = email,
                status = true,
                data_criacao = DateTime.UtcNow
            };
            
            var response = await _supabase .From<clientes>() .Insert(novoCliente, new QueryOptions { Returning = QueryOptions.ReturnType.Representation });
            var clienteCriado = response.Models.FirstOrDefault();
            
            if (clienteCriado != null)
            {
                return new ClientesDto()
                {
                    id = clienteCriado.id,
                    Nome = clienteCriado.nome,
                    email = clienteCriado.email,
                    status = clienteCriado.status,
                    data_criacao = clienteCriado.data_criacao
                };
            }

            return null;
        }

        public async Task<List<ClientesDto>> GetClientesAsync()
        {
            var clientesLista = await _supabase.From<clientes>().Get();
            return clientesLista.Models.Select(c => new ClientesDto
            {
                id = c.id,
                Nome = c.nome,
                documento = c.documento,
                email = c.email,
                status = c.status,
                data_criacao = c.data_criacao
            }).ToList();
        }
    }   
}
