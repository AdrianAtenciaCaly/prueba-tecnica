using CuentasService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace CuentasService.Infrastructure.Messaging;

public class ClienteActualizadoConsumer : IConsumer<ClienteActualizadoIntegrationEvent>
{
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;
    private readonly ILogger<ClienteActualizadoConsumer> _logger;

    public ClienteActualizadoConsumer(IClienteReferenciaRepository clienteReferenciaRepository, ILogger<ClienteActualizadoConsumer> logger)
    {
        _clienteReferenciaRepository = clienteReferenciaRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ClienteActualizadoIntegrationEvent> context)
    {
        var evento = context.Message;
        _logger.LogInformation("Evento ClienteActualizado recibido para ClienteId {ClienteId}", evento.ClienteId);

        await _clienteReferenciaRepository.UpsertAsync(evento.ClienteId, evento.Nombre, evento.Estado, context.CancellationToken);
        await _clienteReferenciaRepository.GuardarCambiosAsync(context.CancellationToken);
    }
}
