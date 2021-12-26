using Curso.Domain;
using CursoEFCore.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set;}

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
           //optionsBuilder.UseSqlServer("Data source=localdb;Initial Catalog=CursoEFCore; Integrated Security=true");
           optionsBuilder.UseSqlServer("Server=localhost;Database=CursoEFCore;Trusted_Connection=True");
       } 

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
       }
    }
}