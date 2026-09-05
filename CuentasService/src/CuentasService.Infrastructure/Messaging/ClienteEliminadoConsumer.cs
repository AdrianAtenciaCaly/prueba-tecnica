using CuentasService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace CuentasService.Infrastructure.Messaging;

public class ClienteEliminadoConsumer : IConsumer<ClienteEliminadoIntegrationEvent>
{
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;
    private readonly ILogger<ClienteEliminadoConsumer> _logger;

    public ClienteEliminadoConsumer(IClienteReferenciaRepository clienteReferenciaRepository, ILogger<ClienteEliminadoConsumer> logger)
    {
        _clienteReferenciaRepository = clienteReferenciaRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClienteEliminadoIntegrationEvent> context)
    {
        var evento = context.Message;
        _logger.LogInformation("Evento ClienteEliminado recibido para ClienteId {ClienteId}", evento.ClienteId);

        await _clienteReferenciaRepository.MarcarEliminadoAsync(evento.ClienteId, context.CancellationToken);
        await _clienteReferenciaRepository.GuardarCambiosAsync(context.CancellationToken);
    }
}
