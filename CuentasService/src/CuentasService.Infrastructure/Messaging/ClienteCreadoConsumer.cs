using CuentasService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace CuentasService.Infrastructure.Messaging;

/// <summary>
/// Consumidor asíncrono del evento publicado por ClientesService cuando se crea un cliente.
/// Esta es la mitad "receptora" de la comunicación asíncrona entre los dos microservicios:
/// CuentasService nunca llama por HTTP a ClientesService; en su lugar, mantiene su propio read-model
/// (ClienteReferencia) actualizado reactivamente a partir de estos eventos (Event-Carried State Transfer).
/// </summary>
public class ClienteCreadoConsumer : IConsumer<ClienteCreadoIntegrationEvent>
{
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;
    private readonly ILogger<ClienteCreadoConsumer> _logger;

    public ClienteCreadoConsumer(IClienteReferenciaRepository clienteReferenciaRepository, ILogger<ClienteCreadoConsumer> logger)
    {
        _clienteReferenciaRepository = clienteReferenciaRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClienteCreadoIntegrationEvent> context)
    {
        var evento = context.Message;
        _logger.LogInformation("Evento ClienteCreado recibido para ClienteId {ClienteId}", evento.ClienteId);

        await _clienteReferenciaRepository.UpsertAsync(evento.ClienteId, evento.Nombre, evento.Estado, context.CancellationToken);
        await _clienteReferenciaRepository.GuardarCambiosAsync(context.CancellationToken);
    }
}
