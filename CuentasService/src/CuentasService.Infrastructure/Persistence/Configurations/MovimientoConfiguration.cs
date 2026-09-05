using CuentasService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CuentasService.Infrastructure.Persistence.Configurations;

public class MovimientoConfiguration : IEntityTypeConfiguration<Movimiento>
{
    public void Configure(EntityTypeBuilder<Movimiento> builder)
    {
        builder.ToTable("movimientos");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasColumnName("id");

        builder.Property(m => m.Fecha).HasColumnName("fecha").IsRequired();
        builder.Property(m => m.TipoMovimiento).HasColumnName("tipo_movimiento").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(m => m.Valor).HasColumnName("valor").HasColumnType("numeric(18,2)");
        builder.Property(m => m.Saldo).HasColumnName("saldo").HasColumnType("numeric(18,2)");
        builder.Property(m => m.CuentaId).HasColumnName("cuenta_id").IsRequired();
    }
}
