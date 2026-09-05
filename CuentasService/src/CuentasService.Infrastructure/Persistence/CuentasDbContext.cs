using CuentasService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CuentasService.Infrastructure.Persistence;

/// <summary>DbContext de EF Core para la base de datos cuentas_db (PostgreSQL).</summary>
public class CuentasDbContext : DbContext
{
    public CuentasDbContext(DbContextOptions<CuentasDbContext> options) : base(options) { }

    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();
    public DbSet<ClienteReferencia> ClientesReferencia => Set<ClienteReferencia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CuentasDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
