using ClientesService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesService.Infrastructure.Persistence.Configurations;

/// <summary>
/// Mapeo de la entidad Cliente (y de su base Persona, mapeada como TPH sobre la misma tabla ya que
/// en este ejercicio Persona no tiene otras subclases). Se usan los setters privados de la entidad
/// vía reflexión (comportamiento estándar de EF Core), preservando el encapsulamiento del dominio.
/// </summary>
public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        // Campos heredados de Persona
        builder.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
        builder.Property(c => c.Genero).HasColumnName("genero").HasMaxLength(20).IsRequired();
        builder.Property(c => c.Edad).HasColumnName("edad").IsRequired();
        builder.Property(c => c.Identificacion).HasColumnName("identificacion").HasMaxLength(30).IsRequired();
        builder.Property(c => c.Direccion).HasColumnName("direccion").HasMaxLength(250).IsRequired();
        builder.Property(c => c.Telefono).HasColumnName("telefono").HasMaxLength(20).IsRequired();

        // Campos propios de Cliente
        builder.Property(c => c.ClienteId).HasColumnName("cliente_id").HasMaxLength(50).IsRequired();
        builder.Property(c => c.ContrasenaHash).HasColumnName("contrasena_hash").IsRequired();
        builder.Property(c => c.Estado).HasColumnName("estado").IsRequired();

        builder.HasIndex(c => c.ClienteId).IsUnique();
        builder.HasIndex(c => c.Identificacion).IsUnique();
    }
}
