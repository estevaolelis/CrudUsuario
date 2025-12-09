using Supabase.Postgrest;
using Supabase.Postgrest.Models;

namespace ApiFinanceira.Services
{
    public class ClientesService
    {
        private readonly Supabase.Client _supabase;

        public ClientesService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }
        
        public async Task<clientes> PostClientesAsync(string nome, string documento, string email)
        {
            var novoCliente = new clientes
            {
                nome = nome,
                documento = documento,
                email = email,
                status = true,
                data_criacao = DateTime.UtcNow
            };
            
            var response = await _supabase
                .From<clientes>()
                .Insert(novoCliente, new QueryOptions { Returning = QueryOptions.ReturnType.Representation });
            
            return response.Models.FirstOrDefault();
        }
        
        public async Task<List<clientes>> GetClientesAsync()
        {
            var response = await _supabase.From<clientes>().Get();
            
            return response.Models; 
        }

        public async Task<clientes> GetClientesByIdAsync(int id)
        {
            var response = await _supabase.From<clientes>().Where(c => c.id == id).Get();
            return response.Models.FirstOrDefault();
        }
    }   
}