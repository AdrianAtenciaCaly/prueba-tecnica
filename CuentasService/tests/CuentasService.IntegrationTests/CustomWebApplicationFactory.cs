using CuentasService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CuentasService.IntegrationTests;

/// <summary>
/// F6: igual que en ClientesService, se reemplaza PostgreSQL por EF Core InMemory para pruebas rápidas
/// y aisladas. MassTransit se configura contra un bus en memoria (transporte de prueba de MassTransit,
/// sin RabbitMQ real) para que el arranque de la API no dependa de infraestructura externa en CI.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public string DbName { get; } = $"CuentasTestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("RabbitMq:Host", "localhost");

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<CuentasDbContext>));
            if (dbContextDescriptor is not null) services.Remove(dbContextDescriptor);

            services.AddDbContext<CuentasDbContext>(options => options.UseInMemoryDatabase(DbName));
        });
    }

    /// <summary>Helper para sembrar un cliente de referencia sin depender de RabbitMQ en las pruebas.</summary>
    public async Task SembrarClienteReferenciaAsync(string clienteId, string nombre, bool estado = true)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CuentasDbContext>();
        context.ClientesReferencia.Add(new CuentasService.Domain.Entities.ClienteReferencia(clienteId, nombre, estado));
        await context.SaveChangesAsync();
    }
}
