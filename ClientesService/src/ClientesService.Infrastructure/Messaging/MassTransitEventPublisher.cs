using ClientesService.Application.Interfaces;
using MassTransit;

namespace ClientesService.Infrastructure.Messaging;

/// <summary>
/// Adaptador concreto de IEventPublisher usando MassTransit sobre RabbitMQ.
/// Es la pieza que materializa la "comunicación asíncrona entre los 2 microservicios" pedida en el ejercicio:
/// ClientesService publica eventos de integración a un exchange de RabbitMQ, y CuentasService los consume
/// de forma independiente y desacoplada (sin llamadas síncronas HTTP entre servicios).
/// </summary>
public class MassTransitEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublicarAsync<TEvent>(TEvent evento, CancellationToken cancellationToken = default) where TEvent : class
    {
        await _publishEndpoint.Publish(evento, cancellationToken);
    }
}
