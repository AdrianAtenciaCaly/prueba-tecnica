using ClientesService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientesService.Infrastructure.Persistence;

/// <summary>DbContext de EF Core para la base de datos clientes_db (PostgreSQL).</summary>
public class ClientesDbContext : DbContext
{
    public ClientesDbContext(DbContextOptions<ClientesDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
