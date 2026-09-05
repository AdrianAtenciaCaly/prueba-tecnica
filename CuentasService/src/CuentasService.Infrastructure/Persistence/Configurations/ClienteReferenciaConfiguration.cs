using CuentasService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CuentasService.Infrastructure.Persistence.Configurations;

public class ClienteReferenciaConfiguration : IEntityTypeConfiguration<ClienteReferencia>
{
    public void Configure(EntityTypeBuilder<ClienteReferencia> builder)
    {
        builder.ToTable("clientes_referencia");

        builder.HasKey(c => c.ClienteId);
        builder.Property(c => c.ClienteId).HasColumnName("cliente_id").HasMaxLength(50);
        builder.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
        builder.Property(c => c.Estado).HasColumnName("estado").IsRequired();
        builder.Property(c => c.ActualizadoEn).HasColumnName("actualizado_en").IsRequired();
    }
}
