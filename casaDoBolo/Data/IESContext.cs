using casaDoBolo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
// using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore; // Removido, pois não é necessário aqui

namespace casaDoBolo.Data
{
    public class IESContext : IdentityDbContext
    {
        public IESContext(DbContextOptions<IESContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Sabor> Sabores { get; set; }
        public DbSet<Tamanho> Tamanhos { get; set; }

        // ⭐️ DbSet para o Histórico de Vendas ⭐️
        public DbSet<VendasAcumuladas> VendasAcumuladas { get; set; }

        public DbSet<ApplicationUser> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tamanho>()
                .Property(t => t.Valor)
                .HasPrecision(4, 2);

            modelBuilder.Entity<VendasAcumuladas>()
                .Property(v => v.ValorVendidoMes)
                .HasPrecision(18, 2);

           
            modelBuilder.Entity<Pedido>()
                .Property(p => p.ValorTotal)
                .HasPrecision(18, 2);
        }
    }
}