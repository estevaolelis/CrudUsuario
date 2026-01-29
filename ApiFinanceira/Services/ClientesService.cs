using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFinanceira.Data;
using ApiFinanceira.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceira.Services
{
    public class ClientesService
    {
        private readonly AppDbContext _context;

        public ClientesService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Cliente> PostClientesAsync(string nome, string documento, string email)
        {
            var novoCliente = new Cliente
            {
                Nome = nome,
                Documento = documento,
                Email = email,
                Status = true,
                DataCriacao = DateTime.UtcNow
            };
            
            _context.Clientes.Add(novoCliente);
            await _context.SaveChangesAsync();
            
            return novoCliente;
        }
        
        public async Task<List<Cliente>> GetClientesAsync()
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(c => c.Status == true)
                .ToListAsync();
        }
        
        public async Task<Cliente?> GetClientesByIdAsync(int id)
        {
            return await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.Status == true);
        }
        
        public async Task<Cliente?> PutClientesAsync(int id, string nome, string documento, string email)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            cliente.Nome = nome;
            cliente.Documento = documento;
            cliente.Email = email;

            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> DeleteClientesAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;
            
            cliente.Status = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }   
}