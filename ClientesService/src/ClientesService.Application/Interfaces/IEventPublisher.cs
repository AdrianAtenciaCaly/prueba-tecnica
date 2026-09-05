namespace ClientesService.Application.Interfaces;

/// <summary>
/// Abstracción sobre el bus de mensajería (Dependency Inversion Principle).
/// La capa de Aplicación solo sabe que "publica eventos"; MassTransit/RabbitMQ es un detalle de Infrastructure.
/// Esto es lo que habilita la comunicación asíncrona con CuentasService sin acoplar ambos microservicios.
/// </summary>
public interface IEventPublisher
{
    Task PublicarAsync<TEvent>(TEvent evento, CancellationToken cancellationToken = default) where TEvent : class;
}
