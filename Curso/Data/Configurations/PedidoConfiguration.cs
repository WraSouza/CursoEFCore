using Curso.Domain;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore.Data.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
               builder.HasKey(p => p.Id);
               builder.Property(p => p.IniciadoEm).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
               builder.Property(p => p.Status).HasConversion<string>();
               builder.Property(p => p.TipoFrete).HasConversion<int>();
               builder.Property(p => p.Observacao).HasColumnType("VARCHAR(12)");

               builder.HasMany(p => p.Itens)
               .WithOne(p => p.Pedido)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}