using Curso.Domain;
using CursoEFCore.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set;}
        public DbSet<Produto> Produtos { get; set;}
        public DbSet<Cliente> Clientes { get; set;}

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
           
           optionsBuilder
           .UseLoggerFactory(_logger)
           .EnableSensitiveDataLogging()
           .UseSqlServer("Server=localhost;Database=CursoEFCore;Trusted_Connection=True");
       } 

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
       }
    }
}