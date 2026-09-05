using ClientesService.Domain.Entities;

namespace ClientesService.Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Cliente?> ObtenerPorClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<Cliente?> ObtenerPorIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    void Actualizar(Cliente cliente);
    void Eliminar(Cliente cliente);
    Task<bool> ExisteClienteIdAsync(string clienteId, CancellationToken cancellationToken = default);
    Task<bool> ExisteIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);
    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default);
}
