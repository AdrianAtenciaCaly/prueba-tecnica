using CuentasService.Domain.Entities;

namespace CuentasService.Domain.Interfaces;

public interface IClienteReferenciaRepository
{
    Task<ClienteReferencia?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);
    Task UpsertAsync(string clienteId, string nombre, bool estado, CancellationToken cancellationToken = default);
    Task MarcarEliminadoAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default);
}
