using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFinanceira.Data;
using ApiFinanceira.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceira.Services
{
    public class ContaService
    {
        private readonly AppDbContext _context;

        public ContaService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<ApiFinanceira.Models.Conta>> GetContasAsync()
        {
            return await _context.Contas
                .AsNoTracking()
                .Include(c => c.Cliente)
                .ToListAsync();
        }
        
        public async Task<Conta> PostContaAsync(int usuarioId, string tipoConta, decimal saldoInicial)
        {
            var novaConta = new Conta
            {
                UsuarioId = usuarioId,
                TipoConta = tipoConta,
                Saldo = saldoInicial,
                DataCriacao = DateTime.UtcNow
            };

            _context.Contas.Add(novaConta);
            await _context.SaveChangesAsync();

            return novaConta;
        }

        public async Task<List<Conta>> GetContasPorUsuarioAsync(int usuarioId)
        {
            return await _context.Contas
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }
}