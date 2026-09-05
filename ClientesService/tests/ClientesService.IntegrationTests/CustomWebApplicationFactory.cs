using ClientesService.Application.Interfaces;
using ClientesService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ClientesService.IntegrationTests;

/// <summary>
/// F6: Fábrica de pruebas de integración. Levanta la API completa (Program.cs real, pipeline HTTP real,
/// DI real) mientras reemplaza únicamente los bordes externos que no tiene sentido probar en CI:
/// - PostgreSQL -> EF Core InMemory (rápido, aislado por prueba)
/// - RabbitMQ (IEventPublisher) -> un mock, ya que no hay broker disponible en el entorno de pruebas
/// Todo lo demás (controladores, middleware, validación, capa de aplicación, repositorio, mapeo EF) es real.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IEventPublisher> EventPublisherMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ClientesDbContext>));
            if (dbContextDescriptor is not null) services.Remove(dbContextDescriptor);

            services.AddDbContext<ClientesDbContext>(options =>
                options.UseInMemoryDatabase($"ClientesTestDb_{Guid.NewGuid()}"));

            var publisherDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEventPublisher));
            if (publisherDescriptor is not null) services.Remove(publisherDescriptor);
            services.AddScoped(_ => EventPublisherMock.Object);
        });
    }
}
