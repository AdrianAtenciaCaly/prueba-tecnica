using CuentasService.Application.Interfaces;
using CuentasService.Domain.Exceptions;
using CuentasService.Domain.Interfaces;

namespace CuentasService.Application.Services;

public class ClienteValidator : IClienteValidator
{
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;

    public ClienteValidator(IClienteReferenciaRepository clienteReferenciaRepository)
    {
        _clienteReferenciaRepository = clienteReferenciaRepository;
    }

    public async Task ValidarClienteActivoAsync(string clienteId, CancellationToken cancellationToken = default)
    {
        var referencia = await _clienteReferenciaRepository.ObtenerPorClienteIdAsync(clienteId, cancellationToken);

        if (referencia is null || !referencia.Estado)
            throw new ClienteInvalidoException(clienteId);
    }
}
