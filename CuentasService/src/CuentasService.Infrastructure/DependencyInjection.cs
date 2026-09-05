using CuentasService.Application.Interfaces;
using CuentasService.Application.Services;
using CuentasService.Domain.Interfaces;
using CuentasService.Infrastructure.Messaging;
using CuentasService.Infrastructure.Persistence;
using CuentasService.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CuentasService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CuentasDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CuentasDb")));

        services.AddScoped<ICuentaRepository, CuentaRepository>();
        services.AddScoped<IMovimientoRepository, MovimientoRepository>();
        services.AddScoped<IClienteReferenciaRepository, ClienteReferenciaRepository>();
        services.AddScoped<IClienteValidator, ClienteValidator>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ClienteCreadoConsumer>();
            x.AddConsumer<ClienteActualizadoConsumer>();
            x.AddConsumer<ClienteEliminadoConsumer>();

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
