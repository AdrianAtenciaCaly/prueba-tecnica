using ClientesService.Application.Interfaces;
using ClientesService.Domain.Interfaces;
using ClientesService.Infrastructure.Messaging;
using ClientesService.Infrastructure.Persistence;
using ClientesService.Infrastructure.Security;
using ClientesService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientesService.Infrastructure;

/// <summary>
/// Punto único de registro de dependencias de Infrastructure (Composition Root parcial).
/// Mantiene Program.cs limpio y hace explícito qué implementaciones concretas se usan para cada abstracción.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ClientesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ClientesDb")));

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"] ?? "localhost", "/", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
