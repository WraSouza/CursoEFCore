using System;
using System.Linq;
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
           //A opção EnableRetryOnFailure informa quantas vezes a aplicação irá tentar se reconectar ao banco em caso de falha na conexão.
           //Se não passar parâmetro algum, ele irá tentar até 6 vezes até completar 1 minuto.
           //MaxRetryCount informa quantas vezes a aplicação poderá tentar se reconectar.
           //MaxRetryDelay informa quanto tempo deve esperar até a aplicação tentar se reconectar novamente.
           optionsBuilder
           .UseLoggerFactory(_logger)
           .EnableSensitiveDataLogging()
           .UseSqlServer("Server=localhost;Database=CursoEFCore;Trusted_Connection=True",
           p => p.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null)
           .MigrationsHistoryTable("curso_ef_core"));
       } 

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
       }

        //Esse método é para mapear propriedades que não foram configuradas.
        //No exemplo abaixo iremos verificar as strings.
       private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
       {
           foreach(var entity in modelBuilder.Model.GetEntityTypes())
           {
               var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

               foreach(var property in properties)
               {
                   if(string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                   {
                       //Nesse caso irá verificar se uma propriedade do tipo string está vazia e irá setar o tamanho máximo
                       property.SetMaxLength(100);

                       //Aqui, irá configurar todas as propriedades do tipo string vazias como varchar(100)
                       property.SetColumnType("VARCHAR(100)");
                   }
               }
           }
       }
    }
}