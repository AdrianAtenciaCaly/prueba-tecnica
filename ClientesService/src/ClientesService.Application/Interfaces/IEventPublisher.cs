namespace ClientesService.Application.Interfaces;
public interface IEventPublisher
{
    Task PublicarAsync<TEvent>(TEvent evento, CancellationToken cancellationToken = default) where TEvent : class;
}
