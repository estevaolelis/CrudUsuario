using ApiFinanceira.DTO.Clientes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiFinanceira.Services
{
    public class ClientesService
    {
        private readonly Supabase.Client _supabase;
        public ClientesService(Supabase.Client supabase)
        {
            _supabase = supabase;
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
