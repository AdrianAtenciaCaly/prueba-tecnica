using CuentasService.Domain.Entities;

namespace CuentasService.Domain.Interfaces;

/// <summary>Repositorio del read-model de clientes alimentado por los eventos de integración.</summary>
public interface IClienteReferenciaRepository
{
    Task<ClienteReferencia?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);
    Task UpsertAsync(string clienteId, string nombre, bool estado, CancellationToken cancellationToken = default);
    Task MarcarEliminadoAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default);
}
