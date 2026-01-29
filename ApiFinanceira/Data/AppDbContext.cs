using Microsoft.EntityFrameworkCore;
using ApiFinanceira.Models;

namespace ApiFinanceira.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Conta> Contas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .Property(c => c.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Conta>()
                .Property(c => c.Id)
                .UseIdentityColumn();

            base.OnModelCreating(modelBuilder);
        }
    }
}