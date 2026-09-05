using CuentasService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CuentasService.Infrastructure.Persistence.Configurations;

public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
{
    public void Configure(EntityTypeBuilder<Cuenta> builder)
    {
        builder.ToTable("cuentas");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.NumeroCuenta).HasColumnName("numero_cuenta").HasMaxLength(20).IsRequired();
        builder.Property(c => c.TipoCuenta).HasColumnName("tipo_cuenta").HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(c => c.SaldoInicial).HasColumnName("saldo_inicial").HasColumnType("numeric(18,2)");
        builder.Property(c => c.SaldoActual).HasColumnName("saldo_actual").HasColumnType("numeric(18,2)");
        builder.Property(c => c.Estado).HasColumnName("estado").IsRequired();
        builder.Property(c => c.ClienteId).HasColumnName("cliente_id").HasMaxLength(50).IsRequired();

        builder.HasIndex(c => c.NumeroCuenta).IsUnique();
        builder.HasIndex(c => c.ClienteId);

        builder.HasMany(c => c.Movimientos)
            .WithOne()
            .HasForeignKey(m => m.CuentaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata.FindNavigation(nameof(Cuenta.Movimientos))!
            .SetPropertyAccessMode(Microsoft.EntityFrameworkCore.PropertyAccessMode.Field);
    }
}
