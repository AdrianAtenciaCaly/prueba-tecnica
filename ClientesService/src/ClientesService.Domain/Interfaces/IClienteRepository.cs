using ClientesService.Domain.Entities;

namespace ClientesService.Domain.Interfaces;

/// <summary>
/// Puerto (en términos de arquitectura hexagonal) que define cómo la capa de Aplicación accede a la persistencia
/// de Clientes, sin conocer detalles de EF Core / PostgreSQL (Dependency Inversion Principle).
/// La implementación concreta vive en Infrastructure.
/// </summary>
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
